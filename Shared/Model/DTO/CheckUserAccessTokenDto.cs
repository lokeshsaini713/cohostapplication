namespace Shared.Model.DTO
{
    public class CheckUserAccessTokenDto
    {
        public bool IsTokenExists { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class SortOrderDto
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
    }
}
