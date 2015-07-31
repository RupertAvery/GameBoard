using System.Drawing;
using System.Windows.Forms;

namespace GameBoard
{
    public class GameRunner : BaseGameRunner
    {
        private Ball ball;

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
        }
        
        protected override void ProcessInputs()
        {

            if (KeyIsDown(Keys.W))
            {
            }

            if (KeyIsDown(Keys.S))
            {
            }

            if (KeyIsDown(Keys.Up))
            {
            }

            if (KeyIsDown(Keys.Down))
            {
            }

        }

        protected override void UpdateObjects()
        {
            ball.MoveX();
            if (ball.OutsideBoundsX(0, CanvasWidth)) ball.SpeedX = -ball.SpeedX;

            ball.MoveY();
            if (ball.OutsideBoundsY(0, CanvasHeight)) ball.SpeedY = -ball.SpeedY;
        }

        protected override void DrawObjects(Graphics g)
        {
            ball.Draw(g);
        }
    }
}