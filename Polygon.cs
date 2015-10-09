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
using System.Text;
using Microsoft.Xna.Framework;

namespace ImagineCup2012
{
    public class Polygon
    {
        private List<Vector2> points = new List<Vector2>();
        private List<Vector2> edges = new List<Vector2>();

        public void buildEdges()
        {
            Vector2 p1;
            Vector2 p2;

            edges.Clear();

            for (int i = 0; i < points.Count; i++)
            {
                p1 = points[i];

                if (i + 1 >= points.Count)
                {
                    p2 = points[0];
                }
                else
                {
                    p2 = points[i + 1];
                }

                edges.Add(p2 - p1);
            }
        }

        public void offset(Vector2 vector)
        {
            offset(vector.X, vector.Y);
        }

        public void offset(float x, float y)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 p = points[i];
                points[i] = new Vector2(p.X + x, p.Y + y);
            }
        }

        public List<Vector2> getEdges()
        {
            return edges;
        }

        public List<Vector2> getPoints()
        {
            return points;
        }

        public Vector2 getCenter()
        {
            float totalX = 0;
            float totalY = 0;

            for (int i = 0; i < points.Count; i++)
            {
                totalX += points[i].X;
                totalY += points[i].Y;
            }

            return new Vector2(totalX / (float)points.Count, totalY / (float)points.Count);
        }

        /*
        public void rotate(float angle)
        {
            float radAngle = angle * MathHelper.TO_RADIANS;

            Vector2 c = Center;

            // Translate to center, rotate and translate back
            for (int i = 0; i < Points.Count; i++)
            {
                points[i] += -c;

                float x = (float)Math.Cos(radAngle) * Points[i].X + (float)-Math.Sin(radAngle) * Points[i].Y;
                float y = (float)Math.Sin(radAngle) * Points[i].X + (float)Math.Cos(radAngle) * Points[i].Y;
                points[i] = new Vector2(x, y);

                points[i] += c;
            }

            BuildEdges();
        }
         * */

        public override string ToString()
        {
            string text = "";

            foreach (Vector2 point in points)
            {
                text += "X: " + point.X + ", Y: " + point.Y + "- ";
            }

            return text;
        }
    }
}