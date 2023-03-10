using System.Device.Gpio;

namespace CheckSeeSaw
{
    public partial class MySeesaw
    {
        public ulong TestDigitalReadBulk(short delay = 8)
        {
            var buf = Read(MySeesawModule.Gpio, MySeesawFunction.GpioBulk, 8, delay);
            return (uint)BitConverter.ToInt32(buf, 0);
        }
        
        public void TestPinModeBulk(int capacity, int offset, ulong pins, PinMode mode)
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