using ToolHub.Services;
using ToolHub.Utils;

namespace ToolHub;

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            const int minimumSignupAge = 18;
            bool IsValidInput = false;
            Console.WriteLine("\n--- Age Verification ---");
            Console.WriteLine("Age Check: How old are you?");

            while (!IsValidInput)
            {
                Console.Write("Age: ");
                string? ageInput = Console.ReadLine();

                if (int.TryParse(ageInput, out int userAge))
                {
                    if (userAge >= minimumSignupAge)
                    {
                        Console.Clear();
                        Console.WriteLine("\nAge check succeeded. Would you like to:");
                        Console.WriteLine("1. Create an account");
                        Console.WriteLine("2. Sign in");
                        Console.WriteLine("3. Exit");

                        bool menuChoiceValid = false;
                        while (!menuChoiceValid)
                        {
                            Console.Write("Please enter 1, 2, or 3(Default: 1): ");
                            string signUpOrIn = Console.ReadLine() ?? "1";

                            if (signUpOrIn == "1")
                            {
                                Visuals.SimulateLoading("Redirecting to signup...");
                                AccountManager.GetInformation(userAge);
                                menuChoiceValid = true;
                            }
                            else if (signUpOrIn == "2")
                            {
                                Visuals.SimulateLoading("Redirecting to Login...");
                                SignIn.Login();
                                menuChoiceValid = true;
                            }
                            else if (signUpOrIn == "3")
                            {
                                Visuals.SimulateLoading("Program Ending...");
                                menuChoiceValid = true;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(
                                    "Invalid choice. Please input your age with numerical values."
                                );
                                Console.ResetColor();
                            }
                        }
                        IsValidInput = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sorry, you must be 18+ to sign up.");
                        Console.ResetColor();
                        IsValidInput = true;
                    }
                }
                else
                {
                    Console.WriteLine("Please input an actual age (numbers only).");
                }
            }
        }
    }
}
