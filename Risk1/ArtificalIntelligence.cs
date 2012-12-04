using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk1
{
    public abstract class ArtificalIntelligence
    {
        /// <summary>
        /// Bu metod Yapay zekanın ismini döndürmelidir.
        /// </summary>
        /// <returns>name</returns>
        abstract public string getName();

        /// <summary>
        /// Bu metod bölgeler seçilirken istenen bolgenin indexini dondurmektedir.
        /// </summary>
        /// <param name="form1"></param>
        /// <returns>istenen bolgenin indexi</returns>
        abstract public int requestTerritory(GameData form1);

        /// <summary>
        /// İlk başta tüm oyunculara verilen ilk askerler dağıtılırken her oyuncuya tek tek
        /// askerlerini nereye koyması gerektiği sorulur bu metod ilk ordunun koyulması gereken bolgenin indexini dondurur
        /// </summary>
        /// <param name="form1"></param>
        /// <returns>ilk ordunun koyulması gereken bolgenin indexini dondurur</returns>
        abstract public int getTerritoryToPlaceFirstArmies(GameData form1);

        /// <summary>
        /// Oyuncu her turn'de kazandığı yeni askerleri hangi
        /// bolgelere yerleştireceğini bu metod sayesinde belirler.
        /// </summary>
        /// <param name="form1"></param>
        /// <returns>Askerlerin yerleştirilmek istendiği bolge indexleri</returns>
        abstract public List<int> getTerritoriesIndexToPlaceNewArmies(GameData form1);

        /// <summary>
        /// Kullanıcının hamle yapmaya devam edip etmeyeceğini belirler.
        /// </summary>
        /// <param name="form1"></param>
        /// <returns>Devam etmek istiyorsa true devam etmek istemiyorsa false.</returns>
        abstract public bool devam(GameData form1);

        /// <summary>
        /// Bu metod kullanıcının saldırı yapmak isteyip istemediğini belirler
        /// </summary>
        /// <param name="form1"></param>
        /// <returns>Eğer saldırı yapmak istiyorsa true istemiyorsa false döndürür.</returns>
        abstract public bool saldiracanMi(GameData form1);

        /// <summary>
        /// Bu metod savaş yapılacak bolgelerin indexlerini belirlemek içindir.
        /// </summary>
        /// <returns>Metod geriye iki elemanlı bir liste döndürür.
        /// Birinci eleman nereden saldırı yapılacağını ikinci eleman ise nereye saldırı yapılacağını gösterir.
        /// Burada iki bolgenin komsu olmasına dikkat edilmelidir.saldırılar varsayılan olarak Doordie şeklinde yapılacaktır. 
        /// Listenin 3. elemanı -1 olarak gönderilirse saldırı attack şeklinde yapılacaktır </returns>
        abstract public List<int> getSavasBolgeIndexleri(GameData form1);

        /// <summary>
        /// Oyuncu bir bölgeyi igal ettikten sonra o bölge ile kendi bölgesi arasındaki
        /// askerleri nasıl paylaştıracağına bu metod vasıtasıyla karar verir.
        /// </summary>
        /// <param name="index1">Saldırının gerçekleştiği bölgenin index'i</param>
        /// <param name="index2">İşgal edilen bölgenin index'i</param>
        /// <returns>Geriye iki elemanlı bir liste döndürülecek birinci eleman index1 ile belirttiğimiz
        /// bolgede kalacak asker sayısını ikinci eleman ise index2
        /// ile belirttiğimiz bolgede kalacak eleman sayısını gostermelidir.</returns>
        abstract public List<int> divideArmies(int index1, int index2, GameData form1);

        abstract public bool askerTransferiYapacakmisin(GameData form1);

        abstract public List<int> askerTransferiYap(GameData form1);

        public int getKomsuDusmanSayisi(Bolge b)
        {
            int toplam = 0;
            foreach (Bolge bb in b.komsular)
            {
                if (bb.sahip != b.sahip)
                {
                    toplam++;
                }
            }
            return toplam;
        }


        public int getDostSayisi(Bolge b)
        {
            int toplam = 0;
            foreach (Bolge bb in b.komsular)
            {
                if (bb.sahip == b.sahip)
                {
                    toplam++;
                }
            }
            return toplam;
        }

        /// <summary>
        /// Bölgelerden sahipsiz olanlarından rastgele birisinin indexini döndürür.
        /// </summary>
        /// <param name="f1">Oyun Verisi</param>
        /// <returns>Rastgele Seçilen Bölge İndexi</returns>
        public int rastgeleBolgeSec(GameData f1)
        {
            List<int> sahipsizBolgeIndexleri = new List<int>();

            foreach (Bolge b in f1.bolgeler)
            {
                if (!b.sahipli)
                {
                    sahipsizBolgeIndexleri.Add(b.index);
                }
            }

            Random r = new Random();

            return sahipsizBolgeIndexleri[r.Next(0, sahipsizBolgeIndexleri.Count - 1)];
        }

        /// <summary>
        /// Oyuncunun sahip olduğu bölgeler içinden en fazla sayıda asker bulunduran bölgenin indexini gönderir.
        /// </summary>
        /// <param name="f1">Oyun Verisi</param>
        /// <returns>En Güçlü Bölge İndexi</returns>
        public int getEnGucluBolgemIndex(GameData f1)
        {
            int max = 0;
            Bolge bolge = null;
            foreach (Bolge b in f1.players[f1.turn].bolgeler)
            {
                if (b.ordu.askerSayisi >= max)
                {
                    max = b.ordu.askerSayisi;
                    bolge = b;
                }
            }
            return bolge.index;
        }

        /// <summary>
        /// Oyuncunun bölgeleri içinden rastgele seçilen bölgenin indexini gönderir.
        /// </summary>
        /// <param name="f1">Oyun Verisi</param>
        /// <returns>Rastgele Bölge İndexi</returns>
        public int bolgelerimdenRastgeleSec(GameData f1)
        {
            List<int> benimBolgeIndexleri = new List<int>();

            foreach (Bolge b in f1.bolgeler)
            {
                if (b.sahip==f1.players[f1.turn])
                {
                    benimBolgeIndexleri.Add(b.index);
                }
            }

            Random r = new Random();

            return benimBolgeIndexleri[r.Next(0, benimBolgeIndexleri.Count - 1)];
        }

        /// <summary>
        /// Oyunda bulunan bütün düşmanların toplam asker sayısını döndürür.
        /// </summary>
        /// <param name="f1">Oyun Verisi</param>
        /// <returns>Oyundaki Toplam Düşman Asker Sayısı</returns>
        public int getToplamDusmanSayisi(GameData f1)
        {
            int toplam = 0;
            foreach (Player p in f1.players)
            {
                if (p != f1.players[f1.turn])
                {
                    toplam += p.getTotalNumberOfArmies();
                }
            }

            return toplam;
        }
    }
}