//=================================================================
// database.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2009  FlexRadio Systems
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

namespace PowerSDR
{
    using System;
    using System.Data;
    using System.Windows.Forms;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;

	class DB
	{
		#region Variable Declaration

		public static DataSet ds;

        /// <summary>
        /// the complete filename of the datbase file to use including the full path
        /// </summary>
        private static string file_name = "";
        public static string FileName
        {
            set { file_name = value; }
        }

		#endregion

		#region Private Member Functions
// ======================================================
// Private Member Functions
// ======================================================

		private static void VerifyTables()
		{
			if(!ds.Tables.Contains("BandText"))
				AddBandTextTable();

			if(!ds.Tables.Contains("BandStack"))
				AddBandStackTable();

			if(!ds.Tables.Contains("Memory"))
				AddMemoryTable();

			if(!ds.Tables.Contains("GroupList"))
				AddGroupListTable();

			if(!ds.Tables.Contains("TXProfile"))
				AddTXProfileTable();

			if(!ds.Tables.Contains("TXProfileDef"))
				AddTXProfileDefTable();

			Update();
		}

		private static void AddFormTable(string name)
		{
			ds.Tables.Add(name);
			ds.Tables[name].Columns.Add("Key", typeof(string));
			ds.Tables[name].Columns.Add("Value", typeof(string));
		}

		private static void AddBandTextTable()
		{
			ds.Tables.Add("BandText");
			DataTable t = ds.Tables["BandText"];

			t.Columns.Add("Low", typeof(double));
			t.Columns.Add("High", typeof(double));
			t.Columns.Add("Name", typeof(string));
			t.Columns.Add("TX", typeof(bool));

			object[] data = {
								1.800000, 1.809999, "160M CW/Digital Modes",	true,
								1.810000, 1.810000, "160M CW QRP",				true,
								1.810001, 1.842999, "160M CW",					true,
								1.843000, 1.909999, "160M SSB/SSTV/Wide Band",	true,
								1.910000, 1.910000, "160M SSB QRP",				true,
								1.910001, 1.994999, "160M SSB/SSTV/Wide Band",	true,
								1.995000, 1.999999, "160M Experimental",		true,

								3.500000, 3.524999, "80M Extra CW",				true,
								3.525000, 3.579999, "80M CW",					true,
								3.580000, 3.589999, "80M RTTY",					true,
								3.590000, 3.590000, "80M RTTY DX",				true,
								3.590001, 3.599999, "80M RTTY",					true,
								3.600000, 3.699999, "75M Extra SSB",			true,
								3.700000, 3.789999, "75M Ext/Adv SSB",			true,
								3.790000, 3.799999, "75M Ext/Adv DX Window",	true,
								3.800000, 3.844999, "75M SSB",					true,
								3.845000, 3.845000, "75M SSTV",					true,
								3.845001, 3.884999, "75M SSB",					true,
								3.885000, 3.885000, "75M AM Calling Frequency", true,
								3.885001, 3.999999, "75M SSB",	                true,

                                5.258500, 5.258500, "60M Channel FA",           true,
								5.278500, 5.278500, "60M Channel FB",           true,
                                5.288500, 5.288500, "60M Channel FC",           true,
 								5.330500, 5.330500, "60M Channel 1",			true,
								5.346500, 5.346500, "60M Channel 2",			true,
                                5.366500, 5.366500, "60M Channel 3/FK",         true,
                                5.371500, 5.371500, "60M Channel 4/FL",         true,
                                5.398500, 5.398500, "60M Channel FE",           true,
                                5.403500, 5.403500, "60M Channel 5/FM",         true,

								7.000000, 7.024999, "40M Extra CW",				true,
								7.025000, 7.039999, "40M CW",					true,
								7.040000, 7.040000, "40M RTTY DX",				true,
								7.040001, 7.099999, "40M RTTY",					true,
								7.100000, 7.124999, "40M CW",					true,
								7.125000, 7.170999, "40M Ext/Adv SSB",			true,
								7.171000, 7.171000, "40M SSTV",					true,
								7.171001, 7.174999, "40M Ext/Adv SSB",			true,
								7.175000, 7.289999, "40M SSB",					true,
								7.290000, 7.290000, "40M AM Calling Frequency", true,
								7.290001, 7.299999, "40M SSB",					true,
								
								10.100000, 10.129999, "30M CW",					true,
								10.130000, 10.139999, "30M RTTY",				true,
								10.140000, 10.149999, "30M Packet",				true,

								14.000000, 14.024999, "20M Extra CW",			true,
								14.025000, 14.069999, "20M CW",					true,
								14.070000, 14.094999, "20M RTTY",				true,
								14.095000, 14.099499, "20M Packet",				true,
								14.099500, 14.099999, "20M CW",					true,
								14.100000, 14.100000, "20M NCDXF Beacons",		true,
								14.100001, 14.100499, "20M CW",					true,
								14.100500, 14.111999, "20M Packet",				true,
								14.112000, 14.149999, "20M CW",					true,
								14.150000, 14.174999, "20M Extra SSB",			true,
								14.175000, 14.224999, "20M Ext/Adv SSB",		true,
								14.225000, 14.229999, "20M SSB",				true,
								14.230000, 14.230000, "20M SSTV",				true,
								14.230001, 14.285999, "20M SSB",				true,
								14.286000, 14.286000, "20M AM Calling Frequency", true,
								14.286001, 14.349999, "20M SSB",				true,
								
								18.068000, 18.099999, "17M CW",					true,
								18.100000, 18.104999, "17M RTTY",				true,
								18.105000, 18.109999, "17M Packet",				true,
								18.110000, 18.110000, "17M NCDXF Beacons",		true,
								18.110001, 18.167999, "17M SSB",				true,
								
								21.000000, 21.024999, "15M Extra CW",			true,
								21.025000, 21.069999, "15M CW",					true,
								21.070000, 21.099999, "15M RTTY",				true,
								21.100000, 21.109999, "15M Packet",				true,
								21.110000, 21.149999, "15M CW",					true,
								21.150000, 21.150000, "15M NCDXF Beacons",		true,
								21.150001, 21.199999, "15M CW",					true,
								21.200000, 21.224999, "15M Extra SSB",			true,
								21.225000, 21.274999, "15M Ext/Adv SSB",		true,
								21.275000, 21.339999, "15M SSB",				true,
								21.340000, 21.340000, "15M SSTV",				true,
								21.340001, 21.449999, "15M SSB",				true,
								
								24.890000, 24.919999, "12M CW",					true,
								24.920000, 24.924999, "12M RTTY",				true,
								24.925000, 24.929999, "12M Packet",				true,
								24.930000, 24.930000, "12M NCDXF Beacons",		true,
								24.930001, 24.989999, "12M SSB",				true,
								
								28.000000, 28.069999, "10M CW",					true,
								28.070000, 28.149999, "10M RTTY",				true,
								28.150000, 28.199999, "10M CW",					true,
								28.200000, 28.200000, "10M NCDXF Beacons",		true,
								28.200001, 28.299999, "10M Beacons",			true,
								28.300000, 28.679999, "10M SSB",				true,
								28.680000, 28.680000, "10M SSTV",				true,
								28.680001, 28.999999, "10M SSB",				true,
								29.000000, 29.199999, "10M AM",					true,
								29.200000, 29.299999, "10M SSB",				true,
								29.300000, 29.509999, "10M Satellite Downlinks", true,
								29.510000, 29.519999, "10M Deadband",			true,
								29.520000, 29.589999, "10M Repeater Inputs",	true,
								29.590000, 29.599999, "10M Deadband",			true,
								29.600000, 29.600000, "10M FM Simplex",			true,
								29.600001, 29.609999, "10M Deadband",			true,
								29.610000, 29.699999, "10M Repeater Outputs",	true,
								
								50.000000, 50.059999, "6M CW",					true,
								50.060000, 50.079999, "6M Beacon Sub-Band",		true,
								50.080000, 50.099999, "6M CW",					true,
								50.100000, 50.124999, "6M DX Window",			true,
								50.125000, 50.125000, "6M Calling Frequency",	true,
								50.125001, 50.299999, "6M SSB",					true,
								50.300000, 50.599999, "6M All Modes",			true,
								50.600000, 50.619999, "6M Non Voice",			true,
								50.620000, 50.620000, "6M Digital Packet Calling", true,
								50.620001, 50.799999, "6M Non Voice",			true,
								50.800000, 50.999999, "6M RC",					true,
								51.000000, 51.099999, "6M Pacific DX Window",	true,
								51.100000, 51.119999, "6M Deadband",			true,
								51.120000, 51.179999, "6M Digital Repeater Inputs", true,
								51.180000, 51.479999, "6M Repeater Inputs",		true,
								51.480000, 51.619999, "6M Deadband",			true,
								51.620000, 51.679999, "6M Digital Repeater Outputs", true,
								51.680000, 51.979999, "6M Repeater Outputs",	true,
								51.980000, 51.999999, "6M Deadband",			true,
								52.000000, 52.019999, "6M Repeater Inputs",		true,
								52.020000, 52.020000, "6M FM Simplex",			true,
								52.020001, 52.039999, "6M Repeater Inputs",		true,
								52.040000, 52.040000, "6M FM Simplex",			true,
								52.040001, 52.479999, "6M Repeater Inputs",		true,
								52.480000, 52.499999, "6M Deadband",			true,
								52.500000, 52.524999, "6M Repeater Outputs",	true,
								52.525000, 52.525000, "6M Primary FM Simplex",	true,
								52.525001, 52.539999, "6M Deadband",			true,
								52.540000, 52.540000, "6M Secondary FM Simplex", true,
								52.540001, 52.979999, "6M Repeater Outputs",	true,
								52.980000, 52.999999, "6M Deadbands",			true,
								53.000000, 53.000000, "6M Remote Base FM Spx",	true,
								53.000001, 53.019999, "6M Repeater Inputs",		true,
								53.020000, 53.020000, "6M FM Simplex",			true,
								53.020001, 53.479999, "6M Repeater Inputs",		true,
								53.480000, 53.499999, "6M Deadband",			true,
								53.500000, 53.519999, "6M Repeater Outputs",	true,
								53.520000, 53.520000, "6M FM Simplex",			true,
								53.520001, 53.899999, "6M Repeater Outputs",	true,
								53.900000, 53.900000, "6M FM Simplex",			true,
								53.900010, 53.979999, "6M Repeater Outputs",	true,
								53.980000, 53.999999, "6M Deadband",			true,
								
								144.000000, 144.099999, "2M CW",				true,
								144.100000, 144.199999, "2M CW/SSB",			true,
								144.200000, 144.200000, "2M Calling",			true,
								144.200001, 144.274999, "2M CW/SSB",			true,
								144.275000, 144.299999, "2M Beacon Sub-Band",	true,
								144.300000, 144.499999, "2M Satellite",			true,
								144.500000, 144.599999, "2M Linear Translator Inputs", true,
								144.600000, 144.899999, "2M FM Repeater",		true,
								144.900000, 145.199999, "2M FM Simplex",		true,
								145.200000, 145.499999, "2M FM Repeater",		true,
								145.500000, 145.799999, "2M FM Simplex",		true,
								145.800000, 145.999999, "2M Satellite",			true,
								146.000000, 146.399999, "2M FM Repeater",		true,
								146.400000, 146.609999, "2M FM Simplex",		true,
								146.610000, 147.389999, "2M FM Repeater",		true,
								147.390000, 147.599999, "2M FM Simplex",		true,
								147.600000, 147.999999, "2M FM Repeater",		true,

								222.000000, 222.024999, "1.25M EME/Weak Signal", true,
								222.025000, 222.049999, "1.25M Weak Signal",	true,
								222.050000, 222.059999, "1.25M Propagation Beacons", true,
								222.060000, 222.099999, "1.25M Weak Signal",	true,
								222.100000, 222.100000, "1.25M SSB/CW Calling",	true,
								222.100001, 222.149999, "1.25M Weak Signal CW/SSB", true,
								222.150000, 222.249999, "1.25M Local Option",	true,
								222.250000, 223.380000, "1.25M FM Repeater Inputs", true,
								222.380001, 223.399999, "1.25M General", true,
								223.400000, 223.519999, "1.25M FM Simplex",		true,
								223.520000, 223.639999, "1.25M Digital/Packet",	true,
								223.640000, 223.700000, "1.25M Links/Control",	true,
								223.700001, 223.709999, "1.25M General",	true,
								223.710000, 223.849999, "1.25M Local Option",	true,
								223.850000, 224.980000, "1.25M Repeater Outputs", true,

								420.000000, 425.999999, "70CM ATV Repeater",	true,
								426.000000, 431.999999, "70CM ATV Simplex",		true,
								432.000000, 432.069999, "70CM EME",				true,
								432.070000, 432.099999, "70CM Weak Signal CW",	true,
								432.100000, 432.100000, "70CM Calling Frequency", true,
								432.100001, 432.299999, "70CM Mixed Mode Weak Signal", true,
								432.300000, 432.399999, "70CM Propagation Beacons", true,
								432.400000, 432.999999, "70CM Mixed Mode Weak Signal", true,
								433.000000, 434.999999, "70CM Auxillary/Repeater Links", true,
								435.000000, 437.999999, "70CM Satellite Only",	true,
								438.000000, 441.999999, "70CM ATV Repeater",	true,
								442.000000, 444.999999, "70CM Local Repeaters",	true,
								445.000000, 445.999999, "70CM Local Option",	true,
								446.000000, 446.000000, "70CM Simplex",			true,
								446.000001, 446.999999, "70CM Local Option",	true,
								447.000000, 450.000000, "70CM Local Repeaters", true,

								902.000000, 902.099999, "33CM Weak Signal SSTV/FAX/ACSSB", true,
								902.100000, 902.100000, "33CM Weak Signal Calling", true,
								902.100001, 902.799999, "33CM Weak Signal SSTV/FAX/ACSSB", true,
								902.800000, 902.999999, "33CM Weak Signal EME/CW", true,
								903.000000, 903.099999, "33CM Digital Modes",	true,
								903.100000, 903.100000, "33CM Alternate Calling", true,
								903.100001, 905.999999, "33CM Digital Modes",	true,
								906.000000, 908.999999, "33CM FM Repeater Inputs", true,
								909.000000, 914.999999, "33CM ATV",				true,
								915.000000, 917.999999, "33CM Digital Modes",	true,
								918.000000, 920.999999, "33CM FM Repeater Outputs", true,
								921.000000, 926.999999, "33CM ATV",				true,
								927.000000, 928.000000, "33CM FM Simplex/Links", true,
								
								1240.000000, 1245.999999, "23CM ATV #1",		true,
								1246.000000, 1251.999999, "23CM FMN Point/Links", true,
								1252.000000, 1257.999999, "23CM ATV #2, Digital Modes", true,
								1258.000000, 1259.999999, "23CM FMN Point/Links", true,
								1260.000000, 1269.999999, "23CM Sat Uplinks/Wideband Exp.", true,
								1270.000000, 1275.999999, "23CM Repeater Inputs", true,
								1276.000000, 1281.999999, "23CM ATV #3",		true,
								1282.000000, 1287.999999, "23CM Repeater Outputs",	true,
								1288.000000, 1293.999999, "23CM Simplex ATV/Wideband Exp.", true,
								1294.000000, 1294.499999, "23CM Simplex FMN",		true,
								1294.500000, 1294.500000, "23CM FM Simplex Calling", true,
								1294.500001, 1294.999999, "23CM Simplex FMN",		true,
								1295.000000, 1295.799999, "23CM SSTV/FAX/ACSSB/Exp.", true,
								1295.800000, 1295.999999, "23CM EME/CW Expansion",	true,
								1296.000000, 1296.049999, "23CM EME Exclusive",		true,
								1296.050000, 1296.069999, "23CM Weak Signal",		true,
								1296.070000, 1296.079999, "23CM CW Beacons",		true,
								1296.080000, 1296.099999, "23CM Weak Signal",		true,
								1296.100000, 1296.100000, "23CM CW/SSB Calling",	true,
								1296.100001, 1296.399999, "23CM Weak Signal",		true,
								1296.400000, 1296.599999, "23CM X-Band Translator Input", true,
								1296.600000, 1296.799999, "23CM X-Band Translator Output", true,
								1296.800000, 1296.999999, "23CM Experimental Beacons", true,
								1297.000000, 1300.000000, "23CM Digital Modes",		true,

								2300.000000, 2302.999999, "2.3GHz High Data Rate", true,
								2303.000000, 2303.499999, "2.3GHz Packet",		true,
								2303.500000, 2303.800000, "2.3GHz TTY Packet",	true,
								2303.800001, 2303.899999, "2.3GHz General",	true,
								2303.900000, 2303.900000, "2.3GHz Packet/TTY/CW/EME", true,
								2303.900001, 2304.099999, "2.3GHz CW/EME",		true,
								2304.100000, 2304.100000, "2.3GHz Calling Frequency", true,
								2304.100001, 2304.199999, "2.3GHz CW/EME/SSB",	true,
								2304.200000, 2304.299999, "2.3GHz SSB/SSTV/FAX/Packet AM/Amtor", true,
								2304.300000, 2304.319999, "2.3GHz Propagation Beacon Network", true,
								2304.320000, 2304.399999, "2.3GHz General Propagation Beacons", true,
								2304.400000, 2304.499999, "2.3GHz SSB/SSTV/ACSSB/FAX/Packet AM", true,
								2304.500000, 2304.699999, "2.3GHz X-Band Translator Input", true,
								2304.700000, 2304.899999, "2.3GHz X-Band Translator Output", true,
								2304.900000, 2304.999999, "2.3GHz Experimental Beacons", true,
								2305.000000, 2305.199999, "2.3GHz FM Simplex", true,
								2305.200000, 2305.200000, "2.3GHz FM Simplex Calling", true,
								2305.200001, 2305.999999, "2.3GHz FM Simplex", true,
								2306.000000, 2308.999999, "2.3GHz FM Repeaters", true,
								2309.000000, 2310.000000, "2.3GHz Control/Aux Links", true,
								2390.000000, 2395.999999, "2.3GHz Fast-Scan TV", true,
								2396.000000, 2398.999999, "2.3GHz High Rate Data", true,
								2399.000000, 2399.499999, "2.3GHz Packet", true,
								2399.500000, 2399.999999, "2.3GHz Control/Aux Links", true,
								2400.000000, 2402.999999, "2.4GHz Satellite", true,
								2403.000000, 2407.999999, "2.4GHz Satellite High-Rate Data", true,
								2408.000000, 2409.999999, "2.4GHz Satellite", true,
								2410.000000, 2412.999999, "2.4GHz FM Repeaters", true,
								2413.000000, 2417.999999, "2.4GHz High-Rate Data", true,
								2418.000000, 2429.999999, "2.4GHz Fast-Scan TV", true,
								2430.000000, 2432.999999, "2.4GHz Satellite", true,
								2433.000000, 2437.999999, "2.4GHz Sat. High-Rate Data", true,
								2438.000000, 2450.000000, "2.4GHz Wideband FM/FSTV/FMTV", true,

								3456.000000, 3456.099999, "3.4GHz General", true,
								3456.100000, 3456.100000, "3.4GHz Calling Frequency", true,
								3456.100001, 3456.299999, "3.4GHz General", true,
								3456.300000, 3456.400000, "3.4GHz Propagation Beacons", true,

								5760.000000, 5760.099999, "5.7GHz General", true,
								5760.100000, 5760.100000, "5.7GHz Calling Frequency", true,
								5760.100001, 5760.299999, "5.7GHz General", true,
								5760.300000, 5760.400000, "5.7GHz Propagation Beacons", true,

								10368.000000, 10368.099999, "10GHz General", true,
								10368.100000, 10368.100000, "10GHz Calling Frequency", true,
								10368.100001, 10368.400000, "10GHz General", true,

								24192.000000, 24192.099999, "24GHz General", true,
								24192.100000, 24192.100000, "24GHz Calling Frequency", true,
								24192.100001, 24192.400000, "24GHz General", true,

								47088.000000, 47088.099999, "47GHz General", true,
								47088.100000, 47088.100000, "47GHz Calling Frequency", true,
								47088.100001, 47088.400000, "47GHz General", true,

								2.500000, 2.500000, "WWV",						false,
								5.000000, 5.000000, "WWV",						false,
								10.000000, 10.000000, "WWV",					false,
								15.000000, 15.000000, "WWV",					false,
								20.000000, 20.000000, "WWV",					false,

								0.153000, 0.279000, "AM - Long Wave",			false,
								0.530000, 1.710000, "Broadcast AM Med Wave",	false,				
								2.300000, 2.495000, "120M Short Wave",			false,
								3.200000, 3.400000, "90M Short Wave",			false,
								4.750000, 4.995000, "60M Short Wave",			false,
								5.005000, 5.060000, "60M Short Wave",			false,
								5.730000, 6.295000, "49M Short Wave",			false,
								7.300000, 7.350000, "41M Short Wave",			false,
								9.250000, 9.990000, "31M Short Wave",			false,
								11.500000, 12.600000, "25M Short Wave",			false,
								13.570000, 13.870000, "22M Short Wave",			false,
								15.030000, 15.800000, "19M Short Wave",			false,
								17.480000, 17.900000, "16M Short Wave",			false,
								18.900000, 19.020000, "15M Short Wave",			false,
								21.450000, 21.850000, "13M Short Wave",			false,
								25.670000, 26.100000, "11M Short Wave",			false,
			};

			for(int i=0; i<data.Length/4; i++)
			{
				DataRow dr = t.NewRow();
				dr["Low"] = (double)data[i*4+0];
				dr["High"] = (double)data[i*4+1];
				dr["Name"] = (string)data[i*4+2];
				dr["TX"] = (bool)data[i*4+3];
				t.Rows.Add(dr);
			}
		}

		private static void AddBandStackTable()
		{
			ds.Tables.Add("BandStack");
			DataTable t = ds.Tables["BandStack"];

			t.Columns.Add("BandName", typeof(string));
			t.Columns.Add("Mode", typeof(string));
			t.Columns.Add("Filter", typeof(string));
			t.Columns.Add("Freq", typeof(double));

			object[] data = {
								"160M", "CWL", "F5", 1.810000,
								"160M", "CWU", "F1", 1.835000,
								"160M", "USB", "F6", 1.845000,
								"80M", "CWL", "F1", 3.501000,
								"80M", "LSB", "F6", 3.751000,
								"80M", "LSB", "F6", 3.850000,

                                "60M", "USB", "F6", 5.258500,
								"60M", "USB", "F6", 5.278500,
								"60M", "USB", "F6", 5.288500,
                                "60M", "USB", "F6", 5.398500,
								"60M", "USB", "F6", 5.366500,
								"60M", "USB", "F6", 5.371500,                               
								"60M", "USB", "F6", 5.403500,

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
								"17M", "CWU", "F1", 18.090000,
								"17M", "USB", "F6", 18.125000,
								"17M", "USB", "F6", 18.140000,
								"15M", "CWU", "F1", 21.001000,
								"15M", "USB", "F6", 21.255000,
								"15M", "USB", "F6", 21.300000,
								"12M", "CWU", "F1", 24.895000,
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
				dr["BandName"] = (string)data[i*4+0];
				dr["Mode"] = (string)data[i*4+1];
				dr["Filter"] = (string)data[i*4+2];
				dr["Freq"] = ((double)data[i*4+3]).ToString("f6");
				ds.Tables["BandStack"].Rows.Add(dr);
			}
		}


		private static void AddMemoryTable()
		{
			ds.Tables.Add("Memory");
			DataTable t = ds.Tables["Memory"];

			t.Columns.Add("GroupID", typeof(int));
			t.Columns.Add("Freq", typeof(double));
			t.Columns.Add("ModeID", typeof(int));
			t.Columns.Add("FilterID", typeof(int));
			t.Columns.Add("Callsign", typeof(string));
			t.Columns.Add("Comments", typeof(string));
			t.Columns.Add("Scan", typeof(bool));
			t.Columns.Add("Squelch", typeof(int));
			t.Columns.Add("StepSizeID", typeof(int));
			t.Columns.Add("AGCID", typeof(int));
			t.Columns.Add("Gain", typeof(string));
			t.Columns.Add("FilterLow", typeof(int));
			t.Columns.Add("FilterHigh", typeof(int));
			t.Columns.Add("CreateDate", typeof(string));
		}

		private static void AddGroupListTable()
		{
			ds.Tables.Add("GroupList");
			DataTable t = ds.Tables["GroupList"];

			t.Columns.Add("GroupID", typeof(int));
			t.Columns.Add("GroupName", typeof(string));

			string[] vals = { "AM", "FM", "SSB", "SSTV", "CW", "PSK", "RTTY" };
			
			for(int i=0; i<vals.Length; i++)
			{
				DataRow dr = t.NewRow();
				dr[0] = i;
				dr[1] = vals[i];
				t.Rows.Add(dr);
			}
		}

		private static void AddTXProfileTable()
		{
			ds.Tables.Add("TXProfile");
			DataTable t = ds.Tables["TXProfile"];

			t.Columns.Add("Name", typeof(string));
			t.Columns.Add("FilterLow", typeof(int));
			t.Columns.Add("FilterHigh", typeof(int));
			t.Columns.Add("TXEQNumBands", typeof(int));
			t.Columns.Add("TXEQEnabled", typeof(bool));
			t.Columns.Add("TXEQPreamp", typeof(int));
			t.Columns.Add("TXEQ1", typeof(int));
			t.Columns.Add("TXEQ2", typeof(int));
			t.Columns.Add("TXEQ3", typeof(int));
			t.Columns.Add("TXEQ4", typeof(int));
			t.Columns.Add("TXEQ5", typeof(int));
			t.Columns.Add("TXEQ6", typeof(int));
			t.Columns.Add("TXEQ7", typeof(int));
			t.Columns.Add("TXEQ8", typeof(int));
			t.Columns.Add("TXEQ9", typeof(int));
			t.Columns.Add("TXEQ10", typeof(int));
			t.Columns.Add("DXOn", typeof(bool));
			t.Columns.Add("DXLevel", typeof(int));
			t.Columns.Add("CompanderOn", typeof(bool));
			t.Columns.Add("CompanderLevel", typeof(int));
			t.Columns.Add("MicGain", typeof(int));
			t.Columns.Add("Lev_On", typeof(bool));
			t.Columns.Add("Lev_Slope", typeof(int));
			t.Columns.Add("Lev_MaxGain", typeof(int));
			t.Columns.Add("Lev_Attack", typeof(int));
			t.Columns.Add("Lev_Decay", typeof(int));
			t.Columns.Add("Lev_Hang", typeof(int));
			t.Columns.Add("Lev_HangThreshold", typeof(int));
			t.Columns.Add("ALC_Slope", typeof(int));
			t.Columns.Add("ALC_MaxGain", typeof(int));
			t.Columns.Add("ALC_Attack", typeof(int));
			t.Columns.Add("ALC_Decay", typeof(int));
			t.Columns.Add("ALC_Hang", typeof(int));
			t.Columns.Add("ALC_HangThreshold", typeof(int));
			t.Columns.Add("Power", typeof(int));

			#region Default

			DataRow dr = t.NewRow();
			dr["Name"] = "Default";
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 2;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region Default DX

			dr = t.NewRow();
			dr["Name"] = "Default DX";
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = true;
			dr["DXLevel"] = 5;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 2;
			dr["MicGain"] = 5;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion
		}

		private static void AddTXProfileDefTable()
		{
			ds.Tables.Add("TXProfileDef");
			DataTable t = ds.Tables["TXProfileDef"];

			t.Columns.Add("Name", typeof(string));
			t.Columns.Add("FilterLow", typeof(int));
			t.Columns.Add("FilterHigh", typeof(int));
			t.Columns.Add("TXEQNumBands", typeof(int));
			t.Columns.Add("TXEQEnabled", typeof(bool));
			t.Columns.Add("TXEQPreamp", typeof(int));
			t.Columns.Add("TXEQ1", typeof(int));
			t.Columns.Add("TXEQ2", typeof(int));
			t.Columns.Add("TXEQ3", typeof(int));
			t.Columns.Add("TXEQ4", typeof(int));
			t.Columns.Add("TXEQ5", typeof(int));
			t.Columns.Add("TXEQ6", typeof(int));
			t.Columns.Add("TXEQ7", typeof(int));
			t.Columns.Add("TXEQ8", typeof(int));
			t.Columns.Add("TXEQ9", typeof(int));
			t.Columns.Add("TXEQ10", typeof(int));
			t.Columns.Add("DXOn", typeof(bool));
			t.Columns.Add("DXLevel", typeof(int));
			t.Columns.Add("CompanderOn", typeof(bool));
			t.Columns.Add("CompanderLevel", typeof(int));
			t.Columns.Add("MicGain", typeof(int));
			t.Columns.Add("Lev_On", typeof(bool));
			t.Columns.Add("Lev_Slope", typeof(int));
			t.Columns.Add("Lev_MaxGain", typeof(int));
			t.Columns.Add("Lev_Attack", typeof(int));
			t.Columns.Add("Lev_Decay", typeof(int));
			t.Columns.Add("Lev_Hang", typeof(int));
			t.Columns.Add("Lev_HangThreshold", typeof(int));
			t.Columns.Add("ALC_Slope", typeof(int));
			t.Columns.Add("ALC_MaxGain", typeof(int));
			t.Columns.Add("ALC_Attack", typeof(int));
			t.Columns.Add("ALC_Decay", typeof(int));
			t.Columns.Add("ALC_Hang", typeof(int));
			t.Columns.Add("ALC_HangThreshold", typeof(int));
			t.Columns.Add("Power", typeof(int));

			#region Default

			DataRow dr = t.NewRow();
			dr["Name"] = "Default";
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 2;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion
			
			#region Default DX

			dr = t.NewRow();
			dr["Name"] = "Default DX";
			dr["FilterLow"] = 200;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = true;
			dr["DXLevel"] = 5;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 2;
			dr["MicGain"] = 5;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region AM

			dr = t.NewRow();
			dr["Name"] = "AM";
			dr["FilterLow"] = 0;
			dr["FilterHigh"] = 4000;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region Conventional

			dr = t.NewRow();
			dr["Name"] = "Conventional";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region D-104

			dr = t.NewRow();
			dr["Name"] = "D-104";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = -6;
			dr["TXEQ1"] = 7;
			dr["TXEQ2"] = 3;
			dr["TXEQ3"] = 4;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 5;
			dr["MicGain"] = 25;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region D-104+CPDR

			dr = t.NewRow();
			dr["Name"] = "D-104+CPDR";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = false;
			dr["TXEQPreamp"] = -6;
			dr["TXEQ1"] = 7;
			dr["TXEQ2"] = 3;
			dr["TXEQ3"] = 4;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 5;
			dr["MicGain"] = 20;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region D-104+EQ

			dr = t.NewRow();
			dr["Name"] = "D-104+EQ";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3500;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = true;
			dr["TXEQPreamp"] = -6;
			dr["TXEQ1"] = 7;
			dr["TXEQ2"] = 3;
			dr["TXEQ3"] = 4;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 5;
			dr["MicGain"] = 20;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region DX / Constest

			dr = t.NewRow();
			dr["Name"] = "DX / Contest";
			dr["FilterLow"] = 250;
			dr["FilterHigh"] = 3250;
			dr["TXEQNumBands"] = 10;
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
			dr["DXOn"] = true;
			dr["DXLevel"] = 5;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region ESSB

			dr = t.NewRow();
			dr["Name"] = "ESSB";
			dr["FilterLow"] = 50;
			dr["FilterHigh"] = 3650;
			dr["TXEQNumBands"] = 10;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = false;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region HC4-5

			dr = t.NewRow();
			dr["Name"] = "HC4-5";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 5;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region HC4-5+CPDR

			dr = t.NewRow();
			dr["Name"] = "HC4-5+CPDR";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3100;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 5;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region PR40+W2IHY

			dr = t.NewRow();
			dr["Name"] = "PR40+W2IHY";
			dr["FilterLow"] = 50;
			dr["FilterHigh"] = 3650;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region PR40+W2IHY+CPDR

			dr = t.NewRow();
			dr["Name"] = "PR40+W2IHY+CPDR";
			dr["FilterLow"] = 50;
			dr["FilterHigh"] = 3650;
			dr["TXEQNumBands"] = 3;
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
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region PR781+EQ

			dr = t.NewRow();
			dr["Name"] = "PR781+EQ";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3200;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = true;
			dr["TXEQPreamp"] = -11;
			dr["TXEQ1"] = -6;
			dr["TXEQ2"] = 2;
			dr["TXEQ3"] = 8;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = false;
			dr["CompanderLevel"] = 3;
			dr["MicGain"] = 12;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion

			#region PR781+EQ+CPDR

			dr = t.NewRow();
			dr["Name"] = "PR781+EQ+CPDR";
			dr["FilterLow"] = 100;
			dr["FilterHigh"] = 3200;
			dr["TXEQNumBands"] = 3;
			dr["TXEQEnabled"] = true;
			dr["TXEQPreamp"] = -9;
			dr["TXEQ1"] = -8;
			dr["TXEQ2"] = 3;
			dr["TXEQ3"] = 7;
			dr["TXEQ4"] = 0;
			dr["TXEQ5"] = 0;
			dr["TXEQ6"] = 0;
			dr["TXEQ7"] = 0;
			dr["TXEQ8"] = 0;
			dr["TXEQ9"] = 0;
			dr["TXEQ10"] = 0;
			dr["DXOn"] = false;
			dr["DXLevel"] = 3;
			dr["CompanderOn"] = true;
			dr["CompanderLevel"] = 2;
			dr["MicGain"] = 10;
			dr["Lev_On"] = true;
			dr["Lev_Slope"] = 0;
			dr["Lev_MaxGain"] = 5;
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
			t.Rows.Add(dr);

			#endregion
		}

		private static void CheckBandTextValid()
		{
			ArrayList bad_rows = new ArrayList();

			if(ds == null) return;
			foreach(DataRow dr in ds.Tables["BandText"].Rows)
			{
				// check low freq
				string f = ((double)dr["Low"]).ToString("f6");
				f = f.Replace(",", ".");
				DataRow[] rows = ds.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}

				// check high freq
				f = ((double)dr["High"]).ToString("f6");
				f = f.Replace(",", ".");
				rows = ds.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length > 1)
				{
					// handle multiple entries
					if(!bad_rows.Contains(dr))
						bad_rows.Add(dr);
				}
			}

			foreach(DataRow dr in bad_rows)
				ds.Tables["BandText"].Rows.Remove(dr);
		}

		#endregion

		#region Public Member Functions
// ======================================================
// Public Member Functions 
// ======================================================

		public static void Init()
		{
			ds = new DataSet("Data");

             if (File.Exists(file_name))
                ds.ReadXml(file_name);
 
			VerifyTables();

			CheckBandTextValid();
		}

		public static void Update()
		{
		     ds.WriteXml(file_name, XmlWriteMode.WriteSchema);
		}

		public static void Exit()
		{
			Update();
			ds = null;
		}

		public static bool BandText(double freq, out string outStr)
		{
			try
			{
				outStr = "";
				string f = freq.ToString("f6");
				f = f.Replace(",", ".");
				DataRow[] rows = ds.Tables["BandText"].Select(f+">=Low AND "+f+"<=High");
				if(rows.Length == 0)		// band not found
				{
					outStr = "Out of Band";
					return false;
				}
				else if(rows.Length == 1)	// found band
				{
					outStr = ((string)rows[0]["Name"]);
					return (bool)rows[0]["TX"];
				}
				else //if(rows.Length > 1)	// this should never happen
				{
					MessageBox.Show("Error reading BandInfo table.", "Database Error",
						MessageBoxButtons.OK, MessageBoxIcon.Error);
					outStr = "Error";
					return false;
				}
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
				DataRow[] rows = ds.Tables["BandStack"].Select("'"+s+"' = BandName");
				retvals[i] = rows.Length;
			}

			return retvals;
		}

		public static bool GetBandStack(string band, int index, out string mode, out string filter, out double freq)
		{
			DataRow[] rows = ds.Tables["BandStack"].Select("'"+band+"' = BandName");

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
			freq = (double)((DataRow)rows[index])["Freq"];
			return true;
		}

		public static void AddBandStack(string band, string mode, string filter, double freq)
		{
			DataRow dr = ds.Tables["BandStack"].NewRow();
			dr["BandName"] = band;
			dr["Mode"] = mode;
			dr["Filter"] = filter;
			dr["Freq"] = freq;
			ds.Tables["BandStack"].Rows.Add(dr);
		}

		public static void SaveBandStack(string band, int index, string mode, string filter, double freq)
		{
			DataRow[] rows = ds.Tables["BandStack"].Select("'"+band+"' = BandName");

			if(rows.Length == 0) return;

			foreach(DataRow datarow in rows)			// prevent duplicates
			{
				if((string)datarow["BandName"] == band &&
					(double)datarow["Freq"] == freq)
				{
					datarow["Filter"] = filter;
					datarow["Mode"] = mode;
					return;
				}
			}

			index = index % rows.Length;

			DataRow d = (DataRow)rows[index];
			d["Mode"] = mode;
			d["Filter"] = filter;
			d["Freq"] = freq;
		}

        // This removes the notches from the state database so we can rewrite all of them without
        // having one that was previously deleted staying in the database
        public static void PurgeNotches()
        {
            // make sure there is a State table
            if (!ds.Tables.Contains("State")) return;

            // find all the notches and remove them
            var rows = ds.Tables["State"].Select("Key like 'notchlist*'");
            if (rows != null)
            {
                foreach (var row in rows)
                    row.Delete();
            }
        }

		public static void SaveVars(string tableName, ref ArrayList list)
		{
			if(!ds.Tables.Contains(tableName))
				AddFormTable(tableName);

			foreach(string s in list)
			{
				string[] vals = s.Split('/');
				if(vals.Length > 2)
				{
					for(int i=2; i<vals.Length; i++)
						vals[1] += "/"+vals[i];
				}

               if(vals.Length <= 1) // skip it as no data was provided
                    continue;

					DataRow[] rows = ds.Tables[tableName].Select("Key = '"+vals[0]+"'");
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
		}

		public static ArrayList GetVars(string tableName)
		{
			ArrayList list = new ArrayList();
			if(!ds.Tables.Contains(tableName))
				return list;

			DataTable t = ds.Tables[tableName];

			for(int i=0; i<t.Rows.Count; i++)
			{
				list.Add(t.Rows[i][0].ToString()+"/"+t.Rows[i][1].ToString());
			}

			return list;
		}

		public static bool ImportDatabase(string filename)
		{
			if(!File.Exists(filename)) return false;

			DataSet file = new DataSet();

			try
			{
				file.ReadXml(filename);
			}
			catch(Exception)
			{
				return false;
			}

			ds = file;
			
            // Handle change of mode from FMN to just FM
            DataRow[] rows = ds.Tables["BandStack"].Select("Mode = 'FMN'");
            foreach(DataRow dr in rows)
                dr["Mode"] = "FM";
				
			return true;
		}

		#endregion
	}
}
