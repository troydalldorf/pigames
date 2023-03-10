// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using System.Device.Gpio;

namespace CheckSeeSaw
{
    public partial class MySeesaw
    {
        public ulong TestDigitalReadBulk(short delay = 8)
        {
            if (!HasModule(MySeesawModule.Gpio))
            {
                throw new InvalidOperationException($"The hardware on I2C Bus {this.I2CDevice.ConnectionSettings.BusId}, Address 0x{this.I2CDevice.ConnectionSettings.DeviceAddress:X2} does not support Adafruit SeeSaw GPIO functionality");
            }
            
            var buf = Read(MySeesawModule.Gpio, MySeesawFunction.GpioBulk, 8, delay);
            return (uint)BitConverter.ToInt32(buf, 0);
        }
        
        public void TestPinModeBulk(int capacity, int offset, uint pins, PinMode mode)
        {
            byte[] cmd = new byte[capacity];
            BitConverter.GetBytes(pins).CopyTo(cmd, offset);

            switch (mode)
            {
                case PinMode.Output:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirsetBulk, cmd);
                    break;
                case PinMode.Input:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, cmd);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioPullenclr, cmd);
                    break;
                case PinMode.InputPullUp:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, cmd);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioPullenset, cmd);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioBulkSet, cmd);
                    break;
                case PinMode.InputPullDown:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, cmd);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioPullenset, cmd);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioBulkClr, cmd);
                    break;
                default:
                    throw new ArgumentException("Invalid pin mode");
            }
        }
    }
}