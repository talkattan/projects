using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03.GarageLogic
{
    public class Car : Vehicle
    {
        private const uint k_NumOfWheelsInCar = 4;
        internal enum eCarColor
        {
            Red,
            Blue,
            Black,
            Gray
        }
        internal enum eNumOfDoors
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5
        }
        eCarColor m_Color;
        eNumOfDoors m_NumOfDoors;

        public string SetColor
        { 
            set
            {
                eCarColor carColor;
                bool check = System.Enum.TryParse(value, out carColor);
                if (!check || !Enum.IsDefined(typeof(eCarColor), value))
                {
                    throw new ArgumentException();
                }
                m_Color = carColor;
            }
        }

        public string SetNumOfDoors
        {
            set
            {
                eNumOfDoors numOfDoors;
                bool check = System.Enum.TryParse(value, out numOfDoors);
                if (!check || !Enum.IsDefined(typeof(eNumOfDoors), value))
                {
                    throw new ArgumentException();
                }
                m_NumOfDoors = numOfDoors;
            }
        }

        internal Car(string i_LicenseNum) : base(i_LicenseNum, k_NumOfWheelsInCar)
        { }

        public override string ToString()
        {
            string str = base.ToString();
            str += string.Format(@"Color: {0}
Number of doors: {1}", m_Color, m_NumOfDoors);
            return str;
        }
    }
}
