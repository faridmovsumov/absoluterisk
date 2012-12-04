using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    public class AI4:ArtificalIntelligence
    {
        public static int aiid = 4;
        public static string name = "Fatih Sultan Mehmet";
        public static Image resim = Properties.Resources.fatih;

        public int saldirmaDurumu = 0;//Saldırı türümüzü belirler
        public List<int> durum3SaldiriIndexleri = new List<int>(2);

        public override string getName()
        {
            return name;
        }

        public override int requestTerritory(GameData f1)
        {
            if (!f1.bolgeler[9].sahipli)
            {
                return 9;
            }
            if (!f1.bolgeler[10].sahipli)
            {
                return 10;
            }
            if (!f1.bolgeler[11].sahipli)
            {
                return 11;
            }
            if (!f1.bolgeler[12].sahipli)
            {
                return 12;
            }

            return rastgeleBolgeSec(f1);
        }

        public override int getTerritoryToPlaceFirstArmies(GameData f1)
        {
            List<int> sahipoldugubolgeler = new List<int>();
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                sahipoldugubolgeler.Add(b.index);
            }

            Random r = new Random();

            int sayi = r.Next(0, sahipoldugubolgeler.Count - 1);

            int index = getEnGucluBolgemIndex(f1);

            if (sahipoldugubolgeler.Contains(9))
            {
                return f1.bolgeler[9].index;
            }
            if (sahipoldugubolgeler.Contains(10))
            {
                return f1.bolgeler[10].index;
            }
            if (sahipoldugubolgeler.Contains(11))
            {
                return f1.bolgeler[11].index;
            }
            if (sahipoldugubolgeler.Contains(12))
            {
                return f1.bolgeler[12].index;
            }
            

            return f1.bolgeler[index].index;
        }

        bool southAmericaBendeMi(GameData f1)
        {
            if (f1.bolgeler[9].sahip != f1.players[f1.turn])
            {
                return false;
            }
            if (f1.bolgeler[10].sahip != f1.players[f1.turn])
            {
                return false;
            }
            if (f1.bolgeler[11].sahip != f1.players[f1.turn])
            {
                return false;
            }
            if (f1.bolgeler[12].sahip != f1.players[f1.turn])
            {
                return false;
            }

            return true;
        }

        bool southAmericaSinirindaDusmanVarMi(GameData f1)
        {
            //13 ve 8
            if (f1.bolgeler[13].sahip != f1.players[f1.turn])
            {
                return true;
            }
            if (f1.bolgeler[8].sahip != f1.players[f1.turn])
            {
                return true;
            }

            return false;
        }

        int dusmanNerede(GameData f1)
        {
            List<int> ikibolge=new List<int>();
            ikibolge.Add(13);
            ikibolge.Add(8);
            if (f1.bolgeler[13].sahip != f1.players[f1.turn] && f1.bolgeler[8].sahip != f1.players[f1.turn])
            {
                Random r = new Random();

                if (r.Next(1, 100) % 2 == 0)
                {
                    return ikibolge[0];
                }
                else
                {
                    return ikibolge[1];
                }
            }

            if (f1.bolgeler[13].sahip != f1.players[f1.turn])
            {
                return 13;
            }
            if (f1.bolgeler[8].sahip != f1.players[f1.turn])
            {
                return 8;
            }

            return -1;
        }

        int southAmericadaToprakVarMi(GameData f1)
        {
            //9 10 11 12
            if (f1.bolgeler[9].sahip == f1.players[f1.turn])
            {
                return 9;
            }
            if (f1.bolgeler[10].sahip == f1.players[f1.turn])
            {
                return 10;
            }
            if (f1.bolgeler[11].sahip == f1.players[f1.turn])
            {
                return 11;
            }
            if (f1.bolgeler[12].sahip == f1.players[f1.turn])
            {
                return 12;
            }

            return -1;
        }

        public override List<int> getTerritoriesIndexToPlaceNewArmies(GameData f1)
        {
            List<int> gonderilecekler = new List<int>();
            int territory=bolgelerimdenRastgeleSec(f1);
            if (southAmericaBendeMi(f1))
            {
                if (southAmericaSinirindaDusmanVarMi(f1))
                {
                    if (dusmanNerede(f1) == 13)
                    {
                        territory = 11;
                    }
                    else if (dusmanNerede(f1) == 8)
                    {
                        territory = 9;
                    }
                }
                else
                {
                    Random r = new Random();

                    List<Bolge> komsuluktaDusmanOlanBolgeler = new List<Bolge>();

                    foreach (Bolge bb in f1.players[f1.turn].bolgeler)
                    {
                        if (getKomsuDusmanSayisi(bb) > 0)
                        {
                            komsuluktaDusmanOlanBolgeler.Add(bb);
                        }
                    }

                    int z = r.Next(0, komsuluktaDusmanOlanBolgeler.Count - 1);

                    territory = komsuluktaDusmanOlanBolgeler[z].index;
                }
            }
            else
            {
                if (southAmericadaToprakVarMi(f1)!=-1)//Evet var
                {
                    territory = southAmericadaToprakVarMi(f1);
                }
                else//yok
                {
                    territory = getEnGucluBolgemIndex(f1);
                }
            }

            int k = f1.players[f1.turn].yeniAskerler;

            for (int i = 0; i < k; i++)
            {
                gonderilecekler.Add(territory);
            }

            return gonderilecekler;
        }

        public override bool devam(GameData f1)
        {
            durum3SaldiriIndexleri.Clear();
            if (southAmericaBendeMi(f1))
            {
                //Ordusu diğerlerinin toplamından buyuk olduğunda her tarafa saldıracak...
                if (getToplamDusmanSayisi(f1)/2 < f1.players[f1.turn].getTotalNumberOfArmies())
                {
                    foreach (Bolge b in f1.players[f1.turn].bolgeler)
                    {
                        foreach (Bolge komsu in b.komsular)
                        {
                            if (b.ordu.askerSayisi > komsu.ordu.askerSayisi && b.sahip != komsu.sahip)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else//Güney Amerikayı ele geçirmeye çalışacak...
            {
                if (southAmericadaToprakVarMi(f1)!=-1)
                {
                    foreach (Bolge b in f1.players[f1.turn].bolgeler)
                    {
                        if (getKomsuDusmanSayisi(b) > 0)
                        {
                            foreach (Bolge b2 in b.komsular)
                            {
                                if (b2.sahip != b.sahip)
                                {
                                    if (b.ordu.askerSayisi > b2.ordu.askerSayisi+10 && buBolgeGuneyAmerikadaMi(b2))
                                    {
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }
                else//Guney Amerikada Toprak yoksa...
                {
                    Graph graph = new Graph();
                    foreach (Bolge b in f1.bolgeler)
                    {
                        graph.AddNode(b.index.ToString());
                    }

                    foreach (Bolge b in f1.bolgeler)
                    {
                        foreach (Bolge b2 in b.komsular)
                        {
                            if (b2.sahip != f1.players[f1.turn])
                            {
                                graph.AddConnection(b.index.ToString(), b2.index.ToString(), b2.ordu.askerSayisi, false);
                            }
                        }
                    }

                    var calculator = new DistanceCalculator();
                    var distances = calculator.CalculateDistances(graph, getEnGucluBolgemIndex(f1).ToString());  // Start from "G"

                    String temp = "";

                    //SA Sınırları
                    List<int> gaSinirlari = new List<int>();
                    gaSinirlari.Add(0);
                    gaSinirlari.Add(0);

                    foreach (var d in distances)
                    {
                        temp += d.Key + " - " + d.Value + "-";
                        if (d.Key == "9")
                        {
                            gaSinirlari[0] = (int)d.Value;
                        }
                        if (d.Key == "11")
                        {
                            gaSinirlari[1] = (int)d.Value;
                        }
                    }

                    int min = gaSinirlari.Min();
                    int minIndex = gaSinirlari.IndexOf(min);


                    int minBolgeIndex = 0;
                    if (minIndex == 0)
                    {
                        minBolgeIndex = 9;
                    }
                    if (minIndex == 1)
                    {
                        minBolgeIndex = 11;
                    }

                    List<String> gidilecekyol = new List<String>();

                    //calculator = new DistanceCalculator();

                    gidilecekyol = calculator.getYol(getEnGucluBolgemIndex(f1).ToString(), minBolgeIndex.ToString(), graph);


                    if (f1.bolgeler[getEnGucluBolgemIndex(f1)].ordu.askerSayisi > min + 10)//Gideceğiz Avrupaya
                    {
                        if (f1.bolgeler[getEnGucluBolgemIndex(f1)].komsular.Contains(f1.bolgeler[Convert.ToInt32(gidilecekyol[gidilecekyol.Count - 1])]))
                        {
                            saldirmaDurumu = 3;
                            durum3SaldiriIndexleri.Add(getEnGucluBolgemIndex(f1));
                            durum3SaldiriIndexleri.Add(Convert.ToInt32(gidilecekyol[gidilecekyol.Count - 1]));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else//Biraz daha büyüyeceğiz...
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        bool buBolgeGuneyAmerikadaMi(Bolge b)
        {
            if (b.index == 9)
            {
                return true;
            }
            if (b.index == 10)
            {
                return true;
            }
            if (b.index == 11)
            {
                return true;
            }
            if (b.index == 12)
            {
                return true;
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

            if (southAmericaBendeMi(f1))
            {
                //Ordusu diğerlerinin toplamından buyuk olduğunda her tarafa saldıracak...
                if (getToplamDusmanSayisi(f1)/2 < f1.players[f1.turn].getTotalNumberOfArmies())
                {
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
                }
            }
            else//Güney Amerikayı ele geçirmeye çalışacak...
            {
                if (southAmericadaToprakVarMi(f1) != -1)
                {
                    foreach (Bolge b in f1.players[f1.turn].bolgeler)
                    {
                        if (getKomsuDusmanSayisi(b) > 0)
                        {
                            foreach (Bolge b2 in b.komsular)
                            {
                                if (b2.sahip != b.sahip)
                                {
                                    if (b.ordu.askerSayisi > b2.ordu.askerSayisi+10 && buBolgeGuneyAmerikadaMi(b2))
                                    {
                                        sonuc.Add(b.index);
                                        sonuc.Add(b2.index);
                                    }
                                }
                            }

                        }
                    }
                }
                else//Güney amerikayı ele geçirmeye çalışacak...
                {
                    if (saldirmaDurumu == 3)
                    {
                        saldirmaDurumu = 0;

                        return durum3SaldiriIndexleri;
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
