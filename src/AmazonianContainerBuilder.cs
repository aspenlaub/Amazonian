using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Components;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Interfaces;
using Autofac;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian;

public static class AmazonianContainerBuilder {
    public static async Task<ContainerBuilder> UseAmazonianAsync(this ContainerBuilder builder) {
        builder.RegisterType<AmazonianRepository>().As<IAmazonianRepository>();
        return await Task.FromResult(builder);
    }
}