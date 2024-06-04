using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;
using static Ex02.PairsGame;

namespace Ex02
{
    public class IO
    {
        private const string k_PVPModeInput = "1";
        private const string k_PVCModeInput = "2";
        private const string k_YChar = "Y";
        private const string k_NChar = "N";
        private const int k_MinBoardHeightAndWidth = 4;
        private const int k_MaxBoardHeightAndWidth = 6;
        private const int k_AllowedCellInputLength = 2;
        public const char k_FirstColoumnLetter = 'A';
        public const char k_FirstRowDigit = '1';
        public const char k_ZeroDigit = '0';
        private const string k_ExitGame = "Q";


        // Ask for the player's name, and check input integrity
        public static string GetPlayerName(string currentPlayer)
        {
            string playerName = string.Empty;
            bool nameIntegrity = false;

            do
            {
                Console.WriteLine(string.Format(("Please enter {0} player's name: "), currentPlayer));
                playerName = Console.ReadLine();
            } while (nameIntegrity); // TODO: Check for input integrity

            return playerName;
        }

        // Ask the player for his wanted game mode, and check input integrity
        public static PairsGame.eGameMode GetGameMode()
        {
            PairsGame.eGameMode eChosenMod;
            string chosenMod = null;

            do
            {
                Console.WriteLine("Please enter desired mode");
                Console.WriteLine("Press 1 for PVP");
                Console.WriteLine("Press 2 for PVC");
                chosenMod = Console.ReadLine();
            } while (!checkGameModeValidity(chosenMod, out eChosenMod));

            return eChosenMod;
        }

        // Check for game mode input integrity
        private static bool checkGameModeValidity(string i_ModeInput, out PairsGame.eGameMode o_Mode)
        {
            o_Mode = PairsGame.eGameMode.PlayerVsPlayer;
            bool isValid = false;

            switch (i_ModeInput)
            {
                case k_PVPModeInput:
                    o_Mode = PairsGame.eGameMode.PlayerVsPlayer;
                    isValid = true;
                    break;
                case k_PVCModeInput:
                    o_Mode = PairsGame.eGameMode.PlayerVsComputer;
                    isValid = true;
                    break;
                default:
                    Console.WriteLine("Invalid game mode input");
                    break;
            }

            return isValid;
        }

        // Get board's height and width from user
        public static void GetBoardHeightAndWidth(out int o_BoardHeight, out int o_BoardWidth)
        {
            string heightInput = null;
            string widthInput = null;
            o_BoardHeight = 0;
            o_BoardWidth = 0;

            do
            {
                Console.WriteLine("Please enter board height");
                heightInput = Console.ReadLine();
                Console.WriteLine("Please enter board width");
                widthInput = Console.ReadLine();
            } while (!checkBoardHeightAndWidthValidity(heightInput, widthInput, out o_BoardHeight, out o_BoardWidth));
        }

        // Check board height and width input for validity
        private static bool checkBoardHeightAndWidthValidity(string i_HeightInput, string i_WidthInput, out int o_BoardHeight, out int o_BoardWidth)
        {
            bool isValid = false;
            bool firstParse = int.TryParse(i_HeightInput, out o_BoardHeight);
            bool secondParse = int.TryParse(i_WidthInput, out o_BoardWidth);

            // Check if both parses were successful, and act accordingly   
            if (firstParse && secondParse)
            {
                if (o_BoardHeight >= k_MinBoardHeightAndWidth && o_BoardHeight <= k_MaxBoardHeightAndWidth && o_BoardWidth >= k_MinBoardHeightAndWidth && o_BoardWidth <= k_MaxBoardHeightAndWidth)
                {
                    // Check if even amount of cells in board
                    if ((o_BoardHeight * o_BoardWidth) % 2 == 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Console.WriteLine("Odd amount of cells in board is not allowed!");
                    }
                }
                else
                {
                    Console.WriteLine("Board's height or width input is invalid!");
                }
            }
            else
            {
                Console.WriteLine("Board's height or width input is invalid!");
            }

            return isValid;
        }

        // TODO: MAKE PRETTIER CODE
        public static void PrintBoard(Board i_Board)
        {
            Console.Write("  "); // For row numbers alignment
            // Printing the top line of letters
            for (int i = 0; i < i_Board.BoardWidth; i++)
            {
                char columnLetter = (char)(k_FirstColoumnLetter + i);
                Console.Write("  " + columnLetter + " ");
            }

            Console.Write(Environment.NewLine);
            Console.Write("  ");
            for (int i = 0; i < i_Board.BoardWidth; i++)
            {
                Console.Write("====");
            }

            Console.Write(Environment.NewLine);

            for (int i = 0; i < i_Board.BoardHeight; i++)
            {
                Console.Write(i + 1);

                for (int j = 0; j < i_Board.BoardWidth; j++)
                {
                    if (i_Board.BoardMatrix[i, j].IsVisible)
                    {
                        Console.Write(" | ");
                        Console.Write(i_Board.BoardMatrix[i, j].Char);
                    }
                    else
                    {
                        Console.Write(" |  ");
                    }
                }

                Console.Write(" |");
                Console.Write(Environment.NewLine);
                Console.Write("  ");
                for (int j = 0; j < i_Board.BoardWidth; j++)
                {
                    Console.Write("====");
                }

                Console.Write(Environment.NewLine);
            }

        }

        // This function asks a player for the cell he chooses
        public static string GetCellFromPlayer(HumanPlayer i_CurrentPlayer, Board i_Board)
        {
            string chosenCell;

            do
            {
                Console.WriteLine($"{i_CurrentPlayer.Name}, please enter cell (ex. B4)");
                chosenCell = Console.ReadLine();

                if(chosenCell.ToUpper() == k_ExitGame)      // Exit game if Q is pressed
                {
                    Environment.Exit(1);
                }

            } while (!checkCellInputValidity(chosenCell, i_Board));

            return chosenCell;
        }

        // This function checks the board cell input validity
        private static bool checkCellInputValidity(string i_CellInput, Board i_Board)
        {
            bool isValid = false;
            char lastLetterInCols = (char)(k_FirstColoumnLetter + (i_Board.BoardWidth - 1));
            char lastDigitInRows = (char)(k_FirstRowDigit + (i_Board.BoardHeight - 1));

            if (i_CellInput.Length != k_AllowedCellInputLength)
            {
                Console.WriteLine("Cell input must contain 2 letters!");
            }
            else if (!char.IsLetter(i_CellInput[0]))
            {
                Console.WriteLine("First character must be a letter!");
            }
            else if (!char.IsDigit(i_CellInput[1]))
            {
                Console.WriteLine("Second character must be a digit!");
            }
            else if (i_CellInput[0] > lastLetterInCols || i_CellInput[1] > lastDigitInRows)
            {
                Console.WriteLine("Cell doesn't exist in the board!");
            }
            else if (i_Board.CheckCellVisibility(i_CellInput))
            {
                Console.WriteLine("You chose a visible cell. Choose a different cell.");
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }
        // This function clears the screen
        public static void ClearScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
        }

        // this function prints the winner and final score
        public static void PrintWinnerAndScores(HumanPlayer firstPlayer, HumanPlayer secondPlayer, ComputerPlayer computerPlayer, PairsGame.eGameMode i_GameMode)
        {
            // Clear screen
            ClearScreen();

            // Print winner and scores
            if (i_GameMode == PairsGame.eGameMode.PlayerVsPlayer)
            {
                // Get The winner
                HumanPlayer winnerPlayer = firstPlayer.Points > secondPlayer.Points ? firstPlayer : secondPlayer;

                // Print winner message
                Console.WriteLine($"The Winner Is {winnerPlayer.Name}!");

                // Print final scores
                Console.WriteLine($"{secondPlayer.Name} finished with {secondPlayer.Points} points!");
            }
            else
            {
                // Print winner message
                if (firstPlayer.Points > computerPlayer.Points)
                {
                    Console.WriteLine($"The Winner Is {firstPlayer.Name}!");
                }
                else
                {
                    Console.WriteLine($"The Winner Is The Computer Player!");
                }

                // Print final scores
                Console.WriteLine($"Computer player finished with {secondPlayer.Points} points!");
            }
            Console.WriteLine($"{firstPlayer.Name} finished with {firstPlayer.Points} points!");
        }

        // This function asks the player for another round
        public static bool AskPlayerForAnotherRound()
        {
            bool isQuit = false;
            string response;

            do
            {
                Console.WriteLine("Would you like to play another round? Type y for yes, and n for no");
                response = Console.ReadLine();
            } while (!validateResponse(response));

            if (response.ToUpper() == k_NChar)
            {
                isQuit = true;
            }

            return isQuit;
        }

        // This function checks the validity of the response in the function 'AskPlayerForAnotherRound'
        private static bool validateResponse(string response)
        {
            bool isValid = false;

            if(response.ToUpper() == k_YChar || response.ToUpper() == k_NChar)
            {
                isValid = true;
            }
            else
            {
                Console.WriteLine("Invalid character or string");
            }

            return isValid;
        }
    }
}