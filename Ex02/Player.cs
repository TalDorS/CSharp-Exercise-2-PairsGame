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
        private Dictionary<string, MatrixCell> m_KnownCells = new Dictionary<string, MatrixCell>();

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

        public Dictionary<string, MatrixCell> KnownCells
        {
            get { return m_KnownCells; }
        }

        private string getCellAndTurnItVisible(Board io_Board, out bool o_IsQPressed)
        {
            string cellString = IO.GetCellFromPlayer(this, io_Board);
            o_IsQPressed = false;

            if (cellString.ToUpper() == IO.k_ExitGame)
            {
                o_IsQPressed = true;
            }
            else
            {
                io_Board.ChangeCellVisibilityByString(cellString, true);
            }

            return cellString;
        }

        public bool MakeHumanTurn(Board io_Board, out bool o_IsQPressed)
        {
            string firstCellString = getCellAndTurnItVisible(io_Board, out o_IsQPressed);
            string secondCellString = string.Empty;
            bool didSucceedTurn = false;

            if (!o_IsQPressed)
            {
                IO.ClearScreen();
                IO.PrintBoard(io_Board);
                secondCellString = getCellAndTurnItVisible(io_Board, out o_IsQPressed);
                IO.ClearScreen();
                IO.PrintBoard(io_Board);

                if (io_Board.GetCellWithString(firstCellString).Char != io_Board.GetCellWithString(secondCellString).Char)
                {
                    IO.Sleep2Seconds();
                    io_Board.ChangeCellVisibilityByString(firstCellString, false);
                    io_Board.ChangeCellVisibilityByString(secondCellString, false);
                }
                else
                {
                    m_Points++;
                    io_Board.NumOfPairs--;
                    didSucceedTurn = true;
                }
            }

            return didSucceedTurn;
        }

        public bool MakeComputerTurn(Board io_Board)
        {
            bool didSucceedTurn = false;
            string firstCell;
            string secondCell;
            MatrixCell firstChoiceCellValue;
            MatrixCell secondChoiceCellValue;

            bool isPairFound = lookForKnownPairs(io_Board, out firstCell, out secondCell);

            if (isPairFound)
            {
                firstChoiceCellValue = io_Board.GetCellWithString(firstCell);
                secondChoiceCellValue = io_Board.GetCellWithString(secondCell);
            }
            else
            {
                firstChoiceCellValue = makeRandomTurnForComputerPlayer(io_Board, out firstCell);
                secondChoiceCellValue = makeRandomTurnForComputerPlayer(io_Board, out secondCell);

                if (firstChoiceCellValue.Char == secondChoiceCellValue.Char)
                {
                    m_KnownCells.Remove(firstCell);
                    m_KnownCells.Remove(secondCell);
                }
            }

            IO.ClearScreen();
            IO.PrintBoard(io_Board);
            IO.PrintComputersTurnMessage();
            IO.Sleep2Seconds();

            if (firstChoiceCellValue.Char != secondChoiceCellValue.Char)
            {
                io_Board.ChangeCellVisibilityByString(firstCell, false);
                io_Board.ChangeCellVisibilityByString(secondCell, false);
                IO.ClearScreen();
                IO.PrintBoard(io_Board);
            }
            else
            {
                m_Points++;
                io_Board.NumOfPairs--;
                didSucceedTurn = true;
            }

            return didSucceedTurn;
        }

        private bool lookForKnownPairs(Board io_Board, out string o_FirstCell, out string o_SecondCell)
        {
            List<string> keysToRemove = new List<string>();
            bool isPairFound = false;
            o_FirstCell = string.Empty;
            o_SecondCell = string.Empty;

            foreach (var cell1 in m_KnownCells)
            {
                foreach (var cell2 in m_KnownCells)
                {
                    if (cell1.Key != cell2.Key && cell1.Value.Char == cell2.Value.Char && !cell1.Value.IsVisible && !cell2.Value.IsVisible && !isPairFound)
                    {
                        if (!io_Board.CheckCellVisibility(cell1.Key) || !io_Board.CheckCellVisibility(cell2.Key)) 
                        {
                            o_FirstCell = cell1.Key;
                            o_SecondCell = cell2.Key;
                            io_Board.ChangeCellVisibilityByString(o_FirstCell, true);
                            io_Board.ChangeCellVisibilityByString(o_SecondCell, true);
                            keysToRemove.Add(cell1.Key);
                            keysToRemove.Add(cell2.Key);
                            isPairFound = true;
                        }
                    }
                }
            }

            foreach (var key in keysToRemove)
            {
                m_KnownCells.Remove(key);
            }

            return isPairFound;
        }
        private MatrixCell makeRandomTurnForComputerPlayer(Board io_Board, out string o_KeyPressed)
        {
            char randomColumnForComputerPlayer;
            char randomRowForComputerPlayer;
            MatrixCell cellValue;
            Random rnd = new Random();

            do
            {
                randomColumnForComputerPlayer = (char)(IO.k_FirstColoumnLetter + rnd.Next(0, io_Board.BoardWidth));
                randomRowForComputerPlayer = (char)(IO.k_FirstRowDigit + rnd.Next(0, io_Board.BoardHeight));
                o_KeyPressed = String.Format("{0}{1}", randomColumnForComputerPlayer, randomRowForComputerPlayer);
                cellValue = io_Board.GetCellWithString(o_KeyPressed);
            } while (cellValue.IsVisible);

            io_Board.ChangeCellVisibilityByString(o_KeyPressed, true);
            m_KnownCells[o_KeyPressed] = cellValue;

            return cellValue;
        }
    }
}