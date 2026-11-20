namespace iquise_lab14.Api.Models;

public record Product(int Id, string Name, decimal Price);

public record CreateProductRequest(string Name, decimal Price);
