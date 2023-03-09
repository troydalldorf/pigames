# Guide
https://learn.adafruit.com/adafruit-rgb-matrix-bonnet-for-raspberry-pi/driving-matrices

# Install .Net
The secure shell will prompt you for the password for the user pi. Once you are connected to the Raspberry Pi, issue the following command to download and install the .NET 5 dotnet SDK and runtime.

``` bash
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c Current --runtime dotnet
```

If you will be building from your Raspberry Pi directly, you must install the .NET SDK and runtime by issuing the following command instead.
``` bash
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin -c Current
```

To execute .NET applications correctly from your Raspberry Pi, you must add the .dotnet directory to your system's $Path and add a DOTNET_ROOT environment variable. You can do this by editing your user's .bashrc file. You can also make the appropriate changes automatically by executing the following command.
```bash
echo 'export DOTNET_ROOT=$HOME/.dotnet' >> ~/.bashrc
echo 'export PATH=$PATH:$HOME/.dotnet' >> ~/.bashrc
source ~/.bashrc
```

### TEST

``` csharp
using System;
 
Console.WriteLine("Hello from wellsb.com");
Console.ReadLine();
```

# LED RGB Matrix C#
https://github.com/hzeller/rpi-rgb-led-matrix/tree/master/bindings/c%23

# GPIO

# I2C

### Debug
``` bash
sudo i2cdetect -y 1
```

### Python (Just to See if things Work)
https://github.com/adafruit/Adafruit_CircuitPython_seesaw/blob/main/examples/seesaw_arcade_qt_multi_board.py

### C#
https://github.com/dotnet/iot

install Iot.Device.Bindings

### Hardware
- https://www.adafruit.com/product/5296
