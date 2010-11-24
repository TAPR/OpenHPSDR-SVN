//
// Copyright © 2008, Nathan B. Evans
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//   - Redistributions of source code must retain the above copyright notice, 
//     this list of conditions and the following disclaimer.
//
//   - Redistributions in binary form must reproduce the above copyright notice, 
//     this list of conditions and the following disclaimer in the documentation 
//     and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
// NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, 
// OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.
//

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Threading;

using MRG.Controls.UI;

namespace DataDecoder {

    /// <summary>
    /// Used for dialogs that need to indicate potentially long-duration asynchronous operations to the user.
    /// </summary>
    [ToolboxItem(false)]
    public class AsyncBaseDialog : Form {

        #region Win32 Interop Methods
        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest, // handle to destination DC 
            int nXDest,     // x-coord of destination upper-left corner 
            int nYDest,     // y-coord of destination upper-left corner 
            int nWidth,     // width of destination rectangle 
            int nHeight,    // height of destination rectangle 
            IntPtr hdcSrc,  // handle to source DC 
            int nXSrc,      // x-coordinate of source upper-left corner 
            int nYSrc,      // y-coordinate of source upper-left corner 
            Int32 dwRop     // raster operation code 
        );

        [DllImport("user32.dll")]
        private extern static IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private extern static int ReleaseDC(IntPtr hWnd, IntPtr hDC); 
        #endregion

        #region Private Fields
        private FlickerFreePanel asyncPanel = new FlickerFreePanel();
        private LoadingCircle barberPole = new LoadingCircle();
        private int reference_count = 0;

        private SolidBrush fillBrush = new SolidBrush(Color.FromArgb(225, SystemColors.Window));
        private Bitmap background = null; 
        #endregion

        /// <summary>
        /// Determines if any asynchronous tasks are still running.
        /// </summary>
        protected bool IsAsyncBusy { get { return (reference_count > 0); } }

        protected AsyncBaseDialog() {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void OnPaintAsyncPanel(object sender, PaintEventArgs e) {
            if (background != null)
                //
                // Paint background image...
                e.Graphics.DrawImageUnscaled(background, 0, 0, background.Width, background.Height);
            else
                //
                // No background available (BeginAsyncOperation was probably called during the subclass constructor)...
                // So just fill with the normal back colour instead...
                e.Graphics.FillRectangle(fillBrush, 0, 0, asyncPanel.Width, asyncPanel.Height);
        }

        private void OnClickForExit(object sender, EventArgs e) {
            #if DEBUG
                EndAsyncIndication();
            #endif
        }

        #region Begin/End AsyncIndication Methods
        /// <summary>
        /// Begin indicating that an asynchronous operation is occuring.
        /// </summary>
        protected void BeginAsyncIndication() {
            //
            // Ensure that we aren't in DesignMode...
            if (DesignMode)
                return;

            //
            //
            if (!IsAsyncBusy) {
                //
                // Capture snapshot of the form...
                if (base.IsHandleCreated) {
                    //
                    // Get DC of the form...
                    IntPtr srcDc = GetDC(this.Handle);

                    //
                    // Create bitmap to store image of form...
                    Bitmap bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);

                    //
                    // Create a GDI+ context from the created bitmap...
                    using (Graphics g = Graphics.FromImage(bmp)) {
                        //
                        // Copy image of form into bitmap...
                        IntPtr bmpDc = g.GetHdc();
                        BitBlt(bmpDc, 0, 0, bmp.Width, bmp.Height, srcDc, 0, 0, 0x00CC0020 /* SRCCOPY */);

                        //
                        // Release resources...
                        ReleaseDC(this.Handle, srcDc);
                        g.ReleaseHdc(bmpDc);

                        //
                        // Blur...
                        //Smooth(bmp, 1); <-- uncomment this if you want a blurred effect
                        Grayscale(bmp);

                        //
                        // Apply translucent overlay...
                        g.FillRectangle(fillBrush, 0, 0, bmp.Width, bmp.Height);
                    }//using

                    //
                    // Store bitmap so that it can be painted by asyncPanel...
                    background = bmp;

                } else {
                    //
                    // Handle is not created yet... normally this occurs if BeginAsyncOperation() is called in the subclassed constructor.
                    // We could optionally throw an exception here but it is probably better handled by just rendering a blank background with the normal barber pole animation.
                    //
                }

                //
                // Show asyncPanel...
                asyncPanel.Visible = barberPole.Active = true;
            }

            //
            // Increment reference count...
            reference_count++;
        }

        /// <summary>
        /// Ends the current asynchronous indication.
        /// </summary>
        protected void EndAsyncIndication() {
            //
            // Decrement reference count if it is still > 0...
            if (IsAsyncBusy)
                reference_count--;

            //
            // If the reference count == 0 then stop the async indication...
            if (!IsAsyncBusy) {
                asyncPanel.Visible = barberPole.Active = false;

                if (background != null) {
                    background.Dispose();
                    background = null;
                }
            }
        } 
        #endregion        

        #region Min/Max Size and WndProc Filtering
        public override Size MaximumSize {
            get {
                if (!IsAsyncBusy)
                    return base.MaximumSize;
                else
                    return Size;
            }
            set {
                base.MaximumSize = value;
            }
        }

        public override Size MinimumSize {
            get {
                if (!IsAsyncBusy)
                    return base.MinimumSize;
                else
                    return Size;
            }
            set {
                base.MinimumSize = value;
            }
        }

        protected override void WndProc(ref Message m) {
            //
            // Filter out maximize/minimize/restore commands when IsAsyncBusy==TRUE.
            // Also filter out double-clicks of the titlebar to prevent the user min/max'ing that way.
            // Otherwise there will bad bad graphical glitches...
            if (IsAsyncBusy) {
                if (m.Msg == 0x112 /* WM_SYSCOMMAND */) {
                    int w = m.WParam.ToInt32();

                    if (w == 0xf120 /* SC_RESTORE */ || w == 0xf030 /* SC_MAXIMIZE */ || w == 0xf020 /* SC_MINIMIZE */)
                        return; // short circuit

                } else if (m.Msg == 0xa3 /* WM_NCLBUTTONDBLCLK */)
                    return; // short circuit
            }

            base.WndProc(ref m);
        } 
        #endregion

        protected override void OnLoad(EventArgs e) {
            if (DesignMode)
                return; // prevent controls being added when in design mode

            SuspendLayout();

            barberPole.BackColor = Color.Transparent;
            barberPole.Dock = DockStyle.Fill;
            barberPole.StylePreset = LoadingCircle.StylePresets.Custom;
            barberPole.SpokeThickness = 8;
            barberPole.InnerCircleRadius = 30;
            barberPole.OuterCircleRadius = 35;
            barberPole.NumberSpoke = 100;
            barberPole.Color = SystemColors.ControlDark;
            barberPole.RotationSpeed = 15;

            asyncPanel.Dock = DockStyle.Fill;
            asyncPanel.Controls.Add(barberPole);
            Controls.Add(asyncPanel);

            asyncPanel.BringToFront();

            ResumeLayout(false);

            asyncPanel.Paint += new PaintEventHandler(OnPaintAsyncPanel);

            #if DEBUG
                barberPole.Click += new EventHandler(OnClickForExit);
            #endif
        }

        /// <summary>
        /// Used internally as the surface to render the background bitmap on and a container for the barber pole.
        /// </summary>
        private sealed class FlickerFreePanel : Control {

            public FlickerFreePanel() {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
                Visible = false;
            }

        }//class

        #region Run Asynchronous Operation Method and Delegate
        /// <summary>
        /// Represents a callback method to be executed in a non-GUI thread.
        /// </summary>
        public delegate void AsyncProcessDelegate();
        private delegate void SafeWinFormsThreadDelegate();

        /// <summary>
        /// Queues a method for execution in a non-GUI thread and indicates on the form that an asynchronous operation is occuring.
        /// </summary>
        /// <param name="callback"></param>
        protected void RunAsyncOperation(AsyncProcessDelegate callback) {
            WaitCallback d = delegate(object not_used) {
                try {
                    callback.Invoke();

                } finally {
                    this.Invoke(new SafeWinFormsThreadDelegate(EndAsyncIndication));
                }
            };

            BeginAsyncIndication();
            ThreadPool.QueueUserWorkItem(d);
        }
        #endregion

        #region Bitmap Manipulation (acquired from http://www.codeproject.com/KB/GDI-plus/csharpfilters.aspx)
        private static bool Conv3x3(Bitmap b, ConvMatrix m) {
            // Avoid divide by zero errors
            if (0 == m.Factor) return false;

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride + 6 - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y) {
                    for (int x = 0; x < nWidth; ++x) {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return true;
        }

        private static bool Smooth(Bitmap b, int nWeight /* default to 1 */) {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.Factor = nWeight + 8;

            return Conv3x3(b, m);
        }

        private sealed class ConvMatrix {
            public int TopLeft = 0, TopMid = 0, TopRight = 0;
            public int MidLeft = 0, Pixel = 1, MidRight = 0;
            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
            public int Factor = 1;
            public int Offset = 0;
            public void SetAll(int nVal) {
                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
            }
        }//class

        private static bool Grayscale(Bitmap b) {
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;

                byte red, green, blue;

                for (int y = 0; y < b.Height; ++y) {
                    for (int x = 0; x < b.Width; ++x) {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            return true;
        }
        #endregion

    }//class

}//namespace
