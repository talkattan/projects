using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        const int k_MinLengthOfLicenseNum = 7;
        const int k_MaxLengthOfLicenseNum = 8;
        Dictionary<string, VehicleInGarage> m_GarageDict;

        public Garage()
        {
            m_GarageDict = new Dictionary<string, VehicleInGarage>();
        }

        public bool IsVehicleInGarage(string i_LicenseNum)
        {
            return m_GarageDict.ContainsKey(i_LicenseNum);
        }

        public bool PutNewVehicleInGarage(string i_LicenseNum)
        {
            if (!CheckIfValidNumRange(i_LicenseNum, k_MinLengthOfLicenseNum, k_MaxLengthOfLicenseNum))
            {
                throw new System.ArgumentException();
            }
            bool checkIfInDict = IsVehicleInGarage(i_LicenseNum);
            if (!checkIfInDict)
            {
                m_GarageDict.Add(i_LicenseNum, new VehicleInGarage());//not a good imlementation, we did not send the new car
            }
            else
            {
                m_GarageDict[i_LicenseNum].ChangeStatusToInRepair();
            }
            return checkIfInDict;
        }

        public static bool CheckIfValidNumRange(string i_LicenseNum, int i_Min, int i_Max)
        {
            bool check = true;
            foreach (char c in i_LicenseNum)
            {
                if (c < '0' || c > '9')
                    check &= false;
            }
            if (i_LicenseNum.Length > i_Max || i_LicenseNum.Length < i_Min)
            {
                check = false;
            }
            return check;
        }

        public VehicleInGarage GetVehicleInGarage(string i_UserInput)
        {
            VehicleInGarage result;
            bool inDict = m_GarageDict.TryGetValue(i_UserInput, out result);
            return (inDict) ? result : null;
        }

        public void PutVehicleObjInDict(Vehicle i_Vehicle, string i_LicenseNum)
        {
            m_GarageDict[i_LicenseNum].m_Vehicle = i_Vehicle;
        }

        public List<string> TraverseLicenseNumbersInGarageDict(VehicleInGarage.eVehicleStatus io_inputStatus)
        {
            List<string> listOfVehiclesToReturn = new List<string>();
            foreach (KeyValuePair<string, VehicleInGarage> entry in m_GarageDict)//KeyValuePair cus in dictionery we are going over pairs
            {
                if (entry.Value.GetStatus == io_inputStatus)// VehicleInGarage is the value, and GetStatus is method at VehicleInGarage
                {
                    listOfVehiclesToReturn.Add(entry.Key);//adds the lisence num
                }
            }
            return listOfVehiclesToReturn;
        }

        public List<string> TraverseLicenseNumbersInGarageDict()
        {
            return new List<string>(m_GarageDict.Keys);
        }

        public bool IsEmpty()
        {
            return m_GarageDict.Count == 0;// Count is a property of dictionery
        }
    }
}
