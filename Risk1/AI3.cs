using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    public class AI3 : ArtificalIntelligence
    {
        public static int aiid = 3;
        public static string name = "Babak";
        public static Image resim = Properties.Resources.babek;

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
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                if (getKomsuDusmanSayisi(b) > 0)
                {
                    return b.index;
                }
            }

            return f1.players[f1.turn].bolgeler[0].index;
        }


        public override List<int> getTerritoriesIndexToPlaceNewArmies(GameData f1)
        {
            List<int> gonderilecekler = new List<int>();
            int orduyuNereyeKoyalim = -1;

            if (getKomsuDusmanSayisi(f1.bolgeler[getEnGucluBolgemIndex(f1)]) > 0)
            {
                orduyuNereyeKoyalim = getEnGucluBolgemIndex(f1);
            }
            else
            {
                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    if (getDahaGucluDusmanSayisi(b) > 0)
                    {
                        orduyuNereyeKoyalim = b.index;
                    }
                }
            }

            if (orduyuNereyeKoyalim == -1)
            {
                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    if (getKomsuDusmanSayisi(b) > 0)
                    {
                        orduyuNereyeKoyalim = b.index;
                    }
                }
            }


            int k=f1.players[f1.turn].yeniAskerler;
            for (int i = 0; i <k ; i++)
            {
                gonderilecekler.Add(orduyuNereyeKoyalim);
            }
            return gonderilecekler;
        }

        public override bool devam(GameData f1)
        {
            List<int> sonuc = new List<int>();

            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                foreach (Bolge komsu in b.komsular)
                {
                    if (b.ordu.askerSayisi > komsu.ordu.askerSayisi+30 && b.sahip!=komsu.sahip)
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

        public int getDahaGucluDusmanSayisi(Bolge b)
        {
            int toplam = 0;
            foreach (Bolge bb in b.komsular)
            {
                if (bb.sahip != b.sahip && b.ordu.askerSayisi<bb.ordu.askerSayisi)
                {
                    toplam++;
                }
            }
            return toplam;
        }


        public override List<int> getSavasBolgeIndexleri(GameData f1)
        {
            List<int> sonuc = new List<int>();

            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                foreach (Bolge komsu in b.komsular)
                {
                    if (b.ordu.askerSayisi > komsu.ordu.askerSayisi+30 && b.sahip != komsu.sahip)
                    {
                        sonuc.Add(b.index);
                        sonuc.Add(komsu.index);
                    }
                }
            }
            return sonuc;
        }

        public override List<int> divideArmies(int index1, int index2,GameData f1)
        {
            List<int> sonuc = new List<int>();

            int toplam = f1.bolgeler[index1].ordu.askerSayisi + f1.bolgeler[index2].ordu.askerSayisi;

            sonuc.Add(1);
            sonuc.Add(toplam - 1);

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
