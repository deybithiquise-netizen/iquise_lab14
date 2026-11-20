using System.Collections.Concurrent;
using iquise_lab14.Api.Models;

namespace iquise_lab14.Api.Services;

/// <summary>
/// In-memory store so the app can be deployed without a database.
/// </summary>
public class ProductStore
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private int _lastId;

    public ProductStore()
    {
        Add(new CreateProductRequest("Teclado", 120.00m));
        Add(new CreateProductRequest("Mouse", 45.50m));
        Add(new CreateProductRequest("Monitor", 780.90m));
    }

    public IReadOnlyList<Product> GetAll() => _products.Values.OrderBy(p => p.Id).ToList();

    public Product? GetById(int id) => _products.GetValueOrDefault(id);

    public Product Add(CreateProductRequest request)
    {
        var product = new Product(Interlocked.Increment(ref _lastId), request.Name, request.Price);
        _products[product.Id] = product;
        return product;
    }

    public bool Delete(int id) => _products.TryRemove(id, out _);
}
