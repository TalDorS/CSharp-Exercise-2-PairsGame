using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Board
    {
        private MatrixCell[,] m_Board;
        private int m_BoardHeight;
        private int m_BoardWidth;

        // Board CTOR 
        public Board(int i_BoardHeight, int i_BoardWidth)
        {
            // Assign height and width
            m_BoardHeight = i_BoardHeight;
            m_BoardWidth = i_BoardWidth;

            // Initialize board cells
            m_Board = new MatrixCell[i_BoardHeight, i_BoardWidth];
            initializeBoard();
        }

        // Properties of matrix cell
        public MatrixCell[,] BoardMatrix
        {
            get { return m_Board; }
        }

        // Get board height
        public int BoardHeight
        {
            get { return m_BoardHeight; }
        }

        // Get board width
        public int BoardWidth
        {
            get { return m_BoardWidth; }
        }

        // This board initializes the board cells
        private void initializeBoard()
        {
            Random rnd = new Random();
            char[] allowedChars = { 'I', 'J', 'K', 'L', 'M', 'N' , 'O' , 'P' , 'Q' , 'R' , 'S' , 'T' , 'U' , 'V' , 'W' , 'X' , 'Y' , 'Z' };
            int amountOfCharsUsed = (m_BoardHeight * m_BoardWidth) / 2;
            int[] rowsToPlaceCharAt = new int[2];
            int[] colsToPlaceCharAt = new int[2];

            // Iterate through the amount of words we can use, and randomly find a cell for them
            for (int i = 0; i < amountOfCharsUsed; i++)
            {
                do
                {
                    // Generate two random cells to place the chars at
                    rowsToPlaceCharAt[0] = rnd.Next(0, m_BoardHeight);
                    rowsToPlaceCharAt[1] = rnd.Next(0, m_BoardHeight);
                    colsToPlaceCharAt[0] = rnd.Next(0, m_BoardWidth);
                    colsToPlaceCharAt[1] = rnd.Next(0, m_BoardWidth);
                } while (m_Board[rowsToPlaceCharAt[0], colsToPlaceCharAt[0]].Char != null || m_Board[rowsToPlaceCharAt[1], colsToPlaceCharAt[1]].Char != null);
               
                m_Board[rowsToPlaceCharAt[0], colsToPlaceCharAt[0]].Char = allowedChars[i];
                m_Board[rowsToPlaceCharAt[1], colsToPlaceCharAt[1]].Char = allowedChars[i];
            }
        }
    }
}
