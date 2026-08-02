using System.Threading.Tasks;
using Autofac;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian;

public static class AmazonianContainerBuilder {
    public static async Task<ContainerBuilder> UseAmazonianAsync(this ContainerBuilder builder) {
        return await Task.FromResult(builder);
    }
}