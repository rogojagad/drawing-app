﻿using System.Collections.Generic;
using System.Drawing;

namespace DrawingApp.Shapes
{
    class Text : DrawingObject
    {
        public string Value { get; set; }
        public PointF Position { get; set; }

        private Brush brush;
        private Font font;

        public Text()
        {
            this.brush = new SolidBrush(Color.Black);

            FontFamily fontFamily = new FontFamily("Arial");

            font = new Font(
                fontFamily,
                16,
                FontStyle.Regular,
                GraphicsUnit.Pixel
            );
        }

        public override bool AddDrawingObject(DrawingObject obj)
        {
            return false;
        }

        public override bool RemoveDrawingObject(DrawingObject obj)
        {
            return false;
        }

        public override bool Intersect(int xTest, int yTest)
        {
            return false;
        }

        public override void RenderOnEditingView()
        {
            GetGraphics().DrawString(Value, font, brush, Position);
        }

        public override void RenderOnPreview()
        {
            GetGraphics().DrawString(Value, font, brush, Position);
        }

        public override void RenderOnStaticView()
        {
            GetGraphics().DrawString(Value, font, brush, Position);
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            Position = new PointF(Position.X + xAmount, Position.Y + yAmount);
        }

        public override void SetCornerPoints()
        {
            throw new System.NotImplementedException();
        }
    }
}
