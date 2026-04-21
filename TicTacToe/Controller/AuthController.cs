using TicTacToe.Model;
using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe.Controller
{
    public class AuthController
    {
        private IAuthService<User> authService;
        private ConsoleUI consoleUI;
        private Validation<User> validator;

        /// <summary>
        /// Constructor for initializing objects of other layers
        /// </summary>
        /// <param name="authService"> Object reference of the authentication services</param>
        /// <param name="consoleUI"> Object reference for the console UI layer</param>
        /// <param name="validator">Object reference for the validation layer</param>
        public AuthController(IAuthService<User> authService, ConsoleUI consoleUI, Validation<User> validator)
        {
            this.authService = authService;
            this.consoleUI = consoleUI;
            this.validator = validator;
        }

        /// <summary>
        /// Method to run once the application starts
        /// </summary>
        public Session Run()
        {
            consoleUI.DrawHeader();
            while (true)
            {
                int? userInput = consoleUI.GetUserInput();
                if(userInput != null)
                {
                    switch (userInput)
                    {
                        case 1:
                            RegistrationHandler();
                            break;
                        case 2:
                            string userName = consoleUI.GetStringInput("User name");
                            if(userName == null)
                            {
                                consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                                return null;
                            }
                            string password = consoleUI.GetStringInput("Password");
                            if(password == null)
                            {
                                consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                                return null;
                            }
                            var session = authService.Login(userName, password);
                            if (session != null)
                            {
                                consoleUI.PrintSuccessMessage("Login Successful\n");
                                return session;
                            }
                            else
                            {
                                consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                                return null;
                            }
                        case 3:
                            Environment.Exit(0);
                            break;
                        default:
                            consoleUI.PrintErrorMessage("Invalid option selection... Try again");
                            break;
                    }
                }
                else
                {
                    consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                    return null;
                }
            }
        }

        /// <summary>
        /// Method tha handles the registration
        /// </summary>
        private void RegistrationHandler()
        {
            while (true)
            {
                string userName = consoleUI.GetStringInput("User name");

                if (userName == null)
                {
                    consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                    return;
                }

                if (!validator.ValidateUserName(userName))
                    continue;

                string password = consoleUI.GetStringInput("Password\nPassword must be 8+ characters with uppercase, lowercase, number, and special character.");

                if (password == null)
                {
                    consoleUI.PrintErrorMessage("Too many invalid attempts. Exiting...");
                    return;
                }

                if (!validator.ValidatePassword(password))
                    continue;

                if (authService.Register(userName, password))
                {
                    consoleUI.PrintSuccessMessage("Sign-up successful\n");
                    break;
                }

                consoleUI.PrintErrorMessage("Sign-up not successful\n");
            }
        }
    }
}
