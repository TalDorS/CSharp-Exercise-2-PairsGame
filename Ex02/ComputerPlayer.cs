using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class ComputerPlayer
    {
        private const int TwoSeconds = 2000;
        private int m_Points;

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        private MatrixCell makeSingleTurn(Board io_Board, out string io_KeyPressed)
        {
            Random rnd = new Random(); // Consider moving this outside the method if the method is called frequently
            char randomColumnForComputerPlayer;
            char randomRowForComputerPlayer;
            MatrixCell cellValue = new MatrixCell(); // DELETE

            do  // To ensure we dont choose a visible cell
            {
                randomColumnForComputerPlayer = (char)(IO.k_FirstColoumnLetter + rnd.Next(0, io_Board.BoardWidth));
                randomRowForComputerPlayer = (char)(IO.k_ZeroDigit + rnd.Next(1, io_Board.BoardHeight));
                io_KeyPressed = $"{randomColumnForComputerPlayer}{randomRowForComputerPlayer}";
                // cellValue = io_Board.SetCellToVisibleOnBoardAndGetCellValue(io_KeyPressed);

            } while (cellValue.IsVisible); // Keep randomizing until the chosen cell is invisible

            IO.ClearScreen(); // Clear the screen before getting the cell
            IO.PrintBoard(io_Board);

            return cellValue;
        }

        public bool MakeTurn(Board io_Board)
        {
            char? firstChoiceCellValue = makeSingleTurn(io_Board, out string KeyPressed1).Char; // First choice for the human player
            char? secondChoiceCellValue;
            string KeyPressed2;
            bool didSucceedTurn = false; // To check if player wins round

            do
            {
                secondChoiceCellValue = makeSingleTurn(io_Board, out KeyPressed2).Char; // Second choice for the human player
            } while (KeyPressed2 == KeyPressed1); // Check if second choice is equal to first choice, if so, repeat second move

            if (firstChoiceCellValue != secondChoiceCellValue)
            {
                System.Threading.Thread.Sleep(TwoSeconds);
                io_Board.SetCellToInvisibleOnBoard(KeyPressed1);
                io_Board.SetCellToInvisibleOnBoard(KeyPressed2);
                IO.ClearScreen();
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