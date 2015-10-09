using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagineCup2012
{
    public class BuildBackground
    {
        private bool active;
        private int index;
        private BackgroundObject backObject;

        public BuildBackground(bool active, int index, BackgroundObject backObject)
        {
            this.active = active;
            this.index = index;
            this.backObject = backObject;
        }

        public bool isActive()
        {
            return active;
        }

        public int getIndex()
        {
            return index;
        }

        public BackgroundObject getBackObject()
        {
            return backObject;
        }

        public void setActive(bool active)
        {
            this.active = active;
        }
    }
}