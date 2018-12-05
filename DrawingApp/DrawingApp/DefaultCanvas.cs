﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingApp
{
    public class DefaultCanvas : Control, ICanvas
    {
        private ITool activeTool;
        private List<DrawingObject> drawingObjects;

        public DefaultCanvas()
        {
            Init();
        }
        
        private void Init()
        {
            this.drawingObjects = new List<DrawingObject>();
            this.DoubleBuffered = true;

            this.BackColor = Color.White;
            this.Dock = DockStyle.Fill;

            this.Paint += DefaultCanvas_Paint;
            this.MouseDown += DefaultCanvas_MouseDown;
            this.MouseUp += DefaultCanvas_MouseUp;
            this.MouseMove += DefaultCanvas_MouseMove;
            this.MouseDoubleClick += DefaultCanvas_MouseDoubleClick;

            this.KeyDown += DefaultCanvas_KeyDown;
            this.KeyUp += DefaultCanvas_KeyUp;
            this.PreviewKeyDown += DefaultCanvas_PreviewKeyDown;
        }

        private void DefaultCanvas_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    e.IsInputKey = true;
                    break;
                case Keys.Up:
                    e.IsInputKey = true;
                    break;
                case Keys.Down:
                    e.IsInputKey = true;
                    break;
                case Keys.Left:
                    e.IsInputKey = true;
                    break;
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void DefaultCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolKeyUp(sender, e);
            }
        }

        private void DefaultCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolKeyDown(sender, e);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Control | Keys.G:
                        Debug.WriteLine("CTRL + G");
                        if (this.activeTool != null)
                        {
                            this.activeTool.ToolHotKeysDown(this, Keys.Control | Keys.G);
                            this.Repaint();
                        }
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DefaultCanvas_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseDoubleClick(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseMove(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseUp(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.activeTool != null)
            {
                this.activeTool.ToolMouseDown(sender, e);
                this.Repaint();
            }
        }

        private void DefaultCanvas_Paint(object sender, PaintEventArgs e)
        {
            foreach (DrawingObject obj in drawingObjects)
            {
                obj.SetGraphics(e.Graphics);
                obj.Draw();
            }
        }

        public void Repaint()
        {
            this.Invalidate();
            this.Update();
        }

        public void SetActiveTool(ITool tool)
        {
            this.activeTool = tool;
        }

        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        public void AddDrawingObject(DrawingObject drawingObject)
        {
            this.drawingObjects.Add(drawingObject);
            this.Repaint();
            Debug.WriteLine("New drawing object");
        }

        public void RemoveDrawingObject(DrawingObject drawingObject)
        {
            this.drawingObjects.Remove(drawingObject);
        }

        public DrawingObject GetObjectAt(int x, int y)
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                if (obj.Intersect(x,y))
                {
                    return obj;
                }
            }

            return null;
        }

        public DrawingObject SelectObjectAt(int x, int y)
        {
            DrawingObject obj = GetObjectAt(x, y);

            if (obj != null)
            {
                obj.Select();
            }

            return obj;
        }

        public void DeselectAllObjects()
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.Deselect();
            }
        }

        public List<DrawingObject> GetDrawingObjects()
        {
            return this.drawingObjects;
        }

        public void CheckAlignedObjects(DrawingObject activeObject)
        {
            Guid activeObjID = activeObject.ID;

            List<Point> activeObjCornerPoints = new List<Point>();
            List<Point> storedObjectCornerPoints = new List<Point>();

            activeObjCornerPoints.AddRange(activeObject.GetCornerPoints());
            storedObjectCornerPoints.AddRange(this.GetStoredObjectsCornerPoints(activeObjID));

            foreach (Point activeObjPoint in activeObjCornerPoints)
            {
                foreach (Point storedObjPoint in storedObjectCornerPoints)
                {
                    if( activeObjPoint.X == storedObjPoint.X )
                    {
                        Debug.WriteLine("X is same");
                    }

                    if (activeObjPoint.Y == storedObjPoint.Y)
                    {
                        Debug.WriteLine("Y is same");
                    }
                }
            }
        }

        public List<Point> GetStoredObjectsCornerPoints(Guid activeObjID)
        {
            List<Point> storedObjectCornerPoints = new List<Point>();

            foreach (DrawingObject obj in this.drawingObjects)
            {
                if (obj.ID != activeObjID)
                {
                    storedObjectCornerPoints.AddRange(obj.GetCornerPoints());
                }
            }

            return storedObjectCornerPoints;
        }
    }
}
