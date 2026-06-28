using System;
using System.Collections.Generic;
using System.Text;

namespace DockForge.Core.Models
{
    public class ContainerPort
    {
        public int PrivatePort { get; set; }
        public int PublicPort { get; set; }
        public string Type { get; set; } = string.Empty;
    }
    
}
