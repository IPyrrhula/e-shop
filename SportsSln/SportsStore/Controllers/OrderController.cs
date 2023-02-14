using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers;

public class OrderController : Controller
{
    private IOrderRepository _repository;
    private Cart _cart;

    public OrderController(IOrderRepository repoService, Cart cartService)
    {
        _repository = repoService;
        _cart = cartService;
    }
    
    public ViewResult Checkout() => View(new Order());

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (_cart.Lines.Count() == 0)
        {
            ModelState.AddModelError("", "Sory, your cart is empty!");
        }

        if (ModelState.IsValid)
        {
            order.Lines = _cart.Lines.ToArray();
            _repository.SaveOrder(order);
            _cart.Clear();
            return RedirectToPage("/Completed", new {orderId = order.OrderId});
        }
        else
        {
            return View();
        }
    }
}