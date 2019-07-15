using System;
using System.Windows.Forms;
using System.Drawing;

namespace Ex05
{
    public class GameSettingsForm : Form
    {
        private const int k_MaximumBoardSize = 12;
        private const int k_MinimumBoardSize = 6;
        private bool m_GameAgainstComputer = false;
        private int m_BoardSize = k_MinimumBoardSize;
        private Button m_ButtonBoardSize = new Button();
        private Button m_ButtonComputer = new Button();
        private Button m_ButtonPlayer = new Button();

        public int BoardSize
        {
            get { return m_BoardSize; }
        }

        public GameSettingsForm()
        {
            this.Size = new Size(300, 180);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            initializeControls();
        }

        private void initializeControls()
        {
            m_ButtonBoardSize.Text = "Board Size: 6x6 (click to increase)";
            m_ButtonBoardSize.Size = new Size(this.ClientSize.Width - 40, 50);
            m_ButtonBoardSize.Location = new Point((this.ClientSize.Width / 2) - (m_ButtonBoardSize.Size.Width / 2), 20);

            m_ButtonComputer.Text = "Play against the Computer";
            m_ButtonComputer.Size = new Size((m_ButtonBoardSize.Size.Width / 2) - 20, 40);
            int topLeft = m_ButtonBoardSize.Top + (this.ClientSize.Height / 2);
            m_ButtonComputer.Location = new Point(m_ButtonBoardSize.Location.X, topLeft);

            m_ButtonPlayer.Text = "Play against your friend";
            m_ButtonPlayer.Size = m_ButtonComputer.Size;
            m_ButtonPlayer.Location = new Point(m_ButtonComputer.Width + 60, topLeft);

            this.Controls.AddRange(new Control[] { m_ButtonBoardSize, m_ButtonComputer, m_ButtonPlayer });

            this.m_ButtonBoardSize.Click += new EventHandler(boardSize_Click);
            this.m_ButtonPlayer.Click += new EventHandler(gameAgainstPlayer_Click);
            this.m_ButtonComputer.Click += new EventHandler(gameAgainstComputer_Click);
        }

        private void boardSize_Click(object sender, EventArgs e)
        {
            m_BoardSize += 2;

            if (m_BoardSize > k_MaximumBoardSize)
            {
                m_BoardSize = k_MinimumBoardSize;
            }

            string BoardSizeButtonString = string.Format("Board Size: {0}x{1} (click to increase)", m_BoardSize, m_BoardSize);
            this.m_ButtonBoardSize.Text = BoardSizeButtonString;
        }

       private void gameAgainstComputer_Click(object sender, EventArgs e)
        {
            m_GameAgainstComputer = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void gameAgainstPlayer_Click(object sender, EventArgs e)
        {
            m_GameAgainstComputer = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public bool IsAgainstComputer
        {
            get { return m_GameAgainstComputer; }
        }
    }
}