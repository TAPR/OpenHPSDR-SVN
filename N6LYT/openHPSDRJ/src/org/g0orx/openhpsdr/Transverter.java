package org.g0orx.openhpsdr;

import java.io.Serializable;

public class Transverter extends Band implements Serializable {

        private static final long serialVersionUID = 1L;

    public Transverter() {

    }

        public Transverter(int bandstacks) {
        super(bandstacks);
        }

        public void setIfFrequency(long frequency) {
                iffrequency=frequency;
        }

        public long getIfFrequency() {
                return iffrequency;
        }

        private long iffrequency;
}
