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

        public Board(int i_BoardHeight, int i_BoardWidth)
        {
            m_BoardHeight = i_BoardHeight;
            m_BoardWidth = i_BoardWidth;
            m_Board = new MatrixCell[i_BoardHeight, i_BoardWidth];
            initializeBoardCells();
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

        private void initializeBoardCells()
        {
            Random rnd = new Random();
            char[] allowedChars = { 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            bool[,] filledCells = new bool[m_BoardHeight, m_BoardWidth];

            m_NumOfPairs = (m_BoardHeight * m_BoardWidth) / 2;

            for (int i = 0; i < m_NumOfPairs; i++)
            {
                int[] rowsToPlaceCharAt = new int[2];
                int[] colsToPlaceCharAt = new int[2];

                for (int j = 0; j < 2; j++)
                {
                    int row, col;

                    do
                    {
                        row = rnd.Next(0, m_BoardHeight);
                        col = rnd.Next(0, m_BoardWidth);
                    } while (filledCells[row, col]);

                    rowsToPlaceCharAt[j] = row;
                    colsToPlaceCharAt[j] = col;
                    filledCells[row, col] = true;
                }

                m_Board[rowsToPlaceCharAt[0], colsToPlaceCharAt[0]].Char = allowedChars[i];
                m_Board[rowsToPlaceCharAt[1], colsToPlaceCharAt[1]].Char = allowedChars[i];
            }
        }

        public bool CheckCellVisibility(string i_Cell)
        {
            int cellColoum = i_Cell[0] - IO.k_FirstColoumnLetter;
            int cellRow = i_Cell[1] - IO.k_ZeroDigit - 1;

            return m_Board[cellRow, cellColoum].IsVisible;
        }

        public static bool CheckIfBoardHasEvenNumberOfCells(int io_BoardHeight, int io_BoardWidth)
        {
            bool isEvenNumberOfCells = false;

            if ((io_BoardHeight * io_BoardWidth) % 2 == 0)
            {
                isEvenNumberOfCells = true;
            }

            return isEvenNumberOfCells;
        }

        public void ChangeCellVisibilityByString(string i_KeyPressed, bool i_IsVisible)
        {
            int row = i_KeyPressed[1] - IO.k_FirstRowDigit;
            int col = i_KeyPressed[0] - IO.k_FirstColoumnLetter;

            BoardMatrix[row, col].IsVisible = i_IsVisible;
        }

        public MatrixCell GetCellWithString(string i_KeyPressed)
        {
            int row = i_KeyPressed[1] - IO.k_FirstRowDigit;
            int col = i_KeyPressed[0] - IO.k_FirstColoumnLetter;

            return this.BoardMatrix[row, col];
        }
    }
}