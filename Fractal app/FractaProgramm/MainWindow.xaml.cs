using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FractaProgramm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] FractalsList { get; }
        public int Depth
        {
            get
            {
                int.TryParse(FractalDepthTextBox.Text, out int depth);
                return depth;
            }
        }
        public int FractalIndex => FractalTypeComboBox.SelectedIndex;
        private TreeFractal treeFractal;
        private KochFractal kochFractal;

        //TextBlock helpblock = new TextBlock();
        private string readMeText = "Выберите фрактал и нажмите отрисовать, чтобы нарисоваь его. Максимальная глубина фрактала 8";
        private Color myGreen = Color.FromRgb(120, 180, 60);
        private Color myRed = Color.FromRgb(220, 50, 50);
        private Color myGrey = Color.FromRgb(220, 50, 50);
        private int recursionDepthLimit = 8;

        public MainWindow()
        {
            InitializeComponent();

            FractalsList = new string[] { "Помощь и выбор фрактала", "Обдуваемое ветром фрактальное дерево", "Кривая Коха", "Ковер Серпинского", "Треугольник Серпинского", "Множество Кантора" };
            DataContext = this;
            helpBlock.Text = readMeText;
            treeFractal = new TreeFractal(CanvasForFractal, 30, 0.5);
            kochFractal = new KochFractal(CanvasForFractal);

        }

        private void FractalTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //TODO

        private void ToggleOptions()
        {
            throw new NotImplementedException();
            //if (FractalTypeComboBox.SelectedIndex == 1)
            //{

            //}
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOptionsCorrect())
            {
                PaintImage();
            }
            else
            {
                OptionsTrobleshoot();
            }
        }

        /// <summary>
        /// Отрисовывет фрактал
        /// </summary>
        private void PaintImage()
        {
            if (IsOptionsCorrect())
            {
                PaintFractal();
            }
        }

        private void OptionsTrobleshoot()
        {
            string message = "Неправильно указаны параметры, они выделены красным\n Допустимые значения записаны в разделе Помощь и выбор фрактала ";
            
            MessageBox.Show(message);
        }

        private void PaintFractal()
        {
            int.TryParse(FractalTypeComboBox.Text, out int itterations);
            if (!(CanvasForFractal.Children is null))
            {
                CanvasForFractal.Children.Clear();
            }
            
            helpBlock.Visibility = Visibility.Hidden;
            switch (FractalTypeComboBox.SelectedIndex)
            {
                case 0:
                    PrintHelpScreen();
                    break;
                case 1:
                    treeFractal.Draw(itterations);
                    break;
                case 2:
                    kochFractal.Draw(itterations);
                    break;
                default:
                    break;
            }

        }

        private void PrintKochCurve(Point p1, Point p5, int depth)
        {
            if (depth == 0)
            {
                CanvasForFractal.Children.Add(LineCreate(p1, p5, Brushes.Black));
            }
            else
            {

                //double cos60 = 0.5;/*Math.Cos(Math.PI/3);*/
                //double sin60 = Math.Sin(-Math.PI / 3);/*-0.866;1 - cos60;*/
                //Point p2 = new Point(p1.X + (p5.X - p1.X) / 3, p1.Y + (p5.Y - p1.Y) / 3);
                //Point p4 = new Point(p1.X + 2 * (p5.X - p1.X) / 3, p1.Y + 2 * (p5.Y - p1.Y) / 3);
                //Point p3 = new Point(p2.X + cos60 * (p4.X - p2.X) - sin60 * (p4.Y - p2.Y), p2.Y + sin60 * (p4.X - p2.X) - cos60 * (p4.Y - p2.Y));
                //PrintKochCurve(p1, p2, depth - 1);
                //PrintKochCurve(p2, p3, depth - 1);
                //PrintKochCurve(p3, p4, depth - 1);
                //PrintKochCurve(p4, p5, depth - 1);

                Point p2 = new Point(p1.X + (p5.X - p1.X) / 3, p1.Y + (p5.Y - p1.Y) / 3);
                Point p4 = new Point(p1.X + 2 * (p5.X - p1.X) / 3, p1.Y + 2 * (p5.Y - p1.Y) / 3);
                //double L = Math.Sqrt((p1.X - p5.X) * (p1.X - p5.X) + (p1.Y - p5.Y)*(p1.Y - p5.Y));
                //double h = L / 2 / Math.Sqrt(3);
                //double sina = (p5.Y - p1.Y)/L;
                //double cosa = (p5.X - p1.X) / L;
                Point p3 = new Point((p1.X + p5.X) / 2 + (p5.Y - p1.Y) / 2 / Math.Sqrt(3),
                                     (p1.Y + p5.Y) / 2 - (p5.X - p1.X) / 2 / Math.Sqrt(3));
                PrintKochCurve(p1, p2, depth - 1);
                PrintKochCurve(p2, p3, depth - 1);
                PrintKochCurve(p3, p4, depth - 1);
                PrintKochCurve(p4, p5, depth - 1);
            }


        }

        private Line LineCreate(Point a, Point b, Brush color)
        {
            Line res = new Line();
            res.Stroke = color;
            res.Fill = color;
            res.X1 = a.X;
            res.Y1 = a.Y;

            res.X2 = b.X;
            res.Y2 = b.Y;
            return res;
        }

        private void PrintHelpScreen()
        {
            helpBlock.Visibility = Visibility.Visible;
        }

        //private void PrintTreeFractal(int i)
        //{
        //    Size canvasSize = GetCanvasSize();
        //    Line line = new Line();

        //    line.Fill = Brushes.Black;
        //    line.Stroke = Brushes.Black;

        //    line.X1 = 0;
        //    line.Y1 = 0;

        //    line.X2 = canvasSize.Width;
        //    line.Y2 = canvasSize.Height;

        //    CanvasForFractal.Children.Add(line);
        //}

        private Size GetCanvasSize()
        {
            return CanvasForFractal.RenderSize;
        }

        /// <summary>
        /// Меняет цвет фона для глубины, показывая правильно ли задан параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalDepthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsDepthCorrect())
            {
                DepthRecrangle.Fill = new SolidColorBrush(myGreen);
            }
            else
            {
                DepthRecrangle.Fill = new SolidColorBrush(myRed);
            }
            PaintFractal();
        }

        /// <summary>
        /// Проверяет правильно ли заданы параметры фрактала
        /// </summary>
        /// <returns></returns>
        private bool IsOptionsCorrect()
        {
            switch (FractalTypeComboBox.SelectedIndex)
            {
                case 0:
                    return true;
                case 1:
                    return int.TryParse(FractalDepthTextBox.Text, out int itterations) &&
                        int.TryParse(FractalAngleTextBox.Text, out int angle)
                        && double.TryParse(FractalCoefTextBox.Text, out double coef) &&
                        treeFractal.IsParametersCorrect(itterations, angle, coef);
                case 2:
                case 3:
                case 4:
                case 5:
                    return IsDepthCorrect();
                default:
                    return false;
            };
        }

        /// <summary>
        /// Проверяет правильно ли задана глубина рекурсии
        /// </summary>
        /// <returns></returns>
        private bool IsDepthCorrect()
        {
            return int.TryParse(FractalDepthTextBox.Text, out int itterations) && itterations >= 0 && itterations < recursionDepthLimit;

        }

        private void FractalCoefTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(FractalCoefTextBox.Text, out double coef) && treeFractal.IsCoefCorrect(coef))
            {
                treeFractal.coeficent = coef;
            }
            if (FractalTypeComboBox.SelectedIndex != 1)
            {
                CoefRectangle.Fill = new SolidColorBrush(myGrey);
            }
            else if (double.TryParse(FractalCoefTextBox.Text, out coef) && treeFractal.IsCoefCorrect(coef))
            {
                CoefRectangle.Fill = new SolidColorBrush(myGreen);
            }
            else
            {
                CoefRectangle.Fill = new SolidColorBrush(myRed);
            }
        }

        private void FractalAngleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(FractalCoefTextBox.Text, out int angle) && treeFractal.IsAngleCorrect(angle))
            {
                treeFractal.angle = angle;
            }
            if (FractalTypeComboBox.SelectedIndex != 1)
            {
                AngleRectangle.Fill = new SolidColorBrush(myGrey);
            }
            else if (int.TryParse(FractalCoefTextBox.Text, out angle) && treeFractal.IsAngleCorrect(angle))
            {
                AngleRectangle.Fill = new SolidColorBrush(myGreen);
            }
            else
            {
                AngleRectangle.Fill = new SolidColorBrush(myRed);
            }
        }
    }
}
