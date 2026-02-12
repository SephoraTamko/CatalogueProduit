using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using AdvancedDevSample.Test.API.Integration;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using AdvancedDevSample.Domain.Interfaces.Products;

namespace AdvancedDevSample.Test.API.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //Supprimer le vrai repository si necessaire
                services.RemoveAll(typeof(IProductRepository));
                //Ajouter un repository in memeory
                object value = services.AddSingleton<IProductRepository, InMemoryProductRepositoryAsync>();
            });
           
        }
    }
}
