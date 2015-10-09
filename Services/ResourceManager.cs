using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using GLEED2D;

namespace ImagineCup2012
{
    public class ResourceManager
    {
        private static readonly ResourceManager instance = new ResourceManager();

        private ContentManager content;
        private List<Level> levels;
        private List<Level> menus;
        private List<Level> huds;
        private Dictionary<string, Texture2D> runtimeTextures; //Textures loaded runtime
        public SpriteFont Font1;
        public SpriteFont Font2;

        private ResourceManager()
        {
            levels = new List<Level>();
            menus = new List<Level>();
            huds = new List<Level>();
            runtimeTextures = new Dictionary<string, Texture2D>();
        }

        public static ResourceManager getInstance
        {
            get
            {
                return instance;
            }
        }

        public ContentManager getContent()
        {
            return content;
        }

        public List<Level> getLevels()
        {
            return levels;
        }

        public List<Level> getMenus()
        {
            return menus;
        }

        public List<Level> getHuds()
        {
            return huds;
        }

        public Dictionary<string, Texture2D> getRuntimeTextures()
        {
            return runtimeTextures;
        }

        public void start(ContentManager content)
        {
            Console.WriteLine("ResourceManager.start");

            this.content = content;
            List<string> sLevels;
            List<string> sMenus;
            List<string> sHuds;

            SettingsManager.getInstance.getLevels(out sLevels);
            SettingsManager.getInstance.getMenus(out sMenus);
            SettingsManager.getInstance.getHuds(out sHuds);

            //Loading levels
            if (sLevels.Count > 0)
            {
                foreach (string sLevel in sLevels)
                {
                    Level level = Level.FromFile("Content//Xml//Levels//" + sLevel, this.content);
                    level.copyFromItemList();
                    levels.Add(level);
                }
            }
            else
            {
                Console.WriteLine("FAILED TO LOAD LEVELS");
            }

            //Loading menus
            if (sMenus.Count > 0)
            {
                foreach (string sMenu in sMenus)
                {
                    Level menu = Level.FromFile("Content//Xml//Menus//" + sMenu, this.content);
                    menu.copyFromItemList();
                    menus.Add(menu);
                }
            }
            else
            {
                Console.WriteLine("FAILED TO LOAD MENUS");
            }

            //Loading hud
            if (sHuds.Count > 0)
            {
                foreach (string sHud in sHuds)
                {
                    Level hud = Level.FromFile("Content//Xml//Huds//" + sHud, this.content);
                    hud.copyFromItemList();
                    huds.Add(hud);
                }
            }
            else
            {
                Console.WriteLine("FAILED TO LOAD HUDS");
            }

            runtimeTextures.Add("Shovel", this.content.Load<Texture2D>("Textures//shovelPointer"));
            runtimeTextures.Add("Pickaxe", this.content.Load<Texture2D>("Textures//pickaxePointer"));
            runtimeTextures.Add("Mouse", this.content.Load<Texture2D>("Textures//mouse"));
            runtimeTextures.Add("FantasyTool", this.content.Load<Texture2D>("Textures//fantasyToolPointer"));
            runtimeTextures.Add("FantasyTool2", this.content.Load<Texture2D>("Textures//fantasyTool2Pointer"));
            runtimeTextures.Add("Tree", this.content.Load<Texture2D>("Textures//tree"));
            runtimeTextures.Add("TreeWithFood", this.content.Load<Texture2D>("Textures//treeWithFood"));
            runtimeTextures.Add("Field", this.content.Load<Texture2D>("Textures//field"));
            runtimeTextures.Add("FieldFertilised", this.content.Load<Texture2D>("Textures//fieldFertilised"));
            runtimeTextures.Add("FieldWithFood", this.content.Load<Texture2D>("Textures//fieldWithFood"));
            runtimeTextures.Add("OilCan", this.content.Load<Texture2D>("Textures//oilCan"));
            runtimeTextures.Add("Screw", this.content.Load<Texture2D>("Textures//screw"));
            runtimeTextures.Add("Cement", this.content.Load<Texture2D>("Textures//cement"));
            runtimeTextures.Add("Shadow", this.content.Load<Texture2D>("Textures//shadow"));
            runtimeTextures.Add("InventoryHudMain", this.content.Load<Texture2D>("Textures//inventoryHudMain"));
            runtimeTextures.Add("InventoryHudOther", this.content.Load<Texture2D>("Textures//inventoryHudOther"));
            runtimeTextures.Add("HudApple", this.content.Load<Texture2D>("Textures//hudApple"));
            runtimeTextures.Add("HudCorn", this.content.Load<Texture2D>("Textures//hudCorn"));
            runtimeTextures.Add("HudCement", this.content.Load<Texture2D>("Textures//hudCement"));
            runtimeTextures.Add("HudScrew", this.content.Load<Texture2D>("Textures//hudScrew"));
            runtimeTextures.Add("HudOilCan", this.content.Load<Texture2D>("Textures//hudOilCan"));
            runtimeTextures.Add("HudFantasyTool", this.content.Load<Texture2D>("Textures//hudFantasyTool"));
            runtimeTextures.Add("HudShovel", this.content.Load<Texture2D>("Textures//hudShovel"));
            runtimeTextures.Add("HudPickaxe", this.content.Load<Texture2D>("Textures//hudPickaxe"));
            runtimeTextures.Add("CurrentQuestsMarked", this.content.Load<Texture2D>("Textures//QuestsHud_CurrentQuests_Marked"));
            runtimeTextures.Add("FinishedQuestsMarked", this.content.Load<Texture2D>("Textures//QuestsHud_FinishedQuests_Marked"));
            runtimeTextures.Add("CurrentQuestsUnmarked", this.content.Load<Texture2D>("Textures//QuestsHud_CurrentQuests_Unmarked"));
            runtimeTextures.Add("FinishedQuestsUnmarked", this.content.Load<Texture2D>("Textures//QuestsHud_FinishedQuests_Unmarked"));
            
            Font1 = this.content.Load<SpriteFont>("Fonts//font1");
            Font2 = this.content.Load<SpriteFont>("Fonts//font2");
        }

        public string loadType(TextureItem item, string property)
        {
            string type = null;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    Console.WriteLine(property + " Loaded");
                    type = (String)item.CustomProperties[property].value;
                }
            }
            else
            {
                Console.WriteLine("Failed to load " + property);
            }
            return type;
        }

        public int loadAnimationProperty(TextureItem item, string property)
        {
            string type = null;
            int tall = 0;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    Console.WriteLine(property + " Loaded");
                    type = (String)item.CustomProperties[property].value;

                    tall = int.Parse(type);
                }
            }
            else
            {
                Console.WriteLine("Failed to load " + property);
            }
            return tall;
        }

        public int[] loadAnimationProperty2(TextureItem item, string property)
        {
            string type = null;
            int[] fArray = null;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    Console.WriteLine(property + " Loaded");
                    type = (String)item.CustomProperties[property].value;

                    string[] array = type.Split(new char[] { ' ' });

                    fArray = new int[array.Count()];
                    for (int i = 0; i < array.Count(); i++)
                    {
                        fArray[i] = int.Parse(array[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to load " + property);
            }
            return fArray;
        }

        public Texture2D[] loadAnimationSprite(TextureItem item, string property)
        {
            Texture2D [] animationSprites = null;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    Console.WriteLine(property + " Loaded");
                    String animationPath = (String)item.CustomProperties[property].value;

                    string[] array = animationPath.Split(new char[] { ' ' });
                    animationSprites = new Texture2D[array.Count()];

                    for (int i = 0; i < array.Count(); i++)
                    {
                        animationSprites[i] = content.Load<Texture2D>(array[i]);
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to load " + property);
            }
            return animationSprites;
        }

        public List<AABB> loadAxisAlignedBoxes(TextureItem item, string property)
        {  
            List<AABB> axisAlignedBoxes = new List<AABB>();
            int numberToLoad = 1;
            int numberOfLoaded = 0;
            bool loadNext = true;

            while (loadNext)
            {
                string tempProperty = property + numberToLoad.ToString();

                if (item.CustomProperties.ContainsKey(tempProperty))
                {
                    if (item.CustomProperties[tempProperty].type == typeof(Item))
                    {
                        Console.WriteLine(tempProperty + " Loaded");
                        RectangleItem rectangleItem = (RectangleItem)item.CustomProperties[tempProperty].value;
                        AABB axisAlignedBox = new AABB(rectangleItem.Position, rectangleItem.Width, rectangleItem.Height);
                        axisAlignedBoxes.Add(axisAlignedBox);
                        numberToLoad++;
                        numberOfLoaded++;
                    }
                }
                else
                {
                    loadNext = false;
                }
            }

            if (numberOfLoaded == 0)
            {
                axisAlignedBoxes = null;
                //Console.WriteLine("Did not load AxisAlignedBoxes");
            }
            return axisAlignedBoxes;
        }

        public List<Polygon> loadPolygons(TextureItem item, string property)
        {
            List<Polygon> polygons = new List<Polygon>();
            int numberToLoad = 1;
            int numberOfLoaded = 0;
            bool loadNext = true;

            while (loadNext)
            {
                string tempProperty = property + numberToLoad.ToString();

                if (item.CustomProperties.ContainsKey(tempProperty))
                {
                    //Console.WriteLine(tempProperty);
                    if (item.CustomProperties[tempProperty].type == typeof(Item))
                    {
                        Console.WriteLine(tempProperty + " Loaded");
                        PathItem pathItem = (PathItem)item.CustomProperties[tempProperty].value;
                        if (pathItem != null)
                        {
                            List<Vector2> liste = new List<Vector2>(pathItem.WorldPoints);
                            Polygon polygon = new Polygon();

                            foreach (Vector2 point in liste)
                            {
                                polygon.getPoints().Add(point);
                            }
                            polygon.buildEdges();
                            polygons.Add(polygon);
                            numberToLoad++;
                            numberOfLoaded++;
                        }
                        else
                        {
                            loadNext = false;
                        }
                    }
                }
                else
                {
                    loadNext = false;
                }
            }

            if (numberOfLoaded == 0)
            {
                polygons = null;
                //Console.WriteLine("Did not load Polygons");
            }
            return polygons;
        }
       
        public float loadDepthPosition(TextureItem item, string property)
        {
            string depthPosition = null;
            float fDepthPosition = 0;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    depthPosition = (String)item.CustomProperties[property].value;
                    if (depthPosition != "")
                    {
                        fDepthPosition = float.Parse(depthPosition);
                    }
                }
            }
            else
            {
                //Console.WriteLine("Failed to load " + property);
            }
            return fDepthPosition;
        }

        public string loadName(TextureItem item, string property)
        {
            string name = null;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(string))
                {
                    name = (String)item.CustomProperties[property].value;
                }
            }
            else
            {
                //Console.WriteLine("Failed to load " + property);
            }
            return name;
        }

        public bool loadIsAnimated(TextureItem item, string property)
        {
            bool isAnimated = false;

            if (item.CustomProperties.ContainsKey(property))
            {
                if (item.CustomProperties[property].type == typeof(bool))
                {
                    isAnimated = (bool)item.CustomProperties[property].value;
                }
            }
            else
            {
                //Console.WriteLine("Failed to load " + property);
            }
            return isAnimated;
        }
    }
}