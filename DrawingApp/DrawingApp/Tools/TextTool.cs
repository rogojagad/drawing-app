using DrawingApp.Shapes;
using System.Windows.Forms;

namespace DrawingApp.Tools
{
    class TextTool : ToolStripButton, ITool
    {
        private Text text;
        private ICanvas canvas;

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

        public TextTool()
        {
            this.Name = "Text Tool";
            this.ToolTipText = "Text Tool";
            this.Image = IconSet.font;
            this.CheckOnClick = true;
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                text = new Text();
                text.Value = "Text";
                text.Position = new System.Drawing.PointF(e.X, e.Y);
                canvas.AddDrawingObject(text);
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {

        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
