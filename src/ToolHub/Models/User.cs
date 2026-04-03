namespace ToolHub.Models;

public class User
{
    public int UserID { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Salt { get; set; }
    public int Age { get; set; }
    public ConsoleColor FavoriteColor { get; set; } = ConsoleColor.White;
}
