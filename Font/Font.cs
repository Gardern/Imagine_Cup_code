using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    //The font type. STATIC means it is standing still and DYNAMIC means it is moving in a direction
    public enum EFontType { STATIC, DYNAMIC_LEFT, DYNAMIC_RIGHT, DYNAMIC_DOWN, DYNAMIC_UP }

    public class Font
    {
        private EFontType fontType;
        private int id;
        private string name;
        private bool active;
        private SpriteFont spriteFont;
        private string text;
        private Vector2 position;
        private float speed;
        private Color color;
        private float timeToFinish;
        private float lifetime;

        /*
         * fontType: The font type
         * id: Font id
         * name: Font name
         * spriteFont: The actual font
         * text: The fonts text
         * position: Position or start position of the font (if it is moving)
         * speed: Speed of the font (Used for moving fonts)
         * color: The color of the font
         * timeToFinish: The time (in seconds) it takes for the font to vanish. -1.0 if you dont want it to vanish
         * */
        public Font(EFontType fontType, int id, string name, SpriteFont spriteFont, string text, Vector2 position, float speed, Color color, float timeToFinish)
        {
            this.fontType = fontType;
            this.id = id;
            this.name = name;
            active = true;
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            this.speed = speed;
            this.color = color;
            this.timeToFinish = timeToFinish;
            lifetime = 0.0f;
        }

        /*
         * fontType: The font type
         * id: Font id
         * name: Font name
         * spriteFont: The actual font
         * text: The fonts text
         * position: Position or start position of the font (if it is moving)
         * color: The color of the font
         * timeToFinish: The time (in seconds) it takes for the font to vanish. -1.0 if you dont want it to vanish
         * */
        public Font(EFontType fontType, int id, string name, SpriteFont spriteFont, string text, Vector2 position, Color color, float timeToFinish)
        {
            this.fontType = fontType;
            this.id = id;
            this.name = name;
            active = true;
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
            speed = 0.0f;
            this.color = color;
            this.timeToFinish = timeToFinish;
            lifetime = 0.0f;
        }

        public EFontType getFontType()
        {
            return fontType;
        }

        public int getId()
        {
            return id;
        }

        public string getName()
        {
            return name;
        }

        public bool getActive()
        {
            return active;
        }

        public SpriteFont getSpriteFont()
        {
            return spriteFont;
        }

        public string getText()
        {
            return text;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public float getSpeed()
        {
            return speed;
        }

        public Color getColor()
        {
            return color;
        }

        public float getTimeToFinish()
        {
            return timeToFinish;
        }

        public float getLifetime()
        {
            return lifetime;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setSpriteFont(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public void setText(string text)
        {
            this.text = text;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void setColor(Color color)
        {
            this.color = color;
        }

        public void setTimeToFinish(float timeToFinish)
        {
            this.timeToFinish = timeToFinish;
        }

        public void setLifetime(float lifetime)
        {
            this.lifetime = lifetime;
        }

        public void update(GameTime gameTime)
        {
            //We only increase the fonts lifetime if timeToFinish != -1
            if (timeToFinish != -1.0f)
            {
                lifetime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (lifetime >= timeToFinish)
                {
                    active = false;
                }
            }

            bool hasCollided = SceneManager.getInstance.getPlayer1().Properties.getProperty<bool>("HasCollided");
            Vector2 velocity = SceneManager.getInstance.getPlayer1().Properties.getProperty<Vector2>("Velocity");
            Vector2 position = this.position;

            if (!hasCollided)
            {
                position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (fontType == EFontType.DYNAMIC_LEFT)
            {
                position = new Vector2(position.X - speed, position.Y);
            }
            else if (fontType == EFontType.DYNAMIC_RIGHT)
            {
                position = new Vector2(position.X + speed, position.Y);
            }
            else if (fontType == EFontType.DYNAMIC_DOWN)
            {
                position = new Vector2(position.X, position.Y + speed);
            }
            else if (fontType == EFontType.DYNAMIC_UP)
            {
                position = new Vector2(position.X, position.Y - speed);
            }

            this.position = position;
            //Console.WriteLine("Font pos: " + this.position);
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            Vector2 position = this.position;

            player1Velocity = player1Velocity / 2;

            position += player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds +
                    collisionResult.MinimumTranslationVector;

            this.position = position;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();

            //We draw everything except for the inventory fonts and quest fonts
            if (name != "FontInventory" && name != "FontQuestHudQuest1" && name != "FontQuestHudQuest2")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }

            //If the inventory is open we draw the inventory fonts too
            if (player1.getOpenInventory() &&  name == "FontInventory")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }

            //Console.WriteLine("Quest: " + player1.getOpenQuests());
            //Console.WriteLine("Current: " + player1.getOpenCurrentQuests());
            //Console.WriteLine("Mission1: " + !LevelHandler.getInstance.isMission1Finished());
            //Console.WriteLine();

            if (player1.getOpenQuests() && player1.getOpenCurrentQuests() && !LevelHandler.getInstance.isQuest1Finished() && name == "FontQuestHudQuest1")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }

            if (player1.getOpenQuests() && player1.getOpenCurrentQuests() && !LevelHandler.getInstance.isQuest2Finished() && name == "FontQuestHudQuest2")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }

            if (player1.getOpenQuests() && player1.getOpenFinishedQuests() && LevelHandler.getInstance.isQuest1Finished() && name == "FontQuestHudQuest1")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }

            if (player1.getOpenQuests() && player1.getOpenFinishedQuests() && LevelHandler.getInstance.isQuest2Finished() && name == "FontQuestHudQuest2")
            {
                spriteBatch.DrawString(spriteFont, text, position, color);
            }
        }
    }
}