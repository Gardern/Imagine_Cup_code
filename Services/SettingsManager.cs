using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace ImagineCup2012
{
    public class SettingsManager
    {
        private static readonly SettingsManager instance = new SettingsManager();

        private XmlDocument doc;
        private XmlElement root;
        private List<Keys> keyboardKeys;
        private List<MouseKeys> mouseKeys;

        private SettingsManager()
        {
            doc = new XmlDocument();
            keyboardKeys = new List<Keys>();
            mouseKeys = new List<MouseKeys>();
        }

        public static SettingsManager getInstance
        {
            get
            {
                return instance;
            }
        }

        public List<Keys> getKeyboardKeys()
        {
            return keyboardKeys;
        }

        public List<MouseKeys> getMouseKeys()
        {
            return mouseKeys;
        }

        private void loadXmlFile(string filepath)
        {
            doc.Load(filepath);
            root = doc.DocumentElement;
        }

        public void getControllerSettings()
        {
            Console.WriteLine("Loading controller settings");
            loadXmlFile("Content//Xml//GameData//properties.xml");
            XmlNodeList nodes = root.SelectNodes("game//controller");

            bool finished = false;
            XmlNode keyboardNode = nodes.Item(0)["keyboard"].FirstChild;
            XmlNode mouseNode = nodes.Item(0)["mouse"].FirstChild;

            while (!finished)
            {
                Keys value;
                string key = keyboardNode.InnerText;
                Console.WriteLine("Settings: " + key);

                Input.getInstance.getkeyboardKeys().TryGetValue(key, out value);
                keyboardKeys.Add(value);

                keyboardNode = keyboardNode.NextSibling;

                if (keyboardNode == null)
                {
                    finished = true;
                }
            }

            finished = false;

            while (!finished)
            {
                MouseKeys value;
                string key = mouseNode.InnerText;
                Console.WriteLine("Settings: " + key);

                Input.getInstance.getMouseKeys().TryGetValue(key, out value);
                mouseKeys.Add(value);

                mouseNode = mouseNode.NextSibling;

                if (mouseNode == null)
                {
                    finished = true;
                }
            }
        }

        public void getGraphicsSettings(out int height, out int width, out int fps, out bool fullscreen)
        {
            XmlNodeList nodes;
            Console.WriteLine("Loading graphics settings");

            loadXmlFile("Content//Xml//GameData//properties.xml");
            nodes = root.SelectNodes("game//graphics//resolution");

            height = int.Parse(nodes.Item(0)["height"].InnerText);
            width = int.Parse(nodes.Item(0)["width"].InnerText);

            loadXmlFile("Content//Xml//GameData//properties.xml");
            nodes = root.SelectNodes("game//graphics");

            fps = int.Parse(nodes.Item(0)["fps"].InnerText);
            fullscreen = bool.Parse(nodes.Item(0)["fullscreen"].InnerText);
        }

        
        public void getLevels(out List<string> levels)
        {
            Console.WriteLine("Loading level from XML file");
            levels = new List<string>();
            string level = "";
            loadXmlFile("Content//Xml//GameData//properties.xml");
            XmlNodeList nodes = root.SelectNodes("game");;
            
            bool finished = false;
            XmlNode levelNode = nodes.Item(0)["levels"].FirstChild;

            while (!finished)
            {
                level = levelNode.InnerText;
                Console.WriteLine("Settings: " + level);
                levels.Add(level);

                levelNode = levelNode.NextSibling;

                if (levelNode == null)
                {
                    finished = true;
                }
            }
        }

        public void getMenus(out List<string> menus)
        {
            Console.WriteLine("Loading menus from XML file");
            menus = new List<string>();
            string menu = "";
            loadXmlFile("Content//Xml//GameData//properties.xml");
            XmlNodeList nodes = root.SelectNodes("game"); ;

            bool finished = false;
            XmlNode levelNode = nodes.Item(0)["menus"].FirstChild;

            while (!finished)
            {
                menu = levelNode.InnerText;
                Console.WriteLine("Settings: " + menu);
                menus.Add(menu);

                levelNode = levelNode.NextSibling;

                if (levelNode == null)
                {
                    finished = true;
                }
            }
        }

        public void getHuds(out List<string> huds)
        {
            Console.WriteLine("Loading menus from XML file");
            huds = new List<string>();
            string hud = "";
            loadXmlFile("Content//Xml//GameData//properties.xml");
            XmlNodeList nodes = root.SelectNodes("game"); ;

            bool finished = false;
            XmlNode levelNode = nodes.Item(0)["huds"].FirstChild;

            while (!finished)
            {
                hud = levelNode.InnerText;
                Console.WriteLine("Settings: " + hud);
                huds.Add(hud);

                levelNode = levelNode.NextSibling;

                if (levelNode == null)
                {
                    finished = true;
                }
            }
        }
    }
}