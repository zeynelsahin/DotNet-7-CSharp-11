using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WiredBrainCoffee.MinApi;
using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

namespace WiredBrainCoffee.Tests;

public class OrderTests
{
    private readonly Mock<IOrderService> orderService = new Mock<IOrderService>();

    public OrderTests()
    {
        orderService.Setup(x => x.GetOrders()).Returns(new List<Order>()
        {
            new Order() { Id = 5 }
        });

        orderService.Setup(x => x.GetOrderById(It.IsAny<int>())).Returns(new Order() { Id = 5 });
    }

    [Fact]
    public void GetOrdersReturnsOk()
    {
        var result = OrderEndpoints.GetOrders(orderService.Object);
        Assert.IsType<Ok<List<Order>>>(result);
    }

    [Fact]
    public void GetOrderByIdReturnsOk()
    {
        var result = (Ok<Order>)OrderEndpoints.GetOrderById(orderService.Object, 3);
        Assert.Equal(200,result.StatusCode);
        Assert.IsAssignableFrom<Order>(result.Value);
    }
}