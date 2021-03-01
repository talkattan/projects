using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    class MotorcycleUI
    {
        internal static void askForLicenseType(Motorcycle io_Motorcycle)
        {
            bool check = false;
            System.Console.WriteLine("Please enter a license type: 'A', 'B', 'A2' etc:");
            string input = System.Console.ReadLine();
            while (!check)
            {
                try
                {
                    io_Motorcycle.SetLicenseType = input;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Input is invalid. Please enter a license type (A, B etc.):");
                    input = System.Console.ReadLine();
                }
            }
        }

        internal static void askForEngineVolume(Motorcycle io_Motorcycle)
        {
            bool check = false;
            System.Console.WriteLine("Please enter a positive engine volume:");
            string input = System.Console.ReadLine();
            while (!check)
            {
                try
                {
                    io_Motorcycle.SetEngineVolume = input;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Input is invalid. Please enter a positive engine volume:");
                    input = System.Console.ReadLine();
                }
            }
            System.Console.Clear();
        }
    }
}
