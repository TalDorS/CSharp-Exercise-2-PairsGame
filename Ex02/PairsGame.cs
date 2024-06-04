using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class PairsGame
    {
        private string k_FirstPlayerMessage = "first";
        private string k_SecondPlayerMessage = "second";
        private HumanPlayer m_FirstPlayer;
        private HumanPlayer m_SecondPlayer;
        private ComputerPlayer m_ComputerPlayer;
        private Board m_GameBoard;
        private eGameMode m_GameMode;

        public enum eGameMode
        {
            PlayerVsPlayer,
            PlayerVsComputer
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
                IO.PrintWinnerAndScores(m_FirstPlayer, m_SecondPlayer, m_ComputerPlayer, m_GameMode);

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
                    // add here a print maybe to announce its player 1 turn
                    isChangedPlayer = m_FirstPlayer.MakeTurn(m_GameBoard);
                    if (!isChangedPlayer)
                    {
                        isFirstPlayerTurn = false;
                    }
                }
                else
                {
                    // add here a print maybe to announce its player 2 turn
                    if (m_GameMode == eGameMode.PlayerVsPlayer)
                    {
                        isChangedPlayer = m_SecondPlayer.MakeTurn(m_GameBoard);
                        if (!isChangedPlayer)
                        {
                            isFirstPlayerTurn = true;
                        }
                    }
                    else
                    {
                        isChangedPlayer = m_ComputerPlayer.MakeTurn(m_GameBoard);
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
        public void initializeGameBoard()
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
            string secondPlayerName = null;

            // Ask the first player for his name
            firstPlayerName = IO.GetPlayerName(k_FirstPlayerMessage);
            m_FirstPlayer = new HumanPlayer(firstPlayerName);

            // Get game mode from the computer (1: Player vs player, 2: Player vs computer)
            m_GameMode = IO.GetGameMode();

            // If the game mode is pvp, get the name of the second player
            if (m_GameMode == eGameMode.PlayerVsPlayer)
            {
                secondPlayerName = IO.GetPlayerName(k_SecondPlayerMessage);
                m_SecondPlayer = new HumanPlayer(secondPlayerName);
            }
            else
            {
                m_ComputerPlayer = new ComputerPlayer();
            }
        }

        // This function resets players' points
        private void resetPlayersPoints()
        {
            m_FirstPlayer.Points = 0;
            if(m_GameMode == eGameMode.PlayerVsPlayer) 
            {
                m_SecondPlayer.Points = 0;
            }
            else
            {
                m_ComputerPlayer.Points = 0;
            }
        }
    }
}