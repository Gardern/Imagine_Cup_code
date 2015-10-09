using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class DepthComponent : GameObjectComponent
    {
        private float depthPosition;

        public DepthComponent(GameObject gameObject, float depthPosition)
            : base(gameObject)
        {
            this.depthPosition = depthPosition;

            ParentObject.Properties.updateProperty<float>("DepthPosition", depthPosition);
            ParentObject.Properties.updateProperty<bool>("IsDepthActive", true);
        }

        public override void update(GameTime gameTime)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsDepthActive");

            if (Active)
            {
                depthPosition = ParentObject.Properties.getProperty<float>("DepthPosition");
                bool hasCollided = ParentObject.Properties.getProperty<bool>("HasCollided");
                float drawDepth = ParentObject.Properties.getProperty<float>("DrawDepth");
                Vector2 velocity = ParentObject.Properties.getProperty<Vector2>("Velocity");

                if (!hasCollided)
                {
                    depthPosition += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    drawDepth = depthPosition / (SceneManager.getInstance.getAxisAlignedBox().getPosition().Y + SceneManager.getInstance.getAxisAlignedBox().getHeight());
                }

                ParentObject.Properties.updateProperty<float>("DrawDepth", drawDepth);
                ParentObject.Properties.updateProperty<float>("DepthPosition", depthPosition);
            }
        }
    }
}