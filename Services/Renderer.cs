using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public enum TEXTURE_DEPTH { DEFAULT, PLAYER_IN_FRONT };

    public class Renderer
    {
        private static readonly Renderer instance = new Renderer();

        private Game1 game;
        private GraphicsDeviceManager graphics;
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;
        private Vector2 screenCenter;
        private int height;
        private int width;
        private int fps;
        private bool fullscreen;
        public Vector3 screenScalingFactor;

        private Renderer()
        {
            height = 0;
            width = 0;
            fps = 0;
            fullscreen = false;
        }

        public static Renderer getInstance
        {
            get
            {
                return instance;
            }
        }

        public Game1 getGame()
        {
            return game;
        }

        public GraphicsDevice getGraphicsDevice()
        {
            return graphicsDevice;
        }

        public SpriteBatch getSpriteBatch()
        {
            return spriteBatch;
        }

        public Vector2 getScreenCenter()
        {
            return screenCenter;
        }

        public void setGraphics(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }
          
        public void start(Game1 game, GraphicsDevice graphicsDevice)
        {
            Console.WriteLine("Renderer.start");
            this.game = game; 
            this.graphicsDevice = graphicsDevice; 
            spriteBatch = new SpriteBatch(graphicsDevice); 

            SettingsManager.getInstance.getGraphicsSettings(out height, out width, out fps, out fullscreen); 

            /*
            const bool resultionIndependent = true;
            Vector2 baseScreenSize = new Vector2(800, 600);

            screenScalingFactor = new Vector3(1f, 1f, 1f);
            float horScaling = 0;
            float verScaling = 0;
            if (resultionIndependent)
            {
                horScaling = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / baseScreenSize.X;
                verScaling = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / baseScreenSize.Y;

                screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            }
            else
            {
                screenScalingFactor = new Vector3(1, 1, 1);
            }

            Console.WriteLine("Scale factor: " + screenScalingFactor);

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            screenCenter = new Vector2(horScaling / 2f, verScaling / 2f);
            */

            if (fullscreen)
            {
                graphics.ToggleFullScreen();
            }
            else
            {
                graphics.PreferredBackBufferHeight = height;
                graphics.PreferredBackBufferWidth = width;
                graphics.ApplyChanges();
            }
              
            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);

            this.game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / fps);

            Console.WriteLine("Width: " + game.Window.ClientBounds.Width);
            Console.WriteLine("Height: " + game.Window.ClientBounds.Height);
            Console.WriteLine("ScreenCenter: " + screenCenter);
            Console.WriteLine("Fps: " + fps);
        }

        public void update(GameTime gameTime)
        {
            graphicsDevice.Clear(Color.White);

            SceneManager.getInstance.draw(spriteBatch, SceneManager.getInstance.getGameState());
        }
    }
}