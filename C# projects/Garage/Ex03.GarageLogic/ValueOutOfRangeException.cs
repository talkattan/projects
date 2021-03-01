using System;
using System.Runtime.Serialization;

namespace Ex03.GarageLogic
{
    [Serializable]
    public class ValueOutOfRangeException : Exception
    {
        public float m_MaxValue;
        public float m_MinValue;

        public ValueOutOfRangeException()
        {
        }

        public ValueOutOfRangeException(Exception i_innerException) : base("The value entered is out of range.", i_innerException)
        {
        }

        public ValueOutOfRangeException(Exception i_innerException, float i_MaxValue, float i_MinValue) 
            : base(string.Format("The value entered is out of the range {0} to {1} inclusive.", i_MinValue, i_MaxValue), i_innerException)
        {
            m_MaxValue = i_MaxValue;
            m_MinValue = i_MinValue;
        }

        public ValueOutOfRangeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}