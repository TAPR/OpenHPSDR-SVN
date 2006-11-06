//=================================================================
// ringbuffer.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004, 2005, 2006  FlexRadio Systems
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
//    12100 Technology Blvd.
//    Austin, TX 78727
//    USA
//=================================================================
// Derived from jack/ringbuffer.h
// Translated to C# by Eric Wachsmann KE5DTO and Bob McGwier N4HY.
//=================================================================

namespace PowerSDR
{
	unsafe public class RingBuffer
	{
		#region Variable Declaration

		private float[] buf;
		private int wptr;
		private	int rptr;
		private	int size;
		private	int mask;

		#endregion

		#region Constructor

		public RingBuffer(int sz2)
		{
			size = nblock2(sz2);
			buf = new float[size];
			mask = size-1;
			wptr = rptr = 0;
		}

		#endregion
		
		#region Private Functions


		#endregion

		#region Public Functions
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

		public int	nblock2(int n)
		{
			return 1 << npoof2(n);
		}

		public int ReadSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return w - r;
			else return (size-r+w) & mask;
		}

		public int WriteSpace()
		{
			int w = wptr, r = rptr;
			if (w > r) return ((size-w+r) & mask) - 1;
			else if (w < r) return r - w - 1;
			else return size - 1;
		}

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
			//Win32.memcpy(dest, buf[rptr], n1);
//			for(int i=0; i<n1; i++)
//				dest[i] = buf[rptr+i];

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				fixed(void *r = &buf[rptr])
					fixed(void *w = &dest[n1])
						Win32.memcpy(w, r, n2*sizeof(float));
				//Win32.memcpy(dest + n1, buf[rptr], n2);
//				for(int i=0; i<n2; i++)
//					dest[i+n1] = buf[rptr+i];

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}
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
			//Win32.memcpy(dest, buf[rptr], n1);
			//			for(int i=0; i<n1; i++)
			//				dest[i] = buf[rptr+i];

			rptr = (rptr + n1) & mask;
			
			if(n2 != 0) 
			{
				w = (void *)&dest[n1];
				fixed(void *r = &buf[rptr])
						Win32.memcpy(w, r, n2*sizeof(float));
				//Win32.memcpy(dest + n1, buf[rptr], n2);
				//				for(int i=0; i<n2; i++)
				//					dest[i+n1] = buf[rptr+i];

				rptr = (rptr + n2) & mask;
			}
			return to_read;
		}

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
			//Win32.memcpy(buf[wptr], src, n1);
//			for(int i=0; i<n1; i++)
//				buf[wptr+i] = src[i];

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				fixed(void *r = &src[n1])
					fixed(void *w = &buf[wptr])
						Win32.memcpy(w, r, n1*sizeof(float));
				//Win32.memcpy(buf[wptr], src + n1, n2);
				for(int i=0; i<n2; i++)
					buf[wptr+i] = src[i+n1];

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

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
			//Win32.memcpy(buf[wptr], src, n1);
			//			for(int i=0; i<n1; i++)
			//				buf[wptr+i] = src[i];

			wptr = (wptr + n1) & mask;
			if(n2 != 0) 
			{
				r = (void *)(src+n1);
					fixed(void *w = &buf[wptr])
						Win32.memcpy(w, r, n1*sizeof(float));
				//Win32.memcpy(buf[wptr], src + n1, n2);
				for(int i=0; i<n2; i++)
					buf[wptr+i] = src[i+n1];

				wptr = (wptr + n2) & mask;
			}
			return to_write;
		}

		public void Reset()
		{
			rptr = 0;
			wptr = 0;
		}

		public void Clear(int nfloats)
		{
			float[] zero;
			zero = new float[1];
			for(int i=0;i<nfloats;i++) Write(zero,1);
		}

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