using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImagineCup2012
{
    public class PropertyContainer
    {
        protected Dictionary<String, object> properties;

        public PropertyContainer()
        {
            properties = new Dictionary<String, object>();
        }

        public int getSize()
        {
            return properties.Count();
        }

        public void updateProperty<T>(String name, T obj)
        {
            if (properties.ContainsKey(name))
            {
                properties[name] = obj;
            }
            else
            {
                properties.Add(name, obj);
            }
        }

        public T getProperty<T>(String name)
        {
            if (properties.ContainsKey(name))
            {
                return (T)properties[name];
            }
            return default(T);
        }
    }
}