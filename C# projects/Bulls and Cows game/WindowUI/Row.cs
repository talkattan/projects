using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowUI
{
    internal class Row
    {
        const int k_NumOfColors = 4;
        Button[] m_GameButtons;
        Button m_SubmitGuess;
        Button[] m_ResultsOfGuess;
        GameLogic.GameLogic m_GameLogic;
        Action<int> m_EnableNextRow;
        Action m_RevealFirstRow;

        internal Row(int i_NumOfRow, GameLogic.GameLogic i_GameLogic)
        {
            m_GameLogic = i_GameLogic;
            m_GameButtons = new Button[k_NumOfColors];
            for (int i = 0; i < m_GameButtons.Length; i++)
            {
                m_GameButtons[i] = new Button();
                m_GameButtons[i].Enabled = false;
                m_GameButtons[i].Height = 40;
                m_GameButtons[i].Width = 40;
                m_GameButtons[i].Location = new System.Drawing.Point(
                    10 + i * (m_GameButtons[i].Width + 5),
                    60 + i_NumOfRow * (m_GameButtons[i].Height + 5));
                m_GameButtons[i].Click += new System.EventHandler(colorButton_Click);

            }

            m_SubmitGuess = new Button();
            m_SubmitGuess.Text = "-->>";
            m_SubmitGuess.Enabled = false;
            m_SubmitGuess.Height = m_GameButtons[0].Height / 2;
            m_SubmitGuess.Width = m_GameButtons[0].Width;
            m_SubmitGuess.Location = new System.Drawing.Point(
                m_GameButtons[m_GameButtons.Length - 1].Location.X + m_GameButtons[m_GameButtons.Length - 1].Width + 4,
                m_GameButtons[m_GameButtons.Length - 1].Location.Y + m_GameButtons[m_GameButtons.Length - 1].Height / 4);
            m_SubmitGuess.Click += submitButton_Click;

            m_ResultsOfGuess = new Button[k_NumOfColors];
            for(int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m_ResultsOfGuess[2 * i + j] = new Button();
                    m_ResultsOfGuess[2 * i + j].Enabled = false;
                    m_ResultsOfGuess[2 * i + j].Width = 18;
                    m_ResultsOfGuess[2 * i + j].Height = 18;
                    m_ResultsOfGuess[2 * i + j].Location = new System.Drawing.Point(
                        m_SubmitGuess.Location.X + m_SubmitGuess.Width + 10 + j * (m_ResultsOfGuess[2 * i + j].Width + 4),
                        m_GameButtons[0].Location.Y  + i * (m_ResultsOfGuess[2 * i + j].Height + 4));

                }
            }
        }

        internal void AttachReveal(Action i_RevealFirstRow)
        {
            m_RevealFirstRow += i_RevealFirstRow;
        }

        internal void AttachActivateNextRowMethod(Action<int> i_ActivateNextRow)
        {
            m_EnableNextRow += i_ActivateNextRow;
        }

        internal void DisablePickColorButtons()
        {
            foreach (Button button in m_GameButtons)
            {
                button.Enabled = false;
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            Color[] colors = getUserGuess();
            int[] colorsInNumbers = ColorForm.convertColorsToNumbers(colors, k_NumOfColors);
            int[] resultsOfGuess = m_GameLogic.CalcResults(colorsInNumbers);

            int k = 0;
            for (int i = 0; i < resultsOfGuess[0]; i++)
            {
                m_ResultsOfGuess[k].BackColor = Color.Black;
                k++;
            }
            for (int i = 0; i < resultsOfGuess[1]; i++)
            {
                m_ResultsOfGuess[k].BackColor = Color.Yellow;
                k++;
            }
            m_GameLogic.CurrentGuess++;

            if (m_GameLogic.IsFinished)
            {
                revealFirstRow();
            }
            else
            {
                m_EnableNextRow.Invoke(m_GameLogic.CurrentGuess);
            }
            DisablePickColorButtons();
            m_SubmitGuess.Enabled = false;
        }

        private void revealFirstRow()
        {
            m_RevealFirstRow.Invoke();
        }

        private Color[] getUserGuess()
        {
            Color[] colors = new Color[k_NumOfColors];
            for (int i = 0; i < k_NumOfColors; i++)
            {
                colors[i] = m_GameButtons[i].BackColor;
            }
            return colors;
        }

        internal void EnablePickColorButtons()
        {
            foreach (Button button in m_GameButtons)
            {
                button.Enabled = true;
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            new ColorForm(sender as Button, m_GameLogic).ShowDialog();
            if (m_GameLogic.CheckIfFullGuess())
            {
                m_SubmitGuess.Enabled = true;
            }
        }

        internal void AddButtonsToForm(Action<Control> i_AddButtonToForm)
        {
            foreach (Button button in m_GameButtons)
            {
                i_AddButtonToForm(button);
            }
            i_AddButtonToForm(m_SubmitGuess);
            foreach (Button button in m_ResultsOfGuess)
            {
                i_AddButtonToForm(button);
            }
        }
    }
}