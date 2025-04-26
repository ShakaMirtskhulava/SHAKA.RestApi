using System;
using System.Text.Json;

namespace SHAKA.RestApi.Idempotency
{
    /// <summary>
    /// Represents a stored response from an idempotent operation.
    /// </summary>
    public class IdempotentResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the content type of the response.
        /// </summary>
        public string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the serialized response data.
        /// </summary>
        public string? Body { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the response was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
