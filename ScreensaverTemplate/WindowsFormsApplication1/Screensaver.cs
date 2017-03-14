using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        private Bitmap image;
        private Graphics imgGrphx;

        private System.Timers.Timer timer;

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
            image = new Bitmap((int)(Width * internalScale), (int)(Height * internalScale));
            imgGrphx = Graphics.FromImage(image);
            imgGrphx.SmoothingMode = SmoothingMode.AntiAlias;

            Cursor.Hide();
            TopMost = true;

            //Letting the user set up what they need on startup
            Setup();

            timer = new System.Timers.Timer(1000/fps);
            timer.Elapsed += (sender1, e1) => { Tick(); Invalidate(); };
            timer.Start();
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
            Render(imgGrphx, image.Width, image.Height);
            g.DrawImage(image, 0, 0, Width, Height);
        }

        private void stop()
        {
            timer.Dispose();
            Application.Exit();
        }

    }
}
