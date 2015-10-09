using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class InventoryHud
    {
        private List<InventorySlotHud> slots;

        public InventoryHud()
        {
            slots = new List<InventorySlotHud>();
        }

        public List<InventorySlotHud> getItems()
        {
            return slots;
        }

        public void update(GameTime gameTime)
        {
            foreach (InventorySlotHud slotHud in slots)
            {
                slotHud.update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (SceneManager.getInstance.getPlayer1().getOpenInventory())
            {
                foreach (InventorySlotHud slotHud in slots)
                {
                    slotHud.draw(spriteBatch);
                }
            }
        }

        public void onCollision(GameTime gameTime, PolygonCollisionResult collisionResult, Vector2 player1Velocity)
        {
            foreach (InventorySlotHud slotHud in slots)
            {
                slotHud.onCollision(gameTime, collisionResult, player1Velocity);
            }
        }

        //sende inn hudStart(400, 450) og hudEnd = 750;
        /*
         * This function creates the inventory hud
         * mainSlots: The number of main slots in this inventory
         * otherSlots: The number of other slots in this inventory
         * hudStart: Where the hud will start on the screen relative to the upper left corner
         * switchDecider: int indicating how many slots there will be in one row
         * returns true if it sucessfully created the inventory hud
         * */
        public bool createInventoryHud(int mainSlots, int otherSlots, Vector2 hudStart, int switchDecider)
        {
            Texture2D inventoryHudMainText = null;
            Texture2D inventoryHudOtherText = null;
            int switchCounter = 0;

            //Loops through and finds the ItemHudMain texture
            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "InventoryHudMain")
                {
                    inventoryHudMainText = texture.Value;
                    break;
                }
            }
            //Loops through and finds the ItemHudOther texture
            foreach (KeyValuePair<string, Texture2D> texture in ResourceManager.getInstance.getRuntimeTextures())
            {
                if (texture.Key == "InventoryHudOther")
                {
                    inventoryHudOtherText = texture.Value;
                    break;
                }
            }

            if (inventoryHudMainText != null && inventoryHudOtherText != null)
            {
                Vector2 topLeftCorner = SceneManager.getInstance.getCamera().getPosition();
                topLeftCorner.X *= -1;
                topLeftCorner.Y *= -1;

                //hudEnd += topLeftCorner.X;

                Console.WriteLine(topLeftCorner);

                Vector2 itemHudPos = new Vector2(topLeftCorner.X + hudStart.X, topLeftCorner.Y + hudStart.Y);

                Console.WriteLine(itemHudPos);

                //Creates the main slots
                for (int i = 0; i < mainSlots; i++)
                {
                    slots.Add(new InventorySlotHud(ESlotType.MAIN, inventoryHudMainText, itemHudPos, 0.0f,
                        new Vector2(inventoryHudMainText.Width / 2, inventoryHudMainText.Height / 2), new Vector2(1, 1)));

                    itemHudPos.X += inventoryHudMainText.Width;

                    switchCounter++;

                    if (switchCounter == switchDecider)
                    {
                        itemHudPos.X = topLeftCorner.X + hudStart.X;
                        itemHudPos.Y += inventoryHudMainText.Height;
                        switchCounter = 0;
                    }
                    Console.WriteLine("Switchdecider: "  + switchDecider);
                    /*
                    //If the X position is higher than the hudEnd, we need to reset X and set Y down
                    if (itemHudPos.X >= hudEnd)
                    {
                        itemHudPos.X = topLeftCorner.X + hudStart.X;
                        itemHudPos.Y += itemHudMainTex.Height;
                    }
                     * */
                }

                //Creates the other slots
                for (int i = 0; i < otherSlots; i++)
                {
                    slots.Add(new InventorySlotHud(ESlotType.OTHER, inventoryHudOtherText, itemHudPos, 0.0f,
                        new Vector2(inventoryHudOtherText.Width / 2, inventoryHudOtherText.Height / 2), new Vector2(1, 1)));

                    itemHudPos.X += inventoryHudOtherText.Width;

                    switchCounter++;

                    if (switchCounter == switchDecider)
                    {
                        itemHudPos.X = topLeftCorner.X + hudStart.X;
                        itemHudPos.Y += inventoryHudOtherText.Height;
                        switchCounter = 0;
                    }
                    Console.WriteLine("Switchdecider: " + switchDecider);
                    /*
                    //If the X position is higher than the hudEnd, we need to reset X and set Y down
                    if (itemHudPos.X >= hudEnd)
                    {
                        itemHudPos.X = topLeftCorner.X + hudStart.X;
                        itemHudPos.Y += itemHudOtherTex.Height;
                    }
                     * */
                }
            }

            //The inventory hud was sucessfully created
            if (slots.Count == mainSlots + otherSlots)
            {
                return true;
            }
            return false;
        }

        /*
         * item:            The item to add if its not already in the list. If it is in the list we only increase the number of this item
         * numberOfItems:   How many of this item we add. Invalid to send in a value that is less than 1
         * slotType:        The slot which we want to insert this item into
         * returns true if it was successfully inserted
         * */
        public bool insertItem(ItemHud item, int numberOfItems, ESlotType slotType)
        {
            if (numberOfItems > 0)
            {
                foreach (InventorySlotHud slotHud in slots)
                {
                    if (slotHud.getSlotType() == slotType)
                    {
                        int id = -1;

                        if (!slotHud.getContainsItem())
                        {
                            //Console.WriteLine("Insert");
                            Vector2 position = slotHud.Properties.getProperty<Vector2>("Position");
                            Texture2D texture = item.Properties.getProperty<Texture2D>("Texture2D");
                            float rotation = item.Properties.getProperty<float>("Rotation");
                            Vector2 origin = item.Properties.getProperty<Vector2>("Origin");
                            Vector2 scale = item.Properties.getProperty<Vector2>("Scale");

                            ItemHud newItem = new ItemHud(item.getItemType(), texture, position, rotation, origin, scale);

                            slotHud.setItemHud(newItem);
                            slotHud.setContainsItem(true);

                            if (slotType == ESlotType.OTHER)
                            {
                                slotHud.setNumberOfItems(slotHud.getNumberOfItems() + numberOfItems);

                                //Checks what type of item we are adding to the inventory
                                if (newItem.getItemType() == EItemType.SCREW)
                                {
                                    id = 0;
                                }
                                else if (newItem.getItemType() == EItemType.OILCAN)
                                {
                                    id = 1;
                                }
                                else if (newItem.getItemType() == EItemType.CEMENT)
                                {
                                    id = 2;
                                }
                                else if (newItem.getItemType() == EItemType.APPLE)
                                {
                                    id = 3;
                                }
                                else if (newItem.getItemType() == EItemType.CORN)
                                {
                                    id = 4;
                                }
                                else if (newItem.getItemType() == EItemType.TREEGUN)
                                {
                                    id = 5;
                                }
                                else if (newItem.getItemType() == EItemType.SHOVEL)
                                {
                                    id = 6;
                                }
                                else if (newItem.getItemType() == EItemType.PICKAXE)
                                {
                                    id = 7;
                                }

                                //Adds the font associated with this item
                                SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, id, "FontInventory", ResourceManager.getInstance.Font2, slotHud.getNumberOfItems() + "",
                                    new Vector2(position.X + 4.0f, position.Y - 35.0f), Color.Red, -1.0f));
                            }
                            
                            return true;
                        }
                        else if (slotHud.getContainsItem() && slotHud.getItemHud().Equals(item))
                        {
                            //Console.WriteLine("Count up");

                            if (slotType == ESlotType.OTHER)
                            {
                                slotHud.setNumberOfItems(slotHud.getNumberOfItems() + numberOfItems);

                                //Checks what type of item we are adding to the inventory
                                if (item.getItemType() == EItemType.SCREW)
                                {
                                    id = 0;
                                }
                                else if (item.getItemType() == EItemType.OILCAN)
                                {
                                    id = 1;
                                }
                                else if (item.getItemType() == EItemType.CEMENT)
                                {
                                    id = 2;
                                }
                                else if (item.getItemType() == EItemType.APPLE)
                                {
                                    id = 3;
                                }
                                else if (item.getItemType() == EItemType.CORN)
                                {
                                    id = 4;
                                }
                                else if (item.getItemType() == EItemType.TREEGUN)
                                {
                                    id = 5;
                                }
                                else if (item.getItemType() == EItemType.SHOVEL)
                                {
                                    id = 6;
                                }
                                else if (item.getItemType() == EItemType.PICKAXE)
                                {
                                    id = 7;
                                }

                                //Adds the font associated with this item
                                SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, id, "FontInventory", ResourceManager.getInstance.Font2, slotHud.getNumberOfItems() + "",
                                    new Vector2(slotHud.Properties.getProperty<Vector2>("Position").X + 4.0f,
                                        slotHud.Properties.getProperty<Vector2>("Position").Y - 35.0f), Color.Red, -1.0f));

                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /*
         * item: The item to remove
         * numberOfItems: The number of this item to remove
         * returns true if it successfully removed a item
         * */
        public bool removeItem(ItemHud item, int numberOfItems)
        {
            foreach (InventorySlotHud slotHud in slots)
            {
                if (slotHud.getContainsItem() && slotHud.getItemHud().Equals(item))
                {
                    int id = -1;

                    if (slotHud.getNumberOfItems() == 1)
                    {
                        slotHud.setItemHud(null);
                        slotHud.setContainsItem(false);
                    }
                    slotHud.setNumberOfItems(slotHud.getNumberOfItems() - numberOfItems);

                    //Checks what type of item we are adding to the inventory
                    if (item.getItemType() == EItemType.SCREW)
                    {
                        id = 0;
                    }
                    else if (item.getItemType() == EItemType.OILCAN)
                    {
                        id = 1;
                    }
                    else if (item.getItemType() == EItemType.CEMENT)
                    {
                        id = 2;
                    }
                    else if (item.getItemType() == EItemType.APPLE)
                    {
                        id = 3;
                    }
                    else if (item.getItemType() == EItemType.CORN)
                    {
                        id = 4;
                    }
                    else if (item.getItemType() == EItemType.TREEGUN)
                    {
                        id = 5;
                    }
                    else if (item.getItemType() == EItemType.SHOVEL)
                    {
                        id = 6;
                    }
                    else if (item.getItemType() == EItemType.PICKAXE)
                    {
                        id = 7;
                    }

                    //Adds the font associated with this item
                    SceneManager.getInstance.getFontManager().addFont(new Font(EFontType.STATIC, id, "FontInventory", ResourceManager.getInstance.Font2, slotHud.getNumberOfItems() + "",
                        new Vector2(slotHud.Properties.getProperty<Vector2>("Position").X + 4.0f,
                            slotHud.Properties.getProperty<Vector2>("Position").Y - 35.0f), Color.Red, -1.0f));

                    return true;
                }
            }
            return false;
        }
    }
}