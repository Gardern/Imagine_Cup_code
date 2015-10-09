using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class AABB
    {
        private Vector2 position;
        private Vector2 oldPosition;
        private float width;
        private float height;

        public AABB(Vector2 position, float width, float height)
        {
            this.oldPosition = new Vector2(0, 0);
            this.position = position;
            this.width = width;
            this.height = height;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public Vector2 getOldPosition()
        {
            return oldPosition;
        }

        public float getWidth()
        {
            return width;
        }

        public float getHeight()
        {
            return height;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void setOldPosition(Vector2 oldPosition)
        {
            this.oldPosition = oldPosition;
        } 
    }
}