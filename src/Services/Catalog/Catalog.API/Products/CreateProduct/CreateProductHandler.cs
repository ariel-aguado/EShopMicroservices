namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category,
    int Count,
    bool IsAvailable) : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create the Product entity from the command
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price,
            Category = command.Category,
            Count = command.Count,
            IsAvailable = command.IsAvailable
        };
        
        // Save to a database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        
        // Return CreateProductResult result
        return new CreateProductResult(product.Id);
    }
}