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

import org.g0orx.openhpsdr.modes.Modes;

/**
 * Created by john on 04/02/15.
 */
public class FrequencyView extends SurfaceView {

    public FrequencyView(Context context, AttributeSet attributes) {
        super(context, attributes);

        Log.i("FrequencyView", "constructor");
        configuration = Configuration.getInstance();
        holder = this.getHolder();
        paint = new Paint();
    }

    public void onLayout(boolean changed, int left, int top, int right, int bottom) {
        super.onLayout(changed, left, top, right, bottom);
        //Log.i("PanadapterView","onLayout changed:"+changed+" left:"+left+" top:"+top+" right:"+right+" bottom:"+bottom);
        if (changed) {
            WIDTH = right - left;
            HEIGHT = bottom - top;
            Log.i("FrequencyView", "width=" + WIDTH + " height=" + HEIGHT);
        }
    }

    public void draw(Canvas canvas) {
        //Log.i("PanadapterView","onDraw: width="+WIDTH+" height="+HEIGHT);
        if (WIDTH == 0 || HEIGHT == 0 || canvas == null) {
            return;
        }

        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        int low=filter.getLow();
        int high=filter.getHigh();
        if(bandstack.getMode()==Modes.CWL) {
            low=-configuration.cwsidetonefrequency-low;
            high=-configuration.cwsidetonefrequency+high;
        } else if(bandstack.getMode()==Modes.CWU) {
            low=configuration.cwsidetonefrequency-low;
            high=configuration.cwsidetonefrequency+high;
        }
        filterLeft = ((int) low - (-(int) configuration.samplerate / 2)) * WIDTH / (int) configuration.samplerate;
        filterRight = ((int) high - (-(int) configuration.samplerate / 2)) * WIDTH / (int) configuration.samplerate;
        if (filterLeft == filterRight) {
            filterRight++;
        }

        long frequency = bandstack.getFrequency();

        // fill in the background
        paint.setTextSize(20.0F);
        paint.setColor(Color.BLACK);
        canvas.drawRect(0, 0, WIDTH, HEIGHT, paint);

        // draw the filter
        paint.setColor(Color.DKGRAY);
        canvas.drawRect(filterLeft, 0, filterRight, HEIGHT, paint);

        paint.setColor(Color.WHITE);
        paint.setStrokeWidth(1);


        // plot the vertical frequency markers
        float hzPerPixel = (float) configuration.samplerate / (float) WIDTH;
        long f = frequency - ((long) configuration.samplerate / 2);
        String fs;
        for (int i = 0; i < WIDTH; i++) {
            f = frequency - ((long) configuration.samplerate / 2) + (long) (hzPerPixel * i);
            if (f > 0) {
                if ((f % 20000) < (long) hzPerPixel) {
                    canvas.drawLine(i, 0, i, HEIGHT - 30, paint);
                    fs = String.format("%d.%02d", f / 1000000, (f % 1000000) / 10000);
                    canvas.drawText(fs, i - 20, HEIGHT - 5, paint);
                } else if((f%10000)<(long)hzPerPixel) {
                    canvas.drawLine(i, 0, i, HEIGHT - 35, paint);
                }
            }
        }

        // plot the cursor
        paint.setColor(Color.RED);
        canvas.drawLine((WIDTH / 2), 0, (WIDTH / 2), HEIGHT, paint);

    }

    public boolean update() {
        if (holder.getSurface().isValid()) {
            Canvas canvas = holder.lockCanvas();
            draw(canvas);
            holder.unlockCanvasAndPost(canvas);
        }
        return holder.getSurface().isValid();
    }

    private Configuration configuration;

    private Paint paint;
    private SurfaceHolder holder;

    private DashPathEffect dashPath = new DashPathEffect(new float[]{1, 4}, 1);

    private int WIDTH = 0;
    private int HEIGHT = 0;

    private int filterLeft;
    private int filterRight;
}
