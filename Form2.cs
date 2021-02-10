using System;
using System.Windows.Forms;
using static Chess.Chess;

namespace Chess
{
    public partial class Form2 : Form
    {
        public Figure Figure = Figure.Null;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Figure = !Turn ? Figure.WhiteQueen : Figure.BlackQueen;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Figure = !Turn ? Figure.WhiteKnight : Figure.BlackKnight;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Figure = !Turn ? Figure.WhiteBishop : Figure.BlackBishop;
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Figure = !Turn ? Figure.WhiteRook : Figure.BlackRook;
            Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Figure != Figure.Null) return;
            Figure = !Turn ? Figure.WhiteQueen : Figure.BlackQueen;
        }
    }
}
