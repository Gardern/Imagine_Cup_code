using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public enum EItemType { SCREW, OILCAN, CEMENT, CORN, APPLE, TREEGUN, SHOVEL, PICKAXE , NONE};

    public class ItemHud : GameObject
    {
        private EItemType itemType;

        public ItemHud(EItemType itemType, Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale)
        {
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 1.0f));

            this.itemType = itemType;
        }

        public EItemType getItemType()
        {
            return itemType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ItemHud item = obj as ItemHud;

            if ((object)item == null)
            {
                return false;
            }

            return Properties.getProperty<Texture2D>("Texture2D") == item.Properties.getProperty<Texture2D>("Texture2D");
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public new void update(GameTime gameTime)
        {
            bool hasCollided = SceneManager.getInstance.getPlayer1().Properties.getProperty<bool>("HasCollided");
            Vector2 velocity = SceneManager.getInstance.getPlayer1().Properties.getProperty<Vector2>("Velocity");
            Vector2 position = Properties.getProperty<Vector2>("Position");

            if (!hasCollided)
            {
                position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            Properties.updateProperty<Vector2>("Position", position);
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            Vector2 position = Properties.getProperty<Vector2>("Position");

            player1Velocity = player1Velocity / 2;

            position += player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds +
                    collisionResult.MinimumTranslationVector;

            Properties.updateProperty<Vector2>("Position", position);
        }
    }
}