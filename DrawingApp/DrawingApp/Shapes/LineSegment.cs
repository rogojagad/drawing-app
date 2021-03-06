﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DrawingApp.Shapes
{
    public class LineSegment : DrawingObject
    {
        private const double EPSILON = 3.0;

        public Point Startpoint { get; set; }
        public Point Endpoint { get; set; }

        private Pen pen;

        public LineSegment()
        {
            this.pen = new Pen(Color.Black);
            pen.Width = 1.5f;
        }

        public LineSegment(Point startpoint) :
            this()
        {
            this.Startpoint = startpoint;
        }

        public LineSegment(Point startpoint, Point endpoint) :
            this(startpoint)
        {
            this.Endpoint = endpoint;
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
            this.Startpoint = new Point(this.Startpoint.X + xAmount, this.Startpoint.Y + yAmount);
            this.Endpoint = new Point(this.Endpoint.X + xAmount, this.Endpoint.Y + yAmount);
            
            this.SetCornerPoints();
        }

        public double GetSlope()
        {
            return (double)(Endpoint.Y - Startpoint.Y) / (double)(Endpoint.X - Startpoint.X);
        }

        public override bool AddDrawingObject(DrawingObject obj)
        {
            return false;   
        }

        public override bool RemoveDrawingObject(DrawingObject obj)
        {
            return false;
        }

        public override void SetCornerPoints()
        {
            if(this.CornerPoints.Count != 0)
            {
                this.CornerPoints.Clear();
            }

            this.CornerPoints.Add(this.Startpoint);
            this.CornerPoints.Add(this.Endpoint);
        }
    }
}
