using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ScottPlot.Plottable;
namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<List<Individ>> populations;
        List<Individ> now_population;

        private ScatterPlot MyScatterPlot;
        private MarkerPlot HighlightedPoint;
        private int LastHighlightedIndex = -1;

        private int flag = 0;

        public Form1()
        {
            InitializeComponent();

            formsPlot1.Hide();
            label5.Hide();
            label6.Hide();
            button2.Hide();
            button3.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Mathos.Parser.MathParser parser = new Mathos.Parser.MathParser();

            string expr = "(x1+(2*x2)/(1-x3))"; // the expression

            double result = 0; // the storage of the result

            parser.LocalVariables.Add("x1", 1); // 41 is the value of x
            parser.LocalVariables.Add("x2", 2); // 41 is the value of x
            parser.LocalVariables.Add("x3", 3); // 41 is the value of x

            result = parser.Parse(expr); // parsing

            MessageBox.Show(result.ToString());*/

            Generic generic = new Generic();
            double[] rez = generic.Count();

            formsPlot1.Show();
            label5.Show();
            label6.Show();
            button2.Show();
            button3.Show();

            double func = Math.Pow((rez[0] - 2), 2) + Math.Pow((rez[1] - 1), 2);
            label5.Text += "x1 = " + rez[0].ToString() + " ; x2 = " + rez[1].ToString() + " ; результат: " + Math.Round(func, 2).ToString();

            populations = generic.Return_populations();
            now_population = populations[flag];
            Plot();
            formsPlot1.Refresh();
        }

        private void Plot()
        {
            formsPlot1.Plot.Title(" 100 лучших особей из поколения " + (flag + 1) + " (из " + populations.Count + ")");
            var cmap = ScottPlot.Drawing.Colormap.Viridis;
            formsPlot1.Plot.AddColorbar(cmap);
            List<Individ> population = now_population;
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();

            for (int j = 0; j < 100; j++)
            {
                double x = population[j].x;
                double y = population[j].y;
                xs.Add(x);
                ys.Add(y);
            }

            MyScatterPlot = formsPlot1.Plot.AddScatterPoints(xs.ToArray(), ys.ToArray());

            for (int j = 0; j < 100; j++)
            {

                double func = population[j].Fitness_func();
                Color c;
                if ((xs[j] - 2 * ys[j] >= 1) && (xs[j] + ys[j] <= 3))
                    c = ScottPlot.Drawing.Colormap.Viridis.GetColor(func);
                else
                    c = Color.Red;

                formsPlot1.Plot.AddPoint(xs[j], ys[j], c);

            }

            HighlightedPoint = formsPlot1.Plot.AddPoint(0, 0);
            HighlightedPoint.Color = Color.Red;
            HighlightedPoint.MarkerSize = 10;
            HighlightedPoint.MarkerShape = ScottPlot.MarkerShape.openCircle;
            HighlightedPoint.IsVisible = false;

            var bubble = formsPlot1.Plot.AddBubblePlot();
            for (double i = 0.25; i <= 1; i += 0.25)
            {
                bubble.Add(x: 2, y: 1, radius: i, fillColor: Color.Transparent, edgeColor: ScottPlot.Drawing.Colormap.Viridis.GetColor(i), edgeWidth: 2);
            }

            bubble.RadiusIsPixels = false;
        }

        private double Return_fitness(int index)
        {
            return now_population[index].Fitness_func();
        }

        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MyScatterPlot != null)
            {
                // determine point nearest the cursor
                (double mouseCoordX, double mouseCoordY) = formsPlot1.GetMouseCoordinates();
                double xyRatio = formsPlot1.Plot.XAxis.Dims.PxPerUnit / formsPlot1.Plot.YAxis.Dims.PxPerUnit;
                (double pointX, double pointY, int pointIndex) = MyScatterPlot.GetPointNearest(mouseCoordX, mouseCoordY, xyRatio);

                // place the highlight over the point of interest
                HighlightedPoint.X = pointX;
                HighlightedPoint.Y = pointY;
                HighlightedPoint.IsVisible = true;

                // render if the highlighted point chnaged
                if (LastHighlightedIndex != pointIndex)
                {
                    LastHighlightedIndex = pointIndex;
                    formsPlot1.Render();
                }

                double fintess = Return_fitness(pointIndex);
                // update the GUI to describe the highlighted point
                label6.Text = $"Точка в ({pointX:N2}, {pointY:N2}); значение: ({fintess:N2})";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (flag > 0)
            {
                formsPlot1.Plot.Clear();
                flag--;
                now_population = populations[flag];
                Plot();
                formsPlot1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (flag < populations.Count - 1)
            {
                formsPlot1.Plot.Clear();
                flag++;
                now_population = populations[flag];
                Plot();
                formsPlot1.Refresh();

                if (flag == populations.Count - 1)
                {
                    var marker_find = formsPlot1.Plot.AddMarker(now_population[0].x, now_population[0].y, MarkerShape.filledTriangleDown);

                    marker_find.MarkerSize = 13;
                    marker_find.MarkerColor = ScottPlot.Drawing.Colormap.Viridis.GetColor(now_population[0].Fitness_func());
                    marker_find.Text = "MIN";
                    marker_find.TextFont.Color = Color.Black;
                    marker_find.TextFont.Alignment = Alignment.UpperCenter;
                    marker_find.TextFont.Size = 10;

                    var marker_extr = formsPlot1.Plot.AddMarker(2.2, 0.6, MarkerShape.filledTriangleUp);

                    marker_extr.MarkerSize = 13;
                    marker_extr.Text = "Extremum";
                    marker_extr.TextFont.Color = Color.Black;
                    marker_extr.TextFont.Alignment = Alignment.UpperCenter;
                    marker_extr.TextFont.Size = 10;

                    formsPlot1.Refresh();
                }
            }
        }
    }
}
