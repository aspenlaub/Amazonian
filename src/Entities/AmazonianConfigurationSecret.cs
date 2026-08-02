using System.IO;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;

public class AmazonianConfigurationSecret : ISecret<AmazonianConfiguration> {
    public AmazonianConfiguration DefaultValue => field ??= new AmazonianConfiguration { OrderFolder = Path.GetTempPath() };

    public string Guid => "D0A8A344-2475-427A-9CF3-91B70EEACEBA";
}