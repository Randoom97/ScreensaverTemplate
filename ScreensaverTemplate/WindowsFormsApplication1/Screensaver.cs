using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Screensaver : Form
    {
        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hwndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        private bool previewMode = false;

        private Graphics g;

        private Task task;

        private long startms;
        private int msDelta;

        private Point mouseLocation;

        public Screensaver()
        {
            InitializeComponent();
        }

        public Screensaver(Rectangle Bounds) : this()
        {
            this.Bounds = Bounds;
            SetStyle(ControlStyles.UserPaint, true);
        }

        public Screensaver(IntPtr PreviewWndHandle) : this()
        {
            //Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            //Make this a child window so it will close when the parent dialog closes
            //GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            previewMode = true;
        }

        private void Screensaver_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            //Letting the user set up what they need on startup
            Setup();

            //Setting up the main thread
            msDelta = 1000 / fps;
            task = new Task(() => {
                while (true)
                {
                    startms = Environment.TickCount;
                    Tick();
                    Invalidate();
                    //Dynamic sleep based on previous render time
                    Thread.Sleep(Math.Max(msDelta - (int) (Environment.TickCount - startms),0));
                }
            });
            task.Start();
        }

        private void Screensaver_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    //Terminate if mouse is moved a significant distance
                    if(Math.Abs(mouseLocation.X - e.X) > 5 || Math.Abs(mouseLocation.Y - e.Y) > 5)
                    {
                        stop();
                    }
                }
                //Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void Screensaver_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                stop();
            }
        }

        private void Screensaver_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
            {
                stop();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Render(e.Graphics);
        }

        private void stop()
        {
            Application.Exit();
        }

    }
}
