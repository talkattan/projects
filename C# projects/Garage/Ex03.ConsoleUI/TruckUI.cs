using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal class TruckUI
    {
        internal static void askForDangerousMaterials(Truck io_Truck)
        {
            bool check = false;
            System.Console.WriteLine("Are there dangerous materials on the truck? If so, write 'Yes', else write 'No'.");
            string input = System.Console.ReadLine();
            while (!check)
            {
                if (input.Equals("Yes"))
                {
                    io_Truck.SetDangerousMaterials = true;
                    check = true;
                }
                else if (input.Equals("No"))
                {
                    io_Truck.SetDangerousMaterials = false;
                    check = true;
                }
                else
                {
                    System.Console.WriteLine("Invalid input. Please answer with Yes or No:");
                    input = System.Console.ReadLine();
                }
            }
        }

        internal static void askForVolumeOfCargo(Truck io_Truck)
        {
            bool check = false;
            System.Console.WriteLine("Please enter volume of cargo (positive number):");
            string input = System.Console.ReadLine();
            while (!check)
            {
                try
                {
                    io_Truck.SetCargoVol = input;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Invalid input. Please enter volume of cargo (positive number):");
                    input = System.Console.ReadLine();
                }
            }
            System.Console.Clear();
        }
    }
}
