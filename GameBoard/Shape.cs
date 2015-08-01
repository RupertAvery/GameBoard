namespace GameBoard
{
    public abstract class Shape
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float SpeedX { get; set; }
        public float SpeedY { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        public float Right { get { return Left + Width; } }
        public float Bottom { get { return Top + Height; } }

        public bool OutsideBoundsX(int left, int right)
        {
            return Left < left || Left + Width > right;
        }

        public bool OutsideBoundsY(int top, int bottom)
        {
            return Top < top || Top + Height > bottom;
        }

        public bool HitTest(int left, int top, int right, int bottom)
        {
            // If moving right, test the right edge for intersection 
            if (SpeedX > 0)
            {
                return Right > left && Right < right && Top > top && Bottom < bottom;
            }
            // If moving left, test the left edge for intersection 
            if (SpeedX < 0)
            {
                return Left > left && Left < right && Top > top && Bottom < bottom;
            }
            return false;
        }

        public bool HitTest(Shape shape)
        {
            // If moving right, test the right edge for intersection 
            if (SpeedX > 0)
            {
                return Right >= shape.Left && Right <= shape.Right && Top >= shape.Top && Bottom <= shape.Bottom;
            }
            // If moving left, test the left edge for intersection 
            if (SpeedX < 0)
            {
                return Left >= shape.Left && Left <= shape.Right && Top >= shape.Top && Bottom <= shape.Bottom;
            }
            return false;
        }
    }
}