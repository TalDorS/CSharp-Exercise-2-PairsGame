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
            // Initialize board, game mode and board
            InitializeGameComponents();

            // Initiate main game loop
            StartGameLoop();
        }

        // This function has the main game loop
        public void StartGameLoop()
        {
            while (true)
            {

            }
        }

        // This function initializes the players, game mode and the board
        public void InitializeGameComponents()
        {
            int boardHeight;
            int boardWidth;
            string firstPlayerName;
            string secondPlayerName = null;

            // Ask the first player for his name
            firstPlayerName = IO.GetPlayerName();
            m_FirstPlayer = new HumanPlayer(firstPlayerName);

            // Get game mode from the computer (1: Player vs player, 2: Player vs computer)
            m_GameMode = IO.GetGameMode();

            // If the game mode is pvp, get the name of the second player
            secondPlayerName = IO.GetSecondPlayerName(m_GameMode);
            if (secondPlayerName != null)
            {
                m_SecondPlayer = new HumanPlayer(secondPlayerName);
            }
            else
            {
                m_ComputerPlayer = new ComputerPlayer();
            }

            // Get height and width of board
            IO.GetBoardHeightAndWidth(out boardHeight, out boardWidth);

            // Create new game board
            m_GameBoard = new Board(boardHeight, boardWidth);
        }
    }
}