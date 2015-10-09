using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class BackgroundObject : GameObject
    {
        private float interactionDistance;

        private bool mouseIsHovering;

        public BackgroundObject()
        {
        }

        public BackgroundObject(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale, 
            List<Polygon> polygons, List<AABB> axisAlignedBoxes, string name)
        {
            Components.Add("CollisionComponent", new CollisionComponent(this, polygons, axisAlignedBoxes));
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 1.0f));

            Name = name;
            interactionDistance = 100.0f;
            mouseIsHovering = false;
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
            Vector2 playerPos = SceneManager.getInstance.getPlayer1().Properties.getProperty<Vector2>("Position");

            if (Name == "Field")
            {
                Console.WriteLine("Field clicked");

                bool isBlending = ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsBlending;

                if (!isBlending)
                {
                    foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                    {
                        if (texture.Key == "FieldFertilised")
                        {
                            Console.WriteLine("Start blending");
                            AudioManager.getInstance.playSound(new Sound("Use tool pickaxe"), AudioManager.USE_TOOL_PICKAXE); 
                            startBlending(texture.Value, new Color(255, 255, 255, 120), "FieldFertilised", 0.2f, 10);
                        }
                    }
                }
            }
            else if (Name == "FieldWithFood")
            {
                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "Field")
                    {
                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                        Name = "Field";
                    }
                }
                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "HudCorn")
                    {
                        SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(EItemType.CORN, texture.Value, new Vector2(), 0.0f, 
                            new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 1, ESlotType.OTHER);
                    }
                }

                SceneManager.getInstance.getFontManager().addFont(
                                new Font(EFontType.STATIC, 10, "FontFetchCorn",
                                    ResourceManager.getInstance.Font1, "You picked up corn",
                                    new Vector2(playerPos.X - 355, playerPos.Y + 240), Color.White, 2));

                AudioManager.getInstance.playSound(new Sound("Fetch apples and corn"), AudioManager.FETCH_APPLES_AND_CORN);
            }
             /*
            else if (Name == "FieldFertilised")
            {
                Console.WriteLine("FieldFertilised clicked");
                //SceneManager.getInstance.getMouseCursor().startRotation(90, 2);

                bool isBlending = ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsBlending;

                if (!isBlending)
                {
                    foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                    {
                        if (texture.Key == "FieldWithFood")
                        {
                            Console.WriteLine("Start blending");
                            startBlending(texture.Value, new Color(255, 255, 255, 120), "FieldWithFood", 1.0f, 10);
                        }
                    }
                }
            }
            * */
        }

        public void onCollision()
        {

        }

        public void onNotCollision()
        {

        }
    }
}