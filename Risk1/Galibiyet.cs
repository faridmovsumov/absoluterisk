using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace Risk1
{
    public partial class Galibiyet : Form
    {
        String winner;
        Form1 f1;
        public Galibiyet(Form1 form1)
        {
            InitializeComponent();
            winner = form1.players[0].name;
            f1 = form1;
        }

        private void Galibiyet_Load(object sender, EventArgs e)
        {
            label1.Text = winner + "\nConquered The World!";
            label1.ForeColor = f1.players[0].color;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            String temp = "";
            int a = 1;
            foreach (String s in f1.statistics.siralama.Reverse<String>())
            {
                temp += a.ToString()+" "+s + "\n";
                a++;
            }

            richTextBox1.Text = temp;
            //SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.korsanlar);
            //simpleSound.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
