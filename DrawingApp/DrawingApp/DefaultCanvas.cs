using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DrawingApp.Shapes;

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
            /*
            if(! this.CornerPointsByGuid.ContainsKey(drawingObject.ID))
            {
                if (drawingObject.GetType() != typeof(GuidingLine))
                {
                    this.StoreObjectCornerPoints(drawingObject);
                }
            }
            */
        }

        public void RemoveDrawingObject(DrawingObject drawingObject)
        {
            this.drawingObjects.Remove(drawingObject);
            Debug.WriteLine("Remove called");
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
            Graphics g = this.CreateGraphics();

            foreach(Point activeObjPoint in activeObject.GetCornerPoints())
            {
                foreach(Point storedObjPoint in this.GetStoredObjectPoints(activeObject.ID))
                {
                    if( activeObjPoint.X == storedObjPoint.X )
                    {
                        this.ShowGuideLine(new Point(activeObjPoint.X, 0), new Point(activeObjPoint.X, 1000), g);
                        break;
                        
                    }
                    else if (activeObjPoint.Y == storedObjPoint.Y)
                    {
                        this.ShowGuideLine(new Point(0, activeObjPoint.Y), new Point(1000, activeObjPoint.Y), g);
                        break;
                    }
                }
                /*
                if (flag == 0)
                {
                    this.DismissGuideLine();
                }
                */
            }

            //this.ShowGuideLine(new Point(100, 0), new Point(100, 100), g);
            
        }

        private void ShowGuideLine(Point startpoint, Point endpoint, Graphics g)
        {
            GuidingLine guideLine = GuidingLine.GetInstance();

            guideLine.Startpoint = startpoint;
            guideLine.Endpoint = endpoint;
            guideLine.SetGraphics(g);

            if (! this.drawingObjects.Contains(guideLine))
            {
                Debug.WriteLine("Guide Line added");
                //this.AddDrawingObject(guideLine);
                this.drawingObjects.Add(guideLine);
            }

            guideLine.Draw();
        }

        public void DismissGuideLine()
        {
            GuidingLine guidingLine = GuidingLine.GetInstance();

            if (this.drawingObjects.Contains(guidingLine))
            {
                Debug.WriteLine("Guiding Line removed");
                this.RemoveDrawingObject(guidingLine);
            }
        }

        private List<Point> GetStoredObjectPoints(Guid activeObjId)
        {
            List<Point> storedObjPoints = new List<Point>();

            foreach(DrawingObject obj in this.drawingObjects)
            {
                if(obj.ID != activeObjId)
                {
                    storedObjPoints.AddRange(obj.GetCornerPoints());
                }
            }

            return storedObjPoints;
        }
    }
}
