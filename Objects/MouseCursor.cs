using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class MouseCursor : GameObject
    {
        private float interactionDistance;
        private bool hoveringOverThisFrame; //If the mouse was hovering over anything in this frame

        public MouseCursor()
        {
        }

        public MouseCursor(Texture2D texture2D, Vector2 position, float rotation, Vector2 origin, Vector2 scale, string name)
        {
            DrawableComponents.Add("RenderComponent", new RenderComponent(this, texture2D, position, rotation, origin, scale, 0.0f));

            position += Input.getInstance.getMousePosRelativeToScreen();

            ((RenderComponent)DrawableComponents.ElementAt(0).Value).IsMouse = true;

            Name = name;
            interactionDistance = 200.0f;

            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "Mouse")
                {
                    Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                }
            }
        }

        public float getInteractionDistance()
        {
            return interactionDistance;
        }

        public bool getHoveringOverThisFrame()
        {
            return hoveringOverThisFrame;
        }

        public void setHoveringOverThisFrame(bool hoveringOverThisFrame)
        {
            this.hoveringOverThisFrame = hoveringOverThisFrame;
        }

        public void startRotation(float degrees, int time)
        {
            ((RenderComponent)DrawableComponents.ElementAt(0).Value).Rotate = true;
        }

        public void onMouseHover(MiddleObject middleObject)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            bool doAction = player1.getDoAction();
            string name = middleObject.Name;
            hoveringOverThisFrame = true;

            if (name != null)
            {
                Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");
                Vector2 staticObjectPos = middleObject.Properties.getProperty<Vector2>("Position");

                Vector2 newVector = playerPos - staticObjectPos;
                float distance = newVector.Length();

                if (distance <= middleObject.getInteractionDistance())
                {
                    if (name == "DeadTree")
                    {
                        middleObject.setMouseIsHovering(true);
                        middleObject.Properties.updateProperty<Color>("Color", new Color(255, 255, 255, 120));

                        if (SceneManager.getInstance.getPlayer1().hasFantasyTool())
                        {
                            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                            {
                                if (texture.Key == "FantasyTool")
                                {
                                    Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                }
                            }
                        }
                        else
                        {
                            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                            {
                                if (texture.Key == "FantasyTool2")
                                {
                                    Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                }
                            }
                        }
                    }
                    else if (name == "TreeWithFood")
                    {
                        middleObject.setMouseIsHovering(true);
                        middleObject.Properties.updateProperty<Color>("Color", new Color(255, 255, 255, 120));

                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "Mouse")
                            {
                                Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                            }
                        }
                    }
                    else if (name == "Screw" || name == "Plank" || name == "Cement" || name == "OilCan")
                    {
                        middleObject.setMouseIsHovering(true);
                        middleObject.Properties.updateProperty<Color>("Color", new Color(255, 255, 255, 120));
                    }
                    else if(name == "Lab" || name == "Instructor" || name == "Granary")
                    {
                        middleObject.setMouseIsHovering(true);
                        middleObject.Properties.updateProperty<Color>("Color", new Color(255, 255, 255, 120));
                    }
                    else //Everything else
                    {
                        middleObject.setMouseIsHovering(true);
                    }
                    if (doAction)
                    {
                        middleObject.onInteract();
                    }
                }
                else
                {
                    if (middleObject.Properties.getProperty<Color>("Color") != Color.White)
                    {
                        middleObject.Properties.updateProperty<Color>("Color", Color.White);
                    }
                }
            }
        }

        public void onMouseHover(BackgroundObject backgroundObject)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            bool doAction = player1.getDoAction();
            string name = backgroundObject.Name;
            hoveringOverThisFrame = true;

            if (name != null)
            {
                Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");
                Vector2 staticObjectPos = backgroundObject.Properties.getProperty<Vector2>("Position");

                Vector2 newVector = playerPos - staticObjectPos;
                float distance = newVector.Length();

                if (distance <= backgroundObject.getInteractionDistance())
                {
                    if (name == "Field" || name == "FieldWithFood") // || name == "FieldFertilised"
                    {
                        backgroundObject.setMouseIsHovering(true);

                        if (name == "Field")
                        {
                            if (SceneManager.getInstance.getPlayer1().hasShovel())
                            {
                                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                                {
                                    if (texture.Key == "Pickaxe")
                                    {
                                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                    }
                                }
                            }
                        }
                        else if (name == "FieldWithFood")
                        {
                            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                            {
                                if (texture.Key == "Mouse")
                                {
                                    Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                                }
                            }
                        }

                        if (doAction)
                        {
                            backgroundObject.onInteract();
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public void onMouseNotHoverThisFrame()
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            bool doAction = player1.getDoAction();

            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "Mouse")
                {
                    Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                }
            }

            Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");
            Vector2 mouseCursorPos = Properties.getProperty<Vector2>("Position");

            Vector2 newVector = playerPos - mouseCursorPos;
            float distance = newVector.LengthSquared();  
        }
        
        public void onMouseHover(MenuButton menuButton)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            bool doAction = player1.getDoAction();
            string name = menuButton.Name;

            if (name != null)
            {
                if (name != "Background" && name != "")
                {
                    if (name != "FinishedQuestsMarked" && name != "CurrentQuestsMarked")
                    {
                        menuButton.Properties.updateProperty<Color>("Color", Color.White);
                    }

                    if (doAction)
                    {
                        menuButton.onInteract();
                    }
                }
            }
        }

        public void onMouseNotHover(MiddleObject middleObject)
        {
            if (middleObject.isMouseHovering())
            {
                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "Mouse")
                    {
                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                    }
                }
                if (middleObject.Properties.getProperty<Color>("Color") != Color.White)
                {
                    middleObject.Properties.updateProperty<Color>("Color", Color.White);
                }
                middleObject.setMouseIsHovering(false);
            }
        }

        public void onMouseNotHover(BackgroundObject backgroundObject)
        {
            if (backgroundObject.isMouseHovering())
            {
                foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                {
                    if (texture.Key == "Mouse")
                    {
                        Properties.updateProperty<Texture2D>("Texture2D", texture.Value);
                    }
                }
                if (backgroundObject.Properties.getProperty<Color>("Color") != Color.White)
                {
                    backgroundObject.Properties.updateProperty<Color>("Color", Color.White);
                }
                backgroundObject.setMouseIsHovering(false);
            }
        }

        public void onMouseNotHover(MenuButton menuButton)
        {
            if (menuButton.Properties.getProperty<Color>("Color") != new Color(255, 255, 255, 120) && menuButton.Name != "Background"
                && menuButton.Name != "")
            {
                menuButton.Properties.updateProperty<Color>("Color", new Color(255, 255, 255, 120));
            }
        }
    }
}