package org.g0orx.openhpsdr;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.DashPathEffect;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.util.Log;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import org.g0orx.openhpsdr.discovery.Discovered;

/**
 * Created by john on 08/02/15.
 */
public class BandscopeView extends SurfaceView {

    public BandscopeView(Context context, AttributeSet attributes) {
        super(context, attributes);
        Log.i("BandscopeView", "constructor");
        configuration = Configuration.getInstance();
        holder = this.getHolder();
        paint = new Paint();
    }

    public void onLayout(boolean changed, int left, int top, int right, int bottom) {
        super.onLayout(changed, left, top, right, bottom);
        Log.i("BandscopeView","onLayout changed:"+changed+" left:"+left+" top:"+top+" right:"+right+" bottom:"+bottom);
        if (changed) {
            width = right - left;
            height = bottom - top;
            points = new float[width * 4];
            Log.i("BandscopeView", "width=" + width + " height=" + height);
        }
    }

    public void draw(Canvas canvas) {
        //Log.i("BandscopeView","onDraw: width="+width+" height="+height);
        if (width == 0 || height == 0 || canvas == null || points==null) {
            return;
        }
        // fill in the background
        paint.setTextSize(16.0F);
        paint.setColor(Color.BLACK);
        canvas.drawRect(0, 0, width, height, paint);

        float hzPerPixel = 62440000F / (float) width; // 124.88 MHz xtal
        if(configuration.discovered.getDevice()== Discovered.DEVICE_HERMES_LITE) {
            hzPerPixel = 30720000F / (float)width; // 61.44 MHz xtal
        }

        Band[] bands=configuration.bands.getBands();
        for(int i=0;i<bands.length;i++) {
            if(bands[i] instanceof Band  && bands[i].canTransmit()) {
                Band band=bands[i];
                BandEdge edges=band.getBandEdge();
                float x1=(float)edges.getLow()/hzPerPixel;
                float x2=(float)edges.getHigh()/hzPerPixel;
                if((int)x1!=(int)x2) {
                    paint.setColor(Color.DKGRAY);
                    canvas.drawRect(x1, 0, x2, height - 30, paint);
                    paint.setColor(Color.WHITE);
                    canvas.drawText(band.getName(), x1, 20, paint);
                }
            }
        }

        // plot the spectrum levels
        int V = HIGH - LOW;
        int numSteps = V / 20;
        paint.setTextSize(20.0F);
        for (int i = 1; i < numSteps; i++) {
            int num = HIGH - i * 20;
            int y = (int) Math.floor((HIGH - num) * height / V);

            paint.setColor(Color.YELLOW);
            paint.setPathEffect(dashPath);
            paint.setStrokeWidth(1);
            canvas.drawLine(0, y, width, y, paint);

            paint.setColor(Color.WHITE);
            canvas.drawText(Integer.toString(num) + " dBm", 3, y - 2, paint);
        }

        // plot the vertical frequency markers

        float f = 0;
        String fs;

        int maxfrequency=65;
        if(configuration.discovered.getDevice()== Discovered.DEVICE_HERMES_LITE) {
            maxfrequency=32;
        }

        for(int i=2;i<65;i+=2) {
            f=(float)i*1000000.0F;
            float x=f/hzPerPixel;
            paint.setColor(Color.LTGRAY);
            canvas.drawLine(x, 0, x, height - 30, paint);
            fs = String.format("%d", i);
            paint.setColor(Color.WHITE);
            canvas.drawText(fs, x - 10, height - 5, paint);
        }

        paint.setColor(Color.YELLOW);
        paint.setPathEffect(null);
        canvas.drawLines(points, paint);

    }

    public void update(float[] samples) {
            //Log.i("BandscopeView","update");

        if(width==samples.length) {

            BandStack bandstack = configuration.bands.get().get();

            if (points == null) {
                points = new float[width * 4];
            }

            int p = 0;
            float sample;
            float previous = 0.0F;

            for (int i = 0; i < width; i++) {

                sample = samples[i];

                sample = sample + (-20.0F);

                sample = (float) Math
                        .floor(((float) HIGH - sample)
                                * (float) height
                                / (float) (HIGH - LOW));
                if (i == 0) {
                    points[p++] = (float) i;
                    points[p++] = sample;
                } else {
                    points[p++] = (float) i;
                    points[p++] = previous;
                }

                points[p++] = (float) i;
                points[p++] = sample;


                previous = sample;
            }

            if (holder.getSurface().isValid()) {
                Canvas canvas = holder.lockCanvas();
                draw(canvas);
                holder.unlockCanvasAndPost(canvas);
            }
        }

    }

    private Configuration configuration;

    private Paint paint;
    private SurfaceHolder holder;
    
    private int width;
    private int height;

    private float[] points;

    private static final int HIGH=-40;
    private static final int LOW=-160;

    private DashPathEffect dashPath = new DashPathEffect(new float[]{1, 4}, 1);
}
