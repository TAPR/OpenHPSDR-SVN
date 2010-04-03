using System;
using System.Runtime.InteropServices;

public class PortAccess
{
	[DllImport("inpout32.dll", EntryPoint="Out32")]
	public static extern void Output(int adress, int value);
}


