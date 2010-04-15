//=================================================================
// display.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// You may contact us via email at: sales@flex-radio.com.
// Paper mail may be sent to: 
//    FlexRadio Systems
//    8900 Marybank Dr.
//    Austin, TX 78750
//    USA
//=================================================================

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;


namespace PowerSDR
{		
	class Display
	{
		#region Variable Declaration

		public static Console console;
		//private static Mutex background_image_mutex;			// used to lock the base display image
		//private static Bitmap background_bmp;					// saved background picture for display
		//private static Bitmap display_bmp;					// Bitmap for use when drawing
		private static int waterfall_counter;
		private static Bitmap waterfall_bmp;					// saved waterfall picture for display
		private static Bitmap waterfall_bmp2;
		private static int[] histogram_data;					// histogram display buffer
		private static int[] histogram_history;					// histogram counter
		//private static Graphics display_graphics;				// GDI graphics object
		public const float CLEAR_FLAG = -999.999F;				// for resetting buffers
		public const int BUFFER_SIZE = 4096;
		public static float[] new_display_data;					// Buffer used to store the new data from the DSP for the display
		public static float[] current_display_data;				// Buffer used to store the current data for the display
		public static float[] new_display_data_bottom;
		public static float[] current_display_data_bottom;
		
		public static float[] rx1_average_buffer;					// Averaged display data buffer
		public static float[] rx2_average_buffer;
		public static float[] rx1_peak_buffer;						// Peak hold display data buffer
		public static float[] rx2_peak_buffer;

		#endregion

		#region Properties

        private static bool pan_fill = true;
        public static bool PanFill
        {
            get { return pan_fill; }
            set { pan_fill = value; }
        }

		private static bool tx_on_vfob = false;
		public static bool TXOnVFOB
		{
			get { return tx_on_vfob; }
			set
			{
				tx_on_vfob = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}
		private static bool split_display = false;
		public static bool SplitDisplay
		{
			get { return split_display; }
			set
			{
				split_display = value;
				DrawBackground();
			}
		}

		/*private static DisplayMode current_display_mode_top = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayModeTop
		{
			get { return current_display_mode_top; }
			set 
			{
				current_display_mode_top = value;
				if(split_display) DrawBackground();
			}
		}*/

		private static DisplayMode current_display_mode_bottom = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayModeBottom
		{
			get { return current_display_mode_bottom; }
			set
			{
				current_display_mode_bottom = value; 
				if(split_display) DrawBackground();
			}
		}

		private static int rx1_filter_low;
		public static int RX1FilterLow
		{
			get { return rx1_filter_low; }
			set { rx1_filter_low = value; }
		}

		private static int rx1_filter_high;
		public static int RX1FilterHigh
		{
			get { return rx1_filter_high; }
			set	{ rx1_filter_high = value; }
		}

		private static int rx2_filter_low;
		public static int RX2FilterLow
		{
			get { return rx2_filter_low; }
			set { rx2_filter_low = value; }
		}

		private static int rx2_filter_high;
		public static int RX2FilterHigh
		{
			get { return rx2_filter_high; }
			set	{ rx2_filter_high = value; }
		}

		private static int tx_filter_low;
		public static int TXFilterLow
		{
			get { return tx_filter_low; }
			set { tx_filter_low = value; }
		}

		private static int tx_filter_high;
		public static int TXFilterHigh
		{
			get { return tx_filter_high; }
			set	{ tx_filter_high = value; }
		}

		private static Color sub_rx_zero_line_color = Color.LightSkyBlue;
		public static Color SubRXZeroLine
		{
			get { return sub_rx_zero_line_color; }
			set
			{
				sub_rx_zero_line_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled)
					DrawBackground();
			}
		}

		private static Color sub_rx_filter_color = Color.Blue;
		public static Color SubRXFilterColor
		{
			get { return sub_rx_filter_color; }
			set
			{
				sub_rx_filter_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled)
					DrawBackground();
			}
		}

		private static bool sub_rx1_enabled = false;
		public static bool SubRX1Enabled
		{
			get { return sub_rx1_enabled; }
			set
			{
				sub_rx1_enabled = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static bool split_enabled = false;
		public static bool SplitEnabled
		{
			get { return split_enabled; }
			set
			{
				split_enabled = value;
				if(current_display_mode == DisplayMode.PANADAPTER && draw_tx_filter)
					DrawBackground();
			}
		}

		private static bool show_freq_offset = false;
		public static bool ShowFreqOffset
		{
			get { return show_freq_offset; }
			set
			{
				show_freq_offset = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static Color band_edge_color = Color.Red;
		public static Color BandEdgeColor
		{
			get{ return band_edge_color;}
			set
			{
				band_edge_color = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static long vfoa_hz;
		public static long VFOA
		{
			get { return vfoa_hz; }
			set
			{
				vfoa_hz = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static long vfoa_sub_hz;
		public static long VFOASub
		{
			get { return vfoa_sub_hz; }
			set 
			{
				vfoa_sub_hz = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static long vfob_hz;
		public static long VFOB
		{
			get { return vfob_hz; }
			set
			{
				vfob_hz = value;
				if((current_display_mode == DisplayMode.PANADAPTER && split_enabled && draw_tx_filter) ||
					(current_display_mode == DisplayMode.PANADAPTER && sub_rx1_enabled))
					DrawBackground();
			}
		}

		private static long vfob_sub_hz;
		public static long VFOBSub
		{
			get { return vfob_sub_hz; }
			set 
			{
				vfob_sub_hz = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static int rit_hz;
		public static int RIT
		{ 
			get { return rit_hz; }
			set
			{
				rit_hz = value;
				if(current_display_mode == DisplayMode.PANADAPTER)
					DrawBackground();
			}
		}

		private static int xit_hz;
		public static int XIT
		{ 
			get { return xit_hz; }
			set
			{
				xit_hz = value;
				if(current_display_mode == DisplayMode.PANADAPTER && (draw_tx_filter || mox))
					DrawBackground();
			}
		}

		private static int cw_pitch = 600;
		public static int CWPitch
		{
			get { return cw_pitch; }
			set { cw_pitch = value; }
		}

		private static int H = 0;	// target height
		private static int W = 0;	// target width
		private static Control target = null;
		public static Control Target
		{
			get { return target; }
			set
			{
				target = value;
				H = target.Height;
				W = target.Width;
				Audio.ScopeDisplayWidth = W;
			}
		}

		private static int rx_display_low = -4000;
		public static int RXDisplayLow
		{
			get { return rx_display_low; }
			set { rx_display_low = value; }
		}

		private static int rx_display_high = 4000;
		public static int RXDisplayHigh
		{
			get { return rx_display_high; }
			set { rx_display_high = value; }
		}

		private static int tx_display_low = -4000;
		public static int TXDisplayLow
		{
			get { return tx_display_low; }
			set { tx_display_low = value; }
		}

		private static int tx_display_high = 4000;
		public static int TXDisplayHigh
		{
			get { return tx_display_high; }
			set { tx_display_high = value; }
		}

		private static float rx1_preamp_offset = 0.0f;
		public static float RX1PreampOffset
		{
			get { return rx1_preamp_offset; }
			set	{ rx1_preamp_offset = value; }
		}

		private static float rx2_preamp_offset = 0.0f;
		public static float RX2PreampOffset
		{
			get { return rx2_preamp_offset; }
			set	{ rx2_preamp_offset = value; }
		}

		private static float rx1_display_cal_offset;					// display calibration offset in dB
		public static float RX1DisplayCalOffset
		{
			get { return rx1_display_cal_offset; }
			set { rx1_display_cal_offset = value; }
		}

		private static float rx2_display_cal_offset;					// display calibration offset in dB
		public static float RX2DisplayCalOffset
		{
			get { return rx2_display_cal_offset; }
			set { rx2_display_cal_offset = value; }
		}

		private static Model current_model = Model.FLEX5000;
		public static Model CurrentModel
		{
			get { return current_model; }
			set	{ current_model = value; }
		}

		private static int display_cursor_x;						// x-coord of the cursor when over the display
		public static int DisplayCursorX
		{
			get { return display_cursor_x; }
			set { display_cursor_x = value; }
		}

		private static int display_cursor_y;						// y-coord of the cursor when over the display
		public static int DisplayCursorY
		{
			get { return display_cursor_y; }
			set { display_cursor_y = value; }
		}

		private static ClickTuneMode current_click_tune_mode = ClickTuneMode.Off;
		public static ClickTuneMode CurrentClickTuneMode
		{
			get { return current_click_tune_mode; }
			set { current_click_tune_mode = value; }
		}

		private static int scope_time = 50;
		public static int ScopeTime
		{
			get { return scope_time; }
			set { scope_time = value; }
		}

		private static int sample_rate = 48000;
		public static int SampleRate
		{
			get { return sample_rate; }
			set { sample_rate = value; }
		}

		private static bool high_swr = false;
		public static bool HighSWR
		{
			get { return high_swr; }
			set { high_swr = value; }
		}

		private static DisplayEngine current_display_engine = DisplayEngine.GDI_PLUS;
		public static DisplayEngine CurrentDisplayEngine
		{
			get { return current_display_engine; }
			set	{ current_display_engine = value; }
		}

		private static bool mox = false;
		public static bool MOX
		{
			get { return mox; }
			set { mox = value; }
		}

		private static DSPMode rx1_dsp_mode = DSPMode.USB;
		public static DSPMode RX1DSPMode
		{
			get { return rx1_dsp_mode; }
			set { rx1_dsp_mode = value; }
		}

		private static DSPMode rx2_dsp_mode = DSPMode.USB;
		public static DSPMode RX2DSPMode
		{
			get { return rx2_dsp_mode; }
			set { rx2_dsp_mode = value; }
		}

		private static DisplayMode current_display_mode = DisplayMode.PANADAPTER;
		public static DisplayMode CurrentDisplayMode
		{
			get { return current_display_mode; }
			set 
			{
				//PrepareDisplayVars(value);

				current_display_mode = value;

				/*switch(current_display_mode)
				{
					case DisplayMode.PANADAPTER:
					case DisplayMode.WATERFALL:
						DttSP.SetPWSmode(0, 0, 1);
						DttSP.NotPan = false;
						break;
					default:
						DttSP.SetPWSmode(0, 0, 0);
						DttSP.NotPan = true;
						break;
				}*/

				switch(current_display_mode)
				{
					case DisplayMode.PHASE2:
						Audio.phase = true;
						break;
					default:
						Audio.phase = false;
						break;
				}

				if(average_on) ResetRX1DisplayAverage();
				if(peak_on) ResetRX1DisplayPeak();

				DrawBackground();
			}
		}

		private static float max_x;								// x-coord of maxmimum over one display pass
		public static float MaxX
		{
			get { return max_x; }
			set { max_x = value; }
		}

		private static float max_y;								// y-coord of maxmimum over one display pass
		public static float MaxY
		{
			get { return max_y; }
			set { max_y = value; }
		}

		private static bool average_on;							// True if the Average button is pressed
		public static bool AverageOn
		{
			get { return average_on; }
			set 
			{
				average_on = value;
				if(!average_on) ResetRX1DisplayAverage();
			}
		}

		private static bool rx2_avg_on;
		public static bool RX2AverageOn
		{
			get { return rx2_avg_on; }
			set
			{
				rx2_avg_on = value;
				if(!rx2_avg_on) ResetRX2DisplayAverage();
			}
		}

		private static bool peak_on;							// True if the Peak button is pressed
		public static bool PeakOn
		{
			get { return peak_on; }
			set
			{
				peak_on = value;
				if(!peak_on) ResetRX1DisplayPeak();
			}
		}

		private static bool rx2_peak_on;
		public static bool RX2PeakOn
		{
			get { return rx2_peak_on; }
			set
			{
				rx2_peak_on = value;
				if(!rx2_peak_on) ResetRX2DisplayPeak();
			}
		}

		private static bool data_ready;					// True when there is new display data ready from the DSP
		public static bool DataReady
		{
			get { return data_ready; }
			set { data_ready = value; }
		}

		private static bool data_ready_bottom;
		public static bool DataReadyBottom
		{
			get { return data_ready_bottom; }
			set { data_ready_bottom = value; }
		}

		public static float display_avg_mult_old = 1 - (float)1/5;
		public static float display_avg_mult_new = (float)1/5;
		private static int display_avg_num_blocks = 5;
		public static int DisplayAvgBlocks
		{
			get { return display_avg_num_blocks; }
			set
			{
				display_avg_num_blocks = value;
				display_avg_mult_old = 1 - (float)1/display_avg_num_blocks;
				display_avg_mult_new = (float)1/display_avg_num_blocks;
			}
		}

		public static float waterfall_avg_mult_old = 1 - (float)1/18;
		public static float waterfall_avg_mult_new = (float)1/18;
		private static int waterfall_avg_num_blocks = 18;
		public static int WaterfallAvgBlocks
		{
			get { return waterfall_avg_num_blocks; }
			set
			{
				waterfall_avg_num_blocks = value;
				waterfall_avg_mult_old = 1 - (float)1/waterfall_avg_num_blocks;
				waterfall_avg_mult_new = (float)1/waterfall_avg_num_blocks;
			}
		}

		private static int spectrum_grid_max = 0;
		public static int SpectrumGridMax
		{
			get{ return spectrum_grid_max;}
			set
			{	
				spectrum_grid_max = value;
				DrawBackground();
			}
		}

		private static int spectrum_grid_min = -160;
		public static int SpectrumGridMin
		{
			get{ return spectrum_grid_min;}
			set
			{
				spectrum_grid_min = value;
				DrawBackground();
			}
		}

		private static int spectrum_grid_step = 10;
		public static int SpectrumGridStep
		{
			get{ return spectrum_grid_step;}
			set
			{
				spectrum_grid_step = value;
				DrawBackground();
			}
		}

		private static Color grid_text_color = Color.Yellow;
		public static Color GridTextColor
		{
			get{ return grid_text_color;}
			set
			{
				grid_text_color = value;
				DrawBackground();
			}
		}

		private static Color grid_zero_color = Color.Red;
		public static Color GridZeroColor
		{
			get{ return grid_zero_color;}
			set
			{
				grid_zero_color = value;
				DrawBackground();
			}
		}

		private static Color grid_color = Color.Purple;
		public static Color GridColor
		{
			get{ return grid_color;}
			set
			{
				grid_color = value;
				DrawBackground();
			}
		}

		private static Pen data_line_pen = new Pen(new SolidBrush(Color.White), display_line_width);
		private static Color data_line_color = Color.White;
		public static Color DataLineColor
		{
			get{ return data_line_color;}
			set
			{
				data_line_color = value;
				data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
				DrawBackground();
			}
		}

		private static Color display_filter_color = Color.FromArgb(65, 255, 255, 255);
		public static Color DisplayFilterColor
		{
			get { return display_filter_color; }
			set
			{
				display_filter_color = value;
				DrawBackground();
			}
		}

		private static Color display_filter_tx_color = Color.Yellow;
		public static Color DisplayFilterTXColor
		{
			get { return display_filter_tx_color; }
			set
			{
				display_filter_tx_color = value;
				DrawBackground();
			}
		}

		private static bool draw_tx_filter = false;
		public static bool DrawTXFilter
		{
			get { return draw_tx_filter; }
			set
			{
				draw_tx_filter = value;
				DrawBackground();
			}
		}

		private static bool draw_tx_cw_freq = false;
		public static bool DrawTXCWFreq
		{
			get { return draw_tx_cw_freq; }
			set
			{
				draw_tx_cw_freq = value;
				DrawBackground();
			}
		}

		private static Color display_background_color = Color.Black;
		public static Color DisplayBackgroundColor
		{
			get { return display_background_color; }
			set
			{
				display_background_color = value;
				DrawBackground();
			}
		}
	
		private static Color waterfall_low_color = Color.Black;
		public static Color WaterfallLowColor
		{
			get { return waterfall_low_color; }
			set { waterfall_low_color = value; }
		}

		private static Color waterfall_mid_color = Color.Red;
		public static Color WaterfallMidColor
		{
			get { return waterfall_mid_color; }
			set { waterfall_mid_color = value; }
		}

		private static Color waterfall_high_color = Color.Yellow;
		public static Color WaterfallHighColor
		{
			get { return waterfall_high_color; }
			set { waterfall_high_color = value; }
		}

		private static float waterfall_high_threshold = -80.0F;
		public static float WaterfallHighThreshold
		{
			get { return waterfall_high_threshold; }
			set { waterfall_high_threshold = value; }
		}

		private static float waterfall_low_threshold = -130.0F;
		public static float WaterfallLowThreshold
		{
			get { return waterfall_low_threshold; }
			set { waterfall_low_threshold = value; }
		}

		private static float display_line_width = 1.0F;
		public static float DisplayLineWidth
		{
			get { return display_line_width; }
			set
			{
				display_line_width = value;
				data_line_pen = new Pen(new SolidBrush(data_line_color), display_line_width);
			}
		}

		private static DisplayLabelAlignment display_label_align = DisplayLabelAlignment.LEFT;
		public static DisplayLabelAlignment DisplayLabelAlign
		{
			get { return display_label_align; }
			set
			{
				display_label_align = value;
				DrawBackground();
			}
		}

		private static int phase_num_pts = 100;
		public static int PhaseNumPts
		{
			get{ return phase_num_pts;}
			set{ phase_num_pts = value;}
		}

		#endregion

		#region General Routines

		public static void Init()
		{
			histogram_data = new int[W];
			histogram_history = new int[W];
			for(int i=0; i<W; i++)
			{
				histogram_data[i] = Int32.MaxValue;
				histogram_history[i] = 0;
			}

			//display_bmp = new Bitmap(W, H);
			//display_graphics = Graphics.FromImage(display_bmp);
			waterfall_bmp = new Bitmap(W, H-16, PixelFormat.Format24bppRgb);	// initialize waterfall display
			waterfall_bmp2 = new Bitmap(W, H-16, PixelFormat.Format24bppRgb);

			rx1_average_buffer = new float[BUFFER_SIZE];	// initialize averaging buffer array
			rx1_average_buffer[0] = CLEAR_FLAG;		// set the clear flag

			rx2_average_buffer = new float[BUFFER_SIZE];	// initialize averaging buffer array
			rx2_average_buffer[0] = CLEAR_FLAG;		// set the clear flag

			rx1_peak_buffer = new float[BUFFER_SIZE];
			rx1_peak_buffer[0] = CLEAR_FLAG;

			rx2_peak_buffer = new float[BUFFER_SIZE];
			rx2_peak_buffer[0] = CLEAR_FLAG;

			//background_image_mutex = new Mutex(false);

			new_display_data = new float[BUFFER_SIZE];
			current_display_data = new float[BUFFER_SIZE];
			new_display_data_bottom = new float[BUFFER_SIZE];
			current_display_data_bottom = new float[BUFFER_SIZE];

			for(int i=0; i<BUFFER_SIZE; i++)
			{
				new_display_data[i] = -200.0f;
				current_display_data[i] = -200.0f;
				new_display_data_bottom[i] = -200.0f;
				current_display_data_bottom[i] = -200.0f;
			}

			/*if(!DirectXInit()) console.SetupForm.DirectX = false;
			if(current_display_engine != DisplayEngine.DIRECT_X)
				DirectXRelease();*/
		}

		public static void DrawBackground()
		{
			// draws the background image for the display based
			// on the current selected display mode.

			if(current_display_engine == DisplayEngine.GDI_PLUS)
			{
				/*switch(current_display_mode)
				{
					case DisplayMode.SPECTRUM:
						DrawSpectrumGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.PANADAPTER:
						DrawPanadapterGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.SCOPE:
						DrawScopeGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.PHASE:
						DrawPhaseGrid(ref background_bmp, W, H);
						break;	
					case DisplayMode.PHASE2:
						DrawPhaseGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.WATERFALL:
						DrawWaterfallGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.HISTOGRAM:
						DrawSpectrumGrid(ref background_bmp, W, H);
						break;
					case DisplayMode.OFF:
						DrawOffBackground(ref background_bmp, W, H);
						break;
					default:
						break;
				}
*/
				target.Invalidate();
			}
			/*else if(current_display_engine == DisplayEngine.DIRECT_X)
			{
				switch(current_display_mode)
				{
					case DisplayMode.SPECTRUM:
						current_background = SetupSpectrum();
						break;
					case DisplayMode.PANADAPTER:
						break;
					case DisplayMode.SCOPE:
						current_background = SetupScope();
						break;
					case DisplayMode.PHASE:
						break;	
					case DisplayMode.PHASE2:
						break;
					case DisplayMode.WATERFALL:
						break;
					case DisplayMode.HISTOGRAM:
						break;
					case DisplayMode.OFF:
						break;
					default:
						break;
				}				
				
				// redraw screen now if not starting up and if in standby
				//if(console.SetupForm != null && !console.PowerOn) RenderDirectX();
			}*/
		}

		#endregion

		#region GDI+

		unsafe public static void RenderGDIPlus(ref PaintEventArgs e)
		{
			/*BitmapData display_bmpData = display_bmp.LockBits(
				new Rectangle(0, 0, W, H),
				ImageLockMode.WriteOnly,
				display_bmp.PixelFormat);

			background_image_mutex.WaitOne();			// get background image

			BitmapData background_bmpData = background_bmp.LockBits(
				new Rectangle(0, 0, background_bmp.Width, background_bmp.Height),
				ImageLockMode.ReadOnly,
				background_bmp.PixelFormat);
				
			int total_size = background_bmpData.Stride * background_bmpData.Height;		// find buffer size

			byte *srcptr = (byte *)background_bmpData.Scan0.ToPointer();
			byte *destptr = (byte *)display_bmpData.Scan0.ToPointer();

			Win32.memcpy(destptr, srcptr, total_size);

			background_bmp.UnlockBits(background_bmpData);
			background_image_mutex.ReleaseMutex();

			display_bmp.UnlockBits(display_bmpData);*/

			//Graphics g = Graphics.FromImage(display_bitmap);
			//g.SmoothingMode = SmoothingMode.AntiAlias;
			bool update = true;
			
			if(!split_display)
			{
				switch(current_display_mode) 
				{
					case DisplayMode.SPECTRUM:
						update = DrawSpectrum(e.Graphics, W, H, false);
						break;
					case DisplayMode.PANADAPTER:
						update = DrawPanadapter(e.Graphics, W, H, 1, false);
						break;
					case DisplayMode.SCOPE:
						update = DrawScope(e.Graphics, W, H, false);
						break;
					case DisplayMode.PHASE:
						update = DrawPhase(e.Graphics, W, H, false);
						break;
					case DisplayMode.PHASE2:
						DrawPhase2(e.Graphics, W, H, false);
						break;
					case DisplayMode.WATERFALL:
						update = DrawWaterfall(e.Graphics, W, H, 1, false);
						break;
					case DisplayMode.HISTOGRAM:
						update = DrawHistogram(e.Graphics, W, H);
						break;
					case DisplayMode.PANAFALL:
						split_display = true;
						update = DrawPanadapter(e.Graphics, W, H/2, 1, false);
						update = DrawWaterfall(e.Graphics, W, H/2, 1, true);
						split_display = false;
						break;
					case DisplayMode.PANASCOPE:
						split_display = true;
						update = DrawPanadapter(e.Graphics, W, H/2, 1, false);
						update = DrawScope(e.Graphics, W, H/2, true);
						split_display = false;
						break;
					case DisplayMode.OFF:
						//Thread.Sleep(1000);
						break;
					default:
						break;
				}
			}
			else
			{
				switch(current_display_mode)
				{
					case DisplayMode.SPECTRUM:
						update = DrawSpectrum(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PANADAPTER:
						update = DrawPanadapter(e.Graphics, W, H/2, 1, false);
						break;
					case DisplayMode.SCOPE:
						update = DrawScope(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PHASE:
						update = DrawPhase(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.PHASE2:
						DrawPhase2(e.Graphics, W, H/2, false);
						break;
					case DisplayMode.WATERFALL:
						update = DrawWaterfall(e.Graphics, W, H/2, 1, false);
						break;
					case DisplayMode.HISTOGRAM:
						update = DrawHistogram(e.Graphics, W, H/2);
						break;
					case DisplayMode.OFF:
						DrawOffBackground(e.Graphics, W, H/2, false);
						break;
					default:
						break;
				}

				switch(current_display_mode_bottom)
				{
					case DisplayMode.SPECTRUM:
						update = DrawSpectrum(e.Graphics, W, H/2, true);
						break;
					case DisplayMode.PANADAPTER:
						update = DrawPanadapter(e.Graphics, W, H/2, 2, true);
						break;
					case DisplayMode.SCOPE:
						update = DrawScope(e.Graphics, W, H/2, true);
						break;
					case DisplayMode.PHASE:
						update = DrawPhase(e.Graphics, W, H/2, true);
						break;
					case DisplayMode.PHASE2:
						DrawPhase2(e.Graphics, W, H/2, true);
						break;
					case DisplayMode.WATERFALL:
						update = DrawWaterfall(e.Graphics, W, H/2, 2, true);
						break;
					case DisplayMode.HISTOGRAM:
						update = DrawHistogram(e.Graphics, W, H/2);
						break;
					case DisplayMode.OFF:
						DrawOffBackground(e.Graphics, W, H/2, true);
						break;
					default:
						break;
				}
			}

			if(update)
			{
				//e.Graphics.DrawImage(display_bmp, 0, 0);
			}
			else
			{
				Debug.WriteLine("display update = false");
			}
		}

		private static void UpdateDisplayPeak(float[] buffer, float[] new_data) 
		{
			if(buffer[0] == CLEAR_FLAG)
			{
				//Debug.WriteLine("Clearing peak buf"); 
				for(int i=0; i<BUFFER_SIZE; i++)
					buffer[i] = new_data[i];
			}
			else
			{
				for(int i=0; i<BUFFER_SIZE; i++)
				{
					if(new_data[i] > buffer[i])
						buffer[i] = new_data[i];
					new_data[i] = buffer[i];
				}
			}
		}

		#region Drawing Routines
		// ======================================================
		// Drawing Routines
		// ======================================================

		
		private static void DrawPhaseGrid(ref Graphics g, int W, int H, bool bottom)
		{
			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			for(double i=0.50; i < 3; i+=.50)	// draw 3 concentric circles
			{
				if(bottom) g.DrawEllipse(new Pen(grid_color), (int)(W/2-H*i/2), H+(int)(H/2-H*i/2), (int)(H*i), (int)(H*i));
				else g.DrawEllipse(new Pen(grid_color), (int)(W/2-H*i/2), (int)(H/2-H*i/2), (int)(H*i), (int)(H*i));
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawScopeGrid(ref Graphics g, int W, int H, bool bottom)
		{
			// draw background
			//if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			//else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			if(bottom)
			{
				g.DrawLine(new Pen(grid_color), 0, H+H/2, W, H+H/2);	// draw horizontal line
				g.DrawLine(new Pen(grid_color), W/2, H, W/2, H+H);	// draw vertical line
			}
			else
			{
				g.DrawLine(new Pen(grid_color), 0, H/2, W, H/2);	// draw horizontal line
				g.DrawLine(new Pen(grid_color), W/2, 0, W/2, H);	// draw vertical line
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawSpectrumGrid(ref Graphics g, int W, int H, bool bottom)
		{
			System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
			SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
			Pen grid_pen = new Pen(grid_color);

			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			int low = 0;								// init limit variables
			int high = 0;

			int center_line_x = (int)(-(double)low/(high-low)*W);

			if(!mox)
			{
				low = rx_display_low;				// get RX display limits
				high = rx_display_high;
			}
			else
			{
				if(rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)
				{
					low = rx_display_low;
					high = rx_display_high;
				}
				else
				{
					low = tx_display_low;			// get RX display limits
					high = tx_display_high;
				}
			}

			int mid_w = W/2;
			int[] step_list = {10, 20, 25, 50};
			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;
			int y_range = spectrum_grid_max - spectrum_grid_min;
			int grid_step = spectrum_grid_step;
			if(split_display) grid_step *= 2;

			if(high == 0)
			{
				int f = -low;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);

				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = W-(int)Math.Floor(i*pixel_step_size);	// for negative numbers
					
					if(bottom) g.DrawLine(grid_pen, x, H, x, H+H);
					else g.DrawLine(grid_pen, x, 0, x, H);				// draw right line
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)((label.Length+1)*4.1);
					if(x-offset >= 0)
					{
						if(bottom) g.DrawString("-"+label, font, grid_text_brush, x-offset, H+(float)Math.Floor(H*.01));
						else g.DrawString("-"+label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				num_steps = V/grid_step;
				pixel_step_size = H/num_steps;

				for(int i=1; i<num_steps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)
                            xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), W-1, H, W-1, H+H);
					g.DrawLine(new Pen(grid_zero_color), W-2, H, W-2, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), W-1, 0, W-1, H);
					g.DrawLine(new Pen(grid_zero_color), W-2, 0, W-2, H);
				}
			}
			else if(low == 0)
			{
				int f = high;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = (int)Math.Floor(i*pixel_step_size);// for positive numbers
					
					if(bottom) g.DrawLine(grid_pen, x, H, x, H+H);
					else g.DrawLine(grid_pen, x, 0, x, H);				// draw right line
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)(label.Length*4.1);
					if(x-offset+label.Length*7 < W)
					{
						if(bottom) g.DrawString(label, font, grid_text_brush, x-offset, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)
                            xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), 0, H, 0, H+H);
					g.DrawLine(new Pen(grid_zero_color), 1, H, 1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), 0, 0, 0, H);
					g.DrawLine(new Pen(grid_zero_color), 1, 0, 1, H);
				}
			}
			else if(low < 0 && high > 0)
			{
				int f = high;

				// Calculate horizontal step size
				while(f/freq_step_size > 4)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				int pixel_step_size = W/2*freq_step_size/f;
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int xLeft = mid_w-(i*pixel_step_size);			// for negative numbers
					int xRight = mid_w+(i*pixel_step_size);		// for positive numbers
					if(bottom)
					{
						g.DrawLine(grid_pen, xLeft, H, xLeft, H+H);		// draw left line
						g.DrawLine(grid_pen, xRight, H, xRight, H+H);		// draw right line
					}
					else
					{
						g.DrawLine(grid_pen, xLeft, 0, xLeft, H);		// draw left line
						g.DrawLine(grid_pen, xRight, 0, xRight, H);		// draw right line
					}
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offsetL = (int)((label.Length+1)*4.1);
					int offsetR = (int)(label.Length*4.1);
					if(xLeft-offsetL >= 0)
					{
						if(bottom)
						{
							g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, H+(float)Math.Floor(H*.01));
							g.DrawString(label, font, grid_text_brush, xRight-offsetR, H+(float)Math.Floor(H*.01));
						}
						else
						{
							g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, (float)Math.Floor(H*.01));
							g.DrawString(label, font, grid_text_brush, xRight-offsetR, (float)Math.Floor(H*.01));
						}
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);
					if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
					else g.DrawLine(grid_pen, 0, y, W, y);

					// Draw horizontal line labels
                    if (i != 1) // avoid intersecting vertical and horizontal labels
                    {
                        string label = num.ToString();
                        if (label.Length == 3)
                            xOffset = (int)g.MeasureString("-", font).Width - 2;
                        int offset = (int)(label.Length * 4.1);
                        SizeF size = g.MeasureString(label, font);

                        int x = 0;
                        switch (display_label_align)
                        {
                            case DisplayLabelAlignment.LEFT:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.CENTER:
                                x = center_line_x + xOffset;
                                break;
                            case DisplayLabelAlignment.RIGHT:
                                x = (int)(W - size.Width - 3);
                                break;
                            case DisplayLabelAlignment.AUTO:
                                x = xOffset + 3;
                                break;
                            case DisplayLabelAlignment.OFF:
                                x = W;
                                break;
                        }

                        y -= 8;
                        if (y + 9 < H)
                        {
                            if (bottom) g.DrawString(label, font, grid_text_brush, x, H + y);
                            g.DrawString(label, font, grid_text_brush, x, y);
                        }
                    }
				}

				// Draw middle vertical line
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), mid_w, H, mid_w, H+H);
					g.DrawLine(new Pen(grid_zero_color), mid_w-1, H, mid_w-1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), mid_w, 0, mid_w, H);
					g.DrawLine(new Pen(grid_zero_color), mid_w-1, 0, mid_w-1, H);
				}
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawPanadapterGrid(ref Graphics g, int W, int H, int rx, bool bottom)
		{
			// draw background
			/*if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);*/

			bool local_mox = mox;
			if(rx==2) local_mox = false;
			int low = rx_display_low;					// initialize variables
			int high = rx_display_high;
			int mid_w = W/2;
			int[] step_list = {10, 20, 25, 50};
			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;
            int inbetweenies = 5;
			int grid_step = spectrum_grid_step;
			if(split_display) grid_step *= 2;

			System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
			SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
			//Pen grid_pen = new Pen(grid_color);
            Pen grid_pen = new Pen(Color.FromArgb(42, Color.White));
            Pen grid_pen_dark = new Pen(Color.FromArgb(16, Color.White));
            //display_filter_color = Color.FromArgb(40, 255, 255, 255);
			Pen tx_filter_pen = new Pen(display_filter_tx_color);
			int y_range = spectrum_grid_max - spectrum_grid_min;
			int filter_low, filter_high;

			int center_line_x = (int)(-(double)low/(high-low)*W);

			if(local_mox) // get filter limits
			{
				filter_low = tx_filter_low;
				filter_high = tx_filter_high;
			}
			else if(rx == 1)
			{
				filter_low = rx1_filter_low;
				filter_high = rx1_filter_high;
			}
			else //if(rx == 2)
			{
				filter_low = rx2_filter_low;
				filter_high = rx2_filter_high;
			}

			if((rx1_dsp_mode == DSPMode.DRM && rx==1) ||
				(rx2_dsp_mode == DSPMode.DRM && rx==2))
			{
				filter_low = -5000;
				filter_high = 5000;
			}

			// Calculate horizontal step size
			int width = high-low;
			while(width/freq_step_size > 10)
			{
                /*inbetweenies = step_list[step_index] / 10;
                if (inbetweenies == 1) inbetweenies = 10;*/
				freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
				step_index = (step_index+1)%4;
				if(step_index == 0) step_power++;
			}
			double w_pixel_step = (double)W*freq_step_size/width;
			int w_steps = width/freq_step_size;

			// calculate vertical step size
			int h_steps = (spectrum_grid_max - spectrum_grid_min)/grid_step;
			double h_pixel_step = (double)H/h_steps;
			int top = (int)((double)grid_step*H/y_range);

			if(!local_mox && sub_rx1_enabled && rx==1)
			{
				// draw Sub RX filter
				// get filter screen coordinates
				int filter_left_x = (int)((float)(filter_low-low+vfoa_sub_hz-vfoa_hz-rit_hz)/(high-low)*W);
				int filter_right_x = (int)((float)(filter_high-low+vfoa_sub_hz-vfoa_hz-rit_hz)/(high-low)*W);

				// make the filter display at least one pixel wide.
				if(filter_left_x == filter_right_x) filter_right_x = filter_left_x+1;

				// draw rx filter
				if(bottom)
				{
					g.FillRectangle(new SolidBrush(sub_rx_filter_color),	// draw filter overlay
						filter_left_x, H+top, filter_right_x-filter_left_x, H+H-top);
				}
				else
				{
					g.FillRectangle(new SolidBrush(sub_rx_filter_color),	// draw filter overlay
						filter_left_x, top, filter_right_x-filter_left_x, H-top);
				}

				// draw Sub RX 0Hz line
				int x = (int)((float)(vfoa_sub_hz-vfoa_hz-low)/(high-low)*W);
				if(bottom)
				{
					g.DrawLine(new Pen(sub_rx_zero_line_color), x, H+top, x, H+H);
					g.DrawLine(new Pen(sub_rx_zero_line_color), x-1, H+top, x-1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(sub_rx_zero_line_color), x, top, x, H);
					g.DrawLine(new Pen(sub_rx_zero_line_color), x-1, top, x-1, H);
				}
			}

			if(!(local_mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)))
			{
				// get filter screen coordinates
				int filter_left_x = (int)((float)(filter_low-low)/(high-low)*W);
				int filter_right_x = (int)((float)(filter_high-low)/(high-low)*W);

				// make the filter display at least one pixel wide.
				if(filter_left_x == filter_right_x) filter_right_x = filter_left_x+1;

				// draw rx filter
				if(bottom)
				{
					g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
						filter_left_x, H+top, filter_right_x-filter_left_x, H+H-top);
				}
				else
				{
					g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
						filter_left_x, top, filter_right_x-filter_left_x, H-top);
				}
			}

			if(!local_mox && draw_tx_filter &&
				(rx1_dsp_mode != DSPMode.CWL && rx1_dsp_mode != DSPMode.CWU))
			{
				// get tx filter limits
				int filter_left_x; 
				int filter_right_x;

				if(tx_on_vfob)
				{
					if(!split_enabled)
					{
						filter_left_x = (int)((float)(tx_filter_low - low + xit_hz - rit_hz /*+ (vfob_hz - vfoa_hz)*/) / (high - low) * W);
						filter_right_x = (int)((float)(tx_filter_high - low + xit_hz - rit_hz /*+ (vfob_hz - vfoa_hz)*/) / (high - low) * W);
					}
					else
					{
						filter_left_x = (int)((float)(tx_filter_low - low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (high - low) * W);
						filter_right_x = (int)((float)(tx_filter_high - low + xit_hz - rit_hz + (vfob_sub_hz - vfoa_hz)) / (high - low) * W);
					}
				}
				else
				{
					if(!split_enabled)
					{
						filter_left_x = (int)((float)(tx_filter_low - low + xit_hz - rit_hz) / (high - low) * W);
						filter_right_x = (int)((float)(tx_filter_high - low + xit_hz - rit_hz) / (high - low) * W);
					}
					else
					{
						filter_left_x = (int)((float)(tx_filter_low - low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
						filter_right_x = (int)((float)(tx_filter_high - low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);
					}
				}
				
				if(bottom && tx_on_vfob)
				{
					g.DrawLine(tx_filter_pen, filter_left_x, H+top, filter_left_x, H+H);		// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_left_x+1, H+top, filter_left_x+1, H+H);	// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x, H+top, filter_right_x, H+H);	// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x+1, H+top, filter_right_x+1, H+H);// draw tx filter overlay
				}
				else if(!tx_on_vfob)
				{
					g.DrawLine(tx_filter_pen, filter_left_x, top, filter_left_x, H);		// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_left_x+1, top, filter_left_x+1, H);	// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x, top, filter_right_x, H);	// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x+1, top, filter_right_x+1, H);// draw tx filter overlay
				}
			}
		
			if(!local_mox && draw_tx_cw_freq &&
				(rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
			{
				int pitch = cw_pitch;
				if(rx1_dsp_mode == DSPMode.CWL)
					pitch = -cw_pitch;

				int cw_line_x;
				if(!split_enabled)
					cw_line_x = (int)((float)(pitch - low + xit_hz - rit_hz)/(high - low) * W);
				else
					cw_line_x = (int)((float)(pitch - low + xit_hz - rit_hz + (vfoa_sub_hz - vfoa_hz)) / (high - low) * W);

				if(bottom)
				{
					g.DrawLine(tx_filter_pen, cw_line_x, H+top, cw_line_x, H+H);
					g.DrawLine(tx_filter_pen, cw_line_x+1, H+top, cw_line_x+1, H+H);
				}
				else
				{
					g.DrawLine(tx_filter_pen, cw_line_x, top, cw_line_x, H);
					g.DrawLine(tx_filter_pen, cw_line_x+1, top, cw_line_x+1, H);
				}
			}

			double vfo;
			
			if(local_mox)
			{
				if(split_enabled)
					vfo = vfoa_sub_hz;
				else
					vfo = vfoa_hz;
				vfo += xit_hz;
			}
			else if(rx==1)
			{
				vfo = vfoa_hz + rit_hz;
			}
			else //if(rx==2)
			{
				vfo = vfob_hz + rit_hz;
			}

			switch(rx1_dsp_mode)
			{
				case DSPMode.CWL:
					vfo += cw_pitch;
					break;
				case DSPMode.CWU:
					vfo -= cw_pitch;
					break;
				default:
					break;
			}

			long vfo_round = ((long)(vfo/freq_step_size))*freq_step_size;
			long vfo_delta = (long)(vfo - vfo_round);

			int f_steps = (width/freq_step_size)+1;
			// Draw vertical lines
			for(int i=0; i<f_steps+1; i++)
			{
				string label;
				int offsetL;
				int offsetR;

				int fgrid = i*freq_step_size + (low/freq_step_size)*freq_step_size;
				double actual_fgrid = ((double)(vfo_round+fgrid))/1000000;
				int vgrid = (int)((double)(fgrid-vfo_delta-low)/(high-low)*W);

				if(!show_freq_offset)
				{
					if(actual_fgrid == 1.8 || actual_fgrid == 2.0 || 
						actual_fgrid == 3.5 ||	actual_fgrid == 4.0 || 
						actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
						actual_fgrid == 10.1 || actual_fgrid == 10.15 || 
						actual_fgrid == 14.0 || actual_fgrid == 14.35 || 
						actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
						actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
						actual_fgrid == 24.89 || actual_fgrid == 24.99 || 
						actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
						actual_fgrid == 28.0 || actual_fgrid == 29.7 || 
						actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
						actual_fgrid == 144.0 || actual_fgrid == 148.0)
					{
						if(bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H+top, vgrid, H+H);
						else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

						label = actual_fgrid.ToString("f3"); 
						if(actual_fgrid < 10) offsetL = (int)((label.Length+1)*4.1) - 14;
						else if(actual_fgrid < 100.0) offsetL = (int)((label.Length+1)*4.1) - 11;
						else offsetL = (int)((label.Length+1)*4.1) - 8;		

						if(bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid-offsetL, (float)Math.Floor(H*.01));

                        int fgrid_2 = ((i + 1) * freq_step_size) + (int)((low / freq_step_size) * freq_step_size);
                        int x_2 = (int)(((float)(fgrid_2 - vfo_delta - low) / width * W));
                        float scale = (float)(x_2 - vgrid) / inbetweenies;

                        for (int j = 1; j < inbetweenies; j++)
                        {
                            float x3 = (float)vgrid + (j * scale);
                            if (bottom) g.DrawLine(grid_pen_dark, x3, H + top, x3, H + H);
                            else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                        }
					}
					else
					{
						if(bottom) g.DrawLine(grid_pen, vgrid, H+top, vgrid, H+H);
						else g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

                        int fgrid_2 = ((i + 1) * freq_step_size) + (int)((low / freq_step_size) * freq_step_size);
                        int x_2 = (int)(((float)(fgrid_2 - vfo_delta - low) / width * W));
                        float scale = (float)(x_2 - vgrid) / inbetweenies;

                        for (int j = 1; j < inbetweenies; j++)
                        {
                            float x3 = (float)vgrid + (j * scale);
                            if (bottom) g.DrawLine(grid_pen_dark, x3, H+top, x3, H+H);
                            else g.DrawLine(grid_pen_dark, x3, top, x3, H);
                        }

						if ( ((double)((int)(actual_fgrid*1000))) == actual_fgrid * 1000) 
						{
							label = actual_fgrid.ToString("f3"); //wa6ahl
							
							//if(actual_fgrid > 1300.0)
							//	label = label.Substring(label.Length-4);

							if(actual_fgrid < 10) offsetL = (int)((label.Length+1)*4.1) - 14;
							else if(actual_fgrid < 100.0) offsetL = (int)((label.Length+1)*4.1) - 11;
							else offsetL = (int)((label.Length+1)*4.1) - 8;							
						}
						else 
						{
							string temp_string;
							int jper; 
							label = actual_fgrid.ToString("f4"); 
							temp_string = label;
							jper = label.IndexOf('.')+4;
							label = label.Insert(jper," ");

							//if(actual_fgrid > 1300.0)
							//	label = label.Substring(label.Length-4);

							if(actual_fgrid < 10) offsetL = (int)((label.Length)*4.1) - 14;
							else if(actual_fgrid < 100.0) offsetL = (int)((label.Length)*4.1) - 11;
							else offsetL = (int)((label.Length)*4.1) - 8;		
						}

						if(bottom) g.DrawString(label, font, grid_text_brush, vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, vgrid-offsetL, (float)Math.Floor(H*.01));
					}
				}
				else
				{
					vgrid = Convert.ToInt32((double)-(fgrid-low)/(low-high)*W);	//wa6ahl
					if(bottom) g.DrawLine(grid_pen, vgrid, H+top, vgrid, H+H);
					else g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

					double new_fgrid = (vfoa_hz + fgrid) / 1000000;

					label = fgrid.ToString(); 
					offsetL = (int)((label.Length+1)*4.1);
					offsetR = (int)(label.Length*4.1);
					if ((vgrid-offsetL >=0) && (vgrid+offsetR < W) && (fgrid != 0))
					{
						if(bottom) g.DrawString(label, font, grid_text_brush, vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, vgrid-offsetL, (float)Math.Floor(H*.01));
					}
				}
			}

			int[] band_edge_list = { 18068000, 18168000, 1800000, 2000000, 3500000, 4000000,
				7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 21000000, 21450000,
				24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 };
			
			for(int i=0; i<band_edge_list.Length; i++)
			{
				double band_edge_offset = band_edge_list[i] - vfo;
				if (band_edge_offset >= low && band_edge_offset <= high)
				{
					int temp_vline =  (int)((double)(band_edge_offset-low)/(high-low)*W);//wa6ahl
					if(bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H+top, temp_vline, H+H);//wa6ahl
					else g.DrawLine(new Pen(band_edge_color), temp_vline, top, temp_vline, H);//wa6ahl
				}
				//if(i == 1 && !show_freq_offset) break;
			}

			// Draw horizontal lines
			for(int i=1; i<h_steps; i++)
			{
				int xOffset = 0;
				int num = spectrum_grid_max - i*grid_step;
				int y = (int)((double)(spectrum_grid_max - num)*H/y_range);
				if(bottom) g.DrawLine(grid_pen, 0, H+y, W, H+y);
				else g.DrawLine(grid_pen, 0, y, W, y);

				// Draw horizontal line labels
				if(i != 1) // avoid intersecting vertical and horizontal labels
				{
					num = spectrum_grid_max - i*grid_step;
					string label = num.ToString();
					if(label.Length == 3)
						xOffset = (int)g.MeasureString("-", font).Width - 2;
					int offset = (int)(label.Length*4.1);
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = center_line_x+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width - 3);
							break;
						case DisplayLabelAlignment.AUTO:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;
					if(y+9 < H)
					{
						if(bottom) g.DrawString(label, font, grid_text_brush, x, H+y);
						else g.DrawString(label, font, grid_text_brush, x, y);
					}
				}
			}

			// Draw 0Hz vertical line if visible
			if(center_line_x >= 0 && center_line_x <= W)
			{
				if(bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x, H+top, center_line_x, H+H);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1, H+top, center_line_x+1, H+H);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x, top, center_line_x, H);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1, top, center_line_x+1, H);
				}
			}

			if(show_freq_offset)
			{
				if(bottom) g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, H+(float)Math.Floor(H*.01));
				else g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, (float)Math.Floor(H*.01));
			}

			if(high_swr && rx==1)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawWaterfallGrid(ref Graphics g, int W, int H, int rx, bool bottom)
		{
			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			int low = rx_display_low;					// initialize variables
			int high = rx_display_high;
			int mid_w = W/2;
			int[] step_list = {10, 20, 25, 50};
			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;

			System.Drawing.Font font = new System.Drawing.Font("Arial", 9);
			SolidBrush grid_text_brush = new SolidBrush(grid_text_color);
			Pen grid_pen = new Pen(grid_color);
			Pen tx_filter_pen = new Pen(display_filter_tx_color);
			int y_range = spectrum_grid_max - spectrum_grid_min;
			int filter_low, filter_high;

			int center_line_x = (int)(-(double)low/(high-low)*W);

			if(mox) // get filter limits
			{
				filter_low = tx_filter_low;
				filter_high = tx_filter_high;
			}
			else if(rx==1)
			{
				filter_low = rx1_filter_low;
				filter_high = rx1_filter_high;
			}
			else //if(rx==2)
			{
				filter_low = rx2_filter_low;
				filter_high = rx2_filter_high;
			}

			if((rx1_dsp_mode == DSPMode.DRM && rx==1) ||
				(rx2_dsp_mode == DSPMode.DRM && rx==2))
			{
				filter_low = -5000;
				filter_high = 5000;
			}

			// Calculate horizontal step size
			int width = high-low;
			while(width/freq_step_size > 10)
			{
				freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
				step_index = (step_index+1)%4;
				if(step_index == 0) step_power++;
			}
			double w_pixel_step = (double)W*freq_step_size/width;
			int w_steps = width/freq_step_size;

			// calculate vertical step size
			int h_steps = (spectrum_grid_max - spectrum_grid_min)/spectrum_grid_step;
			double h_pixel_step = (double)H/h_steps;
			int top = (int)((double)spectrum_grid_step*H/y_range);
			if(bottom) top *= 2;

			if(!bottom)
			{
				if(!mox && sub_rx1_enabled)
				{
					// draw Sub RX 0Hz line
					int x = (int)((float)(vfoa_sub_hz-vfoa_hz-low)/(high-low)*W);
					g.DrawLine(new Pen(sub_rx_zero_line_color), x, 0, x, top);
					g.DrawLine(new Pen(sub_rx_zero_line_color), x-1, 0, x-1, top);

					// draw Sub RX filter
					// get filter screen coordinates
					int filter_left_x = (int)((float)(filter_low-low+vfoa_sub_hz-vfoa_hz)/(high-low)*W);
					int filter_right_x = (int)((float)(filter_high-low+vfoa_sub_hz-vfoa_hz)/(high-low)*W);

					// make the filter display at least one pixel wide.
					if(filter_left_x == filter_right_x) filter_right_x = filter_left_x+1;

					// draw rx filter
					g.FillRectangle(new SolidBrush(sub_rx_filter_color),	// draw filter overlay
						filter_left_x, 0, filter_right_x-filter_left_x, top);
				}

				if(!(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU)))
				{
					// get filter screen coordinates
					int filter_left_x = (int)((float)(filter_low-low)/(high-low)*W);
					int filter_right_x = (int)((float)(filter_high-low)/(high-low)*W);

					// make the filter display at least one pixel wide.
					if(filter_left_x == filter_right_x) filter_right_x = filter_left_x+1;

					// draw rx filter
					g.FillRectangle(new SolidBrush(display_filter_color),	// draw filter overlay
						filter_left_x, 0, filter_right_x-filter_left_x, top);
				}

				if(!mox && draw_tx_filter &&
					(rx1_dsp_mode != DSPMode.CWL && rx1_dsp_mode != DSPMode.CWU))
				{
					// get tx filter limits
					int filter_left_x; 
					int filter_right_x;

					if(!split_enabled)
					{
						filter_left_x = (int)((float)(tx_filter_low-low+xit_hz)/(high-low)*W);
						filter_right_x = (int)((float)(tx_filter_high-low+xit_hz)/(high-low)*W);
					}
					else
					{
						filter_left_x = (int)((float)(tx_filter_low-low+xit_hz+(vfoa_sub_hz-vfoa_hz))/(high-low)*W);
						filter_right_x = (int)((float)(tx_filter_high-low+xit_hz+(vfoa_sub_hz-vfoa_hz))/(high-low)*W);
					}
				
					g.DrawLine(tx_filter_pen, filter_left_x, H, filter_left_x, H+top);		// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_left_x+1, H, filter_left_x+1, H+top);	// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x, H, filter_right_x, H+top);		// draw tx filter overlay
					g.DrawLine(tx_filter_pen, filter_right_x+1, H, filter_right_x+1, H+top);	// draw tx filter overlay
				}
		
				if(!mox && draw_tx_cw_freq &&
					(rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					int pitch = cw_pitch;
					if(rx1_dsp_mode == DSPMode.CWL)
						pitch = -cw_pitch;

					int cw_line_x;
					if(!split_enabled)
						cw_line_x = (int)((float)(pitch-low+xit_hz)/(high-low)*W);
					else
						cw_line_x = (int)((float)(pitch-low+xit_hz+(vfoa_sub_hz-vfoa_hz))/(high-low)*W);

					g.DrawLine(tx_filter_pen, cw_line_x, top, cw_line_x, H);
					g.DrawLine(tx_filter_pen, cw_line_x+1, top, cw_line_x+1, H);
				}
			}

			double vfo;
			
			if(mox)
			{
				if(split_enabled)
					vfo = vfoa_sub_hz;
				else
					vfo = vfoa_hz;
				vfo += xit_hz;
			}
			else if(rx==1)
			{
				vfo = vfoa_hz + rit_hz;
			}
			else //if(rx==2)
			{
				vfo = vfob_hz + rit_hz;
			}

			switch(rx1_dsp_mode)
			{
				case DSPMode.CWL:
					vfo += cw_pitch;
					break;
				case DSPMode.CWU:
					vfo -= cw_pitch;
					break;
				default:
					break;
			}

			long vfo_round = ((long)(vfo/freq_step_size))*freq_step_size;
			long vfo_delta = (long)(vfo - vfo_round);

			// Draw vertical lines
			for(int i=0; i<=h_steps+1; i++)
			{
				string label;
				int offsetL;
				int offsetR;

				int fgrid = i*freq_step_size + (low/freq_step_size)*freq_step_size;
				double actual_fgrid = ((double)(vfo_round+fgrid))/1000000;
				int vgrid = (int)((double)(fgrid-vfo_delta-low)/(high-low)*W);

				if(!show_freq_offset)
				{
					if(actual_fgrid == 1.8 || actual_fgrid == 2.0 || 
						actual_fgrid == 3.5 ||	actual_fgrid == 4.0 || 
						actual_fgrid == 7.0 || actual_fgrid == 7.3 ||
						actual_fgrid == 10.1 || actual_fgrid == 10.15 || 
						actual_fgrid == 14.0 || actual_fgrid == 14.35 || 
						actual_fgrid == 18.068 || actual_fgrid == 18.168 ||
						actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
						actual_fgrid == 24.89 || actual_fgrid == 24.99 || 
						actual_fgrid == 21.0 || actual_fgrid == 21.45 ||
						actual_fgrid == 28.0 || actual_fgrid == 29.7 || 
						actual_fgrid == 50.0 || actual_fgrid == 54.0 ||
						actual_fgrid == 144.0 || actual_fgrid == 148.0)
					{
						if(bottom) g.DrawLine(new Pen(band_edge_color), vgrid, H+top, vgrid, H+H);
						else g.DrawLine(new Pen(band_edge_color), vgrid, top, vgrid, H);

						label = actual_fgrid.ToString("f3"); 
						if(actual_fgrid < 10) offsetL = (int)((label.Length+1)*4.1) - 14;
						else if(actual_fgrid < 100.0) offsetL = (int)((label.Length+1)*4.1) - 11;
						else offsetL = (int)((label.Length+1)*4.1) - 8;		

						if(bottom) g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, new SolidBrush(band_edge_color), vgrid-offsetL, (float)Math.Floor(H*.01));
					}
					else
					{

						if (freq_step_size >= 2000)
						{
							double t100;
							double t1000;
							t100 = (actual_fgrid * 100);
							t1000 = (actual_fgrid * 1000);

							int it100 = (int)t100;
							int it1000 = (int)t1000;

							int it100x10 = it100 * 10;

							if ( it100x10 == it1000)
							{
							}
							else
							{
								grid_pen.DashStyle =  DashStyle.Dot;
							}
						}
						else
						{
							if (freq_step_size == 1000)
							{
								double t200;
								double t2000;
								t200 = (actual_fgrid * 200);
								t2000 = (actual_fgrid * 2000);

								int it200 = (int)t200;
								int it2000 = (int)t2000;

								int it200x10 = it200 * 10;

								if ( it200x10 == it2000)
								{
								}
								else
								{
									grid_pen.DashStyle =  DashStyle.Dot;
								}
							}
							else
							{
								double t1000;
								double t10000;
								t1000 = (actual_fgrid * 1000);
								t10000 = (actual_fgrid * 10000);

								int it1000 = (int)t1000;
								int it10000 = (int)t10000;

								int it1000x10 = it1000 * 10;
								
								if ( it1000x10 == it10000)
								{
								}
								else
								{
									grid_pen.DashStyle =  DashStyle.Dot;
								}
							}
						}
						if(bottom) g.DrawLine(grid_pen, vgrid, H+top, vgrid, H+H);
						else g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl
						grid_pen.DashStyle =  DashStyle.Solid;

						if ( ((double)((int)(actual_fgrid*1000))) == actual_fgrid * 1000) 
						{
							label = actual_fgrid.ToString("f3"); //wa6ahl
							
							//if(actual_fgrid > 1300.0)
							//	label = label.Substring(label.Length-4);

							if(actual_fgrid < 10) offsetL = (int)((label.Length+1)*4.1) - 14;
							else if(actual_fgrid < 100.0) offsetL = (int)((label.Length+1)*4.1) - 11;
							else offsetL = (int)((label.Length+1)*4.1) - 8;							
						}
						else 
						{
							string temp_string;
							int jper; 
							label = actual_fgrid.ToString("f4"); 
							temp_string = label;
							jper = label.IndexOf('.')+4;
							label = label.Insert(jper," ");

							//if(actual_fgrid > 1300.0)
							//	label = label.Substring(label.Length-4);

							if(actual_fgrid < 10) offsetL = (int)((label.Length)*4.1) - 14;
							else if(actual_fgrid < 100.0) offsetL = (int)((label.Length)*4.1) - 11;
							else offsetL = (int)((label.Length)*4.1) - 8;		
						}

						if(bottom) g.DrawString(label, font, grid_text_brush, vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, vgrid-offsetL, (float)Math.Floor(H*.01));
					}
				}
				else
				{
					vgrid = Convert.ToInt32((double)-(fgrid-low)/(low-high)*W);	//wa6ahl
					if(bottom) g.DrawLine(grid_pen, vgrid, H+top, vgrid, H+H);
					else g.DrawLine(grid_pen, vgrid, top, vgrid, H);			//wa6ahl

					double new_fgrid = (vfoa_hz + fgrid) / 1000000;

					label = fgrid.ToString(); 
					offsetL = (int)((label.Length+1)*4.1);
					offsetR = (int)(label.Length*4.1);
					if ((vgrid-offsetL >=0) && (vgrid+offsetR < W) && (fgrid != 0))
					{
						if(bottom) g.DrawString(label, font, grid_text_brush, vgrid-offsetL, H+(float)Math.Floor(H*.01));
						else g.DrawString(label, font, grid_text_brush, vgrid-offsetL, (float)Math.Floor(H*.01));
					}
				}
			}

			int[] band_edge_list = { 18068000, 18168000, 1800000, 2000000, 3500000, 4000000,
									   7000000, 7300000, 10100000, 10150000, 14000000, 14350000, 21000000, 21450000,
									   24890000, 24990000, 28000000, 29700000, 50000000, 54000000, 144000000, 148000000 };
			
			for(int i=0; i<band_edge_list.Length; i++)
			{
				double band_edge_offset = band_edge_list[i] - vfo;
				if (band_edge_offset >= low && band_edge_offset <= high)
				{
					int temp_vline =  (int)((double)(band_edge_offset-low)/(high-low)*W);//wa6ahl
					if(bottom) g.DrawLine(new Pen(band_edge_color), temp_vline, H, temp_vline, H+top);//wa6ahl
					else g.DrawLine(new Pen(band_edge_color), temp_vline, 0, temp_vline, top);
				}
				if(i == 1 && !show_freq_offset) break;
			}

			/*// Draw horizontal lines
			for(int i=1; i<h_steps; i++)
			{
				int xOffset = 0;
				int num = spectrum_grid_max - i*spectrum_grid_step;
				int y = (int)((double)(spectrum_grid_max - num)*H/y_range);
				g.DrawLine(grid_pen, 0, y, W, y);

				// Draw horizontal line labels
				if(i != 1) // avoid intersecting vertical and horizontal labels
				{
					num = spectrum_grid_max - i*spectrum_grid_step;
					string label = num.ToString();
					if(label.Length == 3) xOffset = 7;
					//int offset = (int)(label.Length*4.1);
					if(display_label_align != DisplayLabelAlignment.LEFT &&
						display_label_align != DisplayLabelAlignment.AUTO &&
						(current_dsp_mode == DSPMode.USB ||
						current_dsp_mode == DSPMode.CWU))
						xOffset -= 32;
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = center_line_x+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width);
							break;
						case DisplayLabelAlignment.AUTO:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;
					if(y+9 < H)
						g.DrawString(label, font, grid_text_brush, x, y);
				}
			}*/

			// Draw 0Hz vertical line if visible
			if(center_line_x >= 0 && center_line_x <= W)
			{
				if(!bottom)
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x, 0, center_line_x, top);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1, 0, center_line_x+1, top);
				}
				else
				{
					g.DrawLine(new Pen(grid_zero_color), center_line_x, H, center_line_x, H+top);
					g.DrawLine(new Pen(grid_zero_color), center_line_x+1, H, center_line_x+1, H+top);
				}
			}

			if(show_freq_offset)
			{
				if(bottom) g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, H+(float)Math.Floor(H*.01));
				else g.DrawString("0", font, new SolidBrush(grid_zero_color), center_line_x-5, (float)Math.Floor(H*.01));
			}

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static void DrawOffBackground(Graphics g, int W, int H, bool bottom)
		{
			// draw background
			if(bottom) g.FillRectangle(new SolidBrush(display_background_color), 0, H, W, H);
			else g.FillRectangle(new SolidBrush(display_background_color), 0, 0, W, H);

			if(high_swr && !bottom)
				g.DrawString("High SWR", new System.Drawing.Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Red), 245, 20);
		}

		private static float[] scope_min = new float[W];
		public static float[] ScopeMin
		{
			get { return scope_min; }
			set { scope_min = value; }
		}
		private static float[] scope_max = new float[W];
		public static float[] ScopeMax
		{
			get { return scope_max; }
			set { scope_max = value; }
		}
		unsafe private static bool DrawScope(Graphics g, int W, int H, bool bottom)
		{
			if(scope_min.Length < W) 
			{
				scope_min = new float[W];
				Audio.ScopeMin = scope_min;
			}
			if(scope_max.Length < W)
			{
				scope_max = new float[W];
				Audio.ScopeMax = scope_max;
			}

			DrawScopeGrid(ref g, W, H, bottom);

			Point[] points = new Point[W*2];			// create Point array
			for(int i=0; i<W; i++)						// fill point array
			{	
				int pixel = 0;
				if(bottom) pixel = (int)(H/2 * scope_max[i]);
				else pixel = (int)(H/2 * scope_max[i]);
				int y = H/2 - pixel;
				points[i].X = i;
				points[i].Y = y;
				if(bottom) points[i].Y += H;

				if(bottom) pixel = (int)(H/2 * scope_min[i]);
				else pixel = (int)(H/2 * scope_min[i]);
				y = H/2 - pixel;
				points[W*2-1-i].X = i;
				points[W*2-1-i].Y = y;
				if(bottom)points[W*2-1-i].Y += H;
				//if(points[W*2-1-i].Y == points[i].Y)
				//	points[W*2-1-i].Y += 1;
			}

			// draw the connected points
			g.DrawLines(data_line_pen, points);
			g.FillPolygon(new SolidBrush(data_line_pen.Color), points);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom) g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H+H);
				else g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		unsafe private static bool DrawPhase(Graphics g, int W, int H, bool bottom)
		{
			DrawPhaseGrid(ref g, W, H, bottom);
			int num_points = phase_num_pts;

			if(!bottom && data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				// get new data
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready_bottom = false;
			}

			Point[] points = new Point[num_points];		// declare Point array
			for(int i=0,j=0; i<num_points; i++,j+=8)	// fill point array
			{
				int x = 0;
				int y = 0;
				if(bottom)
				{
					x = (int)(current_display_data_bottom[i*2]*H/2);
					y = (int)(current_display_data_bottom[i*2+1]*H/2);
				}
				else
				{
					x = (int)(current_display_data[i*2]*H/2);
					y = (int)(current_display_data[i*2+1]*H/2);
				}
				points[i].X = W/2+x;
				points[i].Y = H/2+y;
				if(bottom) points[i].Y += H;
			}
			
			// draw each point
			for(int i=0; i<num_points; i++)
				g.DrawRectangle(data_line_pen, points[i].X, points[i].Y, 1, 1);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		unsafe private static void DrawPhase2(Graphics g, int W, int H, bool bottom)
		{
			DrawPhaseGrid(ref g, W, H, bottom);
			int num_points = phase_num_pts;

			if(!bottom && data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				// get new data
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
				data_ready_bottom = false;
			}

			Point[] points = new Point[num_points];		// declare Point array
			for(int i=0; i<num_points; i++)	// fill point array
			{
				int x = 0;
				int y = 0;
				if(bottom)
				{
					x = (int)(current_display_data_bottom[i*2]*H*0.5*500);
					y = (int)(current_display_data_bottom[i*2+1]*H*0.5*500);
				}
				else
				{
					x = (int)(current_display_data[i*2]*H*0.5*500);
					y = (int)(current_display_data[i*2+1]*H*0.5*500);
				}
				points[i].X = (int)(W*0.5+x);
				points[i].Y = (int)(H*0.5+y);
				if(bottom) points[i].Y += H;
			}
			
			// draw each point
			for(int i=0; i<num_points; i++)
				g.DrawRectangle(data_line_pen, points[i].X, points[i].Y, 1, 1);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom) g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				else g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}
		}

		private static Point[] points;
		unsafe static private bool DrawSpectrum(Graphics g, int W, int H, bool bottom)
		{
			DrawSpectrumGrid(ref g, W, H, bottom);
			if(points == null || points.Length < W) 
				points = new Point[W];			// array of points to display
			float slope = 0.0f;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int low = 0;
			int high = 0;
			float local_max_y = float.MinValue;

			if(!mox)
			{
				low = rx_display_low;
				high = rx_display_high;
			}
			else
			{
				low = tx_display_low;
				high = tx_display_high;
			}

			if(rx1_dsp_mode == DSPMode.DRM)
			{
				low = 2500;
				high = 21500;
			}

			int yRange = spectrum_grid_max - spectrum_grid_min;

			if(!bottom && data_ready)
			{
				if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data.Length; i++)
						current_display_data[i] = -200.0f;
				}
				else
				{
					fixed(void *rptr = &new_display_data[0])
						fixed(void *wptr = &current_display_data[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;
			}
			else if(bottom && data_ready_bottom)
			{
				if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data_bottom.Length; i++)
						current_display_data_bottom[i] = -200.0f;
				}
				else
				{
					fixed(void *rptr = &new_display_data_bottom[0])
						fixed(void *wptr = &current_display_data_bottom[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);
				}
				data_ready_bottom = false;
			}

			if(!bottom && average_on)
				console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
			else if(bottom && rx2_avg_on)
				console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);

			if(!bottom && peak_on)
				UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
			else if(bottom && rx2_peak_on)
				UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);

			start_sample_index = (BUFFER_SIZE>>1) + (int)((low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);

			if (start_sample_index < 0) start_sample_index = 0;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE - start_sample_index;

			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				if(!bottom)
				{
					if (slope <= 1) 
						max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
					else 
					{
						int rindex = (int)Math.Floor(dval + slope);
						if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
						for(int j=lindex;j<rindex;j++)
							if (current_display_data[j] > max) max=current_display_data[j];
					}
				}
				else
				{
					if (slope <= 1) 
						max =  current_display_data_bottom[lindex]*((float)lindex-dval+1) + current_display_data_bottom[lindex+1]*(dval-(float)lindex);
					else 
					{
						int rindex = (int)Math.Floor(dval + slope);
						if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
						for(int j=lindex;j<rindex;j++)
							if (current_display_data_bottom[j] > max) max=current_display_data_bottom[j];
					}
				}

				if(!bottom) max += rx1_display_cal_offset;
				else max += rx2_display_cal_offset;

				if(!mox)
				{
					if(!bottom)	max += rx1_preamp_offset;
					else max += rx2_preamp_offset;
				}

				if(max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}

				points[i].X = i;
                points[i].Y = (int)Math.Min((Math.Floor((spectrum_grid_max - max) * H / yRange)), H);
				if(bottom) points[i].Y += H;
			}

			max_y = local_max_y;

			g.DrawLines(data_line_pen, points);

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom)
				{
					g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
					g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
				}
				else
				{
					g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
					g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
				}
			}

			return true;
		}

		unsafe static private bool DrawPanadapter(Graphics g, int W, int H, int rx, bool bottom)
		{
			DrawPanadapterGrid(ref g, W, H, rx, bottom);
            if (pan_fill)
            {
                if (points == null || points.Length < W + 2)
                    points = new Point[W + 2];
            }
            else
            {
                if (points == null || points.Length < W)
                    points = new Point[W];			// array of points to display
            }
			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int Low = rx_display_low;
			int High = rx_display_high;
			int yRange = spectrum_grid_max - spectrum_grid_min;
			float local_max_y = float.MinValue;

			if(rx1_dsp_mode == DSPMode.DRM)
			{
				Low += 12000;
				High += 12000;
			}

			if(rx == 1 && data_ready)
			{
				if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data.Length; i++)
						current_display_data[i] = -200.0f;
				}
				else
				{
					fixed(void *rptr = &new_display_data[0])
						fixed(void *wptr = &current_display_data[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;
			}
			else if(rx == 2 && data_ready_bottom)
			{
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

				if ( current_model == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);

				data_ready_bottom = false;
			}

			if(rx==1 && average_on)
				console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
			else if(rx==2 && rx2_avg_on)
				console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);

			if(rx==1 && peak_on)
				UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
			else if(rx==2 && rx2_peak_on)
				UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);

			num_samples = (High - Low);

			start_sample_index = (BUFFER_SIZE>>1) +(int)((Low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);
			if (start_sample_index < 0) start_sample_index += 4096;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE-start_sample_index;

			//Debug.WriteLine("start_sample_index: "+start_sample_index);
			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				int rindex = (int)Math.Floor(dval + slope);

				if(rx == 1)
				{
					if (slope <= 1.0 || lindex == rindex) 
					{
						max =  current_display_data[lindex%4096]*((float)lindex-dval+1) + current_display_data[(lindex+1)%4096]*(dval-(float)lindex);
					}
					else 
					{					
						for(int j=lindex; j<rindex; j++)
							if (current_display_data[j%4096] > max) max=current_display_data[j%4096];
					}
				}
				else if(rx == 2)
				{
					if (slope <= 1.0 || lindex == rindex) 
					{
						max =  current_display_data_bottom[lindex%4096]*((float)lindex-dval+1) + current_display_data_bottom[(lindex+1)%4096]*(dval-(float)lindex);
					}
					else 
					{					
						for(int j=lindex; j<rindex; j++)
							if (current_display_data_bottom[j%4096] > max) max=current_display_data_bottom[j%4096];
					}
				}

				if(rx==1) max += rx1_display_cal_offset;
				else if(rx==2) max += rx2_display_cal_offset;

				if(!mox) 
				{
					if(rx==1) max += rx1_preamp_offset;
					else if(rx==2) max += rx2_preamp_offset;
				}

				if(max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}

				points[i].X = i;
				points[i].Y = (int)(Math.Floor((spectrum_grid_max - max)*H/yRange));
				if(bottom) points[i].Y += H;
			} 
			
			max_y = local_max_y;

            if (pan_fill)
            {
                points[W].X = W; points[W].Y = H;
                points[W+1].X = 0; points[W+1].Y = H;
                if (bottom)
                {
                    points[W].Y += H;
                    points[W+1].Y += H;
                }
                data_line_pen.Color = Color.FromArgb(100, 255, 255, 255);
                g.FillPolygon(data_line_pen.Brush, points);
                points[W] = points[W-1];
                points[W+1] = points[W-1];
                data_line_pen.Color = data_line_color;
                g.DrawLines(data_line_pen, points);                
            }
            else g.DrawLines(data_line_pen, points);
			
			points = null;

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				if(bottom)
				{
					if(display_cursor_y > H)
					{
						g.DrawLine(p, display_cursor_x, H, display_cursor_x, H+H);
						g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
					}
				}
				else
				{
					if(display_cursor_y <= H)
					{
						g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
						g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
					}
				}
			}

			return true;
		}

		private static int waterfall_update_period = 100; // in ms
		public static int WaterfallUpdatePeriod
		{
			get { return waterfall_update_period; }
			set { waterfall_update_period = value; }
		}

		private static HiPerfTimer timer_waterfall = new HiPerfTimer();
		private static HiPerfTimer timer_waterfall2 = new HiPerfTimer();
		private static float[] waterfall_data;
		unsafe static private bool DrawWaterfall(Graphics g, int W, int H, int rx, bool bottom)
		{
			DrawWaterfallGrid(ref g, W, H, rx, bottom);
			if(waterfall_data == null || waterfall_data.Length < W)
				waterfall_data = new float[W];		// array of points to display
			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int Low = rx_display_low;
			int High = rx_display_high;
			int yRange = spectrum_grid_max - spectrum_grid_min;
			float local_max_y = float.MinValue;
			
			if((rx1_dsp_mode == DSPMode.DRM && rx==1) ||
				(rx2_dsp_mode == DSPMode.DRM && rx==2))
			{
				Low += 12000;
				High += 12000;
			}

			if(rx==1 && data_ready)
			{
				if(mox && (rx1_dsp_mode == DSPMode.CWL || rx1_dsp_mode == DSPMode.CWU))
				{
					for(int i=0; i<current_display_data.Length; i++)
						current_display_data[i] = -200.0f;
				}
				else
				{
					fixed(void *rptr = &new_display_data[0])
						fixed(void *wptr = &current_display_data[0])
							Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

					if ( current_model == Model.SOFTROCK40 ) 
						console.AdjustDisplayDataForBandEdge(ref current_display_data);
				}
				data_ready = false;
			}
			else if(rx==2 && data_ready_bottom)
			{
				fixed(void *rptr = &new_display_data_bottom[0])
					fixed(void *wptr = &current_display_data_bottom[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));

				if ( current_model == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data_bottom);

				data_ready_bottom = false;
			}

			if(rx==1)
				console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
			else if(rx==2)
				console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);
			
			if(rx==1 && peak_on)
				UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
			else if(rx==2 && rx2_peak_on)
				UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);

			int duration = 0;
			if(rx == 1)
			{
				timer_waterfall.Stop();
				duration = (int)timer_waterfall.DurationMsec;
			}
			else if(rx == 2)
			{
				timer_waterfall2.Stop();
				duration = (int)timer_waterfall2.DurationMsec;
			}

			if(duration > waterfall_update_period)
			{
				if(rx == 1) timer_waterfall.Start();
				else if(rx == 2) timer_waterfall2.Start();
				num_samples = (High - Low);

				start_sample_index = (BUFFER_SIZE>>1) +(int)((Low * BUFFER_SIZE) / sample_rate);
				num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);
				if (start_sample_index < 0) start_sample_index += 4096;
				if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
					num_samples = BUFFER_SIZE-start_sample_index;

				slope = (float)num_samples/(float)W;
				for(int i=0; i<W; i++)
				{
					float max = float.MinValue;
					float dval = i*slope + start_sample_index;
					int lindex = (int)Math.Floor(dval);
					int rindex = (int)Math.Floor(dval + slope);

					if(rx==1)
					{
						if (slope <= 1.0 || lindex == rindex) 
						{
							max =  current_display_data[lindex%4096]*((float)lindex-dval+1) + current_display_data[(lindex+1)%4096]*(dval-(float)lindex);
						}
						else 
						{					
							for(int j=lindex; j<rindex; j++)
								if (current_display_data[j%4096] > max) max=current_display_data[j%4096];
						}
					}
					else if(rx==2)
					{
						if (slope <= 1.0 || lindex == rindex) 
						{
							max =  current_display_data_bottom[lindex%4096]*((float)lindex-dval+1) + current_display_data_bottom[(lindex+1)%4096]*(dval-(float)lindex);
						}
						else 
						{					
							for(int j=lindex; j<rindex; j++)
								if (current_display_data_bottom[j%4096] > max) max=current_display_data_bottom[j%4096];
						}
					}

					if(rx==1) max += rx1_display_cal_offset;
					else if(rx==2) max += rx2_display_cal_offset;

					if(!mox) 
					{
						if(rx==1) max += rx1_preamp_offset;
						else if(rx==2) max += rx2_preamp_offset;
					}

					if(max > local_max_y)
					{
						local_max_y = max;
						max_x = i;
					}

					waterfall_data[i] = max;
				}

				max_y = local_max_y;

				BitmapData bitmapData;
				if(rx == 1)
				{
					bitmapData = waterfall_bmp.LockBits(
						new Rectangle(0, 0, waterfall_bmp.Width, waterfall_bmp.Height),
						ImageLockMode.ReadWrite,
						waterfall_bmp.PixelFormat);
				}
				else
				{
					bitmapData = waterfall_bmp2.LockBits(
						new Rectangle(0, 0, waterfall_bmp2.Width, waterfall_bmp2.Height),
						ImageLockMode.ReadWrite,
						waterfall_bmp2.PixelFormat);
				}

				int pixel_size = 3;
				byte* row = null;

				// first scroll image
				int total_size = bitmapData.Stride * bitmapData.Height;		// find buffer size
				Win32.memcpy(new IntPtr((int)bitmapData.Scan0+bitmapData.Stride).ToPointer(),
					bitmapData.Scan0.ToPointer(),
					total_size-bitmapData.Stride);

				row = (byte *)bitmapData.Scan0;
				
				// draw new data
				for(int i=0; i<W; i++)	// for each pixel in the new line
				{
					int R, G, B;		// variables to save Red, Green and Blue component values

					/*if(waterfall_data[i] <= waterfall_low_threshold)		// if less than low threshold, just use low color
					{
						R = waterfall_low_color.R;
						G = waterfall_low_color.G;
						B = waterfall_low_color.B;
					}
					else if(waterfall_data[i] >= waterfall_high_threshold)// if more than high threshold, just use high color
					{
						R = waterfall_high_color.R;
						G = waterfall_high_color.G;
						B = waterfall_high_color.B;
					}
					else // use a color between high and low
					{
						float percent = (waterfall_data[i] - waterfall_low_threshold)/(waterfall_high_threshold - waterfall_low_threshold);
						if(percent <= 0.5)	// use a gradient between low and mid colors
						{
							percent *= 2;

							R = (int)((1-percent)*waterfall_low_color.R + percent*waterfall_mid_color.R);
							G = (int)((1-percent)*waterfall_low_color.G + percent*waterfall_mid_color.G);
							B = (int)((1-percent)*waterfall_low_color.B + percent*waterfall_mid_color.B);
						}
						else				// use a gradient between mid and high colors
						{
							percent = (float)(percent-0.5)*2;

							R = (int)((1-percent)*waterfall_mid_color.R + percent*waterfall_high_color.R);
							G = (int)((1-percent)*waterfall_mid_color.G + percent*waterfall_high_color.G);
							B = (int)((1-percent)*waterfall_mid_color.B + percent*waterfall_high_color.B);
						}
					}*/
				
					//waterfall_data[i] = (float)i/W*(waterfall_high_threshold - waterfall_low_threshold) + waterfall_low_threshold;

					if(waterfall_data[i] <= waterfall_low_threshold)
					{
						R = waterfall_low_color.R;
						G = waterfall_low_color.G;
						B = waterfall_low_color.B;
					}
					else if(waterfall_data[i] >= waterfall_high_threshold)
					{
						R = 192;
						G = 124;
						B = 255;
					}
					else // value is between low and high
					{
						float range = waterfall_high_threshold - waterfall_low_threshold;
						float offset = waterfall_data[i] - waterfall_low_threshold;
						float overall_percent = offset / range; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

						if(overall_percent < (float)2/9) // background to blue
						{
							float local_percent = overall_percent / ((float)2/9);
							R = (int)((1.0-local_percent)*waterfall_low_color.R);
							G = (int)((1.0-local_percent)*waterfall_low_color.G);
							B = (int)(waterfall_low_color.B + local_percent*(255-waterfall_low_color.B));
						}
						else if(overall_percent < (float)3/9) // blue to blue-green
						{
							float local_percent = (overall_percent - (float)2/9) / ((float)1/9);
							R = 0;
							G = (int)(local_percent*255);
							B = 255;
						}
						else if(overall_percent < (float)4/9) // blue-green to green
						{
							float local_percent = (overall_percent - (float)3/9) / ((float)1/9);
							R = 0;
							G = 255;
							B = (int)((1.0-local_percent)*255);
						}
						else if(overall_percent < (float)5/9) // green to red-green
						{
							float local_percent = (overall_percent - (float)4/9) / ((float)1/9);
							R = (int)(local_percent*255);
							G = 255;
							B = 0;
						}
						else if(overall_percent < (float)7/9) // red-green to red
						{
							float local_percent = (overall_percent - (float)5/9) / ((float)2/9);
							R = 255;
							G = (int)((1.0-local_percent)*255);
							B = 0;
						}
						else if(overall_percent < (float)8/9) // red to red-blue
						{
							float local_percent = (overall_percent - (float)7/9) / ((float)1/9);
							R = 255;
							G = 0;
							B = (int)(local_percent*255);
						}
						else // red-blue to purple end
						{
							float local_percent = (overall_percent - (float)8/9) / ((float)1/9);
							R = (int)((0.75 + 0.25*(1.0-local_percent))*255);
							G = (int)(local_percent*255*0.5);
							B = 255;
						}
					}

					// set pixel color
					row[i*pixel_size + 0] = (byte)B;	// set color in memory
					row[i*pixel_size + 1] = (byte)G;
					row[i*pixel_size + 2] = (byte)R;
				}
				if(rx == 1)
					waterfall_bmp.UnlockBits(bitmapData);
				else
					waterfall_bmp2.UnlockBits(bitmapData);
			}

			if(bottom) 
			{
				if(rx == 1) g.DrawImageUnscaled(waterfall_bmp, 0, H+16);
				else if(rx == 2) g.DrawImageUnscaled(waterfall_bmp2, 0, H+16);
			}
			else
			{
				if(rx == 1) g.DrawImageUnscaled(waterfall_bmp, 0, 16);	// draw the image on the background	
				else if(rx == 2) g.DrawImageUnscaled(waterfall_bmp2, 0, 16);	// draw the image on the background	
			}

			waterfall_counter++;

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		unsafe static private bool DrawHistogram(Graphics g, int W, int H)
		{
			DrawSpectrumGrid(ref g, W, H, false);
			if(points == null || points.Length < W) 
				points = new Point[W];			// array of points to display
			float slope = 0.0F;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int low = 0;
			int high = 0;
			float local_max_y = Int32.MinValue;

			if(!mox)								// Receive Mode
			{
				low = rx_display_low;
				high = rx_display_high;
			}
			else									// Transmit Mode
			{
				low = tx_display_low;
				high = tx_display_high;
			}

			if(rx1_dsp_mode == DSPMode.DRM)
			{
				low = 2500;
				high = 21500;
			}

			int yRange = spectrum_grid_max - spectrum_grid_min;

			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				// kb9yig sr mod starts 
				if ( current_model == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data);
				// end kb9yig sr mods 

				data_ready = false;
			}

			if(average_on)
			{
				//if(!bottom)
					console.UpdateRX1DisplayAverage(rx1_average_buffer, current_display_data);
				/*else
					console.UpdateRX2DisplayAverage(rx2_average_buffer, current_display_data_bottom);*/
			}
			if(peak_on)
			{
				//if(!bottom)
					UpdateDisplayPeak(rx1_peak_buffer, current_display_data);
				/*else
					UpdateDisplayPeak(rx2_peak_buffer, current_display_data_bottom);*/
			}

			num_samples = (high - low);

			start_sample_index = (BUFFER_SIZE>>1) +(int)((low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);
			if (start_sample_index < 0) start_sample_index = 0;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE-start_sample_index;

			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				if (slope <= 1) 
					max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
				else 
				{
					int rindex = (int)Math.Floor(dval + slope);
					if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
					for(int j=lindex;j<rindex;j++)
						if (current_display_data[j] > max) max=current_display_data[j];

				}

				max += rx1_display_cal_offset;
				if(!mox) max += rx1_preamp_offset;

				switch(rx1_dsp_mode)
				{
					case DSPMode.SPEC:
						max += 6.0F;
						break;
				}
				if(max > local_max_y)
				{
					local_max_y = max;
					max_x = i;
				}

				points[i].X = i;
                points[i].Y = (int)Math.Min((Math.Floor((spectrum_grid_max - max) * H / yRange)), H);
			} 

			max_y = local_max_y;

			// get the average
			float avg = 0.0F;
			int sum = 0;
			foreach(Point p in points)
				sum += p.Y;

			avg = (float)((float)sum/points.Length / 1.12);

			for(int i=0; i<W; i++)
			{
				if(points[i].Y < histogram_data[i])
				{
					histogram_history[i] = 0;
					histogram_data[i] = points[i].Y;
				}
				else
				{
					histogram_history[i]++;
					if(histogram_history[i] > 51)
					{
						histogram_history[i] = 0;
						histogram_data[i] = points[i].Y;
					}

					int alpha = (int)Math.Max(255-histogram_history[i]*5, 0);
					Color c = Color.FromArgb(alpha, 0, 255, 0);
					int height = points[i].Y-histogram_data[i];
					g.DrawRectangle(new Pen(c), i, histogram_data[i], 1, height);
				}

				if(points[i].Y >= avg)		// value is below the average
				{
					Color c = Color.FromArgb(150, 0, 0, 255);
					g.DrawRectangle(new Pen(c), points[i].X, points[i].Y, 1, H-points[i].Y);
				}
				else 
				{
					g.DrawRectangle(new Pen(Color.FromArgb(150, 0, 0, 255)), points[i].X, (int)Math.Floor(avg), 1, H-(int)Math.Floor(avg));
					g.DrawRectangle(new Pen(Color.FromArgb(150, 255, 0, 0)), points[i].X, points[i].Y, 1, (int)Math.Floor(avg)-points[i].Y);
				}
			}

			// draw long cursor
			if(current_click_tune_mode != ClickTuneMode.Off)
			{
				Pen p;
				if(current_click_tune_mode == ClickTuneMode.VFOA)
					p = new Pen(grid_text_color);
				else p = new Pen(Color.Red);
				g.DrawLine(p, display_cursor_x, 0, display_cursor_x, H);
				g.DrawLine(p, 0, display_cursor_y, W, display_cursor_y);
			}

			return true;
		}

		public static void ResetRX1DisplayAverage()
		{
			rx1_average_buffer[0] = CLEAR_FLAG;	// set reset flag
		}

		public static void ResetRX2DisplayAverage()
		{
			rx2_average_buffer[0] = CLEAR_FLAG;	// set reset flag
		}

		public static void ResetRX1DisplayPeak()
		{
			rx1_peak_buffer[0] = CLEAR_FLAG; // set reset flag
		}

		public static void ResetRX2DisplayPeak()
		{
			rx2_peak_buffer[0] = CLEAR_FLAG; // set reset flag
		}

		#endregion

		#endregion

		#region DirectX
/*
		#region Variable Declaration

		private static Device dx_device = null;
		private static VertexBuffer dx_data_vb = null;
		private static VertexBuffer dx_mouse_vb = null;

		private static Background current_background;
		private static Microsoft.DirectX.Direct3D.Font bg_string_font;

		private static float[] data = null;
		private static CustomVertex.TransformedColored[] verts = null;
		
		#endregion

		#region Properties

		private static RenderType directx_render_type = RenderType.NONE;
		public static RenderType DirectXRenderType
		{
			get { return directx_render_type; }
			set { directx_render_type = value; }
		}

		#endregion

		#region Routines

		public static bool DirectXInit()
		{
			// Now let's setup our D3D stuff
			PresentParameters presentParams = new PresentParameters();
			presentParams.Windowed = true;
			presentParams.SwapEffect = SwapEffect.Discard;
			try
			{
				dx_device = new Device(0,
					DeviceType.Hardware, 
					target, 
					CreateFlags.HardwareVertexProcessing | CreateFlags.FpuPreserve, 
					presentParams);
				directx_render_type = RenderType.HARDWARE;
			}
			catch(DirectXException)
			{
				try
				{
					dx_device = new Device(0, 
						DeviceType.Hardware, 
						target, 
						CreateFlags.SoftwareVertexProcessing | CreateFlags.FpuPreserve, 
						presentParams);
					directx_render_type = RenderType.SOFTWARE;
				}
				catch(DirectXException)
				{
					directx_render_type = RenderType.NONE;
					return false;
				}
			}
			OnCreateDirectXDevice(dx_device, null);
			dx_device.RenderState.Lighting = false;
			bg_string_font = new Microsoft.DirectX.Direct3D.Font(
				dx_device, new System.Drawing.Font("Arial", 9.0f));
			
			return true;
		}

		public static void DirectXRelease()
		{
			dx_device = null;
			dx_data_vb = null;
			dx_mouse_vb = null;

			current_background = null;
			bg_string_font = null;

			data = null;
			verts = null;
		}
		
		public static void PrepareDisplayVars(DisplayMode mode)
		{
			switch(console.CurrentDisplayEngine)
			{
				case DisplayEngine.DIRECT_X:
					int data_length = 0;
					int num_verts = 0;
					
					switch(mode)
					{
						case DisplayMode.PANADAPTER:
						case DisplayMode.SPECTRUM:
						case DisplayMode.SCOPE:
							data_length = W;
							num_verts = W;
							break;
					}

					verts = new CustomVertex.TransformedColored[num_verts];
					data = new float[data_length];
					break;
			}
		}

		public static void OnCreateDirectXDevice(object sender, EventArgs e)
		{
			Device dev = (Device)sender;

			dx_data_vb = new VertexBuffer(
				typeof(CustomVertex.TransformedColored),
				W,
				dev,
				Usage.WriteOnly,
				CustomVertex.TransformedColored.Format,
				Pool.Managed);

			dx_mouse_vb = new VertexBuffer(
				typeof(CustomVertex.TransformedColored),
				4,
				dev,
				Usage.WriteOnly,
				CustomVertex.TransformedColored.Format,
				Pool.Managed);
		}

		public static void UpdateDataVertexBuffer(object sender, float[] data)
		{
			VertexBuffer vb = (VertexBuffer)sender;
			//CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[W];

			int color = data_line_color.ToArgb();
			for(int i=0; i<W; i++)
			{
				verts[i].X = i;
				verts[i].Y = data[i];
				verts[i].Z = 0.0f;
				verts[i].Rhw = 1; 
				verts[i].Color = color;
			}

			//GraphicsStream stm = vb.Lock(0, 0, 0);
			//stm.Write(verts);
			//vb.Unlock();
			vb.SetData(verts, 0, LockFlags.None);
		}

		public static void UpdateDataVertexBuffer(object sender, Point[] data)
		{
			VertexBuffer vb = (VertexBuffer)sender;
			CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[data.Length];

			for(int i=0; i<data.Length; i++)
			{
				verts[i].X = data[i].X;
				verts[i].Y = data[i].Y;
				verts[i].Z = 0.0f;
				verts[i].Rhw = 1; 
				verts[i].Color = data_line_color.ToArgb();
			}

			//GraphicsStream stm = vb.Lock(0, 0, 0);
			//stm.Write(verts);
			//vb.Unlock();
			vb.SetData(verts, 0, LockFlags.None);
		}

		public static void UpdateMouseVertexBuffer(object sender, int x, int y)
		{
			VertexBuffer vb = (VertexBuffer)sender;
			CustomVertex.TransformedColored[] verts = new CustomVertex.TransformedColored[4];

			verts[0].X = 0.0f;
			verts[0].Y = (float)y;
			verts[0].Z = 0.0f;
			verts[0].Rhw = 1;
			verts[0].Color = grid_text_color.ToArgb();
				
			verts[1].X = (float)W;
			verts[1].Y = (float)y;
			verts[1].Z = 0.0f;
			verts[1].Rhw = 1;
			verts[1].Color = grid_text_color.ToArgb();

			verts[2].X = (float)x;
			verts[2].Y = 0.0f;
			verts[2].Z = 0.0f;
			verts[2].Rhw = 1;
			verts[2].Color = grid_text_color.ToArgb();
				
			verts[3].X = (float)x;
			verts[3].Y = (float)H;
			verts[3].Z = 0.0f;
			verts[3].Rhw = 1;
			verts[3].Color = grid_text_color.ToArgb();

			//GraphicsStream stm = vb.Lock(0, 0, 0);
			//stm.Write(verts);
			//vb.Unlock();
			vb.SetData(verts, 0, LockFlags.None);
		}

		public static void RenderDXBackground()
		{
			if(current_background != null)
			{		
				// draw grid lines
				foreach(DXLine l in current_background.lines)
				{
					l.Draw();
				}
	
				if(bg_string_font != null) // ensure the font has been instantiated
				{
					//verify background object string content
					Debug.Assert(current_background.str_loc.Count == current_background.strings.Count);

					// draw each string at the indicated points
					for(int i=0; i<current_background.strings.Count; i++)
					{
						bg_string_font.DrawText(
							null, 
							(string)current_background.strings[i], 
							((Point)current_background.str_loc[i]).X, 
							((Point)current_background.str_loc[i]).Y, 
							grid_text_color);
					}
				}				
			}
		}

		public static void RenderDirectX()
		{
			if(dx_device == null) return;

			// setup data
			switch(current_display_mode)
			{
				case DisplayMode.PANADAPTER:
					ConvertDataForPanadapter();
					break;
				case DisplayMode.SPECTRUM:
					ConvertDataForSpectrum();
					break;
				case DisplayMode.PHASE:
					ConvertDataForPhase();
					break;
				case DisplayMode.SCOPE:
					ConvertDataForScope();
					break;
			}
			if(console.LongCrosshair)
				UpdateMouseVertexBuffer(dx_mouse_vb, console.DisplayCursorX, console.DisplayCursorY);

			dx_device.Clear(ClearFlags.Target, display_background_color, 0.0f, 0);

			//Begin the scene
			dx_device.BeginScene();			

			//dx_device.SetTexture(0, dx_texture);
			//dx_device.SetStreamSource(0, dx_background_vb, 0);
			//dx_device.VertexFormat = CustomVertex.TransformedTextured.Format;
			//dx_device.DrawPrimitives(PrimitiveType.TriangleFan, 0, 2);
			//dx_device.SetTexture(0, null);

			RenderDXBackground();

			dx_device.SetStreamSource(0, dx_data_vb, 0);
			dx_device.VertexFormat = CustomVertex.TransformedColored.Format;
			
			switch(current_display_mode)
			{
				case DisplayMode.PANADAPTER:
				case DisplayMode.SPECTRUM:
				case DisplayMode.SCOPE:
					dx_device.DrawPrimitives(PrimitiveType.LineStrip, 0, W-1);
					break;
				case DisplayMode.PHASE:
					dx_device.DrawPrimitives(PrimitiveType.PointList, 0, phase_num_pts);
					break;
			}

			if(console.LongCrosshair)
			{
				dx_device.SetStreamSource(0, dx_mouse_vb, 0);
				dx_device.DrawPrimitives(PrimitiveType.LineStrip, 0, 1);
				dx_device.DrawPrimitives(PrimitiveType.LineStrip, 2, 1);
			}

			/*Microsoft.DirectX.Direct3D.Font font = new Microsoft.DirectX.Direct3D.Font(
				dx_device, 
				new System.Drawing.Font("Arial", 14.0f, FontStyle.Bold));
			font.DrawText(null, string.Format("Testing"), new Rectangle(256, 20, 0, 0), DrawTextFormat.NoClip, Color.Red);

			//if(console.HighSWR)
			//{
			//	if(directx_render_type == RenderType.HARDWARE)
			//	{
			//		Microsoft.DirectX.Direct3D.Font high_swr_font = new Microsoft.DirectX.Direct3D.Font(dx_device, new System.Drawing.Font("Arial", 14.0f, FontStyle.Bold));
			//		high_swr_font.DrawText(null, string.Format("High SWR"), new Rectangle(245, 20, 0, 0), DrawTextFormat.NoClip, Color.Red);
			//	}
			//}

			//End the scene
			dx_device.EndScene();
			dx_device.Present();
		}

		unsafe private static void ConvertDataForPanadapter()
		{
			//float[] data = new float[W];			// array of points to display
			float slope = 0.0f;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int Low = -10000;
			int High = 10000;
			int yRange = spectrum_grid_max - spectrum_grid_min;

			if(console.CurrentDSPMode == DSPMode.DRM)
			{
				Low = 2000;
				High = 22000;
			}

			max_y = Int32.MinValue;

			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				// kb9yig sr mod starts 
				if ( console.CurrentModel == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data);
				// end kb9yig sr mods 

				data_ready = false;
			}

			if(average_on)
				console.UpdateDisplayAverage();
			if(peak_on)
				UpdateDisplayPeak();

			num_samples = (High - Low);

			start_sample_index = (BUFFER_SIZE>>1) +(int)((Low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((High - Low) * BUFFER_SIZE / sample_rate);
			if (start_sample_index < 0) start_sample_index = 0;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE-start_sample_index;

			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				int rindex = (int)Math.Floor(dval + slope);
				if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
				
				for(int j=lindex;j<rindex;j++)
					if (current_display_data[j] > max) max=current_display_data[j];

				max = max +
					console.DisplayCalOffset + 
					console.PreampOffset;

				if(max > max_y)
				{
					max_y = max;
					max_x = i;
				}

				data[i] = (int)(Math.Floor((spectrum_grid_max - max)*H/yRange));
			} 

			UpdateDataVertexBuffer(dx_data_vb, data);
		}

		unsafe static private void ConvertDataForSpectrum()
		{
			//float[] data = new float[W];			// array of points to display
			float slope = 0.0f;						// samples to process per pixel
			int num_samples = 0;					// number of samples to process
			int start_sample_index = 0;				// index to begin looking at samples
			int low = 0;
			int high = 0;

			max_y = Int32.MinValue;

			if(!console.MOX)
			{
				low = rx_display_low;
				high = rx_display_high;
			}
			else
			{
				low = tx_display_low;
				high = tx_display_high;
			}

			if(console.CurrentDSPMode == DSPMode.DRM)
			{
				low = 2000;
				high = 22000;
			}

			int yRange = spectrum_grid_max - spectrum_grid_min;

			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				// kb9yig sr mod starts 
				if ( console.CurrentModel == Model.SOFTROCK40 ) 
					console.AdjustDisplayDataForBandEdge(ref current_display_data);
				// end kb9yig sr mods 

				data_ready = false;
			}

			if(average_on)
				console.UpdateDisplayAverage();
			if(peak_on)
				UpdateDisplayPeak();

			start_sample_index = (BUFFER_SIZE>>1) + (int)((low * BUFFER_SIZE) / sample_rate);
			num_samples = (int)((high - low) * BUFFER_SIZE / sample_rate);

			if (start_sample_index < 0) start_sample_index = 0;
			if ((num_samples - start_sample_index) > (BUFFER_SIZE+1))
				num_samples = BUFFER_SIZE - start_sample_index;

			slope = (float)num_samples/(float)W;
			for(int i=0; i<W; i++)
			{
				float max = float.MinValue;
				float dval = i*slope + start_sample_index;
				int lindex = (int)Math.Floor(dval);
				int rindex;

				if (slope <= 1) 
					max =  current_display_data[lindex]*((float)lindex-dval+1) + current_display_data[lindex+1]*(dval-(float)lindex);
				else 
				{
					rindex = (int)Math.Floor(dval + slope);
					if (rindex > BUFFER_SIZE) rindex = BUFFER_SIZE;
					for(int j=lindex; j<rindex; j++)
						if (current_display_data[j] > max) max=current_display_data[j];

				}

				max = max + 
					console.DisplayCalOffset + 
					console.PreampOffset;

				if(max > max_y)
				{
					max_y = max;
					max_x = i;
				}

				data[i] = (int)(Math.Floor((spectrum_grid_max - max)*H/yRange));
			}

			UpdateDataVertexBuffer(dx_data_vb, data);
		}

		unsafe private static void ConvertDataForPhase()
		{
			int num_points = phase_num_pts;

			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				data_ready = false;
			}

			Point[] points = new Point[num_points];		// declare Point array
			for(int i=0,j=0; i<num_points; i++,j+=8)	// fill point array
			{
				int x = (int)(current_display_data[i*2]*H/2);
				int y = (int)(current_display_data[i*2+1]*H/2);
				points[i].X = W/2+x;
				points[i].Y = H/2+y;
			}
			
			// draw each point
			UpdateDataVertexBuffer(dx_data_vb, points);

			points = null;
		}

		unsafe private static void ConvertDataForScope()
		{
			if(data_ready)
			{
				// get new data
				fixed(void *rptr = &new_display_data[0])
					fixed(void *wptr = &current_display_data[0])
						Win32.memcpy(wptr, rptr, BUFFER_SIZE*sizeof(float));
						
				data_ready = false;
			}

			double num_samples = console.ScopeTime/1000.0*console.SampleRate1;
			double slope = num_samples/(double)W;

			//float[] data = new float[W];				// create Point array
			for(int i=0; i<W; i++)						// fill point array
			{	
				int pixels = (int)(H/2 * current_display_data[(int)Math.Floor(i*slope)]);
				int y = H/2 - pixels;
				if(y < max_y)
				{
					max_y = y;
					max_x = i;
				}
				
				data[i] = y;
			}

			// draw the connected points
			UpdateDataVertexBuffer(dx_data_vb, data);
		}

		private static Background SetupScope()
		{			
			Background bg = new Background();
			
			// Add horizontal line
			bg.lines.Add(new DXLine(new Point(0, H/2), new Point(W, H/2), 1, grid_color, dx_device));
			
			// Add vertical line
			bg.lines.Add(new DXLine(new Point(W/2, 0), new Point(W/2, H), 1, grid_color, dx_device));

			return bg;
		}

		private static Background SetupSpectrum()
		{
			Background bg = new Background();
			Graphics g = console.CreateGraphics();
			System.Drawing.Font font = new System.Drawing.Font("Arial", 9);

			int low = 0;								// init limit variables
			int high = 0;

			if(!console.MOX)
			{
				low = rx_display_low;				// get RX display limits
				high = rx_display_high;
			}
			else
			{
				low = tx_display_low;				// get TX display limits
				high = tx_display_high;
			}

			int mid_w = W/2;
			int[] step_list = {10, 20, 25, 50};
			int step_power = 1;
			int step_index = 0;
			int freq_step_size = 50;

			int y_range = spectrum_grid_max - spectrum_grid_min;

			if(high == 0)
			{
				int f = -low;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);

				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = W-(int)Math.Floor(i*pixel_step_size);	// for negative numbers
					
					bg.lines.Add(new DXLine(new Point(x, 0), new Point(x, H), 1, grid_color, dx_device));
					//g.DrawLine(grid_pen, x, 0, x, H);				// draw right line
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)((label.Length+1)*4.1);
					if(x-offset >= 0)
					{
						//g.DrawString("-"+label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
						bg.strings.Add("-"+label);
						bg.str_loc.Add(new Point(x-offset, (int)Math.Floor(H*.01)));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				num_steps = V/spectrum_grid_step;
				pixel_step_size = H/num_steps;

				for(int i=1; i<num_steps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*spectrum_grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					//g.DrawLine(grid_pen, 0, y, W, y);
					bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

					// Draw horizontal line labels
					string label = num.ToString();
					int offset = (int)(label.Length*4.1);
					if(label.Length == 3)
						xOffset = (int)g.MeasureString("-", font).Width - 2;
					SizeF size = g.MeasureString(label, font);

					y -= 8;
					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = W/2+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
						case DisplayLabelAlignment.AUTO:
							x = (int)(W-size.Width);
							break;						
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					if(y+9 < H)
					{
						//g.DrawString(label, font, grid_text_brush, x, y);
						bg.strings.Add(label);
						bg.str_loc.Add(new Point(x, y));
					}
				}

				// Draw middle vertical line
				//g.DrawLine(new Pen(grid_zero_color), W-1, 0, W-1, H);
				//g.DrawLine(new Pen(grid_zero_color), W-2, 0, W-2, H);
				bg.lines.Add(new DXLine(new Point(W-1, 0), new Point(W-1, H), 1, grid_zero_color, dx_device));
				bg.lines.Add(new DXLine(new Point(W-2, 0), new Point(W-2, H), 1, grid_zero_color, dx_device));
			}
			else if(low == 0)
			{
				int f = high;
				// Calculate horizontal step size
				while(f/freq_step_size > 7)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				float pixel_step_size = (float)(W*freq_step_size/f);
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int x = (int)Math.Floor(i*pixel_step_size);// for positive numbers
					
					//g.DrawLine(grid_pen, x, 0, x, H);			// draw right line
					bg.lines.Add(new DXLine(new Point(x, 0), new Point(x, H), 1, grid_color, dx_device));
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offset = (int)(label.Length*4.1);
					if(x-offset+label.Length*7 < W)
					{
						//g.DrawString(label, font, grid_text_brush, x-offset, (float)Math.Floor(H*.01));
						bg.strings.Add(label);
						bg.str_loc.Add(new Point(x-offset, (int)Math.Floor(H*.01)));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/spectrum_grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*spectrum_grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);

					//g.DrawLine(grid_pen, 0, y, W, y);
					bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

					// Draw horizontal line labels
					string label = num.ToString();
					if(label.Length == 3)
						xOffset = (int)g.MeasureString("-", font).Width - 2;
					int offset = (int)(label.Length*4.1);
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
						case DisplayLabelAlignment.AUTO:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
							x = W/2+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width);
							break;
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;
					if(y+9 < H)
					{
						//g.DrawString(label, font, grid_text_brush, x, y);
						bg.strings.Add(label);
						bg.str_loc.Add(new Point(x, y));
					}
				}

				// Draw middle vertical line
				//g.DrawLine(new Pen(grid_zero_color), 0, 0, 0, H);
				//g.DrawLine(new Pen(grid_zero_color), 1, 0, 1, H);
				bg.lines.Add(new DXLine(new Point(0, 0), new Point(0, H), 1, grid_zero_color, dx_device));
				bg.lines.Add(new DXLine(new Point(1, 0), new Point(1, H), 1, grid_zero_color, dx_device));
			}
			if(low < 0 && high > 0)
			{
				int f = high;

				// Calculate horizontal step size
				while(f/freq_step_size > 4)
				{
					freq_step_size = step_list[step_index]*(int)Math.Pow(10.0, step_power);
					step_index = (step_index+1)%4;
					if(step_index == 0) step_power++;
				}
				int pixel_step_size = W/2*freq_step_size/f;
				int num_steps = f/freq_step_size;

				// Draw vertical lines
				for(int i=1; i<=num_steps; i++)
				{
					int xLeft = mid_w-(i*pixel_step_size);			// for negative numbers
					int xRight = mid_w+(i*pixel_step_size);		// for positive numbers
					//g.DrawLine(grid_pen, xLeft, 0, xLeft, H);		// draw left line
					//g.DrawLine(grid_pen, xRight, 0, xRight, H);		// draw right line
					bg.lines.Add(new DXLine(new Point(xLeft, 0), new Point(xLeft, H), 1, grid_color, dx_device));
					bg.lines.Add(new DXLine(new Point(xRight, 0), new Point(xRight, H), 1, grid_color, dx_device));
				
					// Draw vertical line labels
					int num = i*freq_step_size;
					string label = num.ToString();
					int offsetL = (int)((label.Length+1)*4.1);
					int offsetR = (int)(label.Length*4.1);
					if(xLeft-offsetL >= 0)
					{
						//g.DrawString("-"+label, font, grid_text_brush, xLeft-offsetL, (float)Math.Floor(H*.01));
						bg.strings.Add("-"+label);
						bg.str_loc.Add(new Point(xLeft-offsetL, (int)Math.Floor(H*.01)));
						//g.DrawString(label, font, grid_text_brush, xRight-offsetR, (float)Math.Floor(H*.01));
						bg.strings.Add(label);
						bg.str_loc.Add(new Point(xRight-offsetR, (int)Math.Floor(H*.01)));
					}
				}

				// Draw horizontal lines
				int V = (int)(spectrum_grid_max - spectrum_grid_min);
				int numSteps = V/spectrum_grid_step;
				pixel_step_size = H/numSteps;
				for(int i=1; i<numSteps; i++)
				{
					int xOffset = 0;
					int num = spectrum_grid_max - i*spectrum_grid_step;
					int y = (int)Math.Floor((double)(spectrum_grid_max - num)*H/y_range);
					//g.DrawLine(grid_pen, 0, y, W, y);
					bg.lines.Add(new DXLine(new Point(0, y), new Point(W, y), 1, grid_color, dx_device));

					// Draw horizontal line labels
					string label = num.ToString();
					if(label.Length == 3) xOffset = 7;
					int offset = (int)(label.Length*4.1);
					SizeF size = g.MeasureString(label, font);

					int x = 0;
					switch(display_label_align)
					{
						case DisplayLabelAlignment.LEFT:
							x = xOffset + 3;
							break;
						case DisplayLabelAlignment.CENTER:
						case DisplayLabelAlignment.AUTO:
							x = W/2+xOffset;
							break;
						case DisplayLabelAlignment.RIGHT:
							x = (int)(W-size.Width);
							break;
						case DisplayLabelAlignment.OFF:
							x = W;
							break;
					}

					y -= 8;
					if(y+9 < H)
					{
						//g.DrawString(label, font, grid_text_brush, x, y);
						bg.strings.Add(label);
						bg.str_loc.Add(new Point(x, y));
					}
				}

				// Draw middle vertical line
				//g.DrawLine(new Pen(grid_zero_color), mid_w, 0, mid_w, H);
				//g.DrawLine(new Pen(grid_zero_color), mid_w-1, 0, mid_w-1, H);
				bg.lines.Add(new DXLine(new Point(mid_w, 0), new Point(mid_w, H), 1, grid_zero_color, dx_device));
				bg.lines.Add(new DXLine(new Point(mid_w-1, 0), new Point(mid_w-1, H), 1, grid_zero_color, dx_device));
			}

			if(console.HighSWR)
			{
				//g.DrawString("High SWR", high_swr_font, red_brush, 245, 20);
				bg.strings.Add("High SWR");
				bg.str_loc.Add(new Point(245, 20));
			}

			g = null;
			font = null;

			return bg;
		}

		#endregion

		#endregion


		#region Background Class

		public class Background
		{
			public ArrayList strings;		// array of strings to be drawn on the background
			public ArrayList str_loc;		// array of top/left location of strings to be drawn
			public ArrayList lines;			// array of DXLines to be drawn
			public ArrayList overlay;		// array of points for overlay to be drawn

			public Background()
			{
				strings = new ArrayList();
				str_loc = new ArrayList();
				lines = new ArrayList();
				overlay = new ArrayList();
			}
		}

		#endregion

		#region DXLine Class

		public class DXLine
		{
			// Line object 
			private Microsoft.DirectX.Direct3D.Line mLine;
			private Vector2[] line_vectors;
            
			// Starting point for the line 
			private Point mStartPoint;
			public Point StartPoint
			{
				get { return mStartPoint; }
				set
				{
					mStartPoint = value;
					UpdateLineVectors();
				}
			}
            
			// Ending point for the line 
			private Point mEndPoint;
			public Point EndPoint
			{
				get { return mEndPoint; }
				set
				{
					mEndPoint = value;
					UpdateLineVectors();
				}
			}
			
			// Width of the line 
			private int mWidth;
			public int Width 
			{
				get { return mWidth; }
				set { mWidth = value; }
			}

			// Color for the line 
			private Color mColor;
			public Color Color
			{
				get { return mColor; }
				set { mColor = value; }
			}
			
			// Line class constructor 
			public DXLine(Point startPoint, Point endPoint, int width, Color color, Device device)
			{
				// Store the data passed into the class constructor 
				mStartPoint = startPoint;
				mEndPoint = endPoint;
				mWidth = width;
				mColor = color;

				// create line vectors
				line_vectors = new Vector2[2];
				UpdateLineVectors();

				// Create the line object 
				mLine = new Line(device);
			}

			// Draw the line using the current class values for starpoint, endpoint, width and color 
			public void Draw()
			{
				// Render the line 
				mLine.Begin();
				mLine.Draw(line_vectors, Color);
				mLine.End();
			} 

			// Construct the Line Vectors based on the Start and End Points 
			private void UpdateLineVectors()
			{
				// Set the starting point of the line 
				line_vectors[0].X = StartPoint.X;
				line_vectors[0].Y = StartPoint.Y; 

				// Set the end point of the line 
				line_vectors[1].X = EndPoint.X;
				line_vectors[1].Y = EndPoint.Y;
			}
		}
*/
		#endregion
	}
}