//=================================================================
// ringbuffer.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2008  FlexRadio Systems
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
// Derived from jack/ringbuffer.h
// Translated to C# by Eric Wachsmann KE5DTO and Bob McGwier N4HY.
//=================================================================

using System.Diagnostics;

namespace PowerSDR
{
	unsafe public class RingBufferFloat
	{
		#region Variable Declaration

		private float[] buf; /// actual internal buffer used to store the data
		private int wptr;	/// Write Pointer
		private	int rptr;	/// Read Pointer
		private	int size;	/// Size of the RingBuffer
		private	int mask;	/// mask used to speed reads/writes

		#endregion

		#region Constructor

		public RingBufferFloat(int sz2)
		{
			size = nblock2(sz2);
			buf = new float[size];
			mask = size-1;
			wptr = rptr = 0;
		}

		#endregion

		#region Public Functions

		// returns the power of 2 that is equal/larger than n
		public int npoof2(int n)
		{
			int i = 0;
			--n;
			while (n > 0)
			{
				n >>= 1;
				i++;
			}
			return i;
		}

		// returns the next power of 2 larger/equal to n
		public int nblock2(int n)
		{
			return 1 << npoof2(n);
		}

		/// <summary>
		/// Get the number of elements available to be read from the ringbuffer.
		/// </summary>
		/// <returns>Return the number of elements available to be read.</returns>
		public int ReadSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return w - r;
			else return (size-r+w) & mask;
		}

		/// <summary>
		/// Get the number of elements that will fit into the ringbuffer.
		/// </summary>
		/// <returns>The amount of space available to be written.</returns>
 		public int WriteSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return ((size-w+r) & mask) - 1;
			else if (w < r) return r - w - 1;
			else return size - 1;
		}

		/// <summary>
		/// Reads data out of the ringbuffer into the dest array.
		/// </summary>
		/// <param name="dest">Array to read data into.</param>
		/// <param name="cnt">Requested number of elements to read</param>
		/// <returns>Actual number of elements read</returns>
		public int Read(float[] dest, int cnt)
		{
			int free_cnt = ReadSpace();
			if(free_cnt == 0) return 0;
			
			int to_read = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = rptr + to_read;
			int n1=0, n2=0;

			if (cnt2 > size)
			{
				n1 = size - rptr; 
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_read;
				n2 = 0;
			}
			fixed(void *r = &buf[rptr])
				fixed(void *w = &dest[0])
					Win32.memcpy(w, r, n1*sizeof(float));

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				fixed(void *r = &buf[rptr])
					fixed(void *w = &dest[n1])
						Win32.memcpy(w, r, n2*sizeof(float));

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}

		/// <summary>
		/// Read elements out of the ringbuffer into the array pointed to by dest.
		/// </summary>
		/// <param name="dest">Points to destination array to put read elements.</param>
		/// <param name="cnt">Requested number of elements to read.</param>
		/// <returns>The actual number of elements read.</returns>
		public int ReadPtr(float *dest, int cnt)
		{
			void *w;
			int free_cnt = ReadSpace();
			if(free_cnt == 0) return 0;
			
			int to_read = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = rptr + to_read;
			int n1=0, n2=0;

			if (cnt2 > size)
			{
				n1 = size - rptr; 
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_read;
				n2 = 0;
			}
			w = &dest[0];
			fixed(void *r = &buf[rptr])
					Win32.memcpy(w, r, n1*sizeof(float));

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				w = (void *)&dest[n1];
				fixed(void *r = &buf[rptr])
						Win32.memcpy(w, r, n2*sizeof(float));

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}

		/// <summary>
		/// Writes from the src array into the ringbuffer.
		/// </summary>
		/// <param name="src">The souce array to be written to the ringbuffer.</param>
		/// <param name="cnt">The requested number of elements to write.</param>
		/// <returns>The actual number of elements written.</returns>
		public int Write(float[] src, int cnt)
		{
			int free_cnt = WriteSpace();
			if(free_cnt == 0) return 0;

			int to_write = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = wptr + to_write;
			int n1=0, n2=0;

			if(cnt2 > size)
			{
				n1 = size - wptr;
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_write;
				n2 = 0;
			}
			fixed(void *r = &src[0])
				fixed(void *w = &buf[wptr])
					Win32.memcpy(w, r, n1*sizeof(float));

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				fixed(void *r = &src[n1])
					fixed(void *w = &buf[wptr])
						Win32.memcpy(w, r, n1*sizeof(float));

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

		/// <summary>
		/// Writes from the array pointed to by src into the ringbuffer.
		/// </summary>
		/// <param name="src">Points to the array to be used to write data into the ringbuffer.</param>
		/// <param name="cnt">Requested number of elements to write.</param>
		/// <returns>Actual number of elements written.</returns>
		public int WritePtr(float *src, int cnt)
		{
			void *r;
			int free_cnt = WriteSpace();
			if(free_cnt == 0) return 0;

			int to_write = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = wptr + to_write;
			int n1=0, n2=0;

			if(cnt2 > size)
			{
				n1 = size - wptr;
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_write;
				n2 = 0;
			}

			r = (void *)src;
				fixed(void *w = &buf[wptr])
					Win32.memcpy(w, r, n1*sizeof(float));

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				r = (void *)(src+n1);
					fixed(void *w = &buf[wptr])
						Win32.memcpy(w, r, n1*sizeof(float));

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

		/// <summary>
		/// Resets the ringbuffer pointers (will be empty afterwards).
		/// </summary>
		public void Reset()
		{
			rptr = 0;
			wptr = 0;
		}

		/// <summary>
		/// Zero the data in the buffer.
		/// </summary>
		/// <param name="nfloats">Number of elements to zero.</param>
		public void Clear(int nfloats)
		{
			float[] zero;
			zero = new float[1];
			for(int i=0;i<nfloats;i++) Write(zero,1);
		}

		/// <summary>
		/// Reset the pointers and zero the actual data.
		/// </summary>
		/// <param name="nfloats">Number of elements to zero.</param>
		public void Restart(int nfloats) 
		{
			Reset();
			Clear(nfloats);
		}
        //public int Peek(float *dest, int cnt);		
		//public void ReadAdvance(int cnt);
		//public void WriteAdvance(int cnt);

		#endregion
	}

	unsafe public class RingBufferByte
	{
		#region Variable Declaration

		private byte[] buf; /// actual internal buffer used to store the data
		private int wptr;	/// Write Pointer
		private	int rptr;	/// Read Pointer
		private	int size;	/// Size of the RingBuffer
		private	int mask;	/// mask used to speed reads/writes

		#endregion

		#region Constructor

		public RingBufferByte(int sz2)
		{
			size = nblock2(sz2);
			buf = new byte[size];
			mask = size-1;
			wptr = rptr = 0;
		}

		#endregion

		#region Public Functions

		// returns the power of 2 that is equal/larger than n
		public int npoof2(int n)
		{
			int i = 0;
			--n;
			while (n > 0)
			{
				n >>= 1;
				i++;
			}
			return i;
		}

		// returns the next power of 2 larger/equal to n
		public int nblock2(int n)
		{
			return 1 << npoof2(n);
		}

		/// <summary>
		/// Get the number of elements available to be read from the ringbuffer.
		/// </summary>
		/// <returns>Return the number of elements available to be read.</returns>
		public int ReadSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return w - r;
			else return (size-r+w) & mask;
		}

		/// <summary>
		/// Get the number of elements that will fit into the ringbuffer.
		/// </summary>
		/// <returns>The amount of space available to be written.</returns>
		public int WriteSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return ((size-w+r) & mask) - 1;
			else if (w < r) return r - w - 1;
			else return size - 1;
		}

		/// <summary>
		/// Reads data out of the ringbuffer into the dest array.
		/// </summary>
		/// <param name="dest">Array to read data into.</param>
		/// <param name="cnt">Requested number of elements to read</param>
		/// <returns>Actual number of elements read</returns>
		public int Read(byte[] dest, int cnt)
		{
			int free_cnt = ReadSpace();
			if(free_cnt == 0) return 0;
			
			int to_read = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = rptr + to_read;
			int n1=0, n2=0;

			if (cnt2 > size)
			{
				n1 = size - rptr; 
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_read;
				n2 = 0;
			}
			fixed(void *r = &buf[rptr])
				fixed(void *w = &dest[0])
					Win32.memcpy(w, r, n1*sizeof(byte));

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				fixed(void *r = &buf[rptr])
					fixed(void *w = &dest[n1])
						Win32.memcpy(w, r, n2*sizeof(byte));

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}

		/// <summary>
		/// Read elements out of the ringbuffer into the array pointed to by dest.
		/// </summary>
		/// <param name="dest">Points to destination array to put read elements.</param>
		/// <param name="cnt">Requested number of elements to read.</param>
		/// <returns>The actual number of elements read.</returns>
		public int ReadPtr(byte *dest, int cnt)
		{
			void *w;
			int free_cnt = ReadSpace();
			if(free_cnt == 0) return 0;
			
			int to_read = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = rptr + to_read;
			int n1=0, n2=0;

			if (cnt2 > size)
			{
				n1 = size - rptr; 
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_read;
				n2 = 0;
			}
			w = &dest[0];
			fixed(void *r = &buf[rptr])
				Win32.memcpy(w, r, n1*sizeof(byte));

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				w = (void *)&dest[n1];
				fixed(void *r = &buf[rptr])
					Win32.memcpy(w, r, n2*sizeof(byte));

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}

		/// <summary>
		/// Writes from the src array into the ringbuffer.
		/// </summary>
		/// <param name="src">The souce array to be written to the ringbuffer.</param>
		/// <param name="cnt">The requested number of elements to write.</param>
		/// <returns>The actual number of elements written.</returns>
		public int Write(byte[] src, int cnt)
		{
			int free_cnt = WriteSpace();
			if(free_cnt == 0) return 0;

			int to_write = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = wptr + to_write;
			int n1=0, n2=0;

			if(cnt2 > size)
			{
				n1 = size - wptr;
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_write;
				n2 = 0;
			}
			fixed(void *r = &src[0])
				fixed(void *w = &buf[wptr])
					Win32.memcpy(w, r, n1*sizeof(byte));

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				fixed(void *r = &src[n1])
					fixed(void *w = &buf[wptr])
						Win32.memcpy(w, r, n1*sizeof(byte));

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

		/// <summary>
		/// Writes from the array pointed to by src into the ringbuffer.
		/// </summary>
		/// <param name="src">Points to the array to be used to write data into the ringbuffer.</param>
		/// <param name="cnt">Requested number of elements to write.</param>
		/// <returns>Actual number of elements written.</returns>
		public int WritePtr(byte *src, int cnt)
		{
			void *r;
			int free_cnt = WriteSpace();
			if(free_cnt == 0) return 0;

			int to_write = cnt > free_cnt ? free_cnt : cnt;
			int cnt2 = wptr + to_write;
			int n1=0, n2=0;

			if(cnt2 > size)
			{
				n1 = size - wptr;
				n2 = cnt2 & mask;
			}
			else
			{
				n1 = to_write;
				n2 = 0;
			}

			r = (void *)src;
			fixed(void *w = &buf[wptr])
				Win32.memcpy(w, r, n1*sizeof(byte));

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				r = (void *)(src+n1);
				fixed(void *w = &buf[wptr])
					Win32.memcpy(w, r, n1*sizeof(byte));

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

		/// <summary>
		/// Resets the ringbuffer pointers (will be empty afterwards).
		/// </summary>
		public void Reset()
		{
			rptr = 0;
			wptr = 0;
		}

		/// <summary>
		/// Zero the data in the buffer.
		/// </summary>
		/// <param name="nfloats">Number of elements to zero.</param>
		public void Clear(int nfloats)
		{
			byte[] zero;
			zero = new byte[1];
			for(int i=0;i<nfloats;i++) Write(zero,1);
		}

		/// <summary>
		/// Reset the pointers and zero the actual data.
		/// </summary>
		/// <param name="nfloats">Number of elements to zero.</param>
		public void Restart(int nfloats) 
		{
			Reset();
			Clear(nfloats);
		}
		//public int Peek(float *dest, int cnt);		
		//public void ReadAdvance(int cnt);
		//public void WriteAdvance(int cnt);

		#endregion
	}
}