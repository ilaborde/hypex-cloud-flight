using HyperxHeadset.Library;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var headset = new Headset();
            Console.WriteLine($"Battery level: {headset.ReadBattery()}");
            headset.Close();
        }
    }
}
