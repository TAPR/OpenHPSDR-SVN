package com.sun.itcto;

import java.nio.ByteBuffer;

public class G711A implements Codec {

    /**
      decode a-law (8 bit) to 16 bit linear
    */
    public byte[] decode(byte[] data,int offset,int remaining) {
        byte[] linearData=new byte[remaining*2];
	int outIx = 0;
        int i;
        short v;

        for (int inIx = offset; inIx < (offset+remaining); inIx++) {
            i=data[inIx]&0xFF;
            v=decodetable[i];
            // assumes BIGENDIAN
            linearData[outIx++]=(byte)((v>>8)&0xFF);
            linearData[outIx++]=(byte)(v&0xFF);
        }
        return linearData;
    }

    /**
      encode 16 bit linear to 8 bit a-Law
    */
    public void encode(ByteBuffer buffer,byte[] data,int offset,int length) {
        int sample;
        byte b;
        for(int i=0;i<length;i+=2) {
            // assumes BIGENDIAN
            sample=data[i]<<8;
            sample+=data[i+1]&0xFF;
    
            b=encodetable[sample&0xFFFF];
            buffer.put(b);
        }
    }

    private static short[] decodetable=new short[256];
    private static byte[] encodetable=new byte[65536];

    static {
   
        for (int i = 0; i < 256; i++) {
            int input = i ^ 85;
            int mantissa = (input & 15) << 4;
            int segment = (input & 112) >> 4;
            int value = mantissa + 8;
            if (segment >= 1) value += 256;
            if (segment > 1) value <<= (segment - 1);
            if ((input & 128) == 0) value = -value;
            decodetable[i]=(short)value;
        }

        for(int i=0;i<65536;i++) {
            short sample=(short)i;

            int sign=(sample&0x8000) >> 8;
            if(sign != 0){
                sample=(short)-sample;
                sign=0x80;
            }

            if(sample > 32635) sample = 32635;

            int exp=7;
            for(int expMask=0x4000;(sample&expMask)==0 && exp>0; exp--, expMask >>= 1) {
            }
            int mantis = (sample >> ((exp == 0) ? 4 : (exp + 3))) & 0x0f;
            byte alaw = (byte)(sign | exp << 4 | mantis);
            encodetable[i]=(byte)(alaw^0xD5);
        }
    }

}
