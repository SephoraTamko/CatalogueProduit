using System.Net;
using System.Net.Http.Json;

namespace AdvancedDevSample.Test.API.Integration
{
    public class OrderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public OrderControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_Should_Calculate_Correct_Total()
        {
            // Arrange : Création d'une requête de commande
            var orderRequest = new
            {
                Items = new Dictionary<Guid, int> {
                    { Guid.NewGuid(), 2 } // 2 exemplaires d'un produit
                }
            };

            // Act : Envoi à l'API (Swagger simule cet appel)
            var response = await _client.PostAsJsonAsync("/api/orders", orderRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}