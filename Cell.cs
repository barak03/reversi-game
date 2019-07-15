using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Ex05
{
    public delegate void ButtonEventHandler(Color i_NewColor, int i_Row, int i_Col);

    public class Cell
    {
        private readonly int r_Row;
        private readonly int r_Col;
        private Color m_Color;

        public event ButtonEventHandler ColorIsChanged;

        public Cell(Color i_Color, int i_Row, int i_Col)
        {
            m_Color = i_Color;
            r_Row = i_Row;
            r_Col = i_Col;
        }

        public int Row
        {
            get { return r_Row; }
        }

        public int Col
        {
            get { return r_Col; }
        }

        public Color Color
        {
            get { return m_Color; }
            set 
            {
                m_Color = value;
                ColorIsChanged?.Invoke(value, r_Row, r_Col);
            }
        }
    }
}
