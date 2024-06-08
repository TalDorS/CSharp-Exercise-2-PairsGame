using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ex02.Player;

namespace Ex02
{
    public class PairsGame
    {
        private const string k_FirstPlayerMessage = "first";
        private const string k_SecondPlayerMessage = "second";
        private const string k_ComputerPlayerName = "Computer Player";
        private Player m_FirstPlayer;
        private Player m_SecondPlayer;
        private Board m_GameBoard;
        private eGameMode m_GameMode;

        public enum eGameMode
        {
            PlayerVsPlayer,
            PlayerVsComputer
        }

        public void RunGame()
        {
            bool isQuit = false;
            bool isPressedQ = false;

            initializePlayersAndMode();

            while (!isQuit)
            {
                initializeGameBoard();
                startGameLoop(out isPressedQ);

                if(isPressedQ)
                {
                    break;
                }

                IO.PrintWinnerAndScores(m_FirstPlayer, m_SecondPlayer);
                isQuit = IO.AskPlayerForAnotherRound();

                if (!isQuit)
                {
                    m_FirstPlayer.Points = 0;
                    m_SecondPlayer.Points = 0;
                    m_SecondPlayer.KnownCells.Clear();
                    IO.ClearScreen();
                }
            }
        }

        private void startGameLoop(out bool o_IsQPressed)
        {
            bool isChangedPlayer = false;
            bool isFirstPlayerTurn = true;
            o_IsQPressed = false;

            while (m_GameBoard.NumOfPairs != 0)
            {
                IO.PrintBoard(m_GameBoard);

                if (isFirstPlayerTurn)
                {
                    isChangedPlayer = m_FirstPlayer.MakeHumanTurn(m_GameBoard, out o_IsQPressed);

                    if (o_IsQPressed)
                    {
                        break;
                    }
                    else if (!isChangedPlayer)
                    {
                        isFirstPlayerTurn = false;
                    }
                }
                else
                {
                    if (m_SecondPlayer.PlayerType == Player.ePlayerType.HumanPlayer)
                    {
                        isChangedPlayer = m_SecondPlayer.MakeHumanTurn(m_GameBoard, out o_IsQPressed);

                        if (o_IsQPressed)
                        {
                            break;
                        }
                        else if (!isChangedPlayer)
                        {
                            isFirstPlayerTurn = true;
                        }
                    }
                    else
                    {
                        isChangedPlayer = m_SecondPlayer.MakeComputerTurn(m_GameBoard);

                        if (!isChangedPlayer)
                        {
                            isFirstPlayerTurn = true;
                        }
                    }
                }
                
                IO.ClearScreen();
            }
        }

        private void initializeGameBoard()
        {
            int boardHeight;
            int boardWidth;
            
            IO.GetBoardHeightAndWidth(out boardHeight, out boardWidth);
            m_GameBoard = new Board(boardHeight, boardWidth);
            IO.ClearScreen();
        }

        private void initializePlayersAndMode()
        {
            string firstPlayerName;
            string secondPlayerName;

            firstPlayerName = IO.GetPlayerName(k_FirstPlayerMessage);
            m_FirstPlayer = new Player(firstPlayerName,Player.ePlayerType.HumanPlayer);
            m_GameMode = IO.GetGameMode();

            if (m_GameMode == eGameMode.PlayerVsPlayer)
            {
                secondPlayerName = IO.GetPlayerName(k_SecondPlayerMessage);
                m_SecondPlayer = new Player(secondPlayerName, Player.ePlayerType.HumanPlayer);
            }
            else
            {
                m_SecondPlayer = new Player(k_ComputerPlayerName, Player.ePlayerType.ComputerPlayer);
            }
        }
    }
}