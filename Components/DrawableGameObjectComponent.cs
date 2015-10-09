using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public abstract class DrawableGameObjectComponent : GameObjectComponent
    {
        public DrawableGameObjectComponent(GameObject parentObject)
            : base(parentObject)
        {

        }

        public abstract void draw(SpriteBatch spriteBatch);
    }
}