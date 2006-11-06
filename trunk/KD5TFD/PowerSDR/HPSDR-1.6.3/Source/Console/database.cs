//=================================================================
// database.cs
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

using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using ADOX;

namespace PowerSDR
{
	class DB
	{
		#region Variable Declaration

		private static OleDbConnection conn;

		private static OleDbDataAdapter bandTextAdapter;
		private static OleDbDataAdapter bandStackAdapter;
		private static OleDbDataAdapter optionsAdapter;
		private static OleDbDataAdapter memoryAdapter;
		private static OleDbDataAdapter groupAdapter;
		private static OleDbDataAdapter txProfileAdapter;

		private static OleDbCommandBuilder bandTextCB;
		private static OleDbCommandBuilder bandStackCB;
		private static OleDbCommandBuilder optionsCB;
		private static OleDbCommandBuilder memoryCB;
		private static OleDbCommandBuilder groupCB;
		private static OleDbCommandBuilder txProfileCB;

		private static DataSet dsBandText;
		private static DataSet dsBandStack;
		private static DataSet dsOptions;
		public static DataSet dsMemory;
		public static DataSet dsTxProfile;

		#endregion

		#region Private Member Functions
// ======================================================
// Private Member Functions
// ======================================================

		private static void Create()
		{
			// create the database file
			Catalog cat = new Catalog();
			cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=PowerSDR.mdb;" +
				"Jet OLEDB:Engine Type=5");

			// Create the tables
			AddStateTable(ref cat);
			AddOptionsTable(ref cat);
			AddCWTable(ref cat);
			AddCWXTable(ref cat);
			AddBandTextTable(ref cat);
			AddBandStackTable(ref cat);
			AddMemoryTable(ref cat);
			AddGroupListTable(ref cat);
			AddUCBTable(ref cat);
			AddXVTRTable(ref cat);
			AddTxProfileTable(ref cat);
			AddEQFormTable(ref cat);
			AddProdTestTable(ref cat);
			AddWaveOptionsTable(ref cat);

			cat = null;
		}

		private static void FillTables()
		{
			// Connect to the database
			OleDbConnection connection = new OleDbConnection();
			connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Persist Security Info=False;" +
				"Data source=PowerSDR.mdb";
			connection.Open();

			// Add Multiple Field Primary Keys
			AddMemoryPK(ref connection);
			AddBandStackPK(ref connection);

			// initialize tables
			FillBandStack(ref connection);
			FillBandText(ref connection);
			FillGroupList(ref connection);
			FillTxProfile(ref connection);
			connection.Close();
		}

		private static void AddStateTable(ref Catalog c)
		{
			// State Table
			Table stateTable = new Table();
			stateTable.Name = "State";
			stateTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			stateTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			stateTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(stateTable);
		}

		private static void AddOptionsTable(ref Catalog c)
		{
			Table optionsTable = new Table();
			optionsTable.Name = "Options";
			optionsTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			optionsTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);   
			optionsTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(optionsTable);
		}

		private static void AddCWTable(ref Catalog c)
		{
			Table cwTable = new Table();
			cwTable.Name = "CW";
			cwTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			cwTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);   
			cwTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(cwTable);
		}
		private static void AddCWXTable(ref Catalog c)
		{
			Table cwxTable = new Table();
			cwxTable.Name = "CWX";
			cwxTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			cwxTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);   
			cwxTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(cwxTable);
		}
		private static void AddBandTextTable(ref Catalog c)
		{
			Table bandTextTable = new Table();
			bandTextTable.Name = "BandText";
			bandTextTable.Columns.Append("Low", DataTypeEnum.adDouble, 1);
			bandTextTable.Columns.Append("High", DataTypeEnum.adDouble, 1);
			bandTextTable.Columns.Append("Name", DataTypeEnum.adVarWChar, 50);
			bandTextTable.Columns.Append("Transmit", DataTypeEnum.adVarWChar, 5);
			bandTextTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "Low", null, null);
			c.Tables.Append(bandTextTable);
		}

		private static void AddBandStackTable(ref Catalog c)
		{
			Table bandStackTable = new Table();
			bandStackTable.Name = "BandStack";
			bandStackTable.Columns.Append("BandName", DataTypeEnum.adVarWChar, 50);
			bandStackTable.Columns.Append("Mode", DataTypeEnum.adVarWChar, 50);
			bandStackTable.Columns.Append("Filter", DataTypeEnum.adVarWChar, 50);
			bandStackTable.Columns.Append("Frequency", DataTypeEnum.adDouble, 6);
			bandStackTable.Indexes.Append("PrimaryIndex", "BandName");
			c.Tables.Append(bandStackTable);
		}

		private static void AddBandStackPK(ref OleDbConnection connection)
		{
			// set multiple field Primary Key for Band Memory
			string pkcmd = "alter table BandStack add constraint pk_BandStack primary key (BandName, Frequency)";

			OleDbCommand cmd = new OleDbCommand(pkcmd, connection);
			cmd.ExecuteNonQuery();
		}

		private static void AddMemoryTable(ref Catalog c)
		{
			Table memoryTable = new Table();
			memoryTable.Name = "Memory";
			memoryTable.Columns.Append("GroupID", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("Frequency", DataTypeEnum.adDouble, 6);
			memoryTable.Columns.Append("ModeID", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("FilterID", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("Callsign", DataTypeEnum.adVarWChar, 50);
			memoryTable.Columns.Append("Comments", DataTypeEnum.adVarWChar, 100);
			memoryTable.Columns.Append("Scan", DataTypeEnum.adBoolean, 8);
			memoryTable.Columns.Append("Squelch", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("StepSizeID", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("AGCID", DataTypeEnum.adInteger, 50);
			memoryTable.Columns.Append("Gain", DataTypeEnum.adVarWChar, 50);
			memoryTable.Columns.Append("FilterLow", DataTypeEnum.adInteger, 10);
			memoryTable.Columns.Append("FilterHigh", DataTypeEnum.adInteger, 10);
			memoryTable.Columns.Append("CreateDate", DataTypeEnum.adVarWChar, 50);
			//memoryTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "CreateDate", null, null);
			c.Tables.Append(memoryTable);
		}

		private static void AddMemoryPK(ref OleDbConnection connection)
		{
			// set multiple field Primary Key for Band Memory
			string pkcmd = "alter table Memory add constraint pk_Memory primary key (Frequency, CreateDate)";

			OleDbCommand cmd = new OleDbCommand(pkcmd, connection);
			cmd.ExecuteNonQuery();
		}

		private static void AddGroupListTable(ref Catalog c)
		{
			Table groupTable = new Table();
			groupTable.Name = "GroupList";
			groupTable.Columns.Append("GroupID", DataTypeEnum.adInteger, 50);
			groupTable.Columns.Append("GroupName", DataTypeEnum.adVarWChar, 100);
			groupTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "GroupID", null, null);
			c.Tables.Append(groupTable);
		}

		private static void AddUCBTable(ref Catalog c)
		{
			Table ucbTable = new Table();
			ucbTable.Name = "UCB";
			ucbTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			ucbTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			ucbTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(ucbTable);
		}

		private static void AddXVTRTable(ref Catalog c)
		{
			Table xvtrTable = new Table();
			xvtrTable.Name = "XVTR";
			xvtrTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			xvtrTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			xvtrTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(xvtrTable);
		}

		private static void AddTxProfileTable(ref Catalog c)
		{
			Table txProfileTable = new Table();
			txProfileTable.Name = "TxProfile";
			txProfileTable.Columns.Append("Name", DataTypeEnum.adVarWChar, 50);
			txProfileTable.Columns.Append("FilterLow", DataTypeEnum.adInteger, 10);
			txProfileTable.Columns.Append("FilterHigh", DataTypeEnum.adInteger, 10);
			txProfileTable.Columns.Append("TXEQNumBands", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQEnabled", DataTypeEnum.adBoolean, 8);
			txProfileTable.Columns.Append("TXEQPreamp", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ1", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ2", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ3", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ4", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ5", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ6", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ7", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ8", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ9", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ10", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ11", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ12", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ13", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ14", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ15", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ16", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ17", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ18", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ19", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ20", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ21", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ22", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ23", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ24", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ25", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ26", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ27", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ28", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ29", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ30", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("TXEQ31", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("CompressorOn", DataTypeEnum.adBoolean, 8);
			txProfileTable.Columns.Append("CompressorLevel", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("CompanderOn", DataTypeEnum.adBoolean, 8);
			txProfileTable.Columns.Append("CompanderLevel", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("MicGain", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_Slope", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_MaxGain", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_Attack", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_Decay", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_Hang", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Lev_HangThreshold", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_Slope", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_MaxGain", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_Attack", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_Decay", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_Hang", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("ALC_HangThreshold", DataTypeEnum.adInteger, 5);
			txProfileTable.Columns.Append("Power", DataTypeEnum.adInteger, 5);

			txProfileTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "Name", null, null);

			c.Tables.Append(txProfileTable);
		}

		private static void AddEQFormTable(ref Catalog c)
		{
			Table eqFormTable = new Table();
			eqFormTable.Name = "EQForm";
			eqFormTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			eqFormTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			eqFormTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(eqFormTable);
		}

		private static void AddProdTestTable(ref Catalog c)
		{
			Table prodTestTable = new Table();
			prodTestTable.Name = "ProdTest";
			prodTestTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			prodTestTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			prodTestTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(prodTestTable);
		}

		private static void AddWaveOptionsTable(ref Catalog c)
		{
			// State Table
			Table waveOptionsTable = new Table();
			waveOptionsTable.Name = "WaveOptions";
			waveOptionsTable.Columns.Append("ID", DataTypeEnum.adVarWChar, 50);
			waveOptionsTable.Columns.Append("SaveValue", DataTypeEnum.adVarWChar, 120);
			waveOptionsTable.Keys.Append("PrimaryKey", KeyTypeEnum.adKeyPrimary, "ID", null, null);
			c.Tables.Append(waveOptionsTable);
		}

		private static void FillBandStack(ref OleDbConnection conn)
		{
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
			myDataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM BandStack", conn);
			OleDbCommandBuilder cb = new OleDbCommandBuilder(myDataAdapter);
			DataSet ds = new DataSet();
			myDataAdapter.Fill(ds, "BandStack");			

			string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

			object[] data = {
								"160M", "CWL", "F5", 1.810000,
								"160M", "CWU", "F1", 1.835000,
								"160M", "USB", "F6", 1.845000,
								"80M", "CWL", "F1", 3.501000,
								"80M", "LSB", "F6", 3.751000,
								"80M", "LSB", "F6", 3.850000,
								"60M", "USB", "F6", 5.330500,
								"60M", "USB", "F6", 5.346500,
								"60M", "USB", "F6", 5.366500,
								"60M", "USB", "F6", 5.371500,
								"60M", "USB", "F6", 5.403500,
								"40M", "CWL", "F1", 7.001000,
								"40M", "LSB", "F6", 7.152000,
								"40M", "LSB", "F6", 7.255000,
								"30M", "CWU", "F1", 10.120000,
								"30M", "CWU", "F1", 10.130000,
								"30M", "CWU", "F5", 10.140000,
								"20M", "CWU", "F1", 14.010000,
								"20M", "USB", "F6", 14.230000,
								"20M", "USB", "F6", 14.336000,
								"17M", "CWU", "F1", 18.068600,
								"17M", "USB", "F6", 18.125000,
								"17M", "USB", "F6", 18.140000,
								"15M", "CWU", "F1", 21.001000,
								"15M", "USB", "F6", 21.255000,
								"15M", "USB", "F6", 21.300000,
								"12M", "CWU", "F1", 24.890600,
								"12M", "USB", "F6", 24.900000,
								"12M", "USB", "F6", 24.910000,
								"10M", "CWU", "F1", 28.010000,
								"10M", "USB", "F6", 28.300000,
								"10M", "USB", "F6", 28.400000,
								"6M", "CWU", "F1", 50.010000,
								"6M", "USB", "F6", 50.125000,
								"6M", "USB", "F6", 50.200000,								
								"2M", "CWU", "F1", 144.010000,
								"2M", "USB", "F6", 144.200000,
								"2M", "USB", "F6", 144.210000,
								"WWV", "SAM", "F7", 2.500000,
								"WWV", "SAM", "F7", 5.000000,
								"WWV", "SAM", "F7", 10.000000,
								"WWV", "SAM", "F7", 15.000000,
								"WWV", "SAM", "F7", 20.000000,
								"GEN", "SAM", "F6", 13.845000,
								"GEN", "SAM", "F7", 5.975000,
								"GEN", "SAM", "F7", 9.550000,
								"GEN", "SAM", "F8", 0.590000,
								"GEN", "SAM", "F7", 3.850000,
			};

			for(int i=0; i<data.Length/4; i++)
			{
				DataRow dr = ds.Tables["BandStack"].NewRow();
				dr[0] = (string)data[i*4+0];
				dr[1] = (string)data[i*4+1];
				dr[2] = (string)data[i*4+2];
				dr[3] = ((double)data[i*4+3]).ToString("f6");
				ds.Tables["BandStack"].Rows.Add(dr);
			}

			// save the updates
			myDataAdapter.Update(ds, "BandStack");	
		}

		private static void FillBandText(ref OleDbConnection conn)
		{
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
			myDataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM BandText", conn);
			OleDbCommandBuilder cb = new OleDbCommandBuilder(myDataAdapter);
			DataSet ds = new DataSet();
			myDataAdapter.Fill(ds, "BandText");
			
			object[] data = {
								1.800000, 1.809999, "160M CW/Digital Modes",	"True",
								1.810000, 1.810000, "160M CW QRP",				"True",
								1.810001, 1.842999, "160M CW",					"True",
								1.843000, 1.909999, "160M SSB/SSTV/Wide Band",	"True",
								1.910000, 1.910000, "160M SSB QRP",				"True",
								1.910001, 1.994999, "160M SSB/SSTV/Wide Band",	"True",
								1.995000, 1.999999, "160M Experimental",		"True",

								3.500000, 3.524999, "80M Extra CW",				"True",
								3.525000, 3.579999, "80M CW",					"True",
								3.580000, 3.589999, "80M RTTY",					"True",
								3.590000, 3.590000, "80M RTTY DX",				"True",
								3.590001, 3.619999, "80M RTTY",					"True",
								3.620000, 3.644999, "80M Packet",				"True",
								3.645000, 3.749999, "80M CW",					"True",
								3.750000, 3.774999, "75M Extra SSB",			"True",
								3.775000, 3.789999, "75M Ext/Adv SSB",			"True",
								3.790000, 3.799999, "75M Ext/Adv DX Window",	"True",
								3.800000, 3.844999, "75M SSB",					"True",
								3.845000, 3.845000, "75M SSTV",					"True",
								3.845001, 3.884999, "75M SSB",					"True",
								3.885000, 3.885000, "75M AM Calling Frequency", "True",
								3.885001, 3.999999, "75M SSB",					"True",

								5.330500, 5.330500, "60M Channel 1",			"True",
								5.346500, 5.346500, "60M Channel 2",			"True",
								5.366500, 5.366500, "60M Channel 3",			"True",
								5.371500, 5.371500, "60M Channel 4",			"True",
								5.403500, 5.403500, "60M Channel 5",			"True",
								
								7.000000, 7.024999, "40M Extra CW",				"True",
								7.025000, 7.039999, "40M CW",					"True",
								7.040000, 7.040000, "40M RTTY DX",				"True",
								7.040001, 7.099999, "40M RTTY",					"True",
								7.100000, 7.149999, "40M CW",					"True",
								7.150000, 7.170999, "40M Ext/Adv SSB",			"True",
								7.171000, 7.171000, "40M SSTV",					"True",
								7.171001, 7.224999, "40M Ext/Adv SSB",			"True",
								7.225000, 7.289999, "40M SSB",					"True",
								7.290000, 7.290000, "40M AM Calling Frequency", "True",
								7.290001, 7.299999, "40M SSB",					"True",
								
								10.100000, 10.129999, "30M CW",					"True",
								10.130000, 10.139999, "30M RTTY",				"True",
								10.140000, 10.149999, "30M Packet",				"True",

								14.000000, 14.024999, "20M Extra CW",			"True",
								14.025000, 14.069999, "20M CW",					"True",
								14.070000, 14.094999, "20M RTTY",				"True",
								14.095000, 14.099499, "20M Packet",				"True",
								14.099500, 14.099999, "20M CW",					"True",
								14.100000, 14.100000, "20M NCDXF Beacons",		"True",
								14.100001, 14.100499, "20M CW",					"True",
								14.100500, 14.111999, "20M Packet",				"True",
								14.112000, 14.149999, "20M CW",					"True",
								14.150000, 14.174999, "20M Extra SSB",			"True",
								14.175000, 14.224999, "20M Ext/Adv SSB",		"True",
								14.225000, 14.229999, "20M SSB",				"True",
								14.230000, 14.230000, "20M SSTV",				"True",
								14.230001, 14.285999, "20M SSB",				"True",
								14.286000, 14.286000, "20M AM Calling Frequency", "True",
								14.286001, 14.349999, "20M SSB",				"True",
								
								18.068000, 18.099999, "17M CW",					"True",
								18.100000, 18.104999, "17M RTTY",				"True",
								18.105000, 18.109999, "17M Packet",				"True",
								18.110000, 18.110000, "17M NCDXF Beacons",		"True",
								18.110001, 18.167999, "17M SSB",				"True",
								
								21.000000, 21.024999, "15M Extra CW",			"True",
								21.025000, 21.069999, "15M CW",					"True",
								21.070000, 21.099999, "15M RTTY",				"True",
								21.100000, 21.109999, "15M Packet",				"True",
								21.110000, 21.149999, "15M CW",					"True",
								21.150000, 21.150000, "15M NCDXF Beacons",		"True",
								21.150001, 21.199999, "15M CW",					"True",
								21.200000, 21.224999, "15M Extra SSB",			"True",
								21.225000, 21.299999, "15M Ext/Adv SSB",		"True",
								21.300000, 21.339999, "15M SSB",				"True",
								21.340000, 21.340000, "15M SSTV",				"True",
								21.340001, 21.449999, "15M SSB",				"True",
								
								24.890000, 24.919999, "12M CW",					"True",
								24.920000, 24.924999, "12M RTTY",				"True",
								24.925000, 24.929999, "12M Packet",				"True",
								24.930000, 24.930000, "12M NCDXF Beacons",		"True",
								24.930001, 24.989999, "12M SSB Wideband",		"True",
								
								28.000000, 28.069999, "10M CW",					"True",
								28.070000, 28.149999, "10M RTTY",				"True",
								28.150000, 28.199999, "10M CW",					"True",
								28.200000, 28.200000, "10M NCDXF Beacons",		"True",
								28.200001, 28.299999, "10M Beacons",			"True",
								28.300000, 28.679999, "10M SSB",				"True",
								28.680000, 28.680000, "10M SSTV",				"True",
								28.680001, 28.999999, "10M SSB",				"True",
								29.000000, 29.199999, "10M AM",					"True",
								29.200000, 29.299999, "10M SSB",				"True",
								29.300000, 29.509999, "10M Satellite Downlinks", "True",
								29.510000, 29.519999, "10M Deadband",			"True",
								29.520000, 29.589999, "10M Repeater Inputs",	"True",
								29.590000, 29.599999, "10M Deadband",			"True",
								29.600000, 29.600000, "10M FM Simplex",			"True",
								29.600001, 29.609999, "10M Deadband",			"True",
								29.610000, 29.699999, "10M Repeater Outputs",	"True",
								
								50.000000, 50.059999, "6M CW",					"True",
								50.060000, 50.079999, "6M Beacon Sub-Band",		"True",
								50.080000, 50.099999, "6M CW",					"True",
								50.100000, 50.124999, "6M DX Window",			"True",
								50.125000, 50.125000, "6M Calling Frequency",	"True",
								50.125001, 50.299999, "6M SSB",					"True",
								50.300000, 50.599999, "6M All Modes",			"True",
								50.600000, 50.619999, "6M Non Voice",			"True",
								50.620000, 50.620000, "6M Digital Packet Calling", "True",
								50.620001, 50.799999, "6M Non Voice",			"True",
								50.800000, 50.999999, "6M RC",					"True",
								51.000000, 51.099999, "6M Pacific DX Window",	"True",
								51.100000, 51.119999, "6M Deadband",			"True",
								51.120000, 51.179999, "6M Digital Repeater Inputs", "True",
								51.180000, 51.479999, "6M Repeater Inputs",		"True",
								51.480000, 51.619999, "6M Deadband",			"True",
								51.620000, 51.679999, "6M Digital Repeater Outputs", "True",
								51.680000, 51.979999, "6M Repeater Outputs",	"True",
								51.980000, 51.999999, "6M Deadband",			"True",
								52.000000, 52.019999, "6M Repeater Inputs",		"True",
								52.020000, 52.020000, "6M FM Simplex",			"True",
								52.020001, 52.039999, "6M Repeater Inputs",		"True",
								52.040000, 52.040000, "6M FM Simplex",			"True",
								52.040001, 52.479999, "6M Repeater Inputs",		"True",
								52.480000, 52.499999, "6M Deadband",			"True",
								52.500000, 52.524999, "6M Repeater Outputs",	"True",
								52.525000, 52.525000, "6M Primary FM Simplex",	"True",
								52.525001, 52.539999, "6M Deadband",			"True",
								52.540000, 52.540000, "6M Secondary FM Simplex", "True",
								52.540001, 52.979999, "6M Repeater Outputs",	"True",
								52.980000, 52.999999, "6M Deadbands",			"True",
								53.000000, 53.000000, "6M Remote Base FM Spx",	"True",
								53.000001, 53.019999, "6M Repeater Inputs",		"True",
								53.020000, 53.020000, "6M FM Simplex",			"True",
								53.020001, 53.479999, "6M Repeater Inputs",		"True",
								53.480000, 53.499999, "6M Deadband",			"True",
								53.500000, 53.519999, "6M Repeater Outputs",	"True",
								53.520000, 53.520000, "6M FM Simplex",			"True",
								53.520001, 53.899999, "6M Repeater Outputs",	"True",
								53.900000, 53.900000, "6M FM Simplex",			"True",
								53.900010, 53.979999, "6M Repeater Outputs",	"True",
								53.980000, 53.999999, "6M Deadband",			"True",
								
								144.000000, 144.099999, "2M CW",				"True",
								144.100000, 144.199999, "2M CW/SSB",			"True",
								144.200000, 144.200000, "2M Calling",			"True",
								144.200001, 144.274999, "2M CW/SSB",			"True",
								144.275000, 144.299999, "2M Beacon Sub-Band",	"True",
								144.300000, 144.499999, "2M Satellite",			"True",
								144.500000, 144.599999, "2M Linear Translator Inputs", "True",
								144.600000, 144.899999, "2M FM Repeater",		"True",
								144.900000, 145.199999, "2M FM Simplex",		"True",
								145.200000, 145.499999, "2M FM Repeater",		"True",
								145.500000, 145.799999, "2M FM Simplex",		"True",
								145.800000, 145.999999, "2M Satellite",			"True",
								146.000000, 146.399999, "2M FM Repeater",		"True",
								146.400000, 146.609999, "2M FM Simplex",		"True",
								146.610000, 147.389999, "2M FM Repeater",		"True",
								147.390000, 147.599999, "2M FM Simplex",		"True",
								147.600000, 147.999999, "2M FM Repeater",		"True",

								222.000000, 222.024999, "1.25M EME/Weak Signal", "True",
								222.025000, 222.049999, "1.25M Weak Signal",	"True",
								222.050000, 222.059999, "1.25M Propagation Beacons", "True",
								222.060000, 222.099999, "1.25M Weak Signal",	"True",
								222.100000, 222.100000, "1.25M SSB/CW Calling",	"True",
								222.100001, 222.149999, "1.25M Weak Signal CW/SSB", "True",
								222.150000, 222.249999, "1.25M Local Option",	"True",
								222.250000, 223.380000, "1.25M FM Repeater Inputs", "True",
								223.400000, 223.519999, "1.25M FM Simplex",		"True",
								223.520000, 223.639999, "1.25M Digital/Packet",	"True",
								223.640000, 223.700000, "1.25M Links/Control",	"True",
								223.710000, 223.849999, "1.25M Local Option",	"True",
								223.850000, 224.980000, "1.25M Repeater Outputs", "True",

								420.000000, 425.999999, "70CM ATV Repeater",	"True",
								426.000000, 431.999999, "70CM ATV Simplex",		"True",
								432.000000, 432.069999, "70CM EME",				"True",
								432.070000, 432.099999, "70CM Weak Signal CW",	"True",
								432.100000, 432.100000, "70CM Calling Frequency", "True",
								432.100001, 432.299999, "70CM Mixed Mode Weak Signal", "True",
								432.300000, 432.399999, "70CM Propagation Beacons", "True",
								432.400000, 432.999999, "70CM Mixed Mode Weak Signal", "True",
								433.000000, 434.999999, "70CM Auxillary/Repeater Links", "True",
								435.000000, 437.999999, "70CM Satellite Only",	"True",
								438.000000, 441.999999, "70CM ATV Repeater",	"True",
								442.000000, 444.999999, "70CM Local Repeaters",	"True",
								445.000000, 445.999999, "70CM Local Option",	"True",
								446.000000, 446.000000, "70CM Simplex",			"True",
								446.000001, 446.999999, "70CM Local Option",	"True",
								447.000000, 450.000000, "70CM Local Repeaters", "True",

								902.000000, 902.099999, "33CM Weak Signal SSTV/FAX/ACSSB", "True",
								902.100000, 902.100000, "33CM Weak Signal Calling", "True",
								902.100001, 902.799999, "33CM Weak Signal SSTV/FAX/ACSSB", "True",
								902.800000, 902.999999, "33CM Weak Signal EME/CW", "True",
								903.000000, 903.099999, "33CM Digital Modes",	"True",
								903.100000, 903.100000, "33CM Alternate Calling", "True",
								903.100001, 905.999999, "33CM Digital Modes",	"True",
								906.000000, 908.999999, "33CM FM Repeater Inputs", "True",
								909.000000, 914.999999, "33CM ATV",				"True",
								915.000000, 917.999999, "33CM Digital Modes",	"True",
								918.000000, 920.999999, "33CM FM Repeater Outputs", "True",
								921.000000, 926.999999, "33CM ATV",				"True",
								927.000000, 928.000000, "33CM FM Simplex/Links", "True",
								
								1240.000000, 1245.999999, "23CM ATV #1",		"True",
								1246.000000, 1251.999999, "23CM FMN Point/Links", "True",
								1252.000000, 1257.999999, "23CM ATV #2, Digital Modes", "True",
								1258.000000, 1259.999999, "23CM FMN Point/Links", "True",
								1260.000000, 1269.999999, "23CM Sat Uplinks/Wideband Exp.", "True",
								1270.000000, 1275.999999, "23CM Repeater Inputs", "True",
								1276.000000, 1281.999999, "23CM ATV #3",		"True",
								1282.000000, 1287.999999, "23CM Repeater Outputs",	"True",
								1288.000000, 1293.999999, "23CM Simplex ATV/Wideband Exp.", "True",
								1294.000000, 1294.499999, "23CM Simplex FMN",		"True",
								1294.500000, 1294.500000, "23CM FM Simplex Calling", "True",
								1294.500001, 1294.999999, "23CM Simplex FMN",		"True",
								1295.000000, 1295.799999, "23CM SSTV/FAX/ACSSB/Exp.", "True",
								1295.800000, 1295.999999, "23CM EME/CW Expansion",	"True",
								1296.000000, 1296.050000, "23CM EME Exclusive",		"True",
								1296.070000, 1296.080000, "23CM CW Beacons",		"True",
								1296.100000, 1296.100000, "23CM CW/SSB Calling",	"True",
								1296.400000, 1296.599999, "23CM X-Band Translator Input", "True",
								1296.600000, 1296.799999, "23CM X-Band Translator Output", "True",
								1296.800000, 1296.999999, "23CM Experimental Beacons", "True",
								1297.000000, 1300.000000, "23CM Digital Modes",		"True",

								2300.000000, 2302.999999, "2.3GHz High Data Rate", "True",
								2303.000000, 2303.499999, "2.3GHz Packet",		"True",
								2303.500000, 2303.800000, "2.3GHz TTY Packet",	"True",
								2303.900000, 2303.900000, "2.3GHz Packet/TTY/CW/EME", "True",
								2303.900001, 2304.099999, "2.3GHz CW/EME",		"True",
								2304.100000, 2304.100000, "2.3GHz Calling Frequency", "True",
								2304.100001, 2304.199999, "2.3GHz CW/EME/SSB",	"True",
								2304.200000, 2304.299999, "2.3GHz SSB/SSTV/FAX/Packet AM/Amtor", "True",
								2304.300000, 2304.319999, "2.3GHz Propagation Beacon Network", "True",
								2304.320000, 2304.399999, "2.3GHz General Propagation Beacons", "True",
								2304.400000, 2304.499999, "2.3GHz SSB/SSTV/ACSSB/FAX/Packet AM", "True",
								2304.500000, 2304.699999, "2.3GHz X-Band Translator Input", "True",
								2304.700000, 2304.899999, "2.3GHz X-Band Translator Output", "True",
								2304.900000, 2304.999999, "2.3GHz Experimental Beacons", "True",
								2305.000000, 2305.199999, "2.3GHz FM Simplex", "True",
								2305.200000, 2305.200000, "2.3GHz FM Simplex Calling", "True",
								2305.200001, 2305.999999, "2.3GHz FM Simplex", "True",
								2306.000000, 2308.999999, "2.3GHz FM Repeaters", "True",
								2309.000000, 2310.000000, "2.3GHz Control/Aux Links", "True",
								2390.000000, 2395.999999, "2.3GHz Fast-Scan TV", "True",
								2396.000000, 2398.999999, "2.3GHz High Rate Data", "True",
								2399.000000, 2399.499999, "2.3GHz Packet", "True",
								2399.500000, 2399.999999, "2.3GHz Control/Aux Links", "True",
								2400.000000, 2402.999999, "2.4GHz Satellite", "True",
								2403.000000, 2407.999999, "2.4GHz Satellite High-Rate Data", "True",
								2408.000000, 2409.999999, "2.4GHz Satellite", "True",
								2410.000000, 2412.999999, "2.4GHz FM Repeaters", "True",
								2413.000000, 2417.999999, "2.4GHz High-Rate Data", "True",
								2418.000000, 2429.999999, "2.4GHz Fast-Scan TV", "True",
								2430.000000, 2432.999999, "2.4GHz Satellite", "True",
								2433.000000, 2437.999999, "2.4GHz Sat. High-Rate Data", "True",
								2438.000000, 2450.000000, "2.4GHz Wideband FM/FSTV/FMTV", "True",

								3456.100000, 3456.100000, "3.4GHz Calling Frequency", "True",
								3456.300000, 3456.400000, "3.4GHz Propagation Beacons", "True",

								5760.100000, 5760.100000, "5.7GHz Calling Frequency", "True",
								5760.300000, 5760.400000, "5.7GHz Propagation Beacons", "True",

								10368.100000, 10368.100000, "10GHz Calling Frequency", "True",
								24192.100000, 24192.100000, "24GHz Calling Frequency", "True",
								47088.100000, 47088.100000, "47GHz Calling Frequency", "True",

								2.500000, 2.500000, "WWV",						"False",
								5.000000, 5.000000, "WWV",						"False",
								10.000000, 10.000000, "WWV",					"False",
								15.000000, 15.000000, "WWV",					"False",
								20.000000, 20.000000, "WWV",					"False",

								0.153000, 0.279000, "AM - Long Wave",			"False",
								0.530000, 1.710000, "Broadcast AM Med Wave",	"False",				
								2.300000, 2.495000, "120M Short Wave",			"False",
								3.200000, 3.400000, "90M Short Wave",			"False",
								4.750000, 4.999999, "60M Short Wave",			"False",
								5.000001, 5.060000, "60M Short Wave",			"False",
								5.900000, 6.200000, "49M Short Wave",			"False",
								7.300000, 7.350000, "41M Short Wave",			"False",
								9.400000, 9.900000, "31M Short Wave",			"False",
								11.600000, 12.100000, "25M Short Wave",			"False",
								13.570000, 13.870000, "22M Short Wave",			"False",
								15.100000, 15.800000, "19M Short Wave",			"False",
								17.480000, 17.900000, "16M Short Wave",			"False",
								18.900000, 19.020000, "15M Short Wave",			"False",
								21.450000, 21.850000, "13M Short Wave",			"False",
								25.600000, 26.100000, "11M Short Wave",			"False",
			};

			for(int i=0; i<data.Length/4; i++)
			{
				DataRow dr = ds.Tables["BandText"].NewRow();
				dr["Low"] = (double)data[i*4+0];
				dr["High"] = (double)data[i*4+1];
				dr["Name"] = (string)data[i*4+2];
				dr["Transmit"] = (string)data[i*4+3];
				ds.Tables["BandText"].Rows.Add(dr);
			}

			// save the updates
			myDataAdapter.Update(ds, "BandText");	
		}

		private static void FillGroupList(ref OleDbConnection conn)
		{
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
			myDataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM GroupList", conn);
			OleDbCommandBuilder cb = new OleDbCommandBuilder(myDataAdapter);
			DataSet ds = new DataSet();
			myDataAdapter.Fill(ds, "GroupList");

			DataRow dr = ds.Tables["GroupList"].NewRow();
			dr[0] = 0;
			dr[1] = "AM";
			ds.Tables["GroupList"].Rows.Add(dr);

			dr = ds.Tables["GroupList"].NewRow();
			dr[0] = 1;
			dr[1] = "FM";
			ds.Tables["GroupList"].Rows.Add(dr);

			dr = ds.Tables["GroupList"].NewRow();
			dr[0] = 2;
			dr[1] = "SSB";
			ds.Tables["GroupList"].Rows.Add(dr);

			dr = ds.Tables["GroupList"].NewRow();
			dr[0] = 3;
			dr[1] = "SSTV";
			ds.Tables["GroupList"].Rows.Add(dr);

			myDataAdapter.Update(ds, "GroupList");
		}

		private static void FillTxProfile(ref OleDbConnection conn)
		{
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
			myDataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM TxProfile", conn);
			OleDbCommandBuilder cb = new OleDbCommandBuilder(myDataAdapter);
			DataSet ds = new DataSet();
			myDataAdapter.Fill(ds, "TxProfile");

			DataRow dr = ds.Tables["TxProfile"].NewRow();
			dr["Name"] = "Conventional";
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 2900;
			dr["TXEQNumBands"] = 15;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
			dr["TXEQ1"] = 0;
			dr["TXEQ2"] = 0;
			dr["TXEQ3"] = 0;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["TXEQ11"] = 0;
			dr["TXEQ12"] = 0;
			dr["TXEQ13"] = 0;
			dr["TXEQ14"] = 0;
			dr["TXEQ15"] = 0;
			dr["TXEQ16"] = 0;
			dr["TXEQ17"] = 0;
			dr["TXEQ18"] = 0;
			dr["TXEQ19"] = 0;
			dr["TXEQ20"] = 0;
			dr["TXEQ21"] = 0;
			dr["TXEQ22"] = 0;
			dr["TXEQ23"] = 0;
			dr["TXEQ24"] = 0;
			dr["TXEQ25"] = 0;
			dr["TXEQ26"] = 0;
			dr["TXEQ27"] = 0;
			dr["TXEQ28"] = 0;
			dr["TXEQ29"] = 0;
			dr["TXEQ30"] = 0;
			dr["TXEQ31"] = 0;
			dr["CompressorOn"] = false;
			dr["CompressorLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 50;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 10;
			dr["Lev_Attack"] = 2;
			dr["Lev_Decay"] = 500;
			dr["Lev_Hang"] = 500;
			dr["Lev_HangThreshold"] = 0;
			dr["ALC_Slope"] = 0;
			dr["ALC_MaxGain"] = -20;
			dr["ALC_Attack"] = 2;
			dr["ALC_Decay"] = 10;
			dr["ALC_Hang"] = 500;
			dr["ALC_HangThreshold"] = 0;
			dr["Power"] = 50;
			ds.Tables["TxProfile"].Rows.Add(dr);

			dr = ds.Tables["TxProfile"].NewRow();
			dr["Name"] = "DX / Contest";
			dr["FilterLow"] = 300;
			dr["FilterHigh"] = 3300;
			dr["TXEQNumBands"] = 15;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
			dr["TXEQ1"] = 0;
			dr["TXEQ2"] = 0;
			dr["TXEQ3"] = 0;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["TXEQ11"] = 0;
			dr["TXEQ12"] = 0;
			dr["TXEQ13"] = 0;
			dr["TXEQ14"] = 0;
			dr["TXEQ15"] = 0;
			dr["TXEQ16"] = 0;
			dr["TXEQ17"] = 0;
			dr["TXEQ18"] = 0;
			dr["TXEQ19"] = 0;
			dr["TXEQ20"] = 0;
			dr["TXEQ21"] = 0;
			dr["TXEQ22"] = 0;
			dr["TXEQ23"] = 0;
			dr["TXEQ24"] = 0;
			dr["TXEQ25"] = 0;
			dr["TXEQ26"] = 0;
			dr["TXEQ27"] = 0;
			dr["TXEQ28"] = 0;
			dr["TXEQ29"] = 0;
			dr["TXEQ30"] = 0;
			dr["TXEQ31"] = 0;
			dr["CompressorOn"] = true;
			dr["CompressorLevel"] = 5;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 50;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 10;
			dr["Lev_Attack"] = 2;
			dr["Lev_Decay"] = 500;
			dr["Lev_Hang"] = 500;
			dr["Lev_HangThreshold"] = 0;
			dr["ALC_Slope"] = 0;
			dr["ALC_MaxGain"] = -20;
			dr["ALC_Attack"] = 2;
			dr["ALC_Decay"] = 10;
			dr["ALC_Hang"] = 500;
			dr["ALC_HangThreshold"] = 0;
			dr["Power"] = 50;
			ds.Tables["TxProfile"].Rows.Add(dr);

			dr = ds.Tables["TxProfile"].NewRow();
			dr["Name"] = "ESSB";
			dr["FilterLow"] = 50;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 15;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
			dr["TXEQ1"] = 0;
			dr["TXEQ2"] = 0;
			dr["TXEQ3"] = 0;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["TXEQ11"] = 0;
			dr["TXEQ12"] = 0;
			dr["TXEQ13"] = 0;
			dr["TXEQ14"] = 0;
			dr["TXEQ15"] = 0;
			dr["TXEQ16"] = 0;
			dr["TXEQ17"] = 0;
			dr["TXEQ18"] = 0;
			dr["TXEQ19"] = 0;
			dr["TXEQ20"] = 0;
			dr["TXEQ21"] = 0;
			dr["TXEQ22"] = 0;
			dr["TXEQ23"] = 0;
			dr["TXEQ24"] = 0;
			dr["TXEQ25"] = 0;
			dr["TXEQ26"] = 0;
			dr["TXEQ27"] = 0;
			dr["TXEQ28"] = 0;
			dr["TXEQ29"] = 0;
			dr["TXEQ30"] = 0;
			dr["TXEQ31"] = 0;
			dr["CompressorOn"] = false;
			dr["CompressorLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 50;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 10;
			dr["Lev_Attack"] = 2;
			dr["Lev_Decay"] = 500;
			dr["Lev_Hang"] = 500;
			dr["Lev_HangThreshold"] = 0;
			dr["ALC_Slope"] = 0;
			dr["ALC_MaxGain"] = -20;
			dr["ALC_Attack"] = 2;
			dr["ALC_Decay"] = 10;
			dr["ALC_Hang"] = 500;
			dr["ALC_HangThreshold"] = 0;
			dr["Power"] = 50;
			ds.Tables["TxProfile"].Rows.Add(dr);

			dr = ds.Tables["TxProfile"].NewRow();
			dr["Name"] = "AM";
			dr["FilterLow"] = 0;
			dr["FilterHigh"] = 4000;
			dr["TXEQNumBands"] = 15;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = 0;
			dr["TXEQ1"] = 0;
			dr["TXEQ2"] = 0;
			dr["TXEQ3"] = 0;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["TXEQ11"] = 0;
			dr["TXEQ12"] = 0;
			dr["TXEQ13"] = 0;
			dr["TXEQ14"] = 0;
			dr["TXEQ15"] = 0;
			dr["TXEQ16"] = 0;
			dr["TXEQ17"] = 0;
			dr["TXEQ18"] = 0;
			dr["TXEQ19"] = 0;
			dr["TXEQ20"] = 0;
			dr["TXEQ21"] = 0;
			dr["TXEQ22"] = 0;
			dr["TXEQ23"] = 0;
			dr["TXEQ24"] = 0;
			dr["TXEQ25"] = 0;
			dr["TXEQ26"] = 0;
			dr["TXEQ27"] = 0;
			dr["TXEQ28"] = 0;
			dr["TXEQ29"] = 0;
			dr["TXEQ30"] = 0;
			dr["TXEQ31"] = 0;
			dr["CompressorOn"] = false;
			dr["CompressorLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 50;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 10;
			dr["Lev_Attack"] = 2;
			dr["Lev_Decay"] = 500;
			dr["Lev_Hang"] = 500;
			dr["Lev_HangThreshold"] = 0;
			dr["ALC_Slope"] = 0;
			dr["ALC_MaxGain"] = -20;
			dr["ALC_Attack"] = 2;
			dr["ALC_Decay"] = 10;
			dr["ALC_Hang"] = 500;
			dr["ALC_HangThreshold"] = 0;
			dr["Power"] = 50;
			ds.Tables["TxProfile"].Rows.Add(dr);

			// save the updates
			myDataAdapter.Update(ds, "TxProfile");	
		}

		private static void CheckBandTextValid()
		{
			ArrayList bad_rows = new ArrayList();

			if(dsBandText == null) return;
			foreach(DataRow dr in dsBandText.Tables["BandText"].Rows)
			{
				// check low freq
				string f = ((double)dr["Low"]).ToString("f6");
				f = f.Replace(",", ".");
				DataRow[] rows = dsBandText.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}

				// check high freq
				f = ((double)dr["High"]).ToString("f6");
				f = f.Replace(",", ".");
				rows = dsBandText.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}
			}

            foreach(DataRow dr in bad_rows)
				dsBandText.Tables["BandText"].Rows.Remove(dr);
		}
		
		private static void VerifyTables()
		{
			ArrayList a = new ArrayList();

			string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data source=PowerSDR.mdb";

			ADODB.Connection adodb_conn = new ADODB.Connection();
//			adodb_conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
//				"Persist Security Info=False;" +
//				"Data source=PowerSDR.mdb";
			adodb_conn.Open(connectionStr, "", "", 0);

			Catalog cat = new Catalog();
			cat.ActiveConnection = adodb_conn;

			foreach(Table t in cat.Tables)
				a.Add(t.Name);

			if(a.BinarySearch("State") < 0)
				AddStateTable(ref cat);
			
			if(a.BinarySearch("Options") < 0)
				AddOptionsTable(ref cat);

			if(a.BinarySearch("CW") < 0)
				AddCWTable(ref cat);

			if(a.BinarySearch("CWX") < 0)
				AddCWXTable(ref cat);

			if(a.BinarySearch("BandText") < 0)
			{
				AddBandTextTable(ref cat);

				// Connect to the database
				OleDbConnection connection = new OleDbConnection();
				connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
					"Persist Security Info=False;" +
					"Data source=PowerSDR.mdb";
				connection.Open();

				FillBandText(ref connection);

				connection.Close();
			}

			if(a.BinarySearch("BandStack") < 0)
			{
				AddBandStackTable(ref cat);

				// Connect to the database
				OleDbConnection connection = new OleDbConnection();
				connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
					"Persist Security Info=False;" +
					"Data source=PowerSDR.mdb";
				connection.Open();

				FillBandStack(ref connection);	
			
				connection.Close();
			}

			if(a.BinarySearch("Memory") < 0)
				AddMemoryTable(ref cat);

			if(a.BinarySearch("GroupList") < 0)
			{
				AddGroupListTable(ref cat);

				// Connect to the database
				OleDbConnection connection = new OleDbConnection();
				connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
					"Persist Security Info=False;" +
					"Data source=PowerSDR.mdb";
				connection.Open();

				FillGroupList(ref connection);

				connection.Close();
			}

			if(a.BinarySearch("UCB") < 0)
				AddUCBTable(ref cat);

			if(a.BinarySearch("XVTR") < 0)
				AddXVTRTable(ref cat);

			if(a.BinarySearch("TxProfile") < 0)
			{
				AddTxProfileTable(ref cat);

				// Connect to the database
				OleDbConnection connection = new OleDbConnection();
				connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
					"Persist Security Info=False;" +
					"Data source=PowerSDR.mdb";
				connection.Open();

				FillTxProfile(ref connection);

				connection.Close();
			}

			if(a.BinarySearch("EQForm") < 0)
				AddEQFormTable(ref cat);

			if(a.BinarySearch("ProdTest") < 0)
				AddProdTestTable(ref cat);

			if(a.BinarySearch("WaveOptions") < 0)
				AddWaveOptionsTable(ref cat);

			cat = null;
		}

		#endregion

		#region Public Member Functions
// ======================================================
// Public Member Functions 
// ======================================================

		public static void Init()
		{
			if(!File.Exists("PowerSDR.mdb"))
			{
				Create();
				FillTables();
			}
			else
				VerifyTables();

			Connect(out conn);
			bandTextAdapter = new OleDbDataAdapter();
			bandTextAdapter.SelectCommand = new OleDbCommand("SELECT * FROM BandText", conn);
			bandTextCB = new OleDbCommandBuilder(bandTextAdapter);
			dsBandText = new DataSet();
			bandTextAdapter.Fill(dsBandText, "BandText");

			bandStackAdapter = new OleDbDataAdapter();
			bandStackAdapter.SelectCommand = new OleDbCommand("SELECT * FROM BandStack", conn);
			bandStackCB = new OleDbCommandBuilder(bandStackAdapter);
			dsBandStack = new DataSet();
			bandStackAdapter.Fill(dsBandStack, "BandStack");

			optionsAdapter = new OleDbDataAdapter();
			optionsAdapter.SelectCommand = new OleDbCommand("SELECT * FROM Options", conn);
			optionsCB = new OleDbCommandBuilder(optionsAdapter);
			dsOptions = new DataSet();
			optionsAdapter.Fill(dsOptions, "Options");

			memoryAdapter = new OleDbDataAdapter();
			memoryAdapter.SelectCommand = new OleDbCommand("SELECT * FROM Memory", conn);
			memoryCB = new OleDbCommandBuilder(memoryAdapter);
			dsMemory = new DataSet();
			memoryAdapter.Fill(dsMemory, "Memory");

			groupAdapter = new OleDbDataAdapter();
			groupAdapter.SelectCommand = new OleDbCommand("SELECT * FROM GroupList", conn);
			groupCB = new OleDbCommandBuilder(groupAdapter);
			groupAdapter.Fill(dsMemory, "GroupList");

			txProfileAdapter = new OleDbDataAdapter();
			txProfileAdapter.SelectCommand = new OleDbCommand("SELECT * FROM TxProfile", conn);
			txProfileCB = new OleDbCommandBuilder(txProfileAdapter);
			dsTxProfile = new DataSet();
			txProfileAdapter.Fill(dsTxProfile, "TxProfile");

			CheckBandTextValid();
		}

		private static void Connect(out OleDbConnection c)
		{
			c = new OleDbConnection();
			c.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Persist Security Info=False;" +
				"Data source="+Application.StartupPath+"\\PowerSDR.mdb";
			try
			{				
				c.Open();
			}
			catch (Exception)
			{
				MessageBox.Show("Dynamic Database creation failed.  Please update MDAC to version" +
					"2.8 or later.", "Database Not Found",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void Update()
		{
			optionsAdapter.Update(dsOptions, "Options");
			memoryAdapter.Update(dsMemory, "Memory");
			txProfileAdapter.Update(dsTxProfile, "TxProfile");
		}

		public static void Exit()
		{		
			Update();
			conn.Close();
		}

		public static bool BandText(double freq, out string outStr)
		{
			try
			{
				outStr = "";
				string f = freq.ToString("f6");
				f = f.Replace(",", ".");
				DataRow[] rows = dsBandText.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length == 0)		// band not found
				{
					outStr = "Out of Band";
					return false;
				}
				else if(rows.Length == 1)	// found band
				{
					outStr = ((string)rows[0]["Name"]);
					if(((string)rows[0]["Transmit"]).ToLower() == "true")
						return true;
					else if(((string)rows[0]["Transmit"]).ToLower() == "false")
						return false;
				}
				else //if(rows.Length > 1)	// this should never happen
				{
					MessageBox.Show("Error reading BandInfo table.", "Database Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					outStr = "Error";
					return false;
				}
				return false;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message + "\n\n\n" + e.StackTrace, "Database Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				outStr = "Error";
				return false;
			}
		}

		public static int[] GetBandStackNum()
		{
			string[] band_list = {"160M", "80M", "60M", "40M", "30M", "20M", "17M",
								  "15M", "12M", "10M", "6M", "2M", "WWV", "GEN",
								  "VHF0", "VHF1", "VHF2", "VHF3", "VHF4", "VHF5",
								  "VHF6", "VHF7", "VHF8", "VHF9", "VHF10", "VHF11",
								  "VHF12", "VHF13" };

			int[] retvals = new int[band_list.Length];

			for(int i=0; i<band_list.Length; i++)
			{
				string s = band_list[i];
				DataRow[] rows = dsBandStack.Tables["BandStack"].Select("'"+s+"' = BandName");
				retvals[i] = rows.Length;
			}

			return retvals;
		}

		public static bool GetBandStack(string band, int index, out string mode, out string filter, out double freq)
		{
			DataRow[] rows = dsBandStack.Tables["BandStack"].Select("'"+band+"' = BandName");

			if(rows.Length == 0)
			{
				MessageBox.Show("No Entries found for Band: "+band, "No Entry Found",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
				mode = "";
				filter = "";
				freq = 0.0f;
				return false;
			}

			index = index % rows.Length;
			
			mode = (string)((DataRow)rows[index])["Mode"];
			filter = (string)((DataRow)rows[index])["Filter"];
			freq = (double)((DataRow)rows[index])["Frequency"];
			return true;
		}

		public static void AddBandStack(string band, string mode, string filter, double freq)
		{
			DataRow dr = dsBandStack.Tables["BandStack"].NewRow();
			dr["BandName"] = band;
			dr["Mode"] = mode;
			dr["Filter"] = filter;
			dr["Frequency"] = freq;
			dsBandStack.Tables["BandStack"].Rows.Add(dr);

			if(conn.State == ConnectionState.Open)
				bandStackAdapter.Update(dsBandStack, "BandStack");
		}

		public static void SaveBandStack(string band, int index, string mode, string filter, double freq)
		{
			DataRow[] rows = dsBandStack.Tables["BandStack"].Select("'"+band+"' = BandName");

			foreach(DataRow datarow in rows)			// prevent duplicates
			{
				if((string)datarow["BandName"] == band &&
					(double)datarow["Frequency"] == freq)
//					&& (string)datarow["Filter"] == filter &&
//					(string)datarow["Mode"] == mode)
				{
					datarow["Filter"] = filter;
					datarow["Mode"] = mode;
					goto end;
				}
			}

			index = index % rows.Length;

			DataRow d = (DataRow)rows[index];
			d["Mode"] = mode;
			d["Filter"] = filter;
			d["Frequency"] = freq;

			end:
			if(conn.State == ConnectionState.Open)
				bandStackAdapter.Update(dsBandStack, "BandStack");
		}

		public static void SaveVars(string tableName, ref ArrayList list)
		{
			OleDbConnection connection;
			Connect(out connection);
			OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
			myDataAdapter.SelectCommand = new OleDbCommand("SELECT * FROM "+tableName, connection);
			OleDbCommandBuilder cb = new OleDbCommandBuilder(myDataAdapter);
			DataSet ds = new DataSet();
			myDataAdapter.Fill(ds, tableName);
			foreach(string s in list)
			{
				string[] vals = s.Split('/');
				if(vals.Length > 2)
				{
					for(int i=2; i<vals.Length; i++)
						vals[1] += "/"+vals[i];
				}

				DataRow[] rows = ds.Tables[tableName].Select("ID = '"+vals[0]+"'");
				if(rows.Length == 0)	// name is not in list
				{
					DataRow newRow = ds.Tables[tableName].NewRow();
					newRow[0] = vals[0];
					newRow[1] = vals[1];
					ds.Tables[tableName].Rows.Add(newRow);
				}
				else if(rows.Length == 1)
				{
					rows[0][1] = vals[1];
				}
			}

			myDataAdapter.Update(ds, tableName);

			connection.Close();
		}

		public static ArrayList GetVars(string tableName)
		{
			ArrayList list = new ArrayList();
			OleDbConnection connection;
			Connect(out connection);
			OleDbCommand cmd = new OleDbCommand("SELECT * FROM "+tableName, connection);
			OleDbDataReader myReader = cmd.ExecuteReader();

			while(myReader.Read())
			{
				list.Add(myReader.GetString(0)+"/"+myReader.GetString(1));
			}

			myReader.Close();
			connection.Close();
			return list;
		}

		public static void SaveOptions(ref ArrayList list)
		{
			foreach(string s in list)
			{
				string[] vals = s.Split('/');
				if(vals.Length > 2)
				{
					for(int i=2; i<vals.Length; i++)
						vals[1] += "/"+vals[i];
				}
                    					
				DataRow[] rows = dsOptions.Tables["Options"].Select("ID = '"+vals[0]+"'");
				if(rows.Length == 0)	// name is not in list
				{
					DataRow newRow = dsOptions.Tables["Options"].NewRow();
					newRow[0] = vals[0];
					newRow[1] = vals[1];
					dsOptions.Tables["Options"].Rows.Add(newRow);
				}
				else if(rows.Length == 1)
				{
					rows[0][1] = vals[1];
				}
			}
		}

		public static ArrayList GetOptions()
		{
			ArrayList list = new ArrayList();
			
			foreach(DataRow dr in dsOptions.Tables["Options"].Rows)
				list.Add(dr[0].ToString()+"/"+dr[1].ToString());

			return list;
		}

		public static bool ImportDatabase(string filename)
		{
			// check if file exists
			if(!File.Exists(filename))
			{
				MessageBox.Show("File does not exist: "+filename);
				return false;
			}

			Exit(); // force all tables to save
			Init(); // reopen database connection

			// verify tables exist
			ADODB.Connection adodb_conn = new ADODB.ConnectionClass();
			adodb_conn.Open("Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data source="+filename, "", "", 0);

			Catalog cat = new CatalogClass();
            cat.ActiveConnection = adodb_conn;

			ArrayList a = new ArrayList();

			foreach(Table t in cat.Tables)
				a.Add(t.Name);

			// all tables found -- close the catalog
			adodb_conn.Close();
			
			// open old database
			OleDbConnection c = new OleDbConnection();
			c.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Persist Security Info=False;" +
				"Data source="+filename;
			c.Open();

			// copy table data
			
			
			try
			{
				// State table
				if(a.BinarySearch("State") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM State", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "State");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM State", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "State");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["State"].Rows)
					{
				
						DataRow[] found = old_ds.Tables["State"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["State"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["State"].Rows.Add(new_dr);
						}
					}
			
					old_adapter.Update(old_ds, "State");
				}

				// Options Table
				if(a.BinarySearch("Options") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Options", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "Options");

					foreach(DataRow dr in ds.Tables["Options"].Rows)
					{
						DataRow[] found = dsOptions.Tables["Options"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = dsOptions.Tables["Options"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							dsOptions.Tables["Options"].Rows.Add(new_dr);
						}
					}

					optionsAdapter.Update(dsOptions, "Options");
				}

				// CW Table
				if(a.BinarySearch("CW") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CW", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "CW");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM CW", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "CW");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["CW"].Rows)
					{
						DataRow[] found = old_ds.Tables["CW"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["CW"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["CW"].Rows.Add(new_dr);
						}
					}

					old_adapter.Update(old_ds, "CW");
				}
				// CWX Table
				if(a.BinarySearch("CWX") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM CWX", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "CWX");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM CWX", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "CWX");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["CWX"].Rows)
					{
						DataRow[] found = old_ds.Tables["CWX"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["CWX"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["CWX"].Rows.Add(new_dr);
						}
					}

					old_adapter.Update(old_ds, "CWX");
				}
				// GroupList Table
				if(a.BinarySearch("GroupList") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM GroupList", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "GroupList");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM GroupList", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "GroupList");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["GroupList"].Rows)
					{
						DataRow[] found = old_ds.Tables["GroupList"].Select("GroupID = '"+dr["GroupID"]+"'");

						if(found.Length > 0)
							found[0]["GroupName"] = dr["GroupName"];
						else
						{
							DataRow new_dr = old_ds.Tables["GroupList"].NewRow();
							new_dr[0] = dr[0];
							new_dr[1] = dr[1];
							old_ds.Tables["GroupList"].Rows.Add(new_dr);
						}
					}	
			
					old_adapter.Update(old_ds, "GroupList");
				}

				// BandStack Table
				if(a.BinarySearch("BandStack") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM BandStack", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "BandStack");

					foreach(DataRow dr in dsBandStack.Tables["BandStack"].Rows)
						dr.Delete();

					foreach(DataRow dr in ds.Tables["BandStack"].Rows)
					{
						DataRow new_dr = dsBandStack.Tables["BandStack"].NewRow();
						for(int i=0; i<ds.Tables["BandStack"].Columns.Count; i++)
							new_dr[i] = dr[i];
						dsBandStack.Tables["BandStack"].Rows.Add(new_dr);
					}

					bandStackAdapter.Update(dsBandStack, "BandStack");
				}

/*
				// BandText Table
				if(a.BinarySearch("BandText") >= 0)
				{				
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM BandText", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "BandText");

					foreach(DataRow dr in ds.Tables["BandText"].Rows)
					{
						DataRow[] found = dsBandText.Tables["BandText"].Select("Low = '"+dr["Low"]+"'");

						if(found.Length > 0)
						{
							found[0]["High"] = dr["High"];
							found[0]["Name"] = dr["Name"];
							found[0]["Transmit"] = dr["Transmit"];
						}
						else
						{
							DataRow new_dr = dsBandText.Tables["BandText"].NewRow();
							for(int i=0; i<ds.Tables["BandText"].Columns.Count; i++)
								new_dr[i] = dr[i];
							dsBandText.Tables["BandText"].Rows.Add(new_dr);
						}
					}

					bandTextAdapter.Update(dsBandText, "BandText");
				}
*/
				// Memory Table
				if(a.BinarySearch("Memory") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Memory", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "Memory");

					foreach(DataRow dr in dsMemory.Tables["Memory"].Rows)
						dr.Delete();

					foreach(DataRow dr in ds.Tables["Memory"].Rows)
					{
						DataRow new_dr = dsMemory.Tables["Memory"].NewRow();
						for(int i=0; i<ds.Tables["Memory"].Columns.Count; i++)
							new_dr[i] = dr[i];
						dsMemory.Tables["Memory"].Rows.Add(new_dr);
					}

					memoryAdapter.Update(dsMemory, "Memory");
				}

				// UCB Table
				if(a.BinarySearch("UCB") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM UCB", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "UCB");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM UCB", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "UCB");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["UCB"].Rows)
					{
						DataRow[] found = old_ds.Tables["UCB"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["UCB"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["UCB"].Rows.Add(new_dr);
						}
					}

					old_adapter.Update(old_ds, "UCB");
				}

				// TxProfile Table
				if(a.BinarySearch("TxProfile") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM TxProfile", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "TxProfile");

					foreach(DataRow dr in dsTxProfile.Tables["TxProfile"].Rows)
						dr.Delete();

					foreach(DataRow dr in ds.Tables["TxProfile"].Rows)
					{
						if((string)dr["Name"] == "ESSB+") dr["Name"] = "ESSB Plus";
						DataRow new_dr = dsTxProfile.Tables["TxProfile"].NewRow();
						for(int i=0; i<ds.Tables["TxProfile"].Columns.Count; i++)
							new_dr[i] = dr[i];
						dsTxProfile.Tables["TxProfile"].Rows.Add(new_dr);
					}

					txProfileAdapter.Update(dsTxProfile, "TxProfile");
				}

				// XVTR Table
				if(a.BinarySearch("XVTR") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM XVTR", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "XVTR");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM XVTR", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "XVTR");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["XVTR"].Rows)
					{
						DataRow[] found = old_ds.Tables["XVTR"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["XVTR"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["XVTR"].Rows.Add(new_dr);
						}
					}

					old_adapter.Update(old_ds, "XVTR");
				}

				// Production Table
				if(a.BinarySearch("ProdTest") >= 0)
				{
					OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM ProdTest", c);
					DataSet ds = new DataSet();
					adapter.Fill(ds, "ProdTest");

					OleDbDataAdapter old_adapter = new OleDbDataAdapter("SELECT * FROM ProdTest", conn);
					DataSet old_ds = new DataSet();
					old_adapter.Fill(old_ds, "ProdTest");

					OleDbCommandBuilder cmdBuilder = new OleDbCommandBuilder(old_adapter);

					foreach(DataRow dr in ds.Tables["ProdTest"].Rows)
					{
						DataRow[] found = old_ds.Tables["ProdTest"].Select("ID = '"+dr["ID"]+"'");
				
						if(found.Length > 0)
							found[0]["SaveValue"] = dr["SaveValue"];
						else
						{
							DataRow new_dr = old_ds.Tables["ProdTest"].NewRow();
							new_dr["ID"] = dr["ID"];
							new_dr["SaveValue"] = dr["SaveValue"];
							old_ds.Tables["ProdTest"].Rows.Add(new_dr);
						}
					}

					old_adapter.Update(old_ds, "ProdTest");
				}

				CheckBandTextValid();

				c.Close();
				return true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message+"\n\n\n"+ex.StackTrace);
				return false;
			}			
		}

		#endregion
	}
}
