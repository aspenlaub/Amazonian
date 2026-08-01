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
    public async Task SaveOrdersAsync(IList<AmazonianOrder> orders) {
        IFolder orderFolder = await GetOrderFolderAsync();

        foreach (AmazonianOrder order in orders) {
            SaveOrder(orderFolder, order);
        }
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
}