using System;
using System.Windows.Forms;
using System.Windows.Threading;

namespace GameBoard
{
    public partial class Form1 : Form
    {
        private GameRunner runner;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            runner = new GameRunner(this)
            {
                Dispatcher = Dispatcher.CurrentDispatcher,
                ShowFPS = true
            };
            runner.Run();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            runner.Stop();
        }
    }
}
