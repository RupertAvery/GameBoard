using System.Drawing;

namespace GameBoard
{
    public class Paddle : Shape
    {

        public Paddle()
        {
            Width = 20;
            Height = 160;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, Left, Top, Width, Height);
        }

        public void MoveX()
        {
            Left += SpeedX;
        }

        public void MoveY()
        {
            Top += SpeedY;
        }
    }
}