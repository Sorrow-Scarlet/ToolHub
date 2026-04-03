using System.Text.Json;
using ToolHub.Models;
using ToolHub.Services;

namespace ToolHub.Utils;

public class SettingsMenu
{
    public static void OpenSettings()
    {
        bool backToMenu = false;
        while (!backToMenu)
        {
            Visuals.DisplayLogo();
            Console.WriteLine("--- Settings ---");
            Console.WriteLine("1. View User Profile");
            Console.WriteLine("2. Change Theme Color");
            Console.WriteLine("3. Change Password");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("\nSelection: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowProfile();
                    break;
                case "2":
                    ChangeColor();
                    break;
                case "3":
                    ChangePassword();
                    break;
                case "4":
                    backToMenu = true;
                    break;
            }
        }
    }

    private static void ShowProfile()
    {
        if (AccountManager.CurrentUser == null)
        {
            Console.WriteLine("No user logged in. Please log in first.");
            return;
        }
        Console.Clear();
        Console.WriteLine("--- User Profile ---");
        Console.WriteLine($"Username: {AccountManager.CurrentUser.Username}");
        Console.WriteLine($"Email:    {AccountManager.CurrentUser.Email}");
        Console.WriteLine($"User ID:  {AccountManager.CurrentUser.UserID}");
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void ChangeColor()
    {
        if (AccountManager.CurrentUser == null)
        {
            Console.WriteLine("No user logged in. Please log in first.");
            return;
        }
        Console.Clear();
        Console.WriteLine("--- Choose a Theme Color ---");
        Console.WriteLine("1. Cyan");
        Console.WriteLine("2. Magenta");
        Console.WriteLine("3. Yellow");
        Console.WriteLine("4. White");
        Console.WriteLine("5. Green");
        Console.Write("\nSelection: ");

        string choice = Console.ReadLine() ?? "";
        switch (choice)
        {
            case "1":
                AccountManager.CurrentUser.FavoriteColor = ConsoleColor.Cyan;
                break;
            case "2":
                AccountManager.CurrentUser.FavoriteColor = ConsoleColor.Magenta;
                break;
            case "3":
                AccountManager.CurrentUser.FavoriteColor = ConsoleColor.Yellow;
                break;
            case "4":
                AccountManager.CurrentUser.FavoriteColor = ConsoleColor.White;
                break;
            case "5":
                AccountManager.CurrentUser.FavoriteColor = ConsoleColor.Green;
                break;
            default:
                Console.WriteLine("Invalid input. Please enter a number between 1 and 5.");
                ChangeColor();
                break;
        }

        SaveSettings();
        Console.WriteLine("Theme updated! Press any key...");
        Console.ReadKey();
    }

    private static void SaveSettings()
    {
        if (AccountManager.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to change settings.");
            return;
        }
        string fileName = "users.json";
        if (File.Exists(fileName))
        {
            var users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(fileName));
            if (users == null)
            {
                Console.WriteLine("Failed to deserialize user data, please try again later.");
                return;
            }
            var user = users.FirstOrDefault(u => u.UserID == AccountManager.CurrentUser.UserID);
            if (user != null)
            {
                user.FavoriteColor = AccountManager.CurrentUser.FavoriteColor;
                File.WriteAllText(
                    fileName,
                    JsonSerializer.Serialize(
                        users,
                        new JsonSerializerOptions { WriteIndented = true }
                    )
                );
            }
        }
    }

    private static void ChangePassword()
    {
        if (AccountManager.CurrentUser is null)
        {
            Console.WriteLine("You must be logged in to change your password.");
            return;
        }
        Console.WriteLine("\n--- Change Password ---");
        Console.Write("Enter Current Password: ");
        string oldPassword = AccountManager.GetMaskedPassword();

        if (
            AccountManager.HashPassword(oldPassword, AccountManager.CurrentUser.Salt)
            != AccountManager.CurrentUser.PasswordHash
        )
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Incorrect current password.");
            Console.ResetColor();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter New Password: ");
        string newPassword = AccountManager.GetMaskedPassword();

        AccountManager.CurrentUser.PasswordHash = AccountManager.HashPassword(
            newPassword,
            AccountManager.CurrentUser.Salt
        );

        string fileName = "users.json";
        if (File.Exists(fileName))
        {
            try
            {
                string json = File.ReadAllText(fileName);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

                var userToUpdate = users.FirstOrDefault(u =>
                    u.UserID == AccountManager.CurrentUser.UserID
                );
                if (userToUpdate != null)
                {
                    userToUpdate.PasswordHash = AccountManager.CurrentUser.PasswordHash;
                    File.WriteAllText(
                        fileName,
                        JsonSerializer.Serialize(
                            users,
                            new JsonSerializerOptions { WriteIndented = true }
                        )
                    );

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nPassword updated successfully!");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving password: {ex.Message}");
            }
        }

        Console.WriteLine("Press any key to return...");
        Console.ReadKey();
    }
}
