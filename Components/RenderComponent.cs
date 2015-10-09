using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class RenderComponent : DrawableGameObjectComponent
    {
        public bool IsMouse { get; set; }
        public bool IsMenu { get; set; }
        public bool Rotate { get; set; }
        public bool IsBlending { get; set; }
        public Texture2D BlendingTexture { get; set; }
        public Color BlendingColor { get; set; }
        public string BlenderName { get; set; }
        public float BlendingTime { get; set; }
        public byte BlendingAlphaByte { get; set; }

        private float deltaTime;

        private Texture2D texture2D;
        private Vector2 position;
        private float rotation;
        private Vector2 origin;
        private Vector2 scale;
        private float drawDepth;
        private Color color;
        private Color[] originalColor;

        public RenderComponent(GameObject gameObject, Texture2D texture2D, Vector2 position, float rotation, Vector2 origin,
            Vector2 scale, float drawDepth)
            : base(gameObject)
        {
            Name = "RenderComponent";

            IsMouse = false;
            IsMenu = false;
            Rotate = false;
            IsBlending = false;
            BlendingTexture = null;
            BlendingColor = Color.White;
            BlenderName = "";
            BlendingTime = 0.0f;
            BlendingAlphaByte = 0;
            this.drawDepth = drawDepth;

            deltaTime = 0.0f;

            if (texture2D != null)
            {
                int size = texture2D.Width * texture2D.Height;
                originalColor = new Color[size];
                texture2D.GetData<Color>(originalColor);
            }
            else
            {
                originalColor = null;
            }

            ParentObject.Properties.updateProperty<Texture2D>("Texture2D", texture2D);
            ParentObject.Properties.updateProperty<Vector2>("Position", position);
            ParentObject.Properties.updateProperty<float>("Rotation", rotation);
            ParentObject.Properties.updateProperty<Vector2>("Origin", origin);
            ParentObject.Properties.updateProperty<Vector2>("Scale", scale);
            ParentObject.Properties.updateProperty<float>("DrawDepth", this.drawDepth);
            ParentObject.Properties.updateProperty<bool>("IsRenderActive", true);
            ParentObject.Properties.updateProperty<Color>("Color", Color.White);
            ParentObject.Properties.updateProperty<Color[]>("OriginalColor", originalColor);
        }

        public override void update(GameTime gameTime)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsRenderActive");

            if (Active)
            {
                if (IsMouse)
                {
                    position = ParentObject.Properties.getProperty<Vector2>("Position");
                    position = Input.getInstance.getMousePosRelativeToWorld();
                    ParentObject.Properties.updateProperty<Vector2>("Position", position);
                }
                else
                {
                    if (IsBlending)
                    {
                        deltaTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (deltaTime > BlendingTime)
                        {
                            Color color = BlendingColor;
                            BlendingColor = new Color(255, 255, 255, color.A + BlendingAlphaByte);
                            deltaTime = 0.0f;
                        }
                    }
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsRenderActive");

            if (Active)
            {
                texture2D = ParentObject.Properties.getProperty<Texture2D>("Texture2D");
                position = ParentObject.Properties.getProperty<Vector2>("Position");
                rotation = ParentObject.Properties.getProperty<float>("Rotation");
                origin = ParentObject.Properties.getProperty<Vector2>("Origin");
                scale = ParentObject.Properties.getProperty<Vector2>("Scale");
                drawDepth = ParentObject.Properties.getProperty<float>("DrawDepth");
                color = ParentObject.Properties.getProperty<Color>("Color");

                spriteBatch.Draw(texture2D, position, null, color, rotation, origin, scale, SpriteEffects.None, drawDepth);

                if (!IsMouse)
                {
                    if (IsBlending)
                    {
                        if (BlendingTexture != null)
                        {
                            spriteBatch.Draw(BlendingTexture, position, null, BlendingColor, rotation, origin, scale, SpriteEffects.None, drawDepth);

                            if (BlendingColor == Color.White)
                            {
                                IsBlending = false;
                                ParentObject.Properties.updateProperty<Texture2D>("Texture2D", BlendingTexture);
                                ParentObject.Name = BlenderName;

                                if (ParentObject.Name == "FieldFertilised")
                                {
                                    if (!IsBlending)
                                    {
                                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                                        {
                                            if (texture.Key == "FieldWithFood")
                                            {
                                                Console.WriteLine("Start blending");
                                                ParentObject.startBlending(texture.Value, new Color(255, 255, 255, 120), "FieldWithFood", 5.0f, 20);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}