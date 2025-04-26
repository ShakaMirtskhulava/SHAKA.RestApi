# SHAKA.RestApi - REST API Abstraction Library

This library provides flexible, infrastructure-independent abstractions for implementing common REST API patterns and features in ASP.NET Core applications. While the current focus is on idempotency support, the library is designed to evolve as a comprehensive toolkit for building robust, well-architected REST APIs.

## Idempotency Implementation

The current version provides a complete abstraction layer for implementing idempotent API endpoints. It supports both controller-based APIs and minimal APIs, allowing you to ensure that duplicate requests with the same idempotency key produce the same result without re-executing operations.

## Core Concepts

**Idempotency** in APIs means that multiple identical requests have the same effect as a single request. This is crucial for:
- Ensuring data consistency when network failures occur
- Preventing duplicate resource creation
- Supporting retry mechanisms without side effects

## Components Overview

### Core Interfaces

#### `IIdempotencyKeyProvider`

Defines how to extract idempotency keys from HTTP requests.

```csharp
public interface IIdempotencyKeyProvider
{
    Task<string?> GetIdempotencyKeyAsync(CancellationToken cancellationToken = default);
}
```

Implement this interface to customize how idempotency keys are extracted from requests (e.g., from headers, query parameters, or request body).

#### `IIdempotencyStore`

Handles the storage and retrieval of idempotent operation results.

```csharp
public interface IIdempotencyStore
{
    Task<bool> TryCreateRequestRecordAsync(string key, string requestPath, string requestMethod, 
        TimeSpan expirationTime, CancellationToken cancellationToken = default);
    
    Task<IdempotentResponse?> GetResponseAsync(string key, CancellationToken cancellationToken = default);
    
    Task StoreResponseAsync(string key, IdempotentResponse response, CancellationToken cancellationToken = default);
}
```

This interface should be implemented in infrastructure-specific libraries (e.g., SHAKA.API.EF, SHAKA.API.Dapper) to provide persistence for idempotency records.

#### `IIdempotencyDetector`

Determines if an operation is marked as idempotent.

```csharp
public interface IIdempotencyDetector
{
    bool IsIdempotent(string requestPath, string httpMethod, MethodInfo? methodInfo, out TimeSpan expirationTime);
}
```

This interface allows the system to detect idempotent operations based on attributes or global configuration.

### Model Classes

#### `IdempotentResponse`

Represents a stored response from an idempotent operation.

```csharp
public class IdempotentResponse
{
    public int StatusCode { get; set; }
    public string? ContentType { get; set; }
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

This class contains all the necessary information to reconstruct an HTTP response from a previously executed idempotent operation.

### Attributes

#### `IdempotentAttribute`

Mark API endpoints or controller classes as idempotent.

```csharp
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class IdempotentAttribute : Attribute
{
    public IdempotentAttribute(double expirationInHours = 24.0) { ... }
    public TimeSpan ExpirationTime { get; }
}
```

Apply this attribute to controllers or individual action methods to enable idempotency. You can specify how long the idempotency key should remain valid.

### Configuration

#### `IdempotencyOptions`

Configuration options for the idempotency system.

```csharp
public class IdempotencyOptions
{
    public string IdempotencyHeaderName { get; set; } = "Idempotency-Key";
    public TimeSpan DefaultExpirationTime { get; set; } = TimeSpan.FromHours(24);
    public bool AllPostsAreIdempotent { get; set; } = false;
    public bool AllPutsAreIdempotent { get; set; } = true;
    public bool EnforceStrictContentValidation { get; set; } = true;
}
```

These options control the behavior of the idempotency middleware, including which HTTP methods are treated as idempotent by default.

## Usage Examples

### 1. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
// Add core idempotency services
builder.Services.AddIdempotency(options => {
    options.IdempotencyHeaderName = "X-Idempotency-Key";
    options.AllPostsAreIdempotent = true;
    options.DefaultExpirationTime = TimeSpan.FromHours(48);
});

// Register a specific implementation of IIdempotencyStore
// This would come from your implementation library, e.g.:
// builder.Services.AddEntityFrameworkIdempotencyStore(options => { ... });
// or
// builder.Services.AddDapperIdempotencyStore(options => { ... });
```

### 2. Controller-based API Usage

```csharp
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [Idempotent(expirationInHours: 72)]  // Specify expiration time if needed
    public async Task<ActionResult<Order>> CreateOrder(OrderRequest request)
    {
        // Your implementation here
        // If this endpoint is called multiple times with the same idempotency key,
        // only the first request will be processed, and subsequent requests will
        // receive the cached response
    }
    
    // You can also mark an entire controller as idempotent
    // [Idempotent]
    // This will apply to all action methods in the controller
}
```

### 3. Minimal API Usage

```csharp
app.MapPost("/api/orders", [Idempotent] async (OrderRequest request, 
    [FromServices] IOrderService orderService) => 
{
    // Create order logic here
    var order = await orderService.CreateOrderAsync(request);
    return Results.Created($"/api/orders/{order.Id}", order);
})
.WithName("CreateOrder");
```

## Implementation Notes

This library provides only the abstraction layer for idempotency. To use it in a real application, you need to:

1. Implement the `IIdempotencyStore` interface for your specific storage mechanism (e.g., EF Core, Dapper)
2. Create a middleware that uses these abstractions to intercept requests and provide idempotent behavior

## Extension Points

The library is designed to be extensible:

- Implement custom `IIdempotencyKeyProvider` to change how keys are extracted from requests
- Create different `IIdempotencyStore` implementations for various storage technologies
- Implement custom `IIdempotencyDetector` for more complex idempotency detection logic

## Infrastructure-Specific Implementations

Future libraries that will provide concrete implementations:

- **SHAKA.API.EF**: Entity Framework Core implementation
- **SHAKA.API.Dapper**: Dapper implementation
- **SHAKA.API.Redis**: Redis-based implementation for high-performance scenarios

## Best Practices

1. Use idempotency keys for non-idempotent operations like POST requests
2. Choose appropriate expiration times based on your business requirements
3. Document the idempotency key requirement in your API documentation
4. Consider implementing a fallback mechanism when idempotency keys are missing
5. Implement proper error handling for idempotency-related failures
