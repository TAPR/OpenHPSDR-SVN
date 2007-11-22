/* -----------------------------------------------------------------
 * 
 * LED initialization code written by Levent S. 
 * E-mail: ls@izdir.com
 * 
 * This code is provided without implied warranty so the author is
 * not responsible about damages by the use of the code.
 * 
 * You can use this code for any purpose even in any commercial 
 * distributions by referencing my name. 
 * 
 * ! Don't remove or alter this notice in any distribution !
 * 
 * -----------------------------------------------------------------*/
using System;
using System.Runtime.InteropServices;

public class PortAccess
{
	[DllImport("inpout32.dll", EntryPoint="Out32")]
	public static extern void Output(int adress, int value);
}


