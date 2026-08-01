using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;

public class AmazonianOrder {
    public string OrderNumber { get; set; }
    public int AmountInEuroRoundedDown { get; set; }
    public List<string> Products { get; set; }
}
