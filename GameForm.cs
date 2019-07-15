using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05
{
    public class GameForm : Form
    {
        private const int k_ButtonSize = 40;
        private const string k_ButtonText = "O";
        private const int k_ButtonSpacing = 5;
        private readonly int r_BoardDimension;
        private Image m_CoinRedImage = Properties.Resources.CoinRed; 
        private Image m_CoinYellowImage = Properties.Resources.CoinYellow;
        private GameButton[,] m_ButtonMatrix;
        private GameManage m_GameManager = null;

        public GameForm(int i_BoardDimension, GameManage i_GameManager)
        {
            m_GameManager = i_GameManager;
            r_BoardDimension = i_BoardDimension;
            generateButtonMatrix();
            int formSize = ((k_ButtonSize + k_ButtonSpacing) * r_BoardDimension) + k_ButtonSpacing;
            this.ClientSize = new Size(formSize, formSize);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Text = " ";
        }

        public void RestartGame()
        {
            generateButtonMatrix();
        }

        public GameButton this[int i_row, int i_col]
        {
            get { return m_ButtonMatrix[i_row, i_col]; }
        }

        private void generateButtonMatrix()
        {
            m_ButtonMatrix = new GameButton[r_BoardDimension, r_BoardDimension];

            for (int row = 0; row < r_BoardDimension; row++)
            {
                for (int col = 0; col < r_BoardDimension; col++)
                {
                    m_ButtonMatrix[row, col] = new GameButton();
                    m_ButtonMatrix[row, col].X = col;
                    m_ButtonMatrix[row, col].Y = row;
                    m_ButtonMatrix[row, col].Width = m_ButtonMatrix[row, col].Height = k_ButtonSize;
                    m_ButtonMatrix[row, col].Location = new System.Drawing.Point((row * (k_ButtonSize + k_ButtonSpacing)) + k_ButtonSpacing, (col * (k_ButtonSpacing + k_ButtonSize)) + k_ButtonSpacing);
                    m_ButtonMatrix[row, col].Click += button_Click;
                    m_ButtonMatrix[row, col].TabIndex = ((row + 1) * r_BoardDimension) + (col + 1);
                    m_ButtonMatrix[row, col].Enabled = false;
                    m_GameManager.GetCell(row, col).ColorIsChanged += buttonChangeColor;

                    this.Controls.Add(m_ButtonMatrix[row, col]);
                }
            }
        }

        private void buttonChangeColor(Color i_NewColor, int i_X, int i_Y)
        {
            if (i_NewColor == Color.Green)
            {
                m_ButtonMatrix[i_Y, i_X].BackColor = i_NewColor;
                m_ButtonMatrix[i_Y, i_X].Enabled = true;
            }
            else
            {
                m_ButtonMatrix[i_Y, i_X].BackColor = Color.Empty;
                m_ButtonMatrix[i_Y, i_X].Enabled = false;
            }

            if (i_NewColor != Color.Green && i_NewColor != Color.Empty)
            {
                m_ButtonMatrix[i_Y, i_X].Enabled = true;
                if (i_NewColor == Color.Red)
                {
                    m_ButtonMatrix[i_Y, i_X].Image = (Image)(new Bitmap(m_CoinRedImage, new Size(35, 35)));
                }
                else
                {
                    m_ButtonMatrix[i_Y, i_X].Image = (Image)(new Bitmap(m_CoinYellowImage, new Size(35, 35)));
                }
            }
            else
            {
                m_ButtonMatrix[i_Y, i_X].Text = string.Empty;
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            GameButton button = sender as GameButton;
            int x = button.X;
            int y = button.Y;
            m_GameManager.PlayTurn(x, y);
        }
    }
}
