using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class MenuManager
    {
        private List<Menu> gameMenus;
        private int currentIndex;
        private string currentClickedButton;
        private string currentSelectedTechnology;

        public MenuManager()
        {
            gameMenus = new List<Menu>();
            currentIndex = -1;
            currentClickedButton = "";
            currentSelectedTechnology = "";
        }

        public List<Menu> getGameMenus()
        {
            return gameMenus;
        }

        public int getCurrentIndex()
        {
            return currentIndex;
        }

        public string getCurrentClickedButton()
        {
            return currentClickedButton;
        }
        public string getCurrentSelectedTechnology()
        {
            return currentSelectedTechnology;
        }

        public void setCurrentClickedButton(string currentClickedButton)
        {
            this.currentClickedButton = currentClickedButton;
        }

        public void setCurrentSelectedTechnology(string currentSelectedTechnology)
        {
            this.currentSelectedTechnology = currentSelectedTechnology;
        }

        /*
         * Finds the index of the specified menu
         * */
        public void findIndex(string menuName)
        {
            currentIndex = -1;
            int counter = 0;
            foreach (Menu gameMenu in gameMenus)
            {
                if (gameMenu.getName() == menuName)
                {
                    currentIndex = counter;
                    Console.WriteLine(gameMenu.getName());
                    AudioManager.getInstance.stopSound(new Sound("Walk grass"));
                    break;
                }
                counter++;
            }
        }

        /*
         * Returns the last found button with the specified name or null if it cant find it
         * */
        public MenuButton getButtonByName(string name)
        {
            MenuButton menuButton = null;

            foreach (Menu gameMenu in gameMenus)
            {
                foreach (MenuButton button in gameMenu.getMenuButtons())
                {
                    if (button.Name == name)
                    {
                        menuButton = button;
                    }
                }
            }
            return menuButton;
        }

        /*
         * Returns the index of the button with the specified name or -1 if it cant find it
         * */
        public int getButtonIndexByName(string name)
        {
            int index = -1;
            int counter = -1;

            foreach (Menu gameMenu in gameMenus)
            {
                foreach (MenuButton button in gameMenu.getMenuButtons())
                {
                    counter++;

                    if (button.Name == name)
                    {
                        index = counter;
                        break;
                    }
                }
            }
            return index;
        }

        /*
         * Returns the last found menu with the specified name or null if it cant find it
         * */
        public Menu getMenuByName(string name)
        {
            Menu menu = null;

            foreach (Menu gameMenu in gameMenus)
            {
                if (gameMenu.getName() == name)
                {
                    menu = gameMenu;
                }
            }
            return menu;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            gameMenus.ElementAt(currentIndex).draw(spriteBatch);
        }

        public void update(GameTime gameTime)
        {
            foreach (Menu gameMenu in gameMenus)
            {
                gameMenu.update(gameTime);
            }
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            foreach (Menu gameMenu in gameMenus)
            {
                gameMenu.onCollision(gameTime, collisionResult, player1Velocity);
            }
        }
    }
}