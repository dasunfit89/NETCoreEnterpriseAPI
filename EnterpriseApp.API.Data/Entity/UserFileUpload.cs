namespace EnterpriseApp.API.Data.Entity
{
    public class UserFileUpload : BaseEntity
    {
        public string FileName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public string UserId { get; set; }
    }
}