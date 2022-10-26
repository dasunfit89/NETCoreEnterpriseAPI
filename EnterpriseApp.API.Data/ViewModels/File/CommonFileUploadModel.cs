namespace EnterpriseApp.API.Data.ViewModels
{
    public class CommonFileUploadModel
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int UserId { get; set; }

        public int ArticleId { get; set; }

        public string Url { get; set; }

        public bool IsMainImage { get; set; }

        public int Status { get; set; }

        public CommonFileUploadModel()
        {

        }

    }
}