using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace DrawDiagrams
{
    public static class Diagrams
    {
        public static void DrawStackHistogram<TList>(this PaintEventArgs e, List<TList> list, Dictionary<string,Color> propColor, string infotitle)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.DrawBorder();
            e.DrawBackGroundLines();
            e.DrawVerticalAxis();
            e.DrawMetrics();
            e.DrawBarChart(list, propColor);
            e.DrawBarChartInfo(propColor, infotitle);
        }
        public static void SetScaleAndStep<TList>(List<TList> list, Dictionary<string, Color> propColor)
        {
            Type T = typeof(TList);
            PropertyInfo[] props = propColor.Skip(1).Select(name => T.GetProperty(name.Key)).ToArray();
            float maxsize = 0F;
            list.ForEach(item =>
            {
                float itemsize = 0F;
                foreach (PropertyInfo prop in props)
                    itemsize += (float)Convert.ToDouble(prop.GetValue(item));

                if (itemsize > maxsize)
                    maxsize = itemsize;
            });
            Diagrams.Step = maxsize / (DiagramArea.Height / 15);
            Diagrams.Step = Steps.First(st => (Convert.ToInt32(Diagrams.Step)) / st == 1);
            Diagrams.Scale = DiagramArea.Height / maxsize;
        }
        public static void DrawBarChartInfo(this PaintEventArgs e, IDictionary<string,Color> propertiesColors,string infoTitle)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(infoTitle, Font, Brush, InfoArea);
            PointF start = new PointF(InfoArea.X, InfoArea.Y + InfoVerticalPadding);
            foreach( KeyValuePair<string,Color> keyValue in propertiesColors.Skip(1).Reverse())
            {
                e.Graphics.FillEllipse(new SolidBrush(keyValue.Value), start.X - 4, start.Y - 4, 8, 8);
                e.Graphics.DrawString(keyValue.Key, Font, Brush, start.X + 15, start.Y, format);
                start.Y += Font.Height * 2;
            }
        }
        public static void DrawBarChart<TList>(this PaintEventArgs e, List<TList> list, Dictionary<string,Color> propertiesColors)
        {
            #region PrepareData
            Type T = typeof(TList);
            PropertyInfo[] props = propertiesColors.Skip(1).Select(name => T.GetProperty(name.Key)).ToArray();
            Color[] colors = propertiesColors.Skip(1).Select(item => item.Value).ToArray();
            PropertyInfo columnNamePropery = T.GetProperty(propertiesColors.First().Key);
            //Растояние для колонок
            float columnWidth = DiagramArea.Width / list.Count;

            //Ареа для одного колумна
            RectangleF columnArea = new RectangleF(
                DiagramArea.X + Spacing,
                DiagramArea.Y,
                columnWidth - 2 * Spacing,
                DiagramArea.Height);
            //Ареа рисование тайтла колумна
            RectangleF columnTextArea = new RectangleF(
                DiagramArea.X + Spacing,
                DiagramArea.Y + DiagramArea.Height - Font.Height,
                columnWidth - 2 * Spacing,
                Font.Height);
            #endregion PrepareData
            #region DrawingData
            //Проход по всей коллекции значений
            list.ForEach( item =>
            {
                float valueHeight = 0F;
                float filledHeight = 0F;
                //Проход по свойствам для отображения
                for (int i = 0; i < props.Count(); ++i)
                {
                    //Высота столбца для одного из полей
                    valueHeight = (float)Convert.ToDouble(props[i].GetValue(item)) * Scale;
                    //Отрисовка части колонки
                    e.Graphics.FillRectangle(new SolidBrush(colors[i]),
                        columnArea.X,
                        columnArea.Y + columnArea.Height - valueHeight - filledHeight,
                        columnArea.Width,
                        valueHeight
                        );

                    filledHeight += valueHeight;
                }
                
                //Подпись колонки
                e.Graphics.DrawString(columnNamePropery.GetValue(item).ToString(), Font, Brush, columnTextArea , StringFormat);

                //Cмещение текста и колонки
                columnTextArea.X += columnWidth;
                columnArea.X += columnWidth;
            });
            #endregion DrawingData
        }
        public static void DrawBorder(this PaintEventArgs e)
            => e.Graphics.DrawRectangles(Pen, new[]{ Border});
        public static void DrawVerticalAxis(this PaintEventArgs e) 
            => e.Graphics.DrawLine(Pen,
                DiagramArea.X, 
                DiagramArea.Y, 
                DiagramArea.X,
                DiagramArea.Y + DiagramArea.Height);
        public static void DrawMetrics(this PaintEventArgs e)
        {
            float text = Step;
            PointF start = new PointF(DiagramArea.X, DiagramArea.Y + DiagramArea.Height - Step * Scale);
            while (start.Y > DiagramArea.Y)
            {
                //Отрисовка дот и текста для метрики
                e.Graphics.FillEllipse(Brush, start.X - 1, start.Y - 1, 2, 2);
                e.Graphics.DrawString(text.ToString(), new Font("Arial", 8.25F),
                    Brush, start.X - Padding.Width/2,start.Y, StringFormat);
                start.Y -= Step * Scale;
                text += Step;
            }
        }
        public static void DrawBackGroundLines(this PaintEventArgs e)
        {
            PointF start = new PointF(DiagramArea.X, DiagramArea.Y + DiagramArea.Height);
            while (start.Y > DiagramArea.Y)
            { 
                e.Graphics.DrawLine(Pen, start.X, start.Y, start.X + DiagramArea.Width, start.Y);
                start.Y -= Step * Scale;
            }
        }

        #region configurations
        public static Pen Pen;
        public static Brush Brush;
        public static SizeF Margin;
        public static SizeF Padding;
        public static RectangleF Area;
        public static RectangleF Border;
        public static RectangleF DiagramArea;
        public static RectangleF InfoArea;
        public static float Step;
        public static float Spacing;
        public static Font Font;
        public static StringFormat StringFormat;
        public static float Scale;
        public static float InfoVerticalPadding;
        #endregion configurations
        //list of human readable metrics
        private static readonly List<int> Steps = new List<int>()
        {
            5,10,25,30,50,75,100,125,150,200,250,500,750,1000,1500,3000,5000,10000,15000,30000,50000,100000
        };
        static Diagrams()
        {
            StringFormat = new StringFormat();
            StringFormat.Alignment = StringAlignment.Center;
            StringFormat.LineAlignment = StringAlignment.Center;
            //default configuration
            //Rectangle clientSize = (sender as Control).ClientRectangle;
            Rectangle clientSize = new Rectangle(0, 0, 800, 450);
            Diagrams.Area = new RectangleF(clientSize.X, clientSize.Y, clientSize.Width, clientSize.Height);
            Diagrams.Margin = new SizeF(20F, 15F);
            Diagrams.Padding = new SizeF(40F, 20F);
            Diagrams.Border = Diagrams.Area.Shrink(Diagrams.Margin);
            Diagrams.DiagramArea = Diagrams.Border.Shrink(Diagrams.Padding);
            Diagrams.DiagramArea.Width -= 120;
            Diagrams.Pen = new Pen(Color.FromArgb(20, 0, 0, 0), 2F);
            Diagrams.Spacing = 15F;
            Diagrams.Brush = Brushes.Black;
            Diagrams.InfoArea = new RectangleF(
                Diagrams.DiagramArea.X + Diagrams.DiagramArea.Width,
                Diagrams.DiagramArea.Y,
                120F + Diagrams.Padding.Width / 2,
                Diagrams.DiagramArea.Height).Shrink(Diagrams.Margin);
            Diagrams.Font = new Font("Tascoma", 9.25F);
            Diagrams.InfoVerticalPadding = Font.Height * 4;
            Diagrams.InfoArea.X += 40;
            Diagrams.Step = 10F;
            Diagrams.Scale = 1F;
        }
        public static RectangleF Shrink(this RectangleF rect, SizeF size)
            => new RectangleF(
                rect.X + size.Width,
                rect.Y + size.Height,
                rect.Width - 2 * size.Width,
                rect.Height - 2 * size.Height);
    }
}
