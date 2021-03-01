using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    abstract public class Vehicle
    {
        public class Wheel
        {
            string m_Manufacturer;
            float m_CurrentAirPressure;
            float m_MaximalAirPressure;
            
            internal Wheel()
            { }

            public string SetWheelMaxAmount //becuse os 'set', func dont get argument' just the value
            {
                set
                {
                    float result;
                    bool check = float.TryParse(value, out result);
                    if (!check || result <= 0)
                    {
                        throw new ValueOutOfRangeException();
                    }
                    m_MaximalAirPressure = result;
                }
            }

            public string SetWheelCurrentAmount
            {
                set
                {
                    float result;
                    bool check = float.TryParse(value, out result);
                    if (!check || result < 0 || result > m_MaximalAirPressure)
                    {
                        throw new ValueOutOfRangeException();
                    }
                    m_CurrentAirPressure = result;
                }
            }

            public string SetWheelManufacturer
            {
                set
                {
                    m_Manufacturer = value;
                }
            }

            internal void Inflate(float i_AirToAdd)
            {
                if (i_AirToAdd + m_CurrentAirPressure > m_MaximalAirPressure || i_AirToAdd < 0)
                {
                    throw new ValueOutOfRangeException();
                }
                m_CurrentAirPressure += i_AirToAdd;
            }

            internal void InflateToMax()
            {
                m_CurrentAirPressure = m_MaximalAirPressure;
            }

            public override string ToString()
            {
                return string.Format(@"Manufacturer name: {0}
Current air pressure: {1}
Max air pressure as recommended by manufacturer: {2}", m_Manufacturer, m_CurrentAirPressure, m_MaximalAirPressure);
            }
        }

        internal void InflateAllWheels()
        {
            foreach (Wheel wheel in m_Wheels)
            {
                wheel.InflateToMax();
            }
        }

        string m_LicenseNum;
        string m_ModelName;
        Energy m_VehicleEnergy;
        Wheel[] m_Wheels;

        protected Vehicle(string i_LicenseNum, uint i_NumOfWheels)
        {
            m_LicenseNum = i_LicenseNum;
            m_Wheels = new Wheel[i_NumOfWheels];
            // Initialize all wheels, so methods accessing it won't have null dereference
            for (uint i = 0; i < m_Wheels.Length; i++)
            {
                m_Wheels[i] = new Wheel();
            }
        }

        public int GetNumOfWheels
        {
            get
            {
                return m_Wheels.Length;
            }
        }

        public string ModelName
        {
            set
            {
                m_ModelName = value;
            }
        }

        public Energy GetEnergySource
        {
            get
            {
                return m_VehicleEnergy;
            }
        }

        public float RemainingEnergyPercentage()
        {
            return m_VehicleEnergy.GetCurrentAmount / m_VehicleEnergy.GetMaxAmount;
        }

        // Would be better if this was an enum instead of a string. Using it will be more intuitive
        public string EnergySource
        {
            set
            {
                switch (value)
                {
                    case "Fuel": m_VehicleEnergy = new Fuel();
                        break;
                    case "Electricity": m_VehicleEnergy = new Electricity();
                        break;
                    default: throw new System.ArgumentException();
                    
                }
            }
        }

        public string FuelType {
            set
            {
                if (!(m_VehicleEnergy is Fuel))
                {
                    throw new System.ArgumentException();
                }
                Fuel fuelOfVehicle = m_VehicleEnergy as Fuel;
                fuelOfVehicle.FuelType = value;
            }

            get
            {
                if (!(m_VehicleEnergy is Fuel))
                {
                    throw new System.ArgumentException();
                }
                Fuel fuelOfVehicle = m_VehicleEnergy as Fuel;
                return fuelOfVehicle.FuelType;
            }
        }

        public string MaxAmount {
            set
            {
                m_VehicleEnergy.MaxAmount = value;
            }
        }

        public string CurrentAmount {
            set
            {
                m_VehicleEnergy.CurrentAmount = value;
            }
        }

        public Energy Energy
        {
            get
            {
                return m_VehicleEnergy;
            }
        }

        protected void SetModelName(string i_ModelName)
        {
            m_ModelName = i_ModelName;
        }

        protected void SetEnergyType(string i_EnergyType)
        {
            if (i_EnergyType == "Fuel")
            {
                m_VehicleEnergy = new Fuel();
            }
            else if (i_EnergyType == "Electricity")
            {
                m_VehicleEnergy = new Electricity();
            }
            else
            {
                throw new System.ArgumentException();
            }
        }

        public void SetWheelMaxAmount(int i_WheelNum, string i_Input)
        {
            m_Wheels[i_WheelNum - 1].SetWheelMaxAmount = i_Input;
        }

        public void SetWheelCurrentAmount(int i, string input)
        {
            // This could be null dereference if m_Wheels[i - 1] isn't initialized, for example directly after
            // a call to the constructor, if it didn't initialize all of the array members
            m_Wheels[i - 1].SetWheelCurrentAmount = input;
        }

        public override string ToString()
        {
            string str = string.Format(@"Model name: {0}
License number: {1}
Percentage of energy left: {2}
Details about each wheel:
", m_ModelName, m_LicenseNum, RemainingEnergyPercentage());
            int i = 1;
            foreach (Wheel wheel in m_Wheels)
            {
                str += string.Format(@"Wheel number {0}:
{1}
", i, wheel);
                i++;
            }
            return str;
        }

        public void SetWheelManufacturer(int i_WheelNum, string i_Manufacturer)
        {
            m_Wheels[i_WheelNum - 1].SetWheelManufacturer = i_Manufacturer;
        }
    }
}