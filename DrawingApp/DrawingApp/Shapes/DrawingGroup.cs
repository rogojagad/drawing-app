using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingApp.States;

namespace DrawingApp.Shapes
{
    class DrawingGroup : DrawingObject
    {
        private List<DrawingObject> drawingObjects = new List<DrawingObject>();

        public override void ChangeState(DrawingState state)
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.ChangeState(state);
            }

            this.state = state;
        }

        public override void Draw()
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.State.Draw(obj);
            }
        }

        public override void Select()
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.Select();
            }
        }

        public override bool Intersect(int xTest, int yTest)
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                if (obj.Intersect(xTest, yTest))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            foreach (DrawingObject obj in this.drawingObjects)
            {
                obj.Translate(x, y, xAmount, yAmount);
            }
        }

        public override bool AddDrawingObject(DrawingObject obj)
        {
            this.drawingObjects.Add(obj);
            return true;
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
            throw new NotImplementedException();
        }
    }
}
