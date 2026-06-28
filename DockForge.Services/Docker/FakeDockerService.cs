using System;
using System.Collections.Generic;
using System.Text;
using DockForge.Core.Models;
using DockForge.Core.Interfaces;

namespace DockForge.Services.Docker
{
    public class FakeDockerService : IDockerService
    {
        public Task<List<ContainerInfo>> GetContainersAsync()
        {
            var containers = new List<ContainerInfo>
            {
                new()
                {
                    Name = "satisfactory-server",
                    Image = "wolveix/satisfactory-server",
                    Status = "Running"
                },
                new()
                {
                    Name = "portainer",
                    Image = "portainer/portainer-ce",
                    Status = "Running"
                }
            };

            return Task.FromResult(containers);
        }
    }
}
