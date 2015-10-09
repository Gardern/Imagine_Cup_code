using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public abstract class GameObject
    {
        public PropertyContainer Properties { get; set; }

        public Dictionary<String, GameObjectComponent> Components { get; set; }
        public Dictionary<String, DrawableGameObjectComponent> DrawableComponents { get; set; }

        public String Type { get; protected set; }
        public bool Active { get; set; }
        public string Name { get; set; }

        public GameObject()
        {
            Type = "GameObject";
            Active = true;
            Name = "";
            Properties = new PropertyContainer();
            Components = new Dictionary<String, GameObjectComponent>();
            DrawableComponents = new Dictionary<String, DrawableGameObjectComponent>();
        }

        /*
         * Starts and initializes blending for an object
         * blendingTexture: The texture to blend into this objects current texture
         * blendingColor: the blending start color
         * name: The name the finished blended object will have
         * time: The time in seconds it takes for each update to the colors alpha value
         * alpha: how much alpha the color will increase with on each time update
         * */
        public void startBlending(Texture2D blendingTexture, Color blendingColor, string name, float time, byte alpha)
        {
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).BlendingTexture = blendingTexture;
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).BlendingColor = blendingColor;
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).BlenderName = name;
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).BlendingTime = time;
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).BlendingAlphaByte = alpha;
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsBlending = true;
        }

        public bool containsComponent(string name)
        {
            foreach (KeyValuePair<String, GameObjectComponent> component in Components)
            {
                if (component.Value.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void initialize()
        {
        }

        public virtual void update(GameTime gameTime)
        {
            foreach (KeyValuePair<String, GameObjectComponent> component in Components)
                component.Value.update(gameTime);

            foreach (KeyValuePair<String, DrawableGameObjectComponent> component in DrawableComponents)
                component.Value.update(gameTime);
        }

        public virtual void draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<String, DrawableGameObjectComponent> component in DrawableComponents)
                component.Value.draw(spriteBatch);
        }
    }
}