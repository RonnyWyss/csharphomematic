/* This file was generated using InterfaceGenerator, any modifications made to this file will be lost */

using csharpmatic.Generic;
using csharpmatic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace csharpmatic.Interfaces.Devices
{
  public partial class HMIP_PCBS : Device, ISingleSwitchControlDevice, IHmIPDevice
  {
		public TypedDatapoint<System.Decimal> Actuator_Actual_Temperature { get; private set; }

		public TypedDatapoint<System.Int32> Actuator_Actual_Temperature_Status { get; private set; }

		public TypedDatapoint<System.Boolean> Config_Pending { get; private set; }

		public TypedDatapoint<System.Boolean> Duty_Cycle { get; private set; }

		public TypedDatapoint<String> Error_Code { get; private set; }

		public TypedDatapoint<Boolean> Error_Overheat { get; private set; }

		public TypedDatapoint<System.Decimal> Operating_Voltage { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.IHmIP_Operating_Voltage_Status_Enum> Operating_Voltage_Status { get; private set; }

		public TypedDatapoint<System.String> Rssi_Device { get; private set; }

		public TypedDatapoint<System.String> Rssi_Peer { get; private set; }

		public TypedDatapoint<System.Boolean> Unreach { get; private set; }

		public TypedDatapoint<System.Boolean> Update_Pending { get; private set; }

		public TypedDatapoint<Boolean> Press_Long { get; private set; }

		public TypedDatapoint<Boolean> Press_Short { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ISingleSwitchControlDevice_Process> Process { get; private set; }

		public TypedDatapoint<System.Int32> Section { get; private set; }

		public TypedDatapoint<csharpmatic.Interfaces.ISingleSwitchControlDevice_Section_Status> Section_Status { get; private set; }

		public TypedDatapoint<System.Boolean> State { get; private set; }


      public HMIP_PCBS(XMLAPI.DeviceList.Device d, XMLAPI.Client CGIClient, DeviceManager dm) : base(d, CGIClient, dm)
      {
			Actuator_Actual_Temperature = new TypedDatapoint<System.Decimal>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE"]);

			Actuator_Actual_Temperature_Status = new TypedDatapoint<System.Int32>(base.Channels[0].Datapoints["ACTUATOR_ACTUAL_TEMPERATURE_STATUS"]);

			Config_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["CONFIG_PENDING"]);

			Duty_Cycle = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["DUTY_CYCLE"]);

			Error_Code = new TypedDatapoint<String>(base.Channels[0].Datapoints["ERROR_CODE"]);

			Error_Overheat = new TypedDatapoint<Boolean>(base.Channels[0].Datapoints["ERROR_OVERHEAT"]);

			Operating_Voltage = new TypedDatapoint<System.Decimal>(base.Channels[0].Datapoints["OPERATING_VOLTAGE"]);

			Operating_Voltage_Status = new TypedDatapoint<csharpmatic.Interfaces.IHmIP_Operating_Voltage_Status_Enum>(base.Channels[0].Datapoints["OPERATING_VOLTAGE_STATUS"]);

			Rssi_Device = new TypedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_DEVICE"]);

			Rssi_Peer = new TypedDatapoint<System.String>(base.Channels[0].Datapoints["RSSI_PEER"]);

			Unreach = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UNREACH"]);

			Update_Pending = new TypedDatapoint<System.Boolean>(base.Channels[0].Datapoints["UPDATE_PENDING"]);

			Press_Long = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_LONG"]);

			Press_Short = new TypedDatapoint<Boolean>(base.Channels[1].Datapoints["PRESS_SHORT"]);

			Process = new TypedDatapoint<csharpmatic.Interfaces.ISingleSwitchControlDevice_Process>(base.Channels[3].Datapoints["PROCESS"]);

			Section = new TypedDatapoint<System.Int32>(base.Channels[3].Datapoints["SECTION"]);

			Section_Status = new TypedDatapoint<csharpmatic.Interfaces.ISingleSwitchControlDevice_Section_Status>(base.Channels[3].Datapoints["SECTION_STATUS"]);

			State = new TypedDatapoint<System.Boolean>(base.Channels[3].Datapoints["STATE"]);

      }
  }
}
