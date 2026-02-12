using System.Text.Json;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces.Products;
using AdvanceDevSample.Infrastructure.Entities;
using AdvanceDevSample.Infrastructure.Exceptions;

namespace AdvanceDevSample.Infrastructure.Repositories
{
    public class EfProductRepository : IProductRepository
    {
        private static readonly string DataDirectory =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        private static readonly string FilePath =
            Path.Combine(DataDirectory, "Products.json");

        private static Dictionary<Guid, ProductEntity> _db = LoadFromFile();

        public Product? GetById(Guid id) =>
            _db.TryGetValue(id, out var entity) ? MapToDomain(entity) : null;

        public IEnumerable<Product> GetAll() =>
            _db.Values.Select(MapToDomain).ToList();

        public void Add(Product product)
        {
            _db[product.Id] = MapToEntity(product);
            SaveToFile();
        }

        public void Update(Product product)
        {
            if (!_db.ContainsKey(product.Id))
                throw new InfrastructureException("Produit inexistant");

            _db[product.Id] = MapToEntity(product);
            SaveToFile();
        }

        public void Delete(Guid id)
        {
            _db.Remove(id);
            SaveToFile();
        }

        private static void SaveToFile()
        {
            if (!Directory.Exists(DataDirectory))
                Directory.CreateDirectory(DataDirectory);

            Console.WriteLine("Saving to: " + Path.GetFullPath(FilePath));

            var json = JsonSerializer.Serialize(_db,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(FilePath, json);
        }

        private static Dictionary<Guid, ProductEntity> LoadFromFile()
        {
            Console.WriteLine("Loading from: " + Path.GetFullPath(FilePath));

            if (!File.Exists(FilePath))
                return new Dictionary<Guid, ProductEntity>();

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<Dictionary<Guid, ProductEntity>>(json)
                   ?? new Dictionary<Guid, ProductEntity>();
        }

        private static Product MapToDomain(ProductEntity e) =>
            new Product(e.Id, e.Name, e.Price, e.IsActive, e.CreatedAt);

        private static ProductEntity MapToEntity(Product p) =>
            new ProductEntity
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            };
    }
}