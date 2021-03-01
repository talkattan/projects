using System;
using System.Windows.Forms;
using WindowUI;

namespace Program
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            new MainForm().ShowDialog();
        }
    }
}
