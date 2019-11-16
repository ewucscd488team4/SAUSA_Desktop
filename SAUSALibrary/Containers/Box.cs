using System;
using System.Collections.Generic;
using System.Text;

namespace SAUSALibrary.crates
{
    public class Box : Container
    {
        private List<string> extendedAttricutes;

        public Box(List<string> b, List<string> a)
            : base(b)
        {
            extendedAttricutes = a;
        }
               

    }
}
