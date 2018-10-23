using DrawingApp.States;
using System;
using System.Drawing;
using System.Diagnostics;

namespace DrawingApp
{
    public abstract class DrawingObject
    {
        public Guid ID { get; set; }
        public Graphics Graphics { get; set; }

        private DrawingState state;

        public DrawingState State
        {
            get
            {
                return this.state;
            }
        }

        public DrawingObject()
        {
            ID = Guid.NewGuid();
            this.ChangeState(PreviewState.GetInstance());
        }

        public abstract bool Intersect(int xTest, int yTest);

        public abstract void RenderOnPreview();
        public abstract void RenderOnEditingView();
        public abstract void RenderOnStaticView();

        public void ChangeState(DrawingState state)
        {
            this.state = state;
        }

        public virtual void Draw()
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
