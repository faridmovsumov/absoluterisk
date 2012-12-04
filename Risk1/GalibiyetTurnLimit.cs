using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Risk1
{
    public partial class GalibiyetTurnLimit : Form
    {
        Form1 f1;
        public GalibiyetTurnLimit(Form1 form1)
        {
            InitializeComponent();
            f1 = form1;
        }

        private void GalibiyetTurnLimit_Load(object sender, EventArgs e)
        {
            textBox1.TextAlign = HorizontalAlignment.Center;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            List<Player> pls = new List<Player>();
            pls = f1.players; 
            pls.Sort((x,y) => bolgeSayisi(x , y));
            
            int i = 1;
            foreach (Player p in pls.Reverse<Player>())
            {
                if (i == 1)
                {
                    textBox1.Text = p.name;
                    textBox1.ForeColor = p.color;
                }
                richTextBox1.Text += i.ToString()+" "+p.name + " " + p.getNumberOfTerritories()+" ("+p.getTotalNumberOfArmies()+")\n";
                i++;
            }

            String temp = "";
            foreach (String s in f1.statistics.siralama.Reverse<String>())
            {
                temp += i.ToString() + " " + s+ " (Died) " + "\n";
                i++;
            }

            richTextBox1.Text += temp;


        }

        //Birincisi boyukduse 1 ikincisi boyukduse -1
        private static int bolgeSayisi(Player p1, Player p2)
        {
            if (p1.getNumberOfTerritories() > p2.getNumberOfTerritories())
            {
                return 1;
            }
            else if (p1.getNumberOfTerritories() < p2.getNumberOfTerritories())
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
