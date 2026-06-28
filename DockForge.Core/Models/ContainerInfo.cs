using System;
using System.Collections.Generic;
using System.Text;

namespace DockForge.Core.Models
{
    public class ContainerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Created { get; set; } = string.Empty;
        public List<ContainerPort> Ports { get; set; } = [];

    }
}
