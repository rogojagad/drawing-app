using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DrawingApp.Commands
{
    public class BlackCanvasBgCommands : ICommand
    {
        private ICanvas canvas;

        public BlackCanvasBgCommands(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Execute()
        {
            Debug.WriteLine("Changing background color to black...");
            this.canvas.SetBackgroundColor(Color.Black);
            this.canvas.Repaint();
        }
    }
}
