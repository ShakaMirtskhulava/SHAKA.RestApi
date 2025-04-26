using System;
using System.Threading;
using System.Threading.Tasks;

namespace SHAKA.RestApi.Idempotency
{
    /// <summary>
    /// Defines a contract for storing and retrieving idempotent operation results.
    /// </summary>
    public interface IIdempotencyStore
    {
        /// <summary>
        /// Attempts to create a new idempotent request record.
        /// </summary>
        /// <param name="key">The idempotency key.</param>
        /// <param name="requestPath">The path of the request.</param>
        /// <param name="requestMethod">The HTTP method of the request.</param>
        /// <param name="expirationTime">How long the idempotency key should be valid.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if the key was created; false if it already exists.</returns>
        Task<bool> TryCreateRequestRecordAsync(
            string key, 
            string requestPath, 
            string requestMethod, 
            TimeSpan expirationTime,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a previously stored response for an idempotent request.
        /// </summary>
        /// <param name="key">The idempotency key.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The stored response if found; otherwise, null.</returns>
        Task<IdempotentResponse?> GetResponseAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Stores a response for an idempotent request.
        /// </summary>
        /// <param name="key">The idempotency key.</param>
        /// <param name="response">The response to store.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StoreResponseAsync(string key, IdempotentResponse response, CancellationToken cancellationToken = default);
    }
}
