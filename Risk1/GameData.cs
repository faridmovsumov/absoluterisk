using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Risk1
{
    public class GameData
    {
        /// <summary>
        /// Oyundaki bölgelerin listesini tutar.
        /// </summary>
        public List<Bolge> bolgeler = new List<Bolge>();
        /// <summary>
        /// Oyunda bulunan oyuncuların listesini tutar.
        /// </summary>
        public List<Player> players = new List<Player>();
        /// <summary>
        /// Oyunda bulunan kıtaların listesini tutar.
        /// </summary>
        public List<Continent> kitalar = new List<Continent>();
        /// <summary>
        /// players listesindeki oyuncular arasında sıranın kimde olduğunu belirtir.
        /// </summary>
        public int turn;
        /// <summary>
        /// Ganimet bölgesinin index değerine bu değişken sayesinde erişilir.
        /// </summary>
        public int ganimetBolgesi;
        /// <summary>
        /// Ganimet bölgesinin oyuncuya kazandırdığı asker sayısıdır. Varsayılan değeri 1 dir.
        /// </summary>
        public int ganimetKazanci;

        /// <summary>
        /// Turn limit varsa bu değer true olacaktır
        /// </summary>
        public bool turnLimitVarMi;

        /// <summary>
        /// Turn limitin kaç olduğunu belirtir
        /// </summary>
        public int turnLimit;
    }
}
