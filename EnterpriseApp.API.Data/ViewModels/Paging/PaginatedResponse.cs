using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int TotalDataRecords { get; set; }
    }
}
