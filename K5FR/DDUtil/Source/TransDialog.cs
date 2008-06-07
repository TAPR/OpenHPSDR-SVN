using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TransDlg
{
    /// <summary>
    /// Base class that gives any form that derives from it the effect of slowly 
    /// appearing and then disapperaing. Much like outlook email notification pop-ups
    /// </summary>
    public class TransDialog : System.Windows.Forms.Form
    {
        #region Constructor
        public TransDialog()
        {
			InitializeComponents();
        }
		public TransDialog(bool disposeAtEnd)
		{
			m_bDisposeAtEnd = disposeAtEnd;
            InitializeComponents();
		}
        void InitializeComponents()
		{
            this.components = new System.ComponentModel.Container();
            this.m_clock =  new Timer(this.components);
            this.m_clock.Interval = 50;
            this.SuspendLayout();
            //m_clock
            this.m_clock.Tick += new EventHandler(Animate);
            //TransDialog
            this.Load += new EventHandler(TransDialog_Load);
            this.Closing += new CancelEventHandler(TransDialog_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();
		}
        #endregion

        #region Event handlers
        private void TransDialog_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.0;
            m_bShowing = true;

            m_clock.Start();
        }

        private void TransDialog_Closing(object sender, CancelEventArgs e)
        {
			if (!m_bForceClose)
			{
				m_origDialogResult = this.DialogResult;
				e.Cancel = true;
				m_bShowing = false;
				m_clock.Start();
			}
			else
			{
				this.DialogResult = m_origDialogResult;
			}
        }

        #endregion

        #region Private methods
        private void Animate(object sender, EventArgs e)
        {
            if (m_bShowing)
            {
                if (this.Opacity < 1)
                {
                    this.Opacity += 0.1;
                }
                else
                {
                    m_clock.Stop();
                }
            }
            else
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.03;
                }
                else
                {
                    m_clock.Stop();
                    m_bForceClose = true;
                    this.Close();
					if (m_bDisposeAtEnd)
						this.Dispose();
                }
            }
        }

        #endregion

        #region overrides
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region private variables
        private System.ComponentModel.IContainer components = null;
        private Timer m_clock;
        private bool m_bShowing = true;
        private bool m_bForceClose = false;
		private DialogResult m_origDialogResult;
		private bool m_bDisposeAtEnd = false;
        #endregion // private variables

    }
}