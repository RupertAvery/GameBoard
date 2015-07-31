using System.Drawing;

namespace GameBoard
{
    public class Paddle : Shape
    {

        public Paddle()
        {
            Width = 20;
            Height = 200;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.White, Left, Top, Width, Height);
        }

    }
}