using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class QuestHud
    {
        private string name;
        private List<MenuButton> buttons;

        public QuestHud(string name)
        {
            buttons = new List<MenuButton>();
            this.name = name;
        }

        public List<MenuButton> getButtons()
        {
            return buttons;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (SceneManager.getInstance.getPlayer1().getOpenQuests())
            {
                foreach (MenuButton menuButton in buttons)
                {
                    menuButton.draw(spriteBatch);
                }
            }
        }

        public void update(GameTime gameTime)
        {
            foreach (MenuButton menuButton in buttons)
            {
                menuButton.update(gameTime);
            }
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            foreach (MenuButton menuButton in buttons)
            {
                menuButton.onCollision(gameTime, collisionResult, player1Velocity);
            }
        }
    }
}