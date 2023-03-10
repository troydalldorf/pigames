// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using System.Device.Gpio;

namespace CheckSeeSaw
{
    public partial class MySeesaw : IDisposable
    {
        public ulong TestDigitalReadBulkshort(short delay = 8)
        {
            var buf = Read(MySeesawModule.Gpio, MySeesawFunction.GpioBulk, 8, delay);
            return (uint)BitConverter.ToInt32(buf.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Set the PinMode for a GPIO Pin.
        /// </summary>
        /// <param name="pin">The pin that has its mode set.</param>
        /// <param name="mode">The pin mode to be set.</param>
        public void SetGpioPinMode(byte pin, PinMode mode)
        {
            if (pin > 63)
            {
                throw new ArgumentOutOfRangeException(nameof(pin), "Gpio pin must be within 0-63 range.");
            }

            SetGpioPinModeBulk((ulong)(1 << pin), mode);
        }

        /// <summary>
        /// Set the PinMode for a number of GPIO pins
        /// </summary>
        /// <param name="pins">A 64bit integer containing 1 bit for each pin. If a bit is set to 1 then the pin mode is set for the associated pin.</param>
        /// <param name="mode">The pin mode to be set.</param>
        public void SetGpioPinModeBulk(ulong pins, PinMode mode)
        {
            byte[] pinArray;

            if (!HasModule(MySeesawModule.Gpio))
            {
                throw new InvalidOperationException(
                    $"The hardware on I2C Bus {I2CDevice.ConnectionSettings.BusId}, Address 0x{I2CDevice.ConnectionSettings.DeviceAddress:X2} does not support Adafruit SeeSaw GPIO functionality");
            }

            pinArray = Attiny8X7PinsToPinArray(pins);

            switch (mode)
            {
                case PinMode.Output:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirsetBulk, pinArray);
                    break;

                case PinMode.Input:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, pinArray);
                    break;

                case PinMode.InputPullUp:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, pinArray);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioPullenset, pinArray);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioBulkSet, pinArray);
                    break;

                case PinMode.InputPullDown:
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioDirclrBulk, pinArray);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioPullenset, pinArray);
                    Write(MySeesawModule.Gpio, MySeesawFunction.GpioBulkClr, pinArray);
                    break;
            }
        }

        public uint ReadGpioInterruptFlags()
        {
            if (!HasModule(MySeesawModule.Gpio))
            {
                throw new InvalidOperationException(
                    $"The hardware on I2C Bus {I2CDevice.ConnectionSettings.BusId}, Address 0x{I2CDevice.ConnectionSettings.DeviceAddress:X2} does not support Adafruit SeeSaw GPIO functionality");
            }

            return BinaryPrimitives.ReadUInt32BigEndian(Read(MySeesawModule.Gpio, MySeesawFunction.GpioIntflag, 4));
        }

        //private static ulong Attiny8X7PinArrayToPins(byte[] pinArray) => ((ulong)pinArray[2] << 40) | ((ulong)pinArray[3] << 32) | ((ulong)pinArray[4] << 24) | ((ulong)pinArray[5] << 16) |
        //                                                                 ((ulong)pinArray[6] << 8) | pinArray[7];

        private static byte[] Attiny8X7PinsToPinArray(ulong pins) => new byte[] { (byte)(pins >> 40), (byte)(pins >> 32), (byte)(pins >> 24), (byte)(pins >> 16), (byte)(pins >> 8), (byte)pins, 0, 0 };
    }
}