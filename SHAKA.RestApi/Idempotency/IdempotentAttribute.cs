using System;

namespace SHAKA.RestApi.Idempotency
{
    /// <summary>
    /// Marks an API endpoint or HTTP method as idempotent, ensuring duplicate requests with the same
    /// idempotency key will return the same result without re-executing the operation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class IdempotentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdempotentAttribute"/> class.
        /// </summary>
        /// <param name="expirationInHours">The number of hours for which the idempotency key remains valid. Defaults to 24 hours.</param>
        public IdempotentAttribute(double expirationInHours = 24.0)
        {
            ExpirationTime = TimeSpan.FromHours(expirationInHours);
        }

        /// <summary>
        /// Gets the expiration time for the idempotency key.
        /// </summary>
        public TimeSpan ExpirationTime { get; }
    }
}
