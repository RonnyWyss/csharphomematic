﻿using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.XMLAPI.Generic
{
    public class Device
    {
        public Channel[] Channels { get; private set; }
        [JsonIgnore]

        public Dictionary<string, Channel> ChannelByISEID { get; private set; }
        public Dictionary<string, Datapoint> DatapointByType { get; private set; }

        public string Name { get; private set; }

        public string ISEID { get; private set; }

        public bool Reachable { get; private set; }

        public bool PendingConfig { get; private set; }

        public string Address { get; private set; }
                
        public string Interface { get; private set; }        

        public string DeviceType { get; private set; }

        public bool ReadyConfig { get; private set; }            

        HashSet<string> Rooms { get { return new HashSet<string>(Channels.SelectMany(c => c.Rooms)); } }

        HashSet<string> Functions { get { return new HashSet<string>(Channels.SelectMany(c => c.Functions)); } }

        ILog LOG = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Device(CGI.DeviceList.Device d, CGI.CGIClient CGIClient)
        {
            FillFromDeviceListDevice(d);
            FillFromRoomList(CGIClient.RoomList);
            FillFromFunctionList(CGIClient.FunctionList);
            FillFromStateList(CGIClient.StateList);

            InitDatapointByType();
        }

        private void InitDatapointByType()
        {
            //there can be multiple datapoints with the same type
            //if that is the case, only the datapoint with channel of direction RECEIVER is the one we should selected   
   
           
            var dict = new Dictionary<string, List<Datapoint>>();

            foreach (var c in Channels)
            {
                foreach(var dp in c.Datapoints)
                {
                    if (dict.ContainsKey(dp.Key))
                        dict[dp.Key].Add(dp.Value);
                    else
                    {
                        var list = new List<Datapoint>();
                        list.Add(dp.Value);
                        dict.Add(dp.Key, list);
                    }
                }
            }

            DatapointByType = new Dictionary<string, Datapoint>();

            foreach (var kvp in dict)
            {
                if (kvp.Value.Count == 1)
                    DatapointByType.Add(kvp.Key, kvp.Value[0]);
                else
                {
                    var first = kvp.Value.Where(w => w.Channel.Direction == "RECEIVER").FirstOrDefault();
                    if (first == null)
                    {
                        //nothing interesting. ignoring.
                    }
                    else
                    {
                        DatapointByType.Add(first.Type, first);
                    }
                }              
            }
        }

        public Datapoint GetDatapointByType(string type)
        {
            Datapoint dp = null;

            foreach(Channel c in Channels)
            {
                if (c.Datapoints.TryGetValue(type, out dp))
                    return dp;
            }

            return null;            
        }

        private void FillFromDeviceListDevice(CGI.DeviceList.Device d)
        {
            Name = d.Name;
            ISEID = d.Ise_id;
            Reachable = false; //todo from state list
            PendingConfig = true; //todo from state list
            Address = d.Address;
            Interface = d.Interface;
            DeviceType = d.Device_type;
            ReadyConfig = String.IsNullOrWhiteSpace(d.Ready_config) ?  false : Convert.ToBoolean(d.Ready_config);

            ChannelByISEID = new Dictionary<string, Channel>();

            Channels = new Channel[d.Channel.Count];
            for (int i = 0; i < Channels.Length; i++)
            {
                Channels[i] = new Channel(d.Channel[i], this);
                if(!ChannelByISEID.ContainsKey(Channels[i].ISEID))
                    ChannelByISEID.Add(Channels[i].ISEID, Channels[i]);
                else
                    throw new Exception(String.Format("Duplicated channel iseid {0} on device {1}.", Channels[i].ISEID, d.Name));                   
            }  
        }

        private void FillFromRoomList(CGI.RoomList.RoomList roomList)
        {
            foreach(var room in roomList.Room)
            {
                foreach(var channel in room.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if (!c.Rooms.Contains(room.Name))
                        {
                            c.Rooms.Add(room.Name);
                        }
                    }                   
                }
            }
        }

        private void FillFromFunctionList(CGI.FunctionList.FunctionList funcList)
        {
            foreach(var function in funcList.Function)
            {
                foreach (var channel in function.Channel)
                {
                    Channel c = null;
                    if (ChannelByISEID.TryGetValue(channel.Ise_id, out c))
                    {
                        if(!c.Functions.Contains(function.Name))
                        {
                            c.Functions.Add(function.Name);
                        }
                    }
                }
            }
        }

        private void FillFromStateList(CGI.StateList.Device d)
        {
            PendingConfig = String.IsNullOrWhiteSpace(d.Config_pending) ? false : Convert.ToBoolean(d.Config_pending);
            Reachable = String.IsNullOrWhiteSpace(d.Unreach) ? true : !Convert.ToBoolean(d.Unreach);

            foreach (var c in d.Channel)
            {
                var dc = ChannelByISEID[c.Ise_id];
                dc.UpdateDataPoints(c.Datapoint);
            }
        }

        private void FillFromStateList(CGI.StateList.StateList stateList)
        {
            foreach(var d in stateList.Device)
                if(d.Ise_id == ISEID)
                    FillFromStateList(d);    
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }  
}
