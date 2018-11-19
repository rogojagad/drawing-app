using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using DrawingApp.Shapes;
using DrawingApp.States;

namespace DrawingApp.Tools
{
    class ConnectorTool : ToolStripButton, ITool
    {
        private ICanvas canvas;
        private Connector connector;

        Rectangle sourceObj;
        Rectangle destObj;

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

        public ConnectorTool()
        {
            this.Name = "Connector Tool";
            this.ToolTipText = "Connector Tool";
            this.Image = IconSet.diagonal_line;
            this.CheckOnClick = true;
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button ==  MouseButtons.Left)
            {
                foreach (DrawingObject obj in this.canvas.GetDrawingObjects().Reverse<DrawingObject>())
                {
                    if (obj.Intersect(e.X, e.Y) && obj.GetType() == typeof(Rectangle) )
                    {
                        Debug.WriteLine("Source found");

                        this.sourceObj = (Rectangle) obj;

                        this.connector = new Connector(this.sourceObj);

                        this.connector.ChangeState(PreviewState.GetInstance());

                        break;
                    }
                }
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            if (this.sourceObj != null)
            {
                foreach (DrawingObject obj in this.canvas.GetDrawingObjects().Reverse<DrawingObject>())
                {
                    if (obj.Intersect(e.X, e.Y) && obj.GetType() == typeof(Rectangle))
                    {
                        this.destObj = (Rectangle)obj;

                        Debug.WriteLine("Destination found");

                        this.connector.Destination = (Rectangle) obj;

                        connector.Source.AddObserver(connector);
                        connector.Destination.AddObserver(connector);

                        connector.ChangeState(StaticState.GetInstance());

                        canvas.AddDrawingObject(connector);
                    }
                }
            }
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {
         
        }

        public void ToolKeyUp(object sender, KeyEventArgs e)
        {
         
        }

        public void ToolKeyDown(object sender, KeyEventArgs e)
        {
         
        }

        public void ToolHotKeysDown(object sender, Keys e)
        {
         
        }
    }
}
