using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    enum CollisionType { NO_TYPE, POLYGON, AABB, POLYGON_AND_AABB, OBB, OBB_AND_AABB }; //OBB

    public class CollisionComponent : GameObjectComponent
    {
        private CollisionType collisionType;
        private List<AABB> axisAlignedBoxes;
        private List<Polygon> polygons;
        private bool hasCollided;

        public CollisionComponent(GameObject gameObject, List<Polygon> polygons, List<AABB> axisAlignedBoxes) //List<Polygon> polygons
            : base(gameObject)
        {
            Name = "CollisionComponent";

            this.axisAlignedBoxes = axisAlignedBoxes;
            this.polygons = polygons;
            hasCollided = false;

            if (polygons != null && axisAlignedBoxes != null)
            {
                collisionType = CollisionType.POLYGON_AND_AABB;
                ParentObject.Properties.updateProperty<List<AABB>>("AABB", axisAlignedBoxes);
                ParentObject.Properties.updateProperty<List<Polygon>>("Polygon", polygons);
            }
            else if (polygons != null)
            {
                collisionType = CollisionType.POLYGON;
                ParentObject.Properties.updateProperty<List<Polygon>>("Polygon", polygons);
            }
            else if (axisAlignedBoxes != null)
            {
                collisionType = CollisionType.AABB;
                ParentObject.Properties.updateProperty<List<AABB>>("AABB", axisAlignedBoxes);
            }
            else
            {
                collisionType = CollisionType.NO_TYPE;
            }

            ParentObject.Properties.updateProperty<CollisionType>("CollisionType", collisionType);
            ParentObject.Properties.updateProperty<bool>("HasCollided", hasCollided);
            ParentObject.Properties.updateProperty<bool>("IsCollisionActive", true);
        }

        public override void update(GameTime gameTime)
        {
            Active = ParentObject.Properties.getProperty<bool>("IsCollisionActive");

            if (Active)
            {
                if (collisionType == CollisionType.POLYGON_AND_AABB)
                {
                    updatePolygon(gameTime);
                    updateAABB(gameTime);
                }
                else if (collisionType == CollisionType.POLYGON)
                {
                    updatePolygon(gameTime);
                }

                else if (collisionType == CollisionType.AABB)
                {
                    updateAABB(gameTime);
                }
            }
        }

        private void updateAABB(GameTime gameTime)
        {
            axisAlignedBoxes = ParentObject.Properties.getProperty<List<AABB>>("AABB");
            Vector2 velocity = ParentObject.Properties.getProperty<Vector2>("Velocity");

            foreach (AABB axisAlignedBox in axisAlignedBoxes)
            {
                axisAlignedBox.setOldPosition(axisAlignedBox.getPosition());
                axisAlignedBox.setPosition(axisAlignedBox.getPosition() + velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            ParentObject.Properties.updateProperty<List<AABB>>("AABB", axisAlignedBoxes);
        }

        private void updatePolygon(GameTime gameTime)
        {
            polygons = ParentObject.Properties.getProperty<List<Polygon>>("Polygon");
            Vector2 velocity = ParentObject.Properties.getProperty<Vector2>("Velocity");
            bool hasCollided = ParentObject.Properties.getProperty<bool>("HasCollided");

            if (!hasCollided)
            {
                foreach (Polygon polygon in polygons)
                {
                    polygon.offset(velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    polygon.buildEdges();
                }
            }

            ParentObject.Properties.updateProperty<List<Polygon>>("Polygon", polygons);
        }
    }
}