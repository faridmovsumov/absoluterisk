using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Risk1
{
    public class Bolge
    {
        public Bolge(List<Nokta> nok,String ad)
        {
            this.noktalar = nok;
            this.isim = ad;
        }

        public Bolge(){}

        public List<Nokta> noktalar = new List<Nokta>();
        /// <summary>
        /// Bölgenin ismini döndürür.
        /// </summary>
        public String isim="";
        /// <summary>
        /// Komşu bölgelerin listesini tutar.
        /// </summary>
        public List<Bolge> komsular = new List<Bolge>();
        /// <summary>
        /// Bölgenin sahipli olup olmadığını belirtir.
        /// </summary>
        public bool sahipli = false;
        /// <summary>
        /// Bölgenin sahibi olan oyuncuyu döndürür.
        /// </summary>
        public Player sahip;
        /// <summary>
        /// Asker sayısına Erişmek için kullanılır.
        /// </summary>
        public Ordu ordu=new Ordu();
        /// <summary>
        /// Bölgenin numarasını belirtir
        /// </summary>
        public int number;
        /// <summary>
        /// Bölgenin listedeki yerini belirtir.
        /// </summary>
        public int index;


        public Color getColor()
        {
            if (noktalar.Count > 0)
                return noktalar[0].renk;
            else
                return Color.SaddleBrown;
        }

        
    }
}
