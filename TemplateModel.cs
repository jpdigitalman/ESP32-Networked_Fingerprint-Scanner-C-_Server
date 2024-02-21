using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_RayFingerNetwork
{
    public class TemplateModel
    {
        public int Id { get; set; } // Assuming you want an auto-incremented Id as the primary key
        public string Name { get; set; }
        public byte[] Template { get; set; }
    }
}
