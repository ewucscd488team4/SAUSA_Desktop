using SAUSALibrary.crates;
using System;
using System.Collections.Generic;

namespace CommandLineTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> baseAtt = new List<string>();
            List<string> extended = new List<string>();
            extended.Add("150");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("3");
            baseAtt.Add("test3");
            Box test = new Box(baseAtt, extended);
            
        }
    }
}
