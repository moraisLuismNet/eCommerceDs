namespace eCommerceDs.DTOs
{
    public class UserUpdateDTO
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Role { get; set; }

    }
}
