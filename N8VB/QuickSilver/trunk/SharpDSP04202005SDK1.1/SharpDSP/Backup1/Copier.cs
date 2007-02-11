//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//copier.cs
//object copier - makes deep copies
//
//==============================================================

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharpDSP
{	
	public class Copier
	{
		public Copier()
		{			
		}

		public object CopyObject(Object obj)
		{
			MemoryStream ms = new MemoryStream();
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(ms, obj);
			ms.Seek(0, SeekOrigin.Begin);
			object clone = bf.Deserialize(ms);
			ms.Close();		
			return clone;			
		}

	}
}
