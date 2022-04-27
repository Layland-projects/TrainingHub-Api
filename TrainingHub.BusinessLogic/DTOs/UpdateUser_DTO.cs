namespace TrainingHub.BusinessLogic.DTOs
{
    public class UpdateUser_DTO
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Role { get; set; }
        public string? Username { get; set; }
    }
}
