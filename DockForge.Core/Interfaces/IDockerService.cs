using System;
using System.Collections.Generic;
using System.Text;
using DockForge.Core.Models;

namespace DockForge.Core.Interfaces
{
    public interface IDockerService
    {
        Task<List<ContainerInfo>> GetContainersAsync();

        Task RestartContainerAsync(string containerId);

        Task<string> GetContainerLogsAsync(string containerId, int tail = 200);
    }
}
