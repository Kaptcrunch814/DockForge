using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;
using DockForge.Core.Interfaces;
using DockForge.Core.Models;

namespace DockForge.Services.Docker;

public sealed class DockerEngineService : IDockerService, IDisposable
{
    private readonly DockerClient _client;

    public DockerEngineService()
    {
        var dockerUri = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "npipe://./pipe/docker_engine"
            : "unix:///var/run/docker.sock";

        _client = new DockerClientConfiguration(new Uri(dockerUri))
            .CreateClient();
    }

    public async Task<List<ContainerInfo>> GetContainersAsync()
    {
        var containers = await _client.Containers.ListContainersAsync(
            new ContainersListParameters
            {
                All = true
            });

        return containers.Select(container => new ContainerInfo
        {
            Id = container.ID,
            Name = container.Names.FirstOrDefault()?.TrimStart('/') ?? container.ID,
            Image = container.Image,
            Status = container.State,
            Created = container.Created,
            Ports = container.Ports.Select(port => new ContainerPort
            {
                PrivatePort = port.PrivatePort,
                PublicPort = port.PublicPort,
                Type = port.Type ?? string.Empty
            }).ToList()
        }).ToList();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}