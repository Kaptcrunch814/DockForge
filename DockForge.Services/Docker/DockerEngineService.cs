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
        var dockerContainers = await _client.Containers.ListContainersAsync(
            new ContainersListParameters
            {
                All = true
            });

        var results = new List<ContainerInfo>();

        foreach (var dockerContainer in dockerContainers)
        {
            DateTime? startedAt = null;

            try
            {
                var inspect = await _client.Containers.InspectContainerAsync(dockerContainer.ID);

                if (inspect.State is not null &&
                    inspect.State.Running &&
                    DateTime.TryParse(inspect.State.StartedAt.ToString(), out var parsedStartedAt))
                {
                    startedAt = parsedStartedAt;
                }
            }
            catch
            {
                // If inspect fails, we still want to show the container.
                startedAt = null;
            }

            results.Add(new ContainerInfo
            {
                Id = dockerContainer.ID,
                Name = dockerContainer.Names.FirstOrDefault()?.TrimStart('/') ?? dockerContainer.ID,
                Image = dockerContainer.Image,
                Status = dockerContainer.State,
                Created = dockerContainer.Created,
                StartedAt = startedAt,
                Ports = dockerContainer.Ports
                    .GroupBy(port => new
                    {
                        port.PrivatePort,
                        port.PublicPort,
                        Type = port.Type ?? string.Empty
                    })
                    .Select(group => group.First())
                    .Select(port => new ContainerPort
                    {
                        PrivatePort = port.PrivatePort,
                        PublicPort = port.PublicPort,
                        Type = port.Type ?? string.Empty
                    })
                    .ToList()
            });
        }

        return results;
    }

    public async Task<string> GetContainerLogsAsync(string containerId, int tail = 200)
    {
        using var logs = await _client.Containers.GetContainerLogsAsync(
            containerId,
            tty: false,
            new ContainerLogsParameters
            {
                ShowStdout = true,
                ShowStderr = true,
                Timestamps = true,
                Tail = tail.ToString()
            });

        var (stdout, stderr) = await logs.ReadOutputToEndAsync(CancellationToken.None);

        return string.Concat(stdout, stderr);
    }

    public async Task RestartContainerAsync(string containerId)
    {
        await _client.Containers.RestartContainerAsync(containerId, new ContainerRestartParameters());
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}