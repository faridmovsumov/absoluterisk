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
    public partial class Rules : Form
    {
        Form1 f1;
        int temp;
        public Rules(Form1 form1)
        {
            InitializeComponent();
            f1=form1;
            temp = f1.ilkAskerSayisi;
        }

        private void Rules_Load(object sender, EventArgs e)
        {
            int TL = f1.turnLimit;
            int s = f1.ganimetKazanci;
            textBox1.Text = temp.ToString();
            checkBox1.Checked = f1.isRandom;
            textBox2.Text = s.ToString();
            textBox3.Text = TL.ToString();
            checkBox2.Checked = f1.isTurnLimit;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(textBox1.Text) > 1 && Convert.ToInt32(textBox2.Text) > 0)
                {
                    if (checkBox2.Checked)
                    {
                        f1.isTurnLimit = true;
                        f1.turnLimit = Convert.ToInt32(textBox3.Text);
                    }
                    else
                    {
                        f1.isTurnLimit = false;
                    }

                    f1.ilkAskerSayisi = Convert.ToInt32(textBox1.Text);
                    f1.ganimetKazanci = Convert.ToInt32(textBox2.Text);
                    if (checkBox1.Checked)
                    {
                        f1.isRandom = true;
                    }
                    else
                    {
                        f1.isRandom = false;
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gold Terrritory must be greater than zero\nNumber of initial armies must be greater than one");
                }
            }
            catch
            {
                MessageBox.Show("Invalid Value!");
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
