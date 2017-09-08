using System;
using System.Windows.Forms;

namespace AudioFade
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private AudioFadeLibrary.Audio AFL;

        private void Form1_Load(object sender, EventArgs e)
        {
            AFL = new AudioFadeLibrary.Audio(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AFL.Volume = 0.5f;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AFL.Volume = 0.0f;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AFL.FadeOut(2000, AFL.Volume, 0.0f);
            AFL.Muted = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(AFL.Volume.ToString());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AFL.Volume = 0;
            AFL.Muted = false;
            AFL.FadeOut(2000, 0f, 0.90f);
        }
    }
}