using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class MiddleObject : GameObject
    {
        private float interactionDistance;

        private bool mouseIsHovering;

        public int dRand;

        public MiddleObject(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale,
            List<Polygon> polygons, List<AABB> axisAlignedBoxes, float depthPosition, string name)
        {
            Components.Add("CollisionComponent", new CollisionComponent(this, polygons, axisAlignedBoxes));
            Components.Add("DepthComponent", new DepthComponent(this, depthPosition));
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, -1.0f));

            Name = name;
            interactionDistance = 150.0f;
            mouseIsHovering = false;

            Random r = new Random();
            dRand = r.Next(0, 5);
        }

        public MiddleObject(Texture2D[] textures, Vector2 position, float rotation, Vector2 origin, Vector2 scale,
            List<Polygon> polygons, List<AABB> axisAlignedBoxes, int numberOfAnimations, int[] animationSpeeds, int[] numberOfFrames,
                int[] numberOfRows, float depthPosition, string name)
        {
            Components.Add("CollisionComponent", new CollisionComponent(this, polygons, axisAlignedBoxes));
            Components.Add("DepthComponent", new DepthComponent(this, depthPosition));
            DrawableComponents.Add("AnimationComponent", new AnimationComponent(this, textures, position, rotation, origin, scale,
                numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows));

            Name = name;
            interactionDistance = 200.0f;
            mouseIsHovering = false;

            Random r = new Random();
            dRand = r.Next(0, 5);
        }

        public float getInteractionDistance()
        {
            return interactionDistance;
        }

        public bool isMouseHovering()
        {
            return mouseIsHovering;
        }

        public void setMouseIsHovering(bool mouseIsHovering)
        {
            this.mouseIsHovering = mouseIsHovering;
        }

        public void onInteract()
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");

            if (Name == "Screw" || Name == "Plank" || Name == "Cement" || Name == "OilCan")
            {
                player1.getInventory().getResources().increaseResource(Name, 1);
                //player1.getInventory().getResources().numberOfResources();
                Active = false;

                AudioManager.getInstance.playSound(new Sound("Pickup"), AudioManager.PICKUP);  
            }
            else if (Name == "Instructor")
            {
                SceneManager.getInstance.setGameState(GameState.IN_INGAMEMENU);
                SceneManager.getInstance.getMenuManager().findIndex("MissionMenu");

                AudioManager.getInstance.playSound(new Sound("Convers start"), AudioManager.CONVERS_START);
            }
            else if (Name == "Lab")
            {
                SceneManager.getInstance.setGameState(GameState.IN_INGAMEMENU);
                SceneManager.getInstance.getMenuManager().findIndex("MainLabMenu");

                AudioManager.getInstance.playSound(new Sound("Lab sounds"), AudioManager.LAB_SOUNDS);
            }
            else if (Name == "DeadTree" && SceneManager.getInstance.getPlayer1().hasFantasyTool())
            {
                bool isBlending = ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsBlending;

                if (!isBlending)
                {
                    foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                    {
                        if (texture.Key == "TreeWithFood")
                        {
                            AudioManager.getInstance.playSound(new Sound("Use tool fantasy"), AudioManager.USE_TOOL_FANTASY);  

                            startBlending(texture.Value, new Color(255, 255, 255, 100), "TreeWithFood", 0.4f, 20);
                        }
                    }
                }
            }
            else if (Name == "TreeWithFood")
            {
                //Bytte ut treets tekstur
                bool isBlending = ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsBlending;

                if (!isBlending)
                {
                    foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                    {
                        if (texture.Key == "Tree")
                        {
                            startBlending(texture.Value, new Color(255, 255, 255, 100), "Tree", 0.1f, 20);

                            Random r = new Random();
                            int numberOfApples = r.Next(3, 6);
                            player1.setNumberOfApples(player1.getNumberOfApples() + numberOfApples);

                            SceneManager.getInstance.getFontManager().addFont(
                                new Font(EFontType.STATIC, 8, "FontFetchApples",
                                    ResourceManager.getInstance.Font1, "You picked up " + numberOfApples +
                                    (numberOfApples == 1 ? " apple" : " apples"),
                                    new Vector2(playerPos.X - 355, playerPos.Y + 240), Color.White, 2)); 

                            foreach (KeyValuePair<string, Texture2D> texture2 in ResourceManager.getInstance.getRuntimeTextures())
                            {
                                if (texture2.Key == "HudApple")
                                {
                                    SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(EItemType.CORN, texture2.Value, new Vector2(), 0.0f,
                                        new Vector2(texture2.Value.Width / 2, texture2.Value.Height / 2), new Vector2(1, 1)), numberOfApples, ESlotType.OTHER);
                                }
                            }
                        }
                    }
                }
            }
            else if (Name == "Granary")
            {
                /*
                 * We loop through the players inventory and removes the food, and adds it to the granary
                 * */
                foreach (InventorySlotHud slotHud in SceneManager.getInstance.getHudManager().getInventoryHud().getItems())
                {
                    // SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(new ItemHud(EItemType.SCREW, texture.Value, new Vector2(), 0.0f,
                    //           new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 8);

                    if (slotHud.getItemHud() != null)
                    {
                        //Console.WriteLine(slotHud.getItemHud().getItemType());

                        if (slotHud.getItemHud().getItemType() == EItemType.CORN && slotHud.getNumberOfItems() > 0)
                        {
                            Texture2D texture = slotHud.getItemHud().Properties.getProperty<Texture2D>("Texture2D");

                            //Update the corn in the granary
                            LevelHandler.getInstance.setNumberOfCorn(LevelHandler.getInstance.getNumberOfCorn() + slotHud.getNumberOfItems());

                            //Adds a font
                            SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, 13, "FontFoodGranary",
                                ResourceManager.getInstance.Font1, "You dropped " + slotHud.getNumberOfItems() + " corn into the food house",
                                    new Vector2(playerPos.X - 350, playerPos.Y + 150), Color.Green, 3.0f));

                            //Remove items from the players inventory
                            SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(
                                new ItemHud(slotHud.getItemHud().getItemType(), texture, new Vector2(), 0.0f,
                                    new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1)), slotHud.getNumberOfItems());
                        }
                        
                        if (slotHud.getItemHud().getItemType() == EItemType.APPLE && slotHud.getNumberOfItems() > 0)
                        {
                            Texture2D texture = slotHud.getItemHud().Properties.getProperty<Texture2D>("Texture2D");

                            //Update the apples in the granary
                            LevelHandler.getInstance.setNumberOfApples(LevelHandler.getInstance.getNumberOfApples() + slotHud.getNumberOfItems());

                            //Adds a font
                            SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, 13, "FontFoodGranary",
                                ResourceManager.getInstance.Font1, "You dropped " + slotHud.getNumberOfItems() + " apples into the food house",
                                    new Vector2(playerPos.X - 350, playerPos.Y + 150), Color.Green, 3.0f));

                            //Remove items from the players inventory
                            SceneManager.getInstance.getHudManager().getInventoryHud().removeItem(
                                new ItemHud(slotHud.getItemHud().getItemType(), texture, new Vector2(), 0.0f,
                                    new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1)), slotHud.getNumberOfItems());
                        }
                    }
                }
            }
        }

        public void onCollision()
        {

        }

        public void onNotCollision()
        {

        }
    }
}