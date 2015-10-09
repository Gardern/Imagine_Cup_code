using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ImagineCup2012
{
    /*
     * Defines an oriented box that can be used for 2D collision
     * */
    public class OBB
    {
        private List<Vector2> corners;
        private List<Vector2> triangle1;
        private List<Vector2> triangle2;

        private List<Vector2> oldCorners;
        private List<Vector2> oldTriangle1;
        private List<Vector2> oldTriangle2;

        /*
         * Takes in a list of all four corners in a rectangle
         * bottom left, bottom right, top right, top left
         * */
        public OBB(List<Vector2> corners)
        {
            this.corners = new List<Vector2>();
            triangle1 = new List<Vector2>();
            triangle2 = new List<Vector2>();
            oldTriangle1 = new List<Vector2>();
            oldTriangle2 = new List<Vector2>();
            oldCorners = new List<Vector2>();
            this.corners = corners;

            if (this.corners == null)
            {
                Console.WriteLine("CORNERS == NULL");
            }
            createTriangles(this.corners);
        }

        /*
         * Takes in four corners (positions) for a rectangle
         * bottom left, bottom right, top right, top left
         * */
        public OBB(Vector2 corner1, Vector2 corner2, Vector2 corner3, Vector2 corner4)
        {
            corners = new List<Vector2>();
            triangle1 = new List<Vector2>();
            triangle2 = new List<Vector2>();
            oldTriangle1 = new List<Vector2>();
            oldTriangle2 = new List<Vector2>();
            oldCorners = new List<Vector2>();

            corners.Add(corner1);
            corners.Add(corner2);
            corners.Add(corner3);
            corners.Add(corner4);

            createTriangles(corners);
        }

        public List<Vector2> getCorners()
        {
            return corners;
        }

        public List<Vector2> getTriangle1()
        {
            return triangle1;
        }

        public List<Vector2> getTriangle2()
        {
            return triangle2;
        }

        public void setCorners(List<Vector2> corners)
        {
            this.corners = corners;
        }

        public void setTriangle1(List<Vector2> triangle1)
        {
            this.triangle1 = triangle1;
        }

        public void setTriangle2(List<Vector2> triangle2)
        {
            this.triangle2 = triangle2;
        }

        public List<Vector2> getOldCorners()
        {
            return oldCorners;
        }

        public List<Vector2> getOldTriangle1()
        {
            return oldTriangle1;
        }

        public List<Vector2> getOldTriangle2()
        {
            return oldTriangle2;
        }

        public void setOldCorners(List<Vector2> oldCorners)
        {
            this.oldCorners = oldCorners;
        }

        public void setOldTriangle1(List<Vector2> oldTriangle1)
        {
            this.oldTriangle1 = oldTriangle1;
        }

        public void setOldTriangle2(List<Vector2> oldTriangle2)
        {
            this.oldTriangle2 = oldTriangle2;
        }

        /*
         * Creates two triangles for this rectangle
         * */
        private void createTriangles(List<Vector2> corners)
        {
            triangle1.Add(new Vector2(corners.ElementAt(0).X, corners.ElementAt(0).Y));
            triangle1.Add(new Vector2(corners.ElementAt(1).X, corners.ElementAt(1).Y));
            triangle1.Add(new Vector2(corners.ElementAt(3).X, corners.ElementAt(3).Y));
            
            triangle2.Add(new Vector2(corners.ElementAt(1).X, corners.ElementAt(1).Y));
            triangle2.Add(new Vector2(corners.ElementAt(2).X, corners.ElementAt(2).Y));
            triangle2.Add(new Vector2(corners.ElementAt(3).X, corners.ElementAt(3).Y));
        }
    }
}