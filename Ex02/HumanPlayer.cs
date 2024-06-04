using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class HumanPlayer
    {
        private const int k_TwoSeconds = 2000;
        private string m_Name;
        private int m_Points;

        public HumanPlayer(string i_Name)
        {
            m_Name = i_Name;
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        } 

        private int[] getCellAndTurnItVisible(Board io_Board)
        {
            int[] cellCoordinates = new int[2];
            string cellString;

            cellString = IO.GetCellFromPlayer(this, io_Board);
            cellCoordinates[0] = cellString[1] - IO.k_ZeroDigit - 1;
            cellCoordinates[1] = cellString[0] - IO.k_FirstColoumnLetter;
            io_Board.BoardMatrix[cellCoordinates[0], cellCoordinates[1]].IsVisible = true;

            return cellCoordinates;
        }
        public bool MakeTurn(Board io_Board)
        {
            int[] firstCellCoordinates;
            int[] secondCellCoordinates;
            bool didSucceedTurn = false; // To check if player wins round

            // Make first cell choice
            firstCellCoordinates = getCellAndTurnItVisible(io_Board); // First choice for the human player

            // Clear board
            IO.ClearScreen();
            IO.PrintBoard(io_Board);

            // Make second cell choice
            secondCellCoordinates = getCellAndTurnItVisible(io_Board); // Second choice for the human player

            // Clear board
            IO.ClearScreen();
            IO.PrintBoard(io_Board);

            // Compare both choices' chars
            if (io_Board.BoardMatrix[firstCellCoordinates[0], firstCellCoordinates[1]].Char != io_Board.BoardMatrix[secondCellCoordinates[0], secondCellCoordinates[1]].Char)
            {
                // Sleep for two seconds
                System.Threading.Thread.Sleep(k_TwoSeconds);

                // Turn first and second choices to invisible
                io_Board.BoardMatrix[firstCellCoordinates[0], firstCellCoordinates[1]].IsVisible = false;
                io_Board.BoardMatrix[secondCellCoordinates[0], secondCellCoordinates[1]].IsVisible = false;
            }
            else
            {
                m_Points++;
                io_Board.NumOfPairs--; 
                didSucceedTurn = true;// We want the player to play again in the next round
            }

            return didSucceedTurn;
        }
    }
}
