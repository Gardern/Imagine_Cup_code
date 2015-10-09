using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class PhysicsComponent : GameObjectComponent
    {
        public bool[] Movement { get; set; }
        public bool IsMoveable { get; set; }

        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;

        public PhysicsComponent(GameObject gameObject, Vector2 position, Vector2 acceleration)
            : base(gameObject)
        {
            Name = "PhysicsComponent";
            Movement = new bool[5];
            IsMoveable = false;

            ParentObject.Properties.updateProperty<bool[]>("Movement", Movement);
            ParentObject.Properties.updateProperty<Vector2>("OldPosition", Vector2.Zero);
            ParentObject.Properties.updateProperty<Vector2>("Position", position);
            ParentObject.Properties.updateProperty<Vector2>("Velocity", Vector2.Zero);
            ParentObject.Properties.updateProperty<Vector2>("Acceleration", acceleration);
            ParentObject.Properties.updateProperty<bool>("IsPhysicsActive", true);
        }

        public override void update(GameTime gameTime)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsPhysicsActive");

            if (Active)
            {
                position = ParentObject.Properties.getProperty<Vector2>("Position");
                bool hasCollided = ParentObject.Properties.getProperty<bool>("HasCollided");
                velocity = ParentObject.Properties.getProperty<Vector2>("Velocity");
                acceleration = ParentObject.Properties.getProperty<Vector2>("Acceleration");

                if (IsMoveable)
                {
                    ParentObject.Properties.updateProperty<bool[]>("Movement", Movement);
                    Movement = ParentObject.Properties.getProperty<bool[]>("Movement");

                    if (Movement[0] && Movement[2])
                    {
                        velocity = new Vector2(0, 0);
                    }
                    else if (Movement[2] && Movement[1])
                    {
                        velocity = new Vector2(0, 0);
                    }
                    else if (Movement[1] && Movement[3])
                    {
                        velocity = new Vector2(0, 0);
                    }
                    else if (Movement[3] && Movement[0])
                    {
                        velocity = new Vector2(0, 0);
                    }
                    else if (Movement[0])
                    {
                        velocity = new Vector2(-350, -350);
                    }
                    else if (Movement[1])
                    {
                        velocity = new Vector2(350, 350);
                    }
                    else if (Movement[2])
                    {
                        velocity = new Vector2(350, -350);
                    }
                    else if (Movement[3])
                    {
                        velocity = new Vector2(-350, 350);
                    }
                    else
                    {
                        velocity = new Vector2(0, 0);
                    }
                }

                if (!hasCollided)
                {
                    position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Vector2 shadowPos = new Vector2(position.X - 11, position.Y - 8);
                    SceneManager.getInstance.getPlayer1Shadow().Properties.updateProperty<Vector2>("Position", shadowPos);
                }

                ParentObject.Properties.updateProperty<Vector2>("Position", position);
                ParentObject.Properties.updateProperty<Vector2>("Velocity", velocity);
                ParentObject.Properties.updateProperty<Vector2>("Acceleration", acceleration);
            }
        }
    }
}