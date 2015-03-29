package org.g0orx.openhpsdr;

import java.io.Serializable;

public class BandStack implements Serializable {

        private static final long serialVersionUID = 2L;

        public BandStack() {
        }

        public void setFrequency(long frequency) {
                this.frequency=frequency;
        }

        public void setSubRxFrequency(long frequency) {
                this.subrxfrequency=frequency;
        }

        public void setMode(int mode) {
                this.mode=mode;
        }

    public void setFilter(int filter) { this.filter=filter; }

        public void setRxAntenna(int antenna) {
                rxantenna=antenna;
        }

        public void setTxAntenna(int antenna) {
                txantenna=antenna;
        }

        public long getFrequency() {
                return frequency;
        }

        public long getSubRxFrequency() {
                return subrxfrequency;
        }

        public int getMode() {
                return mode;
        }

    public int getFilter() { return filter; }

        public int getRxAntenna() {
                return rxantenna;
        }

        public int getTxAntenna() {
                return txantenna;
        }

        private long frequency;
        private long subrxfrequency;
        private int mode;
    private int filter;


        private int rxantenna=NONE;
        private int txantenna=ANT1;

        public static final int ANT1=0;
        public static final int ANT2=1;
        public static final int ANT3=2;

        public static final int NONE=0;
        public static final int RX1=1; // ANAN EXT2
        public static final int RX2=2; // ANAN ERT1
        public static final int RXXV=3;

        //private static final String[] txantennanames={"ANT1","ANT2","ANT3"};
        //private static final String[] rxantennanames={"NONE (uses tx antenna)","RX1","RX2","RXXV"};

        //public static String getTxAntennaName(int antenna) {
        //      return txantennanames[antenna];
        //}

        //public static String getRxAntennaName(int antenna) {
        //      return rxantennanames[antenna];
        //}

}
