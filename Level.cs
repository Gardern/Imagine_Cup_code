/*
 * Microsoft Public License (Ms-PL)

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.

1. Definitions

The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the software.

A "contributor" is any person that distributes its contribution under this license.

"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights

(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.

(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations

(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.

(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.

(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.

(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.

(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
 * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using ImagineCup2012;

namespace GLEED2D
{
    public partial class Level
    {
        /// <summary>
        /// The name of the level.
        /// </summary>
        [XmlAttribute()]
        public String Name;

        [XmlAttribute()]
        public bool Visible;

        /// <summary>
        /// A Level contains several Layers. Each Layer contains several Items.
        /// </summary>
        public List<Layer> Layers;

        /// <summary>
        /// A Dictionary containing any user-defined Properties.
        /// </summary>
        public SerializableDictionary CustomProperties;


        public Level()
        {
            Visible = true;
            Layers = new List<Layer>();
            CustomProperties = new SerializableDictionary();
        }

        public static Level FromFile(string filename, ContentManager cm)
        {
            FileStream stream = File.Open(filename, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            Level level = (Level)serializer.Deserialize(stream);
            stream.Close();

            foreach (Layer layer in level.Layers)
            {
                foreach (Item item in layer.Items)
                {
                    item.CustomProperties.RestoreItemAssociations(level);
                    item.load(cm);
                }
            }

            return level;
        }

        public Item getItemByName(string name)
        {
            foreach (Layer layer in Layers)
            {
                foreach (Item item in layer.Items)
                {
                    if (item.Name == name) return item;
                }
            }
            return null;
        }

        public Layer getLayerByName(string name)
        {
            foreach (Layer layer in Layers)
            {
                if (layer.Name == name) return layer;
            }
            return null;
        }

        //Returns a list of TextureItems with the specified assetName
        public List<TextureItem> getListOfTextureItems(string assetName)
        {
            List<TextureItem> textureItems = new List<TextureItem>();

            foreach (Layer layer in Layers)
            {
                foreach (TextureItem textureItem in layer.textureItems)
                {
                    if (textureItem.asset_name == assetName)
                    {
                        textureItems.Add(textureItem);
                    }
                }
            }
            return textureItems;
        }

        //Searches through all TextureItems with the specific propertyName and returns a list of
        //all TextureItems with the specific objectName
        public List<TextureItem> getTextureItems(string propertyName, string objectName)
        {
            List<TextureItem> textureItems = new List<TextureItem>();
            int counter = 0;

            foreach (Layer layer in Layers)
            {
                foreach (TextureItem textureItem in layer.textureItems)
                {
                    if (textureItem.CustomProperties.ContainsKey(propertyName))
                    {
                        if (textureItem.CustomProperties[propertyName].type == typeof(String))
                        {
                            String thisObjectName = (String)textureItem.CustomProperties[propertyName].value;

                            if (thisObjectName == objectName)
                            {
                                textureItems.Add(textureItem);
                                counter++;
                            }
                        }
                    }
                }
            }
            if (counter == 0)
            {
                Console.WriteLine("Failed to load objects of type " + objectName);
            }
            else
            {
                Console.WriteLine("Loaded " + counter + " objects of type " + objectName);
            }
            return textureItems;
        }

        //Returns all texture items in a level file
        public List<TextureItem> getAllTextureItems()
        {
            List<TextureItem> textureItems = new List<TextureItem>();

            foreach (Layer layer in Layers)
            {
                foreach (TextureItem textureItem in layer.textureItems)
                {
                    textureItems.Add(textureItem);
                }
            }
            return textureItems;
        }
        /*
        public void setCollisionBoxesForStaticObjects
            (string propertyName, List<TextureItem> textureItems, List<StaticObject> staticObjects)
        {
            foreach (TextureItem items in textureItems)
            {
                CollisionBox collisionBox = null;

                if (items.CustomProperties.ContainsKey(propertyName))
                {
                    if (items.CustomProperties[propertyName].type == typeof(Item))
                    {
                        Console.WriteLine("StaticObject CollisionBox");
                        RectangleItem rectangleItem = (RectangleItem)items.CustomProperties[propertyName].value;
                        collisionBox = new CollisionBox(rectangleItem.Position, rectangleItem.Width, rectangleItem.Height);

                    }
                }
                staticObjects.Add(new StaticObject(items.texture, items.asset_name, items.Name, items.Visible, items.Position, new Vector2(8, 8),
                    items.Rotation, items.Scale, items.Origin, collisionBox));
            }
        }
        */
        public void copyFromItemList()
        {
            foreach (Layer layer in Layers)
            {
                foreach (Item item in layer.Items)
                {
                    if (item is TextureItem)
                    {
                        TextureItem textureItem = (TextureItem)item;
                        layer.textureItems.Add(textureItem);
                    }
                    else if (item is RectangleItem)
                    {
                        RectangleItem rectangleItem = (RectangleItem)item;
                        layer.rectangleItems.Add(rectangleItem);
                    }
                    else if (item is PathItem)
                    {
                        PathItem pathItem = (PathItem)item;
                        layer.pathItems.Add(pathItem);
                    }
                    else if (item is CircleItem)
                    {
                        CircleItem circleItem = (CircleItem)item;
                        layer.circleItems.Add(circleItem);
                    }
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            foreach (Layer layer in Layers) layer.draw(sb);
        }
    }

    public partial class Layer
    {
        /// <summary>
        /// The name of the layer.
        /// </summary>
        [XmlAttribute()]
        public String Name;

        /// <summary>
        /// Should this layer be visible?
        /// </summary>
        [XmlAttribute()]
        public bool Visible;

        /// <summary>
        /// The list of the items in this layer.
        /// </summary>
        public List<Item> Items;

        public List<TextureItem> textureItems;
        public List<RectangleItem> rectangleItems;
        public List<CircleItem> circleItems;
        public List<PathItem> pathItems;

        /// <summary>
        /// The Scroll Speed relative to the main camera. The X and Y components are 
        /// interpreted as factors, so (1;1) means the same scrolling speed as the main camera.
        /// Enables parallax scrolling.
        /// </summary>
        public Vector2 ScrollSpeed;


        public Layer()
        {
            Items = new List<Item>();
            textureItems = new List<TextureItem>();
            rectangleItems = new List<RectangleItem>();
            circleItems = new List<CircleItem>();
            pathItems = new List<PathItem>();

            ScrollSpeed = Vector2.One;
        }

        public void draw(SpriteBatch sb)
        {
            if (!Visible) return;
            foreach (Item item in Items) item.draw(sb);
        }
    }

    [XmlInclude(typeof(TextureItem))]
    [XmlInclude(typeof(RectangleItem))]
    [XmlInclude(typeof(CircleItem))]
    [XmlInclude(typeof(PathItem))]
    public partial class Item
    {
        /// <summary>
        /// The name of this item.
        /// </summary>
        [XmlAttribute()]
        public String Name;

        /// <summary>
        /// Should this item be visible?
        /// </summary>
        [XmlAttribute()]
        public bool Visible;

        /// <summary>
        /// The item's position in world space.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// A Dictionary containing any user-defined Properties.
        /// </summary>
        public SerializableDictionary CustomProperties;


        public Item()
        {
            CustomProperties = new SerializableDictionary();
        }

        /// <summary>
        /// Called by Level.FromFile(filename) on each Item after the deserialization process.
        /// Should be overriden and can be used to load anything needed by the Item (e.g. a texture).
        /// </summary>
        public virtual void load(ContentManager cm)
        {
        }

        public virtual void draw(SpriteBatch sb)
        {
        }
    }


    public partial class TextureItem : Item
    {
        /// <summary>
        /// The item's rotation in radians.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The item's scale vector.
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// The color to tint the item's texture with (use white for no tint).
        /// </summary>
        public Color TintColor;

        /// <summary>
        /// If true, the texture is flipped horizontally when drawn.
        /// </summary>
        public bool FlipHorizontally;

        /// <summary>
        /// If true, the texture is flipped vertically when drawn.
        /// </summary>
        public bool FlipVertically;

        /// <summary>
        /// The path to the texture's filename (including the extension) relative to ContentRootFolder.
        /// </summary>
        public String texture_filename;

        /// <summary>
        /// The texture_filename without extension. For using in Content.Load<Texture2D>().
        /// </summary>
        public String asset_name;

        /// <summary>
        /// The XNA texture to be drawn. Can be loaded either from file (using "texture_filename") 
        /// or via the Content Pipeline (using "asset_name") - then you must ensure that the texture
        /// exists as an asset in your project.
        /// Loading is done in the Item's load() method.
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// The item's origin relative to the upper left corner of the texture. Usually the middle of the texture.
        /// Used for placing and rotating the texture when drawn.
        /// </summary>
        public Vector2 Origin;

        public TextureItem()
        {
            //collisionBoxes = new List<CollisionBox>();
        }

        /// <summary>
        /// Called by Level.FromFile(filename) on each Item after the deserialization process.
        /// Loads all assets needed by the TextureItem, especially the Texture2D.
        /// You must provide your own implementation. However, you can rely on all public fields being
        /// filled by the level deserialization process.
        /// </summary>
        public override void load(ContentManager cm)
        {
            //throw new NotImplementedException();

            //TODO: provide your own implementation of how a TextureItem loads its assets
            //for example:
            //this.texture = Texture2D.FromFile(<GraphicsDevice>, texture_filename);
            //or by using the Content Pipeline:
            this.texture = cm.Load<Texture2D>(asset_name);

            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        
        public override void draw(SpriteBatch sb)
        {
            if (!Visible) return;
            SpriteEffects effects = SpriteEffects.None;
            if (FlipHorizontally) effects |= SpriteEffects.FlipHorizontally;
            if (FlipVertically) effects |= SpriteEffects.FlipVertically;
            sb.Draw(texture, Position, null, TintColor, Rotation, Origin, Scale, effects, 0);
        }
    }


    public partial class RectangleItem : Item
    {
        public float Width;
        public float Height;
        public Color FillColor;

        public RectangleItem()
        {
        }
    }


    public partial class CircleItem : Item
    {
        public float Radius;
        public Color FillColor;

        public CircleItem()
        {
        }
    }


    public partial class PathItem : Item
    {
        public Vector2[] LocalPoints;
        public Vector2[] WorldPoints;
        public bool IsPolygon;
        public int LineWidth;
        public Color LineColor;

        public PathItem()
        {
        }
    }


    ///////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////
    //
    //    NEEDED FOR SERIALIZATION. YOU SHOULDN'T CHANGE ANYTHING BELOW!
    //
    ///////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////////


    public class CustomProperty
    {
        public string name;
        public object value;
        public Type type;
        public string description;

        public CustomProperty()
        {
        }

        public CustomProperty(string n, object v, Type t, string d)
        {
            name = n;
            value = v;
            type = t;
            description = d;
        }

        public CustomProperty clone()
        {
            CustomProperty result = new CustomProperty(name, value, type, description);
            return result;
        }
    }

    public class SerializableDictionary : Dictionary<String, CustomProperty>, IXmlSerializable
    {

        public SerializableDictionary()
            : base()
        {

        }

        public SerializableDictionary(SerializableDictionary copyfrom)
            : base(copyfrom)
        {
            string[] keyscopy = new string[Keys.Count];
            Keys.CopyTo(keyscopy, 0);
            foreach (string key in keyscopy)
            {
                this[key] = this[key].clone();
            }
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty) return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                CustomProperty cp = new CustomProperty();
                cp.name = reader.GetAttribute("Name");
                cp.description = reader.GetAttribute("Description");

                string type = reader.GetAttribute("Type");
                if (type == "string") cp.type = typeof(string);
                if (type == "bool") cp.type = typeof(bool);
                if (type == "Vector2") cp.type = typeof(Vector2);
                if (type == "Color") cp.type = typeof(Color);
                if (type == "Item") cp.type = typeof(Item);

                if (cp.type == typeof(Item))
                {
                    cp.value = reader.ReadInnerXml();
                    this.Add(cp.name, cp);
                }
                else
                {
                    reader.ReadStartElement("Property");
                    XmlSerializer valueSerializer = new XmlSerializer(cp.type);
                    object obj = valueSerializer.Deserialize(reader);
                    cp.value = Convert.ChangeType(obj, cp.type);
                    this.Add(cp.name, cp);
                    reader.ReadEndElement();
                }

                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (String key in this.Keys)
            {
                writer.WriteStartElement("Property");
                writer.WriteAttributeString("Name", this[key].name);
                if (this[key].type == typeof(string)) writer.WriteAttributeString("Type", "string");
                if (this[key].type == typeof(bool)) writer.WriteAttributeString("Type", "bool");
                if (this[key].type == typeof(Vector2)) writer.WriteAttributeString("Type", "Vector2");
                if (this[key].type == typeof(Color)) writer.WriteAttributeString("Type", "Color");
                if (this[key].type == typeof(Item)) writer.WriteAttributeString("Type", "Item");
                writer.WriteAttributeString("Description", this[key].description);

                if (this[key].type == typeof(Item))
                {
                    Item item = (Item)this[key].value;
                    if (item != null) writer.WriteString(item.Name);
                    else writer.WriteString("$null$");
                }
                else
                {
                    XmlSerializer valueSerializer = new XmlSerializer(this[key].type);
                    valueSerializer.Serialize(writer, this[key].value);
                }
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Must be called after all Items have been deserialized. 
        /// Restores the Item references in CustomProperties of type Item.
        /// </summary>
        public void RestoreItemAssociations(Level level)
        {
            foreach (CustomProperty cp in Values)
            {
                if (cp.type == typeof(Item)) cp.value = level.getItemByName((string)cp.value);
            }
        }
    }
}