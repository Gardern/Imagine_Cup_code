using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class FontManager
    {
        private List<Font> spriteFonts; //List including all the fonts that are drawn

        public FontManager()
        {
            spriteFonts = new List<Font>();
        }

        /*
         * This method is used to add fonts
         * */
        public void addFont(Font fontData)
        {
            for(int i = 0; i < spriteFonts.Count; i++)
            {
                //We check if there already is a font with this id and it is static
                if(spriteFonts[i].getId() == fontData.getId() && spriteFonts[i].getFontType() == EFontType.STATIC)
                {
                    spriteFonts.RemoveAt(i);
                }
            }

            //Console.WriteLine("addFont Add");
            spriteFonts.Add(fontData);

            //Console.WriteLine("BUILD");
        }

        public void update(GameTime gameTime)
        {
            //foreach (Font font in spriteFonts)
            //{
            //    font.update(gameTime);
            //}

            //Check if any fonts are unactive, and removes them if so, else we update the font
            //Loops backwards to not miss any elements
            for (int i = spriteFonts.Count() - 1; i > -1; i--)
            {
                if (!spriteFonts.ElementAt(i).getActive())
                {
                    spriteFonts.RemoveAt(i);
                }
                else
                {
                    spriteFonts.ElementAt(i).update(gameTime);
                }
            }

            //Console.WriteLine("Spritefonts size: " + spriteFonts.Count);
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            foreach (Font font in spriteFonts)
            {
                font.onCollision(gameTime, collisionResult, player1Velocity);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Console.WriteLine("Drawing");

            foreach (Font font in spriteFonts)
            {
                font.draw(spriteBatch);
            }
        }
    }
}