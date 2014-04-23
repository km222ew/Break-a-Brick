using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakABrick
{
    class Screen
    {
        protected EventHandler ScreenEvent;
        public Screen(EventHandler screenEvent)
        { 
            ScreenEvent = screenEvent;
        }

        public virtual void Update(GameTime gameTime)
        { 
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        { 
            
        }
    }
}
