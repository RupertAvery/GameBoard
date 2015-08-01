using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameBoard
{
    public enum GameState
    {
        ResetPosition,
        PlayGame
    }


    public class GameRunner : BaseGameRunner
    {
        private Ball ball;
        private Paddle leftPaddle;
        private Paddle rightPaddle;
        private GameState gameState;
        private int player1Score = 0;
        private int player2Score = 0;
        private Font scoreFont;

        public GameRunner(Control container)
            : base(container)
        {
            scoreFont = new Font(new FontFamily("Consolas"), 24, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (scoreFont != null)
                {
                    scoreFont.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected override void GameLoop()
        {
            gameState = GameState.ResetPosition;

            while (running)
            {
                Draw();
                switch (gameState)
                {
                    case GameState.ResetPosition:
                        Pause(2000);
                        ResetPositions();
                        gameState = GameState.PlayGame;
                        break;
                    case GameState.PlayGame:
                        ProcessInputs();
                        UpdateObjects();
                        break;
                }
            }
        }

        private void SaveStates()
        {

        }

        private void ResetPositions()
        {
            ball.Left = (CanvasWidth - ball.Width) / 2;
            ball.Top = (CanvasHeight - ball.Height) / 2;
            leftPaddle.Left = 40;

            ball.SpeedX = Math.Sign(ball.SpeedX) * 5;
            ball.SpeedY = Math.Sign(ball.SpeedY) * 5;
            
            leftPaddle.Top = (CanvasHeight - leftPaddle.Height) / 2;
            rightPaddle.Left = CanvasWidth - rightPaddle.Width - 40;
            rightPaddle.Top = (CanvasHeight - rightPaddle.Height) / 2;
        }

        public override void Initialize()
        {
            base.Initialize();
            ball = new Ball();
            ball.SpeedX = 3;
            ball.SpeedY = 3;

            leftPaddle = new Paddle();

            rightPaddle = new Paddle();
            ResetPositions();

        }



        protected override void ProcessInputs()
        {

            if (KeyIsDown(Keys.W))
            {
                leftPaddle.SpeedY = -5;
            }
            else if (KeyIsDown(Keys.S))
            {
                leftPaddle.SpeedY = 5;
            }
            else
            {
                if (KeyIsUp(Keys.S) || KeyIsUp(Keys.W))
                {
                    if (Math.Abs(leftPaddle.SpeedY) > 0)
                    {
                        leftPaddle.VelocityY = -Math.Sign(leftPaddle.SpeedY) * 0.25f;
                    }
                }
            }

            if (KeyIsDown(Keys.Up))
            {
                rightPaddle.SpeedY = -5;
            }
            else if (KeyIsDown(Keys.Down))
            {
                rightPaddle.SpeedY = 5;
            }
            else
            {
                if (KeyIsUp(Keys.Up) || KeyIsUp(Keys.Down))
                {
                    if (Math.Abs(rightPaddle.SpeedY) > 0)
                    {
                        rightPaddle.VelocityY = -Math.Sign(rightPaddle.SpeedY) * 0.25f;
                    }
                }
            }

        }

        protected override void UpdateObjects()
        {
            leftPaddle.MoveY();
            rightPaddle.MoveY();

            ball.MoveX();

            if (ball.Left < 0 - ball.Width)
            {
                player2Score += 10;
                gameState = GameState.ResetPosition;
            }

            if (ball.Left > CanvasWidth)
            {
                player1Score += 10;
                gameState = GameState.ResetPosition;
            }

            if (ball.HitTest(leftPaddle))
            {
                if (leftPaddle.SpeedY != 0)
                {
                    ball.SpeedY += Math.Sign(leftPaddle.SpeedY);
                    if (Math.Abs(ball.SpeedY) > 5)
                    {
                        ball.SpeedY = Math.Sign(ball.SpeedY) * 5;
                    }
                }
                ball.SpeedX = -ball.SpeedX;
            }

            if (ball.HitTest(rightPaddle))
            {
                if (rightPaddle.SpeedY != 0)
                {
                    ball.SpeedY += Math.Sign(rightPaddle.SpeedY);
                    if (Math.Abs(ball.SpeedY) > 5)
                    {
                        ball.SpeedY = Math.Sign(ball.SpeedY) * 5;
                    }
                }
                ball.SpeedX = -ball.SpeedX;
            }

            ball.MoveY();

            if (ball.OutsideBoundsY(0, CanvasHeight)) ball.SpeedY = -ball.SpeedY;

            if (Math.Abs(leftPaddle.SpeedY) > 0)
            {
                leftPaddle.SpeedY += leftPaddle.VelocityY;
            }
            else
            {
                leftPaddle.VelocityY = 0;
            }

            if (Math.Abs(rightPaddle.SpeedY) > 0)
            {
                rightPaddle.SpeedY += rightPaddle.VelocityY;
            }
            else
            {
                rightPaddle.VelocityY = 0;
            }

            if (rightPaddle.Top < -rightPaddle.Height / 2) rightPaddle.Top = -rightPaddle.Height / 2;
            if (rightPaddle.Bottom > CanvasHeight + rightPaddle.Height / 2) rightPaddle.Top = CanvasHeight - rightPaddle.Height /2;

            if (leftPaddle.Top < -leftPaddle.Height / 2) leftPaddle.Top = -leftPaddle.Height / 2;
            if (leftPaddle.Bottom > CanvasHeight + leftPaddle.Height / 2) leftPaddle.Top = CanvasHeight - leftPaddle.Height / 2;
        }

        protected override void DrawObjects(Graphics g)
        {
            g.DrawLine(Pens.White, CanvasWidth / 2, 0, CanvasWidth / 2, CanvasHeight);
            ball.Draw(g);
            leftPaddle.Draw(g);
            rightPaddle.Draw(g);

            var p1 = string.Format("{0}", player1Score);
            var p1x = g.MeasureString(p1, scoreFont);

            var p2 = string.Format("{0}", player2Score);
            //var p2x = g.MeasureString(p2, scoreFont);

            g.DrawString(p1, scoreFont, Brushes.White, CanvasWidth / 2 - p1x.Width - 20, 10);
            g.DrawString(p2, scoreFont, Brushes.White, CanvasWidth / 2 + 20, 10);
        }
    }
}