using System;
using TicTacToe.Model;

namespace TicTacToe.View
{
    /// <summary>
    /// Class that handles all the console interactions
    /// </summary>
    public class ConsoleUI
    {
        /// <summary>
        /// Method to get the user input for authentication
        /// </summary>
        /// <returns> The selected option </returns>
        public int GetUserInput()
        {
            Console.WriteLine("Select the option from below\n");
            Console.WriteLine("Enter [1] if you are a new user");
            Console.WriteLine("Enter [2] to login");
            Console.WriteLine("Enter [3] to close the application");
            string userInput = Console.ReadLine() ?? string.Empty;
            if(string.IsNullOrEmpty(userInput))
            {
                PrintErrorMessage("Input cannot be empty");
            }
            if(!int.TryParse(userInput, out int result))
            {
                PrintErrorMessage("Enter a valid integer");
            }

            return result;
        }

        /// <summary>
        /// Method to select the category of game at each level
        /// </summary>
        /// <typeparam name="T"> Generic type which can be any enum based on the level </typeparam>
        /// <returns> The selected option </returns>
        public T SelectGameCategory<T>() where T : Enum
        {
            var options = Enum.GetValues(typeof(T));
            for(int i = 0; i < options.Length; i++)
            {
                var option = options.GetValue(i);
                Console.WriteLine($"Enter [{i + 1}] to choose {option}");
            }
            string userInput = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(userInput))
            {
                PrintErrorMessage("Input cannot be empty");
            }
            if (!int.TryParse(userInput, out int result) || !Enum.IsDefined(typeof(Mode), result))
            {
                PrintErrorMessage("Enter a valid integer");
            }

            return (T)Enum.ToObject(typeof(T), result);
        }

        /// <summary>
        /// Method to get string input
        /// </summary>
        /// <param name="message"> Message to be displayed </param>
        /// <returns> The string input from the user </returns>
        public string GetStringInput(string message)
        {
            while (true)
            {
                Console.WriteLine($"Enter the {message}");
                string userName = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(userName))
                {
                    PrintErrorMessage($"{message} cannot be empty");
                }
                else
                {
                    return userName;
                }
            }
        }

        /// <summary>
        /// Method to get the grid input for the board
        /// </summary>
        /// <returns> The input from the user </returns>
        public int GetGridInput()
        {
            while (true)
            {
                Console.WriteLine("Enter the grid number from the above example");
                string stringInput = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(stringInput))
                {
                    PrintErrorMessage("Input can not be empty... Try again\n");
                }
                if (int.TryParse(stringInput, out int result))
                {
                    return result;
                }
            }
        }

        /// <summary>
        /// Method to select the symbol to be played with
        /// </summary>
        /// <returns> The selected symbol </returns>
        public Symbols SelectSymbol()
        {
            Console.WriteLine("Select the symbol to play with:");
            while(true)
            {
                Console.WriteLine("Enter [1] to select symbol X");
                Console.WriteLine("Enter [2] to select symbol O");
                string userInput = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrEmpty(userInput))
                {
                    PrintErrorMessage("Symbol can not be empty");
                }
                if(!int.TryParse(userInput,out int result))
                {
                    PrintErrorMessage("Enter a valid integer from the above menu");
                }
                if (Enum.IsDefined(typeof(Symbols), result))
                {
                    return (Symbols) result;
                }
            }
        }

        /// <summary>
        /// Method to get input to continue playing
        /// </summary>
        /// <returns> True if the user wants to continue </returns>
        public bool GetInputToContinue()
        {
            Console.WriteLine("Do you want to continue playing");
            Console.WriteLine("Enter [Y] to continue");
            Console.WriteLine("Enter any other keys to go to main menu");
            string userInput = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(userInput))
            {
                PrintErrorMessage("Input can not be empty");
            }
            if(userInput.ToLower() == "y")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to print the game board
        /// </summary>
        /// <param name="board"> The board of string to be displayed </param>
        public void PrintGameBoard(string[] board)
        {
            Console.Clear();
            Console.WriteLine("┌─────┬─────┬─────┐");
            Console.WriteLine($"│ {board[0]}  │ {board[1]}  │ {board[2]}  │");
            Console.WriteLine("├─────┼─────┼─────┤");
            Console.WriteLine($"│ {board[3]}  │ {board[4]}  │ {board[5]}  │");
            Console.WriteLine("├─────┼─────┼─────┤");
            Console.WriteLine($"│ {board[6]}  │ {board[7]}  │ {board[8]}  │");
            Console.WriteLine("└─────┴─────┴─────┘");
        }

        /// <summary>
        /// Method to print success message
        /// </summary>
        /// <param name="message"></param>
        public void PrintSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Method to print error message
        /// </summary>
        /// <param name="message"></param>
        public void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Method to print the game info
        /// </summary>
        /// <param name="info"></param>
        public void PrintGameInfo(string info)
        {
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine(info);
            Console.ResetColor();
        }
    }
}
