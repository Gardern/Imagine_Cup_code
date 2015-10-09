using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class Menu
    {
        private string name;
        private List<MenuButton> menuButtons;

        public Menu(string name)
        {
            menuButtons = new List<MenuButton>();
            this.name = name;
        }

        public string getName()
        {
            return name;
        }

        public List<MenuButton> getMenuButtons()
        {
            return menuButtons;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (MenuButton menuButton in menuButtons)
            {
                menuButton.draw(spriteBatch);
            }
        }

        public void update(GameTime gameTime)
        {
            foreach (MenuButton menuButton in menuButtons)
            {
                menuButton.update(gameTime);
            }
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            foreach (MenuButton menuButton in menuButtons)
            {
                menuButton.onCollision(gameTime, collisionResult, player1Velocity);
            }
        }
    }
}