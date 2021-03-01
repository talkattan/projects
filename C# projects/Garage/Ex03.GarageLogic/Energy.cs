namespace Ex03.GarageLogic
{
    abstract public class Energy
    {
        float m_CurrentAmount;
        float m_MaximalAmount;

        protected Energy()
        { }

        // This method should not get string parameter, but a float one. The users of the class are responsible for parsing strings
        public string MaxAmount
        {
            set
            {
                float result;
                bool check = float.TryParse(value, out result);
                if (!check || result <= 0)
                {
                    throw new ValueOutOfRangeException();
                }
                m_MaximalAmount = result;
            }
        }

        public float GetMaxAmount
        {
            get
            {
                return m_MaximalAmount;
            }
        }

        public string CurrentAmount {
            set
            {
                float result;
                bool check = float.TryParse(value, out result);
                if (!check || result < 0 || result > m_MaximalAmount)
                {
                    throw new ValueOutOfRangeException();
                }
                m_CurrentAmount = result;
            }
        }

        public float GetCurrentAmount
        {
            get
            {
                return m_CurrentAmount;
            }
        }

        protected void ReEnergize(float i_EnergyToAdd)
        {
            if (i_EnergyToAdd + m_CurrentAmount > m_MaximalAmount || i_EnergyToAdd < 0)
            {
                throw new ValueOutOfRangeException();
            }
            m_CurrentAmount += i_EnergyToAdd;
        }

        public abstract override string ToString();
    }
}
