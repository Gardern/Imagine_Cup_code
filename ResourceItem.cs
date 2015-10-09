using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class ResourceItem
    {
        private int screws;
        private int planks;
        private int cement;
        private int oilCans;

        /*
         * Creates a new resource with 0 of each item
         * */
        public ResourceItem()
        {
            this.screws = 0;
            this.planks = 0;
            this.cement = 0;
            this.oilCans = 0;
        }

        /*
         * Creates a new resource with the specified amount of each item
         * */
        public ResourceItem(int screws, int planks, int cement, int oilCans)
        {
            this.screws = screws;
            this.planks = planks;
            this.cement = cement;
            this.oilCans = oilCans;
        }

        public int getScrews()
        {
            return screws;
        }

        public int getPlanks()
        {
            return planks;
        }

        public int getCement()
        {
            return cement;
        }

        public int getOilCans()
        {
            return oilCans;
        }

        public void setScrews(int screws)
        {
            this.screws = screws;
        }

        public void setPlanks(int planks)
        {
            this.planks = planks;
        }

        public void setCement(int cement)
        {
            this.cement = cement;
        }

        public void setOilCans(int oilCans)
        {
            this.oilCans = oilCans;
        }

        /*
         * Increase a resource with byAmount
         * */
        public void increaseResource(string name, int byAmount)
        {
            string key = "";
            EItemType itemType = EItemType.NONE;

            if (name == "Screw")
            {
                screws += byAmount;
                key = "HudScrew";
                itemType = EItemType.SCREW;
            }
            else if (name == "Plank")
            {
                planks += byAmount;
            }
            else if (name == "Cement")
            {
                cement += byAmount;
                key = "HudCement";
                itemType = EItemType.CEMENT;
            }
            else 
            {
                oilCans += byAmount;
                key = "HudOilCan";
                itemType = EItemType.OILCAN;
            }

            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == key)
                {
                    SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(itemType, texture.Value, new Vector2(), 0.0f,
                        new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), byAmount, ESlotType.OTHER);
                    break;
                }
            }
        }

        /*
         * Decrease a resource with byAmount
         * */
        public void decreaseResource(string name, int byAmount)
        {
            string key = "";
            EItemType itemType = EItemType.NONE;

            if (name == "Screw")
            {
                screws -= byAmount;
                key = "HudScrew";
                itemType = EItemType.SCREW;
            }
            else if (name == "Plank")
            {
                planks -= byAmount;
            }
            else if (name == "Cement")
            {
                cement -= byAmount;
                key = "HudCement";
                itemType = EItemType.CEMENT;
            }
            else
            {
                oilCans -= byAmount;
                key = "HudOilCan";
                itemType = EItemType.OILCAN;
            }

            //foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            //{
               // if (texture.Key == key)
               // {
               //     SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(itemType, texture.Value, new Vector2(), 0.0f,
               //         new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), byAmount, ESlotType.OTHER);
                //    break;
               // }
           // }
        }

        /*
         * DEBUG ONLY
         * */
        public void numberOfResources()
        {
            Console.WriteLine("Screws: " + screws);
            Console.WriteLine("Planks: " + planks);
            Console.WriteLine("Cement: " + cement);
            Console.WriteLine("OilCans: " + oilCans);
        }
    }
}