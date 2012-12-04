using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace Risk1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
        }

        public int turnCounter = 0;
        List<Nokta> noktalar = new List<Nokta>();
        public List<Bolge> bolgeler = new List<Bolge>();
        public List<Player> players = new List<Player>();
        public List<Continent> kitalar = new List<Continent>();
        Bolge ilkTiklananBolge = null;
        public int turn = 0;
        public int ilkAskerler = 0;
        public int ilkAskerSayisi = 15;
        public bool isArmyMoved = false;
        public bool isRandom = true;
        int ganimetBolgesi;
        public int ganimetKazanci = 1;
        DefaultAi defaultAi = new DefaultAi();
        public AI ai=new AI();
        public AI2 ai2 = new AI2();
        public AI3 ai3 = new AI3();
        public AI4 ai4 = new AI4();
        List<Label> labels = new List<Label>();
        public List<ArtificalIntelligence> yapayzekalar =new  List<ArtificalIntelligence>();
        public List<Nokta> deniz = new List<Nokta>();
        public List<Nokta> sinirlar = new List<Nokta>();
        public List<List<int>> history = new List<List<int>>();
        public GameData gameData = new GameData();
        public Statistics statistics = new Statistics();
        public int turnLimit = 250;
        public bool isTurnLimit = false;
        public int pictureIndex = 1;

        public void incrementTurnCounter()
        {
            turnCounter++;
            label43.Text = "TURN: " + turnCounter.ToString();
            if (isTurnLimit)
            {
                if (turnCounter == turnLimit)
                {
                    changeStatistics();
                    GalibiyetTurnLimit g = new GalibiyetTurnLimit(this);
                    g.ShowDialog();
                }
            }
        }

        

        //This method under construction//
        public void endGame()
        {
            players.Clear();
            ilkTiklananBolge = null;
            turn = 0;
            ilkAskerler = 0;
            ilkAskerSayisi = 15;
            isArmyMoved = false;
            yapayzekalar.Clear();

            foreach (Label l in labels)
            {
                l.Text = "0";
            }

            history.Clear();
        }

        public void updateGameData()
        {
            gameData.bolgeler = bolgeler;
            gameData.players = players;
            gameData.kitalar = kitalar;
            gameData.turn = turn;
            gameData.ganimetBolgesi = ganimetBolgesi;
            gameData.ganimetKazanci = ganimetKazanci;
            gameData.turnLimitVarMi = isTurnLimit;
            gameData.turnLimit = turnLimit;
        }

        public GameData getGameData()
        {
            updateGameData();

            return gameData;
        }
        

        //state//
        //hiç bir yere tıklamamışsa daha 0
        //kendi toprağın tıklamışsa 1
        //rakip komşu toprağa tıkladıysa 2
        //kendi komşu toprağına asker aktaracaksa 3
        int state = 0;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (players[turn].isHuman)
            {
                //Kodlar buraya gelecek
                //Tıklanan yerin kordinatını ve rengini gösterir
                Bitmap bm = new Bitmap(pictureBox1.Image);
                Point localmousePosition = pictureBox1.PointToClient(System.Windows.Forms.Cursor.Position);
                Color col = bm.GetPixel(localmousePosition.X, localmousePosition.Y);
                //MessageBox.Show("X= " + localmousePosition.X.ToString() + "\nY= " + localmousePosition.Y.ToString() +"\nRenk:"+ col.Name);

                foreach (Bolge bol in bolgeler)
                {
                    bool broke = false;
                    foreach (Nokta n in bol.noktalar)
                    {
                        if (n.point == localmousePosition)
                        {
                            //MessageBox.Show(bol.isim);
                            //öncelikle Bölgeler Seçilecek
                            if (!butunBolgelerSecildiMi())
                            {
                                if (!bol.sahipli)
                                {
                                    bol.sahip = players[turn];
                                    bol.sahipli = true;
                                    players[turn].bolgeler.Add(bol);
                                    changeTerritoryColor(bol, players[turn]);
                                    //txtInfo.Text = players[getNextTurn()].name + " please choose a territory";
                                    if (butunBolgelerSecildiMi())
                                    {
                                        changeInfo("Game Starts...");
                                    }
                                    changeTurn();
                                    changeLabel(bol);
                                }
                                if (butunBolgelerSecildiMi())
                                {
                                    //changeInfo(players[turn].name + " please place your army. [" + Math.Floor((double)(ilkAskerler / players.Count)).ToString() + "]");
                                    showHistoryToolStripMenuItem.Enabled = true;
                                }
                            }
                            else
                            {
                                //Kodlar Buraya Gelecek
                                if (ilkAskerler > 0)//İlk Askerleri Dağıtılmasıması bitene kadar 
                                {
                                    if (bol.sahip == players[turn])
                                    {
                                        ilkAskerler--;
                                        players[turn].ilkAskerSayisi--;
                                        bol.ordu.askerSayisi++;
                                        changeLabel(bol);
                                        changeTurn();
                                        //changeInfo(players[turn].name + " please place your army. [" +players[turn].ilkAskerSayisi .ToString() + "]");
                                    }
                                    if (ilkAskerler == 0)
                                    {
                                        changeInfo("You have " + players[turn].yeniAskerler + " armies left to place [+SHIFT=place all]");
                                        ganimetBolgseiniBildir();

                                        ToolTip buttonToolTip = new ToolTip();
                                        buttonToolTip.UseFading = true;
                                        buttonToolTip.UseAnimation = true;
                                        //buttonToolTip.IsBalloon = true;
                                        buttonToolTip.ShowAlways = true;
                                        buttonToolTip.AutoPopDelay = 5000;
                                        buttonToolTip.InitialDelay = 1000;
                                        buttonToolTip.ReshowDelay = 500;
                                        buttonToolTip.Show("Gold is here!", label, 4000);
                                        //buttonToolTip.SetToolTip(label, "Gold is Here!");


                                    }
                                }//ilk Askerlerin Dağıtılması Bittikten Sonra
                                else
                                {
                                    //Öncelikle her turn de verilen askerlerin yerine yerleştirilmesi lazım
                                    if (players[turn].yeniAskerler > 0)
                                    {
                                        if (bol.sahip == players[turn])
                                        {
                                            if (Control.ModifierKeys == Keys.Shift)
                                            {
                                                bol.ordu.askerSayisi += players[turn].yeniAskerler;
                                                players[turn].yeniAskerler = 0;
                                                changeLabel(bol);
                                                txtInfo.Text = "Attack or pass (press to spacebar)";
                                            }
                                            else
                                            {
                                                players[turn].yeniAskerler--;
                                                bol.ordu.askerSayisi++;
                                                changeLabel(bol);
                                                changeInfo("You have " + players[turn].yeniAskerler + " armies left to place [+SHIFT=place all]");
                                            }
                                            if (players[turn].yeniAskerler == 0)
                                            {
                                                button2.Enabled = true;
                                            }
                                        }
                                    }
                                    if (players[turn].yeniAskerler == 0)
                                    {
                                        //Askerler Dağıtıldı şimdi hucum yapılabilir
                                        if (state == 0 && bol.sahip == players[turn])//İlk kez kendi yerine tıklıyor
                                        {
                                            state = 1;
                                            txtInfo.Text = "From " + bol.isim + ". Choose a territory to attack or move to (spacebar to pass)";
                                            ilkTiklananBolge = bol;
                                        }
                                        else if (state == 1 && bol.komsular.Contains(ilkTiklananBolge))//komşusuna tıklamış
                                        {
                                            if (bol.sahip == players[turn])//kendi bölgesine transfer
                                            {
                                                if (ilkTiklananBolge.ordu.askerSayisi > 1 || bol.ordu.askerSayisi > 1)
                                                {
                                                    int b1 = ilkTiklananBolge.ordu.askerSayisi;
                                                    int b2 = bol.ordu.askerSayisi;
                                                    ArmyMove am = new ArmyMove(ilkTiklananBolge, bol, this);
                                                    am.ShowDialog();

                                                    if (isArmyMoved)
                                                    {
                                                        changeLabel(ilkTiklananBolge);
                                                        changeLabel(bol);
                                                        changeTurn();
                                                        isArmyMoved = false;
                                                    }
                                                    else
                                                    {
                                                        ilkTiklananBolge.ordu.askerSayisi = b1;
                                                        bol.ordu.askerSayisi = b2;
                                                    }
                                                }
                                                state = 0;
                                                ilkTiklananBolge = null;
                                            }
                                            else //Düşman bölgesine saldırı
                                            {
                                                if (ilkTiklananBolge.ordu.askerSayisi > 1)
                                                {
                                                    SaldiriEkrani saldiriekrani = new SaldiriEkrani(ilkTiklananBolge, bol, this);
                                                    saldiriekrani.ShowDialog();
                                                    isArmyMoved = false;
                                                }
                                                ilkTiklananBolge = null;
                                                state = 0;
                                            }
                                        }
                                        else
                                        {
                                            state = 0;
                                            ilkTiklananBolge = null;
                                        }
                                    }
                                }
                            }
                            broke = true;
                            break;
                        }
                    }
                    if (broke) break;
                }
                olduMu();
                changeStatistics();
            }
        }

        public void changeInfo(String s)
        {
            txtInfo.Text = s;
        }

        public void changeLabel(Bolge bol)
        {
            if (bol.number == 1)
            {
                label1.Text = bol.ordu.askerSayisi.ToString();
                label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 2)
            {
                label2.Text = bol.ordu.askerSayisi.ToString();
                label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 3)
            {
                label3.Text = bol.ordu.askerSayisi.ToString();
                label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 4)
            {
                label4.Text = bol.ordu.askerSayisi.ToString();
                label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 5)
            {
                label5.Text = bol.ordu.askerSayisi.ToString();
                label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 6)
            {
                label6.Text = bol.ordu.askerSayisi.ToString();
                label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 7)
            {
                label7.Text = bol.ordu.askerSayisi.ToString();
                label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 8)
            {
                label8.Text = bol.ordu.askerSayisi.ToString();
                label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 9)
            {
                label9.Text = bol.ordu.askerSayisi.ToString();
                label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 10)
            {
                label10.Text = bol.ordu.askerSayisi.ToString();
                label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 11)
            {
                label11.Text = bol.ordu.askerSayisi.ToString();
                label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 12)
            {
                label12.Text = bol.ordu.askerSayisi.ToString();
                label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 13)
            {
                label13.Text = bol.ordu.askerSayisi.ToString();
                label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 14)
            {
                label14.Text = bol.ordu.askerSayisi.ToString();
                label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 15)
            {
                label15.Text = bol.ordu.askerSayisi.ToString();
                label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 16)
            {
                label16.Text = bol.ordu.askerSayisi.ToString();
                label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 17)
            {
                label17.Text = bol.ordu.askerSayisi.ToString();
                label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 18)
            {
                label18.Text = bol.ordu.askerSayisi.ToString();
                label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 19)
            {
                label19.Text = bol.ordu.askerSayisi.ToString();
                label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 20)
            {
                label20.Text = bol.ordu.askerSayisi.ToString();
                label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 21)
            {
                label21.Text = bol.ordu.askerSayisi.ToString();
                label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 22)
            {
                label22.Text = bol.ordu.askerSayisi.ToString();
                label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 23)
            {
                label23.Text = bol.ordu.askerSayisi.ToString();
                label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 24)
            {
                label24.Text = bol.ordu.askerSayisi.ToString();
                label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 25)
            {
                label25.Text = bol.ordu.askerSayisi.ToString();
                label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 26)
            {
                label26.Text = bol.ordu.askerSayisi.ToString();
                label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 27)
            {
                label27.Text = bol.ordu.askerSayisi.ToString();
                label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 28)
            {
                label28.Text = bol.ordu.askerSayisi.ToString();
                label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 29)
            {
                label29.Text = bol.ordu.askerSayisi.ToString();
                label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 30)
            {
                label30.Text = bol.ordu.askerSayisi.ToString();
                label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 31)
            {
                label31.Text = bol.ordu.askerSayisi.ToString();
                label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 32)
            {
                label32.Text = bol.ordu.askerSayisi.ToString();
                label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 33)
            {
                label33.Text = bol.ordu.askerSayisi.ToString();
                label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 34)
            {
                label34.Text = bol.ordu.askerSayisi.ToString();
                label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 35)
            {
                label35.Text = bol.ordu.askerSayisi.ToString();
                label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 36)
            {
                label36.Text = bol.ordu.askerSayisi.ToString();
                label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 37)
            {
                label37.Text = bol.ordu.askerSayisi.ToString();
                label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 38)
            {
                label38.Text = bol.ordu.askerSayisi.ToString();
                label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 39)
            {
                label39.Text = bol.ordu.askerSayisi.ToString();
                label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 40)
            {
                label40.Text = bol.ordu.askerSayisi.ToString();
                label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 41)
            {
                label41.Text = bol.ordu.askerSayisi.ToString();
                label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            if (bol.number == 42)
            {
                label42.Text = bol.ordu.askerSayisi.ToString();
                label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            }
            
        }

        public void changeTerritoryColor(Bolge b, Player p)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            foreach (Nokta n in b.noktalar)
            {
                bitmap.SetPixel(n.point.X, n.point.Y, p.color);
                pictureBox1.Image = bitmap;
            }
        }

        public void changeDenizColor(Color c)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            foreach (Nokta n in deniz)
            {
                n.renk = c;
                bitmap.SetPixel(n.point.X, n.point.Y, c);
                pictureBox1.Image = bitmap;
            }
        }

        public void changeSinirColor(Color c)
        {
            Bitmap bitmap = new Bitmap(pictureBox1.Image);
            foreach (Nokta n in sinirlar)
            {
                n.renk = c;
                bitmap.SetPixel(n.point.X, n.point.Y, c);
                pictureBox1.Image = bitmap;
            }
        }


        public void refreshGame()
        {
            pictureBox1.Refresh();
            label1.Refresh();
            label2.Refresh();
            label3.Refresh();
            label4.Refresh();
            label5.Refresh();
            label6.Refresh();
            label7.Refresh();
            label8.Refresh();
            label9.Refresh();
            label10.Refresh();
            label11.Refresh();
            label12.Refresh();
            label13.Refresh();
            label14.Refresh();
            label15.Refresh();
            label16.Refresh();
            label17.Refresh();
            label18.Refresh();
            label19.Refresh();
            label20.Refresh();
            label21.Refresh();
            label22.Refresh();
            label23.Refresh();
            label24.Refresh();
            label25.Refresh();
            label26.Refresh();
            label27.Refresh();
            label28.Refresh();
            label29.Refresh();
            label30.Refresh();
            label31.Refresh();
            label32.Refresh();
            label33.Refresh();
            label34.Refresh();
            label35.Refresh();
            label36.Refresh();
            label37.Refresh();
            label38.Refresh();
            label39.Refresh();
            label40.Refresh();
            label41.Refresh();
            label42.Refresh();

            if (this.Width == 1053)
            {
                try
                {
                    listView1.Refresh();
                }
                catch
                {
                    
                }
            }
        }

        public void hideStatistics()
        {
            this.Width = 793;
            hideStatisticsToolStripMenuItem.Enabled = false;
            showStatisticsToolStripMenuItem.Enabled = true;
            listView1.Enabled = false;
            listView1.Visible = false;
        }

        public void changeTurn()
        {
            int toplamInsan = 0;
            foreach (Player p in players)
            {
                if (p.isHuman)
                {
                    toplamInsan++;
                }
            }
            if (toplamInsan == 0)
            {
                button2.Enabled = false;
                pictureBox1.Enabled = false;
                //if (this.Height != 602)
                //{
                //    this.Height = 602;
                //}

                //if (listView1.Enabled)
                //{
                //    hideStatistics();
                //}
                refreshGame();
                showHistoryToolStripMenuItem.Enabled = true;
                Application.DoEvents();
            }

            if ((turn + 1) < players.Count)
            {
                turn++;
            }
            else
            {
                turn = 0;
                incrementTurnCounter();
            }



            if (ilkAskerler == 0)
            {
                players[turn].yeniAskerler = (3 + kitaKazanci(players[turn]));
            }

            changePanel();
            players[turn].askerSayisiTarihi.Add(players[turn].getTotalNumberOfArmies());
            
            
            

            if (players[turn].isHuman)//insansa
            {
                button2.Enabled = false;
                
            }
            else//Bilgisayarda oynama sırası//////////////////////////////////////////////////////////////////////////////////////
            {
                changePanel();
                if (players.Count == 1)
                {
                    return;
                }

                changeStatistics();
                if (!butunBolgelerSecildiMi()  && !isRandom)//butun bolgeler seçimemişse bilgisayar bolge seçecek
                {
                    int index = 0;
                    

                    index = yapayzeka().requestTerritory(getGameData());

                    if (index < 0 || index > 41)
                    {
                        MessageBox.Show(yapayzeka().getName() + " bölge isteme işlemi zamanı kural ihlali yaptı. Index:" + index);
                        index = defaultAi.requestTerritory(getGameData());
                    }

                    if (bolgeler[index].sahipli)
                    {
                        MessageBox.Show(yapayzeka().getName() + "Yapay Zeka Sahipli bir bölge istedi");
                        index = defaultAi.requestTerritory(getGameData());
                    }


                    if (!bolgeler[index].sahipli)
                    {
                        bolgeler[index].sahip = players[turn];
                        bolgeler[index].sahipli = true;
                        players[turn].bolgeler.Add(bolgeler[index]);
                        changeTerritoryColor(bolgeler[index], players[turn]);
                        txtInfo.Text = players[getNextTurn()].name + " please choose a territory";
                        changeLabel(bolgeler[index]);
                        changeTurn();
                        return;
                    }


                    if (butunBolgelerSecildiMi())
                    {
                        changeInfo(players[turn].name + " please place your army. [" + Math.Floor((double)(ilkAskerler / players.Count)).ToString() + "]");
                        showHistoryToolStripMenuItem.Enabled = true;
                    }
                }
                else
                {
                    if (ilkAskerler > 0)//İlk Askerleri Dağıtılmasıması bitene kadar 
                    {
                        Bolge bol=null;

                        bol = bolgeler[yapayzeka().getTerritoryToPlaceFirstArmies(getGameData())];

                        if (bolgeler[ yapayzeka().getTerritoryToPlaceFirstArmies(getGameData())].sahip!=players[turn])
                        {
                            MessageBox.Show(yapayzeka().getName() + " kendine ait olmayan yere asker koymak istedi");
                            MessageBox.Show("Bolge adı:" + bol.isim);
                            bol = bolgeler[defaultAi.getTerritoryToPlaceFirstArmies(getGameData())];
                        }
                        ilkAskerler--;

                        if (ilkAskerler == 0)
                        {
                            changeInfo(players[turn].name + " please place your army. [" + players[turn].yeniAskerler + "]");

                            ganimetBolgseiniBildir();
                        }

                        players[turn].ilkAskerSayisi--;
                        bol.ordu.askerSayisi++;
                        changeLabel(bol);
                        changeTurn();
                        return;
                    }//ilk Askerlerin Dağıtılması Bittikten Sonra
                    else
                    {
                        //Öncelikle her turn de verilen askerlerin yerine yerleştirilmesi lazım
                        List<int> nereyekoyalim = new List<int>();

                        nereyekoyalim = yapayzeka().getTerritoriesIndexToPlaceNewArmies(getGameData());

                        foreach (int i in nereyekoyalim)
                        {
                            if (bolgeler[i].sahip != players[turn])
                            {
                                MessageBox.Show(yapayzeka().getName()+" Yapay zeka kendine ait olmayan yere asker koymak istedi");
                                nereyekoyalim = defaultAi.getTerritoriesIndexToPlaceNewArmies(getGameData());
                            }
                        }


                        int nereye = 0;
                        while (players[turn].yeniAskerler > 0)
                        {
                            players[turn].yeniAskerler--;
                            bolgeler[nereyekoyalim[nereye]].ordu.askerSayisi++;
                            changeLabel(bolgeler[nereyekoyalim[nereye]]);
                            if (players[turn].yeniAskerler == 0)
                            {
                                button2.Enabled = true;
                            }
                            nereye++;
                        }

                        //Askerler Dağıtıldı şimdi hucum yapılabilir

                        bool birSeyYapacanMi = true;
                        while (birSeyYapacanMi)
                        {
                            refreshGame();

                            birSeyYapacanMi = yapayzeka().devam(getGameData());

                            if (birSeyYapacanMi)
                            {
                                //Yapacakları...
                                bool saldiracakMi = false;

                                saldiracakMi = yapayzeka().saldiracanMi(getGameData());

                                if (saldiracakMi)
                                {
                                    List<int> savasBolgeIndexleri = new List<int>();

                                    savasBolgeIndexleri = yapayzeka().getSavasBolgeIndexleri(getGameData());

                                    bool saldirabilir = true;


                                    //validation
                                    if (bolgeler[savasBolgeIndexleri[0]].sahip == bolgeler[savasBolgeIndexleri[1]].sahip)
                                    {
                                        MessageBox.Show(yapayzeka().getName()+" Yapay zeka kendi bolgesine saldırı yapmak istiyor...");
                                        saldirabilir = false;
                                    }

                                    if (!(bolgeler[savasBolgeIndexleri[0]].komsular.Contains(bolgeler[savasBolgeIndexleri[1]])))
                                    {
                                        MessageBox.Show(yapayzeka().getName() + " Yapay Zeka Komşusu olmadığı bölgeye saldırma isteğinde bulundu");
                                        MessageBox.Show(bolgeler[savasBolgeIndexleri[0]].isim + " ve " + bolgeler[savasBolgeIndexleri[1]].isim + " komşu değil");
                                        saldirabilir = false;
                                    }

                                    if (saldirabilir)
                                    {
                                        SaldiriEkrani se = new SaldiriEkrani(bolgeler[savasBolgeIndexleri[0]], bolgeler[savasBolgeIndexleri[1]], this);

                                        if (savasBolgeIndexleri.Count == 3)//Saldırı Attack şeklinde gerçekleşecek
                                        {
                                            if (savasBolgeIndexleri[2] == -1)
                                            {
                                                se.warAttackForAi();
                                                
                                            }
                                        }
                                        else//Saldırı DoOrDie şeklinde gerçekleşecek
                                        {
                                            se.warDoORDieForAi();
                                        }
                                        
                                        olduMu();
                                    }
                                    
                                }
                            }
                            else
                            {
                                bool transferVarMi = yapayzeka().askerTransferiYapacakmisin(getGameData());

                                if (transferVarMi)
                                {
                                    List<int> transferVerileri = new List<int>();
                                    transferVerileri= yapayzeka().askerTransferiYap(getGameData());
                                    bool transferyapilsinmi = true;

                                    if (transferVerileri[2] < 1 || transferVerileri[3] < 1)
                                    {
                                        MessageBox.Show(players[turn].name + " isimli yapay zeka illegal aktarım denemesinde bulundu. Bolgede en az bir asker birakilmalidir");
                                        transferyapilsinmi = false;
                                    }

                                    if (!((bolgeler[transferVerileri[0]].ordu.askerSayisi + bolgeler[transferVerileri[1]].ordu.askerSayisi) == (transferVerileri[2] + transferVerileri[3])))
                                    {
                                        MessageBox.Show(players[turn].name + " isimli yapay zeka illegal aktarım denemesinde bulundu");
                                        transferyapilsinmi = false;
                                    }

                                    if (transferyapilsinmi)
                                    {

                                        bolgeler[transferVerileri[0]].ordu.askerSayisi = transferVerileri[2];
                                        bolgeler[transferVerileri[1]].ordu.askerSayisi = transferVerileri[3];

                                        changeLabel(bolgeler[transferVerileri[0]]);
                                        changeLabel(bolgeler[transferVerileri[1]]);
                                    }
                                }

                                changeTurn();
                                return;
                            }
                        }
                    }
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

        ArtificalIntelligence yapayzeka()
        {
            return yapayzekalar[players[turn].aiId - 1];
        }


        public void olduMu()
        {
            int index = 0;
            int silinecek = -2;
            foreach (Player p in players)
            {
                if (p.getTotalNumberOfArmies() == 0)
                {
                    silinecek = index;
                }
                index++;
            }
            if (silinecek != -2 && ilkAskerler == 0)
            {
                if (silinecek < turn)
                {
                    turn--;
                }
                statistics.siralama.Add(players[silinecek].name);
                players.RemoveAt(silinecek);
                changeStatistics();

                players[turn].buTurnBirisiniOldurduMu = true;
                players[turn].kacKisiOldurdu++;
                players[turn].buTurnKacKisiOldurdu++;
            }

            if (players.Count == 1 && ilkAskerler == 0)
            {
                statistics.siralama.Add(players[0].name);
                Galibiyet g = new Galibiyet(this);
                listView1.Visible = false;
                g.ShowDialog();
                Application.Exit();
            }
        }

        int kitaKazanci(Player p)
        {
            p.kitalar = "";
            int kazanc=0;
            if (kuzeyAmerika(p.bolgeler))
            {
                kazanc += kitalar[0].getirisi;
                p.kitalar += "NA ";
            }
            if (guneyAmerika(p.bolgeler))
            {
                kazanc += kitalar[1].getirisi;
                p.kitalar += "SA ";
            }
            if (afrika(p.bolgeler))
            {
                kazanc += kitalar[2].getirisi;
                p.kitalar += "AF ";
            }
            if (asya(p.bolgeler))
            {
                kazanc += kitalar[3].getirisi;
                p.kitalar += "AS ";
            }
            if (avrupa(p.bolgeler))
            {
                kazanc += kitalar[4].getirisi;
                p.kitalar += "EU ";
            }
            if (avustralya(p.bolgeler))
            {
                kazanc += kitalar[5].getirisi;
                p.kitalar += "AU ";
            }
            if (p.bolgeler.Contains(bolgeler[ganimetBolgesi]))
            {
                kazanc += ganimetKazanci;
            }
            if (p.buTurnBirisiniOldurduMu)
            {
                kazanc += 10*p.buTurnKacKisiOldurdu;
                if (p.kacKisiOldurdu == 1)
                {
                    //MessageBox.Show(p.name + " won extra " + (p.buTurnKacKisiOldurdu*10).ToString() + " armies because he sent off a player");
                }
                else
                {
                    //MessageBox.Show(p.name + " won extra " + (p.buTurnKacKisiOldurdu*10).ToString() + " armies because he sent off players");
                }
                p.buTurnBirisiniOldurduMu = false;
                p.buTurnKacKisiOldurdu = 0;
            }

            //bolge sayısını 3 e bölüp en yakın int e yuvarladığımızda bulunan sayı 3 ten ne kadar büyükse o kadar eksatra kazanç
            decimal a = p.getNumberOfTerritories();
            int aa =(int)Math.Round(a / 3);
            
            if (aa > 3)
            {
                kazanc += aa - 3;
            }

            return kazanc;
        }

        void hangiKitalaraSahip()
        {
            foreach (Player p in players)
            {
                p.kitalar = "";
                if (kuzeyAmerika(p.bolgeler))
                {
                    p.kitalar += "NA ";
                }
                if (guneyAmerika(p.bolgeler))
                {
                    p.kitalar += "SA ";
                }
                if (afrika(p.bolgeler))
                {
                    p.kitalar += "AF ";
                }
                if (asya(p.bolgeler))
                {
                    p.kitalar += "AS ";
                }
                if (avrupa(p.bolgeler))
                {
                    p.kitalar += "EU ";
                }
                if (avustralya(p.bolgeler))
                {
                    p.kitalar += "AU ";
                }
            }
        }

        bool kuzeyAmerika(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[0].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        bool guneyAmerika(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[1].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        bool afrika(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[2].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        bool asya(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[3].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        bool avrupa(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[4].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        bool avustralya(List<Bolge> sahipOlduguBolgeler)
        {
            foreach (Bolge b in kitalar[5].bolgeler)
            {
                if (!sahipOlduguBolgeler.Contains(b))
                {
                    return false;
                }
            }
            return true;
        }

        //////////////////////////////////////////////////
        
        public int getNextTurn()
        {
            if ((turn + 1) < players.Count)
            {
                return turn+1;
            }
            else
            {
                return 0;
            }
        }

        bool butunBolgelerSecildiMi()
        {
            foreach (Bolge b in bolgeler)
            {
                if (b.sahipli == false)
                {
                    return false;
                }
            }
            return true;
        }

        public void changeStatistics()
        {
            try
            {
                if (this.Width == 1053)
                {
                    hangiKitalaraSahip();
                    listView1.Items.Clear();
                    int k = 0;
                    foreach (Player p in players)
                    {
                        ListViewItem lvitem = new ListViewItem();
                        lvitem.Text = p.name;
                        lvitem.BackColor = p.color;
                        lvitem.SubItems.Add(p.getNumberOfTerritories().ToString());
                        lvitem.SubItems.Add(p.getTotalNumberOfArmies().ToString());
                        lvitem.SubItems.Add(p.kitalar);
                        listView1.Items.Add(lvitem);

                        k++;
                    }

                    this.listView1.ListViewItemSorter = new MyListViewSorter(2);
                    this.listView1.Sort();
                }
            }
            catch
            {
 
            }
        }

        public class MyListViewSorter : System.Collections.IComparer
        {
            private int _columnIndex;

            public MyListViewSorter(int columnIndex)
            {
                _columnIndex = columnIndex;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                ListViewItem lvi1 = x as ListViewItem;
                ListViewItem lvi2 = y as ListViewItem;

                int a = Convert.ToInt32(lvi1.SubItems[_columnIndex].Text);
                int b = Convert.ToInt32(lvi2.SubItems[_columnIndex].Text);

                return b.CompareTo(a);
            }

            #endregion
        }

        Label label = null;
        public void ganimetBolgseiniBildir()
        {
            int a=ganimetBolgesi+1;
            Color b = Color.Gold;
            Color f = Color.DimGray;
            
            if (a == 1)
            {
                label1.BackColor = b;
                label = label1;
            }
            else if (a == 2)
            {
                label2.BackColor = b;
                label = label2;
            }
            else if (a == 3)
            {
                label3.BackColor = b;
                label = label3;
            }
            else if (a == 4)
            {
                label4.BackColor = b;
                label = label4;
            }
            else if (a == 5)
            {
                label5.BackColor = b;
                label = label5;
            }
            else if (a == 6)
            {
                label6.BackColor = b;
                label = label6;
            }
            else if (a == 7)
            {
                label7.BackColor = b;
                label = label7;
            }
            else if (a == 8)
            {
                label8.BackColor = b;
                label = label8;
            }
            else if (a == 9)
            {
                label9.BackColor = b;
                label = label9;
            }
            else if (a == 10)
            {
                label10.BackColor = b;
                label = label10;
            }
            else if (a == 11)
            {
                label11.BackColor = b;
                label = label11;
            }
            else if (a == 12)
            {
                label12.BackColor = b;
                label = label12;
            }
            else if (a == 13)
            {
                label13.BackColor = b;
                label = label13;
            }
            else if (a == 14)
            {
                label14.BackColor = b;
                label = label14;
            }
            else if (a == 15)
            {
                label15.BackColor = b;
                label = label15;
            }
            else if (a == 16)
            {
                label16.BackColor = b;
                label = label16;
            }
            else if (a == 17)
            {
                label17.BackColor = b;
                label = label17;
            }
            else if (a == 18)
            {
                label18.BackColor = b;
                label = label18;
            }
            else if (a == 19)
            {
                label19.BackColor = b;
                label = label19;
            }
            else if (a == 20)
            {
                label20.BackColor = b;
                label = label20;
            }
            else if (a == 21)
            {
                label21.BackColor = b;
                label = label21;
            }
            else if (a == 22)
            {
                label22.BackColor = b;
                label = label22;
            }
            else if (a == 23)
            {
                label23.BackColor = b;
                label = label23;
            }
            else if (a == 24)
            {
                label24.BackColor = b;
                label = label24;
            }
            else if (a == 25)
            {
                label25.BackColor = b;
                label = label25;
            }
            else if (a == 26)
            {
                label26.BackColor = b;
                label = label26;
            }
            else if (a == 27)
            {
                label27.BackColor = b;
                label = label27;
            }
            else if (a == 28)
            {
                label28.BackColor = b;
                label = label28;
            }
            else if (a == 29)
            {
                label29.BackColor = b;
                label = label29;
            }
            else if (a == 30)
            {
                label30.BackColor = b;
                label = label30;
            }
            else if (a == 31)
            {
                label31.BackColor = b;
                label = label31;
            }
            else if (a == 32)
            {
                label32.BackColor = b;
                label = label32;
            }
            else if (a == 33)
            {
                label33.BackColor = b;
                label = label33;
            }
            else if (a == 34)
            {
                label34.BackColor = b;
                label = label34;
            }
            else if (a == 35)
            {
                label35.BackColor = b;
                label = label35;
            }
            else if (a == 36)
            {
                label36.BackColor = b;
                label = label36;
            }
            else if (a == 37)
            {
                label37.BackColor = b;
                label = label37;
            }
            else if (a == 38)
            {
                label38.BackColor = b;
                label = label38;
            }
            else if (a == 39)
            {
                label39.BackColor = b;
                label = label39;
            }
            else if (a == 40)
            {
                label40.BackColor = b;
                label = label40;
            }
            else if (a == 41)
            {
                label41.BackColor = b;
                label = label41;
            }
            else if (a == 42)
            {
                label42.BackColor = b;
                label = label42;
            }
            label.ForeColor = Color.Black;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Loading loading = new Loading();
            loading.Show();
            endGameToolStripMenuItem.Enabled = false;
            listView1.Visible = false;
            pictureBox1.Enabled = false;
            //Öncelikle her bir pixelin bilgisini
            //noktalar isimli listeye atıyoruz.
            Bitmap bm = new Bitmap(pictureBox1.Image);
            for (int i = 0; i < 535; i++)
            {
                for (int j = 0; j < 776; j++)
                {
                    Color col = bm.GetPixel(j, i);
                    Nokta n = new Nokta();
                    n.point.X = j;
                    n.point.Y = i;
                    n.renk = col;
                    noktalar.Add(n);
                }
            }

            List<Nokta> b1 = noktalar.FindAll(Find1);
            List<Nokta> b2 = noktalar.FindAll(Find2);
            List<Nokta> b3 = noktalar.FindAll(Find3);
            List<Nokta> b4 = noktalar.FindAll(Find4);
            List<Nokta> b5 = noktalar.FindAll(Find5);
            List<Nokta> b6 = noktalar.FindAll(Find6);
            List<Nokta> b7 = noktalar.FindAll(Find7);
            List<Nokta> b8 = noktalar.FindAll(Find8);
            List<Nokta> b9 = noktalar.FindAll(Find9);
            List<Nokta> b10 = noktalar.FindAll(Find10);
            List<Nokta> b11 = noktalar.FindAll(Find11);
            List<Nokta> b12 = noktalar.FindAll(Find12);
            List<Nokta> b13 = noktalar.FindAll(Find13);
            List<Nokta> b14 = noktalar.FindAll(Find14);
            List<Nokta> b15 = noktalar.FindAll(Find15);
            List<Nokta> b16 = noktalar.FindAll(Find16);
            List<Nokta> b17 = noktalar.FindAll(Find17);
            List<Nokta> b18 = noktalar.FindAll(Find18);
            List<Nokta> b19 = noktalar.FindAll(Find19);
            List<Nokta> b20 = noktalar.FindAll(Find20);
            List<Nokta> b21 = noktalar.FindAll(Find21);
            List<Nokta> b22 = noktalar.FindAll(Find22);
            List<Nokta> b23 = noktalar.FindAll(Find23);
            List<Nokta> b24 = noktalar.FindAll(Find24);
            List<Nokta> b25 = noktalar.FindAll(Find25);
            List<Nokta> b26 = noktalar.FindAll(Find26);
            List<Nokta> b27 = noktalar.FindAll(Find27);
            List<Nokta> b28 = noktalar.FindAll(Find28);
            List<Nokta> b29 = noktalar.FindAll(Find29);
            List<Nokta> b30 = noktalar.FindAll(Find30);
            List<Nokta> b31 = noktalar.FindAll(Find31);
            List<Nokta> b32 = noktalar.FindAll(Find32);
            List<Nokta> b33 = noktalar.FindAll(Find33);
            List<Nokta> b34 = noktalar.FindAll(Find34);
            List<Nokta> b35 = noktalar.FindAll(Find35);
            List<Nokta> b36 = noktalar.FindAll(Find36);
            List<Nokta> b37 = noktalar.FindAll(Find37);
            List<Nokta> b38 = noktalar.FindAll(Find38);
            List<Nokta> b39 = noktalar.FindAll(Find39);
            List<Nokta> b40 = noktalar.FindAll(Find40);
            List<Nokta> b41 = noktalar.FindAll(Find41);
            List<Nokta> b42 = noktalar.FindAll(Find42);
            deniz=noktalar.FindAll(FindDeniz);
            sinirlar = noktalar.FindAll(FindSinir);

            Bolge tempBolge1 = new Bolge(b1, "Alaska");
            Bolge tempBolge2 = new Bolge(b2, "Northwest Territory");
            Bolge tempBolge3 = new Bolge(b3, "Greenland");
            Bolge tempBolge4 = new Bolge(b4, "Alberta");
            Bolge tempBolge5 = new Bolge(b5, "Ontario");
            Bolge tempBolge6 = new Bolge(b6, "Quebec");
            Bolge tempBolge7 = new Bolge(b7, "Western US");
            Bolge tempBolge8 = new Bolge(b8, "Estern US");
            Bolge tempBolge9 = new Bolge(b9, "Central America");
            Bolge tempBolge10 = new Bolge(b10, "Venezuela");
            Bolge tempBolge11 = new Bolge(b11, "Peru");
            Bolge tempBolge12 = new Bolge(b12, "Brazil");
            Bolge tempBolge13 = new Bolge(b13, "Argentina");
            Bolge tempBolge14 = new Bolge(b14, "North Africa");
            Bolge tempBolge15 = new Bolge(b15, "Egypt");
            Bolge tempBolge16 = new Bolge(b16, "East Africa");
            Bolge tempBolge17 = new Bolge(b17, "Congo");
            Bolge tempBolge18 = new Bolge(b18, "South Africa");
            Bolge tempBolge19 = new Bolge(b19, "Madagascar");
            Bolge tempBolge20 = new Bolge(b20, "Iceland");
            Bolge tempBolge21 = new Bolge(b21, "Scandinavia");
            Bolge tempBolge22 = new Bolge(b22, "Azerbaijan");
            Bolge tempBolge23 = new Bolge(b23, "Northern Europe");
            Bolge tempBolge24 = new Bolge(b24, "Great Britain");
            Bolge tempBolge25 = new Bolge(b25, "Western Europe");
            Bolge tempBolge26 = new Bolge(b26, "Southern Europe");
            Bolge tempBolge27 = new Bolge(b27, "Middle East");
            Bolge tempBolge28 = new Bolge(b28, "Yamal-Nemets");
            Bolge tempBolge29 = new Bolge(b29, "Kazakhstan");
            Bolge tempBolge30 = new Bolge(b30, "India");
            Bolge tempBolge31 = new Bolge(b31, "Siam");
            Bolge tempBolge32 = new Bolge(b32, "China");
            Bolge tempBolge33 = new Bolge(b33, "Mongolia");
            Bolge tempBolge34 = new Bolge(b34, "Taymyr");
            Bolge tempBolge35 = new Bolge(b35, "Yakut");
            Bolge tempBolge36 = new Bolge(b36, "Burgat");
            Bolge tempBolge37 = new Bolge(b37, "Koryak");
            Bolge tempBolge38 = new Bolge(b38, "Japan");
            Bolge tempBolge39 = new Bolge(b39, "Indonesia");
            Bolge tempBolge40 = new Bolge(b40, "Western Australia");
            Bolge tempBolge41 = new Bolge(b41, "Estern Australia");
            Bolge tempBolge42 = new Bolge(b42, "New Guinea");

            bolgeler.Add(tempBolge1);
            bolgeler.Add(tempBolge2);
            bolgeler.Add(tempBolge3);
            bolgeler.Add(tempBolge4);
            bolgeler.Add(tempBolge5);
            bolgeler.Add(tempBolge6);
            bolgeler.Add(tempBolge7);
            bolgeler.Add(tempBolge8);
            bolgeler.Add(tempBolge9);
            bolgeler.Add(tempBolge10);
            bolgeler.Add(tempBolge11);
            bolgeler.Add(tempBolge12);
            bolgeler.Add(tempBolge13);
            bolgeler.Add(tempBolge14);
            bolgeler.Add(tempBolge15);
            bolgeler.Add(tempBolge16);
            bolgeler.Add(tempBolge17);
            bolgeler.Add(tempBolge18);
            bolgeler.Add(tempBolge19);
            bolgeler.Add(tempBolge20);
            bolgeler.Add(tempBolge21);
            bolgeler.Add(tempBolge22);
            bolgeler.Add(tempBolge23);
            bolgeler.Add(tempBolge24);
            bolgeler.Add(tempBolge25);
            bolgeler.Add(tempBolge26);
            bolgeler.Add(tempBolge27);
            bolgeler.Add(tempBolge28);
            bolgeler.Add(tempBolge29);
            bolgeler.Add(tempBolge30);
            bolgeler.Add(tempBolge31);
            bolgeler.Add(tempBolge32);
            bolgeler.Add(tempBolge33);
            bolgeler.Add(tempBolge34);
            bolgeler.Add(tempBolge35);
            bolgeler.Add(tempBolge36);
            bolgeler.Add(tempBolge37);
            bolgeler.Add(tempBolge38);
            bolgeler.Add(tempBolge39);
            bolgeler.Add(tempBolge40);
            bolgeler.Add(tempBolge41);
            bolgeler.Add(tempBolge42);

            //Her Bölgenin tek tek komşuları tanıtılacak
            bolgeler[0].komsular.Add(bolgeler[1]);
            bolgeler[0].komsular.Add(bolgeler[3]);
            bolgeler[0].komsular.Add(bolgeler[36]);

            bolgeler[1].komsular.Add(bolgeler[0]);
            bolgeler[1].komsular.Add(bolgeler[3]);
            bolgeler[1].komsular.Add(bolgeler[4]);
            bolgeler[1].komsular.Add(bolgeler[2]);

            bolgeler[2].komsular.Add(bolgeler[1]);
            bolgeler[2].komsular.Add(bolgeler[4]);
            bolgeler[2].komsular.Add(bolgeler[5]);
            bolgeler[2].komsular.Add(bolgeler[19]);

            bolgeler[3].komsular.Add(bolgeler[0]);
            bolgeler[3].komsular.Add(bolgeler[1]);
            bolgeler[3].komsular.Add(bolgeler[4]);
            bolgeler[3].komsular.Add(bolgeler[6]);

            bolgeler[4].komsular.Add(bolgeler[1]);
            bolgeler[4].komsular.Add(bolgeler[3]);
            bolgeler[4].komsular.Add(bolgeler[5]);
            bolgeler[4].komsular.Add(bolgeler[7]);
            bolgeler[4].komsular.Add(bolgeler[6]);
            bolgeler[4].komsular.Add(bolgeler[2]);

            bolgeler[5].komsular.Add(bolgeler[4]);
            bolgeler[5].komsular.Add(bolgeler[7]);
            bolgeler[5].komsular.Add(bolgeler[2]);

            bolgeler[6].komsular.Add(bolgeler[3]);
            bolgeler[6].komsular.Add(bolgeler[4]);
            bolgeler[6].komsular.Add(bolgeler[7]);
            bolgeler[6].komsular.Add(bolgeler[8]);

            bolgeler[7].komsular.Add(bolgeler[5]);
            bolgeler[7].komsular.Add(bolgeler[4]);
            bolgeler[7].komsular.Add(bolgeler[8]);
            bolgeler[7].komsular.Add(bolgeler[6]);

            bolgeler[8].komsular.Add(bolgeler[7]);
            bolgeler[8].komsular.Add(bolgeler[6]);
            bolgeler[8].komsular.Add(bolgeler[9]);

            bolgeler[9].komsular.Add(bolgeler[8]);
            bolgeler[9].komsular.Add(bolgeler[11]);
            bolgeler[9].komsular.Add(bolgeler[10]);

            bolgeler[10].komsular.Add(bolgeler[9]);
            bolgeler[10].komsular.Add(bolgeler[11]);
            bolgeler[10].komsular.Add(bolgeler[12]);

            bolgeler[11].komsular.Add(bolgeler[9]);
            bolgeler[11].komsular.Add(bolgeler[10]);
            bolgeler[11].komsular.Add(bolgeler[12]);
            bolgeler[11].komsular.Add(bolgeler[13]);

            bolgeler[12].komsular.Add(bolgeler[10]);
            bolgeler[12].komsular.Add(bolgeler[11]);

            bolgeler[13].komsular.Add(bolgeler[11]);
            bolgeler[13].komsular.Add(bolgeler[25]);
            bolgeler[13].komsular.Add(bolgeler[14]);
            bolgeler[13].komsular.Add(bolgeler[15]);
            bolgeler[13].komsular.Add(bolgeler[16]);
            bolgeler[13].komsular.Add(bolgeler[24]);

            bolgeler[14].komsular.Add(bolgeler[25]);
            bolgeler[14].komsular.Add(bolgeler[15]);
            bolgeler[14].komsular.Add(bolgeler[26]);
            bolgeler[14].komsular.Add(bolgeler[13]);

            bolgeler[15].komsular.Add(bolgeler[16]);
            bolgeler[15].komsular.Add(bolgeler[18]);
            bolgeler[15].komsular.Add(bolgeler[13]);
            bolgeler[15].komsular.Add(bolgeler[14]);
            bolgeler[15].komsular.Add(bolgeler[26]);
            bolgeler[15].komsular.Add(bolgeler[17]);


            bolgeler[16].komsular.Add(bolgeler[13]);
            bolgeler[16].komsular.Add(bolgeler[15]);
            bolgeler[16].komsular.Add(bolgeler[17]);

            bolgeler[17].komsular.Add(bolgeler[16]);
            bolgeler[17].komsular.Add(bolgeler[15]);
            bolgeler[17].komsular.Add(bolgeler[18]);

            bolgeler[18].komsular.Add(bolgeler[15]);
            bolgeler[18].komsular.Add(bolgeler[17]);

            bolgeler[19].komsular.Add(bolgeler[2]);
            bolgeler[19].komsular.Add(bolgeler[20]);
            bolgeler[19].komsular.Add(bolgeler[23]);

            bolgeler[20].komsular.Add(bolgeler[21]);
            bolgeler[20].komsular.Add(bolgeler[22]);
            bolgeler[20].komsular.Add(bolgeler[23]);
            bolgeler[20].komsular.Add(bolgeler[19]);

            bolgeler[21].komsular.Add(bolgeler[27]);
            bolgeler[21].komsular.Add(bolgeler[28]);
            bolgeler[21].komsular.Add(bolgeler[26]);
            bolgeler[21].komsular.Add(bolgeler[25]);
            bolgeler[21].komsular.Add(bolgeler[22]);
            bolgeler[21].komsular.Add(bolgeler[20]);

            bolgeler[22].komsular.Add(bolgeler[21]);
            bolgeler[22].komsular.Add(bolgeler[20]);
            bolgeler[22].komsular.Add(bolgeler[23]);
            bolgeler[22].komsular.Add(bolgeler[24]);
            bolgeler[22].komsular.Add(bolgeler[25]);

            bolgeler[23].komsular.Add(bolgeler[19]);
            bolgeler[23].komsular.Add(bolgeler[20]);
            bolgeler[23].komsular.Add(bolgeler[22]);
            bolgeler[23].komsular.Add(bolgeler[24]);

            bolgeler[24].komsular.Add(bolgeler[23]);
            bolgeler[24].komsular.Add(bolgeler[22]);
            bolgeler[24].komsular.Add(bolgeler[25]);
            bolgeler[24].komsular.Add(bolgeler[13]);

            bolgeler[25].komsular.Add(bolgeler[14]);
            bolgeler[25].komsular.Add(bolgeler[26]);
            bolgeler[25].komsular.Add(bolgeler[24]);
            bolgeler[25].komsular.Add(bolgeler[13]);
            bolgeler[25].komsular.Add(bolgeler[22]);
            bolgeler[25].komsular.Add(bolgeler[21]);

            bolgeler[26].komsular.Add(bolgeler[14]);
            bolgeler[26].komsular.Add(bolgeler[15]);
            bolgeler[26].komsular.Add(bolgeler[25]);
            bolgeler[26].komsular.Add(bolgeler[21]);
            bolgeler[26].komsular.Add(bolgeler[28]);
            bolgeler[26].komsular.Add(bolgeler[29]);

            bolgeler[27].komsular.Add(bolgeler[21]);
            bolgeler[27].komsular.Add(bolgeler[28]);
            bolgeler[27].komsular.Add(bolgeler[32]);
            bolgeler[27].komsular.Add(bolgeler[33]);

            bolgeler[28].komsular.Add(bolgeler[21]);
            bolgeler[28].komsular.Add(bolgeler[26]);
            bolgeler[28].komsular.Add(bolgeler[29]);
            bolgeler[28].komsular.Add(bolgeler[31]);
            bolgeler[28].komsular.Add(bolgeler[27]);

            bolgeler[29].komsular.Add(bolgeler[26]);
            bolgeler[29].komsular.Add(bolgeler[28]);
            bolgeler[29].komsular.Add(bolgeler[31]);
            bolgeler[29].komsular.Add(bolgeler[30]);

            bolgeler[30].komsular.Add(bolgeler[29]);
            bolgeler[30].komsular.Add(bolgeler[31]);
            bolgeler[30].komsular.Add(bolgeler[38]);

            bolgeler[31].komsular.Add(bolgeler[30]);
            bolgeler[31].komsular.Add(bolgeler[29]);
            bolgeler[31].komsular.Add(bolgeler[28]);
            bolgeler[31].komsular.Add(bolgeler[32]);

            bolgeler[32].komsular.Add(bolgeler[37]);
            bolgeler[32].komsular.Add(bolgeler[31]);
            bolgeler[32].komsular.Add(bolgeler[27]);
            bolgeler[32].komsular.Add(bolgeler[33]);
            bolgeler[32].komsular.Add(bolgeler[35]);
            bolgeler[32].komsular.Add(bolgeler[36]);

            bolgeler[33].komsular.Add(bolgeler[27]);
            bolgeler[33].komsular.Add(bolgeler[32]);
            bolgeler[33].komsular.Add(bolgeler[35]);
            bolgeler[33].komsular.Add(bolgeler[34]);

            bolgeler[34].komsular.Add(bolgeler[33]);
            bolgeler[34].komsular.Add(bolgeler[35]);
            bolgeler[34].komsular.Add(bolgeler[36]);

            bolgeler[35].komsular.Add(bolgeler[33]);
            bolgeler[35].komsular.Add(bolgeler[34]);
            bolgeler[35].komsular.Add(bolgeler[36]);
            bolgeler[35].komsular.Add(bolgeler[32]);

            bolgeler[36].komsular.Add(bolgeler[37]);
            bolgeler[36].komsular.Add(bolgeler[32]);
            bolgeler[36].komsular.Add(bolgeler[35]);
            bolgeler[36].komsular.Add(bolgeler[34]);
            bolgeler[36].komsular.Add(bolgeler[0]);

            bolgeler[37].komsular.Add(bolgeler[32]);
            bolgeler[37].komsular.Add(bolgeler[36]);

            bolgeler[38].komsular.Add(bolgeler[30]);
            bolgeler[38].komsular.Add(bolgeler[41]);
            bolgeler[38].komsular.Add(bolgeler[39]);

            bolgeler[39].komsular.Add(bolgeler[40]);
            bolgeler[39].komsular.Add(bolgeler[41]);
            bolgeler[39].komsular.Add(bolgeler[38]);

            bolgeler[40].komsular.Add(bolgeler[39]);
            bolgeler[40].komsular.Add(bolgeler[41]);

            bolgeler[41].komsular.Add(bolgeler[38]);
            bolgeler[41].komsular.Add(bolgeler[39]);
            bolgeler[41].komsular.Add(bolgeler[40]);

            int k=1;
            foreach (Bolge bb in bolgeler)
            {
                bb.number = k;
                bb.index = k - 1;
                k++;
            }

            for (int i = 0; i <= 9; i++)
            {
                Player p = new Player("Player"+(i+1).ToString(),false,false);
                p.number = i + 1;
                players.Add(p);
            }
            players[0].color = Color.Blue;
            players[1].color = Color.Red;
            players[2].color = Color.Yellow;
            players[3].color = Color.Lime;
            players[4].color = Color.Turquoise;
            players[5].color = Color.Green;
            players[6].color = Color.HotPink;
            players[7].color = Color.DarkOrange;
            players[8].color = Color.Purple;
            players[9].color = Color.DarkRed;

            Bitmap bmr = new Bitmap(pictureBox1.Image);

            foreach (Bolge b in bolgeler)
            {
                foreach (Nokta n in b.noktalar)
                {
                    bmr.SetPixel(n.point.X, n.point.Y, Color.AliceBlue);
                }
            }
            
            foreach (Player pl in players)
            {
                pl.ilkAskerSayisi = ilkAskerSayisi;
                pl.isHuman = false;
                pl.aiId = 1;
                pl.isActive = true;
            }

            players[0].isHuman = true;
            players[1].aiId = 2;
            players[2].aiId = 3;
            players[3].aiId = 4;
            pictureBox1.Image = bmr;

            Continent c1 = new Continent("N. America", 5);

            c1.bolgeler.Add(bolgeler[0]);
            c1.bolgeler.Add(bolgeler[1]);
            c1.bolgeler.Add(bolgeler[2]);
            c1.bolgeler.Add(bolgeler[3]);
            c1.bolgeler.Add(bolgeler[4]);
            c1.bolgeler.Add(bolgeler[5]);
            c1.bolgeler.Add(bolgeler[6]);
            c1.bolgeler.Add(bolgeler[7]);
            c1.bolgeler.Add(bolgeler[8]);

            Continent c2 = new Continent("S. America", 2);

            c2.bolgeler.Add(bolgeler[9]);
            c2.bolgeler.Add(bolgeler[10]);
            c2.bolgeler.Add(bolgeler[11]);
            c2.bolgeler.Add(bolgeler[12]);

            Continent c3 = new Continent("Africa",3);

            c3.bolgeler.Add(bolgeler[13]);
            c3.bolgeler.Add(bolgeler[14]);
            c3.bolgeler.Add(bolgeler[15]);
            c3.bolgeler.Add(bolgeler[16]);
            c3.bolgeler.Add(bolgeler[17]);
            c3.bolgeler.Add(bolgeler[18]);

            Continent c4 = new Continent("Asia",7);

            c4.bolgeler.Add(bolgeler[26]);
            c4.bolgeler.Add(bolgeler[27]);
            c4.bolgeler.Add(bolgeler[28]);
            c4.bolgeler.Add(bolgeler[29]);
            c4.bolgeler.Add(bolgeler[30]);
            c4.bolgeler.Add(bolgeler[31]);
            c4.bolgeler.Add(bolgeler[32]);
            c4.bolgeler.Add(bolgeler[33]);
            c4.bolgeler.Add(bolgeler[34]);
            c4.bolgeler.Add(bolgeler[35]);
            c4.bolgeler.Add(bolgeler[36]);
            c4.bolgeler.Add(bolgeler[37]);

            Continent c5 = new Continent("Europe",5);

            c5.bolgeler.Add(bolgeler[19]);
            c5.bolgeler.Add(bolgeler[20]);
            c5.bolgeler.Add(bolgeler[21]);
            c5.bolgeler.Add(bolgeler[22]);
            c5.bolgeler.Add(bolgeler[23]);
            c5.bolgeler.Add(bolgeler[24]);
            c5.bolgeler.Add(bolgeler[25]);

            Continent c6 = new Continent("Australia",2);

            c6.bolgeler.Add(bolgeler[38]);
            c6.bolgeler.Add(bolgeler[39]);
            c6.bolgeler.Add(bolgeler[40]);
            c6.bolgeler.Add(bolgeler[41]);
            
            kitalar.Add(c1);
            kitalar.Add(c2);
            kitalar.Add(c3);
            kitalar.Add(c4);
            kitalar.Add(c5);
            kitalar.Add(c6);

            this.Height = 602;
            //this.Width = 793;

            //hideStatisticsToolStripMenuItem.Enabled = false;

            showStatisticsToolStripMenuItem_Click(sender, e);

            //Labelleri hepsini tek bir listeye atacağım
            labels.Add(label1);
            labels.Add(label2);
            labels.Add(label3);
            labels.Add(label4);
            labels.Add(label5);
            labels.Add(label6);
            labels.Add(label7);
            labels.Add(label8);
            labels.Add(label9);
            labels.Add(label10);
            labels.Add(label11);
            labels.Add(label12);
            labels.Add(label13);
            labels.Add(label14);
            labels.Add(label15);
            labels.Add(label16);
            labels.Add(label17);
            labels.Add(label18);
            labels.Add(label19);
            labels.Add(label20);
            labels.Add(label21);
            labels.Add(label22);
            labels.Add(label23);
            labels.Add(label24);
            labels.Add(label25);
            labels.Add(label26);
            labels.Add(label27);
            labels.Add(label28);
            labels.Add(label29);
            labels.Add(label30);
            labels.Add(label31);
            labels.Add(label32);
            labels.Add(label33);
            labels.Add(label34);
            labels.Add(label35);
            labels.Add(label36);
            labels.Add(label37);
            labels.Add(label38);
            labels.Add(label39);
            labels.Add(label40);
            labels.Add(label41);
            labels.Add(label42);

            foreach (Label l in labels)
            {
                l.Visible = false;
                l.Enabled = false;
            }


            yapayzekalar.Add(ai);
            yapayzekalar.Add(ai2);
            yapayzekalar.Add(ai3);
            yapayzekalar.Add(ai4);

            changeDenizColor(Color.SlateGray);

            changePicture();

            listView1.Visible = false;

            timer1.Start();

            timer1.Interval = 10;

            loading.Close();
        }

        // Bu metod Türkiyenin bulunduğu bölgeyi bulmak için kullanılıyor
        private static bool Find1(Nokta n)
        {
            if (n.renk.Name == "ff006000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ////////////////////////////////////////////////////////////////
        private static bool Find2(Nokta n)
        {
            if (n.renk.Name == "ffe04000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find3(Nokta n)
        {
            if (n.renk.Name == "ffc0dcc0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find4(Nokta n)
        {
            if (n.renk.Name == "ffc06000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find5(Nokta n)
        {
            if (n.renk.Name == "ff604000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find6(Nokta n)
        {
            if (n.renk.Name == "ffe06000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find7(Nokta n)
        {
            if (n.renk.Name == "ff606000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find8(Nokta n)
        {
            if (n.renk.Name == "ff00c000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find9(Nokta n)
        {
            if (n.renk.Name == "ffc0e000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find10(Nokta n)
        {
            if (n.renk.Name == "ffa00040")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find11(Nokta n)
        {
            if (n.renk.Name == "ffe06040")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find12(Nokta n)
        {
            if (n.renk.Name == "ffe00040")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find13(Nokta n)
        {
            if (n.renk.Name == "ffe0a040")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find14(Nokta n)
        {
            if (n.renk.Name == "ffc00080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find15(Nokta n)
        {
            if (n.renk.Name == "ff006080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find16(Nokta n)
        {
            if (n.renk.Name == "ffe00080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find17(Nokta n)
        {
            if (n.renk.Name == "ff004080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find18(Nokta n)
        {
            if (n.renk.Name == "ff604080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find19(Nokta n)
        {
            if (n.renk.Name == "ffc06080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find20(Nokta n)
        {
            if (n.renk.Name == "ff40c080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find21(Nokta n)
        {
            if (n.renk.Name == "ffc0c080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find22(Nokta n)
        {
            if (n.renk.Name == "ffc0a080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find23(Nokta n)
        {
            if (n.renk.Name == "ff00a080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find24(Nokta n)
        {
            if (n.renk.Name == "ffe0c080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find25(Nokta n)
        {
            if (n.renk.Name == "ffa0c080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find26(Nokta n)
        {
            if (n.renk.Name == "ffa0e080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find27(Nokta n)
        {
            if (n.renk.Name == "ffc000c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find28(Nokta n)
        {
            if (n.renk.Name == "ff8020c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find29(Nokta n)
        {
            if (n.renk.Name == "ff0000c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find30(Nokta n)
        {
            if (n.renk.Name == "ffe040c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find31(Nokta n)
        {
            if (n.renk.Name == "ffc0c0c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find32(Nokta n)
        {
            if (n.renk.Name == "ffa000c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find33(Nokta n)
        {
            if (n.renk.Name == "ffe06080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find34(Nokta n)
        {
            if (n.renk.Name == "ff40c0c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find35(Nokta n)
        {
            if (n.renk.Name == "ffe080c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find36(Nokta n)
        {
            if (n.renk.Name == "ff40a0c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find37(Nokta n)
        {
            if (n.renk.Name == "ff80a0c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find38(Nokta n)
        {
            if (n.renk.Name == "ffe0a0c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find39(Nokta n)
        {
            if (n.renk.Name == "ff6000c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find40(Nokta n)
        {
            if (n.renk.Name == "ff0060c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find41(Nokta n)
        {
            if (n.renk.Name == "ffa6caf0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool Find42(Nokta n)
        {
            if (n.renk.Name == "ffc060c0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ///////////////////////////////////////////////////////////////
        private static bool FindDeniz(Nokta n)
        {
            if (n.renk.Name == "ff000080")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool FindSinir(Nokta n)
        {
            if (n.renk.Name == "ff000000")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public void changePanel()
        {
            txtPlayer.Text = players[turn].name;
            txtPlayer.BackColor = players[turn].color;
            if (ilkAskerler == 0  && players[turn].isHuman)
            {
                txtInfo.Text = "You have " + players[turn].yeniAskerler.ToString() + " armies left to place [+SHIFT=place all]";
            }

            if (!players[turn].isHuman)
            {
                txtInfo.Text = "is playing...";
            }
            else
            {
                if (players[turn].isHuman)
                {
                    if (!butunBolgelerSecildiMi())
                    {
                        txtInfo.Text = "Please claim a territory";
                    }
                    else if (ilkAskerler > 0)
                    {
                        txtInfo.Text = "You have " + players[turn].ilkAskerSayisi + " armies left to place. Please place an army";
                    }
                }
            }


            txtPlayer.Refresh();
            txtInfo.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeTurn();
            ilkTiklananBolge = null;
            state = 0;
        }

        public bool yeniOyunIptal = false;

        


        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yeniOyunIptal = false;
            Giris giris = new Giris(players,this);
            giris.ShowDialog();
            if (!yeniOyunIptal)
            {
                this.Height += 40;
                Random rand = new Random();
                ganimetBolgesi = rand.Next(0, 41);
                newGameToolStripMenuItem.Enabled = false;
                rulesToolStripMenuItem.Enabled = false;
                endGameToolStripMenuItem.Enabled = true;

                foreach (Label l in labels)
                {
                    l.Visible = true;
                    l.Enabled = true;
                }

                panel1.Visible = true;
                pictureBox1.Enabled = true;
                listView1.Visible = true;
                //Aktif olmayan playerleri player listesinden çıkarıyorum
                List<Player> silinecekler = new List<Player>();
                for (int i = 0; i <= 9; i++)
                {
                    if (!players[i].isActive)
                    {
                        silinecekler.Add(players[i]);
                    }
                }
                foreach (Player silinecek in silinecekler)
                {
                    players.Remove(silinecek);
                }

                ilkAskerler = players.Count * ilkAskerSayisi;

                if (isRandom)//Random olma durumunda bolgeler otomatik atanacak...
                {
                    List<int> sayilar = new List<int>();
                    for (int i = 0; i <= 41; i++)
                    {
                        sayilar.Add(i);
                    }

                    for (int i = 0; i <= 41; i++)
                    {
                        Random r = new Random();
                        int index = r.Next(0, sayilar.Count - 1);
                        bolgeler[sayilar[index]].sahip = players[turn];
                        bolgeler[sayilar[index]].sahipli = true;
                        players[turn].bolgeler.Add(bolgeler[sayilar[index]]);
                        changeTerritoryColor(bolgeler[sayilar[index]], players[turn]);
                        changeLabel(bolgeler[sayilar[index]]);
                        

                        if (i == 41)
                        {
                            changeTurn();
                        }
                        else
                        {
                            txtInfo.Text = players[turn].name + " place your army " + "[" + players[turn].ilkAskerSayisi.ToString() + "]";
                            turn = getNextTurn();
                        }

                        sayilar.RemoveAt(index);
                    }
                    showHistoryToolStripMenuItem.Enabled = true;
                }

                
                if (!isRandom)
                {
                    if (!players[turn].isHuman)
                    {
                        changeTurn();
                    }
                    
                    txtInfo.Text = players[turn].name + " please choose a territory";
                    txtPlayer.Text = players[turn].name;
                    txtPlayer.BackColor = players[turn].color;
                }

                
                changeStatistics();
                
                foreach (Player p in players)
                {
                    history.Add(p.OptimizeAskerSayisiTarihi());
                }

                listView1.Visible = true;

                statistics.players = players;
            }
        }

        private void endGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }


        
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Space)
            {
                if (button2.Enabled)
                {
                    changeTurn();
                    ilkTiklananBolge = null;
                    state = 0;
                }
            }


        //    if ((e.Modifiers & Keys.Shift) == Keys.Shift)
        //    {

        //    }
        //    else
        //    {
        //        MessageBox.Show("Shift basılı değil");
        //    }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rules rules = new Rules(this);
            rules.ShowDialog();
            foreach (Player p in players)
            {
                p.ilkAskerSayisi = ilkAskerSayisi;
            }
        }

        private void webSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://developersland.net/index.php/feridmovsumov/74-risk-game-ferid-movsumov.html");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 1053;
            showStatisticsToolStripMenuItem.Enabled = false;
            hideStatisticsToolStripMenuItem.Enabled = true;
            listView1.Enabled = true;
            if (!newGameToolStripMenuItem.Enabled)
            {
                listView1.Visible = true;
            }
        }

        private void hideStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 793;
            hideStatisticsToolStripMenuItem.Enabled = false;
            showStatisticsToolStripMenuItem.Enabled = true;
            listView1.Enabled = false;
            listView1.Visible = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        

        private void facebookPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Risk-Game/256463171079345");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/pages/Risk-Game/256463171079345");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://twitter.com/#!/RISKGAMEWAR");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label14_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label16_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label17_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label18_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label19_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label20_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label21_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label22_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label26_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label25_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label24_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label23_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label41_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label40_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label31_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label30_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label27_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label29_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label28_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label34_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label35_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label37_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label38_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label33_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label36_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void label32_Click(object sender, EventArgs e)
        {
            this.pictureBox1_Click(sender, e);
        }

        private void changeStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStyle cs = new ChangeStyle(this);
            cs.ShowDialog();
        }

        private void showHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chart c = new Chart(this);
            c.Show();
        }

        private void followUsOnTwitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://twitter.com/#!/RISKGAMEWAR");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
        }


        

        public void changePicture()
        {
            if (pictureIndex == 0)
            {
                pictureBox4.Image = Properties.Resources.babek;
            }
            if (pictureIndex == 1)
            {
                pictureBox4.Image = Properties.Resources.savasci;
            }
            if (pictureIndex == 2)
            {
                pictureBox4.Image = Properties.Resources.savascikiz;
            }

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
