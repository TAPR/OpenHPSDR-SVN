// Copyright (c) Bill Tracey (kd5tfd) 2006
// email: bill@ewjt.com
//
using System;
using System.Runtime.InteropServices;
namespace PowerSDR
{
	//
	// routines to access audio from kd5tfd/vk6aph fpga based audio 
	// 
	public class TFDAPHaudio
	{
		public TFDAPHaudio()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int StartAudio(int samples_per_block, PA19.PaStreamCallback cb); 

		[DllImport("KD5TFD-VK6APH-Audio.dll")]
		unsafe public static extern int StopAudio(); 
	}
}
