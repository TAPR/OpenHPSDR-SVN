package org.g0orx.openhpsdr;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Typeface;
import android.util.AttributeSet;
import android.util.Log;
import android.widget.ImageView;

import org.g0orx.openhpsdr.discovery.Discovered;

/**
 * Created by john on 14/11/14.
 */
public class MeterView extends ImageView {

    public MeterView(Context context) {
        super(context);
        initialize();
    }

    public MeterView(Context context, AttributeSet attrs)
    {
        super(context, attrs);
        initialize();
    }

    public MeterView(Context context, AttributeSet attrs, int defStyle)
    {
        super(context, attrs, defStyle);
        initialize();
    }

    public void initialize() {
        configuration=Configuration.getInstance();
        this.setImageResource(R.drawable.meter);
        receiving=true;
    }

    public void setDensity(float density) {
        this.density=density;
        Log.i("SMeterView","density="+density);
    }

    protected void onDraw(Canvas c)
    {
        super.onDraw(c);

        Paint paint=new Paint();

        double radius=(double)getHeight()*4.0/2.0;

        double centerX=(double)getWidth()/2.0;
        double centerY=radius;

        double angle=224.0;

        if(receiving) {
            paint.setColor(Color.GREEN);
            paint.setStrokeWidth(3);

            angle = 224.0 + ((double) (meter + 133.0) * 0.85);
            double pX = centerX + radius * Math.cos(Math.toRadians(angle));
            double pY = centerY + radius * Math.sin(Math.toRadians(angle));
            c.drawLine((float) centerX, (float) centerY, (float) pX, (float) pY, paint);

            paint.setColor(Color.BLACK);
            paint.setTypeface(Typeface.DEFAULT_BOLD);
            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(10.0F*density);
            c.drawText(Integer.toString(meter)+" dBm",(float)centerX,(float)getHeight()-2,paint);

        } else {
            double volts=0.0;
            double watts=0.0;
            double fwdpower=0.0;
            double revpower=0.0;
            double swr=1.0;
            boolean haveswr=false;

            double bridgevoltage = 0.09;
            double refvoltage = 3.3;
            switch(configuration.discovered.getDevice()) {
                case Discovered.DEVICE_METIS:
                    break;
                case Discovered.DEVICE_HERMES:
                    bridgevoltage=0.095;
                    break;
                case Discovered.DEVICE_GRIFFIN:
                    break;
                case Discovered.DEVICE_ANGELIA:
                    break;
                case Discovered.DEVICE_ORION:
                    refvoltage=5.0;
                    bridgevoltage=0.083;
                    break;

            }
            switch(configuration.radio) {
                case Configuration.RADIO_UNKNOWN:
                case Configuration.METIS_PENELOPE:
                case Configuration.METIS_PENNYLANE:
                case Configuration.HERMES_BOARD_ONLY:
                case Configuration.ANGELIA_BOARD_ONLY:
                case Configuration.ORION_BOARD_ONLY:
                    volts=(double)penelope_forward_power/4095.0 * refvoltage;
                    watts = (volts * volts) / bridgevoltage;
                    fwdpower=watts;
                    revpower=0.0;
                    haveswr=false;
                    break;
                default:
                    volts = (double)alex_forward_power / 4095.0 * refvoltage;
                    watts = (volts*volts) / bridgevoltage;

                    fwdpower=watts;

                    volts = (double) alex_reverse_power / 4095.0 * refvoltage;
                    revpower = (volts * volts) / bridgevoltage;

                    double rho = Math.sqrt(revpower / fwdpower);
                    swr = Math.abs((1 + Math.abs(rho)) / (1 - Math.abs(rho)));
                    haveswr=true;
                    break;
            }

            paint.setColor(Color.RED);
            paint.setStrokeWidth(3);

            //Log.i("SMeterView","fwd="+fwdpower+" rev="+revpower+" swr="+swr);

            if(fwdpower<=5.0) {
                angle=224.0+(fwdpower*(13.0/5.0));
            } else if (fwdpower<=10.0) {
                angle=237.0+((fwdpower-5.0)*(7.0/5.0));
            } else if (fwdpower<=20.0) {
                angle=244.0+((fwdpower-10.0)*(10.0/10.0));
            } else if (fwdpower<=30.0) {
                angle=254.0+((fwdpower-20.0)*(8.0/10.0));
            } else if (fwdpower<=40.0) {
                angle=262.0+((fwdpower-30.0)*(7.0/10.0));
            } else if (fwdpower<=50.0) {
                angle=269.0+((fwdpower-40.0)*(7.0/10.0));
            } else if (fwdpower<=60.0) {
                angle=276.0+((fwdpower-50.0)*(7.0/10.0));
            } else if (fwdpower<=70.0) {
                angle=283.0+((fwdpower-60.0)*(6.0/10.0));
            } else if (fwdpower<=80.0) {
                angle=288.0+((fwdpower-70.0)*(6.0/10.0));
            } else if (fwdpower<=90.0) {
                angle=294.0+((fwdpower-90.0)*(6.0/10.0));
            } else if (fwdpower<=100.0) {
                angle = 298.0 + ((fwdpower - 90.0)*(6.0/10.0));
            } else {
                angle = 304 +((fwdpower-100.0) * (6.0/10.0));
            }

            //Log.i("SMeterView","angle="+angle);

            double pX = centerX + radius * Math.cos(Math.toRadians(angle));
            double pY = centerY + radius * Math.sin(Math.toRadians(angle));
            c.drawLine((float) centerX, (float) centerY, (float) pX, (float) pY, paint);

            paint.setColor(Color.BLACK);
            paint.setTypeface(Typeface.DEFAULT_BOLD);
            paint.setTextAlign(Paint.Align.CENTER);
            paint.setTextSize(10.0F*density);
            String w="";
            if(haveswr) {
                w=String.format("%1.2fW %1.1f:1",fwdpower,swr);
            } else {
                w=String.format("%1.2fW",fwdpower);
            }
            c.drawText(w,(float)centerX,(float)getHeight()-2,paint);
        }


    }

    public void setMeter(int meter) {
        this.meter=meter+(int)configuration.preampOffset+(int)configuration.meterCalibrationOffset;
        receiving=true;
        this.postInvalidate();
    }

    public void setPower(int penelope_forward_power,int alex_forward_power,int alex_reverse_power) {
        this.penelope_forward_power=penelope_forward_power;
        this.alex_forward_power=alex_forward_power;
        this.alex_reverse_power=alex_reverse_power;
        receiving=false;
        this.postInvalidate();
    }

    private Configuration configuration;

    private float density;

    private int meter;
    private int penelope_forward_power;
    private int alex_forward_power;
    private int alex_reverse_power;

    private boolean receiving;

}
