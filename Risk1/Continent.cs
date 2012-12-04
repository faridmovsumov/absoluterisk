using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk1
{
    public class Continent
    {
        public Continent(string n, int g)
        {
            this.name = n;
            this.getirisi = g;
        }

        /// <summary>
        /// Kıtanın adının tutulduğu değişkendir.
        /// </summary>
        public String name;
        /// <summary>
        /// Kıtada bulunan bölgelerin listesinin tutulduğu değişkendir.
        /// </summary>
        public List<Bolge> bolgeler=new List<Bolge>();
        /// <summary>
        /// Kıtanın bütün bölgelerine de sahip olan oyuncunun her döngüde kazanacağı extra asker sayısını ifade eder.
        /// </summary>
        public int getirisi;
    }
}
