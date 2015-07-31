using System.Drawing;

namespace GameBoard
{
    public class Ball : Shape
    {
        public Ball()
        {
            Width = 20;
            Height = 20;
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.White, Left, Top, Width, Height);
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