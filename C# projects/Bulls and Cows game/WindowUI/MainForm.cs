using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowUI
{
    public partial class MainForm : Form
    {
        Button m_ButtonStart;
        Button m_ButtonNumOfChances;
        int m_NumOfChancesCount;
        
        public MainForm()
        {
            m_NumOfChancesCount = 4;
            this.Text = "Bool Pgia";
            this.Size = new System.Drawing.Size(300, 130);
            this.StartPosition = FormStartPosition.CenterScreen;

            m_ButtonNumOfChances = new Button();
            m_ButtonNumOfChances.Text = string.Format(@"Number of chances: {0}", m_NumOfChancesCount);
            const int k_Offset = 50;
            m_ButtonNumOfChances.Width = this.Size.Width - k_Offset;
            m_ButtonNumOfChances.Location = new System.Drawing.Point(k_Offset / 2, 5);
            this.Controls.Add(m_ButtonNumOfChances);
            m_ButtonNumOfChances.Click += new System.EventHandler(countingButton_Click);

            m_ButtonStart = new Button();
            m_ButtonStart.Text = "Start";
            m_ButtonStart.Width = 100;
            m_ButtonStart.Location = new System.Drawing.Point(m_ButtonNumOfChances.Location.X + m_ButtonNumOfChances.Width - 100,
                m_ButtonNumOfChances.Location.Y + 2 * m_ButtonNumOfChances.Height);
            this.Controls.Add(m_ButtonStart);
            m_ButtonStart.Click += new System.EventHandler(startButton_Click);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new GameForm(m_NumOfChancesCount).ShowDialog();
            this.Close();
        }

        private void countingButton_Click(object sender, EventArgs e)
        {
            if (m_NumOfChancesCount == 10)
            {
                m_NumOfChancesCount = 4;
            }
            else
            {
                m_NumOfChancesCount++;
            }
            m_ButtonNumOfChances.Text = string.Format(@"Number of chances: {0}", m_NumOfChancesCount);
        }
    }
}
