using System;
using System.Threading;
using System.Threading.Tasks;

namespace SHAKA.RestApi.Idempotency
{
    /// <summary>
    /// Defines a contract for extracting idempotency keys from requests.
    /// </summary>
    public interface IIdempotencyKeyProvider
    {
        /// <summary>
        /// Extracts an idempotency key from the current HTTP context.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The idempotency key if found; otherwise, null.</returns>
        Task<string?> GetIdempotencyKeyAsync(CancellationToken cancellationToken = default);
    }
}
