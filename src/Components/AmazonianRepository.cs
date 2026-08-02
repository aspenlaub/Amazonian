using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Entities;
using Aspenlaub.Net.GitHub.CSharp.Skladasu.Extensions;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Components;

public class AmazonianRepository(ISecretRepository secretRepository) : IAmazonianRepository {
    private IDictionary<string, AmazonianOrder> _OrderCache = new Dictionary<string, AmazonianOrder>();

    public async Task SaveOrdersAsync(IList<AmazonianOrder> orders) {
        IFolder orderFolder = await GetOrderFolderAsync();

        foreach (AmazonianOrder order in orders) {
            SaveOrder(orderFolder, order);
        }

        ResetOrderCache();
    }

    private static void SaveOrder(IFolder orderFolder, AmazonianOrder order) {
        string orderFileName = OrderFileName(orderFolder, order.OrderNumber);
        File.WriteAllText(orderFileName, JsonSerializer.Serialize(order));
    }

    public async Task RemoveOrdersAsync(List<string> orderNumbers) {
        IFolder orderFolder = await GetOrderFolderAsync();
        foreach (string orderFileName in orderNumbers
                .Select(orderNumber => OrderFileName(orderFolder, orderNumber))
                .Where(File.Exists)) {
            File.Delete(orderFileName);
        }

        ResetOrderCache();
    }

    public async Task<AmazonianOrder> FindOrderForPostingAsync(string postingRemark) {
        await FillOrderCacheAsync();
        string[] parts = postingRemark.Split(' ');
        string part = parts.FirstOrDefault(p => _OrderCache.ContainsKey(p));
        return (part == null) ? null : _OrderCache[part];
    }

    private async Task<IFolder> GetOrderFolderAsync() {
        var errorsAndInfos = new ErrorsAndInfos();
        AmazonianConfiguration secret = await secretRepository.GetAsync(new AmazonianConfigurationSecret(), errorsAndInfos);
        return errorsAndInfos.AnyErrors()
            ? throw new Exception(errorsAndInfos.ErrorsToString())
            : new Folder(secret.OrderFolder);
    }

    private static string OrderFileName(IFolder orderFolder, string orderNumber) {
        return orderFolder.FullName + "\\" + orderNumber + ".json";
    }

    private void ResetOrderCache() {
        _OrderCache = new Dictionary<string, AmazonianOrder>();
    }

    private async Task FillOrderCacheAsync() {
        if (_OrderCache.Count != 0) {
            return;
        }

        IFolder orderFolder = await GetOrderFolderAsync();
        foreach (string orderFile in Directory.GetFiles (orderFolder.FullName, "*.json", SearchOption.TopDirectoryOnly)) {
            try {
                AmazonianOrder order = JsonSerializer.Deserialize<AmazonianOrder>(await File.ReadAllTextAsync(orderFile));
                _OrderCache[order.OrderNumber] = order;
            } catch {
                // ignored
            }
        }
    }
}