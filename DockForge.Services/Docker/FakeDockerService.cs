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
                    Id = "fake-satisfactory-server",
                    Name = "satisfactory-server",
                    Image = "wolveix/satisfactory-server",
                    Status = "Running",
                    StartedAt = DateTime.UtcNow.AddHours(-5).AddMinutes(-23)
                },
                new()
                {
                    Id = "fake-portainer",
                    Name = "portainer",
                    Image = "portainer/portainer-ce",
                    Status = "Running",
                    StartedAt = DateTime.UtcNow.AddDays(-2).AddHours(-4)
                }
            };

            return Task.FromResult(containers);
        }

        public Task<string> GetContainerLogsAsync(string containerId, int tail = 200)
        {
            var logs = """
        [INFO] Starting fake container...
        [INFO] Loading configuration...
        [INFO] Container is running.
        [INFO] This is fake log data for local development.
        """;

            return Task.FromResult(logs);
        }

        public Task RestartContainerAsync(string containerId)
        {
            return Task.CompletedTask;
        }
}
}
