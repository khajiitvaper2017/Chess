using System.Drawing;

namespace Chess
{
    public static class Chess
    {
        public static bool Turn { get; set; }
        public struct ChessCell
        {
            public Brush Color { get; set; }
            public Figure Figure { get; set; }

        }
        public struct SelectedChessCell
        {
            public Point Point { get; set; }
            public Brush Color { get; set; }
            public Figure Figure { get; set; }
        }

        public enum Figure : byte
        {
            WhitePawn = 10,
            BlackPawn = 11,
            WhiteKing = 100,
            BlackKing = 101,
            WhiteQueen = 90,
            BlackQueen = 91,
            WhiteKnight = 30,
            BlackKnight = 31,
            WhiteBishop = 40,
            BlackBishop = 41,
            WhiteRook = 50,
            BlackRook = 51,
            Null = 0
        }

    }
}