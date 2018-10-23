using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DrawingApp
{
    public interface ICanvas
    {
        void SetActiveTool(ITool tool);
        void Repaint();
        void SetBackgroundColor(Color color);

        void AddDrawingObject(DrawingObject drawingObject);
        void RemoveDrawingObject(DrawingObject drawingObject);

        DrawingObject SelectObjectAt(int x, int y);
        void DeselectAllObjects();
    }
}
