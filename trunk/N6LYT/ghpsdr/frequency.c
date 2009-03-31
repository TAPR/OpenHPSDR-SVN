//
// frequency.c
//

#include <gtk/gtk.h>
#include "band.h"
#include "frequency.h"

char* outOfBand="Out of band";

struct frequency_info frequencyInfo[]=
    {

 
        {153000, 279000, "AM - Long Wave",                   bandGen,FALSE}, 
        {530000, 1710000, "Broadcast AM Med Wave",            bandGen,FALSE},                                 

        {1800000, 1809999, "160M CW/Digital Modes",            band160, TRUE},
        {1810000, 1810000, "160M CW QRP",                      band160, TRUE},
        {1810001, 1842999, "160M CW",                          band160, TRUE},
        {1843000, 1909999, "160M SSB/SSTV/Wide Band",          band160, TRUE},
        {1910000, 1910000, "160M SSB QRP",                     band160, TRUE},
        {1910001, 1994999, "160M SSB/SSTV/Wide Band",          band160, TRUE},
        {1995000, 1999999, "160M Experimental",                band160, TRUE},

        {2300000, 2495000, "120M Short Wave",                  bandGen,FALSE}, 

        {2500000, 2500000, "WWV",                              bandWWV,FALSE},

        {3200000, 3400000, "90M Short Wave",                   bandGen,FALSE}, 

        {3500000, 3524999, "80M Extra CW",                     band80, TRUE},
        {3525000, 3579999, "80M CW",                           band80, TRUE},
        {3580000, 3589999, "80M RTTY",                         band80, TRUE},
        {3590000, 3590000, "80M RTTY DX",                      band80, TRUE},
        {3590001, 3599999, "80M RTTY",                         band80, TRUE},
        {3600000, 3699999, "75M Extra SSB",                    band80, TRUE},
        {3700000, 3789999, "75M Ext/Adv SSB",                  band80, TRUE},
        {3790000, 3799999, "75M Ext/Adv DX Window",            band80, TRUE},
        {3800000, 3844999, "75M SSB",                          band80, TRUE},
        {3845000, 3845000, "75M SSTV",                         band80, TRUE},
        {3845001, 3884999, "75M SSB",                          band80, TRUE},
        {3885000, 3885000, "75M AM Calling Frequency",         band80, TRUE},
        {3885001, 3999999, "75M SSB",                          band80, TRUE},

        {4750000, 4999999, "60M Short Wave",                   bandGen,FALSE}, 

        {5000000, 5000000, "WWV",                              bandWWV,FALSE}, 

        {5330500, 5330500, "60M Channel 1",                    band60, TRUE},
        {5346500, 5346500, "60M Channel 2",                    band60, TRUE},
        {5366500, 5366500, "60M Channel 3",                    band60, TRUE},
        {5371500, 5371500, "60M Channel 4",                    band60, TRUE},
        {5403500, 5403500, "60M Channel 5",                    band60, TRUE},

        {5900000, 6200000, "49M Short Wave",                   bandGen,FALSE}, 

        {7000000, 7024999, "40M Extra CW",                     band40, TRUE},
        {7025000, 7039999, "40M CW",                           band40, TRUE},
        {7040000, 7040000, "40M RTTY DX",                      band40, TRUE},
        {7040001, 7099999, "40M RTTY",                         band40, TRUE},
        {7100000, 7124999, "40M CW",                           band40, TRUE},
        {7125000, 7170999, "40M Ext/Adv SSB",                  band40, TRUE},
        {7171000, 7171000, "40M SSTV",                         band40, TRUE},
        {7171001, 7174999, "40M Ext/Adv SSB",                  band40, TRUE},
        {7175000, 7289999, "40M SSB",                          band40, TRUE},
        {7290000, 7290000, "40M AM Calling Frequency",         band40, TRUE},
        {7290001, 7299999, "40M SSB",                          band40, TRUE},

        {7300000, 7350000, "41M Short Wave",                   bandGen,FALSE}, 
        {9400000, 9900000, "31M Short Wave",                   bandGen,FALSE}, 

        {10000000, 10000000, "WWV",                            bandWWV,FALSE}, 

        {10100000, 10129999, "30M CW",                         band30, TRUE},
        {10130000, 10139999, "30M RTTY",                       band30, TRUE},
        {10140000, 10149999, "30M Packet",                     band30, TRUE},

        {11600000, 12100000, "25M Short Wave",                 bandGen,FALSE}, 
        {13570000, 13870000, "22M Short Wave",                 bandGen,FALSE}, 

        {14000000, 14024999, "20M Extra CW",                   band20, TRUE},
        {14025000, 14069999, "20M CW",                         band20, TRUE},
        {14070000, 14094999, "20M RTTY",                       band20, TRUE},
        {14095000, 14099499, "20M Packet",                     band20, TRUE},
        {14099500, 14099999, "20M CW",                         band20, TRUE},
        {14100000, 14100000, "20M NCDXF Beacons",              band20, TRUE},
        {14100001, 14100499, "20M CW",                         band20, TRUE},
        {14100500, 14111999, "20M Packet",                     band20, TRUE},
        {14112000, 14149999, "20M CW",                         band20, TRUE},
        {14150000, 14174999, "20M Extra SSB",                  band20, TRUE},
        {14175000, 14224999, "20M Ext/Adv SSB",                band20, TRUE},
        {14225000, 14229999, "20M SSB",                        band20, TRUE},
        {14230000, 14230000, "20M SSTV",                       band20, TRUE},
        {14230000, 14285999, "20M SSB",                        band20, TRUE},
        {14286000, 14286000, "20M AM Calling Frequency",       band20, TRUE},
        {14286001, 14349999, "20M SSB",                        band20, TRUE},

        {15000000, 15000000, "WWV",                            bandWWV,FALSE}, 

        {15100000, 15800000, "19M Short Wave",                 bandGen,FALSE}, 
        {17480000, 17900000, "16M Short Wave",                 bandGen,FALSE}, 

        {18068000, 18099999, "17M CW",                         band17, TRUE},
        {18100000, 18104999, "17M RTTY",                       band17, TRUE},
        {18105000, 18109999, "17M Packet",                     band17, TRUE},
        {18110000, 18110000, "17M NCDXF Beacons",              band17, TRUE},
        {18110001, 18167999, "17M SSB",                        band17, TRUE},

        {18900000, 19020000, "15M Short Wave",                 bandGen,FALSE}, 

        {20000000, 20000000, "WWV",                            bandWWV,FALSE}, 

        {21000000, 21024999, "15M Extra CW",                   band15, TRUE},
        {21025000, 21069999, "15M CW",                         band15, TRUE},
        {21070000, 21099999, "15M RTTY",                       band15, TRUE},
        {21100000, 21109999, "15M Packet",                     band15, TRUE},
        {21110000, 21149999, "15M CW",                         band15, TRUE},
        {21150000, 21150000, "15M NCDXF Beacons",              band15, TRUE},
        {21150001, 21199999, "15M CW",                         band15, TRUE},
        {21200000, 21224999, "15M Extra SSB",                  band15, TRUE},
        {21225000, 21274999, "15M Ext/Adv SSB",                band15, TRUE},
        {21275000, 21339999, "15M SSB",                        band15, TRUE},
        {21340000, 21340000, "15M SSTV",                       band15, TRUE},
        {21340001, 21449999, "15M SSB",                        band15, TRUE},

        {21450000, 21850000, "13M Short Wave",                 bandGen,FALSE}, 

        {24890000, 24919999, "12M CW",                         band12, TRUE},
        {24920000, 24924999, "12M RTTY",                       band12, TRUE},
        {24925000, 24929999, "12M Packet",                     band12, TRUE},
        {24930000, 24930000, "12M NCDXF Beacons",              band12, TRUE},
        {24930001, 24989999, "12M SSB Wideband",               band12, TRUE},

        {25600000, 26100000, "11M Short Wave",                 bandGen,FALSE}, 

        {28000000, 28069999, "10M CW",                         band10, TRUE},
        {28070000, 28149999, "10M RTTY",                       band10, TRUE},
        {28150000, 28199999, "10M CW",                         band10, TRUE},
        {28200000, 28200000, "10M NCDXF Beacons",              band10, TRUE},
        {28200001, 28299999, "10M Beacons",                    band10, TRUE},
        {28300000, 28679999, "10M SSB",                        band10, TRUE},
        {28680000, 28680000, "10M SSTV",                       band10, TRUE},
        {28680001, 28999999, "10M SSB",                        band10, TRUE},
        {29000000, 29199999, "10M AM",                         band10, TRUE},
        {29200000, 29299999, "10M SSB",                        band10, TRUE},
        {29300000, 29509999, "10M Satellite Downlinks",        band10, TRUE},
        {29510000, 29519999, "10M Deadband",                   band10, TRUE},
        {29520000, 29589999, "10M Repeater Inputs",            band10, TRUE},
        {29590000, 29599999, "10M Deadband",                   band10, TRUE},
        {29600000, 29600000, "10M FM Simplex",                 band10, TRUE},
        {29600001, 29609999, "10M Deadband",                   band10, TRUE},
        {29610000, 29699999, "10M Repeater Outputs",           band10, TRUE},

        {50000000, 50059999, "6M CW",                          band6, TRUE},
        {50060000, 50079999, "6M Beacon Sub-Band",             band6, TRUE},
        {50080000, 50099999, "6M CW",                          band6, TRUE},
        {50100000, 50124999, "6M DX Window",                   band6, TRUE},
        {50125000, 50125000, "6M Calling Frequency",           band6, TRUE},
        {50125001, 50299999, "6M SSB",                         band6, TRUE},
        {50300000, 50599999, "6M All Modes",                   band6, TRUE},
        {50600000, 50619999, "6M Non Voice",                   band6, TRUE},
        {50620000, 50620000, "6M Digital Packet Calling",      band6, TRUE},
        {50620001, 50799999, "6M Non Voice",                   band6, TRUE},
        {50800000, 50999999, "6M RC",                          band6, TRUE},
        {51000000, 51099999, "6M Pacific DX Window",           band6, TRUE},
        {51100000, 51119999, "6M Deadband",                    band6, TRUE},
        {51120000, 51179999, "6M Digital Repeater Inputs",     band6, TRUE},
        {51180000, 51479999, "6M Repeater Inputs",             band6, TRUE},
        {51480000, 51619999, "6M Deadband",                    band6, TRUE},
        {51620000, 51679999, "6M Digital Repeater Outputs",    band6, TRUE},
        {51680000, 51979999, "6M Repeater Outputs",            band6, TRUE},
        {51980000, 51999999, "6M Deadband",                    band6, TRUE},
        {52000000, 52019999, "6M Repeater Inputs",             band6, TRUE},
        {52020000, 52020000, "6M FM Simplex",                  band6, TRUE},
        {52020001, 52039999, "6M Repeater Inputs",             band6, TRUE},
        {52040000, 52040000, "6M FM Simplex",                  band6, TRUE},
        {52040001, 52479999, "6M Repeater Inputs",             band6, TRUE},
        {52480000, 52499999, "6M Deadband",                    band6, TRUE},
        {52500000, 52524999, "6M Repeater Outputs",            band6, TRUE},
        {52525000, 52525000, "6M Primary FM Simplex",          band6, TRUE},
        {52525001, 52539999, "6M Deadband",                    band6, TRUE},
        {52540000, 52540000, "6M Secondary FM Simplex",        band6, TRUE},
        {52540001, 52979999, "6M Repeater Outputs",            band6, TRUE},
        {52980000, 52999999, "6M Deadbands",                   band6, TRUE},
        {53000000, 53000000, "6M Remote Base FM Spx",          band6, TRUE},
        {53000001, 53019999, "6M Repeater Inputs",             band6, TRUE},
        {53020000, 53020000, "6M FM Simplex",                  band6, TRUE},
        {53020001, 53479999, "6M Repeater Inputs",             band6, TRUE},
        {53480000, 53499999, "6M Deadband",                    band6, TRUE},
        {53500000, 53519999, "6M Repeater Outputs",            band6, TRUE},
        {53520000, 53520000, "6M FM Simplex",                  band6, TRUE},
        {53520001, 53899999, "6M Repeater Outputs",            band6, TRUE},
        {53900000, 53900000, "6M FM Simplex",                  band6, TRUE},
        {53900010, 53979999, "6M Repeater Outputs",            band6, TRUE},
        {53980000, 53999999, "6M Deadband",                    band6, TRUE},

        {0,        0,        "",                               0,     FALSE}

        

    };

char* getFrequencyInfo(long long frequency) {

    char* result=outOfBand;

    struct frequency_info* info=frequencyInfo;

    while(info->minFrequency!=0) {
        if(frequency<info->minFrequency) {
            break;
        } else if(frequency>=info->minFrequency && frequency<=info->maxFrequency) {
            result=info->info;
            break;
        }
        info++;
    }

    return result;
}

int getBand(long long frequency) {
    int result=bandGen;

    struct frequency_info* info=frequencyInfo;

    while(info->minFrequency!=0) {
        if(frequency<info->minFrequency) {
            break;
        } else if(frequency>=info->minFrequency && frequency<=info->maxFrequency) {
            result=info->band;
            break;
        }
        info++;
    }

    return result;
}

