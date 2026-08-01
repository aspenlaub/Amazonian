using System.Xml.Serialization;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;

[XmlRoot("AmazonianConfiguration")]
public class AmazonianConfiguration : IGuid, ISecretResult<AmazonianConfiguration> {
    [XmlAttribute("guid")]
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();

    [XmlElement("orderfolder")]
    public string OrderFolder { get; set; }

    public AmazonianConfiguration Clone() {
        var clone = (AmazonianConfiguration)MemberwiseClone();
        clone.Guid = System.Guid.NewGuid().ToString();
        return clone;
    }
}