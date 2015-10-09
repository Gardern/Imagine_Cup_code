using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public abstract class GameObjectComponent
    {
        public GameObject ParentObject { get; protected set; }
        public String Name { get; protected set; }
        public bool Active { get; set; }

        public GameObjectComponent(GameObject parentObject)
        {
            ParentObject = parentObject;
            Name = "NoName";
            Active = true;
        }

        public abstract void update(GameTime gameTime);
    }
}