using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public enum ESlotType { MAIN, OTHER };

    public class InventorySlotHud : GameObject
    {
        private bool containsItem;
        private ESlotType slotType;
        private ItemHud item;
        private int numberOfItems;

        public InventorySlotHud(ESlotType slotType, Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale)
        {
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 1.0f));


            //We just initialize these at default values
           // DrawableComponents.Add("RenderComponent2", new RenderComponent(this, null, new Vector2(), 0.0f, new Vector2(), new Vector2(), 1.0f));
            containsItem = false;
            this.slotType = slotType;
            item = null;
            numberOfItems = 0;
        }

        public bool getContainsItem()
        {
            return containsItem;
        }

        public ESlotType getSlotType()
        {
            return slotType;
        }

        public ItemHud getItemHud()
        {
            return item;
        }

        public int getNumberOfItems()
        {
            return numberOfItems;
        }

        public void setContainsItem(bool containsItem)
        {
            this.containsItem = containsItem;
        }

        public void setItemHud(ItemHud item)
        {
            this.item = item;
        }

        public void setNumberOfItems(int numberOfItems)
        {
            this.numberOfItems = numberOfItems;
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

            if (containsItem)
            {
                item.update(gameTime);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);

            if (containsItem)
            {
                item.draw(spriteBatch);
            }
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            if (containsItem)
            {
                item.onCollision(gameTime, collisionResult, player1Velocity);
            }

            Vector2 position = Properties.getProperty<Vector2>("Position");

            player1Velocity = player1Velocity / 2;

            position += player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds +
                    collisionResult.MinimumTranslationVector;

            Properties.updateProperty<Vector2>("Position", position);
        }
    }
}