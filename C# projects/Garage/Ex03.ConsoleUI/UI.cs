using System;
using System.Collections.Generic;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
    class UI
    {
        Garage m_Garage;

        public void Start()
        {
            m_Garage = new Garage();
            while (true)
            {
                int userOp = getOp();
                Console.Clear();
                switch (userOp)
                {
                    case 1:
                        InsertUI.Insert(ref m_Garage);
                        break;
                    case 2:
                        displayVehicles();
                        break;
                    case 3:
                        changeVehicleStatus();
                        break;
                    case 4:
                        inflateWheelsInVehicle();
                        break;
                    case 5:
                        Vehicle vehicleFuel;
                        bool checkFuel = EnergyUI.checkIfValidVehicleLicenseNumAndEnergy("Fuel", out vehicleFuel, ref m_Garage);
                        if (checkFuel)
                        {
                            Fuel fuel = vehicleFuel.Energy as Fuel;
                            string fuelTypeToAdd = EnergyUI.askForFuelTypeToAdd();
                            float amountToRefuel = EnergyUI.askForAmount("Fuel");
                            try
                            {
                                fuel.Refuel(amountToRefuel, fuelTypeToAdd);
                            }
                            catch (ArgumentException)
                            {
                                System.Console.WriteLine(string.Format(@"Wrong type of fuel! This vehicle has fuel of type {0}.
", vehicleFuel.FuelType));
                            }
                            catch (ValueOutOfRangeException)
                            {
                                System.Console.WriteLine(@"You added too much fuel or a negative amount of fuel!
");
                            }
                        }
                        break;
                    case 6:
                        Vehicle vehicleElect;
                        bool checkElect = EnergyUI.checkIfValidVehicleLicenseNumAndEnergy("Electricity", out vehicleElect, ref m_Garage);
                        if (checkElect)
                        {
                            Electricity electricity = vehicleElect.Energy as Electricity;
                            float amountToRecharge = EnergyUI.askForAmount("Electricity");
                            electricity.Recharge(amountToRecharge);
                        }
                        break;
                    case 7:
                        string licenseNum;
                        bool check = getLicenseNumFromUser(out licenseNum, ref m_Garage);
                        if (check)
                        {
                            VehicleInGarage vehicleInGarage = m_Garage.GetVehicleInGarage(licenseNum);
                            System.Console.WriteLine(vehicleInGarage);
                        }
                        break;
                }
            }
        }

        internal static bool getLicenseNumFromUser(out string o_LicenseNum, ref Garage io_Garage)
        {
            System.Console.WriteLine("Please enter a license number:");
            o_LicenseNum = null;
            bool check = false;
            string userInput = System.Console.ReadLine();
            VehicleInGarage vehicleInGarage = io_Garage.GetVehicleInGarage(userInput);
            if (vehicleInGarage == null)
            {
                System.Console.WriteLine(@"License number does not exist in the garage!
");
            }
            else
            {
                o_LicenseNum = userInput;
                check = true;
            }
            return check;
        }

        private void inflateWheelsInVehicle()
        {
            string licenseNum;
            bool check = getLicenseNumFromUser(out licenseNum, ref m_Garage);//ref not needed
            if (check)
            {
                VehicleInGarage vehicleInGarage = m_Garage.GetVehicleInGarage(licenseNum);
                vehicleInGarage.InflateAllWheels();
                System.Console.Clear();
            }
        }

        private void changeVehicleStatus()
        {
            bool isEmpty = false;
            isEmpty = m_Garage.IsEmpty();
            if (isEmpty)
            {
                System.Console.WriteLine(@"No vehicles in the garage! Please add a vehicle first.

");
            }
            else
            {
                string licenseNum;
                bool check = getLicenseNumFromUser(out licenseNum, ref m_Garage);
                if (check)
                {
                    VehicleInGarage vehicleInGarage = m_Garage.GetVehicleInGarage(licenseNum);
                    System.Console.WriteLine("Please enter a status (like 'Repaired', 'Payed_for' etc):");
                    licenseNum = System.Console.ReadLine();
                    check = false;
                    while (!check)
                    {
                        try
                        {
                            vehicleInGarage.Status = licenseNum;
                            check = true;
                        }
                        catch (ArgumentException e)
                        {
                            System.Console.WriteLine(@"Invalid status! Please enter a valid status for the vehicle.");
                            licenseNum = System.Console.ReadLine();

                        }
                    }
                }
                System.Console.Clear();
            }
        }

        private void displayVehicles()
        {
            bool isEmpty = false;
            isEmpty = m_Garage.IsEmpty();
            if (isEmpty)
            {
                System.Console.WriteLine(@"No vehicles in the garage! Please add a vehicle first.

");
            }
            else
            {
                System.Console.WriteLine(@"Choose the option most suitable to you:
    1. No filter
    2. Show only vehicles in status 'In_Repair'
    3. Show only vehicles in status 'Repaired'
    4. Show only vehicles in status 'Payed_for'");
                int userInput;
                bool check = int.TryParse(System.Console.ReadLine(), out userInput);
                while (!check || userInput < 1 || userInput > 4)
                {
                    System.Console.WriteLine("Input is invalid! Please enter a number in the range 1 to 4 (inclusive).");
                    check = int.TryParse(System.Console.ReadLine(), out userInput);
                }

                VehicleInGarage.eVehicleStatus inputStatus;//crate eNum with the status
                List<string> listToDisplay = null;
                switch (userInput)
                {
                    case 1:
                        listToDisplay = m_Garage.TraverseLicenseNumbersInGarageDict();
                        break;
                    case 2:
                        inputStatus = VehicleInGarage.eVehicleStatus.In_Repair;
                        listToDisplay = m_Garage.TraverseLicenseNumbersInGarageDict(inputStatus);
                        break;
                    case 3:
                        inputStatus = VehicleInGarage.eVehicleStatus.Repaired;
                        listToDisplay = m_Garage.TraverseLicenseNumbersInGarageDict(inputStatus);
                        break;
                    case 4:
                        inputStatus = VehicleInGarage.eVehicleStatus.Payed_for;
                        listToDisplay = m_Garage.TraverseLicenseNumbersInGarageDict(inputStatus);
                        break;
                }
                if (listToDisplay.Count == 0)
                {
                    System.Console.WriteLine(@"No vehicles answer this criteria! Try another option.
    ");
                }
                else
                {
                    System.Console.WriteLine("List of vehicle license numbers that answer the criteria:");
                    foreach (string licenseNum in listToDisplay)
                    {
                        System.Console.WriteLine(licenseNum);
                    }
                }
                System.Console.WriteLine();
            }
        }

        private int getOp()
        {
            System.Console.WriteLine(@"Welcome to the garage! Please enter one of the following options:
1. “Insert” a new vehicle into the garage.
2. Display a list of license numbers currently in the garage.
3. Change a certain vehicle’s status.
4. Inflate tires to maximum.
5. Refuel a fuel-based vehicle.
6. Charge an electric-based vehicle.
7. Display vehicle information.

Enter a number based on the option you want.
Please enter a number in the range 1 to 7 (inclusive).");
            int userInput;
            bool check = int.TryParse(System.Console.ReadLine(), out userInput);
            while (!check || userInput < 1 || userInput > 7)
            {
                System.Console.WriteLine("Input is invalid! Please enter a number in the range 1 to 7 (inclusive).");
                check = int.TryParse(System.Console.ReadLine(), out userInput);
            }
            return userInput;
        }
    }
}
