using System;
using System.Collections.Generic;
using System.Text;

namespace SAUSALibrary.crates
{
    public abstract class Container
    {
        private List<String> baseAttributres;

        public Container(List<string> newBaseAttributes)
        {
            baseAttributres = newBaseAttributes;
        }
    }
}
