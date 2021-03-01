using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    internal class EnergyUI
    {
        internal static string askForFuelTypeToAdd()
        {
            System.Console.WriteLine("Please enter a fuel type ('Octane95', 'Soler' etc):");
            string userInput;
            userInput = System.Console.ReadLine();
            return userInput;
        }

        internal static float askForAmount(string i_EnergyType)
        {
            if (i_EnergyType == "Fuel")
            {
                System.Console.WriteLine("Please enter an amount of fuel to refuel (positive amount):");
            }
            else if (i_EnergyType == "Electricity")
            {
                System.Console.WriteLine("Please enter an amount of minutes to recharge (positive amount):");
            }
            string userInput = System.Console.ReadLine();
            float result;
            bool check = float.TryParse(userInput, out result);
            while (!check || result < 0)
            {
                System.Console.WriteLine("Invalid input! Please enter a positive amount):");
                userInput = System.Console.ReadLine();
                check = float.TryParse(userInput, out result);
            }
            return result;
        }

        internal static bool checkIfValidVehicleLicenseNumAndEnergy(string i_EnergyType, out Vehicle o_Vehicle, ref Garage io_Garage)
        {
            string licenseNum;
            bool check = UI.getLicenseNumFromUser(out licenseNum, ref io_Garage);
            bool checkIfCorrectEnergyType = false;
            Vehicle vehicleResult = null;
            if (check)
            {
                VehicleInGarage vehicleInGarage = io_Garage.GetVehicleInGarage(licenseNum);
                vehicleResult = vehicleInGarage.GetVehicle;
                if ((i_EnergyType == "Fuel" && !(vehicleResult.Energy is Fuel)) || (i_EnergyType == "Electricity" && !(vehicleResult.Energy is Electricity)))
                {
                    System.Console.WriteLine(@"This license number has a different energy source! Please try another vehicle license number of energy source.
");
                }
                else
                {
                    check = true;
                    checkIfCorrectEnergyType = true;
                }
            }
            o_Vehicle = vehicleResult;
            return check && checkIfCorrectEnergyType;
        }
    }
}
