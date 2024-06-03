using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class HumanPlayer
    {
        private const char k_FirstColoumnLetter = 'A';
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

        public char? makeSingleTurn(Board io_Board, out string KeyPressed)
        {
            KeyPressed = IO.GetCell(io_Board.BoardHeight, io_Board.BoardWidth);
            char? cellValue= setCellToVisibleOnBoardAndGetCellValue(KeyPressed, io_Board);
            IO.PrintBoard(io_Board);
            return cellValue;
        }
        public char? setCellToVisibleOnBoardAndGetCellValue(string i_KeyPressed, Board io_Board)
        {
            int cellColoum = i_KeyPressed[0]- k_FirstColoumnLetter;           // Get the cell coloum
            int cellRow=i_KeyPressed[1];                                     // Get the cell row
            char? CellValue=io_Board.BoardMatrix[cellRow,cellColoum].Char;  //  Save the char of the cell
            io_Board.BoardMatrix[cellRow, cellColoum].IsVisible = true;     //  update the cell to be exposed

            return CellValue;
        }
        public void setCellToInvisibleOnBoard(string i_KeyPressed, Board io_Board)
        {
            int cellColoum = i_KeyPressed[0] - k_FirstColoumnLetter;           // Get the cell coloum
            int cellRow = i_KeyPressed[1];                                     // Get the cell row
            io_Board.BoardMatrix[cellRow, cellColoum].IsVisible = false;
        }
        
        public void makeTurn(Board io_Board)
        {
            char? firstChoiceCellValue =makeSingleTurn(io_Board,out string KeyPressed1); // First choice for the human player
            char? secondChoiceCellValue=makeSingleTurn(io_Board, out string KeyPressed2); // Second choice for the human player
            bool didSucceedTurn = false; // To check if player wins round
            if(firstChoiceCellValue != secondChoiceCellValue)
            {
                // i need the cells to be seen for two secnds and then update board accordingly
                setCellToInvisibleOnBoard(KeyPressed1, io_Board);
                setCellToInvisibleOnBoard(KeyPressed2,io_Board);
                Ex02.ConsoleUtils.Screen.Clear();
                IO.PrintBoard(io_Board);
            }
            else
            {
                m_Points++;
                io_Board.NumOfPairs--;          // The number of pairs 
                didSucceedTurn = true;// We want the player to play again in the next round
            }
        }
    }
}
