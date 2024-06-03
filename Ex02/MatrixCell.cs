using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public struct MatrixCell
    {
        private char? m_Char;
        private bool m_IsVisible;

        public char? Char
        {
            get
            {
                return m_Char;
            }

            set
            {
                m_Char = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return m_IsVisible;
            }

            set
            {
                m_IsVisible = value;
            }
        }
    }
}