namespace Ex03.GarageLogic
{
    public class Electricity : Energy
    {
        internal Electricity()
        { }

        public void Recharge(float i_MinutesToAdd)
        {
            ReEnergize(i_MinutesToAdd / 60);
        }

        public override string ToString()
        {
            return string.Format(@"Hours Remaining time of engine operation (in hours): {0}
Maximum time of engine operation (in hours): {1}", base.GetCurrentAmount, base.GetMaxAmount);
        }
    }
}
