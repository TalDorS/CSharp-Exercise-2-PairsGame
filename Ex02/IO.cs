using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class IO
    {
        private const string k_PVPModeInput = "1";
        private const string k_PVCModeInput = "2";
        private const int k_MinBoardHeightAndWidth = 4;
        private const int k_MaxBoardHeightAndWidth = 6;
        private const char k_FirstColoumnLetter = 'A';
        //Board m_Board { get; set; }
        // Ask for the player's name, and check input integrity
        public static string GetPlayerName()
        {
            string playerName = string.Empty;
            bool nameIntegrity = false;

            do
            {
                Console.WriteLine("Please enter your name: ");
                playerName = Console.ReadLine();
            } while (!nameIntegrity); // TODO: Check for input integrity

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
            // Try to convert both height and width inputs to int
            bool firstParse = int.TryParse(i_HeightInput, out o_BoardHeight);
            bool secondParse = int.TryParse(i_WidthInput, out o_BoardWidth);

            // Check if both parses were successful, and act accordingly   
            if(firstParse && secondParse)
            {
                if(o_BoardHeight >= k_MinBoardHeightAndWidth && o_BoardHeight <= k_MaxBoardHeightAndWidth && o_BoardWidth >= k_MinBoardHeightAndWidth && o_BoardWidth <= k_MaxBoardHeightAndWidth)
                {
                    // Check if even amount of cells in board
                    if((o_BoardHeight * o_BoardWidth) % 2 == 0)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

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

                for (int j = 0; j <= i_Board.BoardWidth; j++)
                {
                    Console.Write(" |  ");
                }

                Console.Write(Environment.NewLine);
                Console.Write("  ");
                for (int j = 0; j < i_Board.BoardWidth; j++)
                {
                    Console.Write("====");
                }

                Console.Write(Environment.NewLine);
            }

        }
    }
}
