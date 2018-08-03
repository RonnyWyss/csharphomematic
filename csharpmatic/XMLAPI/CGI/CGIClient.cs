﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace csharpmatic.XMLAPI.CGI
{
    public class CGIClient
    {
        public Uri HttpServerUri { get; private set; }
        public DeviceList.DeviceList DeviceList { get; private set; }
        public FunctionList.FunctionList FunctionList { get; private set; }
        public RoomList.RoomList RoomList { get; private set; }
        public StateList.StateList StateList { get; private set;  }

        private static WebClient WebClient = new WebClient();

        private DateTime lastFullUpdateTimestamp = DateTime.MinValue;

        public TimeSpan FullRecheckInternval = new TimeSpan(0, 1, 0);
                
        private static T XMLGetRequest<T>(Uri getUrl)
        {
            using (Stream rdr = WebClient.OpenRead(getUrl))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                T dl = (T)serializer.Deserialize(rdr);
                return dl;
            }
        }

        private static T SafeXMLGetRequest<T>(Uri getUrl, int attempts=3, int retryWaitMs=1000)
        {
            Exception e = null;

            for(int i=0; i<attempts; ++i)
            {
                try
                {
                    T ret = XMLGetRequest<T>(getUrl);
                    return ret;
                }
                catch(Exception ex)
                {
                    e = ex;
                }

                Thread.Sleep(retryWaitMs);
            }

            throw new Exception("HTTP Get Request failed: " + getUrl, e);
        }

        public CGIClient(string httpServerUri)
        {
            HttpServerUri = new Uri(httpServerUri);
        }

        private void FetchDeviceList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/devicelist.cgi");
            DeviceList = SafeXMLGetRequest<DeviceList.DeviceList>(uri);
        }

        private void FetchRoomList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/roomlist.cgi");
            RoomList = SafeXMLGetRequest<RoomList.RoomList>(uri);
        }

        private void FetchStateList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/statelist.cgi");
            StateList = SafeXMLGetRequest<StateList.StateList>(uri);
        }

        private void FetchFunctionList()
        {
            Uri uri = new Uri(HttpServerUri, @"addons/xmlapi/functionlist.cgi");
            FunctionList = SafeXMLGetRequest<FunctionList.FunctionList>(uri);
        }

        public void FetchData(bool force=false)
        {             
            if(DateTime.Now - lastFullUpdateTimestamp > FullRecheckInternval || force)
            {
                FetchDeviceList();
                FetchFunctionList();
                FetchRoomList();
                lastFullUpdateTimestamp = DateTime.Now;
            }

            FetchStateList();            
        }       
    }
}
