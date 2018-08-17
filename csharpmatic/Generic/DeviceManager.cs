﻿using csharpmatic.Interfaces;
using csharpmatic.XMLAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Generic
{
    public class DeviceManager
    {
        private XMLAPI.Client CGIClient;
        public List<Device> Devices { get; private set; }
        public Dictionary<string, Device> DevicesByISEID { get; private set ;}
        private Dictionary<string, Datapoint> PrevDataPointsByISEID { get; set; }
        public List<DatapointEvent> Events { get; private set; }
        public Uri HttpServerUri { get { return CGIClient.HttpServerUri; } }        
        public List<Room> Rooms { get; private set; }
        public Dictionary<string, Room> RoomsByName { get; private set; }
                
        public DeviceManager(string serverAddress)
        {
            CGIClient = new Client("http://" + serverAddress);
            Devices = new List<Device>();
            Refresh();
        }

        public List<T> GetDevicesImplementingInterface<T>() where T : class
        {
            List<T> list = new List<T>();

            return Devices.Where(w => w is T).Select(s => s as T).ToList();
        }

        public List<DatapointEvent> Refresh()
        {
            BuildPrevDataPointsByISEID();

            bool fullRefresh = CGIClient.FetchData();

            if (fullRefresh)
            {
                BuildDeviceList();
                BuildRoomList();
            }
            else
            {
                foreach(var cgi_dev in CGIClient.StateList.Device)
                {
                    Device d = null;
                    if (DevicesByISEID.TryGetValue(cgi_dev.Ise_id, out d))
                        d.FillFromStateList(cgi_dev);                    
                }
            }

            return GetEvents();
        }

        private List<DatapointEvent> GetEvents()
        {
            List<DatapointEvent> list = new List<DatapointEvent>();
                       
            foreach (var d in Devices)
            {
                foreach (var c in d.Channels)
                {
                    foreach (var dpkvp in c.Datapoints)
                    {
                        Datapoint current = dpkvp.Value;

                        Datapoint prev = null;
                        PrevDataPointsByISEID.TryGetValue(current.ISEID, out prev);

                        //new device was just added
                        if (prev == null)
                            list.Add(new DatapointEvent(current, null));
                        else if (current.GetValueString() != prev.GetValueString()|| current.OperationsCounter != prev.OperationsCounter)
                            list.Add(new DatapointEvent(current, prev));                        
                    }
                }
            }

            Events = list;

            return list;
        }             

        private void BuildRoomList()
        {
            Rooms = new List<Room>();
            RoomsByName = new Dictionary<string, Room>();

            foreach (var cgiroom in CGIClient.RoomList.Room)
            {
                Room r = new Room(cgiroom.Name, cgiroom.Ise_id, this);
                Rooms.Add(r);
                RoomsByName.Add(r.Name, r);
            }
        }

        private void BuildPrevDataPointsByISEID()
        {
            if (Devices == null)
                PrevDataPointsByISEID = new Dictionary<string, Datapoint>();
            else
                PrevDataPointsByISEID = Devices.SelectMany(d => d.Channels).SelectMany(c => c.Datapoints).Select(dp => dp.Value.Clone()).ToDictionary(ks => ks.ISEID);
        }

        private void BuildDeviceList()
        {           
            Devices = new List<Device>();
            DevicesByISEID = new Dictionary<string, Device>();

            foreach (var d in CGIClient.DeviceList.Device)
            {                
                Device gd = DeviceFactory.CreateInstance(d, CGIClient, this);
                                
                Devices.Add(gd);
                DevicesByISEID.Add(gd.ISEID, gd);
            }                           
        }
    }
}