namespace WorkPlanner.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Nickanme { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }

        public string Role { get; set; }

    }
}
