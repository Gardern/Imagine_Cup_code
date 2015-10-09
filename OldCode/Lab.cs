using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public enum Technology { Test };

    public class Lab : GameObject
    {
        private string name;

        public Lab()
        {
        }

        public Lab(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale,
            List<Polygon> polygons, List<AABB> axisAlignedBoxes, float depthPosition, string name)
        {
            Components.Add("CollisionComponent", new CollisionComponent(this, polygons, axisAlignedBoxes));
            Components.Add("DepthComponent", new DepthComponent(this, depthPosition));
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, -1.0f));

            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        //Skal returnere teknologien den lager?
        public void createTechnology(Technology technology)
        {
            
        }
    }
}