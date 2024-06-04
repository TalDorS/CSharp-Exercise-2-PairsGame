using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex02.Player;

namespace Ex02
{
    public class PairsGame
    {
        private const string k_FirstPlayerMessage = "first";
        private const string k_SecondPlayerMessage = "second";
        private const string k_ComputerPlayerName = "Computer Player";
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Board m_GameBoard;
        private eGameMode m_GameMode;

        public enum eGameMode
        {
            PlayerVsPlayer,
            PlayerVsComputer,
        }

        // First game running function
        public void RunGame()
        {
            bool isQuit = false;

            // Initialize players and mode
            initializePlayersAndMode();

            while (!isQuit)
            {
                // Initialize board
                initializeGameBoard();

                // Initiate main game loop
                startGameLoop();

                // Print winner and final score
                IO.PrintWinnerAndScores(m_FirstPlayer, m_SecondPlayer);

                // Ask the player if he wants another round
                isQuit = IO.AskPlayerForAnotherRound();

                // Reset player's points
                if (!isQuit)
                {
                    resetPlayersPoints();
                    IO.ClearScreen();
                }
            }
        }

        // This function has the main game loop
        private void startGameLoop()
        {
            bool isChangedPlayer = false;
            bool isFirstPlayerTurn = true;

            while (m_GameBoard.NumOfPairs != 0)
            {
                // Print board at the start of the turn
                IO.PrintBoard(m_GameBoard);

                if (isFirstPlayerTurn)
                {
                    isChangedPlayer = m_FirstPlayer.MakeHumanTurn(m_GameBoard);
                    if (!isChangedPlayer)
                    {
                        isFirstPlayerTurn = false;
                    }
                }
                else
                {
                    if (m_SecondPlayer.PlayerType==Player.ePlayerType.HumanPlayer)// If the player is human
                    {
                        isChangedPlayer = m_SecondPlayer.MakeHumanTurn(m_GameBoard);
                        if (!isChangedPlayer)
                        {
                            isFirstPlayerTurn = true;
                        }
                    }
                    else// If player is computer
                    {
                        isChangedPlayer = m_SecondPlayer.MakeComputerTurn(m_GameBoard);
                        if (!isChangedPlayer)
                        {
                            isFirstPlayerTurn = true;
                        }
                    }
                }
                
                // Clear screen at the end of this loop
                IO.ClearScreen();
            }
        }

        // This function initializes the players, game mode and the board
        private void initializeGameBoard()
        {
            int boardHeight;
            int boardWidth;
            
            // Get height and width of board
            IO.GetBoardHeightAndWidth(out boardHeight, out boardWidth);

            // Create new game board
            m_GameBoard = new Board(boardHeight, boardWidth);

            // Clear the screen
            IO.ClearScreen();
        }

        // This function initializes the players in the game
        private void initializePlayersAndMode()
        {
            string firstPlayerName;
            string secondPlayerName;

            // Ask the first player for his name
            firstPlayerName = IO.GetPlayerName(k_FirstPlayerMessage);
            m_FirstPlayer = new Player(firstPlayerName,Player.ePlayerType.HumanPlayer);

            // Get game mode from the computer (1: Player vs player, 2: Player vs computer)
            m_GameMode = IO.GetGameMode();

            // If the game mode is pvp, get the name of the second player
            if (m_GameMode == eGameMode.PlayerVsPlayer)
            {
                secondPlayerName = IO.GetPlayerName(k_SecondPlayerMessage);
                m_SecondPlayer = new Player(secondPlayerName, Player.ePlayerType.HumanPlayer);
                //set the player mode for player two to be human
            }
            else
            {
                m_SecondPlayer = new Player(k_ComputerPlayerName, Player.ePlayerType.ComputerPlayer);
            }
        }

        // This function resets players' points
        private void resetPlayersPoints()
        {
            m_FirstPlayer.Points = 0;
            m_SecondPlayer.Points = 0; 
        }
    }
}