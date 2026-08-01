using System.Threading.Tasks;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian;

public static class AmazonianContainerBuilder {
    public static async Task<ContainerBuilder> UseAmazonianAsync(this ContainerBuilder builder) {
        return await Task.FromResult(builder);
    }
}