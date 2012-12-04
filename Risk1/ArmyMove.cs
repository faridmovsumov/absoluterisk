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
    public partial class ArmyMove : Form
    {
        Bolge bolge11 = null;
        Bolge bolge22 = null;
        Bolge bolge1 = new Bolge();
        Bolge bolge2 = new Bolge();
        Form1 form1 = null;
        public ArmyMove(Bolge b1,Bolge b2,Form1 f1)
        {
            InitializeComponent();
            bolge11 = b1;
            bolge22 = b2;
            bolge1 = b1;
            bolge2 = b2;
            form1 = f1;
        }

        private void ArmyMove_Load(object sender, EventArgs e)
        {
            panel1.BackColor = bolge1.sahip.color;
            panel2.BackColor = bolge2.sahip.color;
            textBox4.Text = bolge1.isim;
            textBox1.Text = bolge2.isim;
            textBox2.Text = bolge1.ordu.askerSayisi.ToString();
            textBox3.Text = bolge2.ordu.askerSayisi.ToString();
            this.ActiveControl = button4;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bolge1Azalt(1);
        }
        public void bolge1Azalt(int number)
        {
            if ((bolge1.ordu.askerSayisi - number) > 0)
            {
                bolge2.ordu.askerSayisi += number;
                bolge1.ordu.askerSayisi -= number;
                textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                form1.isArmyMoved = false;
            }
        }
        public void bolge2Azalt(int number)
        {
            if ((bolge2.ordu.askerSayisi - number) > 0)
            {
                bolge1.ordu.askerSayisi += number;
                bolge2.ordu.askerSayisi -= number;
                textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                form1.isArmyMoved = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bolge2Azalt(1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bolge2Azalt(10);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            bolge2Azalt(25);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bolge1Azalt(10);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bolge1Azalt(25);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //bolge11 = bolge1;
            //bolge22 = bolge2;
            form1.isArmyMoved = true;
            Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bolge2.ordu.askerSayisi += (bolge1.ordu.askerSayisi - 1);
            bolge1.ordu.askerSayisi = 1;
            
            textBox2.Text = bolge1.ordu.askerSayisi.ToString();
            textBox3.Text = bolge2.ordu.askerSayisi.ToString();
            
            form1.isArmyMoved = true;
            //bolge11 = bolge1;
            //bolge22 = bolge2;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int toplam = bolge1.ordu.askerSayisi + bolge2.ordu.askerSayisi;

            if ((toplam % 2) == 0)
            {
                bolge1.ordu.askerSayisi = toplam / 2;
                bolge2.ordu.askerSayisi = toplam / 2;
                textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                
            }
            else
            {
                bolge1.ordu.askerSayisi=toplam/2;
                bolge2.ordu.askerSayisi=(toplam/2)+1;
                textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                textBox3.Text = bolge2.ordu.askerSayisi.ToString();
            }
            //bolge11 = bolge1;
            //bolge22 = bolge2;
            form1.isArmyMoved = true;
            this.Close();
        }
    }
}
