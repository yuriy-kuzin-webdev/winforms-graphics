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
    public partial class FirstDataSetForm : Form
    {
        public FirstDataSetForm()
        {
            InitializeComponent();
        }

        private void firstDataSetForm_Paint(object sender, PaintEventArgs e)
        {
            var testdata = new[]
{
                new { Country = "UA", Jan = 22, Feb = 12, Mar = 31, Apr = 10, May = 10, Jun = 18, Jul = 19, Aug = 9, Sept = 5, Oct = 3, Nov = 7, Dec = 3},
                new { Country = "GB", Jan = 15, Feb = 10, Mar = 20, Apr = 9, May = 9, Jun = 21, Jul = 11, Aug = 19, Sept = 1, Oct = 4, Nov = 8, Dec = 4},
                new { Country = "HU", Jan = 17, Feb = 11, Mar = 25, Apr = 12, May = 8, Jun = 10, Jul = 17, Aug = 15, Sept = 7, Oct = 7, Nov = 7, Dec = 10},
                new { Country = "BE", Jan = 10, Feb = 7, Mar = 18, Apr = 7, May = 5, Jun = 15, Jul = 21, Aug = 6, Sept = 10, Oct = 10, Nov = 3, Dec = 20},
                new { Country = "KZ", Jan = 19, Feb = 15, Mar = 18, Apr = 20, May = 13, Jun = 18, Jul = 10, Aug = 2, Sept = 2, Oct = 14, Nov = 11, Dec = 8},
                new { Country = "MK", Jan = 13, Feb = 9, Mar = 11, Apr = 25, May = 25, Jun = 22, Jul = 9, Aug = 8, Sept = 8, Oct = 4, Nov = 15, Dec = 1},
            }.ToList();
            Dictionary<string, Color> testdataDict = new Dictionary<string, Color>()
            {
                { "Country", Color.Black },
                { "Jan", Color.FromArgb(86,129,197) },
                { "Feb", Color.FromArgb(1,174,234) },
                { "Mar", Color.FromArgb(82,177,145) },
                { "Apr", Color.FromArgb(119,191,92) },
                { "May", Color.FromArgb(178,209,53) },
                { "Jun", Color.FromArgb(254,206,0) },
                { "Jul", Color.FromArgb(252,170,24) },
                { "Aug", Color.FromArgb(244,134,73) },
                { "Sept", Color.FromArgb(244,112,89) },
                { "Oct", Color.FromArgb(238,65,129) },
                { "Nov", Color.FromArgb(207,82,158) },
                { "Dec", Color.FromArgb(62,106,179) },
            };


            Diagrams.SetScaleAndStep(testdata, testdataDict);
            e.DrawStackHistogram(testdata, testdataDict, "Times per month :");
        }
    }
}
