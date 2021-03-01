using System;

namespace Ex03.GarageLogic
{
    public class Truck : Vehicle
    {
        private const uint k_NumOfWheelsInMotorcycle = 12;
        bool m_ContainsDangerousMaterials;
        float m_CargoVolume;

        internal Truck(string i_LicenseNum) : base(i_LicenseNum, k_NumOfWheelsInMotorcycle)
        { }

        public bool SetDangerousMaterials
        {
            set
            {
                m_ContainsDangerousMaterials = value;
            }
        }

        public string SetCargoVol
        {
            set
            {
                float result;
                bool check = float.TryParse(value, out result);
                if (!check || result <= 0)
                {
                    throw new ValueOutOfRangeException();
                }
                m_CargoVolume = result;
            }
        }

        public override string ToString()
        {
            string str = base.ToString();
            string dangerousMaterials = m_ContainsDangerousMaterials ? "Yes" : "No";
            str += string.Format(@"Contains dangerous materials: {0}
Volume of cargo: {1}", dangerousMaterials, m_CargoVolume.ToString());
            return str;
        }
    }
}
