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
        private const int k_MinBoardHeightAndWidth = 4;
        private const int k_MaxBoardHeightAndWidth = 6;
        private const int k_AllowedCellInputLength = 2;
        private const char k_FirstColoumnLetter = 'A';
        private const char k_FirstRowDigit = '1';

        // Ask for the player's name, and check input integrity
        public static string GetPlayerName()
        {
            string playerName = string.Empty;
            bool nameIntegrity = false;

            do
            {
                Console.WriteLine("Please enter your name: ");
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
                Console.WriteLine("Please enter desired mod");
                Console.WriteLine("Press 1 for PVP");
                Console.WriteLine("Press 2 for PVC");
                chosenMod = Console.ReadLine();
            } while (!checkGameModeValidity(chosenMod, out eChosenMod));

            return eChosenMod;
        }

        // Ask for the second player's name, and check input integrity
        public static string GetSecondPlayerName(PairsGame.eGameMode i_ChosenMode)
        {
            string playerName = null;

            if (i_ChosenMode == eGameMode.PlayerVsPlayer)
            {
                playerName = IO.GetPlayerName();
            }

            return playerName;
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
            Ex02.ConsoleUtils.Screen.Clear();
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
        public static string GetCell(int i_BoardHeight, int i_BoardWidth)
        {
            string chosenCell;

            do
            {
                Console.WriteLine("Please enter cell (ex. B4)");
                chosenCell = Console.ReadLine();
            } while (!checkCellInputValidity(chosenCell, i_BoardHeight, i_BoardWidth));

            Ex02.ConsoleUtils.Screen.Clear();// Clear the screen after finished getting the cell
            return chosenCell;
        }

        // This function checks the board cell input validity
        private static bool checkCellInputValidity(string i_CellInput, int i_BoardHeight, int i_BoardWidth)
        {
            bool isValid = false;
            char lastLetterInCols = (char)(k_FirstColoumnLetter + (i_BoardWidth - 1));
            char lastDigitInRows = (char)(k_FirstRowDigit + (i_BoardHeight - 1));

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
            else
            {
                isValid = true;
            }

            return isValid;
        }
    }
}