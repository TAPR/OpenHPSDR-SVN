//=================================================================
// CATParser.cs
//=================================================================
// Copyright (C) 2005  Bob Tracy
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
// You may contact the author via email at: k5kdn@arrl.net
//=================================================================


using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PowerSDR
{
	/// <summary>
	/// Summary description for CATParser.
	/// </summary>

	#region CATParser Class

	public class CATParser
	{

		#region Variable declarations

		private string current_cat;
		private string prefix;
		private string suffix;
		private string extension;
		private char[] term = new char[1]{';'};
		public int nSet;
		public int nGet;
		public int nAns;
		public bool IsActive;
		private XmlDocument doc;
		private CATCommands cmdlist;
		private Console console;
		public string Error1 = "?;";
		public string Error2 = "E;";
		public string Error3 = "O;";
		private bool IsExtended;

		#endregion Variable declarations

		public CATParser(Console c)
		{
			console = c;
			cmdlist = new CATCommands(console,this);
			GetCATData();
		}

		private void GetCATData()
		{
			string file = "CATStructs.xml";
			doc = new XmlDocument();
			try
			{
				doc.Load(Application.StartupPath+"\\"+file);
			}
			catch(System.IO.FileNotFoundException e)
			{
				throw(e);
			}
		}

		// Overloaded Get method accepts either byte or string
		public string Get(byte[] pCmdString)
		{
			ASCIIEncoding AE = new ASCIIEncoding();
			string rtncmd = Get(AE.GetString(pCmdString));
			return rtncmd;
		}

		public string Get(string pCmdString)
		{
			current_cat = pCmdString;
			string rtncmd = "";

			// Abort if the overall string length is less than 3 (aa;)
			if(current_cat.Length < 3)
				return Error1;

			bool goodcmd = CheckFormat();
			if(goodcmd)
			{
				switch(prefix)
				{
					case "AC":
						break;
					case "AG":
						rtncmd = cmdlist.AG(suffix);
						break;
					case "AI":
						break;
					case "AL":
						break;
					case "AM":
						break;
					case "AN":
						break;
					case "AR":
						break;
					case "AS":
						break;
					case "BC":
						break;
					case "BD":
						rtncmd = cmdlist.BD();
						break;
					case "BP":
						break;
					case "BU":
						rtncmd = cmdlist.BU();
						break;
					case "BY":
						break;
					case "CA":
						break;
					case "CG":
						break;
					case "CH":
						break;
					case "CI":
						break;
					case "CM":
						break;
					case "CN":
						break;
					case "CT":
						break;
					case "DC":
						break;
					case "DN":
						break;
					case "DQ":
						break;
					case "EX":
						break;
					case "FA":
						rtncmd = cmdlist.FA(suffix);
						break;
					case "FB":
						rtncmd = cmdlist.FB(suffix);
						break;
					case "FC":
						break;
					case "FD":
						break;
					case "FR":
						rtncmd = cmdlist.FR(suffix);
						break;
					case "FS":
						break;
					case "FT":
						rtncmd = cmdlist.FT(suffix);
						break;
					case "FW":
						rtncmd = cmdlist.FW(suffix);
						break;
					case "GT":
						rtncmd = cmdlist.GT(suffix);
						break;
					case "ID":
						rtncmd = cmdlist.ID();
						break;
					case "IF":
						rtncmd = cmdlist.IF();
						break;
					case "IS":
						break;
					case "KS":
						break;
					case "KY":
						break;
					case "LK":
						break;
					case "LM":
						break;
					case "LT":
						break;
					case "MC":
						break;
					case "MD":
						rtncmd = cmdlist.MD(suffix);
						break;
					case "MF":
						break;
					case "MG":
						rtncmd = cmdlist.MG(suffix);
						break;
					case "ML":
						break;
					case "MO":
						rtncmd = cmdlist.MO(suffix);
						break;
					case "MR":
						break;
					case "MU":
						break;
					case "MW":
						break;
					case "NB":
						rtncmd = cmdlist.NB(suffix);
						break;
					case "NL":
						break;
					case "NR":
						break;
					case "NT":
						rtncmd = cmdlist.NT(suffix);
						break;
					case "OF":
						break;
					case "OI":
						break;
					case "OS":
						break;
					case "PA":
						break;
					case "PB":
						break;
					case "PC":
						rtncmd = cmdlist.PC(suffix);
						break;
					case "PI":
						break;
					case "PK":
						break;
					case "PL":
						break;
					case "PM":
						break;
					case "PR":
						rtncmd = cmdlist.PR(suffix);
						break;
					case "PS":
						rtncmd = cmdlist.PS(suffix);
						break;
					case "QC":
						break;
					case "QI":
						rtncmd = cmdlist.QI();
						break;
					case "QR":
						break;
					case "RA":
						break;
					case "RC":
						rtncmd = cmdlist.RC();
						break;
					case "RD":
						break;
					case "RG":
						break;
					case "RL":
						break;
					case "RM":
						break;
					case "RT":
						rtncmd = cmdlist.RT(suffix);
						break;
					case "RU":
						break;
					case "RX":
						rtncmd = cmdlist.RX(suffix);
						break;
					case "SA":
						break;
					case "SB":
						break;
					case "SC":
						break;
					case "SD":
						break;
					case "SH":
						rtncmd = cmdlist.SH(suffix);
						break;
					case "SI":
						break;
					case "SL":
						rtncmd = cmdlist.SL(suffix);
						break;
					case "SM":
						rtncmd = cmdlist.SM(suffix);
						break;
					case "SQ":
						rtncmd = cmdlist.SQ(suffix);
						break;
					case "SR":
						break;
					case "SS":
						break;
					case "ST":
						break;
					case "SU":
						break;
					case "SV":
						break;
					case "TC":
						break;
					case "TD":
						break;
					case "TI":
						break;
					case "TN":
						break;
					case "TO":
						break;
					case "TS":
						break;
					case "TX":
						rtncmd = cmdlist.TX(suffix);
						break;
					case "TY":
						break;
					case "UL":
						break;
					case "UP":
						break;
					case "VD":
						break;
					case "VG":
						break;
					case "VR":
						break;
					case "VX":
						break;
					case "XT":
						rtncmd = cmdlist.XT(suffix);
						break;
					case "ZZ":
						rtncmd = ParseExtended();
						break;
				}
				if(prefix != "ZZ")	// if this is a standard command
				{
					// and it's not an error
					if(rtncmd != Error1 && rtncmd != Error2 && rtncmd != Error3)
					{													
						// if it has the correct length
						if(rtncmd.Length == nAns)	
							rtncmd = prefix+rtncmd+";";	// return the formatted CAT answer
						else if(nAns == -1 || rtncmd == "")	// no answer is required
							rtncmd = "";
						else
							rtncmd = Error3;	// processing incomplete for some reason
					}
				}
			}
			else
				rtncmd = Error1;	// this was a bad command

			return rtncmd;	// Read successfully executed
		}

		private bool CheckFormat()
		{
			bool goodprefix,goodsuffix;
			// If there is no terminator, or the prefix or suffix
			// is invalid, abort.

			// If the command has a leading terminator(s) (like sent by WriteLog)
			// dump it and check the rest of the command.
			if(current_cat.StartsWith(";"))
				current_cat = current_cat.TrimStart(term);

			// If there is no terminator, or the prefix
			// is invalid, abort.
			if(current_cat.IndexOfAny(term) < 2)
				return false;

			// Now check to see if it's an extended command
			if(current_cat.Substring(0,2).ToUpper() == "ZZ")
				IsExtended = true;
			else
				IsExtended = false;

			// Check the prefix
			goodprefix = FindPrefix();
			if(!goodprefix)
				return false;

			// Check the suffix
			goodsuffix = FindSuffix();
			if(!goodsuffix)
				return false;

			return true;
		}


		private bool FindPrefix()
		{
			string pfx = "";

			// Extract the prefix from the command string
			if(IsExtended)
				pfx = current_cat.Substring(0,4).ToUpper();
			else
				pfx = current_cat.Substring(0,2).ToUpper();

			try
			// Find the prefix in the xml document and get the parameter
			// values.
			{
			XmlNode struc;
			XmlElement root = doc.DocumentElement;
			string search = "descendant::catstruct[@code='"+pfx+"']";
			struc = root.SelectSingleNode(search);
				if(struc != null)
				{
					foreach(XmlNode x in struc)
					{
						switch(x.Name)
						{
							case "active":
								IsActive = Convert.ToBoolean(x.InnerXml);
								break;
							case "nsetparms":
								nSet = Convert.ToInt16(x.InnerXml);
								break;
							case "ngetparms":
								nGet = Convert.ToInt16(x.InnerXml);
								break;
							case "nansparms":
								nAns = Convert.ToInt16(x.InnerXml);
								break;
						}
					}
//					prefix = pfx;
					// If this is not an active command there is no use continuing.
					if(IsActive)
					{
						if(IsExtended)
						{
							prefix = pfx.Substring(0,2);
							extension = pfx.Substring(2,2);
						}
						else
							prefix = pfx;
						return true;
					}
					else
					{
						return false;
					}
				}
			}
			catch(Exception e)
			{
				throw(e);
			}
			return false;
		}	

		private bool FindSuffix()
		{
			string sfx;
			int len = 3;
			int start = 2;
			int end = 2;

            if(IsExtended)
			{
				len = 5;
				start = 4;
				end = 4;
			}

			// Define the suffix as everything after the prefix and before
			// the first terminator.
			if(current_cat.Length > len)
			{
				sfx = current_cat.Substring(start,current_cat.IndexOf(";")-end);
				// Check the suffix for illegal characters
				// [^0-9] = match any non-numeric character
				Regex sfxpattern = new Regex("[^0-9]");
				if(sfxpattern.IsMatch(sfx))
					return false;
			}
			else
			{
				sfx = "";
			}

			// Check the length against the struct requirements
			if(sfx.Length == nSet | sfx.Length == nGet)
			{
				suffix = sfx;
				return true;
			}
			return false;
		}

		private string ParseExtended()
		{
			string rtncmd = Error1;
			string extended = prefix+extension;

			switch(extended)
			{
				case "ZZAG":
					rtncmd = cmdlist.ZZAG(suffix);
					break;
				case "ZZBI":
					rtncmd = cmdlist.ZZBI(suffix);
					break;
				case "ZZBG":
					rtncmd = cmdlist.ZZBG(suffix);
					break;
				case "ZZBS":
					rtncmd = cmdlist.ZZBS(suffix);
					break;
				case "ZZCL":
					rtncmd = cmdlist.ZZCL(suffix);
					break;
				case "ZZCP":
					rtncmd = cmdlist.ZZCP(suffix);
					break;
				case "ZZCS":
					rtncmd = cmdlist.ZZCS(suffix);
					break;
				case "ZZCU":
					rtncmd = cmdlist.ZZCU();
					break;
				case "ZZDA":
					rtncmd = cmdlist.ZZDA(suffix);
					break;
				case "ZZDM":
					rtncmd = cmdlist.ZZDM(suffix);
					break;
				case "ZZFI":
					rtncmd = cmdlist.ZZFI(suffix);
					break;
				case "ZZGT":
					rtncmd = cmdlist.ZZGT(suffix);
					break;
				case "ZZID":
					rtncmd = cmdlist.ZZID();
					break;
				case "ZZIF":
					rtncmd = cmdlist.ZZIF(suffix);
					break;
				case "ZZIS":
					rtncmd = cmdlist.ZZIS(suffix);
					break;
				case "ZZIT":
					rtncmd = cmdlist.ZZIT(suffix);
					break;
				case "ZZIU":
					rtncmd = cmdlist.ZZIU();
					break;
				case "ZZMA":
					rtncmd = cmdlist.ZZMA(suffix);
					break;
				case "ZZMD":
					rtncmd = cmdlist.ZZMD(suffix);
					break;
				case "ZZMG":
					//					rtncmd = cmdlist.ZZMG(suffix);
					break;
				case "ZZMR":
					rtncmd = cmdlist.ZZMR(suffix);
					break;
				case "ZZMT":
					rtncmd = cmdlist.ZZMT(suffix);
					break;
				case "ZZNB":
					rtncmd = cmdlist.ZZNB(suffix);
					break;
				case "ZZNL":
					rtncmd = cmdlist.ZZNL(suffix);
					break;
				case "ZZNM":
					rtncmd = cmdlist.ZZNM(suffix);
					break;
				case "ZZNR":
					rtncmd = cmdlist.ZZNR(suffix);
					break;
				case "ZZPA":
					rtncmd = cmdlist.ZZPA(suffix);
					break;
				case "ZZPL":
					rtncmd = cmdlist.ZZPL(suffix);
					break;
				case "ZZQM":
					rtncmd = cmdlist.ZZQM();
					break;
				case "ZZQR":
					rtncmd = cmdlist.ZZQR();
					break;
				case "ZZRF":
					rtncmd = cmdlist.ZZRF(suffix);
					break;
				case "ZZRM":
					rtncmd = cmdlist.ZZRM(suffix);
					break;
				case "ZZSF": 
					rtncmd = cmdlist.ZZSF(suffix);
					break;
				case "ZZSM":
					rtncmd = cmdlist.ZZSM(suffix);
					break;
				case "ZZSO":
					rtncmd = cmdlist.ZZSO(suffix);
					break;
				case "ZZSP":
					rtncmd = cmdlist.ZZSP(suffix);
					break; 
				case "ZZSQ":
					rtncmd = cmdlist.ZZSQ(suffix);
					break;
				case "ZZST":
					rtncmd = cmdlist.ZZST();
					break;
				case "ZZTH":
					rtncmd = cmdlist.ZZTH(suffix);
					break;
				case "ZZTL":
					rtncmd = cmdlist.ZZTL(suffix);
					break;
				case "ZZTU":
					rtncmd = cmdlist.ZZTU(suffix);
					break;
				case "ZZVL":
					rtncmd = cmdlist.ZZVL(suffix);
					break;
				case "ZZVN":
					rtncmd = cmdlist.ZZVN();
					break;
				case "ZZVS":
					rtncmd = cmdlist.ZZVS(suffix);
					break;
				case "ZZXC":
					rtncmd = cmdlist.ZZXC();
					break;
				case "ZZXF":
					rtncmd = cmdlist.ZZXF(suffix);
					break;

			}
			if(rtncmd != Error1 && rtncmd != Error2 && rtncmd != Error3)
			{
				if(rtncmd.Length == nAns)
					rtncmd = prefix+extension+suffix+rtncmd+";";
			}
			else
				rtncmd = Error1;

			return rtncmd;
		}
	}

	#endregion CATParser Class

}
