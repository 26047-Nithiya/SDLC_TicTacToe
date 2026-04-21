using System;
using System.Text.RegularExpressions;
using TicTacToe.Repository;
using TicTacToe.View;

namespace TicTacToe.Utility
{
    /// <summary>
    /// Validation class
    /// </summary>
    public class Validation
    {
        private UserRepo userRepo;
        private ConsoleUI consoleUI;

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        /// <param name="userRepo"> Object reference for repo layer </param>
        /// <param name="consoleUI"> Object reference for the console UI layer </param>
        public Validation(UserRepo userRepo, ConsoleUI consoleUI)
        {
            this.userRepo = userRepo;
            this.consoleUI = consoleUI;
        }

        /// <summary>
        /// Method to validate the username
        /// </summary>
        /// <param name="userName"> Name of the user </param>
        /// <returns> True is the username is valid </returns>
        public bool ValidateUserName(string userName)
        {
            if (userName.All(char.IsDigit))
            {
                consoleUI.PrintErrorMessage("User name can not be all numbers");
                return false;
            }
            var users = userRepo.GetUsers();
            foreach(var user in users)
            {
                if(user.Name == userName)
                {
                    consoleUI.PrintErrorMessage("UserName Already Exist\n");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Method to validate the password
        /// </summary>
        /// <param name="password"> Password for the user </param>
        /// <returns> True if the password is valid </returns>
        public bool ValidatePassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            bool isValid = Regex.IsMatch(password, pattern);

            if (password.Length < 8)
            {
                consoleUI.PrintErrorMessage("Password must be at least 8 characters long.\n");
                return false;
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                consoleUI.PrintErrorMessage("Password must contain at least one uppercase letter.\n");
                return false;
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                consoleUI.PrintErrorMessage("Password must contain at least one lowercase letter.\n");
                return false;
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                consoleUI.PrintErrorMessage("Password must contain at least one number.\n");
                return false;
            }

            if (!Regex.IsMatch(password, @"[@$!%*?&]"))
            {
                consoleUI.PrintErrorMessage("Password must contain at least one special character (@$!%*?&).\n");
                return false;
            }

            return isValid;
        }

        /// <summary>
        /// Method to validate the move made by the user
        /// </summary>
        /// <param name="move"> Move of the user </param>
        /// <param name="board"> Board with list of strings </param>
        /// <returns> True if the move is valid </returns>
        public bool ValidateMove(int move, string[] board)
        {
            if (move < 0 || move >= board.Length)
            {
                consoleUI.PrintErrorMessage("Invalid Move. Enter a valid move between 0 and 9");
                return false;
            }

            if (!int.TryParse(board[move - 1], out int _))
            {
                consoleUI.PrintErrorMessage("Invalid Move. Place already Occupied");
                Thread.Sleep(1000);
                return false;
            }

            return true;
        }
    }
}
