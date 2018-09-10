using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SiteHealthChecker.Models
{
    public class WebResource
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the web resource (userfriendly text).
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Uri to the web resource.
        /// </summary>
        [Required]
        [Url]
        public string Uri { get; set; }

        /// <summary>
        /// Http status code returned from the web resource after last request.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Whether the web resource is available
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Date of the last update of the record in the DB.
        /// Record is update only when the chenges on of the folowing properties:
        /// Name, Uri, StatusCode.
        /// </summary>
        public DateTimeOffset LastUpdated { get; set; }
    }
}
