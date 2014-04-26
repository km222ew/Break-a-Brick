using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakABrick.Screens
{
    class ScreenChoice : EventArgs
    {
        public int choice;

        public ScreenChoice(int choice)
        {
            this.choice = choice;
        }
    }
}
