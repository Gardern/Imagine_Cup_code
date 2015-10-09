using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class Animation
    {
        private Texture2D texture2D;
        private Vector2 position;
        private float rotation;
        private Vector2 scale;
        private Vector2 origin;
        private int speed;

        private int frameNumber;
        private int rowNumber;

        private Rectangle clip;
        private bool active;

        private float totalTime;

        private Point frameSize;
        private Point currentFrame;
        private Point sheetSize;

        private float drawDepth;

        public Animation(Texture2D texture2D, Vector2 position, float rotation, Vector2 scale,
                Vector2 origin, int speed, int numberOfFrames, int numberOfRows)
        {
            this.texture2D = texture2D;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            this.origin = origin;
            this.speed = speed;

            frameNumber = 0;
            rowNumber = 0;

            sheetSize = new Point(numberOfFrames, numberOfRows);
            frameSize = new Point(texture2D.Width / numberOfFrames, texture2D.Height / numberOfRows);
            currentFrame = new Point(frameNumber / sheetSize.X * texture2D.Width, rowNumber / sheetSize.Y * texture2D.Height);

            active = false;
            clip = new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        public void setDrawDepth(float drawDepth)
        {
            this.drawDepth = drawDepth;
        }

        public void setFrameNumber(int frameNumber)
        {
            this.frameNumber = frameNumber;
        }

        public void setRowNumber(int rowNumber)
        {
            this.rowNumber = rowNumber;
        }

        public void start(int rowNumber, int speed)
        {
            // If already active, set new direction, else start the animation and set direction
            if (active && this.rowNumber != rowNumber)
            {
                this.rowNumber = rowNumber;

                currentFrame.Y = (int)((float)(rowNumber) / (float)(sheetSize.Y) * (float)(texture2D.Height));
                clip = new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y);
            }
            else
            {
                active = true;

                this.rowNumber = rowNumber;
                this.speed = speed;

                currentFrame.Y = (int)((float)(rowNumber) / (float)(sheetSize.Y) * (float)(texture2D.Height));
                clip = new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y);
            }
        }

        public void stop()
        {
            if (active)
            {
                active = false;

                currentFrame.X = 0;
                currentFrame.Y = (int)((float)(rowNumber) / (float)(sheetSize.Y) * (float)(texture2D.Height));
                clip = new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y);

                totalTime = 0;
            }
        }

        public void update(Vector2 position, float drawDepth, GameTime gameTime)
        {
            this.position = position;
            this.drawDepth = drawDepth;

            if (active)
            {
                totalTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (totalTime >= speed)
                {
                    if (frameNumber >= sheetSize.X)
                    {
                        frameNumber = 0;
                    }

                    currentFrame.X = (int)((float)(frameNumber) / (float)(sheetSize.X) * (float)(texture2D.Width));
                    currentFrame.Y = (int)((float)(rowNumber) / (float)(sheetSize.Y) * (float)(texture2D.Height));

                    clip = new Rectangle(currentFrame.X, currentFrame.Y, frameSize.X, frameSize.Y);

                    totalTime = 0;
                    frameNumber++;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, position, clip, Color.White, rotation, origin, scale, SpriteEffects.None, drawDepth);
        }
    }
}