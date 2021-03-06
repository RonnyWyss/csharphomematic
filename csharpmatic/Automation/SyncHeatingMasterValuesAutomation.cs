﻿using csharpmatic.Generic;
using csharpmatic.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharpmatic.Automation
{
    public class SyncHeatingMasterValuesAutomation
    {
        private static ILog LOGGER = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SyncHeatingMastervalues(ITempControlDevice leader, List<ITempControlDevice> devicesInScope, HashSet<string> masterValuesInScope)
        {
            if (leader == null || leader.PendingConfig || !leader.Reachable)
                return;

            decimal eps = 0.000001M;

            Dictionary<Channel, List<MasterValue>> toChange = new Dictionary<Channel, List<MasterValue>>();

            foreach (var lmv in leader.Channels[1].MasterValues.Values.Where(w => masterValuesInScope.Contains(w.Name)))
            {
                foreach (var d in devicesInScope)
                {
                    if (d == leader || d.Channels.Count() < 2 || d.PendingConfig || !d.Reachable)
                        continue;

                    MasterValue mv = null;

                    if (d.Channels[1].MasterValues.TryGetValue(lmv.Name, out mv))
                    {
                        if (Math.Abs(mv.Value - lmv.Value) > eps)
                        {
                            if (!toChange.ContainsKey(d.Channels[1]))
                                toChange.Add(d.Channels[1], new List<MasterValue>());

                            toChange[d.Channels[1]].Add(lmv);
                            LOGGER.InfoFormat($"Master value sync: found {d.Name} {mv.Name} = {mv.Value} where leader ({leader.Name}) has {lmv.Value}. Syncing with leader.");
                        }
                    }
                }
            }

            foreach (var kvp in toChange)
            {
                kvp.Key.SetMasterValues(kvp.Value);
            }
        }

        public static void SyncHeatingMastervalues(DeviceManager dm)
        {
            var allDevices = dm.GetDevicesImplementingInterface<ITempControlDevice>();
            var houseLeader = allDevices.Where(w => w.ISEID == allDevices.Min(min => min.ISEID)).FirstOrDefault();
            var houseMasterValues = new HashSet<string>(houseLeader.Channels[1].MasterValues.Values.Where(w => !w.Name.Contains("TEMPERATURE")).Select(s => s.Name));
            var roomMasterValues = new HashSet<string>(houseLeader.Channels[1].MasterValues.Values.Where(w => w.Name.Contains("TEMPERATURE")).Select(s => s.Name));

            LOGGER.Debug("Syncing house mastervalues...");
            SyncHeatingMastervalues(houseLeader, allDevices, houseMasterValues);

            var allRooms = allDevices.SelectMany(d => d.Rooms).Distinct().ToList();

            foreach (var r in allRooms)
            {
                var roomDevices = allDevices.Where(w => w.Rooms.Contains(r)).ToList();
                var roomLeader = allDevices.Where(w => w.ISEID == roomDevices.Min(min => min.ISEID)).FirstOrDefault();

                LOGGER.Debug($"Syncing {r} mastervalues...");
                SyncHeatingMastervalues(roomLeader, roomDevices, roomMasterValues);
            }

            LOGGER.Debug("Mastervalues across devices are in sync!");
        }
    }
}
