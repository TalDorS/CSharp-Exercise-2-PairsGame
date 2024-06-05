using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class Player
    {
        private const int k_NumOfTurns = 2;
        private string m_Name;
        private int m_Points;
        private ePlayerType m_PlayerType;

        public enum ePlayerType
        {
            HumanPlayer,
            ComputerPlayer,
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
            set { m_PlayerType = value;}
        }

        public Player(string i_Name, ePlayerType i_PlayerType)
        {
            m_Name = i_Name;
            m_PlayerType = i_PlayerType; 
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

        private int[] getCellAndTurnItVisible(Board io_Board, out bool o_IsQPressed)
        {
            int[] cellCoordinates = new int[k_NumOfTurns];
            string cellString;
            o_IsQPressed = false;
            cellString = IO.GetCellFromPlayer(this, io_Board);

            if (cellString.ToUpper() == IO.k_ExitGame)
            {
                o_IsQPressed = true;
            }
            else
            {
                cellCoordinates[0] = cellString[1] - IO.k_ZeroDigit - 1;
                cellCoordinates[1] = cellString[0] - IO.k_FirstColoumnLetter;
                io_Board.BoardMatrix[cellCoordinates[0], cellCoordinates[1]].IsVisible = true;
            }

            return cellCoordinates;
        }

        public bool MakeHumanTurn(Board io_Board, out bool o_IsQPressed)
        {
            int[] firstCellCoordinates;
            int[] secondCellCoordinates;
            bool didSucceedTurn = false; // To check if player wins round

            // Make first cell choice
            firstCellCoordinates = getCellAndTurnItVisible(io_Board, out o_IsQPressed); // First choice for the human player

            if (!o_IsQPressed)
            {
                // Clear board
                IO.ClearScreen();
                IO.PrintBoard(io_Board);

                // Make second cell choice
                secondCellCoordinates = getCellAndTurnItVisible(io_Board, out o_IsQPressed); // Second choice for the human player

                // Clear board
                IO.ClearScreen();
                IO.PrintBoard(io_Board);

                // Compare both choices' chars
                if (io_Board.BoardMatrix[firstCellCoordinates[0], firstCellCoordinates[1]].Char != io_Board.BoardMatrix[secondCellCoordinates[0], secondCellCoordinates[1]].Char)
                {
                    // Sleep for two seconds
                    IO.Sleep2Seconds();

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
            }

            return didSucceedTurn;
        }

        private MatrixCell makeSingleTurnForComputerPlayer(Board io_Board, out string o_KeyPressed)
        {
            Random rnd = new Random();
            MatrixCell cellValue = new MatrixCell();
            char randomColumnForComputerPlayer;
            char randomRowForComputerPlayer;

            do
            {
                randomColumnForComputerPlayer = (char)(IO.k_FirstColoumnLetter + rnd.Next(0, io_Board.BoardWidth));
                randomRowForComputerPlayer = (char)(IO.k_ZeroDigit + rnd.Next(1, io_Board.BoardHeight));
                o_KeyPressed = $"{randomColumnForComputerPlayer}{randomRowForComputerPlayer}";                         // For comparing if computer got the same cell
                cellValue = io_Board.BoardMatrix[(int)(randomRowForComputerPlayer - 1 - IO.k_ZeroDigit), (int)(randomColumnForComputerPlayer - IO.k_FirstColoumnLetter)];
            } while (cellValue.IsVisible);  // To ensure we dont choose a visible cell

            io_Board.BoardMatrix[(int)(randomRowForComputerPlayer - 1 - IO.k_ZeroDigit), (int)(randomColumnForComputerPlayer - IO.k_FirstColoumnLetter)].IsVisible = true;
            cellValue.IsVisible = true;
            IO.ClearScreen();
            IO.PrintBoard(io_Board);

            return cellValue;
        }

        private void setCellToInvisibleOnBoard(Board io_Board, string i_KeyPressed)
        {
            int cellColoum = i_KeyPressed[0] - IO.k_FirstColoumnLetter;                                // Get the cell coloum
            int cellRow = i_KeyPressed[1] - IO.k_ZeroDigit - 1;                                     // Get the cell row (its the line -1 in the matrix)
            io_Board.BoardMatrix[cellRow, cellColoum].IsVisible = false;
        }

        public bool MakeComputerTurn(Board io_Board)
        {
            MatrixCell firstChoiceCellValue = makeSingleTurnForComputerPlayer(io_Board, out string keyPressed1); // First choice for the human player
            MatrixCell secondChoiceCellValue = makeSingleTurnForComputerPlayer(io_Board, out string keyPressed2);
            bool didSucceedTurn = false; // To check if player wins round

            if (firstChoiceCellValue.Char != secondChoiceCellValue.Char)
            {
                IO.PrintComputersTurnMessage();
                IO.Sleep2Seconds();
                setCellToInvisibleOnBoard(io_Board, keyPressed1);
                setCellToInvisibleOnBoard(io_Board, keyPressed2);
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
