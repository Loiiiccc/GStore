using System.Net;
using System.Net.Http.Json;
using GStore.Models;
using GStore.Services;
using Microsoft.AspNetCore.Mvc;
using TUnit;
using TUnit.Assertions.Extensions;

namespace GStore.Tests
{
    public class UnitTest1
    {


        [Test]
        public async Task AddProductToCart_ShouldReturnOk_WhenSuccessfulAsync()
        {
            var factory =new ApiFactory();
            var httpClient = factory.CreateClient();

            var response = await httpClient.PostAsJsonAsync("/api/addToCart", new { codeClient = 1 });

            await TUnit.Assertions.Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);

            //var userId = 1;
            //var productId = 2;
            //var quantity = 3;
            //var expectedCartItem = new CartItem { Id = 1 };

            //var result = await AddProductToCartAsync(userId, productId, quantity);

            //await TUnit.Assertions.Assert.That(result).IsEqualTo(expectedCartItem);



        }

    }
}
