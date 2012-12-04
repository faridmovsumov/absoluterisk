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
    public partial class ChangePlayer : Form
    {
        Player player;
        
        public ChangePlayer(Player p)
        {
            InitializeComponent();
            player = p;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button1.BackColor = colorDialog1.Color;
        }

        private void ChangePlayer_Load(object sender, EventArgs e)
        {
            textBox1.Text = player.name;
            button1.BackColor = player.color;

            if (player.isHuman)
            {
                comboBox2.SelectedIndex = 0;
                comboBox1.SelectedIndex = 1;
                comboBox2.Enabled = false;
                this.Width = 225;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.Enabled = true;
                comboBox2Ayarla();
                this.Width = 348;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Oyucunun Rengini Belirleriz
            if (button1.BackColor == Color.Black)
            {
                player.color = Color.Gray;
            }
            else
            {
                player.color = button1.BackColor;
            }

            //Oyuncu insan mı yoksa yapay zeka mı onu belirleriz
            if (comboBox1.SelectedIndex == 0)
            {
                player.isHuman = false;
                HangiYapayZekaSecili();
            }
            else
            {
                player.isHuman = true;
            }

            
            
            player.name = textBox1.Text;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)//Human
            {
                comboBox2.Enabled = false;
                this.Width = 225;
                player.isHuman = true;
                //player.name = "Player"+player.number.ToString();
            }
            else if (comboBox1.SelectedIndex == 0)//Computer
            {
                comboBox2.Enabled = true;
                this.Width = 348;
                player.isHuman = false;
                HangiYapayZekaSecili();
            }
            textBox1.Text = player.name;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!player.isHuman)
            {
                HangiYapayZekaSecili();
                textBox1.Text = player.name;
            }
        }

        public void HangiYapayZekaSecili()
        {
            if (comboBox2.SelectedIndex == 0)
            {
                player.aiId = AI.aiid;
                player.name = AI.name;
                pictureBox1.Image = AI.resim;

            }
            else if (comboBox2.SelectedIndex == 1)
            {
                player.aiId = AI2.aiId;
                player.name = AI2.name;
                pictureBox1.Image = AI2.resim;
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                player.aiId = AI3.aiid;
                player.name = AI3.name;
                pictureBox1.Image = AI3.resim;
            }
            else if (comboBox2.SelectedIndex == 3)
            {
                player.aiId = AI4.aiid;
                player.name = AI4.name;
                pictureBox1.Image = AI4.resim;
            }
        }

        public void comboBox2Ayarla()
        {
            if (player.aiId == 1)
            {
                pictureBox1.Image = Properties.Resources.cengizhan;
                comboBox2.SelectedIndex = 0;
            }
            if (player.aiId == 2)
            {
                pictureBox1.Image = Properties.Resources.hitler;
                comboBox2.SelectedIndex = 1;
            }
            if (player.aiId == 3)
            {
                pictureBox1.Image = Properties.Resources.babek;
                comboBox2.SelectedIndex = 2;
            }
            if (player.aiId == 4)
            {
                pictureBox1.Image = Properties.Resources.fatih;
                comboBox2.SelectedIndex = 3;
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            
        }

    }
}
