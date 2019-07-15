using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ex05
{
   public class Board
    {
       private readonly int r_BoardSize;
       private Cell[,] m_BoardMatrix = null;

       public int Size
       {
           get { return r_BoardSize; }
       }

        public Board(int i_BoardSize)
       {
           r_BoardSize = i_BoardSize;
           m_BoardMatrix = new Cell[i_BoardSize, i_BoardSize];
           initializeBoard();
       }

        public Cell[,] BoardMatrix
        {
            get
            {
                return m_BoardMatrix;
            }
        }

        public Cell this[int i_Row, int i_Col]
       {
           get { return m_BoardMatrix[i_Row, i_Col]; }
           set { m_BoardMatrix[i_Row, i_Col] = value; }
       }
       
       private void initializeBoard()
       {
           for (int row = 0; row < r_BoardSize; row++)
           {
               for (int col = 0; col < r_BoardSize; col++)
               {
                   m_BoardMatrix[row, col] = new Cell(Color.Empty, row, col);
               }
           }
       }

       public void RestartBoard()
       {
           for (int i = 0; i < r_BoardSize; i++)
           {
               for (int j = 0; j < r_BoardSize; j++)
               {
                   m_BoardMatrix[j, i].Color = Color.Empty;
               }
           }
       }

       public void InitializeNewOthelloBoard()
       {
           int center = r_BoardSize / 2;
           ChangeCellColor(center - 1, center - 1, Color.Yellow);
           ChangeCellColor(center, center, Color.Yellow);
           ChangeCellColor(center - 1, center, Color.Red);
           ChangeCellColor(center, center - 1, Color.Red);
       }

       public void ChangeCellColor(int i_Row, int i_Col, Color i_Color)
       {
           m_BoardMatrix[i_Row, i_Col].Color = i_Color;
       }
    }
}
