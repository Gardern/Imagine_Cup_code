using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class Shadow : BackgroundObject
    {
        public Shadow(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale, string name)
        {
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 1.0f));

            Name = name;
        }
    }
}