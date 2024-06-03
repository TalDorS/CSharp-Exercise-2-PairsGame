using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class HumanPlayer
    {
        private const int TwoSeconds = 2000;
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

        private MatrixCell makeSingleTurn(Board io_Board, out string io_KeyPressed)
        {
            MatrixCell cellValue;
            do  // To ensure we dont choose a visible cell
            {
                io_KeyPressed = IO.GetCell(io_Board.BoardHeight, io_Board.BoardWidth);
                cellValue = io_Board.SetCellToVisibleOnBoardAndGetCellValue(io_KeyPressed);

                if (cellValue.IsVisible)
                {
                    Console.WriteLine("You chose a visible cell. Choose a different cell.");
                }

            } while (cellValue.IsVisible);

            Ex02.ConsoleUtils.Screen.Clear();// Clear the screen before getting the cell
            IO.PrintBoard(io_Board);

            return cellValue;
        }
        public bool MakeTurn(Board io_Board)
        {
            char? firstChoiceCellValue = makeSingleTurn(io_Board,out string KeyPressed1).Char; // First choice for the human player
            char? secondChoiceCellValue;
            string KeyPressed2;
            bool didSucceedTurn = false; // To check if player wins round

            do
            {
                secondChoiceCellValue = makeSingleTurn(io_Board, out KeyPressed2).Char; // Second choice for the human player
                if (KeyPressed2 == KeyPressed1)
                {
                    Console.WriteLine("You chose the same cell again. Choose a different cell.");
                }
            } while (KeyPressed2 == KeyPressed1); // Check if second choice is equal to first choice, if so, repeat second move

            if (firstChoiceCellValue != secondChoiceCellValue)
            {
                System.Threading.Thread.Sleep(TwoSeconds);      
                io_Board.SetCellToInvisibleOnBoard(KeyPressed1);
                io_Board.SetCellToInvisibleOnBoard(KeyPressed2);
                Ex02.ConsoleUtils.Screen.Clear();
                IO.PrintBoard(io_Board);
            }
            else
            {
                m_Points++;
                io_Board.NumOfPairs--;          // The number of pairs 
                didSucceedTurn = true;// We want the player to play again in the next round
            }

            return didSucceedTurn;
        }
    }
}
