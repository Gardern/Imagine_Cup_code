using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class AnimationComponent : DrawableGameObjectComponent
    {
        private Vector2 position;
        private float drawDepth;
        private List<Animation> animations;
        public bool[] Movement { get; set; }

        public AnimationComponent(GameObject gameObject, Texture2D[] textures, Vector2 position, float rotation, Vector2 origin,
            Vector2 scale, int numberOfAnimations, int[] animationSpeeds, int[] numberOfFrames, int[] numberOfRows)
            : base(gameObject)
        {
            Name = "AnimationComponent";

            animations = new List<Animation>(numberOfAnimations);

            for (int i = 0; i < numberOfAnimations; i++)
            {
                animations.Add(new Animation(textures[i], position, rotation, scale, origin,
                        animationSpeeds[i], numberOfFrames[i], numberOfRows[i]));
            }

            drawDepth = 0.0f;

            ParentObject.Properties.updateProperty<List<Animation>>("Animations", animations);
            ParentObject.Properties.updateProperty<Vector2>("Position", position);
            ParentObject.Properties.updateProperty<float>("Rotation", rotation);
            ParentObject.Properties.updateProperty<Vector2>("Origin", origin);
            ParentObject.Properties.updateProperty<Vector2>("Scale", scale);
            ParentObject.Properties.updateProperty<bool>("IsAnimationActive", true);
        }

        public override void update(GameTime gameTime)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsAnimationActive");

            if (Active)
            {
                animations = ParentObject.Properties.getProperty<List<Animation>>("Animations");
                drawDepth = ParentObject.Properties.getProperty<float>("DrawDepth");
                position = ParentObject.Properties.getProperty<Vector2>("Position");
                Movement = ParentObject.Properties.getProperty<bool[]>("Movement");

                if (Movement[0] && Movement[2])
                {
                    animations.ElementAt(0).stop();
                }
                else if (Movement[2] && Movement[1])
                {
                    animations.ElementAt(0).stop();
                }
                else if (Movement[1] && Movement[3])
                {
                    animations.ElementAt(0).stop();
                }
                else if (Movement[3] && Movement[0])
                {
                    animations.ElementAt(0).stop();
                }
                else if (Movement[0])
                {
                    animations.ElementAt(0).start(2, 100);
                }
                else if (Movement[1])
                {
                    animations.ElementAt(0).start(0, 100);
                }
                else if (Movement[2])
                {
                    animations.ElementAt(0).start(1, 100);
                }
                else if (Movement[3])
                {
                    animations.ElementAt(0).start(3, 100);
                }
                else
                {
                    animations.ElementAt(0).stop();
                }

                foreach (Animation animation in animations)
                {
                    animation.update(position, drawDepth, gameTime);
                }

                ParentObject.Properties.updateProperty<List<Animation>>("Animations", animations);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsAnimationActive");

            if (Active)
            {
                foreach (Animation animation in animations)
                {
                    animation.draw(spriteBatch);
                }
            }
        }
    }
}