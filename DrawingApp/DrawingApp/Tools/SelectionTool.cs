using DrawingApp.Shapes;
using DrawingApp.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace DrawingApp.Tools
{
    public class SelectionTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private DrawingObject selectedObject;
        private int xInitial;
        private int yInitial;

        private bool multiselectState = false;
        private List<DrawingObject> selectedObjects = new List<DrawingObject>();

        public Cursor Cursor
        {
            get
            {
                return Cursors.Arrow;
            }
        }

        public ICanvas TargetCanvas
        {
            get
            {
                return this.canvas;
            }

            set
            {
                this.canvas = value;
            }
        }

        public SelectionTool()
        {
            this.Name = "Selection Tool";
            this.ToolTipText = "Selection Tool";
            this.Image = IconSet.cursor;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            this.xInitial = e.X;
            this.yInitial = e.Y;

            if (e.Button == MouseButtons.Left && canvas != null)
            {
                if (! multiselectState)
                {
                    canvas.DeselectAllObjects();
                    this.selectedObjects.Clear();
                    this.selectedObject = null;
                }

                //this.selectedObject = canvas.SelectObjectAt(e.X, e.Y);

                if (this.selectedObject != null && !multiselectState)
                {
                    this.selectedObject.ChangeState(StaticState.GetInstance());
                }

                foreach (DrawingObject obj in this.canvas.GetDrawingObjects().Reverse<DrawingObject>())
                {
                    if (obj.Intersect(e.X, e.Y))
                    {
                        if (! this.multiselectState)
                        {
                            this.selectedObjects.Clear();
                        }
                        else
                        {
                            //if (!selectedObjects.Any()) selectedObjects.Add(this.selectedObject);
                            this.selectedObjects.Add(obj);
                        }
                        this.selectedObject = obj;
                        obj.ChangeState(EditState.GetInstance());
                        
                        break;
                    }
                }
                
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && canvas != null)
            {
                if (selectedObject != null)
                {
                    int xAmount = e.X - this.xInitial;
                    int yAmount = e.Y - this.yInitial;
                    xInitial = e.X;
                    yInitial = e.Y;

                    selectedObject.Translate(e.X, e.Y, xAmount, yAmount);
                    this.canvas.CheckAlignedObjects(this.selectedObject);
                }
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && canvas != null)
            {
                this.canvas.DismissGuideLine();
            }
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {
            
        }
        
        public void ToolKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.ShiftKey)
            {
                this.multiselectState = false;
            }
        }

        public void ToolKeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine(e.KeyCode);

            if (e.KeyCode == System.Windows.Forms.Keys.ShiftKey)
            {
                this.multiselectState = true;
            }
        }

        public void ToolHotKeysDown(object sender, Keys e)
        {
            if (this.selectedObjects.Count() > 0)
            {
                Debug.WriteLine("Grouping started");

                DrawingGroup drawingGroup = new DrawingGroup();

                foreach (DrawingObject obj in this.selectedObjects)
                {
                    drawingGroup.AddDrawingObject(obj);
                }

                drawingGroup.ChangeState(EditState.GetInstance());
                this.canvas.AddDrawingObject(drawingGroup);
                this.selectedObject = drawingGroup;
            }
        }
    }
}
