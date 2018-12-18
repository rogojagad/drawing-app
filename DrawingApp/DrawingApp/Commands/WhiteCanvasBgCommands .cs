using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DrawingApp.Commands
{
    public class WhiteCanvasBgCommands : ICommand
    {
        private ICanvas canvas;

        public WhiteCanvasBgCommands(ICanvas canvas)
        {
            this.canvas = canvas;
        }

        public void Execute()
        {
            Debug.WriteLine("Changing background color to white...");
            this.canvas.SetBackgroundColor(Color.White);
            this.canvas.Repaint();
        }

        public void Unexecute()
        {
            
        }
    }
}
