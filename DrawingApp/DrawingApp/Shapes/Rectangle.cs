using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace DrawingApp.Shapes
{
    public class Rectangle : DrawingObject, IObservable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point CenterPoint { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        List<IObserver> observerList = new List<IObserver>();

        private Pen pen;
        private List<DrawingObject> drawingObjects;

        public Rectangle()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
            drawingObjects = new List<DrawingObject>();
        }

        public Rectangle(int x, int y) : this()
        {
            this.X = x;
            this.Y = y;
        }

        public Rectangle(int x, int y, int width, int height) : this(x, y)
        {
            this.Width = width;
            this.Height = height;
        }

        public void SetCenterPoint()
        {
            int Xcenter = this.X + this.Width / 2;
            int Ycenter = this.Y + this.Height / 2;

            this.CenterPoint = new Point(Xcenter, Ycenter);
        }

        public override bool Intersect(int xTest, int yTest)
        {
            if ((xTest >= X && xTest <= X + Width) && (yTest >= Y && yTest <= Y + Height))
            {
                Debug.WriteLine("Object " + ID + " is selected");
                return true;
            }

            return false;
        }

        public override void RenderOnEditingView()
        {
            this.pen.Color = Color.Blue;
            this.pen.DashStyle = DashStyle.Solid;
            GetGraphics().DrawRectangle(this.pen, X, Y, Width, Height);

            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.SetGraphics(GetGraphics());
                obj.RenderOnStaticView();
            }

            this.NotifyObserver();
        }

        public override void RenderOnStaticView()
        {
            this.pen.Color = Color.Black;
            this.pen.DashStyle = DashStyle.Solid;
            GetGraphics().DrawRectangle(this.pen, X, Y, Width, Height);

            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.SetGraphics(GetGraphics());
                obj.RenderOnStaticView();
            }

            this.NotifyObserver();
        }

        public override void RenderOnPreview()
        {
            this.pen.Color = Color.Red;
            this.pen.DashStyle = DashStyle.DashDot;
            GetGraphics().DrawRectangle(this.pen, X, Y, Width, Height);

            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.SetGraphics(GetGraphics());
                obj.RenderOnStaticView();
            }

            this.NotifyObserver();
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.X += xAmount;
            this.Y += yAmount;

            this.SetCenterPoint();

            this.SetCornerPoints();

            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.Translate(x, y, xAmount, yAmount);
            }
        }

        public override bool AddDrawingObject(DrawingObject obj)
        {
            drawingObjects.Add(obj);
            return true;
        }

        public override bool RemoveDrawingObject(DrawingObject obj)
        {
            drawingObjects.Remove(obj);
            return true;
        }

        public void AddObserver(IObserver observer)
        {
            this.observerList.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observerList.Remove(observer);
        }

        public void NotifyObserver()
        {
            foreach(IObserver observer in this.observerList)
            {
                observer.Update();
            }
        }

        public override void SetCornerPoints()
        {
            if(this.CornerPoints.Count != 0)
            {
                this.CornerPoints.Clear();
            }

            int halfWidth = this.Width / 2;
            int halfHeight = this.Height / 2;
            int centerX = this.CenterPoint.X;
            int centerY = this.CenterPoint.Y;

            this.CornerPoints.Add(new Point(centerX - halfWidth, centerY + halfHeight));
            this.CornerPoints.Add(new Point(centerX + halfWidth, centerY + halfHeight));
            this.CornerPoints.Add(new Point(centerX - halfWidth, centerY - halfHeight));
            this.CornerPoints.Add(new Point(centerX + halfWidth, centerY - halfHeight));
        }
    }
}
