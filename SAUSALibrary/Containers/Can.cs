﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SAUSALibrary.crates
{
    public class Can
    {
        List<String> specifics = new List<String>();

        public Can(List<String> newAttributes)
        {
            specifics = newAttributes;
        }
    }
}
