using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAUSALibrary.crates
{
    public class CrateWithFeet : Container
    {
        List<String> specifics = new List<String>();

        public CrateWithFeet(List<string> newAttributes)
        {
            specifics = newAttributes;
        }
    }
}
