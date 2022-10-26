using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PagingQueryParam
    {
        [Range(-1, int.MaxValue)]
        public int StartingIndex { get; set; }

        [Range(-1, int.MaxValue)]
        public int PageSize { get; set; } = 20;

        public string Sort { get; set; }

        public string Filter { get; set; }
    }
}
