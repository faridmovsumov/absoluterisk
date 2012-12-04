using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk1
{
    class DefaultAi : ArtificalIntelligence
    {

        public static string name = "Default AI";


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
            return bolgelerimdenRastgeleSec(f1);
        }

        public override List<int> getTerritoriesIndexToPlaceNewArmies(GameData f1)
        {
            List<int> gonderilecekler = new List<int>();

            int orduyuNereyeKoyalim = bolgelerimdenRastgeleSec(f1);

            int k=f1.players[f1.turn].yeniAskerler;

            for (int i = 0; i <k ; i++)
            {
                gonderilecekler.Add(orduyuNereyeKoyalim);
            }

            return gonderilecekler;
        }

        public override bool devam(GameData f1)
        {
            return false;
        }

        public override bool saldiracanMi(GameData f1)
        {
            return devam(f1);
        }

        //Ne olduğunun önemi yok saldırı yapılmayacak
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
            return sonuc;
        }

        public override List<int> divideArmies(int index1, int index2,GameData f1)
        {
            List<int> sonuc = new List<int>();

            int toplam = f1.bolgeler[index1].ordu.askerSayisi + f1.bolgeler[index2].ordu.askerSayisi;

            sonuc.Add(toplam - 1);
            sonuc.Add(1);

            return sonuc;
        }

        public override bool askerTransferiYapacakmisin(GameData f1)
        {
            return false;
        }


        //Ne olduğu önemli değil çalıştırılmayacak
        public override List<int> askerTransferiYap(GameData f1)
        {

            List<int> veriler = new List<int>();

            return veriler;
        }
    }
}
