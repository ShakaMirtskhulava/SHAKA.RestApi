using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SHAKA.RestApi.Idempotency
{
    /// <summary>
    /// Provides methods for detecting if operations are marked as idempotent.
    /// </summary>
    public interface IIdempotencyDetector
    {
        /// <summary>
        /// Determines whether the specified endpoint is marked as idempotent.
        /// </summary>
        /// <param name="requestPath">The request path.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="methodInfo">Optional method info for controller actions.</param>
        /// <param name="expirationTime">If idempotent, receives the configured expiration time.</param>
        /// <returns>True if the operation is idempotent; otherwise, false.</returns>
        bool IsIdempotent(string requestPath, string httpMethod, MethodInfo? methodInfo, out TimeSpan expirationTime);
    }
}
