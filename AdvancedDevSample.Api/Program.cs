using AdvancedDevSample.Api.Middlewares;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Interfaces.Orders;
using AdvancedDevSample.Domain.Interfaces.Products;
using AdvanceDevSample.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();



//
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    foreach (var xmlFile in Directory.GetFiles(basePath, "*.xml"))
    {
        options.IncludeXmlComments(xmlFile);
    }
});

// =============Dependances Application ===================
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

// =============Dependances Infrastructure ===================
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();