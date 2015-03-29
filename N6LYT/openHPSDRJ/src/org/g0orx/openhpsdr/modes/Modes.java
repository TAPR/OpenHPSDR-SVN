package org.g0orx.openhpsdr.modes;

import org.g0orx.openhpsdr.Filter;

public class Modes {

    public Modes() {
    }

    public static int length() {
        return modes.length;
    }

    public static Mode getMode(int i) {
        if(i<modes.length) {
            return modes[i];
        }
        return null;
    }

    private static Filter[] LSBfilters={
            new Filter(-5150,-150,"5.0k"),
            new Filter(-4550,-150,"4.4k"),
            new Filter(-3950,-150,"3.8k"),
            new Filter(-3450,-150,"3.3k"),
            new Filter(-3050,-150,"2.9k"),
            new Filter(-2850,-150,"2.7k"),
            new Filter(-2550,-150,"2.4k"),
            new Filter(-2250,-150,"2.1k"),
            new Filter(-1950,-150,"1.8k"),
            new Filter(-1150,-150,"1.0k")
    };

    private static Filter[] USBfilters={
            new Filter(150,5150,"5.0k"),
            new Filter(150,4550,"4.4k"),
            new Filter(150,3950,"3.8k"),
            new Filter(150,3450,"3.3k"),
            new Filter(150,3050,"2.9k"),
            new Filter(150,2850,"2.7k"),
            new Filter(150,2550,"2.4k"),
            new Filter(150,2250,"2.1k"),
            new Filter(150,1950,"1.8k"),
            new Filter(150,1150,"1.0k")
    };

    private static Filter[] DSBfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };

    private static Filter[] CWLfilters={
            new Filter(500,500,"1.0k"),
            new Filter(400,400,"800"),
            new Filter(375,375,"750"),
            new Filter(300,300,"600"),
            new Filter(250,250,"500"),
            new Filter(200,200,"400"),
            new Filter(125,125,"250"),
            new Filter(50,50,"100"),
            new Filter(25,25,"50"),
            new Filter(13,13,"25")
    };

    private static Filter[] CWUfilters={
            new Filter(500,500,"1.0k"),
            new Filter(400,400,"800"),
            new Filter(375,375,"750"),
            new Filter(300,300,"600"),
            new Filter(250,250,"500"),
            new Filter(200,200,"400"),
            new Filter(125,125,"250"),
            new Filter(50,50,"100"),
            new Filter(25,25,"50"),
            new Filter(13,13,"25")
    };

    private static Filter[] FMNfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };

    private static Filter[] AMfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };

    private static Filter[] DIGLfilters={
            new Filter(-5150,-150,"5.0k"),
            new Filter(-4550,-150,"4.4k"),
            new Filter(-3950,-150,"3.8k"),
            new Filter(-3450,-150,"3.3k"),
            new Filter(-3050,-150,"2.9k"),
            new Filter(-2850,-150,"2.7k"),
            new Filter(-2550,-150,"2.4k"),
            new Filter(-2250,-150,"2.1k"),
            new Filter(-1950,-150,"1.8k"),
            new Filter(-1150,-150,"1.0k")
    };

    private static Filter[] SPECfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };

    private static Filter[] DIGUfilters={
            new Filter(150,5150,"5.0k"),
            new Filter(150,4550,"4.4k"),
            new Filter(150,3950,"3.8k"),
            new Filter(150,3450,"3.3k"),
            new Filter(150,3050,"2.9k"),
            new Filter(150,2850,"2.7k"),
            new Filter(150,2550,"2.4k"),
            new Filter(150,2250,"2.1k"),
            new Filter(150,1950,"1.8k"),
            new Filter(150,1150,"1.0k")
    };

    private static Filter[] SAMfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };

     private static Filter[] DRMfilters={
            new Filter(-8000,8000,"16k"),
            new Filter(-6000,6000,"12k"),
            new Filter(-5000,5000,"10k"),
            new Filter(-4000,4000,"8k"),
            new Filter(-3300,3300,"6.6k"),
            new Filter(-2600,2600,"5.2k"),
            new Filter(-2000,2000,"4.0k"),
            new Filter(-1550,1550,"3.1k"),
            new Filter(-1450,1450,"2.9k"),
            new Filter(-1200,1200,"2.4k")
    };


    private static Mode[] modes={
            new Mode("LSB",LSBfilters),
            new Mode("USB",USBfilters),
            new Mode("DSB",DSBfilters),
            new Mode("CWL",CWLfilters),
            new Mode("CWU",CWUfilters),
            new Mode("FMN",FMNfilters),
            new Mode("AM",AMfilters),
            new Mode("DIGU",DIGUfilters),
            new Mode("SPEC",SPECfilters),
            new Mode("DIGL",DIGLfilters),
            new Mode("SAM",SAMfilters),
            new Mode("DRM",DRMfilters)
    };

                public static final int LSB=0;
                public static final int USB=1;
                public static final int DSB=2;
                public static final int CWL=3;
                public static final int CWU=4;
                public static final int FMN=5;
                public static final int AM=6;
                public static final int DIGU=7;
                public static final int SPEC=8;
                public static final int DIGL=9;
                public static final int SAM=10;
                public static final int DRM=11;

}

