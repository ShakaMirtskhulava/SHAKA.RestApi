namespace SHAKA.RestApi.Idempotency;

/// <summary>
/// Configuration options for the idempotency middleware.
/// </summary>
public class IdempotencyOptions
{
    /// <summary>
    /// Gets or sets the HTTP header name used to extract the idempotency key.
    /// Default is 'Idempotency-Key'.
    /// </summary>
    public string IdempotencyHeaderName { get; set; } = "Idempotency-Key";

    /// <summary>
    /// Gets or sets the default expiration time for idempotency keys.
    /// Default is 24 hours.
    /// </summary>
    public TimeSpan DefaultExpirationTime { get; set; } = TimeSpan.FromHours(24);

    /// <summary>
    /// Gets or sets a value indicating whether all POST requests should be treated as idempotent by default.
    /// Default is false.
    /// </summary>
    public bool AllPostsAreIdempotent { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether all PUT requests should be treated as idempotent by default.
    /// Default is true.
    /// </summary>
    public bool AllPutsAreIdempotent { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the idempotency middleware should enforce
    /// strict content validation (ensuring subsequent requests have identical content).
    /// Default is true.
    /// </summary>
    public bool EnforceStrictContentValidation { get; set; } = true;

    /// <summary>
    /// Gets or sets the interval at which expired idempotency keys are cleaned up.
    /// </summary>
    public TimeSpan CleanupInterval { get; set; }
}
