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
        private const char k_ConvertCharLetterToNumber = 'A';
        private const char k_ConvertCharIntegerToNumber = '0';
        private int m_Points;

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        public MatrixCell MakeSingleTurn(Board io_Board, out string KeyPressed)
        {
            Random rnd = new Random(); // Consider moving this outside the method if the method is called frequently
            char randomColumnForComputerPlayer;
            char randomRowForComputerPlayer;
            MatrixCell cellValue;

            do  // To ensure we dont choose a visible cell
            {
                randomColumnForComputerPlayer = (char)(k_ConvertCharLetterToNumber + rnd.Next(0, io_Board.BoardWidth));
                randomRowForComputerPlayer = (char)(k_ConvertCharIntegerToNumber + rnd.Next(1, io_Board.BoardHeight));
                KeyPressed = $"{randomColumnForComputerPlayer}{randomRowForComputerPlayer}";
                cellValue = io_Board.SetCellToVisibleOnBoardAndGetCellValue(KeyPressed);

            } while (cellValue.IsVisible); // Keep randomizing until the chosen cell is invisible

            IO.PrintBoard(io_Board);

            return cellValue;
        }

        public bool MakeTurn(Board io_Board)
        {
            char? firstChoiceCellValue = MakeSingleTurn(io_Board, out string KeyPressed1).Char; // First choice for the human player
            char? secondChoiceCellValue;
            string KeyPressed2;
            bool didSucceedTurn = false; // To check if player wins round
            do
            {
                secondChoiceCellValue = MakeSingleTurn(io_Board, out KeyPressed2).Char; // Second choice for the human player
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