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
    public partial class SaldiriEkrani : Form
    {
        Bolge bolge1 = null;
        Bolge bolge2 = null;
        Form1 form1 = null;
        int randomAraligi = 20;

        public SaldiriEkrani(Bolge b1, Bolge b2, Form1 f1)
        {
            InitializeComponent();
            bolge1 = b1;
            bolge2 = b2;
            form1 = f1;
        }

        public void refreshSaldiriEkrani()
        {
            textBox2.Refresh();
            textBox3.Refresh();
        }

        private void SaldiriEkrani_Load(object sender, EventArgs e)
        {
            panel1.BackColor = bolge1.sahip.color;
            panel2.BackColor = bolge2.sahip.color;
            textBox4.Text = bolge1.isim;
            textBox1.Text = bolge2.isim;
            textBox2.Text = bolge1.ordu.askerSayisi.ToString();
            textBox3.Text = bolge2.ordu.askerSayisi.ToString();
            this.ActiveControl = button1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            warAttack();
        }

        public void warDoORDieForAi()
        {
            string temp = "";
            temp += bolge1.isim + ":" + bolge1.ordu.askerSayisi.ToString()+"\n";
            temp += bolge2.isim + ":" + bolge2.ordu.askerSayisi.ToString() + "\n---------\n";
            while (bolge1.ordu.askerSayisi > 1 && bolge2.ordu.askerSayisi > 0)
            {
                int bb1;
                int bb2;
                bb1 = random.Next(1, randomAraligi + (bolge1.ordu.askerSayisi / 5));
                System.Threading.Thread.Sleep(20);
                for (int i = 1; i <= 11; i++) { random.Next(); }
                bb2 = random.Next(1, randomAraligi + (bolge2.ordu.askerSayisi / 5));


                temp += bb1.ToString() + " - " + bb2.ToString() + "\n";
                if (bb1 > bb2)
                {
                    bolge2.ordu.askerSayisi--;
                    form1.changeLabel(bolge2);
                    textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                    if (bolge2.ordu.askerSayisi == 0)
                    {
                        bolge2.sahip.bolgeler.Remove(bolge2);
                        bolge1.sahip.bolgeler.Add(bolge2);
                        bolge2.sahip = bolge1.sahip;
                        bolge1.ordu.askerSayisi--;
                        bolge2.ordu.askerSayisi++;
                        List<int> bolmeTalimati = new List<int>();

                        bolmeTalimati = form1.yapayzekalar[form1.players[form1.turn].aiId - 1].divideArmies(bolge1.index, bolge2.index,form1.getGameData());
                        
                        if ((bolge1.ordu.askerSayisi + bolge2.ordu.askerSayisi) == (bolmeTalimati[0] + bolmeTalimati[1]))
                        {
                            bolge1.ordu.askerSayisi = bolmeTalimati[0];
                            bolge2.ordu.askerSayisi = bolmeTalimati[1];
                        }

                        form1.changeLabel(bolge1);
                        form1.changeLabel(bolge2);
                        form1.changeTerritoryColor(bolge2, bolge2.sahip);
                        break;
                    }

                }
                if (bb1 < bb2)//Savunma yapan eşitlik durumunda daha üstün
                {
                    bolge1.ordu.askerSayisi--;
                    form1.changeLabel(bolge1);
                    textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                }
                refreshSaldiriEkrani();
                form1.refreshGame();  
            }
            //MessageBox.Show(temp);
        }
        Random random = new Random();
        
        public void warDoORDie()
        {
            string temp = "";
            temp += bolge1.isim + ":" + bolge1.ordu.askerSayisi.ToString() + "\n";
            temp += bolge2.isim + ":" + bolge2.ordu.askerSayisi.ToString() + "\n---------\n";

            while (bolge1.ordu.askerSayisi > 1 && bolge2.ordu.askerSayisi > 0)
            {
                int bb1;
                int bb2;
                bb1 = random.Next(1, randomAraligi + (bolge1.ordu.askerSayisi / 5));
                System.Threading.Thread.Sleep(20);
                for (int i = 1; i <= 11; i++) { random.Next();}
                bb2 = random.Next(1, randomAraligi + (bolge2.ordu.askerSayisi / 5));


                temp += bb1.ToString() + " - " + bb2.ToString() + "\n";
                if (bb1 > bb2)
                {
                    bolge2.ordu.askerSayisi--;
                    form1.changeLabel(bolge2);
                    textBox3.Text=bolge2.ordu.askerSayisi.ToString();
                    if (bolge2.ordu.askerSayisi == 0)
                    {
                        bolge2.sahip.bolgeler.Remove(bolge2);
                        bolge1.sahip.bolgeler.Add(bolge2);
                        bolge2.sahip = bolge1.sahip;
                        bolge1.ordu.askerSayisi--;
                        bolge2.ordu.askerSayisi++;
                        this.Visible = false;


                        int b1 = bolge1.ordu.askerSayisi;
                        int b2 = bolge2.ordu.askerSayisi;
                        ArmyMove am = new ArmyMove(bolge1, bolge2, form1);
                        am.ShowDialog();

                        if (form1.isArmyMoved)
                        {
                            form1.changeLabel(bolge1);
                            form1.changeLabel(bolge2);
                            form1.isArmyMoved = false;
                        }
                        else
                        {
                            bolge1.ordu.askerSayisi = b1;
                            bolge2.ordu.askerSayisi = b2;
                            form1.changeLabel(bolge1);
                            form1.changeLabel(bolge2);
                        }

                        form1.changeTerritoryColor(bolge2, bolge2.sahip);
                        break;
                    }
                    
                }
                if (bb1 < bb2)
                {
                    bolge1.ordu.askerSayisi--;
                    form1.changeLabel(bolge1);
                    textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                }
                refreshSaldiriEkrani();
                form1.refreshGame();   
            }
            //MessageBox.Show(temp);
        }


        public void warAttackForAi()
        {
            string temp = "";
            temp += bolge1.isim + ":" + bolge1.ordu.askerSayisi.ToString() + "\n";
            temp += bolge2.isim + ":" + bolge2.ordu.askerSayisi.ToString() + "\n---------\n";
            if (bolge1.ordu.askerSayisi > 1 && bolge2.ordu.askerSayisi > 0)
            {
                int bb1;
                int bb2;
                bb1 = random.Next(1, randomAraligi + (bolge1.ordu.askerSayisi / 5));
                System.Threading.Thread.Sleep(20);
                for (int i = 1; i <= 11; i++) { random.Next(); }
                bb2 = random.Next(1, randomAraligi + (bolge2.ordu.askerSayisi / 5));


                temp += bb1.ToString() + " - " + bb2.ToString() + "\n";
                if (bb1 > bb2)
                {
                    bolge2.ordu.askerSayisi--;
                    form1.changeLabel(bolge2);
                    textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                    if (bolge2.ordu.askerSayisi == 0)
                    {
                        bolge2.sahip.bolgeler.Remove(bolge2);
                        bolge1.sahip.bolgeler.Add(bolge2);
                        bolge2.sahip = bolge1.sahip;
                        bolge1.ordu.askerSayisi--;
                        bolge2.ordu.askerSayisi++;
                        List<int> bolmeTalimati = new List<int>();

                        bolmeTalimati = form1.yapayzekalar[form1.players[form1.turn].aiId - 1].divideArmies(bolge1.index, bolge2.index, form1.getGameData());

                        if ((bolge1.ordu.askerSayisi + bolge2.ordu.askerSayisi) == (bolmeTalimati[0] + bolmeTalimati[1]))
                        {
                            bolge1.ordu.askerSayisi = bolmeTalimati[0];
                            bolge2.ordu.askerSayisi = bolmeTalimati[1];
                        }

                        form1.changeLabel(bolge1);
                        form1.changeLabel(bolge2);
                        form1.changeTerritoryColor(bolge2, bolge2.sahip);
                    }

                }
                if (bb1 < bb2)//Savunma yapan eşitlik durumunda daha üstün
                {
                    bolge1.ordu.askerSayisi--;
                    form1.changeLabel(bolge1);
                    textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                }
                refreshSaldiriEkrani();
                form1.refreshGame();
            }
        }


        public void warAttack()
        {
            if (bolge1.ordu.askerSayisi > 1 && bolge2.ordu.askerSayisi > 0)
            {
                Random random = new Random();
                int bb1;
                int bb2;

                bb1 = random.Next(1, randomAraligi + (bolge1.ordu.askerSayisi / 5));
                bb2 = random.Next(1, randomAraligi + (bolge2.ordu.askerSayisi / 5));
               
                if (bb1 > bb2)
                {
                    bolge2.ordu.askerSayisi--;
                    form1.changeLabel(bolge2);
                    textBox3.Text = bolge2.ordu.askerSayisi.ToString();
                    if (bolge2.ordu.askerSayisi == 0)
                    {
                        bolge2.sahip.bolgeler.Remove(bolge2);
                        bolge1.sahip.bolgeler.Add(bolge2);
                        bolge2.sahip = bolge1.sahip;
                        bolge1.ordu.askerSayisi--;
                        bolge2.ordu.askerSayisi++;
                        this.Visible = false;

                        int b1 = bolge1.ordu.askerSayisi;
                        int b2 = bolge2.ordu.askerSayisi;
                        ArmyMove am = new ArmyMove(bolge1, bolge2, form1);
                        am.ShowDialog();

                        if (form1.isArmyMoved)
                        {
                            form1.changeLabel(bolge1);
                            form1.changeLabel(bolge2);
                            form1.isArmyMoved = false;
                        }
                        else
                        {
                            form1.changeLabel(bolge1);
                            form1.changeLabel(bolge2);
                            bolge1.ordu.askerSayisi = b1;
                            bolge2.ordu.askerSayisi = b2;
                        }

                        form1.changeLabel(bolge1);
                        form1.changeLabel(bolge2);
                        form1.changeTerritoryColor(bolge2, bolge2.sahip);
                    }
                }
                else if (bb1 < bb2)
                {
                    bolge1.ordu.askerSayisi--;
                    form1.changeLabel(bolge1);
                    textBox2.Text = bolge1.ordu.askerSayisi.ToString();
                }
                else
                {
                    //Devam..   
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            warDoORDie();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
