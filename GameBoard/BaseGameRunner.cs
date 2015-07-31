using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;

namespace GameBoard
{
    public enum KeyState
    {
        KeyUp,
        KeyDown
    }

    public abstract class BaseGameRunner : IDisposable
    {
        private bool running;
        private Control container;
        private Thread worker;
        private int fps = 0;
        private int fpsctr = 0;
        private Bitmap canvas;
        private Stopwatch s = new Stopwatch();
        private Bitmap titleBitmap;

        protected int CanvasWidth;
        protected int CanvasHeight;

        public bool ShowFPS { get; set; }
        public Dispatcher Dispatcher { get; set; }
        protected KeyState[] keyState;

        protected BaseGameRunner(Control container)
        {
            this.container = container;
            container.KeyDown += ContainerOnKeyDown;
            container.KeyUp += ContainerOnKeyUp;

            keyState = new KeyState[256];

            var size = container.ClientSize;

            CanvasWidth = size.Width;
            CanvasHeight = size.Height;

            // All drawing routines should be done on a buffer.  Drawing directly to the container's Graphics object is slow
            canvas = new Bitmap(CanvasWidth, CanvasHeight);
            Reset();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (canvas != null)
                {
                    canvas.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Initialize()
        {
            titleBitmap = new Bitmap("Title.png");
        }

        public virtual void Cleanup()
        {

        }

        private void ContainerOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            keyState[(int)keyEventArgs.KeyCode] = KeyState.KeyDown;
        }

        private void ContainerOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            keyState[(int)keyEventArgs.KeyCode] = KeyState.KeyUp;
        }


        protected virtual void ProcessInputs()
        {

        }

        public void Reset()
        {
            running = true;
        }

        public void Run()
        {
            worker = new Thread(GameLoop);
            worker.Start();
        }

        protected void ShowTitle(float alpha)
        {
            using (var g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.Black);
                var cm = new ColorMatrix
                {
                    Matrix00 = 1,
                    Matrix11 = 1,
                    Matrix22 = 1,
                    Matrix44 = 1,
                    Matrix33 = alpha
                };

                // Create a new image attribute object and set the color matrix to
                // the one just created
                var ia = new ImageAttributes();
                ia.SetColorMatrix(cm);

                g.DrawImage(titleBitmap, new Rectangle((CanvasWidth - titleBitmap.Width) / 2, (CanvasHeight - titleBitmap.Height) / 2, titleBitmap.Width, titleBitmap.Height), 0, 0, titleBitmap.Width, titleBitmap.Height, GraphicsUnit.Pixel, ia);
            }
            RenderCanvas();
        }

        protected bool KeyIsDown(Keys key)
        {
            return keyState[(int)key] == KeyState.KeyDown;
        }

        protected void GameLoop()
        {
            // This is our main game loop. Everything that happens should pass through here
            // To add additional game states, you might want to override this to provide additional logic
            Initialize();

            // Fade in the the image
            for (float i = 0; i <= 1.0f; i += 0.005f)
            {
                ShowTitle(i);
            }

            s.Reset();
            s.Start();
            while (s.ElapsedMilliseconds < 2000)
            {
                Thread.Sleep(10);
            }

            s.Reset();
            s.Start();
            while (running)
            {
                ProcessInputs();
                UpdateObjects();
                Draw();
            }
            Cleanup();
        }


        protected virtual void UpdateObjects()
        {
            // override this in your subclass
        }

        protected virtual void DrawObjects(Graphics g)
        {
            // override this in your subclass
        }

        private void Draw()
        {
            using (var g = Graphics.FromImage(canvas))
            {
                g.Clear(Color.Black);
                if (ShowFPS)
                {
                    g.DrawString(string.Format("{0}", fps), new Font(new FontFamily("Consolas"), 12, FontStyle.Regular), Brushes.White, 0, 0);
                }

                DrawObjects(g);

                if (s.ElapsedMilliseconds >= 1000)
                {
                    fps = fpsctr;
                    fpsctr = 0;
                    s.Reset();
                }

                fpsctr++;
            }
            RenderCanvas();
        }

        protected void RenderCanvas()
        {
            // Whenever we access objects created on the UI thread, we need to call it inside Dispatcher.Invoke
            // The Dispatcher must have been retrieved from the UI thread's Dispatcher.CurrentDispatcher
            Dispatcher.Invoke(() =>
            {
                // Prevent drawing to a disposed container. This happens when we close the application.  The thread may still be running 
                // and it will try to draw to the disposed container
                if (!container.IsDisposed)
                {
                    using (var f = Graphics.FromHwnd(container.Handle))
                    {
                        f.DrawImageUnscaled(canvas, 0, 0);
                    }
                }
            });
        }

        public void Stop()
        {
            running = false;
        }
    }
}