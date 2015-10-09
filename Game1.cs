using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GLEED2D;

using System.Xml;

namespace ImagineCup2012
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // DEBUG
        private float totalTime1;
        private float totalTime2;
        private int fps1;
        private int fps2;
        private float fps = 60;
        private bool debugDrawCollision;
        private Texture2D debugTexture;

        public Game1()
        {
            Renderer.getInstance.setGraphics(new GraphicsDeviceManager(this));
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Console.WriteLine("\nGame1.Initialize\n");

            Renderer.getInstance.start(this, GraphicsDevice);
            AudioManager.getInstance.start();
            Input.getInstance.start();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Console.WriteLine("\nGame1.LoadContent\n");

            debugTexture = Content.Load<Texture2D>("Textures//test2");
            ResourceManager.getInstance.start(Content);
            SceneManager.getInstance.start(ResourceManager.getInstance.getLevels()[0], ResourceManager.getInstance.getMenus(), 
                ResourceManager.getInstance.getHuds());
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Input.getInstance.update();

            checkForInput();

            SceneManager.getInstance.update(gameTime);

            AudioManager.getInstance.update();

            base.Update(gameTime);

            // DEBUG
            debugFPSUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.getInstance.update(gameTime);

            base.Draw(gameTime);

            // DEBUG
            if (debugDrawCollision)
            {
                debugDraw(gameTime);
            }

            debugFPSdraw(gameTime);
        }

        private void checkForInput()
        {
            Keys moveRight = SettingsManager.getInstance.getKeyboardKeys().ElementAt(0);
            Keys moveLeft = SettingsManager.getInstance.getKeyboardKeys().ElementAt(1);
            Keys moveDown = SettingsManager.getInstance.getKeyboardKeys().ElementAt(2);
            Keys moveUp = SettingsManager.getInstance.getKeyboardKeys().ElementAt(3);
            Keys moveRight2 = SettingsManager.getInstance.getKeyboardKeys().ElementAt(4);
            Keys moveLeft2 = SettingsManager.getInstance.getKeyboardKeys().ElementAt(5);
            Keys moveDown2 = SettingsManager.getInstance.getKeyboardKeys().ElementAt(6);
            Keys moveUp2 = SettingsManager.getInstance.getKeyboardKeys().ElementAt(7);
            Keys quit = SettingsManager.getInstance.getKeyboardKeys().ElementAt(8);
            Keys showInventory = SettingsManager.getInstance.getKeyboardKeys().ElementAt(9);
            Keys showQuests = SettingsManager.getInstance.getKeyboardKeys().ElementAt(10);

            MouseKeys action = SettingsManager.getInstance.getMouseKeys().ElementAt(0);

            if (Input.getInstance.keyDown(quit))
            {
                SceneManager.getInstance.setGameState(GameState.IN_INGAMEMENU);
                SceneManager.getInstance.getMenuManager().findIndex("InGameMenu");
            }
            
            bool [] movement = { false, false, false, false, false};
            bool doAction = false;

            if (Input.getInstance.keyDown(moveLeft) || Input.getInstance.keyStillDown(moveLeft)
                || Input.getInstance.keyDown(moveLeft2) || Input.getInstance.keyStillDown(moveLeft2))
            {
                movement[0] = true;
            }
            if (Input.getInstance.keyDown(moveRight) || Input.getInstance.keyStillDown(moveRight)
                || Input.getInstance.keyDown(moveRight2) || Input.getInstance.keyStillDown(moveRight2))
            {
                movement[1] = true;
            }
            if (Input.getInstance.keyDown(moveUp) || Input.getInstance.keyStillDown(moveUp)
                || Input.getInstance.keyDown(moveUp2) || Input.getInstance.keyStillDown(moveUp2))
            {
                movement[2] = true;
            }
            if (Input.getInstance.keyDown(moveDown) || Input.getInstance.keyStillDown(moveDown)
                || Input.getInstance.keyDown(moveDown2) || Input.getInstance.keyStillDown(moveDown2))
            {
                movement[3] = true;
            }

            if (Input.getInstance.keyStillDown(moveLeft) || Input.getInstance.keyStillDown(moveRight) ||
                Input.getInstance.keyStillDown(moveUp) || Input.getInstance.keyStillDown(moveDown) ||
                Input.getInstance.keyStillDown(moveLeft2) || Input.getInstance.keyStillDown(moveRight2) ||
                Input.getInstance.keyStillDown(moveUp2) || Input.getInstance.keyStillDown(moveDown2))
            {
                if (AudioManager.getInstance.isStopped(new Sound("Walk grass")))
                {
                    AudioManager.getInstance.playSound(new Sound("Walk grass"), AudioManager.WALK_GRASS);
                }

                // movement[4] = true;
            }

            if (!Input.getInstance.keyStillDown(moveLeft) && !Input.getInstance.keyStillDown(moveRight) &&
                !Input.getInstance.keyStillDown(moveUp) && !Input.getInstance.keyStillDown(moveDown) &&
                !Input.getInstance.keyStillDown(moveLeft2) && !Input.getInstance.keyStillDown(moveRight2) &&
                !Input.getInstance.keyStillDown(moveUp2) && !Input.getInstance.keyStillDown(moveDown2))
            {
                AudioManager.getInstance.stopSound(new Sound("Walk grass"));
            }

            if (Input.getInstance.keyDown(showInventory))
            {
                SceneManager.getInstance.getPlayer1().setOpenInventory(!SceneManager.getInstance.getPlayer1().getOpenInventory());
            }

            if (Input.getInstance.keyDown(showQuests))
            {
                SceneManager.getInstance.getPlayer1().setOpenQuests(!SceneManager.getInstance.getPlayer1().getOpenQuests());
            }

            if (Input.getInstance.mouseDown(action))
            {
                doAction = true;
            }

            // DEBUG
            if (Input.getInstance.keyDown(Keys.D1))
            {
                --fps;
                //Console.WriteLine("New FPS: " + fps);
                TargetElapsedTime = TimeSpan.FromSeconds(1.0f / fps);
            }

            if (Input.getInstance.keyDown(Keys.D2))
            {
                ++fps;
                //Console.WriteLine("New FPS: " + fps);
                TargetElapsedTime = TimeSpan.FromSeconds(1.0f / fps);
            }

            // DEBUG
            if (Input.getInstance.keyDown(Keys.D3))
            {
                debugDrawCollision = !debugDrawCollision;
            }

            SceneManager.getInstance.getPlayer1().setMovement(movement);
            SceneManager.getInstance.getPlayer1().setDoAction(doAction);
            SceneManager.getInstance.getPlayer1().setOpenInventory(SceneManager.getInstance.getPlayer1().getOpenInventory());
            SceneManager.getInstance.getPlayer1().setOpenQuests(SceneManager.getInstance.getPlayer1().getOpenQuests());
        }

        // DEBUG
        private void debugFPSUpdate(GameTime gameTime)
        {
            totalTime1 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            fps1++;

            if (totalTime1 >= 1000)
            {
                //Console.WriteLine("Update FPS: " + fps1);

                totalTime1 = 0;
                fps1 = 0;

                if (gameTime.IsRunningSlowly)
                {
                    //Console.WriteLine("FPS is too slow");
                }
            }
        }

        // DEBUG
        private void debugFPSdraw(GameTime gameTime)
        {
            totalTime2 += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            fps2++;

            if (totalTime2 >= 1000)
            {
                //Console.WriteLine("Draw FPS: " + fps2);

                totalTime2 = 0;
                fps2 = 0;
            }
        }

        // DEBUG
        private void debugDraw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Renderer.getInstance.getSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

            debugCollision(spriteBatch);

            spriteBatch.End();
        }

        // DEBUG
        private void debugCollision(SpriteBatch spriteBatch)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            List<MiddleObject> middleObject = SceneManager.getInstance.getMiddleObjects();

            List<Polygon> polygons = player1.Properties.getProperty<List<Polygon>>("Polygon");

            foreach (Polygon polygon in polygons)
            {
                for (int i = 0; i < polygon.getPoints().Count; i++)
                {
                    Vector2 vector1 = polygon.getPoints().ElementAt(i);
                    Vector2 vector2 = new Vector2();

                    if (i + 1 == polygon.getPoints().Count)
                    {
                        vector2 = polygon.getPoints().ElementAt(0);
                    }
                    else
                    {
                        vector2 = polygon.getPoints().ElementAt(i + 1);
                    }

                    drawLine(spriteBatch, vector1, vector2);
                }
            }

            foreach (MiddleObject gameObject in middleObject)
            {
                CollisionType collisionType = gameObject.Properties.getProperty<CollisionType>("CollisionType");

                if (collisionType == CollisionType.POLYGON)
                {
                    List<Polygon> polygons1 = gameObject.Properties.getProperty<List<Polygon>>("Polygon");

                    foreach (Polygon polygon in polygons1)
                    {
                        for (int i = 0; i < polygon.getPoints().Count; i++)
                        {
                            Vector2 vector1 = polygon.getPoints().ElementAt(i);
                            Vector2 vector2 = new Vector2();

                            if (i + 1 == polygon.getPoints().Count)
                            {
                                vector2 = polygon.getPoints().ElementAt(0);
                            }
                            else
                            {
                                vector2 = polygon.getPoints().ElementAt(i + 1);
                            }

                            drawLine(spriteBatch, vector1, vector2);
                        }
                    }
                }
            }
        }

        // DEBUG
        private void drawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2)
        {
            float angle = (float)Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
            float dist = Vector2.Distance(p1, p2);

            spriteBatch.Draw(debugTexture, new Rectangle((int)p2.X, (int)p2.Y, (int)dist, 1), null, Color.White,
                    angle, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}