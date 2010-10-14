//=================================================================
// usbhid.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// Copyright (C) 2004-2010  FlexRadio Systems
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PowerSDR
{
    public class USBHID
    {
        #region Opcode Definition

		public enum Opcode
		{
			USB_OP_CREATE_INTERFACE = 1000,	
			USB_OP_START_INTERFACE,
			USB_OP_SET_CLOCK_SOURCE,
			USB_OP_SET_ROUTE,
			USB_OP_GET_CURRENT_STATUS,
			USB_OP_IS_LOCKED,
			USB_OP_GET_CURRENT_CONFIG,
			USB_OP_INSTALL_CALLBACK,
			USB_OP_UNINSTALL_CALLBACK,
			USB_OP_GLOBAL_MUTE,
			USB_OP_TOGGLE_LED,
			USB_OP_GET_EVM_UI,
			USB_OP_SET_OUT_CHANNEL_VOL,		// AXM20
			USB_OP_SET_IN_CHANNEL_VOL,			// AXM20
			USB_OP_SET_OUT_CHANNEL_MUTE,		// AXM20
			USB_OP_SET_IN_CHANNEL_MUTE,		// AXM20
			USB_OP_GET_IN_CHANNEL_VOL,			// AXM20
			USB_OP_GET_OUT_CHANNEL_VOL,		// AXM20
			USB_OP_NOOP,
			USB_OP_I2C_WRITE_VALUE,
			USB_OP_I2C_WRITE_2_VALUE,
			USB_OP_I2C_READ_VALUE,
			USB_OP_REG_GET,
			USB_OP_REG_SET,
			USB_OP_SET_SAMPLE_RATE,
			USB_OP_GET_RX_NUM,
			USB_OP_GET_RX_PRESENT_MASK,
			USB_OP_GET_RX_ENABLE_MASK,
			USB_OP_SET_RX_ENABLE_MASK,
			USB_OP_GET_TX_NUM,
			USB_OP_GET_TX_PRESENT_MASK,
			USB_OP_GET_TX_ENABLE_MASK,
			USB_OP_SET_TX_ENABLE_MASK,
			USB_OP_SET_DDS_CHAN,
			USB_OP_SET_DDS_FREQ,
			USB_OP_SET_DDS_AMPLITUDE,
			USB_OP_SET_PREAMP_ON,
			USB_OP_SET_ATT_ON,
			USB_OP_SET_ATT_VAL,
			USB_OP_SET_LED,
			USB_OP_SET_FILTER,
			USB_OP_READ_CLOCK_GEN,
			USB_OP_WRITE_CLOCK_GEN,
			USB_OP_READ_DDS,
			USB_OP_WRITE_DDS,
			USB_OP_UPDATE_DDS,
			USB_OP_READ_GPIO,
			USB_OP_WRITE_GPIO,
			USB_OP_READ_GPIO_DDR,
			USB_OP_WRITE_GPIO_DDR,
			USB_OP_SET_TEST_RELAY,
			USB_OP_SET_SIG_GEN_RELAY,
			USB_OP_SET_ADC_MASTER,
			USB_OP_GET_ADC_CAL_STATE,
			USB_OP_SET_ADC_HIGH_PASS_FILTER,
			USB_OP_SET_ADC_ZCAL,
			USB_OP_SET_ADC_RESET,
			USB_OP_SET_ENABLE_QSD,
			USB_OP_SET_POWER_DOWN1,
			USB_OP_SET_POWER_DOWN2,
			USB_OP_GET_PLL_STATUS,
			USB_OP_SET_DDS_MASTER,
			USB_OP_SET_PLL_ON,
			USB_OP_SET_IMPULSE_ON,
			USB_OP_SET_PORT,
            USB_OP_WRITE_SPI,
            USB_OP_REBOOT,

			USB_OP_GET_FIRMWARE_REV = 1200,
			USB_OP_GET_TRX_OK = 1201,
			USB_OP_GET_TRX_REV = 1202,
			USB_OP_GET_TRX_SN = 1203,
			USB_OP_GET_PA_OK = 1204,
			USB_OP_GET_PA_REV = 1205,
			USB_OP_GET_PA_SN = 1206,
			USB_OP_GET_RFIO_OK = 1207,
			USB_OP_GET_RFIO_REV = 1208,
			USB_OP_GET_RFIO_SN = 1209,
			USB_OP_GET_ATU_OK = 1210,
			USB_OP_GET_ATU_REV = 1211,
			USB_OP_GET_ATU_SN = 1212,
			USB_OP_GET_RX2_OK = 1213,
			USB_OP_GET_RX2_REV = 1214,
			USB_OP_GET_RX2_SN = 1215,
			USB_OP_GET_VU_OK = 1216,
			USB_OP_GET_VU_REV = 1217,
			USB_OP_GET_VU_SN = 1218,
			USB_OP_INITIALIZE = 1219,		
			USB_OP_GET_SERIAL_NUM = 1220,
			USB_OP_GET_MODEL = 1221,	
			USB_OP_READ_TRX_EEPROM_UINT8 = 1222,
			USB_OP_WRITE_TRX_EEPROM_UINT8 = 1223,
			USB_OP_READ_TRX_EEPROM_UINT16 = 1224,
			USB_OP_WRITE_TRX_EEPROM_UINT16 = 1225,
			USB_OP_READ_TRX_EEPROM_UINT32 = 1226,	
			USB_OP_WRITE_TRX_EEPROM_UINT32 = 1227,
			USB_OP_READ_CLOCK_REG = 1228,
			USB_OP_WRITE_CLOCK_REG = 1229,
			USB_OP_READ_TRX_DDS_REG = 1230,
			USB_OP_WRITE_TRX_DDS_REG = 1231,
			USB_OP_READ_PIO_REG = 1232,
			USB_OP_WRITE_PIO_REG = 1233,
			USB_OP_TRX_POT_GET_RDAC = 1234,
			USB_OP_TRX_POT_SET_RDAC = 1235,				
			USB_OP_PA_POT_GET_RDAC = 1236,
			USB_OP_PA_POT_SET_RDAC = 1237,	
			USB_OP_SET_MUX = 1241,
			USB_OP_READ_CODEC_REG = 1242,
			USB_OP_WRITE_CODEC_REG = 1243,
			USB_OP_SET_RX1_FREQ = 1244,
			USB_OP_SET_RX2_FREQ = 1245,
			USB_OP_SET_TX_FREQ = 1246,
			USB_OP_SET_TRX_PREAMP = 1247,
			USB_OP_SET_TEST = 1248,
			USB_OP_SET_GEN = 1249,
			USB_OP_SET_SIG = 1250,
			USB_OP_SET_IMPULSE = 1251,
			USB_OP_SET_XVEN = 1252,
			USB_OP_SET_XVTXEN = 1253,
			USB_OP_SET_QSD = 1254,
			USB_OP_SET_QSE = 1255,
			USB_OP_SET_XREF = 1256,
			USB_OP_SET_RX1_FILTER = 1257,
			USB_OP_SET_RX2_FILTER = 1258,
			USB_OP_SET_TX_FILTER = 1259,
			USB_OP_SET_PA_FILTER = 1260,
			USB_OP_SET_INT_SPKR = 1261,
			USB_OP_SET_RX1_TAP = 1262,
			USB_OP_BYPASS_RX1_FILTER = 1263,
			USB_OP_BYPASS_TX_FILTER = 1264,
			USB_OP_BYPASS_PA_FILTER = 1265,
			USB_OP_READ_PTT = 1266,
			USB_OP_SET_HEADPHONE = 1267,
			USB_OP_SET_PLL = 1268,
			USB_OP_SET_RCA_TX1 = 1269,
			USB_OP_SET_RCA_TX2 = 1270,
			USB_OP_SET_RCA_TX3 = 1271,
			USB_OP_SET_FAN = 1272,
			USB_OP_GET_PLL_STATUS2 = 1273,
			USB_OP_READ_PA_ADC = 1274,
			USB_OP_SET_RX1_LOOP = 1275,
			USB_OP_SET_TR = 1276,
			USB_OP_SET_ANT = 1277,
			USB_OP_SET_RX1_ANT = 1278,
			USB_OP_SET_TX_ANT = 1279,
			USB_OP_SET_TXMON = 1280,
			USB_OP_SET_XVCOM = 1281,
			USB_OP_SET_EN_2M = 1282,
			USB_OP_SET_KEY_2M = 1283,
			USB_OP_SET_XVTR = 1284,
			USB_OP_SET_PA_BIAS = 1285,
			USB_OP_SET_POWER_OFF = 1286,
			USB_OP_SET_FPLED = 1287,
			USB_OP_SET_CTS = 1288,
			USB_OP_SET_RTS = 1289,
			USB_OP_SET_PC_RESET = 1290,
			USB_OP_SET_PC_PWRBT = 1291,
			USB_OP_SET_MOX = 1292,
			USB_OP_SET_INT_LED = 1293,
			USB_OP_ATU_SEND_CMD = 1294,
			USB_OP_ATU_GET_RESULT = 1295,
			USB_OP_SET_FULL_DUPLEX = 1296,
			USB_OP_SET_TX_DAC = 1297,
			USB_OP_SET_AMP_TX1 = 1298,
			USB_OP_SET_AMP_TX2 = 1299,
			USB_OP_SET_AMP_TX3 = 1300,
			USB_OP_SET_XVTR_ACTIVE = 1301,
			USB_OP_SET_XVTR_SPLIT = 1302,
			USB_OP_SET_RX1OUT = 1303,
			USB_OP_FLEXWIRE_WRITE_VALUE = 1304,
			USB_OP_FLEXWIRE_WRITE_2_VALUE = 1305,
			USB_OP_SET_RX1_DSP_MODE = 1306,
			USB_OP_SET_RX2_ANT = 1307,
			USB_OP_READ_RX2_EEPROM_UINT8 = 1308,
			USB_OP_WRITE_RX2_EEPROM_UINT8 = 1309,
			USB_OP_READ_RX2_EEPROM_UINT16 = 1310,
			USB_OP_WRITE_RX2_EEPROM_UINT16 = 1311,
			USB_OP_READ_RX2_EEPROM_UINT32 = 1312,	
			USB_OP_WRITE_RX2_EEPROM_UINT32 = 1313,
			USB_OP_READ_RX2_DDS_REG = 1314,
			USB_OP_WRITE_RX2_DDS_REG = 1315,
			USB_OP_SET_RX2_ON = 1319,
			USB_OP_SET_STANDBY = 1320,
			USB_OP_BYPASS_RX2_FILTER = 1321,
			USB_OP_SET_AMP_TX1_DELAY_ENABLE = 1322,
			USB_OP_SET_AMP_TX2_DELAY_ENABLE = 1323,
			USB_OP_SET_AMP_TX3_DELAY_ENABLE = 1324,
			USB_OP_SET_AMP_TX1_DELAY = 1325,
			USB_OP_SET_AMP_TX2_DELAY = 1326,
			USB_OP_SET_AMP_TX3_DELAY = 1327,
			USB_OP_RESET_RX2_DDS = 1328,
			USB_OP_SET_RX2_PREAMP = 1329,
			USB_OP_SET_RX2_DSP_MODE = 1330,
			USB_OP_SET_TRX_POT = 1331,	
			USB_OP_SET_IAMBIC = 1332,
			USB_OP_SET_BREAK_IN = 1333,
			USB_OP_SET_MANUAL_RX1_FILTER = 1334,
			USB_OP_SET_MANUAL_RX2_FILTER = 1335,
			USB_OP_SET_EEPROM_WC = 1336,
			USB_OP_SET_HIZ = 1337,
			USB_OP_ENABLE_ATU = 1338,	
			USB_OP_SET_ATU_ATTN = 1339,
			USB_OP_SET_PDRVMON = 1340,
			USB_OP_SET_RX_ATTN = 1341,
			USB_OP_FLEXWIRE_READ_VALUE = 1342,
			USB_OP_FLEXWIRE_READ_2_VALUE = 1343,
			USB_OP_SET_FAN_PWM = 1344,
			USB_OP_SET_FAN_SPEED = 1345,
			USB_OP_SYNC_PHASE = 1346,
			USB_OP_SET_RX1_FREQ_TW = 1347,
			USB_OP_SET_RX2_FREQ_TW = 1348,
			USB_OP_SET_TX_FREQ_TW = 1349,
			USB_OP_GET_REGION = 1350,
            USB_OP_SET_TX_DSP_FILTER = 1351,
            USB_OP_SET_TX_OFFSET = 1352,
            USB_OP_SET_TX_DSP_MODE = 1353,
            USB_OP_SET_CW_PITCH = 1354,
            USB_OP_GET_STATUS = 1355,
            USB_OP_SET_VU_FAN_HIGH = 1356,
	        USB_OP_SET_VU_KEY_V = 1357,
	        USB_OP_SET_VU_TXIFU = 1358,
	        USB_OP_SET_VU_KEY_U = 1359,
	        USB_OP_SET_VU_RXURX2 = 1360,
	        USB_OP_SET_VU_RX2U = 1361,
	        USB_OP_SET_VU_RXIFU = 1362,
	        USB_OP_SET_VU_K6 = 1363,
	        USB_OP_SET_VU_K7 = 1364,
	        USB_OP_SET_VU_K8 = 1365,
	        USB_OP_SET_VU_K9 = 1366,
	        USB_OP_SET_VU_K10 = 1367,
	        USB_OP_SET_VU_K11 = 1368,
	        USB_OP_SET_VU_K12 = 1369,
	        USB_OP_SET_VU_K13 = 1370,
	        USB_OP_SET_VU_K14 = 1371,
	        USB_OP_SET_VU_RX2V = 1372,
	        USB_OP_SET_VU_TX_U = 1373,
	        USB_OP_SET_VU_TX_V = 1374,
            USB_OP_SET_MIC_SEL = 1375,
            USB_OP_SET_TX_GAIN = 1376,
		}

		#endregion

        #region Private Functions

        private static uint SwapBytes(uint x)
        {
            return (x & 0xff) << 24
                | (x & 0xff00) << 8
                | (x & 0xff0000) >> 8
                | (x & 0xff000000) >> 24;
        }

        private static ushort SwapBytes(ushort x)
        {
            return (ushort)((x & 0xff) << 8 | (x & 0xff00) >> 8);
        }

        #endregion

        #region Public Functions

        public static int GetFirmwareRev(out uint rev)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_FIRMWARE_REV, 0, 0, out rev);
            return val;
        }

        public static int GetTRXRev(out uint rev)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_TRX_REV, 0, 0, out rev);
            rev = SwapBytes(rev);
            return val;
        }

        public static int GetTRXSN(out uint sn)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_TRX_SN, 0, 0, out sn);
            sn = SwapBytes(sn);
            return val;
        }

        public static int GetPARev(out uint rev)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_PA_REV, 0, 0, out rev);
            rev = SwapBytes(rev);
            return val;
        }

        public static int GetPASN(out uint sn)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_PA_SN, 0, 0, out sn);
            sn = SwapBytes(sn);
            return val;
        }

        public static int WriteI2CValue(byte addr, byte data)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_I2C_WRITE_VALUE, (uint)addr, (uint)data);
        }

        public static int WriteI2C2Value(byte addr, byte data1, byte data2)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_I2C_WRITE_2_VALUE, (uint)addr, (uint)((data1 << 8) + data2));
        }

        public static int WriteGPIO(byte data)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_WRITE_GPIO, (uint)data, 0);
        }

        public static int ReadGPIO(out byte data)
        {
            uint x;
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_READ_GPIO, 0, 0, out x);
            data = (byte)x;
            return val;
        }

        public static int WriteSPI(byte num_bytes, byte data)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_WRITE_SPI, (uint)num_bytes, (uint)data);
        }

        public static int Initialize()
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_INITIALIZE, 0, 0);
        }

        public static int GetSerialNum(out uint sn)
        {
            int val = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_SERIAL_NUM, 0, 0, out sn);
            sn = SwapBytes(sn);
            return val;
        }

        unsafe public static int ReadTRXEEPROMByte(uint offset, out byte buf)
        {
            int val;
            fixed (byte* ptr = &buf)
                val = USBHIDAPI.ReadEEPROM((ushort)offset, 1, ptr);
            return val;
        }

        unsafe public static int ReadTRXEEPROMUshort(uint offset, out ushort buf)
        {
            int val;
            byte[] temp = new byte[2];
            fixed (byte* ptr = &temp[0])
                val = USBHIDAPI.ReadEEPROM((ushort)offset, 2, ptr);
            buf = BitConverter.ToUInt16(temp, 0);
            return val;
        }

        unsafe public static int ReadTRXEEPROMUint(uint offset, out uint buf)
        {
            int val;
            byte[] temp = new byte[4];
            fixed (byte* ptr = &temp[0])
                val = USBHIDAPI.ReadEEPROM((ushort)offset, 4, ptr);
            buf = BitConverter.ToUInt32(temp, 0);
            return val;
        }

        unsafe public static int ReadTRXEEPROMFloat(uint offset, out float buf)
        {
            int val;
            byte[] temp = new byte[4];
            fixed (byte* ptr = &temp[0])
                val = USBHIDAPI.ReadEEPROM((ushort)offset, 4, ptr);
            buf = BitConverter.ToSingle(temp, 0);
            return val;
        }

        unsafe public static int WriteTRXEEPROMByte(uint offset, byte buf)
        {
            return USBHIDAPI.WriteEEPROM((ushort)offset, 1, &buf);
        }

        unsafe public static int WriteTRXEEPROMUshort(uint offset, ushort buf)
        {
            byte[] temp = BitConverter.GetBytes(buf);
            fixed (byte* ptr = &temp[0])
                return USBHIDAPI.WriteEEPROM((ushort)offset, 2, ptr);
        }

        unsafe public static int WriteTRXEEPROMUint(uint offset, uint buf)
        {
            byte[] temp = BitConverter.GetBytes(buf);
            fixed (byte* ptr = &temp[0])
                return USBHIDAPI.WriteEEPROM((ushort)offset, 4, ptr);
        }

        unsafe public static int WriteTRXEEPROMFloat(uint offset, float val)
        {
            byte[] temp = BitConverter.GetBytes(val);
            fixed (byte* ptr = &temp[0])
                return USBHIDAPI.WriteEEPROM((ushort)offset, 4, ptr);
        }

        public static int SetTest(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_TEST, Convert.ToUInt32(b), 0);
        }

        public static int SetGen(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_GEN, Convert.ToUInt32(b), 0);
        }

        public static int SetQSD(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_QSD, Convert.ToUInt32(b), 0);
        }

        public static int SetQSE(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_QSE, Convert.ToUInt32(b), 0);
        }

        public static int SetXref(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_XREF, Convert.ToUInt32(b), 0);
        }

        public static int SetRXFilter(int n)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_RX1_FILTER, (uint)n, 0);
        }

        public static int SetPAFilter(int n)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_PA_FILTER, (uint)n, 0);
        }

        public static int ReadPTT(out uint val)
        {
            return USBHIDAPI.ReadOp(Opcode.USB_OP_READ_PTT, 0, 0, out val);
        }

        public static int SetTXOut(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_RCA_TX1, Convert.ToUInt32(b), 0);
        }

        public static int SetLED(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_FPLED, Convert.ToUInt32(b), 0);
        }

        public static int SetMOX(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_TR, Convert.ToUInt32(b), 0);
        }

        public static int SetRXAnt(int n) // PA=0, XVRX=1, XVTX=2, BITE=3
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_RX1_ANT, (uint)n, 0);
        }

        public static int SetTXAnt(int n) // PA=0, XVRX=1, XVTX=2, BITE=3
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_TX_ANT, (uint)n, 0);
        }

        public static int FlexWire_Write2Value(byte addr, byte b1, byte b2)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_FLEXWIRE_WRITE_2_VALUE, addr, (uint)((b1 << 8) + b2));
        }

        public static int SetPreamp(FLEX1500PreampMode mode)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_TRX_PREAMP, (uint)mode, 0);
        }

        public static int SetFreqTW(uint tw) 
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_RX1_FREQ_TW, tw, 0);
        }

        public static int GetRegion(out FRSRegion region)
        {
            uint data;
            int rtn = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_REGION, 0, 0, out data);
            if ((FRSRegion)data >= FRSRegion.LAST)
            {
                region = 0;
                return rtn;
            }
            
            region = (FRSRegion)data;
            return rtn;
        }

        public static bool GetStatus()
        {
            uint status;
            int rtn = USBHIDAPI.ReadOp(Opcode.USB_OP_GET_STATUS, 0, 0, out status);
            return (status != 0);
        }

        unsafe public static int WriteEEPROM(ushort addr, byte[] buf) // up to 32 bytes
        {
            fixed (byte* ptr = &buf[0])
                return USBHIDAPI.WriteEEPROM(addr, (byte)buf.Length, ptr);
        }

        unsafe public static int WriteEEPROM(ushort addr, byte num_bytes, byte[] buf)
        {
            fixed (byte* ptr = &buf[0])
                return USBHIDAPI.WriteEEPROM(addr, (byte)num_bytes, ptr);
        }

        unsafe public static int ReadEEPROM(ushort addr, byte num_bytes, out byte[] buf)
        {
            buf = new byte[num_bytes];
            fixed (byte* ptr = &buf[0])
                return USBHIDAPI.ReadEEPROM(addr, num_bytes, ptr);
        }

        unsafe public static int SetMicSel(bool b)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_MIC_SEL, Convert.ToUInt32(b), 0);
        }

        unsafe public static int SetTXGain(uint val)
        {
            return USBHIDAPI.WriteOp(Opcode.USB_OP_SET_TX_GAIN, val, 0);
        }


        #endregion
    }
}
