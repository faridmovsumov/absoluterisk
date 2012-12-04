using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Risk1
{
    public partial class Chart : Form
    {
        Form1 f1;
        public Chart(Form1 form1)
        {
            InitializeComponent();
            f1 = form1;
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            List<int> tumSayilar = new List<int>();
            foreach (Player p in f1.players)
            {
                foreach (int i in p.OptimizeAskerSayisiTarihi())
                {
                    tumSayilar.Add(i);
                }
            }

            chart1.ChartAreas.Add("area");
            chart1.ChartAreas["area"].AxisX.Minimum = 1;
            chart1.ChartAreas["area"].AxisX.Maximum = f1.players[f1.turn].OptimizeAskerSayisiTarihi().Count;
            chart1.ChartAreas["area"].AxisX.Interval = 1;

            chart1.ChartAreas["area"].AxisY.Minimum = 0;
            chart1.ChartAreas["area"].AxisY.Maximum = tumSayilar.Max();
            chart1.ChartAreas["area"].AxisY.Interval = 10;


            foreach (Player p in f1.players)
            {
                chart1.Series.Add(p.number.ToString());

                chart1.Series[p.number.ToString()].Color = p.color;

                chart1.Series[p.number.ToString()].ChartType = SeriesChartType.Line;

                

                int k = 1;
                foreach (int i in p.OptimizeAskerSayisiTarihi())
                {
                    chart1.Series[p.number.ToString()].Points.AddXY(k, i);
                    k++;
                }
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}