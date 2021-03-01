namespace Ex03.GarageLogic
{
    public class Fuel : Energy
    {
        enum eFuelType
        {
            Soler,
            Octane95,
            Octane96,
            Octane98
        }

        eFuelType m_FuelType;

        internal Fuel()
        { }

        public string FuelType
        {
            set
            {
                eFuelType FuelType;
                bool checkIfValid = false;
                checkIfValid = System.Enum.TryParse(value, out FuelType);
                if (!checkIfValid || !System.Enum.IsDefined(typeof(eFuelType), value))
                {
                    throw new System.ArgumentException();
                }
                m_FuelType = FuelType;
            }

            get
            {
                return m_FuelType.ToString();
            }
        }

        public void Refuel(float i_AmountOfFuelToAdd, string i_FuelTypeToAdd)
        {
            eFuelType FuelTypeToAdd;
            bool check = System.Enum.TryParse(i_FuelTypeToAdd, out FuelTypeToAdd);
            if (!check || m_FuelType != FuelTypeToAdd || !System.Enum.IsDefined(typeof(eFuelType), FuelTypeToAdd))
            {
                throw new System.ArgumentException();
            }
            ReEnergize(i_AmountOfFuelToAdd);
        }

        public override string ToString()
        {
            return string.Format(@"Fuel type: {0}
Current amount of fuel (in litters): {1}
Maximal amount of fuel (in litters): {2}", m_FuelType, base.GetCurrentAmount, base.GetMaxAmount);
        }
    }
}
