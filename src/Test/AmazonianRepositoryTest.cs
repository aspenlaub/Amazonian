using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Components;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Entities;
using Aspenlaub.Net.GitHub.CSharp.Amazonian.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.Amazonian.Test;

[TestClass]
public class AmazonianRepositoryTest {
    private const string _orderNumber1 = "47-11", _orderNumber2 = "47-12";
    private const int _amount1 = 4711, _amount2 = 4712;

    private readonly IAmazonianRepository _Sut;

    public AmazonianRepositoryTest() {
        IContainer container = new ContainerBuilder().UsePegh(null).Build();
        _Sut = new AmazonianRepository(container.Resolve<ISecretRepository>());
    }

    [TestCleanup]
    public async Task Cleanup() {
        await _Sut.RemoveOrdersAsync([_orderNumber1, _orderNumber2]);
    }

    [TestMethod]
    public async Task CanSaveOrders() {
        AmazonianOrder order1 = CreateOrder(_orderNumber1, _amount1);
        AmazonianOrder order2 = CreateOrder(_orderNumber2, _amount2);
        await _Sut.SaveOrdersAsync([ order1, order2 ]);
    }

    private AmazonianOrder CreateOrder(string orderNumber, int amount) {
        return new AmazonianOrder {
            OrderNumber = orderNumber,
            AmountInEuroRoundedDown = amount,
            Products = [
                $"Product 1 of order {orderNumber}",
                $"Product 2 of order {orderNumber}",
            ]
        };

    }
}
