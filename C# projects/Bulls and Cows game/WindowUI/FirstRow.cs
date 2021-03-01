using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowUI
{
    internal class FirstRow
    {
        const int k_NumOfColors = 4;
        GameLogic.GameLogic m_GameLogic;
        Button[] m_HiddenButtons;

        internal FirstRow(GameLogic.GameLogic i_GameLogic)
        {
            m_GameLogic = i_GameLogic;
            m_HiddenButtons = new Button[k_NumOfColors];
            for (int i = 0; i < m_HiddenButtons.Length; i++)
            {
                m_HiddenButtons[i] = new Button();
                m_HiddenButtons[i].BackColor = Color.Black;
                m_HiddenButtons[i].Enabled = false;
                m_HiddenButtons[i].Height = 40;
                m_HiddenButtons[i].Width = 40;
                m_HiddenButtons[i].Location = new Point(
                    10 + i * (m_HiddenButtons[i].Width + 5),
                    10);
            }
        }

        internal void AddButtonsToForm(Action<Control> io_AddButtonToForm)
        {
            foreach (Button button in m_HiddenButtons)
            {
                io_AddButtonToForm(button);
            }
        }

        internal void Reveal()
        {
            int[] hiddenComputerColors = m_GameLogic.GetComputerColors;
            Color[] computerGuess = ColorForm.ConvertNumbersToColors(hiddenComputerColors, k_NumOfColors);
            for (int i = 0; i < computerGuess.Length; i++)
            {
                m_HiddenButtons[i].BackColor = computerGuess[i];
            }
        }
    }
}