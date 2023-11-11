using System.Buffers.Binary;
using System.Device.I2c;

namespace Core.PiConsole.Inputs.Seesaw
{
    /// <summary>
    /// Represents Seesaw device
    /// </summary>
    public partial class Attiny8X7SeeSaw : IDisposable
    {
        private const byte Attiny8X7HwIdCode = 0x87;

        /// <summary>
        /// I2C device used for communication
        /// </summary>
        /// <value></value>
        public I2cDevice I2CDevice { get; private set; }

        private uint options;

        /// <summary>
        /// Initializes new instance of Seesaw.
        /// </summary>
        /// <param name="i2CDevice">The I2cDevice to be used to communicate with the SeeSaw module. Note that the I2cDevice
        /// will be disposed when the along with the SeeSaw device</param>
        public Attiny8X7SeeSaw(I2cDevice i2CDevice)
        {
            I2CDevice = i2CDevice ?? throw new ArgumentNullException(nameof(i2CDevice));
            Initialize(i2CDevice);
        }
        
        /// <summary>
        /// Version of the SeeSaw module.
        /// </summary>
        public uint Version { get; private set; }
        
        /// <summary>
        /// Tests to see if a module has been compiled into the SeeSaw firmware.
        /// </summary>
        /// <param name="moduleAddress">>An Seesaw_Module enum that represents the mdule to test for.</param>
        /// <returns>Returns true if the functionality associated with the module is available.</returns>
        public bool HasModule(Iot.Device.Seesaw.Seesaw.SeesawModule moduleAddress) => (options & (1 << (byte)moduleAddress)) != 0;

        /// <summary>
        /// Performs a soft reset of the SeeSaw module.
        /// </summary>
        public void SoftwareReset() => WriteByte(Iot.Device.Seesaw.Seesaw.SeesawModule.Status, SeesawFunction.StatusSwrst, 0xFF);

        /// <summary>
        /// Initializes the Seesaw device.
        /// <param name="i2CDevice">The I2cDevice to initialize the Seesaw device with.</param>
        /// </summary>
        private void Initialize(I2cDevice i2CDevice)
        {
            SoftwareReset();
            DelayHelper.DelayMilliseconds(10, true);

            var hwid = ReadByte(Iot.Device.Seesaw.Seesaw.SeesawModule.Status, SeesawFunction.StatusHwId);
            if (hwid != Attiny8X7HwIdCode)
            {
                throw new NotSupportedException(
                    $"The hardware on I2C Bus {this.I2CDevice.ConnectionSettings.BusId}, Address 0x{this.I2CDevice.ConnectionSettings.DeviceAddress:X2} does not appear to be an Adafruit SeeSaw module.\n" +
                            $"Expected {Attiny8X7HwIdCode} for AdaFruit ATTINY8X7 processor, but found {hwid} which is neither.");
            }

            options = GetOptions();
            Version = GetVersion();
        }

        /// <summary>
        /// Get the firmware version of the Seesaw board.
        /// </summary>
        /// <returns>Returns the seesaw version.</returns>
        private uint GetVersion() => BinaryPrimitives.ReadUInt32BigEndian(Read(Iot.Device.Seesaw.Seesaw.SeesawModule.Status, SeesawFunction.StatusVersion, 4));

        /// <summary>
        /// Gets the options/modules present on the Seesaw board
        /// </summary>
        /// <returns>Returns 32 bit integer where the bits represent the options/modules present on the Seesaw board.</returns>
        private uint GetOptions() => BinaryPrimitives.ReadUInt32BigEndian(Read(Iot.Device.Seesaw.Seesaw.SeesawModule.Status, SeesawFunction.StatusOptions, 4));

        /// <summary>
        /// Write a byte to the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">An Seesaw_Module enum that represents the module that we are writing to.</param>
        /// <param name="functionAddress">An Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="value">The byte value ro be send as a parameter to the Seesaw device.</param>
        private void WriteByte(Iot.Device.Seesaw.Seesaw.SeesawModule moduleAddress, SeesawFunction functionAddress, byte value)
        {
            Write(moduleAddress, functionAddress, new byte[] { value });
        }

        /// <summary>
        /// Write a series of bytes to the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">An Seesaw_Module enum that represents the module that we are writing to.</param>
        /// <param name="functionAddress">An Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="data">The Span containing data to be send as a parameter or parameters to the Seesaw device.</param>
        private void Write(Iot.Device.Seesaw.Seesaw.SeesawModule moduleAddress, SeesawFunction functionAddress, ReadOnlySpan<byte> data)
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
        private byte ReadByte(Iot.Device.Seesaw.Seesaw.SeesawModule moduleAddress, SeesawFunction functionAddress, short readDelayMicroSeconds = 0) => Read(moduleAddress, functionAddress, 1, readDelayMicroSeconds)[0];

        /// <summary>
        /// Read a byte array from the I2cDevice connected to the Seesaw board.
        /// </summary>
        /// <param name="moduleAddress">A Seesaw_Module enum that represents the module that we are reading from.</param>
        /// <param name="functionAddress">A Seesaw_Function enum that represents the Seesaw function to be called.</param>
        /// <param name="length">The number of bytes that are expected to be returned from the Seesaw device.</param>
        /// <param name="readDelayMicroSeconds">The delay between sending the function and reading the data in microseconds.</param>
        /// <returns>Returns the byte array representing values from the Seesaw device.</returns>
        private byte[] Read(Iot.Device.Seesaw.Seesaw.SeesawModule moduleAddress, SeesawFunction functionAddress, int length, short readDelayMicroSeconds = 0)
        {
            var retval = new byte[length];

            Span<byte> bytesToWrite = stackalloc byte[2]
            {
                (byte)moduleAddress, (byte)functionAddress
            };

            I2CDevice.Write(bytesToWrite);

            if (readDelayMicroSeconds > 0)
            {
                DelayHelper.DelayMicroseconds(readDelayMicroSeconds, true);
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
