using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ex05
{
    public class GameManage
    {
        private readonly string r_RedTurnTitle = "Othello - Red's turn";
        private readonly string r_YellowTurnTitle = "Othello - Yellow's turn";
        private readonly char r_RedColorChar = 'X';
        private readonly char r_YellowColorChar = 'O';
        private int m_YellowPlayerScore = 0;
        private int m_RedPlayerScore = 0;
        private Player m_RedPlayer = new Player(Color.Red);
        private Player m_YellowPlayer = new Player(Color.Yellow);
        private Player m_CurrentPlayer;
        private OtheloLogic m_GameLogic = new OtheloLogic();
        private Board m_GameBoard;
        private GameForm m_GameForm;
        private int m_SelectedBoardSize;
        private char[,] m_CharMatrix;

        public void InitializeGame()
        {
            GameSettingsForm settingsForm = new GameSettingsForm();
            settingsForm.ShowDialog();
            if (settingsForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (settingsForm.IsAgainstComputer == true)
                {
                    m_YellowPlayer.IsComputerPlaying = true;
                }

                m_SelectedBoardSize = settingsForm.BoardSize;
                m_GameBoard = new Board(settingsForm.BoardSize);
                m_GameForm = new GameForm(settingsForm.BoardSize, this);
                startGame();
            }
        }

        private void setListOfValidMoves()
        {
            m_RedPlayer.PlayerValidMoveList = m_GameLogic.MakeListOfValidMoves(m_CharMatrix, r_RedColorChar);
            m_YellowPlayer.PlayerValidMoveList = m_GameLogic.MakeListOfValidMoves(m_CharMatrix, r_YellowColorChar);
        }

        private void startGame()
        {
            m_GameForm.Text = r_RedTurnTitle;
            m_CharMatrix = buildBoard();
            m_CurrentPlayer = m_RedPlayer;
            m_GameBoard.RestartBoard();
            m_GameBoard.InitializeNewOthelloBoard();
            setListOfValidMoves();
            setValidButtons();
            m_GameForm.ShowDialog();
        }

        public void PlayTurn(int i_Row, int i_Col)
        {
            int listIndex;
            ValidMove move;
            Random randomNumber = new Random();
            if (m_GameBoard[i_Row, i_Col].Color == Color.Green)
            {
                if (m_CurrentPlayer.PlayerValidMoveList.Count != 0)
                {
                    if (m_CurrentPlayer == m_RedPlayer)
                    {
                        m_GameLogic.FlipDiscs(ref m_CharMatrix, i_Row, i_Col, r_RedColorChar);
                    }
                    else
                    {
                        m_GameLogic.FlipDiscs(ref m_CharMatrix, i_Row, i_Col, r_YellowColorChar);
                    }

                    switchPlayer();
                }
                else
                {
                    switchPlayer();
                }

                updateGameForm();
            }

            if (m_CurrentPlayer.IsComputerPlaying == true)
            {
                if (m_CurrentPlayer.PlayerValidMoveList.Count != 0)
                {
                    listIndex = randomNumber.Next(0, m_CurrentPlayer.PlayerValidMoveList.Count);
                    move = m_CurrentPlayer.PlayerValidMoveList[listIndex];
                    m_GameLogic.FlipDiscs(ref m_CharMatrix, move.Row, move.Col, r_YellowColorChar);
                    updateGameForm();
                    switchPlayer();
                }
                else
                {
                    switchPlayer();
                }
            }

            setValidButtons();

            if (m_RedPlayer.PlayerValidMoveList.Count == 0 && m_YellowPlayer.PlayerValidMoveList.Count == 0)
            {
                checkWhoWon();
            }
        }

        private void updateGameForm()
        {
            m_GameBoard.RestartBoard();
            for (int row = 0; row < m_CharMatrix.GetLength(0); row++)
            {
                for (int col = 0; col < m_CharMatrix.GetLength(0); col++)
                {
                    if (m_CharMatrix[row, col] == 'X')
                    {
                        m_GameBoard.ChangeCellColor(row, col, Color.Red);
                    }
                    else if (m_CharMatrix[row, col] == 'O')
                    {
                        m_GameBoard.ChangeCellColor(row, col, Color.Yellow);
                    }
                    else if (m_CharMatrix[row, col] == '0')
                    {
                        m_GameBoard.ChangeCellColor(row, col, Color.Empty);
                    }
                }
            }
        }

        private void setValidButtons()
        {
            foreach (ValidMove move in m_CurrentPlayer.PlayerValidMoveList)
            {
                if (m_GameBoard[move.Row, move.Col].Color == Color.Empty)
                {
                    m_GameBoard.ChangeCellColor(move.Row, move.Col, Color.Green);
                }
            }
        }

        public Cell GetCell(int i_Row, int i_Col)
        {
            return m_GameBoard[i_Row, i_Col];
        }

        private void switchPlayer()
        {
            if (m_CurrentPlayer == m_RedPlayer)
            {
                m_CurrentPlayer = m_YellowPlayer;
                m_GameForm.Text = r_YellowTurnTitle;
            }
            else
            {
                m_CurrentPlayer = m_RedPlayer;
                m_GameForm.Text = r_RedTurnTitle;
            }

            setListOfValidMoves();
        }

        private void checkWhoWon()
        {
            int redScore;
            int yellowScore;
            GetPlayersScore(out redScore, out yellowScore);
            string winnerMessage = null;
            if (redScore > yellowScore)
            {
                winnerMessage = string.Format(@"Red Won!! ({0}/{1}) ({2}/{3}) {4} Would you like another round?", redScore, yellowScore, ++m_RedPlayerScore, m_YellowPlayerScore, System.Environment.NewLine);
            }
            else if (yellowScore > redScore)
            {
                winnerMessage = string.Format(@"Yellow Won!! ({0}/{1}) ({2}/{3}) {4} Would you like another round?", yellowScore, redScore, ++m_YellowPlayerScore, m_RedPlayerScore, System.Environment.NewLine);
            }
            else if (yellowScore == redScore)
            {
                winnerMessage = string.Format(@"It's a tie!! ({0}/{1}) ({2}/{3}) {4} Would you like another round?", yellowScore, redScore, m_YellowPlayerScore, m_RedPlayerScore, System.Environment.NewLine);
            }

            DialogResult dialogResult = MessageBox.Show(winnerMessage, "Othello", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                m_GameForm.Dispose();
                m_GameForm = new GameForm(m_SelectedBoardSize, this);
                startGame();
            }
            else
            {
                m_GameForm.Close();
            }
        }

        private void GetPlayersScore(out int o_RedScore, out int o_YellowScore)
        {
            o_RedScore = 0;
            o_YellowScore = 0;
            foreach (Cell tempCell in m_GameBoard.BoardMatrix)
            {
                if (tempCell.Color == Color.Red)
                {
                    o_RedScore++;
                }
                else if (tempCell.Color == Color.Yellow)
                {
                    o_YellowScore++;
                }
            }
        }

        private char[,] buildBoard()
        {
            char[,] board = new char[m_SelectedBoardSize, m_SelectedBoardSize];
            for (int row = 0; row < m_SelectedBoardSize; row++)
            {
                for (int col = 0; col < m_SelectedBoardSize; col++)
                {
                    board[row, col] = '0';
                }
            }

            board[(m_SelectedBoardSize / 2) - 1, (m_SelectedBoardSize / 2) - 1] = 'O';
            board[(m_SelectedBoardSize / 2) - 1, m_SelectedBoardSize / 2] = 'X';
            board[m_SelectedBoardSize / 2, (m_SelectedBoardSize / 2) - 1] = 'X';
            board[m_SelectedBoardSize / 2, m_SelectedBoardSize / 2] = 'O';
            return board;
        }
    }
}