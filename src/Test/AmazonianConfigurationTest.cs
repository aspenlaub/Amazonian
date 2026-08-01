using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Seoa.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Entities;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Test;

[TestClass]
public class AmazonianConfigurationTest {
    [TestMethod]
    public async Task CanGetAmazonianConfiguration() {
        IContainer container = new ContainerBuilder().UsePegh(null).Build();
        ISecretRepository secretRepository = container.Resolve<ISecretRepository>();
        var errorsAndInfos = new ErrorsAndInfos();
        AmazonianConfiguration secret = await secretRepository.GetAsync(new AmazonianConfigurationSecret(), errorsAndInfos);
        Assert.That.ThereWereNoErrors(errorsAndInfos);
        Assert.IsFalse(string.IsNullOrEmpty(secret.OrderFolder), "Order folder is empty");
        IFolderResolver resolver = container.Resolve<IFolderResolver>();
        IFolder sourceFolder = await resolver.ResolveAsync(secret.OrderFolder, errorsAndInfos);
        Assert.That.ThereWereNoErrors(errorsAndInfos);
        Assert.IsTrue(sourceFolder.Exists(), $"Order folder \"{secret.OrderFolder}\" does not exist");
    }
}