using System;
using System.Collections.Generic;
using System.Text;

namespace SAUSALibrary.crates
{
    public class Container_Factory
    {
        /// <summary>
        /// Class is set up to be singleton, use getInstance to instantiate
        /// </summary>
        private static Container_Factory uniqueInstance;

        private Container_Factory()
        {

        }

        /// <summary>
        /// Getter than ensures only a single instance of Container_Factory exists at a time.
        /// </summary>
        /// <returns>Unique Container_Factory</returns>
        public static Container_Factory getInstance()
        {
            if (uniqueInstance is null)
                uniqueInstance = new Container_Factory();
            return uniqueInstance;
        }

        public Crate Get_A_NewCrate(List<string> crateFields)
        {
            return new Crate();
        }

        public Box get_A_Box(List<string> baseFields,List<string> boxFields)
        {
            return new Box(baseFields, boxFields);
        }

        public Can get_A_Can(List<string> canFields)
        {
            return new Can(canFields);
        }
                
    }
}
