using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class LevelHandler
    {
        private static readonly LevelHandler instance = new LevelHandler();

        private string quest1Description;
        private string quest2Description;
        private bool quest1Finished;
        private bool quest2Finished;
        private bool allQuestsFinished;
        private int numberOfCorn;
        private int numberOfApples;

        private LevelHandler()
        {
            //Console.Write("LEVEL CONS");
            quest1Description = "";
            quest2Description = "";
            quest1Finished = false;
            quest2Finished = false;
            allQuestsFinished = false;
        }

        public static LevelHandler getInstance
        {
            get
            {
                return instance;
            }
        }

        public void setNumberOfCorn(int numberOfCorn)
        {
            this.numberOfCorn = numberOfCorn;
        }

        public void setNumberOfApples(int numberOfApples)
        {
            this.numberOfApples = numberOfApples;
        }

        public string getQuest1Description()
        {
            return quest1Description;
        }

        public string getQuest2Description()
        {
            return quest2Description;
        }

        public bool isQuest1Finished()
        {
            return quest1Finished;
        }

        public bool isQuest2Finished()
        {
            return quest2Finished;
        }

        public bool isAllQuestsFinished()
        {
            return allQuestsFinished;
        }

        public int getNumberOfCorn()
        {
            return numberOfCorn;
        }

        public int getNumberOfApples()
        {
            return numberOfApples;
        }

        public void startMissions()
        {
            Vector2 playerPos = SceneManager.getInstance.getPlayer1().Properties.getProperty<Vector2>("Position");

            quest1Description = "1. Use your pickaxe to collect 100 corn from the\nfields";
            quest2Description = "2. Fix the dead trees using the tech gun and collect\n75 apples from these";
            quest1Finished = false;
            quest2Finished = false;

            SceneManager.getInstance.getFontManager().addFont(
                    new Font(EFontType.STATIC, 11, "FontQuestHudQuest1", ResourceManager.getInstance.Font2, quest1Description,
                        new Vector2(playerPos.X - 340, playerPos.Y - 150), Color.White, -1.0f));

            SceneManager.getInstance.getFontManager().addFont(
                new Font(EFontType.STATIC, 12, "FontQuestHudQuest2", ResourceManager.getInstance.Font2, quest2Description,
                    new Vector2(playerPos.X - 340, playerPos.Y - 105), Color.White, -1.0f));
        }

        public void update(GameTime gameTime)
        {
            Character player1 = SceneManager.getInstance.getPlayer1();
            Vector2 playerPos = player1.Properties.getProperty<Vector2>("Position");

           // Console.WriteLine("Corn: " + numberOfCorn);
           // Console.WriteLine("Apple: " + numberOfApples);

            /*
             * We check if there are enough corn and apples in the granary to complete the quests
             * */
            if (!quest1Finished)
            {
                if (numberOfCorn >= 100)
                {
                    quest1Finished = true;

                    //Adds a font
                    SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, 14, "FontQuest1Completed",
                        ResourceManager.getInstance.Font1, "You completed quest 1",
                            new Vector2(playerPos.X - 350, playerPos.Y + 100), Color.Green, 4.0f));
                }
            }

            if (!quest2Finished)
            {
                if (numberOfApples >= 75)
                {
                    quest2Finished = true;

                    //Adds a font
                    SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, 15, "FontQuest2Completed",
                        ResourceManager.getInstance.Font1, "You completed quest 2",
                            new Vector2(playerPos.X - 350, playerPos.Y + 100), Color.Green, 4.0f));
                }
            }

            allQuestsFinished = quest1Finished && quest2Finished;

            if (allQuestsFinished)
            {
                Console.WriteLine("All quests are finished");
                SceneManager.getInstance.getMenuManager().findIndex("WinScreenMenu");

                //Gå til en meny
            }
        }
    }
}