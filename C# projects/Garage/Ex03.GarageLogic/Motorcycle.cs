using System;

namespace Ex03.GarageLogic
{
    public class Motorcycle : Vehicle
    {
        private const uint k_NumOfWheelsInMotorcycle = 2;
        enum eLicenseType
        {
            A,
            A1,
            A2,
            B
        }

        eLicenseType m_LicenseType;
        int m_EngineVolume;
        
        internal Motorcycle(string i_LicenseNum) : base(i_LicenseNum, k_NumOfWheelsInMotorcycle)
        { }

        public string SetLicenseType
        {
            set
            {
                eLicenseType licenseType;
                bool check = System.Enum.TryParse(value, out licenseType);
                if (!check || !Enum.IsDefined(typeof(eLicenseType), value))
                {
                    throw new ArgumentException();
                }
                m_LicenseType = licenseType;
            }
        }

        public string SetEngineVolume
        {
            set
            {
                int result;
                bool check = int.TryParse(value, out result);
                if (!check || result < 0)
                {
                    throw new ArgumentException();
                }
                m_EngineVolume = result;
            }
        }

        public override string ToString()
        {
            string str = base.ToString();
            str += string.Format(@"License type: {0}
Engine volume: {1}", m_LicenseType, m_EngineVolume);
            return str;
        }
    }
}
