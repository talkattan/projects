using System;
using GameLogic;
using System.Windows.Forms;
using System.Drawing;

namespace WindowUI
{
    internal class GameForm : Form
    {
        private GameLogic.GameLogic m_GameLogic;
        private int m_NumOfChancesCount;
        private const int k_NumOfColors = 8;
        private FirstRow m_FirstRow;
        public Row[] m_Rows;

        public GameForm(int i_NumOfChancesCount)
        {
            m_GameLogic = new GameLogic.GameLogic(i_NumOfChancesCount, k_NumOfColors);
            m_NumOfChancesCount = i_NumOfChancesCount;
            m_FirstRow = new FirstRow(m_GameLogic);
            m_FirstRow.AddButtonsToForm(this.Controls.Add);
            m_Rows = new Row[i_NumOfChancesCount];
            for (int i = 0; i < m_Rows.Length; i++)
            {
                m_Rows[i] = new Row(i, m_GameLogic);
                m_Rows[i].AddButtonsToForm(this.Controls.Add);
                m_Rows[i].AttachActivateNextRowMethod(this.ActivateNextRow);
                m_Rows[i].AttachReveal(this.RevealFirstRow);
            }
            this.Text = "Bool Pgia";
            this.Size = new System.Drawing.Size(
                300,
                100 + m_NumOfChancesCount * 45);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.play();
        }

        private void play()
        {
            m_Rows[0].EnablePickColorButtons();
        }

        internal void ActivateNextRow(int i_RowNum)
        {
            m_Rows[i_RowNum].EnablePickColorButtons();
        }

        internal void RevealFirstRow()
        {
            m_FirstRow.Reveal();
        }
    }
}