using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    public class GameLogic
    {
        private static Random m_Random = new Random();
        private int m_UniverseSize;
        private int m_CurrentGuess;
        private int m_NumOfChancesForUser;
        private const int k_GuessLength = 4;
        private HashSet<object> m_SetOfChoices;
        public int[] m_ComputerHiddenGuess;
        private bool m_DidWin;
        private bool m_GameFinished;

        public GameLogic(int i_NumOfChancesCount, int i_UniverseSize)
        {
            m_UniverseSize = i_UniverseSize;
            m_NumOfChancesForUser = i_NumOfChancesCount;
            m_SetOfChoices = new HashSet<object>();
            m_CurrentGuess = 0;
            m_DidWin = false;
            setComputerHiddenGuess();
        }

        public int CurrentGuess
        {
            get
            {
                return m_CurrentGuess;
            }

            set
            {
                m_CurrentGuess = value;
            }
        }

        public bool IsFinished
        {
            get
            {
                return m_GameFinished;
            }
            set
            {
                m_GameFinished = value;
            }
        }

        public int[] GetComputerColors
        {
            get
            {
                return m_ComputerHiddenGuess;
            }
        }

        private void setComputerHiddenGuess()
        {
            HashSet<int> colorsPool = new HashSet<int>();
            m_ComputerHiddenGuess = new int[k_GuessLength];

            while (colorsPool.Count < 4)
            {
                int rand = m_Random.Next(m_UniverseSize);
                colorsPool.Add(rand);
            }
            colorsPool.CopyTo(m_ComputerHiddenGuess);
        }

        public bool CheckIfValidInput(object i_AddOrNotToSet)
        {
            bool flag = false;
            if (!m_SetOfChoices.Contains(i_AddOrNotToSet))
            {
                flag = true;
                m_SetOfChoices.Add(i_AddOrNotToSet);
            }
            return flag;
        }

        public bool CheckIfFullGuess()
        {
            bool flag = false;
            if (m_SetOfChoices.Count == 4)
            {
                flag = true;
            }
            return flag;
        }

        public void RemoveItemFromSet(object i_BackColor)
        {
            m_SetOfChoices.Remove(i_BackColor);
        }

        public int[] CalcResults(int[] i_ColorsInNumbers)
        {
            int[] results = new int[2];
            for (int i = 0; i < i_ColorsInNumbers.Length; i++)
            {
                if (i_ColorsInNumbers[i] == m_ComputerHiddenGuess[i])
                {
                    results[0]++;
                }
                else if (m_ComputerHiddenGuess.Contains(i_ColorsInNumbers[i]))
                {
                    results[1]++;
                }
            }
            m_DidWin = results[0] == 4;
            ResetSet();
            m_GameFinished = m_DidWin || m_CurrentGuess >= m_NumOfChancesForUser - 1;
            return results;
        }

        private void ResetSet()
        {
            m_SetOfChoices = new HashSet<object>();
        }
    }
}
