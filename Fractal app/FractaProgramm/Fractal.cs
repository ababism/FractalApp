using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FractaProgramm
{
    abstract class Fractal
    {
        Canvas canvas;
        readonly int itterationsLimit;
        //abstract public Canvas Canvas { get; set; }
        public Fractal(Canvas canvas)
        {
            this.canvas = canvas;
        }
        abstract public void Draw(int itterations);
        abstract public bool IsDepthCorrect(int itterations);

        internal Line LineCreate(Point a, Point b, Brush color)
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


    }
    class TreeFractal : Fractal
    {
        Canvas canvas;
        private readonly int itterationsLimit = 10;
        private readonly int angeLimit = 90;
        public int angle;
       

        public double PiAngle
        {
            get { 
                return angle * 2 * Math.PI / 360.0;
            }
        }

        public double coeficent;
        public TreeFractal(Canvas canvas, int angle, double coeficent) : base(canvas)
        {
            this.angle = angle;
            this.coeficent = coeficent;
        }
        public override void Draw(int itterations)
        {
            PrintTreeFractal(canvas.RenderSize.Height / 2, canvas.RenderSize.Width / 2, canvas.Height / 3, Math.PI / 2, itterations);
        }
        private void PrintTreeFractal(double x1, double y1, double lenght, double angle, int itteration)
        {
            double x2, y2;
            x2 = x1 + lenght * Math.Sin(angle);
            y2 = y1 + lenght * Math.Cos(angle);
            if (itteration == 0)
            {
                LineCreate(new Point(x1, y2), new Point(x2, y2), Brushes.Black);
            }
            PrintTreeFractal(x2, y2, lenght * coeficent, angle + PiAngle, itteration - 1);
            PrintTreeFractal(x2, y2, lenght * coeficent, angle - PiAngle, itteration - 1);
        }

        public bool IsParametersCorrect(int itterations, int angle, double coefficent)
        {
            return IsDepthCorrect(itterations) && IsAngleCorrect(angle) && IsCoefCorrect(coefficent);
        }

        public bool IsCoefCorrect(double coefficent)
        {
            return coefficent > 0 && coefficent < 1;
        }

        public bool IsAngleCorrect(int angle)
        {
            return angle > 0 && angle < angeLimit;
        }

        public override bool IsDepthCorrect(int itterations)
        {
            return itterations >= 0 && itterations < itterationsLimit;
        }
    }
        class KochFractal : Fractal
        {
            Canvas canvas;
            readonly int itterationsLimit = 8;
            public KochFractal(Canvas canvas) : base(canvas) { }

            
            public override void Draw(int itterations)
            {
                PrintKochCurve(new Point(0, canvas.RenderSize.Height), new Point(canvas.RenderSize.Width, canvas.RenderSize.Height), itterations);
            }
            private void PrintKochCurve(Point p1, Point p5, int depth)
            {
                if (depth == 0)
                {
                    canvas.Children.Add(LineCreate(p1, p5, Brushes.Black));
                }
                else
                {
                    Point p2 = new Point(p1.X + (p5.X - p1.X) / 3, p1.Y + (p5.Y - p1.Y) / 3);
                    Point p4 = new Point(p1.X + 2 * (p5.X - p1.X) / 3, p1.Y + 2 * (p5.Y - p1.Y) / 3);

                    Point p3 = new Point((p1.X + p5.X) / 2 + (p5.Y - p1.Y) / 2 / Math.Sqrt(3),
                                         (p1.Y + p5.Y) / 2 - (p5.X - p1.X) / 2 / Math.Sqrt(3));
                    PrintKochCurve(p1, p2, depth - 1);
                    PrintKochCurve(p2, p3, depth - 1);
                    PrintKochCurve(p3, p4, depth - 1);
                    PrintKochCurve(p4, p5, depth - 1);
                }
            }
            public override bool IsDepthCorrect(int itterations)
            {
                return itterations >= 0 && itterations < itterationsLimit;
            }
        }
    }

