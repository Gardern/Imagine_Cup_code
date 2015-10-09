using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ImagineCup2012
{
    public class Character : GameObject
    {
        private bool doAction;
        private bool openInventory;
        private bool openQuests;
        private bool openCurrentQuests;
        private bool openFinishedQuests;
        private Inventory inventory;
        private Lab lab;
        private bool fantasyTool;
        private bool shovel;
        private int numberOfApples;

        public Character()
        {
            Components.Add("PhysicsComponent", new PhysicsComponent(this, new Vector2(), new Vector2()));
        }

        public Character(Texture2D[] textures, Vector2 position, Vector2 acceleration, float rotation, Vector2 origin,
            Vector2 scale, List<Polygon> polygons, List<AABB> axisAlignedBoxes, int numberOfAnimations, int[] animationSpeeds, int[] numberOfFrames,
                int[] numberOfRows, float depthPosition, string name)
        {
            Components.Add("PhysicsComponent", new PhysicsComponent(this, position, acceleration));
            Components.Add("CollisionComponent", new CollisionComponent(this, polygons, axisAlignedBoxes));
            Components.Add("DepthComponent", new DepthComponent(this, depthPosition));
            DrawableComponents.Add("AnimationComponent", new AnimationComponent(this, textures, position, rotation, origin, scale,
                numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows));
         
            ((PhysicsComponent)Components.ElementAt(0).Value).IsMoveable = true;

            Name = name;
            inventory = new Inventory(new ResourceItem());
            fantasyTool = false;
            shovel = true;
            openInventory = true;
            openQuests = false;
            openCurrentQuests = false;
            openFinishedQuests = false;

            //Adds the players starting equipment to the inventory
            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "HudPickaxe")
                {
                    SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(EItemType.PICKAXE, texture.Value,
                        new Vector2(), 0.0f, new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 1, ESlotType.MAIN);
                }
            }
            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "HudShovel")
                {
                    SceneManager.getInstance.getHudManager().getInventoryHud().insertItem(new ItemHud(EItemType.SHOVEL, texture.Value, 
                        new Vector2(), 0.0f, new Vector2(texture.Value.Width / 2, texture.Value.Height / 2), new Vector2(1, 1)), 1, ESlotType.MAIN);
                }
            }
        }

        public void setDoAction(bool doAction)
        {
            this.doAction = doAction;
        }

        public void setOpenInventory(bool openInventory)
        {
            this.openInventory = openInventory;
        }

        public void setOpenQuests(bool openQuests)
        {
            this.openQuests = openQuests;
        }

        public void setOpenCurrentQuests(bool openCurrentQuests)
        {
            this.openCurrentQuests = openCurrentQuests;
        }

        public void setOpenFinishedQuests(bool openFinishedQuests)
        {
            this.openFinishedQuests = openFinishedQuests;
        }

        public void setMovement(bool[] movement)
        {
            ((PhysicsComponent)Components.ElementAt(0).Value).Movement = movement;
        }

        public void setLab(Lab lab)
        {
            this.lab = lab;
        }

        public void setFantasyTool(bool fantasyTool)
        {
            this.fantasyTool = fantasyTool;
        }

        public void setNumberOfApples(int numberOfApples)
        {
            this.numberOfApples = numberOfApples;
        }

        public bool getDoAction()
        {
            return doAction;
        }

        public bool getOpenInventory()
        {
            return openInventory;
        }

        public bool getOpenQuests()
        {
            return openQuests;
        }

        public bool getOpenCurrentQuests()
        {
            return openCurrentQuests;
        }

        public bool getOpenFinishedQuests()
        {
            return openFinishedQuests;
        }

        public Inventory getInventory()
        {
            return inventory;
        }

        public Lab getLab()
        {
            return lab;
        }

        public int getNumberOfApples()
        {
            return numberOfApples;
        }

        public bool hasFantasyTool()
        {
            return fantasyTool;
        }

        public bool hasShovel()
        {
            return shovel;
        }

        public void createNewInventory(Inventory  inventory)
        {
            this.inventory = inventory;
        }

        /*
        public Camera getCameraInGame()
        {
            Vector2 playerPos = this.Properties.getProperty<Vector2>("Position");

            return new Camera(Matrix.Identity, new Vector2(Renderer.getInstance.getScreenCenter().X - playerPos.X,
                            Renderer.getInstance.getScreenCenter().Y - playerPos.Y));
        }
        */
        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 velocity)
        {
            Vector2 position = Properties.getProperty<Vector2>("Position");
            bool hasCollided = Properties.getProperty<bool>("HasCollided");
            List<AABB> axisAlignedBoxes = Properties.getProperty<List<AABB>>("AABB");
            List<Polygon> polygons = Properties.getProperty<List<Polygon>>("Polygon");
            List<Animation> animations = Properties.getProperty<List<Animation>>("Animations");
            float drawDepth = Properties.getProperty<float>("DrawDepth");
            float depthPosition = Properties.getProperty<float>("DepthPosition");

            hasCollided = true;

            velocity = velocity / 2;

            velocity = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds + collisionResult.MinimumTranslationVector;
            position += velocity;
   
            Vector2 shadowPos = new Vector2(position.X - 11, position.Y - 8);
            SceneManager.getInstance.getPlayer1Shadow().Properties.updateProperty<Vector2>("Position", shadowPos);

            foreach (Polygon polygon in polygons)
            {
                polygon.offset(velocity);
                polygon.buildEdges();
            }

            depthPosition += velocity.Y;
            drawDepth = depthPosition / (SceneManager.getInstance.getAxisAlignedBox().getPosition().Y + SceneManager.getInstance.getAxisAlignedBox().getHeight());

            Properties.updateProperty<Vector2>("Position", position);
            Properties.updateProperty<bool>("HasCollided", hasCollided);
            Properties.updateProperty<List<AABB>>("AABB", axisAlignedBoxes);
            Properties.updateProperty<List<Polygon>>("Polygon", polygons);
            Properties.updateProperty<float>("DrawDepth", drawDepth);
            Properties.updateProperty<float>("DepthPosition", depthPosition);
            Properties.updateProperty<List<Animation>>("Animations", animations); 
        }

        public void onNotCollision()
        {
            bool hasCollided = Properties.getProperty<bool>("HasCollided");

            hasCollided = false;

            Properties.updateProperty<bool>("HasCollided", hasCollided);
        }
    }
}