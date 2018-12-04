using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingApp.Shapes
{
    public class Connector : DrawingObject, IObserver
    {
        private const double EPSILON = 3.0;

        public Point Startpoint { get; set; }
        public Point Endpoint { get; set; }
        public Rectangle Source;
        public Rectangle Destination;

        private Pen pen;

        public Connector()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
        }
        
        public Connector(Rectangle src) :
            this()
        {
            this.Source = src;
        }

        public Connector(Rectangle src, Rectangle dst) :
            this(src)
        {
            this.Destination = dst;

            this.SetPoints();
        }

        private void SetPoints()
        {
            this.Startpoint = this.Source.CenterPoint;

            this.Endpoint = this.Destination.CenterPoint;
        }

        public override void RenderOnStaticView()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.Startpoint, this.Endpoint);
            }
        }

        public override void RenderOnEditingView()
        {
            this.pen = new Pen(Color.Blue);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.Solid;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.Startpoint, this.Endpoint);
                
            }
        }

        public override void RenderOnPreview()
        {
            this.pen = new Pen(Color.Red);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(pen, this.Startpoint, this.Endpoint);
                
            }
        }

        public override bool Intersect(int xTest, int yTest)
        {
            double slope = GetSlope();
            double b = Endpoint.Y - slope * Endpoint.X;
            double yPoint = slope * xTest + b;

            if (Math.Abs(yTest - yPoint) < EPSILON)
            {
                Debug.WriteLine("Object " + ID + " is selected");
                return true;
            }

            return false;
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            
        }

        public double GetSlope()
        {
            return (double)(Endpoint.Y - Startpoint.Y) / (double)(Endpoint.X - Startpoint.X);
        }

        public override bool Add(DrawingObject obj)
        {
            return false;   
        }

        public override bool Remove(DrawingObject obj)
        {
            return false;
        }

        public void Update()
        {
            this.Startpoint = new Point(this.Source.CenterPoint.X, this.Source.CenterPoint.Y);

            this.Endpoint = new Point(this.Destination.CenterPoint.X, this.Destination.CenterPoint.Y);
        }
    }
}
