using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class MenuButton : GameObject
    {
        public MenuButton(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale, string name)
        {
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 1.0f));

            Name = name;
        }

        public void onInteract()
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");
            SceneManager.getInstance.getMenuManager().setCurrentClickedButton(Name);
            string currentClickedButton = SceneManager.getInstance.getMenuManager().getCurrentClickedButton();

            AudioManager.getInstance.playSound(new Sound("Mainmenu click"), AudioManager.MAINMENU_CLICK);  

            if (Name == "Start")
            {
                //Load level 1
                SceneManager.getInstance.loadLevel(ResourceManager.getInstance.getLevels()[0]);
                SceneManager.getInstance.setGameState(GameState.IN_GAME);
            }
            else if (Name == "Quit")
            {
                Renderer.getInstance.getGame().Exit();
            }
            else if (Name == "Highscores")
            {

            }
            else if (Name == "Options")
            {

            }
            else if (Name == "Map1")
            {
                SceneManager.getInstance.getMenuManager().findIndex("Map1Menu");
            }
            else if (Name == "StartGame")
            {
                //Load level 1
                SceneManager.getInstance.loadLevel(ResourceManager.getInstance.getLevels()[0]);
                SceneManager.getInstance.setGameState(GameState.IN_GAME);
            }
            else if (Name == "Continue")
            {
                SceneManager.getInstance.setGameState(GameState.IN_GAME);
            }
            else if (Name == "MainMenu")
            {
                SceneManager.getInstance.setGameState(GameState.IN_STARTMENU);
                SceneManager.getInstance.getMenuManager().findIndex("StartMenu");

                AudioManager.getInstance.stopSound(new Sound("Level 1 ingame"));
                AudioManager.getInstance.playSound(new Sound("Menu Music"), AudioManager.MAINMENU);
            }
            else if (Name == "StartMissions")
            {
                SceneManager.getInstance.setGameState(GameState.IN_GAME);

                SceneManager.getInstance.getFontManager().addFont(
                    new Font(EFontType.STATIC, 9, "FontQuestGained",
                        ResourceManager.getInstance.Font1, "New quest gained",
                        new Vector2(playerPos.X - 355, playerPos.Y + 240), Color.White, 2)); 

                LevelHandler.getInstance.startMissions();

                AudioManager.getInstance.playSound(new Sound("Convers end"), AudioManager.CONVERS_END);
                AudioManager.getInstance.playSound(new Sound("Quest gained"), AudioManager.QUEST_GAINED);
            }
            else if (Name == "LabMenuBack_Main")
            {
                SceneManager.getInstance.setGameState(GameState.IN_GAME);

                AudioManager.getInstance.stopSound(new Sound("Lab sounds"));
            }
            else if (Name == "BuyTechnologies")
            {
                SceneManager.getInstance.getMenuManager().findIndex("TechLabMenu");
            }
            else if (Name == "ShowInventory")
            {
                //SceneManager.getInstance.getMenuManager().findIndex("InventoryLabMenu");
            }
            else if (Name == "LabMenuBack_Tech")
            {
                SceneManager.getInstance.getMenuManager().findIndex("MainLabMenu");
            }
            else if (Name == "ItemBlock_FantasyTool")
            {
                SceneManager.getInstance.getMenuManager().setCurrentSelectedTechnology(Name);
            }
            else if (Name == "BuyTech") // && currentClickedButton == "ItemBlock" && nok ressurser
            {
                ResourceItem resource = SceneManager.getInstance.getPlayer1().getInventory().getResources();
                string currentSelectedTech = SceneManager.getInstance.getMenuManager().getCurrentSelectedTechnology();
                int screws = resource.getScrews();
                int oilCans = resource.getOilCans();
                int cement = resource.getCement();
                Console.WriteLine(currentSelectedTech);
                //screws >= 8 && oilCans >= 6 && cement >= 8
                if (currentSelectedTech == "ItemBlock_FantasyTool" && screws >= 8 && oilCans >= 6 && cement >= 8) //&& enough resources for this technology
                // if (currentSelectedTech == "ItemBlock_FantasyTool" && screws >= 0 && oilCans >= 0 && cement >= 0) //&& enough resources for this technology
                {
                    Console.WriteLine("You bought the: " + currentSelectedTech);

                    MenuButton button = null;
                    int index = -1;

                    if (currentSelectedTech == "ItemBlock_FantasyTool")
                    {
                        button = SceneManager.getInstance.getMenuManager().getButtonByName("FantasyTool");
                        index = SceneManager.getInstance.getMenuManager().getButtonIndexByName("ItemBlock_FantasyTool");

                        SceneManager.getInstance.getPlayer1().setFantasyTool(true);

                        if (AudioManager.getInstance.isStopped(new Sound("Buy tech")))
                        {
                            AudioManager.getInstance.playSound(new Sound("Buy tech"), AudioManager.BUY_TECH);
                        }

                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "HudFantasyTool")
                            {
                                SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(EItemType.TREEGUN, texture.Value, new Vector2(), 0.0f,
                                    new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 1, ESlotType.MAIN);
                            }
                        }

                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "HudScrew")
                            {
                                SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(new ItemHud(EItemType.SCREW, texture.Value, new Vector2(), 0.0f,
                                    new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 8);
                            }
                        }
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "HudOilCan")
                            {
                                SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(new ItemHud(EItemType.OILCAN, texture.Value, new Vector2(), 0.0f,
                                    new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 6);
                            }
                        }
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "HudCement")
                            {
                                SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(new ItemHud(EItemType.CEMENT, texture.Value, new Vector2(), 0.0f,
                                    new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 8);
                            }
                        }

                        resource.decreaseResource("Screw", 8);
                        resource.decreaseResource("OilCan", 6);
                        resource.decreaseResource("Cement", 8);
                    }

                    if (button != null && index > -1)
                    {
                        Console.WriteLine(index);
                        Menu menu = SceneManager.getInstance.getMenuManager().getMenuByName("InventoryLabMenu");
                        menu.getMenuButtons().Add(button);
                    }
                }
            }
            else if (Name == "LabMenuBack_Inventory")
            {
                SceneManager.getInstance.getMenuManager().findIndex("MainLabMenu");
            }
            else if (Name == "ReturnToMenu")
            {
                SceneManager.getInstance.getMenuManager().findIndex("StartMenu");
            }
            else if (Name == "CurrentQuestsUnmarked")
            {
                SceneManager.getInstance.getPlayer1().setOpenCurrentQuests(true);
                SceneManager.getInstance.getPlayer1().setOpenFinishedQuests(false);

                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "CurrentQuestsMarked")
                    {
                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                        Name = texture.Key;
                    }
                }

                foreach (MenuButton button in SceneManager.getInstance.getHudManager().getQuestHud().getButtons())
                {
                    if (button.Name == "FinishedQuestsMarked")
                    {
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "FinishedQuestsUnmarked")
                            {
                                button.Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                button.Name = texture.Key;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            else if (Name == "FinishedQuestsUnmarked")
            {
                SceneManager.getInstance.getPlayer1().setOpenCurrentQuests(false);
                SceneManager.getInstance.getPlayer1().setOpenFinishedQuests(true);

                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "FinishedQuestsMarked")
                    {
                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                        Name = texture.Key;
                    }
                }

                foreach (MenuButton button in SceneManager.getInstance.getHudManager().getQuestHud().getButtons())
                {
                    if (button.Name == "CurrentQuestsMarked")
                    {
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "CurrentQuestsUnmarked")
                            {
                                button.Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                button.Name = texture.Key;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
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