// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers.Binary;
using System.Device.I2c;

namespace CheckSeeSaw
{
    /// <summary>
    /// Represents Seesaw device
    /// </summary>
    public partial class MySeesaw : IDisposable
    {
        private const byte ATTINY8X7_HW_ID_CODE = 0x87;

        /// <summary>
        /// I2C device used for communication
        /// </summary>
        /// <value></value>
        public I2cDevice I2CDevice { get; private set; }

        public short DefaultReadDelayMicroseconds { get; }

        private uint options;

        /// <summary>
        /// Initializes new instance of Seesaw.
        /// </summary>
        /// <param name="i2CDevice">The I2cDevice to be used to communicate with the SeeSaw module. Note that the I2cDevice
        /// will be disposed when the along with the SeeSaw device</param>
        /// <param name="defaultReadDelayMicroseconds"></param>
        public MySeesaw(I2cDevice i2CDevice, short defaultReadDelayMicroseconds = 0)
        {
            I2CDevice = i2CDevice ?? throw new ArgumentNullException(nameof(i2CDevice));
            DefaultReadDelayMicroseconds = defaultReadDelayMicroseconds;
            Initialize(i2CDevice);
        }
        
        /// <summary>
        /// Version of the SeeSaw module.
        /// </summary>
        public uint Version { get; private set; }

        /// <summary>
        /// Reads the temperature of the SeeSaw device.
        /// </summary>
        /// <returns>A float that represents the temperature in degrees celcius.</returns>
        public float GetTemperature() => (1.0F / (1UL << 16)) * BinaryPrimitives.ReadUInt32BigEndian(Read(MySeesawModule.Status, MySeesawFunction.StatusTemp, 4, 1000));

        /// <summary>
        /// Tests to see if a module has been compiled into the SeeSaw firmware.
        /// </summary>
        /// <param name="moduleAddress">>An Seesaw_Module enum that represents the mdule to test for.</param>
        /// <returns>Returns true if the functionality associated with the module is available.</returns>
        public bool HasModule(MySeesawModule moduleAddress) => (options & (1 << (byte)moduleAddress)) != 0;

        /// <summary>
        /// Performs a soft reset of the SeeSaw module.
        /// </summary>
        public void SoftwareReset() => WriteByte(MySeesawModule.Status, MySeesawFunction.StatusSwrst, 0xFF);

        /// <summary>
        /// Initializes the Seesaw device.
        /// <param name="i2CDevice">The I2cDevice to initialize the Seesaw device with.</param>
        /// </summary>
        private void Initialize(I2cDevice i2CDevice)
        {
            SoftwareReset();
            MyDelayHelper.DelayMilliseconds(10, true);

            var hwid = ReadByte(MySeesawModule.Status, MySeesawFunction.StatusHwId);
            if (hwid != ATTINY8X7_HW_ID_CODE)
            {
                throw new NotSupportedException(
                    $"The hardware on I2C Bus {this.I2CDevice.ConnectionSettings.BusId}, Address 0x{this.I2CDevice.ConnectionSettings.DeviceAddress:X2} does not appear to be an Adafruit SeeSaw module.\n" +
                            $"Expected {ATTINY8X7_HW_ID_CODE} for AdaFruit ATTINY8X7 processo, but found {hwid} which is neither.");
            }

            options = GetOptions();
            Version = GetVersion();
        }

        /// <summary>
        /// Get the firmware version of the Seesaw board.
        /// </summary>
        /// <returns>Returns the seesaw version.</returns>
        private uint GetVersion() => BinaryPrimitives.ReadUInt32BigEndian(Read(MySeesawModule.Status, MySeesawFunction.StatusVersion, 4));

        /// <summary>
        /// Gets the options/modules present on the Seesaw board
        /// </summary>
        /// <returns>Returns 32 bit integer where the bits represent the options/modules present on the Seesaw board.</returns>
        private uint GetOptions() => BinaryPrimitives.ReadUInt32BigEndian(Read(MySeesawModule.Status, MySeesawFunction.StatusOptions, 4));

        /// <summary>
        /// Write a byte to the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">An Seesaw_Module enum that represents the module that we are writing to.</param>
        /// <param name="functionAddress">An Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="value">The byte value ro be send as a parameter to the Seesaw device.</param>
        private void WriteByte(MySeesawModule moduleAddress, MySeesawFunction functionAddress, byte value)
        {
            Write(moduleAddress, functionAddress, new byte[] { value });
        }

        /// <summary>
        /// Write a series of bytes to the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">An Seesaw_Module enum that represents the module that we are writing to.</param>
        /// <param name="functionAddress">An Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="data">The Span containing data to be send as a parameter or parameters to the Seesaw device.</param>
        private void Write(MySeesawModule moduleAddress, MySeesawFunction functionAddress, ReadOnlySpan<byte> data)
        {
            const int stackThreshold = 512;

            var buffer = data.Length < stackThreshold ? stackalloc byte[data.Length + 2] : new byte[data.Length + 2];

            buffer[0] = (byte)moduleAddress;
            buffer[1] = (byte)functionAddress;
            data.CopyTo(buffer[2..]);

            I2CDevice.Write(buffer);
        }

        /// <summary>
        /// Read a byte from the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">A Seesaw_Module enum that represents the module that we are reading from.</param>
        /// <param name="functionAddress">A Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="readDelayMicroSeconds">The delay between sending the function and readinng the data in microseconds.</param>
        /// <returns>Returns the byte value from the Seesaw device.</returns>
        private byte ReadByte(MySeesawModule moduleAddress, MySeesawFunction functionAddress, short readDelayMicroSeconds = 0) => Read(moduleAddress, functionAddress, 1, readDelayMicroSeconds)[0];

        /// <summary>
        /// Read a byte array from the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">A Seesaw_Module enum that represents the module that we are reading from.</param>
        /// <param name="functionAddress">A Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="length">The number of bytes that are expected to be returned from the Seesaw device.</param>
        /// <param name="readDelayMicroSeconds">The delay between sending the function and reading the data in microseconds.</param>
        /// <returns>Returns the byte array representing values from the Seesaw device.</returns>
        private byte[] Read(MySeesawModule moduleAddress, MySeesawFunction functionAddress, int length, short? readDelayMicroSeconds = 0)
        {
            var delay = readDelayMicroSeconds ?? DefaultReadDelayMicroseconds;
            var retval = new byte[length];

            Span<byte> bytesToWrite = stackalloc byte[2]
            {
                (byte)moduleAddress, (byte)functionAddress
            };

            I2CDevice.Write(bytesToWrite);

            if (delay > 0)
            {
                MyDelayHelper.DelayMicroseconds(delay, true);
            }

            I2CDevice.Read(retval);

            return retval;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            I2CDevice?.Dispose();
            I2CDevice = null!;
        }
    }
}
