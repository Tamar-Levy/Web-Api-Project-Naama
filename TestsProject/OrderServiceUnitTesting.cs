﻿using Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsProject
{
    public class OrderServiceUnitTesting
    {

        [Fact]
        public async void CheckOrderSum_ValidCredentialsReturnOrder()
        {
            var products = new List<Product>
        {
            new Product { ProductId = 1, Price = 40 },
            new Product { ProductId = 2, Price = 20 }
        };

            var orders = new List<Order>
        {
            new Order
            {
                UserId = 1,
                OrderSum = 80,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 },
                    new OrderItem { ProductId = 2, Quantity = 1 }
                }
            }
        };

            var mockContext = new Mock<MyShop215736745Context>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders);
            mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            var productRepository = new ProductsRepository(mockContext.Object);
            var orderRepository = new OrdersRepository(mockContext.Object);
            var mockLogger = new Mock<ILogger<OrdersService>>();
            var orderService = new OrdersService(orderRepository, mockLogger.Object, productRepository);

            var result = await orderService.AddOrder(orders[0]);
            Assert.Equal(result, orders[0]);
        }

        [Fact]
        public async void CheckOrderSum_UnValidCredentialsReturnExeption()
        {
            var products = new List<Product>
        {
            new Product { ProductId = 1, Price = 40 },
            new Product { ProductId = 2, Price = 20 }
        };

            var invalidOrder = new Order
            {
                UserId = 1,
                OrderSum = 10,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2 },
                    new OrderItem { ProductId = 2, Quantity = 1 }
                }
            };

            var mockContext = new Mock<MyShop215736745Context>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);
            mockContext.Setup(x => x.Orders).ReturnsDbSet(new List<Order>());
            mockContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
            var productRepository = new ProductsRepository(mockContext.Object);
            var orderRepository = new OrdersRepository(mockContext.Object);
            var mockLogger = new Mock<ILogger<OrdersService>>();
            var orderService = new OrdersService(orderRepository, mockLogger.Object, productRepository);

            var result = await orderService.AddOrder(invalidOrder);
            Assert.Equal(result, invalidOrder);
        }
    }
}

