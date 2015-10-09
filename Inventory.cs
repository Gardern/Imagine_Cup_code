using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagineCup2012
{
    public class Inventory
    {
        private ResourceItem resources;

        /*
         * Creates a new inventory with the specified things in the inventory
         * */
        public Inventory(ResourceItem resources)
        {
            this.resources = resources;
        }

        public ResourceItem getResources()
        {
            return resources;
        }
    }
}