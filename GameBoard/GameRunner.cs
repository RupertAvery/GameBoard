using System.Drawing;
using System.Windows.Forms;

namespace GameBoard
{
    public class GameRunner : BaseGameRunner
    {
        private Ball ball;
        private Paddle leftPaddle;
        private Paddle rightPaddle;

        public GameRunner(Control container)
            : base(container)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            ball = new Ball();
            ball.Left = (CanvasWidth - ball.Width) / 2;
            ball.Top = (CanvasHeight - ball.Height) / 2;

            ball.SpeedX = 2;
            ball.SpeedY = 2;

            leftPaddle = new Paddle();
            leftPaddle.Left = 40;
            leftPaddle.Top = (CanvasHeight - leftPaddle.Height) / 2;

            rightPaddle = new Paddle();
            rightPaddle.Left = CanvasWidth - rightPaddle.Width - 40;
            rightPaddle.Top = (CanvasHeight - rightPaddle.Height) / 2;
        }



        protected override void ProcessInputs()
        {

            if (KeyIsDown(Keys.W))
            {
                leftPaddle.Top -= 5;
            }

            if (KeyIsDown(Keys.S))
            {
                leftPaddle.Top += 5;
            }

            if (KeyIsDown(Keys.Up))
            {
                rightPaddle.Top -= 5;
            }

            if (KeyIsDown(Keys.Down))
            {
                rightPaddle.Top += 5;
            }

        }

        protected override void UpdateObjects()
        {
            ball.MoveX();
            if (ball.OutsideBoundsX(0, CanvasWidth)) ball.SpeedX = -ball.SpeedX;

            if (ball.HitTest(leftPaddle)) ball.SpeedX = -ball.SpeedX;
            if (ball.HitTest(rightPaddle)) ball.SpeedX = -ball.SpeedX;

            ball.MoveY();
            if (ball.OutsideBoundsY(0, CanvasHeight)) ball.SpeedY = -ball.SpeedY;
        }

        protected override void DrawObjects(Graphics g)
        {
            ball.Draw(g);
            leftPaddle.Draw(g);
            rightPaddle.Draw(g);
        }
    }
}