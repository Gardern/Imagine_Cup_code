/*
 * Open Source Initiative OSI - The MIT License (MIT):Licensing
 * The MIT License (MIT)
 * Copyright (c) <2012> <A* Games>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
 * associated documentation files (the "Software"), to deal in the Software without restriction, 
 * including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    // Structure that stores the results of the PolygonCollision function
    public struct PolygonCollisionResult
    {
        // True if the polygons will intersect forward in time.
        public bool WillIntersect;

        // True if the polygons currently intersects.
        public bool Intersect;

        // The translation to apply to the polygon to push the polygons apart.
        public Vector2 MinimumTranslationVector;
    }

    public static class CollisionManager
    {
        // Check if polygon A is going to collide with polygon B for the given velocity
        public static PolygonCollisionResult intersects(Polygon polygonA, Polygon polygonB, Vector2 velocity)
        {
            PolygonCollisionResult result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            int edgeCountA = polygonA.getEdges().Count;
            int edgeCountB = polygonB.getEdges().Count;
            float minIntervalDistance = float.PositiveInfinity;
            Vector2 translationAxis = new Vector2();
            Vector2 edge;

            // Loop through all the edges of both polygons
            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {
                if (edgeIndex < edgeCountA)
                {
                    edge = polygonA.getEdges()[edgeIndex];
                }
                else
                {
                    edge = polygonB.getEdges()[edgeIndex - edgeCountA];
                }

                // ===== 1. Find if the polygons are currently intersecting =====

                // Find the axis perpendicular to the current edge
                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0;
                float minB = 0;
                float maxA = 0;
                float maxB = 0;

                projectPolygon(axis, polygonA, ref minA, ref maxA);
                projectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (intervalDistance(minA, maxA, minB, maxB) > 0)
                {
                    result.Intersect = false;
                }

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                // float velocityProjection = axis.Dot(velocity);
                float velocityProjection = Vector2.Dot(axis, velocity);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {
                    minA += velocityProjection;
                }
                else
                {
                    maxA += velocityProjection;
                }

                // Do the same test as above for the new projection
                float iDistance = intervalDistance(minA, maxA, minB, maxB);
                if (iDistance > 0)
                {
                    result.WillIntersect = false;
                }

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect)
                {
                    break;
                }

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                iDistance = Math.Abs(iDistance);
                if (iDistance < minIntervalDistance)
                {
                    minIntervalDistance = iDistance;
                    translationAxis = axis;

                    Vector2 d = polygonA.getCenter() - polygonB.getCenter();
                    if (Vector2.Dot(d, translationAxis) < 0)
                    {
                        translationAxis = -translationAxis;
                    }
                }
            }

            // The minimum translation vector can be used to push the polygons appart.
            // First moves the polygons by their velocity, then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect)
            {
                result.MinimumTranslationVector = translationAxis * minIntervalDistance;
            }

            return result;
        }

        public static bool intersects(Vector2 mousePos, Vector2 objectPos, Vector2 objectOrigin)
        {
            bool collision = false;

            float objectTop = objectPos.Y;
            float objectBottom = objectPos.Y + objectOrigin.Y * 2;
            float objectRight = objectPos.X + objectOrigin.X * 2;
            float objectLeft = objectPos.X;


            if (mousePos.Y >= objectTop && mousePos.Y <= objectBottom && mousePos.X <= objectRight && mousePos.X >= objectLeft)
            {
                collision = true;
            }

            return collision;
        }

        private static float intervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            else
            {
                return minA - maxB;
            }
        }

        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        private static void projectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
        {
            // To project a point on an axis use the dot product
            float d = Vector2.Dot(axis, polygon.getPoints()[0]);
            min = d;
            max = d;

            for (int i = 0; i < polygon.getPoints().Count; i++)
            {
                d = Vector2.Dot(polygon.getPoints()[i], axis);

                if (d < min)
                {
                    min = d;
                }
                else
                {
                    if (d > max)
                    {
                        max = d;
                    }
                }
            }
        }
    }
}