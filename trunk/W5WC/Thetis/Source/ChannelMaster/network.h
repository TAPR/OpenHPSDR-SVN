#define MAX_ADC                 (3)
#define MAX_RX_STREAMS			(12)
#define MAX_TX_STREAMS			(3)
#define MAX_SYNC_RX             (2)
#define CACHE_ALIGN __declspec (align(16))

#include <winsock2.h>
#include <ws2tcpip.h>

#include "analyzer.h"
#include "cmcomm.h"

#ifndef _network_h
#define _network_h

typedef struct CACHE_ALIGN _radionet
{
	double* RxReadBufp;
	double* TxReadBufp;
	unsigned char* ReadBufp;
	//unsigned char* readbuf;
	char* OutBufp;
	double* syncrxbuff[2];
	int rx_base_port;
	int run;
	int sendHighPriority;
	int num_adc;
	int num_dac;
	int ptt_in;
	int dot_in;
	int dash_in;
	int oc_output;
	int supply_volts;
	int user_adc0;
	int user_adc1;
	int user_adc2;
	int user_adc3;
	int user_io;
	unsigned int cc_seq_no;
	unsigned int cc_seq_err;
	HANDLE hReadThreadMain;
	HANDLE hReadThreadInitSem;
	HANDLE hKeepAliveThread;
	HANDLE hTimer;
	LARGE_INTEGER liDueTime;
	CRITICAL_SECTION udpOUT;
	CRITICAL_SECTION rcvpkt;
	CRITICAL_SECTION sndpkt;
	WSAEVENT hDataEvent;
	WSANETWORKEVENTS wsaProcessEvents;

	// puresignal settings
	int puresignal_run;

	// wideband settings
	int wb_base_port;
	int wb_base_dispid;
	int wb_samples_per_packet;
	int wb_sample_size;
	int wb_update_rate;
	int wb_packets_per_frame;
	volatile long wb_enable;

	struct _adc
	{
		int id;
		int rx_step_attn;
		int tx_step_attn;
		int adc_overload;
		int dither;
		int random;
		// wideband dynamic variables & data (per adc)
		int wb_seqnum;
		int wb_state;
		double* wb_buff;
	} adc[MAX_ADC];

	struct _cw
	{
		int sidetone_level;
		int sidetone_freq;
		int keyer_speed;
		int keyer_weight;
		int hang_delay;
		int rf_delay;
#pragma pack(push, 1)
		union
		{
			unsigned char mode_control;
			struct {
				unsigned char eer              :1, // bit 00
                              cw_enable        :1, // bit 01
                              rev_paddle       :1, // bit 02
                              iambic           :1, // bit 03
                              sidetone         :1, // bit 04
                              mode_b           :1, // bit 05
                              strict_spacing   :1, // bit 06
                              break_in         :1; // bit 07
			};
		};
#pragma pack(pop, 1)
	}cw;

	struct _mic
	{
		int line_in_gain;
#pragma pack(push, 1)
		union
		{
			unsigned char mic_control;
			struct {
				unsigned char line_in     :1; // bit 00
                unsigned char mic_boost   :1; // bit 01
                unsigned char mic_ptt     :1; // bit 02
                unsigned char mic_trs     :1; // bit 03
                unsigned char mic_bias    :1; // bit 04
                unsigned char             :1; // bit 05
                unsigned char             :1; // bit 06
                unsigned char             :1; // bit 07
			};
		};
#pragma pack(pop, 1)
	}mic;

	struct _rx
	{
		int id;
		int rx_adc;
		int frequency;
		int enable;
		int sync;
		int sampling_rate;
		int bit_depth;
		int preamp;
		unsigned int rx_in_seq_no;
		unsigned int rx_in_seq_err;
		unsigned int rx_out_seq_no;
		unsigned _int64 time_stamp;
		unsigned int bits_per_sample;
		unsigned int IQ_samples_per_frame;
	} rx[MAX_RX_STREAMS];

	struct _tx
	{
		int id;
		int frequency;
		int sampling_rate;
		int cwx;
	    int dash;
	    int dot;
	    int ptt_out;
		int drive_level;
		int exciter_power;
		int fwd_power;
		int rev_power;
		int phase_shift;
		int epwm_max;
		int epwm_min;
		int pa;
		unsigned int mic_in_seq_no;
		unsigned int mic_in_seq_err;
		unsigned int mic_out_seq_no;
	} tx[MAX_TX_STREAMS];

	struct _discovery
	{
		unsigned char MACAddr[6];
		char BoardType;
		char protocolVersion;
		char fwCodeVersion;
		char MercuryVersion_0;
		char MercuryVersion_1;
		char MercuryVersion_2;
		char MercuryVersion_3;
		char PennyVersion;
		char MetisVersion;
		char numRxs;
	} discovery;

} radionet, *RADIONET;

RADIONET prn;

#pragma pack(push, 1)
typedef struct _rbpfilter // radio band pass filter
{
	int enable;
	union
	{
		unsigned bpfilter;
		struct {
			unsigned _rx_yellow_led:1; // bit 00
			unsigned _13MHz_HPF    :1; // bit 01
			unsigned _20MHz_HPF    :1; // bit 02
			unsigned _6M_preamp    :1; // bit 03
			unsigned _9_5MHz_HPF   :1; // bit 04
			unsigned _6_5MHz_HPF   :1; // bit 05
			unsigned _1_5MHz_HPF   :1; // bit 06
			unsigned               :1; // bit 07

			unsigned _XVTR_Rx_In   :1; // bit 08
			unsigned _Rx_2_In      :1; // bit 09 EXT1
			unsigned _Rx_1_In      :1; // bit 10 EXT2
			unsigned _Rx_1_Out     :1; // bit 11 K36
			unsigned _Bypass       :1; // bit 12
			unsigned _20_dB_Atten  :1; // bit 13
			unsigned _10_dB_Atten  :1; // bit 14
			unsigned _rx_red_led   :1; // bit 15

			unsigned               :1; // bit 16
			unsigned               :1; // bit 17
			unsigned               :1; // bit 18
			unsigned _tx_yellow_led:1; // bit 19
			unsigned _30_20_LPF    :1; // bit 20
			unsigned _60_40_LPF    :1; // bit 21
			unsigned _80_LPF       :1; // bit 22
			unsigned _160_LPF      :1; // bit 23

			unsigned _ANT_1        :1; // bit 24
			unsigned _ANT_2        :1; // bit 25
			unsigned _ANT_3        :1; // bit 26
			unsigned _TR_Relay     :1; // bit 27
			unsigned _tx_red_led   :1; // bit 28
			unsigned _6_LPF        :1; // bit 29
			unsigned _12_10_LPF    :1; // bit 30
			unsigned _17_15_LPF    :1; // bit 31
		};
	};
}rbpfilter, *RBPFILTER;
#pragma pack(pop, 1)
RBPFILTER prbpfilter;

extern __declspec(dllexport) void create_rnet();
void WriteUDPFrame(int id, char *bufp, int buflen);
void sendPacket(SOCKET sock, char *data, int length, int port);
void CmdGeneral(void);
void CmdHighPriority(void);
void CmdRx(void);
void CmdTx(void);
DWORD WINAPI ReadThreadMain(LPVOID);
DWORD WINAPI KeepAliveMain(LPVOID);
void ReadThreadMainLoop();
void KeepAliveLoop();

// IOThread rountines
int IOThreadStop(void);
int MetisStartReadThread(void);
void MetisStopReadThread(void);
int io_keep_running;
int IOThreadRunning;   // non zero if IOThread is running

int DotDashBits;
int XmitBit;
int SampleRate;
int SampleBits;  // how many bits in a sample

unsigned char ControlBytesIn[5];
unsigned char ControlBytesOut[5];
unsigned int LostSyncCount;
unsigned int SyncGainedCount;

int HaveSync;
int VFOfreq;
int ADC_cntrl1;
int ADC_cntrl2;

int C1Mask;
char PennyOCBits; /* pennny open collector bits */

int nreceivers;

int AlexAtten;
int Alex2Atten;
int Alex3Atten;
int Alex4Atten;
int MercDither;
int MercPreamp;
int RX1Preamp;
int RX2Preamp;
int MicTipRing;
int MicBias;
int MicPTT;
int Merc3Preamp;
int Merc4Preamp;
int MercRandom;
int enable_ADC1_step_att;
int enable_ADC2_step_att;
int enable_ADC3_step_att;
int adc1_step_att_data;
int adc2_step_att_data;
int adc3_step_att_data;
int tx_att_data;
int PureSignal;
int MercTxAtten;

int enable_cw_keyer;
int cw_sidetone_volume;
int cw_ptt_delay;
int cw_hang_time;
int cw_sidetone_freq;
int cw_speed;
int cw_mode;
int cw_weight;
int enable_cw_spacing;
int reverse_paddles;
int cw_dash;
int cw_dot;
int cwx;

int MicBoost;
int LineIn;
int LineBoost;

int UserOut0;
int UserOut1;
int UserOut2;
int UserOut3;

unsigned char AlexManEnable;
unsigned char AlexEnabled;
int AlexHPFMask;
int AlexLPFMask;
int AlexTRRelay;
int Alex2HPFMask;
int Alex2LPFMask;
int Alex3HPFMask;
int Alex3LPFMask;
int Alex4HPFMask;
int Alex4LPFMask;

int AlexRxAnt;
int AlexTxAnt;
int AlexRxOut;
int ExciterPower;
int RevPower;
int FwdPower;

int ApolloFilt;
int ApolloTuner;
int ApolloATU;
int HermesFilt;

int ADC_Overload; // Single ADC
int ADC1_Overload;
int ADC2_Overload;
int ADC3_Overload;
int ADC4_Overload;
int ADC_Overloads;
int User_I01;
int User_I02;
int User_I03;
int User_I04;
float swr_protect;
unsigned char pf;

unsigned int OutputPowerFactor;
int HermesDCV;

int CandCAddrMask;
int CandCFwdPowerBit;
int DotDashMask;

SOCKET listenSock;
WSADATA WSAdata;
int WSAinitialized;
static const double const_1_div_2147483648_ = 1.0 / 2147483648.0;


#endif
