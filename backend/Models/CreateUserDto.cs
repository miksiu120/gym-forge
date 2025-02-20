namespace WorkPlanner.Models
{
    public class CreateUserDto
    {
        public string Nickanme { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string? Weight { get; set; }
        public string? Height { get; set; }
    }
}
