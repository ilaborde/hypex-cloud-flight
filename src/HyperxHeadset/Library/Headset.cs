using System.Linq;

namespace HyperxHeadset.Library
{
    public class Headset
    {
        private readonly HIDDevice device;

        public Headset()
        {
            var devices = HIDDevice.getConnectedDevices()
                .Where(x => x.PID == 5923 && x.VID == 2385)
                .ToArray();

            //Select a device from the available devices
            string devicePath = devices[2].devicePath;

            //create a handle to the device by calling the constructor
            device = new HIDDevice(devicePath, false);
        }

        /// <summary>
        /// Read the battery level of the headset, 199 means charging
        /// </summary>
        /// <returns></returns>
        public int ReadBattery()
        {
            byte[] reportIn = GetReport();
            var chargeState = reportIn[3];
            var magicValue = reportIn[4] != 0 ? reportIn[4] : chargeState;
            return CalculatePercentage(chargeState, magicValue);
        }

        public bool IsCharging()
        {
            return ReadBattery() == 199;
        }

        /// <summary>
        /// close the device to release all handles
        /// </summary>
        public void Close()
        {
            device.close();
        }

        private byte[] GetReport()
        {
            byte[] report = new byte[20];
            report[0] = 0x21;
            report[1] = 0xff;
            report[2] = 0x05;
            device.Write(report);

            //Read a byte array from the device
            return device.Read();
        }

        private static int CalculatePercentage(int chargeState, int magicValue)
        {
            if (chargeState == 0x10)
            {
                if (magicValue <= 11)
                {
                    return 200; // full?
                }
                return 199; // charging
            }

            if (chargeState == 0xf)
            {
                if (magicValue >= 130)
                {
                    return 100;
                }

                if (magicValue < 130 && magicValue >= 120)
                {
                    return 95;
                }

                if (magicValue < 120 && magicValue >= 100)
                {
                    return 90;
                }

                if (magicValue < 100 && magicValue >= 70)
                {
                    return 85;
                }

                if (magicValue < 70 && magicValue >= 50)
                {
                    return 80;
                }

                if (magicValue < 50 && magicValue >= 20)
                {
                    return 75;
                }

                if (magicValue < 20 && magicValue > 0)
                {
                    return 70;
                }
            }
            if (chargeState == 0xe)
            {
                if (magicValue < 250 && magicValue > 240)
                {
                    return 65;
                }

                if (magicValue < 240 && magicValue >= 220)
                {
                    return 60;
                }

                if (magicValue < 220 && magicValue >= 208)
                {
                    return 55;
                }

                if (magicValue < 208 && magicValue >= 200)
                {
                    return 50;
                }

                if (magicValue < 200 && magicValue >= 190)
                {
                    return 45;
                }

                if (magicValue < 190 && magicValue >= 180)
                {
                    return 40;
                }

                if (magicValue < 179 && magicValue >= 169)
                {
                    return 35;
                }

                if (magicValue < 169 && magicValue >= 159)
                {
                    return 30;
                }

                if (magicValue < 159 && magicValue >= 148)
                {
                    return 25;
                }

                if (magicValue < 148 && magicValue >= 119)
                {
                    return 20;
                }

                if (magicValue < 119 && magicValue >= 90)
                {
                    return 15;
                }

                if (magicValue < 90)
                {
                    return 10;
                }

                return 66;
            }
            return 255; //error
        }
    }
}
