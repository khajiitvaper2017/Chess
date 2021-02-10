using System;
using System.Drawing;
using System.Windows.Forms;
using static Chess.Chess;
using static System.Drawing.Brushes;
namespace Chess
{
    public partial class Form1 : Form
    {
        public int cellSize;
        public ChessCell[,] chess;
        private int eX, eY;
        private Bitmap figureImage;
        public Graphics graphics;
        public SelectedChessCell[] LastTurn = new SelectedChessCell[2];
        private bool Moved;
        public SelectedChessCell SelectedChessCell;
        private int Xdif, Ydif;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            
            cellSize = pictureBox1.Height / 8;
            graphics = Graphics.FromImage(pictureBox1.Image);


            chess = new ChessCell[8, 8];
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                chess[i, j].Color = (i + j) % 2 == 0 ? White : ForestGreen;

                switch (j)
                {
                    case 1:
                        chess[i, j].Figure = Figure.BlackPawn;
                        break;
                    case 6:
                        chess[i, j].Figure = Figure.WhitePawn;
                        break;
                    case 0:
                        switch (i)
                        {
                            case 0:
                            case 7:
                                chess[i, j].Figure = Figure.BlackRook;
                                break;
                            case 1:
                            case 6:
                                chess[i, j].Figure = Figure.BlackKnight;
                                break;
                            case 2:
                            case 5:
                                chess[i, j].Figure = Figure.BlackBishop;
                                break;
                            case 3:
                                chess[i, j].Figure = Figure.BlackQueen;
                                break;
                            case 4:
                                chess[i, j].Figure = Figure.BlackKing;
                                break;
                        }

                        break;
                    case 7:
                        chess[i, j].Figure = chess[i, 0].Figure - 1;
                        break;
                    default:
                        chess[i, j].Figure = Figure.Null;
                        break;
                }
            }
            

            Render();
        }

        private Bitmap SelectFigureImage(Figure enumChess)
        {
            var point = new Point();
            switch (enumChess)
            {
                case Figure.WhitePawn:
                    point.X = 1000;
                    break;
                case Figure.BlackPawn:
                    point.X = 1000;
                    point.Y = 200;
                    break;
                case Figure.WhiteKing:
                    break;
                case Figure.BlackKing:
                    point.Y = 200;
                    break;
                case Figure.WhiteQueen:
                    point.X = 200;
                    break;
                case Figure.BlackQueen:
                    point.X = 200;
                    point.Y = 200;
                    break;
                case Figure.WhiteKnight:
                    point.X = 600;
                    break;
                case Figure.BlackKnight:
                    point.X = 600;
                    point.Y = 200;
                    break;
                case Figure.WhiteBishop:
                    point.X = 400;
                    break;
                case Figure.BlackBishop:
                    point.X = 400;
                    point.Y = 200;
                    break;
                case Figure.WhiteRook:
                    point.X = 800;
                    break;
                case Figure.BlackRook:
                    point.X = 800;
                    point.Y = 200;
                    break;
                case Figure.Null:
                    return new Bitmap(200, 200);
                default:
                    throw new ArgumentOutOfRangeException(nameof(enumChess), enumChess, null);
            }

            var section = new Rectangle(point, new Size(200, 200));
            var bitmap = new Bitmap(section.Width, section.Height);
            using (var g = Graphics.FromImage(bitmap))
                g.DrawImage(new Bitmap(@"cp.png"), 0, 0, section, GraphicsUnit.Pixel);
            return bitmap;
        }

        private void Render()
        {
            graphics.Clear(Color.White);
            for (var i = 0; i < 8; i++)
            for (var j = 0; j < 8; j++)
            {
                graphics.FillRectangle(chess[i, j].Color, i * cellSize, j * cellSize, cellSize, cellSize);


                figureImage = SelectFigureImage(chess[i, j].Figure);
                if (Turn)
                {
                    figureImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                graphics.DrawImage(figureImage, i * cellSize, j * cellSize, cellSize, cellSize);
            }

            if (SelectedChessCell.Figure != Figure.Null)
            {
                graphics.FillRectangle(
                    SelectedChessCell.Color == White ? LightBlue : CornflowerBlue,
                    SelectedChessCell.Point.X * cellSize,
                    SelectedChessCell.Point.Y * cellSize, cellSize, cellSize);


                figureImage = SelectFigureImage(SelectedChessCell.Figure);
                if (Turn)
                {
                    figureImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                graphics.DrawImage(figureImage, SelectedChessCell.Point.X * cellSize,
                    SelectedChessCell.Point.Y * cellSize, cellSize, cellSize);
            }

            if (Moved)
            {
                graphics.FillRectangle(
                    LastTurn[0].Color == White ? LightGreen : DarkGreen,
                    LastTurn[0].Point.X * cellSize,
                    LastTurn[0].Point.Y * cellSize, cellSize, cellSize);


                graphics.FillRectangle(
                    LastTurn[1].Color == White ? LightGreen : DarkGreen,
                    LastTurn[1].Point.X * cellSize,
                    LastTurn[1].Point.Y * cellSize, cellSize, cellSize);


                figureImage = SelectFigureImage(LastTurn[1].Figure);
                if (Turn)
                {
                    figureImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                graphics.DrawImage(figureImage, LastTurn[1].Point.X * cellSize,
                    LastTurn[1].Point.Y * cellSize, cellSize, cellSize);
            }
            GC.Collect();
            if (Turn)
            {
                pictureBox1.Image.RotateFlip(RotateFlipType.Rotate180FlipX);
            }

            pictureBox1.Refresh();
        }

        private void FigureMove()
        {
            Moved = true;
            switch (chess[eX, eY].Figure)
            {
                case Figure.BlackKing:
                    MessageBox.Show("Белые выиграли!");
                    Close();
                    break;
                case Figure.WhiteKing:
                    MessageBox.Show("Черные выиграли!");
                    Close();
                    break;
            }


            LastTurn[0].Point = new Point(SelectedChessCell.Point.X, SelectedChessCell.Point.Y);
            LastTurn[0].Color = chess[SelectedChessCell.Point.X, SelectedChessCell.Point.Y].Color;
            LastTurn[0].Figure = Figure.Null;

            LastTurn[1].Point = new Point(eX, eY);
            LastTurn[1].Color = chess[eX, eY].Color;
            LastTurn[1].Figure = SelectedChessCell.Figure;

            chess[eX, eY].Figure = SelectedChessCell.Figure;
            chess[SelectedChessCell.Point.X, SelectedChessCell.Point.Y].Figure = Figure.Null;
            Turn = !Turn;
        }

        private void DiagonalMove()
        {
            var check = 0;
            if (Xdif > 0 && Ydif > 0)
            {
                for (var k = 1; k < Xdif; k++)
                {
                    if (chess[eX + k, eY + k].Figure != Figure.Null) break;

                    check++;
                }

                if (Xdif - check == 1) FigureMove();
            }
            else if (Xdif > 0 && Ydif < 0)
            {
                for (var k = 1; k < Xdif; k++)
                {
                    if (chess[eX + k, eY - k].Figure != Figure.Null) break;

                    check++;
                }

                if (Xdif - check == 1) FigureMove();
            }
            else if (Xdif < 0 && Ydif > 0)
            {
                for (var k = 1; k < Math.Abs(Xdif); k++)
                {
                    if (chess[eX - k, eY + k].Figure != Figure.Null) break;

                    check++;
                }

                if (Ydif - check == 1) FigureMove();
            }
            else if (Xdif < 0 && Ydif < 0)
            {
                for (var k = 1; k < Math.Abs(Xdif); k++)
                {
                    if (chess[eX - k, eY - k].Figure != Figure.Null) break;

                    check++;
                }

                if (Math.Abs(Xdif) - check == 1) FigureMove();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Render();
        }

        private void StraightMove()
        {
            var check = 0;
            if (Ydif == 0 && Xdif > 0)
            {
                for (var k = 1; k < Math.Abs(Xdif); k++)
                {
                    if (chess[eX + k, eY].Figure != Figure.Null) break;

                    check++;
                }

                if (Math.Abs(Xdif) - check == 1) FigureMove();
            }
            else if (Ydif == 0 && Xdif < 0)
            {
                for (var k = 1; k < Math.Abs(Xdif); k++)
                {
                    if (chess[eX - k, eY].Figure != Figure.Null) break;

                    check++;
                }

                if (Math.Abs(Xdif) - check == 1) FigureMove();
            }
            else if (Xdif == 0 && Ydif > 0)
            {
                for (var k = 1; k < Math.Abs(Ydif); k++)
                {
                    if (chess[eX, eY + k].Figure != Figure.Null) break;

                    check++;
                }

                if (Math.Abs(Ydif) - check == 1) FigureMove();
            }
            else if (Xdif == 0 && Ydif < 0)
            {
                for (var k = 1; k < Math.Abs(Ydif); k++)
                {
                    if (chess[eX, eY - k].Figure != Figure.Null) break;

                    check++;
                }

                if (Math.Abs(Ydif) - check == 1) FigureMove();
            }
        }

        private void MouseClickOnPictureBox1(object sender, MouseEventArgs e)
        {
            eX = e.X / cellSize;
            eY = e.Y / cellSize;
            if (Turn)
            {
                eY = 7 - eY;
            }
            Xdif = SelectedChessCell.Point.X - eX;
            Ydif = SelectedChessCell.Point.Y - eY;
            if (e.Button == MouseButtons.Right)
            {
                SelectedChessCell.Figure = Figure.Null;
            }
            else if (SelectedChessCell.Figure != Figure.Null)
            {
                if (!Turn)
                    switch (SelectedChessCell.Figure)
                    {
                        case Figure.WhitePawn:
                        {
                            if (chess[eX, eY].Figure == Figure.Null && Xdif == 0 &&
                                (Ydif == 1 || Ydif == 2 && SelectedChessCell.Point.Y == 6) || Ydif == 1 &&
                                Math.Abs(Xdif) == 1 &&
                                (byte) chess[eX, eY].Figure % 2 == 1)
                                FigureMove();

                            if (chess[eX, eY].Figure == SelectedChessCell.Figure && eY == 0)
                            {
                                var form2 = new Form2();
                                form2.ShowDialog();
                                chess[eX, eY].Figure = form2.Figure;
                            }

                            break;
                        }
                        case Figure.WhiteKing:
                        {
                            if ((Math.Abs(Xdif) == 1 ||
                                 Math.Abs(Xdif) == 0) &&
                                (Math.Abs(Ydif) == 1 ||
                                 Math.Abs(Ydif) == 0) &&
                                ((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null))
                                FigureMove();
                            break;
                        }
                        case Figure.WhiteKnight:
                            if ((Math.Abs(Xdif) == 2 &&
                                 Math.Abs(Ydif) == 1 ||
                                 Math.Abs(Xdif) == 1 &&
                                 Math.Abs(Ydif) == 2) &&
                                ((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null))
                                FigureMove();
                            break;
                        case Figure.WhiteBishop:

                            if ((Math.Abs(Xdif) != 0 ||
                                 Math.Abs(Ydif) != 0) &&
                                Math.Abs(Xdif) ==
                                Math.Abs(Ydif) &&
                                ((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null))
                                DiagonalMove();

                            break;
                        case Figure.WhiteRook:

                            if (((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null) &&
                                (Xdif == 0 && Ydif != 0 || Xdif != 0 && Ydif == 0))
                                StraightMove();

                            break;
                        case Figure.WhiteQueen:
                            if ((Math.Abs(Xdif) != 0 ||
                                 Math.Abs(Ydif) != 0) &&
                                Math.Abs(Xdif) ==
                                Math.Abs(Ydif) &&
                                ((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null))
                                DiagonalMove();
                            if (((byte) chess[eX, eY].Figure % 2 != 0 || chess[eX, eY].Figure == Figure.Null) &&
                                (Xdif == 0 && Ydif != 0 || Xdif != 0 && Ydif == 0))
                                StraightMove();
                            break;
                    }
                else
                    switch (SelectedChessCell.Figure)
                    {
                        case Figure.BlackPawn:

                            if (chess[eX, eY].Figure == Figure.Null && Xdif == 0 &&
                                (Ydif == -1 || Ydif == -2 && SelectedChessCell.Point.Y == 1) || Ydif == -1 &&
                                Math.Abs(Xdif) == 1 &&
                                (byte) chess[eX, eY].Figure % 2 == 0)
                                FigureMove();
                            if (chess[eX, eY].Figure == SelectedChessCell.Figure && eY == 0)
                            {
                                var form2 = new Form2();
                                form2.ShowDialog();
                                chess[eX, eY].Figure = form2.Figure;
                            }

                            break;
                        case Figure.BlackKing:
                            if ((Math.Abs(Xdif) == 1 ||
                                 Math.Abs(Xdif) == 0) &&
                                (Math.Abs(Ydif) == 1 ||
                                 Math.Abs(Ydif) == 0) &&
                                ((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null))
                                FigureMove();
                            break;
                        case Figure.BlackKnight:
                            if ((Math.Abs(Xdif) == 2 &&
                                 Math.Abs(Ydif) == 1 ||
                                 Math.Abs(Xdif) == 1 &&
                                 Math.Abs(Ydif) == 2) &&
                                ((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null))
                                FigureMove();
                            break;
                        case Figure.BlackBishop:

                            if ((Math.Abs(Xdif) is not 0 ||
                                 Math.Abs(Ydif) != 0) &&
                                Math.Abs(Xdif) ==
                                Math.Abs(Ydif) &&
                                ((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null))
                                DiagonalMove();

                            break;
                        case Figure.BlackRook:

                            if (((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null) &&
                                (Xdif == 0 && Ydif != 0 || Xdif != 0 && Ydif == 0))
                                StraightMove();

                            break;
                        case Figure.BlackQueen:
                            if ((Math.Abs(Xdif) != 0 ||
                                 Math.Abs(Ydif) != 0) &&
                                Math.Abs(Xdif) ==
                                Math.Abs(Ydif) &&
                                ((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null))
                                DiagonalMove();
                            if (((byte) chess[eX, eY].Figure % 2 == 0 || chess[eX, eY].Figure == Figure.Null) &&
                                (Xdif == 0 && Ydif != 0 || Xdif != 0 && Ydif == 0))
                                StraightMove();
                            break;
                    }

                SelectedChessCell.Figure = Figure.Null;
            }
            else
            {
                if ((byte) chess[eX, eY].Figure % 2 == (Turn ? 1 : 0))
                {
                    SelectedChessCell.Point = new Point(eX, eY);
                    SelectedChessCell.Color = chess[eX, eY].Color;
                    SelectedChessCell.Figure = chess[eX, eY].Figure;
                }
            }


            Render();
        }
    }
}