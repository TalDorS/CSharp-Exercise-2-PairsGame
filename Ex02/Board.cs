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
        private int m_NumOfPairs;
        private const char k_ALetter = 'A';
        private const char k_ZeroLetter = '0';
        private const int k_DividedByTwo = 2;

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
        public int NumOfPairs 
        {  
            get 
            { 
                return m_NumOfPairs;
            } 
            set 
            {
                m_NumOfPairs = value;
            } 
        }
          
        public MatrixCell[,] BoardMatrix
        {
            get { return m_Board; }
        }
        
        public int BoardHeight
        {
            get { return m_BoardHeight; }
        }

        public int BoardWidth
        {
            get { return m_BoardWidth; }
        }

        // This board initializes the board cells
        private void initializeBoard()
        {
            Random rnd = new Random();
            char[] allowedChars = { 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            m_NumOfPairs = (m_BoardHeight * m_BoardWidth) / 2;
            int[] rowsToPlaceCharAt = new int[2];
            int[] colsToPlaceCharAt = new int[2];

            // Iterate through the amount of words we can use, and randomly find a cell for them
            for (int i = 0; i < m_NumOfPairs; i++)
            {
                do
                {
                    // Generate two random cells to place the chars at
                    rowsToPlaceCharAt[0] = rnd.Next(0, m_BoardHeight);
                    rowsToPlaceCharAt[1] = rnd.Next(0, m_BoardHeight);
                    colsToPlaceCharAt[0] = rnd.Next(0, m_BoardWidth);
                    colsToPlaceCharAt[1] = rnd.Next(0, m_BoardWidth);
                } while (!validateCells(rowsToPlaceCharAt, colsToPlaceCharAt));

                m_Board[rowsToPlaceCharAt[0], colsToPlaceCharAt[0]].Char = allowedChars[i];
                m_Board[rowsToPlaceCharAt[1], colsToPlaceCharAt[1]].Char = allowedChars[i];
            }
        }


        // This function checks if a string of cell is visible
        public bool CheckCellVisibility(string i_Cell)
        {
            int cellColoum = i_Cell[0] - k_ALetter;
            int cellRow = i_Cell[1] - k_ZeroLetter - 1;

            return m_Board[cellRow, cellColoum].IsVisible;
        }

        // This function is a utility function of 'initializeBoard', we validate the two chosen cells
        private bool validateCells(int[] rowsToPlaceCharAt, int[] colsToPlaceCharAt)
        {
            bool isValid = false;
            int firstRowIndex = rowsToPlaceCharAt[0];
            int secondRowIndex = rowsToPlaceCharAt[1];
            int firstColIndex = colsToPlaceCharAt[0];
            int secondColIndex = colsToPlaceCharAt[1];
            char? firstChar = m_Board[firstRowIndex, firstColIndex].Char;
            char? secondChar = m_Board[secondRowIndex, secondColIndex].Char;

            if (firstChar == null && secondChar == null && (firstRowIndex != secondRowIndex || firstColIndex != secondColIndex))
            {
                isValid = true;
            }

            return isValid;
        }
        public static bool BoardHasEvenNumberOfCells(int io_BoardHeight, int io_BoardWidth)
        {
            bool isEvenNumberOfCells = false;
            if ((io_BoardHeight * io_BoardWidth) % k_DividedByTwo == 0)
            {
                isEvenNumberOfCells = true;
            }

            return isEvenNumberOfCells;
        }
    }
}