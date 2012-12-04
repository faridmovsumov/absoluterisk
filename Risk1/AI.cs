using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    public class AI : ArtificalIntelligence
    {
        public AI()
        {
            
        }
        public static int aiid = 1;
        public static string name = "Genghis Khan";
        public static Image resim = Properties.Resources.cengizhan;

        public override string getName()
        {
            return name;
        }

        public override int requestTerritory(GameData f1)
        {
            return rastgeleBolgeSec(f1);
        }

        public override int getTerritoryToPlaceFirstArmies(GameData f1)
        {
            return f1.players[f1.turn].bolgeler[0].index;
        }

        public override List<int> getTerritoriesIndexToPlaceNewArmies(GameData f1)
        {
            List<int> gonderilecekler = new List<int>();
            Random r=new Random();
            
            List<Bolge> komsuluktaDusmanOlanBolgeler = new List<Bolge>();

            foreach (Bolge bb in f1.players[f1.turn].bolgeler)
            {
                if (getKomsuDusmanSayisi(bb) > 0)
                {
                    komsuluktaDusmanOlanBolgeler.Add(bb);
                }
            }

            int z = r.Next(0, komsuluktaDusmanOlanBolgeler.Count - 1);

            int orduyuNereyeKoyalim = komsuluktaDusmanOlanBolgeler[z].index;

            int k=f1.players[f1.turn].yeniAskerler;

            for (int i = 0; i <k ; i++)
            {
                gonderilecekler.Add(orduyuNereyeKoyalim);
            }
            return gonderilecekler;
        }

        public override bool devam(GameData f1)
        {
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                foreach (Bolge komsu in b.komsular)
                {
                    if (b.ordu.askerSayisi > komsu.ordu.askerSayisi && b.sahip!=komsu.sahip)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool saldiracanMi(GameData f1)
        {
            return devam(f1);
        }


        public override List<int> getSavasBolgeIndexleri(GameData f1)
        {
            List<int> sonuc = new List<int>();

            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                foreach (Bolge komsu in b.komsular)
                {
                    if (b.ordu.askerSayisi > komsu.ordu.askerSayisi && b.sahip != komsu.sahip)
                    {
                        sonuc.Add(b.index);
                        sonuc.Add(komsu.index);
                    }
                }
            }
            sonuc.Add(-1);

            return sonuc;
        }

        public override List<int> divideArmies(int index1, int index2,GameData f1)
        {
            List<int> sonuc = new List<int>();

            int toplam = f1.bolgeler[index1].ordu.askerSayisi + f1.bolgeler[index2].ordu.askerSayisi;

            if ((toplam % 2) == 0)
            {
                sonuc.Add(toplam / 2);
                sonuc.Add(toplam / 2);
            }
            else
            {
                sonuc.Add(toplam / 2);
                sonuc.Add((toplam / 2) + 1);
            }

            return sonuc;
        }

        public override bool askerTransferiYapacakmisin(GameData f1)
        {
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                if (getKomsuDusmanSayisi(b) == 0)
                {
                    if (b.ordu.askerSayisi > 1)
                    {
                        if (getDostSayisi(b) > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override List<int> askerTransferiYap(GameData f1)
        {
            bool break2 = false;
            List<int> veriler = new List<int>();
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                if (getKomsuDusmanSayisi(b) == 0)
                {
                    if (b.ordu.askerSayisi > 1)
                    {
                        if (getDostSayisi(b) > 0)
                        {
                            //Once nereden transfer yapacagimizi belirledik
                            veriler.Add(b.index);

                            foreach (Bolge komsu in b.komsular)
                            {
                                if (komsu.sahip == b.sahip)
                                {
                                    veriler.Add(komsu.index);
                                    veriler.Add(1);
                                    veriler.Add(komsu.ordu.askerSayisi + b.ordu.askerSayisi - 1);
                                    break2 = true;
                                    break;
                                }
                            }
                            if (break2)
                                break;
                        }
                    }
                }
            }
            return veriler;
        }
    }
}