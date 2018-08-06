/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.XMLAPI.Generic;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.XMLAPI.Interfaces.Devices
{
  public partial class HMIP_PCBS : Device, ISingleSwitchControlDevice, IHmIPDevice
  {
		public ManagedDatapoint<Decimal> Actuator_Actual_Temperature { get; private set; }

		public ManagedDatapoint<Int32> Actuator_Actual_Temperature_Status { get; private set; }

		public ManagedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public ManagedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public ManagedDatapoint<String> Error_Code { get; private set; }

		public ManagedDatapoint<Boolean> Error_Overheat { get; private set; }

		public ManagedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Device { get; private set; }

		public ManagedDatapoint<System.String> Rssi_Peer { get; private set; }

		public ManagedDatapoint<System.Boolean> Unreach { get; private set; }

		public ManagedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public ManagedDatapoint<Boolean> Press_Long { get; private set; }

		public ManagedDatapoint<Boolean> Press_Short { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ISingleSwitchControlDevice_Process> Process { get; private set; }

		public ManagedDatapoint<System.Int32> Section { get; private set; }

		public ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ISingleSwitchControlDevice_Section_Status> Section_Status { get; private set; }

		public ManagedDatapoint<System.Boolean> State { get; private set; }


      public HMIP_PCBS(CGI.DeviceList.Device d, CGI.CGIClient CGIClient) : base(d, CGIClient)
      {
			Actuator_Actual_Temperature = new ManagedDatapoint<Decimal>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE"]);

			Actuator_Actual_Temperature_Status = new ManagedDatapoint<Int32>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE_STATUS"]);

			Config_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code = new ManagedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Error_Overheat = new ManagedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_OVERHEAT"]);

			Operating_Voltage = new ManagedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.IHmIP_Operating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new ManagedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new ManagedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Press_Long = new ManagedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_LONG"]);

			Press_Short = new ManagedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_SHORT"]);

			Process = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ISingleSwitchControlDevice_Process>(base.Channels[3].Datapoints["PROCESS"]);

			Section = new ManagedDatapoint<System.Int32>(base.Channels[3].Datapoints["SECTION"]);

			Section_Status = new ManagedDatapoint<csharpmatic.XMLAPI.Interfaces.ISingleSwitchControlDevice_Section_Status>(base.Channels[3].Datapoints["SECTION_STATUS"]);

			State = new ManagedDatapoint<System.Boolean>(base.Channels[3].Datapoints["STATE"]);

      }
  }
}
