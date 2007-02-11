//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//XMLSerialization.cs
//Serializes the Receiver class
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.IO;


namespace SharpDSP
{
	public class XMLSerialization
	{
		#region Constructor

		public XMLSerialization()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#endregion

		#region Serialization

		public void SerializeState(Receiver obj, string xml_file_name)
		{
			StreamWriter sWriter = new StreamWriter(@xml_file_name);
			XmlSerializer xmlser = new XmlSerializer(obj.GetType());
			xmlser.Serialize(sWriter, obj);
			sWriter.Close();	
		}

		public Receiver DeSerializeState(Receiver obj, string xml_file_name)
		{
			FileStream fStream = new FileStream(@xml_file_name, FileMode.Open, FileAccess.Read);
			XmlSerializer newXmlSer = new XmlSerializer(obj.GetType());
			obj = (Receiver) newXmlSer.Deserialize(fStream);
			fStream.Close();
			return obj;
		}

		public void SerializeState(Transmitter obj, string xml_file_name)
		{
			StreamWriter sWriter = new StreamWriter(@xml_file_name);
			XmlSerializer xmlser = new XmlSerializer(obj.GetType());
			xmlser.Serialize(sWriter, obj);
			sWriter.Close();	
		}

		public Transmitter DeSerializeState(Transmitter obj, string xml_file_name)
		{
			FileStream fStream = new FileStream(@xml_file_name, FileMode.Open, FileAccess.Read);
			XmlSerializer newXmlSer = new XmlSerializer(obj.GetType());
			obj = (Transmitter) newXmlSer.Deserialize(fStream);
			fStream.Close();
			return obj;
		}

		#endregion
	}
}
