using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal class CarUI
    {
        internal static void askForColor(Car io_Car)
        {
            bool check = false;
            System.Console.WriteLine("Please enter a color (Black, Blue etc):");
            string input = System.Console.ReadLine();
            while (!check)
            {
                try
                {
                    io_Car.SetColor = input;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Input is invalid. Please enter a color (Black, Blue, Red...):");
                    input = System.Console.ReadLine();
                }
            }
        }

        internal static void askForNumOfDoors(Car io_Car)
        {
            bool check = false;
            System.Console.WriteLine("Please enter number of doors (Two, Three, Four, Five):");
            string input = System.Console.ReadLine();
            while (!check)
            {
                try
                {
                    io_Car.SetNumOfDoors = input;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Invalid input. Please enter number of doors (Two, Three, Four, Five):");
                    input = System.Console.ReadLine();
                }
            }
            System.Console.Clear();
        }
    }
}
