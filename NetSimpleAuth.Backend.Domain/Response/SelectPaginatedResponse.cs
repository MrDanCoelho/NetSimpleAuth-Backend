using System.Collections.Generic;

namespace NetSimpleAuth.Backend.Domain.Response
{
    public class SelectPaginatedResponse<T> where T : class
    {
        public IEnumerable<T> Obj { get; set; }
        public int Count { get; set; }
    }
}