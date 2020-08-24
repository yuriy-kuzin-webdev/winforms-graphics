using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawDiagrams
{
    public partial class SecondDatasetForm : Form
    {
        #region testdata1
        List<DataItem> dataitems = new List<DataItem>()
            {
            new DataItem(){Name = "Yato", April = 115000, May = 90000F, June = 200000F, July = 250000},
            new DataItem(){Name = "Vorel", April = 180000, May = 120000F, June = 190000F , July = 300000},
            new DataItem(){Name = "Topex", April = 30000, May = 10000, June = 5000F, July = 1000},
            new DataItem(){Name = "NWS", April = 50000, May = 50000F, June = 35000F, July = 80000},
            new DataItem(){Name = "Virok", April = 240000, May = 180000F, June = 90000F, July = 120000},
            new DataItem(){Name = "USH", April = 75000, May = 50000F, June = 125000F, July = 250000},
            };
        Dictionary<string, Color> propertiesColors = new Dictionary<string, Color>()
            {
                {"Name",Color.Blue},
                {"April",Color.FromArgb(119,191,92)},
                {"May",Color.FromArgb(178,209,53)},
                {"June",Color.FromArgb(254,206,0)},
                {"July", Color.FromArgb(252,170,24) }
            };
        #endregion testdata1
        public SecondDatasetForm()
        {
            InitializeComponent();
        }

        private void SecondDatasetForm_Paint(object sender, PaintEventArgs e)
        {
            Diagrams.SetScaleAndStep(dataitems, propertiesColors);
            e.DrawStackHistogram(dataitems, propertiesColors, "Sales per month");
        }
    }
    class DataItem
    {
        public string Name { set; get; }
        public int April { set; get; }
        public float May { set; get; }
        public double June { set; get; }
        public int July { set; get; }
    }
}

