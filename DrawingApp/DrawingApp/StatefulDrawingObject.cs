using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DrawingApp.States;

namespace DrawingApp
{
    public abstract class StatefulDrawingObject : DrawingObject
    {
        public DrawingState State
        {
            get
            {
                return this.state;
            }
        }

        private DrawingState state;

        public abstract void RenderOnStaticView();
        public abstract void RenderOnEditingView();
        public abstract void RenderOnPreview();

        public StatefulDrawingObject()
        {
            this.ChangeState(PreviewState.GetInstance());
        }

        public void ChangeState(DrawingState state)
        {
            this.state = state;
        }

        public override void Draw()
        {
            this.state.Draw(this);
        }

        public void Select()
        {
            Debug.WriteLine("ID" + ID.ToString() + "is selected");
            this.state.Select(this);
        }

        public void Deselect()
        {
            Debug.WriteLine("ID" + ID.ToString() + "is deselected");
            this.state.Deselect(this);
        }
    }
}
