using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    public class HumanPlayer
    {
        private string m_Name;
        private int m_Points;

        // Human player ctor
        public HumanPlayer(string i_Name)
        {
            m_Name = i_Name;
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
    }
}
