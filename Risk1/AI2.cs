using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    //Genel olarak avrupa kıtasını işgal etmeye çalışıyor 
    //Avrupadan bir yer ele geçirdiğinde taktiği devreye girer esasen
    //4 veya daha fazla oyuncuyla oynandığında etkili olur
    public class AI2 : ArtificalIntelligence
    {
        public AI2()
        {
            
        }
        public static int aiId = 2;
        public static string name = "Hitler";
        public static Image resim = Properties.Resources.hitler;

        public override string getName()
        {
            return name;
        }

        public int saldirmaDurumu = 0;//Saldırı türümüzü belirler
        public List<int> durum3SaldiriIndexleri = new List<int>(2);
        //Hitler mümkünse Avrupadan bir bölge seçecek
        public override int requestTerritory(GameData f1)
        {
            if (!f1.bolgeler[22].sahipli)
            {
                return 22;
            }
            if (!f1.bolgeler[20].sahipli)
            {
                return 20;
            }
            if (!f1.bolgeler[21].sahipli)
            {
                return 21;
            }
            if (!f1.bolgeler[19].sahipli)
            {
                return 19;
            }
            if (!f1.bolgeler[23].sahipli)
            {
                return 23;
            }
            if (!f1.bolgeler[24].sahipli)
            {
                return 24;
            }
            if (!f1.bolgeler[25].sahipli)
            {
                return 25;
            }

            return rastgeleBolgeSec(f1);
        }

        //ilk ordularını Avrupadan bir yere koymaya çalışır elinde yoksa avrupadan bir yer random seçer
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

            if (sahipoldugubolgeler.Contains(22))
            {
                return f1.bolgeler[22].index;
            }
            if (sahipoldugubolgeler.Contains(23))
            {
                return f1.bolgeler[23].index;
            }
            if (sahipoldugubolgeler.Contains(20))
            {
                return f1.bolgeler[20].index;
            }
            if (sahipoldugubolgeler.Contains(21))
            {
                return f1.bolgeler[21].index;
            }
            if (sahipoldugubolgeler.Contains(19))
            {
                return f1.bolgeler[19].index;
            }
            if (sahipoldugubolgeler.Contains(24))
            {
                return f1.bolgeler[24].index;
            }
            if (sahipoldugubolgeler.Contains(25))
            {
                return f1.bolgeler[25].index;
            }

            return f1.bolgeler[index].index;
        }

        public override List<int> getTerritoriesIndexToPlaceNewArmies(GameData f1)
        {
            Random r = new Random();
            List<int> gonderilecekler = new List<int>();
            
            int orduyuNereyeKoyalim=-1;
            
            //Avrupadan bir yeri varsa oraya koyar
            if (!avrupadaToprakVarMi(f1))
            {
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

            }
            else
            {
                List<int> sahipoldugubolgeler = new List<int>();
                List<int> orduyuKoyabilecegimizBolgeler = new List<int>();

                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    sahipoldugubolgeler.Add(b.index);
                }
                //Orduyu avrupada toprağımız varsa burada düşman olan herhangi bölgeye koyabilir...
                if (sahipoldugubolgeler.Contains(22) && getKomsuDusmanSayisi(f1.bolgeler[22]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(22);
                }
                if (sahipoldugubolgeler.Contains(23) && getKomsuDusmanSayisi(f1.bolgeler[23]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(23);
                }
                if (sahipoldugubolgeler.Contains(20) && getKomsuDusmanSayisi(f1.bolgeler[20]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(20);
                }
                if (sahipoldugubolgeler.Contains(21) && getKomsuDusmanSayisi(f1.bolgeler[21]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(21);
                }
                if (sahipoldugubolgeler.Contains(19) && getKomsuDusmanSayisi(f1.bolgeler[19]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(19);
                }
                if (sahipoldugubolgeler.Contains(24) && getKomsuDusmanSayisi(f1.bolgeler[24]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(24);
                }
                if (sahipoldugubolgeler.Contains(25) && getKomsuDusmanSayisi(f1.bolgeler[25]) > 0)
                {
                    orduyuKoyabilecegimizBolgeler.Add(25);
                }

                if (orduyuKoyabilecegimizBolgeler.Count != 0)
                {
                    int a = r.Next(0, orduyuKoyabilecegimizBolgeler.Count - 1);
                    orduyuNereyeKoyalim = orduyuKoyabilecegimizBolgeler[a];
                }
                else//Avrupanın zaten tamamı bizde ve etrafta komsulukta dusman yoksa
                {
                    List<int> etraftaDusmanOlanBolgelerim = new List<int>();
                    foreach (Bolge b in f1.players[f1.turn].bolgeler)
                    {
                        if (getKomsuDusmanSayisi(b) > 0)
                        {
                            etraftaDusmanOlanBolgelerim.Add(b.index);
                        }
                    }

                    int a = r.Next(0, etraftaDusmanOlanBolgelerim.Count - 1);
                    orduyuNereyeKoyalim = etraftaDusmanOlanBolgelerim[a];
                }
            }
            
            


            int k = f1.players[f1.turn].yeniAskerler;
            for (int i = 0; i < k; i++)
            {
                gonderilecekler.Add(orduyuNereyeKoyalim);
            }
            return gonderilecekler;
        }

        public bool avrupaBendeMi(GameData f1)
        {
            //19-25 arası 
            List<int> bolgeler=new List<int>();
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                bolgeler.Add(b.index);
            }

            if (!bolgeler.Contains(19))
            {
                return false;
            }
            if (!bolgeler.Contains(20))
            {
                return false;
            }
            if (!bolgeler.Contains(21))
            {
                return false;
            }
            if (!bolgeler.Contains(22))
            {
                return false;
            }
            if (!bolgeler.Contains(23))
            {
                return false;
            }
            if (!bolgeler.Contains(24))
            {
                return false;
            }
            if (!bolgeler.Contains(25))
            {
                return false;
            }

            return true;
        }


        bool avrupadaToprakVarMi(GameData f1)
        {
            List<int> bolgeler = new List<int>();
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                bolgeler.Add(b.index);
            }

            if (bolgeler.Contains(19))
            {
                return true;
            }
            if (bolgeler.Contains(20))
            {
                return true;
            }
            if (bolgeler.Contains(21))
            {
                return true;
            }
            if (bolgeler.Contains(22))
            {
                return true;
            }
            if (bolgeler.Contains(23))
            {
                return true;
            }
            if (bolgeler.Contains(24))
            {
                return true;
            }
            if (bolgeler.Contains(25))
            {
                return true;
            }

            return false;
        }

        int getAvrupadakiDusmanSayisi(GameData f1)
        {
            int toplamDusmanSayisi = 0;
            foreach (Bolge b in f1.kitalar[4].bolgeler)
            {
                if (b.sahip != f1.players[f1.turn])
                {
                    toplamDusmanSayisi += b.ordu.askerSayisi;
                }
            }

            return toplamDusmanSayisi;
        }


        int getAvrupadakiAskerSayisi(GameData f1)
        {
            int toplamAskerSayisi = 0;
            foreach (Bolge b in f1.kitalar[4].bolgeler)
            {
                if (b.sahip == f1.players[f1.turn])
                {
                    toplamAskerSayisi += b.ordu.askerSayisi;
                }
            }

            return toplamAskerSayisi;
        }

        public override bool devam(GameData f1)
        {
            durum3SaldiriIndexleri.Clear();
            saldirmaDurumu = 0;
            //19-25 arasını almak amaç öncelikle
            if (avrupaBendeMi(f1))//avrupa onda
            {
                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    foreach (Bolge komsu in b.komsular)
                    {
                        if (b.ordu.askerSayisi > komsu.ordu.askerSayisi + 10 && b.sahip != komsu.sahip)
                        {
                            saldirmaDurumu = 1;
                            return true;
                        }
                    }
                }
                return false;
            }
            else if (avrupadaToprakVarMi(f1))//avrupada toprağm var sadece
            {
                if (getAvrupadakiAskerSayisi(f1) > getAvrupadakiDusmanSayisi(f1))//Toplam Düşmandan daha fazlaysa Saldırıya geçer //Avrupaya karşı
                {
                    //Eğer Avrupada tek başına komsusundan güçlü olan varsa saldırırız
                    //Toplamımız daha büyük olabilir ama tek tek hiçbir bölgemiz daha güçlü olmayabilir
                    foreach (Bolge b in f1.players[f1.turn].bolgeler)
                    {
                        if (getKomsuDusmanSayisi(b) > 0)
                        {
                            foreach (Bolge b2 in b.komsular)
                            {
                                if (b2.sahip != b.sahip)
                                {
                                    if (b.ordu.askerSayisi > b2.ordu.askerSayisi && buBolgeAvrupadaMi(b2))
                                    {
                                        saldirmaDurumu = 2;
                                        return true;
                                    }
                                }
                            }

                        }
                    }

                    return false;
                }
                else//Düşman daha fazlaysa bekler devam etmez
                {
                    return false;
                }
            }
            else//Avrupayla alakamız yoksa Avrupaya ulaşmaya çalışacağız.
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

                String temp="";

                //Avrupa Sınırları
                List<int> avrupaSinirlari = new List<int>();
                avrupaSinirlari.Add(0);
                avrupaSinirlari.Add(0);
                avrupaSinirlari.Add(0);
                avrupaSinirlari.Add(0);

                foreach (var d in distances)
                {
                    temp+= d.Key+" - "+d.Value+"-";
                    if (d.Key == "19")
                    {
                        avrupaSinirlari[0] =(int)d.Value;
                    }
                    if (d.Key == "21")
                    {
                        avrupaSinirlari[1] = (int)d.Value;
                    }
                    if (d.Key == "24")
                    {
                        avrupaSinirlari[2] = (int)d.Value;
                    }
                    if (d.Key == "25")
                    {
                        avrupaSinirlari[3] = (int)d.Value;
                    }
                }

                int min = avrupaSinirlari.Min();
                int minIndex = avrupaSinirlari.IndexOf(min);


                int minBolgeIndex = 0;
                if (minIndex == 0)
                {
                    minBolgeIndex = 19;
                }
                if (minIndex == 1)
                {
                    minBolgeIndex = 21;
                }
                if (minIndex == 2)
                {
                    minBolgeIndex = 24;
                }
                if (minIndex == 3)
                {
                    minBolgeIndex = 25;
                }

                List<String> gidilecekyol = new List<String>();

                //calculator = new DistanceCalculator();

                gidilecekyol = calculator.getYol(getEnGucluBolgemIndex(f1).ToString(),minBolgeIndex.ToString(), graph);


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
        }

        


        bool buBolgeAvrupadaMi(Bolge b)
        {
            if (b.index == 19)
            {
                return true;
            }
            if (b.index == 20)
            {
                return true;
            }
            if (b.index == 21)
            {
                return true;
            }
            if (b.index == 22)
            {
                return true;
            }
            if (b.index == 23)
            {
                return true;
            }
            if (b.index == 24)
            {
                return true;
            }
            if (b.index == 25)
            {
                return true;
            }

            return false;
        }

        public override List<int> getSavasBolgeIndexleri(GameData f1)
        {
            List<int> savasBolgeleri = new List<int>();
            //19-25 arasını almak amaç öncelikle
            
            //if saldiriDurumu ==1 yani avrupanın bende olma durumu için kod sonra yazılacak
            if (saldirmaDurumu == 1)
            {
                List<int> sonuc = new List<int>();

                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    foreach (Bolge komsu in b.komsular)
                    {
                        if (b.ordu.askerSayisi > komsu.ordu.askerSayisi + 10 && b.sahip != komsu.sahip)
                        {
                            sonuc.Add(b.index);
                            sonuc.Add(komsu.index);
                        }
                    }
                }
                return sonuc;
            }
            else if (saldirmaDurumu==2)//avrupada toprağm var sadece
            {
                bool break2 = false;
                foreach (Bolge b in f1.players[f1.turn].bolgeler)
                {
                    if (getKomsuDusmanSayisi(b) > 0)
                    {
                        foreach (Bolge b2 in b.komsular)
                        {
                            if (b2.sahip != b.sahip)
                            {
                                if (b.ordu.askerSayisi > b2.ordu.askerSayisi  && buBolgeAvrupadaMi(b2))
                                {
                                    savasBolgeleri.Add(b.index);
                                    savasBolgeleri.Add(b2.index);
                                    break2 = true;
                                    break;
                                }
                            }
                        }
                        
                    }
                    if (break2) break;
                }
                return savasBolgeleri;
            }
            else if (saldirmaDurumu == 3)//Avrupayla alakamız yoksa...
            {
                return durum3SaldiriIndexleri;
            }
            else
            {
                //oylesine
                return savasBolgeleri;
            }
        }

        public override bool saldiracanMi(GameData f1)
        {
            return devam(f1);
        }

        

        int getAvrupadakiToprakSayisi(GameData f1)
        {
            //19-25
            int sonuc = 0;
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                if (buBolgeAvrupadaMi(b))
                {
                    sonuc++;
                }
            }
            return sonuc;
        }

        public int getDahaGucluDusmanSayisi(Bolge b)
        {
            int toplam = 0;
            foreach (Bolge bb in b.komsular)
            {
                if (bb.sahip != b.sahip && b.ordu.askerSayisi < bb.ordu.askerSayisi)
                {
                    toplam++;
                }
            }
            return toplam;
        }


        public override List<int> divideArmies(int index1, int index2,GameData f1)
        {
            List<int> sonuc = new List<int>();
            
            if (getAvrupadakiToprakSayisi(f1)>1)
            {
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
            }
            else
            {
                int toplam = f1.bolgeler[index1].ordu.askerSayisi + f1.bolgeler[index2].ordu.askerSayisi;

                sonuc.Add(1);
                sonuc.Add(toplam - 1);
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