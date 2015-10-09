using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class HudManager
    {
        private InventoryHud inventoryHud;
        private QuestHud questHud;

        public HudManager()
        {
            inventoryHud = new InventoryHud();
            questHud = new QuestHud("");
        }

        public InventoryHud getInventoryHud()
        {
            return inventoryHud;
        }

        public QuestHud getQuestHud()
        {
            return questHud;
        }

        public void setQuestHud(QuestHud questHud)
        {
            this.questHud = questHud;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            inventoryHud.draw(spriteBatch);
            questHud.draw(spriteBatch);
        }

        public void update(GameTime gameTime)
        {
            inventoryHud.update(gameTime);
            questHud.update(gameTime);
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            inventoryHud.onCollision(gameTime, collisionResult, player1Velocity);
            questHud.onCollision(gameTime, collisionResult, player1Velocity);
        }
    }
}