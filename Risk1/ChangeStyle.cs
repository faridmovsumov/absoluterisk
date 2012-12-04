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
    public partial class ChangeStyle : Form
    {
        Form1 f1;
        Color c;
        public ChangeStyle(Form1 form1)
        {
            InitializeComponent();
            f1 = form1;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button1.BackColor = colorDialog1.Color;
            //f1.changeDenizColor(colorDialog1.Color);
            c = colorDialog1.Color;
        }

        private void ChangeStyle_Load(object sender, EventArgs e)
        {
            colorDialog1.Color = f1.deniz[0].renk;
            button1.BackColor = f1.deniz[0].renk;
            c = colorDialog1.Color;
            comboBox1.SelectedIndex = f1.pictureIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)//Babek
            {
                f1.pictureIndex = 0;
                f1.changePicture();
            }
            if (comboBox1.SelectedIndex == 1)//Erkek
            {
                f1.pictureIndex = 1;
                f1.changePicture();
            }
            if (comboBox1.SelectedIndex == 2)//kız
            {
                f1.pictureIndex = 2;
                f1.changePicture();
            }



            f1.changeDenizColor(c);
            this.Close();
        }
    }
}
