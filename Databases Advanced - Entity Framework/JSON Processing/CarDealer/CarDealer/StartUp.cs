using CarDealer.App.Core;

namespace CarDealer.App
{
    public class StartUp
    {
        public static void Main()
        {
            var engine = new Engine();

            engine.Run();
        }
    }
}
