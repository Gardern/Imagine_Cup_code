using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GLEED2D;

namespace ImagineCup2012
{
    public enum GameState { IN_STARTMENU, IN_INGAMEMENU, IN_GAME };

    public class SceneManager
    {
        private static readonly SceneManager instance = new SceneManager();

        private GameState gameState;
        private AABB axisAlignedBox;
        private Character player1;
        private Shadow player1Shadow;
        private MouseCursor mouseCursor;
        private List<BackgroundObject> backgroundObjects;
        
        private List<MiddleObject> middleObjects;
        private List<MiddleObject> resourceObjects;
        private MenuManager menuManager;
        private HudManager hudManager;
        private FontManager fontManager;
        private Camera camera;
        bool collision;
        private List<BackgroundObject> elementsToBuild;
        private List<float> timeBeforeBuildFinish;
        private List<float> counterForEachBuild;
        private List<BuildBackground> objectsToBuild;
        private bool allFieldHaveFood;
        private float timerToResetFields;

        private SceneManager()
        {
            player1 = new Character();
            elementsToBuild = new List<BackgroundObject>();
            timeBeforeBuildFinish = new List<float>();
            counterForEachBuild = new List<float>();
            objectsToBuild = new List<BuildBackground>();
            fontManager = new FontManager();
            allFieldHaveFood = false;
            timerToResetFields = 0.0f;
        }

        public static SceneManager getInstance
        {
            get
            {
                return instance;
            }
        }

        public GameState getGameState()
        {
            return gameState;
        }

        public AABB getAxisAlignedBox()
        {
            return axisAlignedBox;
        }

        public Character getPlayer1()
        {
            return player1;
        }

        public Shadow getPlayer1Shadow()
        {
            return player1Shadow;
        }

        public MouseCursor getMouseCursor()
        {
            return mouseCursor;
        }

        public List<BackgroundObject> getBackgroundObjects()
        {
            return backgroundObjects;
        }

        public List<MiddleObject> getMiddleObjects()
        {
            return middleObjects;
        }

        public MenuManager getMenuManager()
        {
            return menuManager;
        }

        public HudManager getHudManager()
        {
            return hudManager;
        }

        public FontManager getFontManager()
        {
            return fontManager;
        }

        public Camera getCamera()
        {
            return camera;
        }

        public void setGameState(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void setCamera(Camera camera)
        {
            this.camera = camera;
        }

       /*
        * Loads all the game menus
        * */
        private void loadMenus(List<Level> menus)
        {
            gameState = GameState.IN_STARTMENU;
            menuManager = new MenuManager();
            List<TextureItem> textureItems;
            string name = null;

            AudioManager.getInstance.stopSound(new Sound("Level 1 ingame"));
            AudioManager.getInstance.playSound(new Sound("Menu Music"), AudioManager.MAINMENU);

            foreach (Level menu in menus)
            {
                Menu gameMenu = new Menu(menu.Name);
                textureItems = menu.getAllTextureItems();

                foreach (TextureItem item in textureItems)
                {
                    name = ResourceManager.getInstance.loadName(item, "Name");
                    gameMenu.getMenuButtons().Add(new MenuButton(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, name));
                }
                menuManager.getGameMenus().Add(gameMenu);
            }
            menuManager.findIndex("StartMenu");
        }

        public void loadHud(List<Level> huds)
        {
            hudManager = new HudManager();
            List<TextureItem> textureItems;
            string name = null;

            foreach (Level hud in huds)
            {
                //Menu gameMenu = new Menu(menu.Name);
                QuestHud questHud = new QuestHud(hud.Name);
                textureItems = hud.getAllTextureItems();

                foreach (TextureItem item in textureItems)
                {
                    name = ResourceManager.getInstance.loadName(item, "Name");
                    questHud.getButtons().Add(new MenuButton(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, name));
                    //gameMenu.getMenuButtons().Add(new MenuButton(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, name));
                }
                hudManager.setQuestHud(questHud);
                //menuManager.getGameMenus().Add(gameMenu);
                //Console.WriteLine("lol " + gameMenu.getName());
                //gameMenus.Add(gameMenu);
            }

            //Creates the inventory hud
            hudManager.getInventoryHud().createInventoryHud(4, 6, new Vector2(480, 470), 5);
        }

        public void loadLevel(Level level)
        {
            player1 = new Character();
            backgroundObjects = new List<BackgroundObject>();
            middleObjects = new List<MiddleObject>();
            resourceObjects = new List<MiddleObject>();
            collision = false;
            List<TextureItem> textureItems;
            AABB axisAlignedBox = null;
            List<AABB> axisAlignedBoxes = null; 
            List<Polygon> polygons = null;
            float depthPosition = 0.0f;
            string name = null;
            bool isAnimated = false;

            RectangleItem levelCollisionBox = (RectangleItem)level.getItemByName("LevelAABB");
            axisAlignedBox = new AABB(levelCollisionBox.Position, levelCollisionBox.Width, levelCollisionBox.Height);
            this.axisAlignedBox = axisAlignedBox;

            /*
           * Load background objects
            * */
            textureItems = level.getTextureItems("ObjectType", "BackgroundObject");
            foreach (TextureItem item in textureItems)
            {
                polygons = null;
                axisAlignedBoxes = null;

                axisAlignedBoxes = ResourceManager.getInstance.loadAxisAlignedBoxes(item, "CollisionBox"); //AABB
                polygons = ResourceManager.getInstance.loadPolygons(item, "Polygon"); //Polygon
                name = ResourceManager.getInstance.loadName(item, "Name");

                if (axisAlignedBoxes != null)
                {
                    backgroundObjects.Add(new BackgroundObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, null, axisAlignedBoxes, name));
                }
                else if (polygons != null)
                {
                    backgroundObjects.Add(new BackgroundObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, polygons, null, name));
                }
                else
                {
                    backgroundObjects.Add(new BackgroundObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, null, null, name));
                }
            }

            /*
             * Load resource objects
             * */
            textureItems = level.getTextureItems("ObjectType", "ResourceObject");
            foreach (TextureItem item in textureItems)
            {
                depthPosition = ResourceManager.getInstance.loadDepthPosition(item, "DepthPosition");
                name = ResourceManager.getInstance.loadName(item, "Name");

                resourceObjects.Add(new MiddleObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, null, null, depthPosition, name));
            }

            /*
            * Load middle objects
             * */
            textureItems = level.getTextureItems("ObjectType", "MiddleObject");
            foreach (TextureItem item in textureItems)
            {
                polygons = null;
                axisAlignedBoxes = null;

                axisAlignedBoxes = ResourceManager.getInstance.loadAxisAlignedBoxes(item, "CollisionBox"); //AABB
                polygons = ResourceManager.getInstance.loadPolygons(item, "Polygon"); //Polygon
                depthPosition = ResourceManager.getInstance.loadDepthPosition(item, "DepthPosition");
                name = ResourceManager.getInstance.loadName(item, "Name");
                isAnimated = ResourceManager.getInstance.loadIsAnimated(item, "IsAnimated");

                if (!isAnimated)
                {
                    if (axisAlignedBoxes != null)
                    {
                        middleObjects.Add(new MiddleObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, null, axisAlignedBoxes, depthPosition, name));
                    }
                    else if (polygons != null)
                    {
                        middleObjects.Add(new MiddleObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, polygons, null, depthPosition, name));
                    }
                    else
                    {
                        middleObjects.Add(new MiddleObject(item.texture, item.Position, item.Rotation, item.Origin, item.Scale, null, null, depthPosition, name));
                    }
                }
                else
                {
                    Texture2D[] animationSprites = ResourceManager.getInstance.loadAnimationSprite(item, "AnimationPath");
                    int numberOfAnimations = ResourceManager.getInstance.loadAnimationProperty(item, "NumberOfAnimations");
                    int[] animationSpeeds = ResourceManager.getInstance.loadAnimationProperty2(item, "AnimationSpeed");
                    int[] numberOfFrames = ResourceManager.getInstance.loadAnimationProperty2(item, "NumberOfFrames");
                    int[] numberOfRows = ResourceManager.getInstance.loadAnimationProperty2(item, "NumberOfRows");

                    if (name == "Player")
                    {
                        if (axisAlignedBoxes != null && polygons != null)
                        {
                            player1 = new Character(animationSprites, item.Position, new Vector2(4, 4), item.Rotation, item.Origin,
                                item.Scale, polygons, axisAlignedBoxes, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name);
                        }
                        else if (axisAlignedBoxes != null)
                        {
                            player1 = new Character(animationSprites, item.Position, new Vector2(4, 4), item.Rotation, item.Origin,
                                item.Scale, null, axisAlignedBoxes, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name);
                        }
                        else if (polygons != null)
                        {
                            player1 = new Character(animationSprites, item.Position, new Vector2(4, 4), item.Rotation, item.Origin,
                                item.Scale, polygons, null, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name);
                        }

                        player1Shadow = new Shadow(ResourceManager.getInstance.getRuntimeTextures()["Shadow"], new Vector2(item.Position.X - 11, item.Position.Y - 8), item.Rotation, item.Origin, new Vector2(0.2f, 0.2f), "Shadow");

                        /*
                        * Initializes camera and set its view
                        * IMPORTANT: The item used here have to point to the player
                         * */
                        camera = new Camera(Matrix.Identity, new Vector2(Renderer.getInstance.getScreenCenter().X - item.Position.X,
                            Renderer.getInstance.getScreenCenter().Y - item.Position.Y));

                        camera.setView(Matrix.CreateTranslation(new Vector3(camera.getPosition() - Renderer.getInstance.getScreenCenter(), 0f)) *
                            Matrix.CreateTranslation(new Vector3(Renderer.getInstance.getScreenCenter(), 0f)));

                        Console.WriteLine("Camera: " + camera.getPosition());
                        Console.WriteLine("Player: " + item.Position);
                    }
                    else
                    {
                        if (axisAlignedBoxes != null)
                        {
                            middleObjects.Add(new MiddleObject(animationSprites, item.Position, item.Rotation, item.Origin, item.Scale, null,
                                axisAlignedBoxes, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name));
                        }
                        else if (polygons != null)
                        {
                            middleObjects.Add(new MiddleObject(animationSprites, item.Position, item.Rotation, item.Origin, item.Scale, polygons,
                                null, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name));
                        }
                        else
                        {
                            middleObjects.Add(new MiddleObject(animationSprites, item.Position, item.Rotation, item.Origin, item.Scale, null,
                                null, numberOfAnimations, animationSpeeds, numberOfFrames, numberOfRows, depthPosition, name));
                        }
                    }
                }
            }

            foreach (MiddleObject resourceObject in resourceObjects)
            {
                Random random = new Random();

                int placeHere = random.Next(2);
                Console.WriteLine("RANDOM1: " + placeHere);

                if (placeHere == 1)
                {
                    Texture2D texture2D = null;
                    Vector2 position = resourceObject.Properties.getProperty<Vector2>("Position");
                    float rotation = resourceObject.Properties.getProperty<float>("Rotation");
                    Vector2 origin = resourceObject.Properties.getProperty<Vector2>("Origin");
                    Vector2 scale = resourceObject.Properties.getProperty<Vector2>("Scale");
                    float depthPos = resourceObject.Properties.getProperty<float>("DepthPosition");

                    int typeOfResource = random.Next(3);
                    Console.WriteLine("RANDOM2: " + typeOfResource);

                    if (typeOfResource == 0)
                    {
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "Screw")
                            {
                                texture2D = texture.Value;
                                break;
                            }
                        }
                    }
                    else if (typeOfResource == 1)
                    {
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "OilCan")
                            {
                                texture2D = texture.Value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
                        {
                            if (texture.Key == "Cement")
                            {
                                texture2D = texture.Value;
                                break;
                            }
                        }
                    }

                    middleObjects.Add(new MiddleObject(texture2D, position, rotation, origin, scale, null, null, depthPos, resourceObject.Name));
                }
            }

            AudioManager.getInstance.stopSound(new Sound("Menu Music"));
            AudioManager.getInstance.playSound(new Sound("Level 1 ingame"), AudioManager.LEVEL1_INGAME);  
        }

        public void start(Level level, List<Level> menus, List<Level> huds)
        {
            Console.WriteLine("SceneManager.start");

            mouseCursor = new MouseCursor();
            TextureItem textureItem;
            string name = null;

            /*
            * Load mouse cursor
            * */
            Item mouseItem = level.getItemByName("MouseCursor");
            textureItem = (TextureItem)mouseItem;

            name = ResourceManager.getInstance.loadName(textureItem, "Name");
            mouseCursor = new MouseCursor(textureItem.texture, textureItem.Position, textureItem.Rotation, textureItem.Origin,
                textureItem.Scale, name);

            /*
            * Initializes camera and set its view
            * IMPORTANT: The item used here have to point to the player
            * */
            //2389 and 5227 is hard coded players start position
            camera = new Camera(Matrix.Identity, new Vector2(Renderer.getInstance.getScreenCenter().X - 2389,
                Renderer.getInstance.getScreenCenter().Y - 5227));

            camera.setView(Matrix.CreateTranslation(new Vector3(camera.getPosition() - Renderer.getInstance.getScreenCenter(), 0f)) *
                Matrix.CreateTranslation(new Vector3(Renderer.getInstance.getScreenCenter(), 0f)));

            loadMenus(menus);
            loadHud(huds);

            Console.WriteLine("Camera: " + camera.getPosition());
            //Console.WriteLine("Player: " + item.Position);
        }

        public void update(GameTime gameTime)
        {
            if (gameState == GameState.IN_STARTMENU || gameState == GameState.IN_INGAMEMENU)
            {
                checkMouseCollisions();

                mouseCursor.update(gameTime);
            }
            else
            {
                checkCollisions(gameTime);
                checkMouseCollisions();

                player1.update(gameTime);
                mouseCursor.update(gameTime);

                foreach (BackgroundObject backgroundObject in backgroundObjects)
                {
                    backgroundObject.update(gameTime);
                }

                foreach (MiddleObject middleObject in middleObjects)
                {
                    middleObject.update(gameTime);
                }

                camera.update(gameTime);
                menuManager.update(gameTime);
                hudManager.update(gameTime);
                fontManager.update(gameTime);
                LevelHandler.getInstance.update(gameTime);
              
                deleteObjects();
            }
        }

        public void draw(SpriteBatch spriteBatch, GameState gameState)
        {
            if (gameState == GameState.IN_INGAMEMENU)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                foreach (BackgroundObject backgroundObject in backgroundObjects)
                {
                    backgroundObject.draw(spriteBatch);
                }

                player1Shadow.draw(spriteBatch);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                player1.draw(spriteBatch);
                foreach (MiddleObject middleObject in middleObjects)
                {
                    middleObject.draw(spriteBatch);
                }

                spriteBatch.End();
                
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                menuManager.draw(spriteBatch);
                mouseCursor.draw(spriteBatch);

                spriteBatch.End();
            }
            else if (gameState == GameState.IN_STARTMENU)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                menuManager.draw(spriteBatch);
                mouseCursor.draw(spriteBatch);

                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                foreach (BackgroundObject backgroundObject in backgroundObjects)
                {
                    backgroundObject.draw(spriteBatch);
                }
                player1Shadow.draw(spriteBatch);

                spriteBatch.End();
                
                spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                player1.draw(spriteBatch);
                foreach (MiddleObject middleObject in middleObjects)
                {
                    middleObject.draw(spriteBatch);
                }

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SceneManager.getInstance.getCamera().getView());

                hudManager.draw(spriteBatch);
                fontManager.draw(spriteBatch);
                mouseCursor.draw(spriteBatch);

                spriteBatch.End();
            }
        }
        
        private void checkCollisions(GameTime gameTime)
        {
            List<Polygon> player1Polygons = player1.Properties.getProperty<List<Polygon>>("Polygon");
            List<AABB> player1AABB = player1.Properties.getProperty<List<AABB>>("AABB");
            Vector2 player1Velocity = player1.Properties.getProperty<Vector2>("Velocity");

            //Check collision for all middle objects
            foreach (MiddleObject middleObject in middleObjects)
            {
                CollisionType collisionType = middleObject.Properties.getProperty<CollisionType>("CollisionType");

                if (collisionType == CollisionType.POLYGON)
                {
                    List<Polygon> middleObjectPolygons = middleObject.Properties.getProperty<List<Polygon>>("Polygon");

                    foreach (Polygon player1Polygon in player1Polygons)
                    {
                        foreach (Polygon middleObjectPolygon in middleObjectPolygons)
                        {
                            PolygonCollisionResult collisionResult = CollisionManager.intersects(player1Polygon, middleObjectPolygon, player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                            if (collisionResult.WillIntersect)
                            {
                                player1.onCollision(gameTime, collisionResult, player1Velocity);
                                camera.onCollision(gameTime, collisionResult, player1Velocity);
                                middleObject.onCollision();
                                menuManager.onCollision(gameTime, collisionResult, player1Velocity);
                                hudManager.onCollision(gameTime, collisionResult, player1Velocity);
                                fontManager.onCollision(gameTime, collisionResult, player1Velocity);

                                collision = true;
                                break;
                            }
                            else
                            {
                                player1.onNotCollision();
                                middleObject.onNotCollision();
                            }
                        }
                        if (collision)
                        {
                            break;
                        }
                    }
                }
                if (collision)
                {
                    collision = false;
                    break;
                }
                /*
                else if (collisionType == CollisionType.AABB)
                {
                    //Console.WriteLine("Type is AABB");
                    List<AABB> staticObjectAABB = staticObject.Properties.getProperty<List<AABB>>("AABB");

                    foreach (AABB player1Box in player1AABB)
                    {
                        foreach (AABB staticBox in staticObjectAABB)
                        {
                            if (CollisionManager.intersects(player1Box, staticBox))
                            {
                                Console.WriteLine("AABB Collision: " + collision);
                                player1.onCollision(collisionResult, gameTime);
                                camera.setPosition(camera.getOldPosition());
                                doAction(player1, staticObject);

                                jumpOver = true;
                                break;
                            }
                        }
                        if (jumpOver)
                        {
                            jumpOver = false;
                            break;
                        }
                    }
                }
                 * */
            }

            //Check collision for all background objects
            foreach (BackgroundObject backgroundObject in backgroundObjects)
            {
                CollisionType collisionType = backgroundObject.Properties.getProperty<CollisionType>("CollisionType");

                if (collisionType == CollisionType.POLYGON)
                {
                    List<Polygon> backgroundObjectPolygons = backgroundObject.Properties.getProperty<List<Polygon>>("Polygon");

                    foreach (Polygon player1Polygon in player1Polygons)
                    {
                        foreach (Polygon backgroundObjectPolygon in backgroundObjectPolygons)
                        {
                            PolygonCollisionResult collisionResult = CollisionManager.intersects(player1Polygon, backgroundObjectPolygon, player1Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                            if (collisionResult.WillIntersect)
                            {
                                backgroundObject.onCollision();

                                collision = true;
                                break;
                            }
                            else
                            {
                                backgroundObject.onNotCollision();
                            }
                        }
                        if (collision)
                        {
                            break;
                        }
                    }
                }
                if (collision)
                {
                    collision = false;
                    break;
                }
            }

            /*
            foreach (Polygon player1Polygon in player1Polygons)
            {
                if (CollisionManager.checkWallCollision(player1Polygon))
                {
                    player1.onCollision();
                    camera.setPosition(camera.getOldPosition());
                    break;
                }
            }
             * */
            /*
            foreach (AABB player1Box in player1AABB)
            {
                if (CollisionManager.checkWallCollision(player1Box))
                {
                    player1.onCollision();
                    camera.setPosition(camera.getOldPosition());
                    break;
                }
            }
             * */
        }

        private void checkMouseCollisions()
        {
            Vector2 mousePos = mouseCursor.Properties.getProperty<Vector2>("Position");
            Vector2 mouseOrigin = mouseCursor.Properties.getProperty<Vector2>("Origin");

            if (gameState == GameState.IN_STARTMENU || gameState == GameState.IN_INGAMEMENU)
            {
                foreach (MenuButton menuButton in menuManager.getGameMenus().ElementAt(menuManager.getCurrentIndex()).getMenuButtons())
                {
                    Vector2 menuPos = menuButton.Properties.getProperty<Vector2>("Position");
                    Vector2 menuOrigin = menuButton.Properties.getProperty<Vector2>("Origin");

                    if (CollisionManager.intersects(mousePos - mouseOrigin, menuPos - menuOrigin, menuOrigin))
                    {
                        mouseCursor.onMouseHover(menuButton);
                    }
                    else
                    {
                        mouseCursor.onMouseNotHover(menuButton);
                    }
                }
            }
            else
            {
                //Later we could add one more foreach here that loops through the differnet huds and checks if they are open,
                //and if so loop through the huds buttons
                if (getPlayer1().getOpenQuests())
                {
                    foreach (MenuButton button in hudManager.getQuestHud().getButtons())
                    {
                        Vector2 menuPos = button.Properties.getProperty<Vector2>("Position");
                        Vector2 menuOrigin = button.Properties.getProperty<Vector2>("Origin");

                        if (CollisionManager.intersects(mousePos - mouseOrigin, menuPos - menuOrigin, menuOrigin))
                        {
                            mouseCursor.onMouseHover(button);
                        }
                        else
                        {
                            mouseCursor.onMouseNotHover(button);
                        }
                    }
                }

                foreach (MiddleObject middleObject in middleObjects)
                {
                    Vector2 objectPos = middleObject.Properties.getProperty<Vector2>("Position");
                    Vector2 objectOrigin = middleObject.Properties.getProperty<Vector2>("Origin");

                    if (CollisionManager.intersects(mousePos - mouseOrigin, objectPos - objectOrigin, objectOrigin))
                    {
                        mouseCursor.onMouseHover(middleObject);
                    }
                    else
                    {
                        mouseCursor.onMouseNotHover(middleObject);
                    }
                }

                foreach (BackgroundObject backgroundObject in backgroundObjects)
                {
                    Vector2 objectPos = backgroundObject.Properties.getProperty<Vector2>("Position");
                    Vector2 objectOrigin = backgroundObject.Properties.getProperty<Vector2>("Origin");

                    if (CollisionManager.intersects(mousePos - mouseOrigin, objectPos - objectOrigin, objectOrigin))
                    {
                        mouseCursor.onMouseHover(backgroundObject);
                    }
                    else
                    {
                        mouseCursor.onMouseNotHover(backgroundObject);
                    }
                }

                if (!mouseCursor.getHoveringOverThisFrame())
                {
                    mouseCursor.onMouseNotHoverThisFrame();
                }
                mouseCursor.setHoveringOverThisFrame(false);
                
                
                //collision++;
            }
        }

        private bool checkFinishedFields()
        {
            bool allFieldsHaveFood = true;

            foreach (BackgroundObject backgroundObject in backgroundObjects)
            {
                string name = backgroundObject.Name;
  
                //Loops through all fields
                if (name == "Field" || name == "FieldFertilised" || name == "FieldWithFood")
                {
                    if (name == "Field" || name == "FieldFertilised")
                    {
                        allFieldsHaveFood = false;
                        break;
                    }
                }
            }
            return allFieldsHaveFood;
        }

        public void addElementToBuild(BackgroundObject backObject, int index)
        {
            objectsToBuild.Add(new BuildBackground(true, index, backObject));
        }

        private void buildElements()
        {
            foreach (BuildBackground buildBackground in objectsToBuild)
            {
                if (buildBackground.isActive())
                {
                    backgroundObjects.Insert(buildBackground.getIndex(), buildBackground.getBackObject());
                    buildBackground.setActive(false);
                }
            }
        }

        private void deleteElements()
        {
            for(int i = objectsToBuild.Count() - 1; i > -1; i--)
            {
                if(!objectsToBuild.ElementAt(i).isActive())
                {
                    objectsToBuild.RemoveAt(i);
                }
            }
        }

        /*
         * This method starts a new build for background objects
         * timeBeforeFinish: The time it takes to build the object (in seconds)
         * element: The object to build
         * */
        public void startBuild(float timeBeforeFinish, BackgroundObject element)
        {
            Console.WriteLine("StartBuild");
            timeBeforeBuildFinish.Add(timeBeforeFinish);
            counterForEachBuild.Add(0.0f);
            elementsToBuild.Add(element);
        }

        /*
         * This method builds objects that have been started
         * */
        private void buildObjects(GameTime gameTime)
        {
            for (int i = 0; i < counterForEachBuild.Count; i++)
            {
                counterForEachBuild[i] += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (counterForEachBuild[i] >= timeBeforeBuildFinish[i])
                {
                    backgroundObjects.Add(elementsToBuild[i]);
                    elementsToBuild.RemoveAt(i);
                    timeBeforeBuildFinish.RemoveAt(i);
                    counterForEachBuild.RemoveAt(i);
                }
            }
        }

        /*
         * This method deletes MiddleObjects that are marked unactive
         * */
        private void deleteObjects()
        {
            for (int i = middleObjects.Count() - 1; i > -1; i--)
            {
                if (!middleObjects.ElementAt(i).Active)
                {
                    middleObjects.RemoveAt(i);
                }
            }
        }
    }
}