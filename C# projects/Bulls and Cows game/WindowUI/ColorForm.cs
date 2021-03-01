using System.Windows.Forms;
using System.Drawing;
using System;
using GameLogic;

namespace WindowUI
{
    internal class ColorForm : Form
    {
        private Button[] m_ColorButtons;
        private Button m_SenderButton;
        private GameLogic.GameLogic m_GameLogic;

        internal ColorForm(Button i_SenderButton, GameLogic.GameLogic i_GameLogic)
        {
            m_SenderButton = i_SenderButton;
            m_GameLogic = i_GameLogic;

            this.Text = "Pick A Color:";
            this.Size = new System.Drawing.Size(210, 140);
            this.StartPosition = FormStartPosition.CenterScreen;

            m_ColorButtons = new Button[8];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_ColorButtons[4 * i + j] = new Button();
                    m_ColorButtons[4 * i + j].Width = 40;
                    m_ColorButtons[4 * i + j].Height = 40;
                    m_ColorButtons[4 * i + j].Location = new Point(
                        10 + j * (m_ColorButtons[0].Width + 5),
                        10 + i * (m_ColorButtons[0].Width + 5));
                    this.Controls.Add(m_ColorButtons[4 * i + j]);
                    m_ColorButtons[4 * i + j].Click += colorButton_Click;
                }
            }

            m_ColorButtons[0].BackColor = Color.HotPink;
            m_ColorButtons[1].BackColor = Color.Red;
            m_ColorButtons[2].BackColor = Color.Green;
            m_ColorButtons[3].BackColor = Color.Turquoise;
            m_ColorButtons[4].BackColor = Color.Blue;
            m_ColorButtons[5].BackColor = Color.Yellow;
            m_ColorButtons[6].BackColor = Color.Brown;
            m_ColorButtons[7].BackColor = Color.White;
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            if (m_SenderButton.BackColor != null)
            {
                if (m_GameLogic.CheckIfValidInput((sender as Button).BackColor))
                {
                    m_GameLogic.RemoveItemFromSet(m_SenderButton.BackColor);
                    m_SenderButton.BackColor = (sender as Button).BackColor;
                }
            }
            else if (m_GameLogic.CheckIfValidInput((sender as Button).BackColor))
            {
                m_SenderButton.BackColor = (sender as Button).BackColor;
            }
            this.Close();
        }

        public static int[] convertColorsToNumbers(Color[] i_Colors, int i_NumOfColors)
        {
            int[] res = new int[i_NumOfColors];
            for (int i = 0; i < i_NumOfColors; i++)
            {
                if (i_Colors[i] == Color.HotPink)
                {
                    res[i] = 0;
                }
                else if (i_Colors[i] == Color.Red)
                {
                    res[i] = 1;
                }
                else if (i_Colors[i] == Color.Green)
                {
                    res[i] = 2;
                }
                else if (i_Colors[i] == Color.Turquoise)
                {
                    res[i] = 3;
                }
                else if (i_Colors[i] == Color.Blue)
                {
                    res[i] = 4;
                }
                else if (i_Colors[i] == Color.Yellow)
                {
                    res[i] = 5;
                }
                else if (i_Colors[i] == Color.Brown)
                {
                    res[i] = 6;
                }
                else if (i_Colors[i] == Color.White)
                {
                    res[i] = 7;
                }
            }
            return res;
        }

        public static Color[] ConvertNumbersToColors(int[] i_Nums, int i_NumOfColors)
        {
            Color[] res = new Color[i_NumOfColors];
            for (int i = 0; i < i_NumOfColors; i++)
            {
                if (i_Nums[i] == 0)
                {
                    res[i] = Color.HotPink;
                }
                else if (i_Nums[i] == 1)
                {
                    res[i] = Color.Red;
                }
                else if (i_Nums[i] == 2)
                {
                    res[i] = Color.Green;
                }
                else if (i_Nums[i] == 3)
                {
                    res[i] = Color.Turquoise;
                }
                else if (i_Nums[i] == 4)
                {
                    res[i] = Color.Blue;
                }
                else if (i_Nums[i] == 5)
                {
                    res[i] = Color.Yellow;
                }
                else if (i_Nums[i] == 6)
                {
                    res[i] = Color.Brown;
                }
                else if (i_Nums[i] == 7)
                {
                    res[i] = Color.White;
                }
            }
            return res;
        }
    }
}