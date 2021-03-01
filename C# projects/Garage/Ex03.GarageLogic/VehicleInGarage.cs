using System;

namespace Ex03.GarageLogic
{
    public class VehicleInGarage
    {
        const int k_MaxLengthOfPhoneNum = 10;
        const int k_MinLengthOfPhoneNum = 10;

        public enum eVehicleStatus
        {
            In_Repair,
            Repaired,
            Payed_for
        }

        internal string m_OwnerName;
        internal string m_OwnerPhoneNum;
        internal Vehicle m_Vehicle;
        internal eVehicleStatus m_VehicleStatus = eVehicleStatus.In_Repair;
        //there is no use to have 'set' to every property' cus we dont change mose of them/ so its better to do construtor to all of them
        //in our implementation' its possible to create object with missing properties
        internal VehicleInGarage()
        { }

        public eVehicleStatus GetStatus
        {
            get
            {
                return m_VehicleStatus;
            }
        }

        public string Status
        {
            set
            {
                eVehicleStatus vehicleStatus;
                bool check = System.Enum.TryParse(value, out vehicleStatus);
                if (!check || !Enum.IsDefined(typeof(eVehicleStatus), value))
                {
                    throw new ArgumentException();
                }
                m_VehicleStatus = vehicleStatus;
            }
        }

        public Vehicle GetVehicle
        {
            get
            {
                return m_Vehicle;
            }
        }

        public string SetOwnerName
        {
            set
            {
                m_OwnerName = value;
            }
        }

        public string SetOwnerPhone
        {
            set
            {
                if (Garage.CheckIfValidNumRange(value, k_MinLengthOfPhoneNum, k_MaxLengthOfPhoneNum))
                {
                    m_OwnerPhoneNum = value;
                }
                else
                {
                    throw new ValueOutOfRangeException();
                }
            }
        }

        public void InflateAllWheels()
        {
            m_Vehicle.InflateAllWheels();
        }

        internal void ChangeStatusToInRepair()
        {
            m_VehicleStatus = eVehicleStatus.In_Repair;
        }

        public override string ToString()
        {
            return string.Format(@"Owner: {0}
Owner phone number: {1}
Vehicle state in garage: {2}
Vehicle details:
{3}", m_OwnerName, m_OwnerPhoneNum, m_VehicleStatus, m_Vehicle);
        }
    }
}