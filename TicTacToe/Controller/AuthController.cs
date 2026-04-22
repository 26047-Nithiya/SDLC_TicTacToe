using TicTacToe.Services;
using TicTacToe.Utility;
using TicTacToe.View;

namespace TicTacToe.Controller
{
    public class AuthController
    {
        private GameController gameController;
        private AuthService authService;
        private ConsoleUI consoleUI;
        private Validation validator;

        /// <summary>
        /// Constructor for initializing objects of other layers
        /// </summary>
        /// <param name="authService"> Object reference of the authentication services</param>
        /// <param name="consoleUI"> Object reference for the console UI layer</param>
        /// <param name="validator">Object reference for the validation layer</param>
        public AuthController(GameController gameController, AuthService authService, ConsoleUI consoleUI, Validation validator)
        {
            this.gameController = gameController;
            this.authService = authService;
            this.consoleUI = consoleUI;
            this.validator = validator;
        }

        /// <summary>
        /// Method to run once the application starts
        /// </summary>
        public bool Run()
        {
            while (true)
            {
                int userInput = consoleUI.GetUserInput();
                switch (userInput)
                {
                    case 1:
                        RegistrationHandler();
                        break;
                    case 2:
                        int retryCount = 3;
                        int i = 0;
                        while (i <= retryCount)
                        {
                            string userName = consoleUI.GetStringInput("User name");
                            string password = consoleUI.GetStringInput("Password");
                            if (authService.Login(userName, password))
                            {
                                consoleUI.PrintSuccessMessage("Login Successful\n");
                                return true;
                            }
                            else
                            {
                                consoleUI.PrintErrorMessage("User name and password not found\n");
                                i++;
                            }
                        }

                        consoleUI.PrintErrorMessage("Consecutive invalid tries... Exiting\n");
                        return false;
                    case 3:
                        return false;
                    default:
                        consoleUI.PrintErrorMessage("Invalid option selection... Try again");
                        break;
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
                if (validator.ValidateUserName(userName))
                {
                    string password = consoleUI.GetStringInput("Password");
                    if (validator.ValidatePassword(password))
                    {
                        if (authService.Register(userName, password))
                        {
                            consoleUI.PrintSuccessMessage("Sign-up successful\n");
                            break;
                        }
                        else
                        {
                            consoleUI.PrintErrorMessage("Sign-up not successful\n");
                        }
                    }
                }
            }
        }
    }
}
