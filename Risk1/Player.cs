using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    public class Player
    {
        /// <summary>
        /// Kullanıcının adını tutar.
        /// </summary>
        public String name;
        public Color color;
        /// <summary>
        /// Kullanıcının insan mı yapay zeka mı olduğu bilgisini tutar.
        /// </summary>
        public bool isHuman;
        /// <summary>
        /// Oyuncunun aktif olup olmadığı bilgisini tutar.
        /// </summary>
        public bool isActive;
        /// <summary>
        /// Kullanıcının sahip olduğu bölgelerin listesidir.
        /// </summary>
        public List<Bolge> bolgeler=new List<Bolge>();
        /// <summary>
        /// Her döngüde oyuncuya gelen asker sayısını belirtir. 
        /// </summary>
        public int yeniAskerler=0;
        /// <summary>
        /// Kullanıcıya oyun başladığında verilen asker sayısını belirtir.
        /// </summary>
        public int ilkAskerSayisi;
        /// <summary>
        /// Kullanıcının sahşp olduğu kıtaların listesidir.
        /// </summary>
        public String kitalar;
        /// <summary>
        /// Oyuncu yapay zekaysa onun ID sini belirten değişkendir. 
        /// Her kullanıcı için farklı değer almak zorundadır.
        /// </summary>
        public int aiId;
        /// <summary>
        /// Oyunda oyuncu bir önceki döngüde bir oyuncuyu elemişse döngü sonunda bu değer 'true' değerini alır.
        /// </summary>
        public bool buTurnBirisiniOldurduMu = false;
       
        public int kacKisiOldurdu = 0;
        /// <summary>
        /// Oyunda oyuncunun 'buTurnBirisiniOldurduMu' değişkeni 'true' değerini almışsa kullanıcının kaç kişiyi öldürdüğü değeri tutulur.
        /// </summary>
        public int buTurnKacKisiOldurdu = 0;
        public int number;
        /// <summary>
        /// İstatiksel bilgilerde kullanılmaktadır.
        /// </summary>
        public List<int> askerSayisiTarihi = new List<int>();



        public List<int> OptimizeAskerSayisiTarihi()
        {
            List<int> askerSayisi = new List<int>();

            if (askerSayisiTarihi.Count > 40)
            {
                int k = 1;
                int toplam = 0;
                int eklenecekSayi;
                foreach (int i in askerSayisiTarihi)
                {
                    toplam += i;
                    
                    if ((k % 10)==0)
                    {
                        eklenecekSayi=toplam/10;
                        askerSayisi.Add(eklenecekSayi);
                        toplam = 0;
                    }
                    k++;
                }

                return askerSayisi;
            }
            else
            {
                return askerSayisiTarihi;
            }

        }


        public Player(){}

        public Player(string n, bool human, bool active)
        {
            this.name = n;
            isHuman = human;
            isActive = active;
        }

        /// <summary>
        /// Oyuncunun toplam asker sayını döndürür.
        /// </summary>
        /// <returns>Toplam Asker Sayısı</returns>
        public int getTotalNumberOfArmies()
        {
            int toplam = 0;
            foreach (Bolge b in bolgeler)
            {
                toplam += b.ordu.askerSayisi;
            }

            return toplam;
        }
        /// <summary>
        /// Oyuncunun toplam bölge sayısını döndürür.
        /// </summary>
        /// <returns>Toplam Bölge Sayısı</returns>
        public int getNumberOfTerritories()
        {
            return bolgeler.Count;
        }

        /// <summary>
        /// Oyunucunun sahip olduğu kıta sayısını döndürür.
        /// </summary>
        /// <returns>Toplam Kıta Sayısı</returns>
        public String getSahipOlduguKitalar()
        {
            return kitalar;
        }

        
    }
}
