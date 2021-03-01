using Ex03.GarageLogic;
using System;

namespace Ex03.ConsoleUI
{
    class InsertUI
    {
        internal static void Insert(ref Garage io_Garage) 
        {
            System.Console.WriteLine("Please enter a license number:");
            bool licenseNumInDict = false;
            string licenseNum = System.Console.ReadLine();
            bool licenseNumValid = false;
            while (!licenseNumValid)
            {
                try
                {
                    licenseNumInDict = io_Garage.PutNewVehicleInGarage(licenseNum);
                    licenseNumValid = true;
                }
                catch (ArgumentException)
                {
                    System.Console.WriteLine(@"License number invalid! Should be a number with 7 or 8 digits.");
                    licenseNum = System.Console.ReadLine();
                }
            }

            if (licenseNumInDict)
            {
                System.Console.WriteLine(@"This license number exists in the garage! Its status has changed to 'In Repair'.
");
            }
            else
            {
                System.Console.WriteLine("Please enter a desired type of vehicle:");
                string vehicleTypeStr = System.Console.ReadLine();
                Vehicle vehicle = null;
                while (vehicle == null)
                {
                    try
                    {
                        vehicle = InputVehicleInitializer.ParseVehicleTypeFromUser(licenseNum, vehicleTypeStr);
                    }
                    catch
                    {
                        System.Console.WriteLine(@"Invalid vehicle type! Try 'Car' for example.");
                        vehicleTypeStr = System.Console.ReadLine();
                    }
                }
                io_Garage.PutVehicleObjInDict(vehicle, licenseNum);

                askForOwnerName(licenseNum, ref io_Garage);//Set OwnerName to userInput
                askForOwnerPhoneNum(licenseNum, ref io_Garage);
                askForModelName(vehicle);
                askForEnergySource(vehicle);
                askForEnergyStats(vehicle);
                askForWheelsInfo(vehicle);

                if (vehicle is Motorcycle)
                {
                    MotorcycleUI.askForLicenseType(vehicle as Motorcycle);
                    MotorcycleUI.askForEngineVolume(vehicle as Motorcycle);
                }
                else if (vehicle is Car)
                {
                    CarUI.askForColor(vehicle as Car);
                    CarUI.askForNumOfDoors(vehicle as Car);
                }
                else if (vehicle is Truck)
                {
                    TruckUI.askForDangerousMaterials(vehicle as Truck);
                    TruckUI.askForVolumeOfCargo(vehicle as Truck);
                }
            }
        }

        private static void askForOwnerName(string i_LicenseNum, ref Garage io_Garage)
        {
            System.Console.WriteLine("Please enter an owner name:");
            string userInput = System.Console.ReadLine();
            io_Garage.GetVehicleInGarage(i_LicenseNum).SetOwnerName = userInput;//GetVehicleInGarage(i_LicenseNum) return VehicleInGarage
        }

        private static void askForOwnerPhoneNum(string i_LicenseNum, ref Garage io_Garage)
        {
            System.Console.WriteLine("Please enter an owner phone number (10 digits):");
            string userInput = System.Console.ReadLine();
            bool check = false;
            while (!check)
            {
                try
                {
                    io_Garage.GetVehicleInGarage(i_LicenseNum).SetOwnerPhone = userInput;
                    check = true;
                }
                catch
                {
                    System.Console.WriteLine("Invalid phone number! Please enter a number of 10 digits:");
                    userInput = System.Console.ReadLine();
                }
            }
        }

        private static void askForModelName(Vehicle o_Vehicle)
        {
            Console.WriteLine("Please enter a model name for your vehicle:");
            string input = System.Console.ReadLine();
            o_Vehicle.ModelName = input;
        }

        private static void askForEnergySource(Vehicle o_Vehicle)
        {
            Console.WriteLine("Please enter an energy source for your vehicle (can be 'Fuel' or 'Electricity' for example):");
            string input = System.Console.ReadLine();
            bool checkIfValid = false;
            while (!checkIfValid)
            {
                try
                {
                    o_Vehicle.EnergySource = input;
                    checkIfValid = true;
                }
                catch (ArgumentException)
                {
                    System.Console.WriteLine("Not a valid energy source! Please enter 'Fuel' or 'Electricity':");
                    input = System.Console.ReadLine();
                }
            }
        }

        private static void askForEnergyStats(Vehicle io_Vehicle)
        {
            bool isFuel = io_Vehicle.GetEnergySource is Fuel;
            string input;
            if (isFuel)
            {
                System.Console.WriteLine("Please enter desired fuel type (for example 'Soler', 'Octane95' etc.):");
                input = System.Console.ReadLine();
                bool checkIfValidFuelType = false;
                while (!checkIfValidFuelType)
                {
                    try
                    {
                        io_Vehicle.FuelType = input;
                        checkIfValidFuelType = true;
                    }
                    catch
                    {
                        System.Console.WriteLine("Not a valid fuel type! Please enter 'Octane95' for example.");
                        input = System.Console.ReadLine();
                    }
                }
            }
            if (isFuel)
            {
                System.Console.WriteLine("Please enter maximal amount of fuel (positive amount):");
            }
            else
            {
                System.Console.WriteLine("Please enter maximal amount of electricity in hours (positive amount):");
            }

            input = System.Console.ReadLine();
            bool checkIfValidMaxAmount = false;
            while (!checkIfValidMaxAmount)
            {
                try
                {
                    io_Vehicle.MaxAmount = input;
                    checkIfValidMaxAmount = true;
                }
                catch
                {
                    System.Console.WriteLine("Not a valid amount! Please enter a positive number:");
                    input = System.Console.ReadLine();
                }
            }

            if (isFuel)
            {
                System.Console.WriteLine("Please enter the current amount of fuel (non-negative amount):");
            }
            else
            {
                System.Console.WriteLine("Please enter the current amount of electricity left in hours (non-negative amount):");
            }

            input = System.Console.ReadLine();
            bool checkIfCurrentAmountValid = false;
            while (!checkIfCurrentAmountValid)
            {
                try
                {
                    io_Vehicle.CurrentAmount = input;
                    checkIfCurrentAmountValid = true;
                }
                catch
                {
                    System.Console.WriteLine("Not a valid amount! Please enter a non-negative number (below maximal amount):");
                    input = System.Console.ReadLine();
                }
            }
        }

        private static void askForWheelsInfo(Vehicle io_Vehicle)
        {
            for (int i = 1; i <= io_Vehicle.GetNumOfWheels; i++)
            {
                System.Console.WriteLine(string.Format("We will now ask for details about wheel number {0}.", i));
                System.Console.WriteLine("Please enter the manufacturer:");
                string manufacturer = System.Console.ReadLine();
                io_Vehicle.SetWheelManufacturer(i, manufacturer);

                System.Console.WriteLine("Please enter maximal air pressure as recommneded by the manufacturer (in PSI):");
                bool checkIfMaxAmountIsValid = false;
                string input = System.Console.ReadLine();
                while (!checkIfMaxAmountIsValid)
                {
                    try
                    {
                        io_Vehicle.SetWheelMaxAmount(i, input);
                        checkIfMaxAmountIsValid = true;
                    }
                    catch
                    {
                        System.Console.WriteLine("Invalid input! Please enter a positive number:");
                        input = System.Console.ReadLine();
                    }
                }

                System.Console.WriteLine("Please enter current air pressure in the wheel (in PSI):");
                bool checkIfCurrAmountIsValid = false;
                input = System.Console.ReadLine();
                while (!checkIfCurrAmountIsValid)
                {
                    try
                    {
                        io_Vehicle.SetWheelCurrentAmount(i, input);
                        checkIfCurrAmountIsValid = true;
                    }
                    catch
                    {
                        System.Console.WriteLine("Invalid input! Please enter a positive number, smaller than the maximal amount:");
                        input = System.Console.ReadLine();
                    }
                }
            }
        }
    }
}
