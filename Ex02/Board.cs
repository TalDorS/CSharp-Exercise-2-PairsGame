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

        // Board CTOR 
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

        // This board initializes the board cells
        private void initializeBoardCells()
        {
            Random rnd = new Random();
            char[] allowedChars = { 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            m_NumOfPairs = (m_BoardHeight * m_BoardWidth) / 2;

            List<int> cellIndices = new List<int>();

            // Fill the list with all possible cell indices
            for (int i = 0; i < m_BoardHeight * m_BoardWidth; i++)
            {
                cellIndices.Add(i);
            }

            // Shuffle the list of cell indices
            cellIndices = cellIndices.OrderBy(x => rnd.Next()).ToList();

            // Place the characters
            for (int i = 0; i < m_NumOfPairs; i++)
            {
                int firstIndex = cellIndices[i * 2];
                int secondIndex = cellIndices[i * 2 + 1];

                int firstRow = firstIndex / m_BoardWidth;
                int firstCol = firstIndex % m_BoardWidth;
                int secondRow = secondIndex / m_BoardWidth;
                int secondCol = secondIndex % m_BoardWidth;

                m_Board[firstRow, firstCol].Char = allowedChars[i];
                m_Board[secondRow, secondCol].Char = allowedChars[i];
                m_Board[firstRow, firstCol].IsVisible = true;
                m_Board[secondRow, secondCol].IsVisible = true;
            }
        }

        // This function checks if a string of cell is visible
        public bool CheckCellVisibility(string i_Cell)
        {
            int cellColoum = i_Cell[0] - k_ALetter;
            int cellRow = i_Cell[1] - k_ZeroLetter - 1;

            return m_Board[cellRow, cellColoum].IsVisible;
        }

        public static bool checkIfBoardHasEvenNumberOfCells(int io_BoardHeight, int io_BoardWidth)
        {
            bool isEvenNumberOfCells = false;

            if ((io_BoardHeight * io_BoardWidth) % 2 == 0)
            {
                isEvenNumberOfCells = true;
            }

            return isEvenNumberOfCells;
        }
    }
}