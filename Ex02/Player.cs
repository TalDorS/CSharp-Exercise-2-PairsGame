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
        private Dictionary<char, List<string>> m_UnmatchedCells = new Dictionary<char, List<string>>();     //a dictonary to monitor the cells the computer chose before and save their data for future moves

        public enum ePlayerType
        {
            HumanPlayer,
            ComputerPlayer
        }

        public ePlayerType PlayerType
        {
            get { return m_PlayerType; }
            set { m_PlayerType = value; }
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
            bool cellFound = false;
            o_KeyPressed = null;

            if (m_UnmatchedCells.Count > 0)
            {
                foreach (var previousCellPosition in m_UnmatchedCells)
                {
                    if (previousCellPosition.Value.Count > 1)
                    {
                        foreach (var position in previousCellPosition.Value)
                        {
                            int cellColoum = position[0] - IO.k_FirstColoumnLetter; // Get the cell column
                            int cellRow = position[1] - IO.k_ZeroDigit - 1; // Get the cell row

                            if (!io_Board.BoardMatrix[cellRow, cellColoum].IsVisible)
                            {
                                o_KeyPressed = position;
                                cellValue = io_Board.BoardMatrix[cellRow, cellColoum];
                                io_Board.BoardMatrix[cellRow, cellColoum].IsVisible = true;
                                cellValue.IsVisible = true;
                                cellFound = true;
                                break;
                            }
                        }
                    }
                    if (cellFound)
                    {
                        break;
                    }
                }
            }

            if (!cellFound)
            {
                do
                {
                    randomColumnForComputerPlayer = (char)(IO.k_FirstColoumnLetter + rnd.Next(0, io_Board.BoardWidth));
                    randomRowForComputerPlayer = (char)(IO.k_ZeroDigit + rnd.Next(1, io_Board.BoardHeight));
                    o_KeyPressed = $"{randomColumnForComputerPlayer}{randomRowForComputerPlayer}";                         // For comparing if computer got the same cell
                    randomColumnForComputerPlayer = (char)(randomColumnForComputerPlayer - IO.k_FirstColoumnLetter);
                    randomRowForComputerPlayer = (char)(randomRowForComputerPlayer - 1 - IO.k_ZeroDigit);
                    cellValue = io_Board.BoardMatrix[(int)randomRowForComputerPlayer, (int)(randomColumnForComputerPlayer)];
                } while (cellValue.IsVisible);  // To ensure we dont choose a visible cell

                io_Board.BoardMatrix[(int)(randomRowForComputerPlayer), (int)(randomColumnForComputerPlayer)].IsVisible = true;
                cellValue.IsVisible = true;
            }

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
            IO.PrintComputersTurnMessage();
            bool didSucceedTurn = false; // To check if player wins round
            MatrixCell firstChoiceCellValue = makeSingleTurnForComputerPlayer(io_Board, out string keyPressed1); // First choice for the human player

            if (!m_UnmatchedCells.ContainsKey((char)firstChoiceCellValue.Char))
            {
                m_UnmatchedCells[(char)firstChoiceCellValue.Char] = new List<string>();
            }

            if (!m_UnmatchedCells[(char)firstChoiceCellValue.Char].Contains(keyPressed1))
            {
                m_UnmatchedCells[(char)firstChoiceCellValue.Char].Add(keyPressed1);
            }

            MatrixCell secondChoiceCellValue = makeSingleTurnForComputerPlayer(io_Board, out string keyPressed2);

            if (!m_UnmatchedCells.ContainsKey((char)secondChoiceCellValue.Char))
            {
                m_UnmatchedCells[(char)secondChoiceCellValue.Char] = new List<string>();
            }

            if (!m_UnmatchedCells[(char)secondChoiceCellValue.Char].Contains(keyPressed2))
            {
                m_UnmatchedCells[(char)secondChoiceCellValue.Char].Add(keyPressed2);
            }

            if (firstChoiceCellValue.Char != secondChoiceCellValue.Char)
            {
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

                if (m_UnmatchedCells.ContainsKey((char)firstChoiceCellValue.Char))
                {
                    m_UnmatchedCells[(char)firstChoiceCellValue.Char].Remove(keyPressed1);

                    if (m_UnmatchedCells[(char)firstChoiceCellValue.Char].Count == 0)
                    {
                        m_UnmatchedCells.Remove((char)firstChoiceCellValue.Char);
                    }
                }
                if (m_UnmatchedCells.ContainsKey((char)secondChoiceCellValue.Char))
                {
                    m_UnmatchedCells[(char)secondChoiceCellValue.Char].Remove(keyPressed2);

                    if (m_UnmatchedCells[(char)secondChoiceCellValue.Char].Count == 0)
                    {
                        m_UnmatchedCells.Remove((char)secondChoiceCellValue.Char);
                    }
                }
            }

            removeVisibleCellsFromUnmatchedCells(io_Board);     //update the m_UnmatchedCells list if chars are found by component
            return didSucceedTurn;
        }
        private void removeVisibleCellsFromUnmatchedCells(Board i_Board)
        {
            var keysToRemove = new List<char>();

            foreach (var previousCellPosition in m_UnmatchedCells)
            {
                // Create a list to store positions that need to be removed
                var positionsToRemove = new List<string>();

                // Iterate through positions in kvp.Value and check if they are visible
                foreach (string position in previousCellPosition.Value)
                {
                    int cellColoum = position[0] - IO.k_FirstColoumnLetter;
                    int cellRow = position[1] - IO.k_ZeroDigit - 1;
                    if (i_Board.BoardMatrix[cellRow, cellColoum].IsVisible)
                    {
                        positionsToRemove.Add(position);
                    }
                }

                // Remove positions that are visible
                foreach (string positionToRemove in positionsToRemove)
                {
                    previousCellPosition.Value.Remove(positionToRemove);
                }

                // If all positions for the key are removed, add the key to keysToRemove list
                if (previousCellPosition.Value.Count == 0)
                {
                    keysToRemove.Add(previousCellPosition.Key);
                }
            }

            // Remove keys that have no positions remaining
            foreach (char key in keysToRemove)
            {
                m_UnmatchedCells.Remove(key);
            }

        }
    }
}
