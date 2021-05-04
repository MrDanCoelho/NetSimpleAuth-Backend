using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace NetSimpleAuth.Backend.Domain.Entities
{
    public class LogEntity
    {
        [Key]
        public int Id { get; set; }

        public string Ip { get; set; }

        public string App { get; set; }

        public string User { get; set; }

        public DateTime Date { get; set; }

        public string RequestType { get; set; }
        
        public string RequestUrl { get; set; }

        public string RequestProtocol { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public int? ContentSize { get; set; }

        public string ResponseUrl { get; set; }

        public string UserAgent { get; set; }
    }
}