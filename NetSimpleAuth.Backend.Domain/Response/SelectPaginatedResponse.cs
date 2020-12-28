using System.Collections.Generic;

namespace NetPOC.Backend.Domain.Dto
{
    public class SelectPaginatedResponse<T> where T : class
    {
        public IEnumerable<T> obj { get; set; }
        public int count { get; set; }
    }
}