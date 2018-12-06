using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using DrawingApp.States;

namespace DrawingApp.Shapes
{
    class GuidingLine : DrawingObject
    {
        private Pen pen;

        private static GuidingLine instance;

        private GuidingLine()
        {
            Debug.WriteLine("Guide line instantiate");
            this.pen = new Pen(Color.Red);
            pen.Width = 1.5f;
            pen.DashStyle = DashStyle.DashDotDot;
        }

        public static GuidingLine GetInstance()
        {
            if (instance == null)
            {
                instance = new GuidingLine();
            }

            return instance;
        }

        public void Draw(Point startpoint, Point endpoint)
        {/*
            Debug.WriteLine("Ada 2 objek dalam satu garis");
            */

            if (this.GetGraphics() != null)
            {
                this.GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                this.GetGraphics().DrawLine(this.pen, startpoint, endpoint);

            }
        }

        public override bool AddDrawingObject(DrawingObject obj)
        {
            return false;
        }

        public override bool Intersect(int xTest, int yTest)
        {
            return false;
        }

        public override bool RemoveDrawingObject(DrawingObject obj)
        {
            return false;
        }

        public override void RenderOnEditingView()
        {
            
        }

        public override void RenderOnPreview()
        {
            
        }

        public override void RenderOnStaticView()
        {
            
        }

        public override void SetCornerPoints()
        {
            
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            
        }
    }
}
