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
    public partial class Giris : Form
    {
        List<Player> players;
        List<Player> playersTemp = new List<Player>();
        Form1 f1;
        public Giris(List<Player> ps, Form1 form1)
        {
            InitializeComponent();
            players = ps;
            playersTemp = ps;
            f1 = form1;
        }
        
        private void Giris_Load(object sender, EventArgs e)
        {
            //10 tane player oluşturup players listesine ekledim

            foreach (Player p in players)
            {
                if (!p.isHuman)//insan değilse ismini değiş
                {
                    p.name=f1.yapayzekalar[p.aiId - 1].getName();
                }
            }


            checkBox1.Checked = players[0].isActive;
            checkBox2.Checked = players[1].isActive;
            checkBox3.Checked = players[2].isActive;
            checkBox4.Checked = players[3].isActive;
            checkBox5.Checked = players[4].isActive;
            checkBox6.Checked = players[5].isActive;
            checkBox7.Checked = players[6].isActive;
            checkBox8.Checked = players[7].isActive;
            checkBox9.Checked = players[8].isActive;
            checkBox10.Checked = players[9].isActive;

            checkBox1.Text = players[0].name;
            checkBox2.Text = players[1].name;
            checkBox3.Text = players[2].name;
            checkBox4.Text = players[3].name;
            checkBox5.Text = players[4].name;
            checkBox6.Text = players[5].name;
            checkBox7.Text = players[6].name;
            checkBox8.Text = players[7].name;
            checkBox9.Text = players[8].name;
            checkBox10.Text = players[9].name;

            checkBox1.BackColor = players[0].color;
            checkBox2.BackColor = players[1].color;
            checkBox3.BackColor = players[2].color;
            checkBox4.BackColor = players[3].color;
            checkBox5.BackColor = players[4].color;
            checkBox6.BackColor = players[5].color;
            checkBox7.BackColor = players[6].color;
            checkBox8.BackColor = players[7].color;
            checkBox9.BackColor = players[8].color;
            checkBox10.BackColor = players[9].color;

            if (players[0].isHuman == true)
            {
                textBox1.Text = "HUMAN";
            }
            else
            {
                textBox1.Text = "COMPUTER";
            }

            if (players[1].isHuman == true)
            {
                textBox2.Text = "HUMAN";
            }
            else
            {
                textBox2.Text = "COMPUTER";
            }

            if (players[2].isHuman == true)
            {
                textBox3.Text = "HUMAN";
            }
            else
            {
                textBox3.Text = "COMPUTER";
            }

            if (players[3].isHuman == true)
            {
                textBox4.Text = "HUMAN";
            }
            else
            {
                textBox4.Text = "COMPUTER";
            }

            if (players[4].isHuman == true)
            {
                textBox5.Text = "HUMAN";
            }
            else
            {
                textBox5.Text = "COMPUTER";
            }

            if (players[5].isHuman == true)
            {
                textBox6.Text = "HUMAN";
            }
            else
            {
                textBox6.Text = "COMPUTER";
            }

            if (players[6].isHuman == true)
            {
                textBox7.Text = "HUMAN";
            }
            else
            {
                textBox7.Text = "COMPUTER";
            }
            if (players[7].isHuman == true)
            {
                textBox8.Text = "HUMAN";
            }
            else
            {
                textBox8.Text = "COMPUTER";
            }

            if (players[8].isHuman == true)
            {
                textBox9.Text = "HUMAN";
            }
            else
            {
                textBox9.Text = "COMPUTER";
            }

            if (players[9].isHuman == true)
            {
                textBox10.Text = "HUMAN";
            }
            else
            {
                textBox10.Text = "COMPUTER";
            }
            ////////////////////////////////
            if (checkBox1.Checked)
            {
                button1.Enabled = true;
            }
            if (checkBox2.Checked)
            {
                button2.Enabled = true;
            }
            if (checkBox3.Checked)
            {
                button3.Enabled = true;
            }
            if (checkBox4.Checked)
            {
                button4.Enabled = true;
            }

            if (checkBox5.Checked)
            {
                button5.Enabled = true;
            }
            if (checkBox6.Checked)
            {
                button6.Enabled = true;
            }
            if (checkBox7.Checked)
            {
                button7.Enabled = true;
            }
            if (checkBox8.Checked)
            {
                button8.Enabled = true;
            }
            if (checkBox9.Checked)
            {
                button9.Enabled = true;
            }
            if (checkBox10.Checked)
            {
                button10.Enabled = true;
            }



        }
        

        private void button11_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (Player p in players)
            {
                if (p.isActive)
                {
                    count++;
                }
            }

            if (count > 1)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Number of Players!\nYou must select at least 2 players");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[0]);
            cp.ShowDialog();
            checkBox1.Text = players[0].name;
            checkBox1.BackColor = players[0].color;
            if (players[0].isHuman)
            {
                textBox1.Text = "HUMAN";
            }
            else
            {
                textBox1.Text = "COMPUTER";
            }

            if (checkBox1.Checked)
            {
                players[0].isActive = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[1]);
            cp.ShowDialog();
            checkBox2.Text = players[1].name;
            checkBox2.BackColor = players[1].color;
            if (players[1].isHuman)
            {
                textBox2.Text = "HUMAN";
            }
            else
            {
                textBox2.Text = "COMPUTER";
            }

            if (checkBox2.Checked)
            {
                players[1].isActive = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[2]);
            cp.ShowDialog();
            checkBox3.Text = players[2].name;
            checkBox3.BackColor = players[2].color;
            if (players[2].isHuman)
            {
                textBox3.Text = "HUMAN";
            }
            else
            {
                textBox3.Text = "COMPUTER";
            }

            if (checkBox3.Checked)
            {
                players[2].isActive = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[3]);
            cp.ShowDialog();
            checkBox4.Text = players[3].name;
            checkBox4.BackColor = players[3].color;
            if (players[3].isHuman)
            {
                textBox4.Text = "HUMAN";
            }
            else
            {
                textBox4.Text = "COMPUTER";
            }

            if (checkBox4.Checked)
            {
                players[3].isActive = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                players[2].isActive = true;
                button3.Enabled = true;
            }
            else
            {
                players[2].isActive = false;
                button3.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                players[0].isActive = true;
                button1.Enabled = true;

            }
            else
            {
                players[0].isActive = false;
                button1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                players[1].isActive = true;
                button2.Enabled = true;
            }
            else
            {
                players[1].isActive = false;
                button2.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                players[3].isActive = true;
                button4.Enabled = true;
            }
            else
            {
                players[3].isActive = false;
                button4.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                players[4].isActive = true;
                button5.Enabled = true;
            }
            else
            {
                players[4].isActive = false;
                button5.Enabled = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                players[5].isActive = true;
                button6.Enabled = true;
            }
            else
            {
                players[5].isActive = false;
                button6.Enabled = false;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                players[6].isActive = true;
                button7.Enabled = true;
            }
            else
            {
                players[6].isActive = false;
                button7.Enabled = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                players[7].isActive = true;
                button8.Enabled = true;
            }
            else
            {
                players[7].isActive = false;
                button8.Enabled = false;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                players[8].isActive = true;
                button9.Enabled = true;
            }
            else
            {
                players[8].isActive = false;
                button9.Enabled = false;
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                players[9].isActive = true;
                button10.Enabled = true;
            }
            else
            {
                players[9].isActive = false;
                button10.Enabled = false;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[9]);
            cp.ShowDialog();
            checkBox10.Text = players[9].name;
            checkBox10.BackColor = players[9].color;
            if (players[9].isHuman)
            {
                textBox10.Text = "HUMAN";
            }
            else
            {
                textBox10.Text = "COMPUTER";
            }

            if (checkBox10.Checked)
            {
                players[9].isActive = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[4]);
            cp.ShowDialog();
            checkBox5.Text = players[4].name;
            checkBox5.BackColor = players[4].color;
            if (players[4].isHuman)
            {
                textBox5.Text = "HUMAN";
            }
            else
            {
                textBox5.Text = "COMPUTER";
            }

            if (checkBox5.Checked)
            {
                players[4].isActive = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            players = playersTemp;
            f1.yeniOyunIptal = true;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[5]);
            cp.ShowDialog();
            checkBox6.Text = players[5].name;
            checkBox6.BackColor = players[5].color;
            if (players[5].isHuman)
            {
                textBox6.Text = "HUMAN";
            }
            else
            {
                textBox6.Text = "COMPUTER";
            }

            if (checkBox6.Checked)
            {
                players[5].isActive = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[6]);
            cp.ShowDialog();
            checkBox7.Text = players[6].name;
            checkBox7.BackColor = players[6].color;
            if (players[6].isHuman)
            {
                textBox7.Text = "HUMAN";
            }
            else
            {
                textBox7.Text = "COMPUTER";
            }

            if (checkBox7.Checked)
            {
                players[6].isActive = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[7]);
            cp.ShowDialog();
            checkBox8.Text = players[7].name;
            checkBox8.BackColor = players[7].color;
            if (players[7].isHuman)
            {
                textBox8.Text = "HUMAN";
            }
            else
            {
                textBox8.Text = "COMPUTER";
            }

            if (checkBox8.Checked)
            {
                players[7].isActive = true;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ChangePlayer cp = new ChangePlayer(players[8]);
            cp.ShowDialog();
            checkBox9.Text = players[8].name;
            checkBox9.BackColor = players[8].color;
            if (players[8].isHuman)
            {
                textBox9.Text = "HUMAN";
            }
            else
            {
                textBox9.Text = "COMPUTER";
            }

            if (checkBox9.Checked)
            {
                players[8].isActive = true;
            }
        }
    }
}
