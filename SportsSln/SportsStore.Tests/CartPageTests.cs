using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;

namespace SportsStore.Tests;

public class CartPageTests
{
    [Fact]
    public void Can_Load_Cart()
    {
        // Arrange
        // - create a mock repository
        var p1 = new Product {ProductId = 1, Name = "P1"};
        var p2 = new Product {ProductId = 2, Name = "P2"};
        var mockRepo = new Mock<IStoreRepository>();
        mockRepo.Setup(m => m.Products).Returns(new[]
        {
            p1, p2
        }.AsQueryable<Product>());

        // - create a cart
        var testCart = new Cart();
        testCart.AddItem(p1, 2);
        testCart.AddItem(p2, 1);

        // Action
        CartModel cartModel = new CartModel(mockRepo.Object, testCart);
        cartModel.OnGet("myUrl");

        // Assert
        Assert.Equal(2, cartModel.Cart.Lines.Count());
        Assert.Equal("myUrl", cartModel.ReturnUrl);
    }

    [Fact]
    public void Can_Update_Cart()
    {
        // Arrange
        // - create a mock repository
        Mock<IStoreRepository> mockRepo = new Mock<IStoreRepository>();
        mockRepo.Setup(m => m.Products).Returns((new Product[]
        {
            new Product() {ProductId = 1, Name = "P1"}
        }).AsQueryable<Product>);

        Cart testCart = new Cart();

        // Action
        CartModel cartModel = new CartModel(mockRepo.Object, testCart);
        cartModel.OnPost(1, "myUrl");

        //Assert
        Assert.Single(testCart.Lines);
        Assert.Equal("P1", testCart.Lines.First().Product.Name);
        Assert.Equal(1, testCart.Lines.First().Quantity);
    }
}