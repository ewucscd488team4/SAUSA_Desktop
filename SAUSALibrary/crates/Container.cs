using System;
using System.Collections.Generic;
using System.Text;

namespace SAUSALibrary.crates
{
    public class Container
    {
        List<String> attributes = new List<String>();

        public void setAttributes(List<String> start)
        {
            foreach (String data in start)
            {
                attributes.Add(data);
            }
        }

        public List<string> getAttributes()
        {
            return attributes;
        }
    }
}
