using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    class Part 
    {

        public Part(string partName, string partType)
        {
            PartName = partName;
            PartType = partType;
            Console.WriteLine("Model Created - Part Name - " + PartName + " Part Type " + PartType);
        }

        public String PartName { get; set; }

        public String PartType { get; set; }


    }



}
