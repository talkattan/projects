using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class InputVehicleInitializer
    {
        public static Vehicle ParseVehicleTypeFromUser(string i_LicenseNum, string i_UserInput)
        {
            switch (i_UserInput)
            {
                case "Car": return new Car(i_LicenseNum);
                case "Motorcycle": return new Motorcycle(i_LicenseNum);
                case "Truck": return new Truck(i_LicenseNum);
                default: throw new System.ArgumentException();
            }
        }
    }
}
