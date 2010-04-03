using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using Floater;

namespace DataDecoder
{
    /// <summary>
    /// Summary description for FormWelcome.
    /// </summary>
    public partial class FormWelcome : FloatingWindow
    {
        /// <summary>
        /// Construct
        /// </summary>
        public FormWelcome()
        {
        }

        private void FormWelcome_Load(object sender, EventArgs e)
        {
        }
        private float mCompletionFraction = 0;
        private Font mTextFont;
        private Font mMsgFont;
        private AnimateMode mMode;
        private TextureBrush mBrush;
        private uint mTime;
        private Rectangle mRScreen;
        private System.Drawing.StringFormat mStringFormat;
        private SolidBrush mTextBrush = new SolidBrush(Color.Gold);
        private string mMessageText;
        public string MessageText
        {
            get { return mMessageText; }
            set { mMessageText = value; }
        }
        public float Progress
        {
            get
            {
                return mCompletionFraction;
            }
            set
            {
                mCompletionFraction = value;
            }
        }
        public void Show(AnimateMode mode, uint time)
        {
            Image logo = Properties.Resources.image_logo;
            this.mBrush = new TextureBrush(logo);
            this.mMode = mode;
            this.mTime = time;
            this.mTextFont = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            this.mMsgFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            this.Size = logo.Size;
//            SizeF textArea;
            this.mRScreen = Screen.PrimaryScreen.Bounds;
            if (this.mStringFormat == null)
            {
                this.mStringFormat = new StringFormat();
                this.mStringFormat.Alignment = StringAlignment.Near;
                this.mStringFormat.LineAlignment = StringAlignment.Near;
                this.mStringFormat.Trimming = StringTrimming.EllipsisWord;
            }
            base.X = mRScreen.Left + mRScreen.Width / 2 - this.Width / 2;
            base.Y = mRScreen.Top + mRScreen.Height / 2 - this.Height / 2;
            if (time > 0)
                base.ShowAnimate(mode, time);
            else
                base.Show();
        }
        public void Close(AnimateMode mode, uint time)
        {
            HideAnimate(mode, time);
        }
        new public void Invalidate()
        {
            base.Invalidate();
        }

        #region Overridden Drawing & Path Creation

        protected override void PerformPaint(PaintEventArgs e)
        {
            if (base.Handle != IntPtr.Zero)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;

                //background (png with alpha-channel)
                g.FillRectangle(this.mBrush, this.Bound);

                //text
                g.DrawString(String.Format("{0}  v{1}", Application.ProductName, Setup.ver), mTextFont, mTextBrush, 40, 15);
//                g.DrawString(mMessageText, SystemFonts.DialogFont, mTextBrush, 40, this.Height - 40);
                g.DrawString(mMessageText, mMsgFont, mTextBrush, 40, this.Height - 40);


                //progress bar
                float w = this.Width - 10 * 2;
                //g.DrawLine(mPenProgress, 40, this.Height - 20, 40 + w * mCompletionFraction, this.Height - 20);
                //line changed to texture:
                g.DrawImageUnscaledAndClipped(
                    Properties.Resources.image_load_progress_texture,
                    new Rectangle(10, 36, (int)(w * mCompletionFraction), Properties.Resources.image_load_progress_texture.Height));
            }
        }
        #endregion
    }
    public class SplashScreen
    {
        public SplashScreen()
        {
            fSplash = null;
        }
        private FormWelcome fSplash;
        private IntPtr mMainWindowHandle = IntPtr.Zero;
        public void Show()
        {
            fSplash = new FormWelcome();
            fSplash.Show(Floater.FloatingWindow.AnimateMode.Blend, 500);
        }
        public void Hide()
        {
            if (mMainWindowHandle != IntPtr.Zero)
                SetForegroundWindow(mMainWindowHandle);
            fSplash.Close(FloatingWindow.AnimateMode.Blend, 500);
            if (fSplash != null)
            {
                fSplash.Dispose();
                fSplash = null;
            }
        }
        /// <summary>
        /// Handle of window to show after splash
        /// </summary>
        public IntPtr MainWindowHandle
        {
            get
            {
                return mMainWindowHandle;
            }
            set
            {
                mMainWindowHandle = value;
            }
        }
        #region updating progress state
        public void SetProgress(string value, double progress)
        {
            try
            {
                if (fSplash != null)
                {
                    fSplash.Progress = (float)progress;
                    fSplash.MessageText = value;
                    fSplash.Invalidate();
                }
            }
            catch
            {
            }
        }
        #endregion

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);
    }
}
