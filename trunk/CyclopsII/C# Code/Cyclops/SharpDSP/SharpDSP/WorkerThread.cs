/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com

*/

//==============================================================
// Written By: Phil Covington N8VB
// Copyright 2007 Philip A. Covington
// This software is licensed under the GNU General Public License
//==============================================================
// workerthread.cs
//==============================================================

using System;
using System.Threading;

namespace SharpDSP2._1
{
	/// <summary>
	/// Description of workerthread.
	/// </summary>
	#region Worker Thread Class		
			
	public class WorkerThread
	{	    
	    readonly object stopLock = new object();
	    bool stopping = false;
	    bool stopped = false;
	    
	    public bool Stopping
	    {
	        get
	        {
	            lock (stopLock)
	            {
	                return stopping;
	            }
	        }
	    }
	    
	    public bool Stopped
	    {
	        get
	        {
	            lock (stopLock)
	            {
	                return stopped;
	            }
	        }
	    }
	
	    public void Stop()
	    {
	        lock (stopLock)
	        {
	            stopping = true;
	        }
	    }
			   
	    protected void SetStopped()
	    {
	        lock (stopLock)
	        {
	            stopped = true;
	        }
	    }
			    
	    public virtual void Run()
	    {
	        try
	        {
	            while (!Stopping)
	            {
	                // Insert work here. Make sure it doesn't tight loop!
	                // (If work is arriving periodically, use a queue and Monitor.Wait,
	                // changing the Stop method to pulse the monitor as well as setting
	                // stopping.)
	
	                // Note that you may also wish to break out *within* the loop
	                // if work items can take a very long time but have points at which
	                // it makes sense to check whether or not you've been asked to stop.
	                // Do this with just:
	                // if (Stopping)
	                // {
	                //     return;
	                // }
	                // The finally block will make sure that the stopped flag is set.
	            }
	        }
	        finally
	        {
	            SetStopped();
	        }
	    }
	}
	#endregion
}
