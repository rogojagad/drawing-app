﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingApp.States
{
    public class EditState : DrawingState
    {
        private static DrawingState instance;

        public static DrawingState GetInstance()
        {
            if (instance == null)
            {
                instance = new EditState();
            }

            return instance;
        }

        public override void Draw(StatefulDrawingObject obj)
        {
            obj.DrawOnEditingView();
        }

        public override void Deselect(StatefulDrawingObject obj)
        {
            obj.ChangeState(StaticState.GetInstance());
        }
    }
}
