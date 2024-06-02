using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class PairsGame
    {
        private string m_FirstPlayerName;
        private string m_SecondPlayerName = null;
        private eGameMode m_GameMode;

        public enum eGameMode
        {
            PlayerVsPlayer,
            PlayerVsComputer
        }

        // First game running function
        public void RunGame()
        {
            int boardHeight;
            int boardWidth;
            
            // Ask the first player for his name
            m_FirstPlayerName = IO.GetPlayerName();

            // Get game mode from the computer (1: Player vs player, 2: Player vs computer)
            m_GameMode = IO.GetGameMode();

            // If the game mode is pvp, get the name of the second player
            m_SecondPlayerName = IO.GetPlayerName();

            // Get height and width of board
            IO.GetBoardHeightAndWidth(out boardHeight, out boardWidth);

            //for checking print board function:
            // Create new game board
            //Board board= new Board(boardHeight, boardWidth); ;
            // Print game board
            //IO.PrintBoard(board);

        }
    }
}
