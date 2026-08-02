using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Interfaces;

public interface IAmazonianRepository {
    Task SaveOrdersAsync(IList<AmazonianOrder> orders);
    Task RemoveOrdersAsync(List<string> orderNumbers);
    Task<AmazonianOrder> FindOrderForPostingAsync(string postingRemark);
}