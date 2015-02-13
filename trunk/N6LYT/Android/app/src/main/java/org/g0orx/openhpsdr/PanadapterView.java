package org.g0orx.openhpsdr;

import java.util.Timer;
import java.util.TimerTask;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.DashPathEffect;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.util.Log;
import android.view.MotionEvent;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.View;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.modes.Modes;

public class PanadapterView extends SurfaceView {

    public PanadapterView(Context context, AttributeSet attributes) {
        super(context, attributes);

        Log.i("PanadapterView", "constructor");

        configuration = Configuration.getInstance();
        holder = this.getHolder();
        paint = new Paint();
    }

    public void setMetis(Metis metis) {
        this.metis = metis;
    }

    public void onLayout(boolean changed, int left, int top, int right, int bottom) {
        super.onLayout(changed, left, top, right, bottom);
        //Log.i("PanadapterView","onLayout changed:"+changed+" left:"+left+" top:"+top+" right:"+right+" bottom:"+bottom);
        if (changed) {
            WIDTH = right - left;
            HEIGHT = bottom - top;

            Log.i("PanadapterView", "width=" + WIDTH + " height=" + HEIGHT);

            points = new float[WIDTH * 4];
        }
    }

    public void onSizeChanged(int w, int h, int oldw, int oldh) {
        super.onSizeChanged(w, h, oldw, oldh);
        Log.i("PanadapterView", "onSizeChanged: w=" + w + " h=" + h + " oldw=" + oldw + " oldh=" + oldh);
    }

    void setSensors(float sensor1, float sensor2, float sensor3) {
        BandStack bandstack = configuration.bands.get().get();
        if (sensor2 > (-1.9F + 4.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() - 1000L);
        } else if (sensor2 > (-1.9F + 3.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() - 100L);
        } else if (sensor2 > (-1.9F + 2.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() - 100L);
        } else if (sensor2 < (-1.9F - 4.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() + 1000L);
        } else if (sensor2 < (-1.9F - 3.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() + 100L);
        } else if (sensor2 < (-1.9F - 2.0F)) {
            bandstack.setFrequency(bandstack.getFrequency() + 10L);
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
        paint.setColor(Color.BLUE);
        canvas.drawRect(0, 0, WIDTH, HEIGHT, paint);

        // draw the filter
        paint.setColor(Color.DKGRAY);
        canvas.drawRect(filterLeft, 0, filterRight, HEIGHT, paint);

        if (configuration.subrx) {
            // draw the subrx filter
            int offset = (int) ((bandstack.getSubRxFrequency() - bandstack.getFrequency()) / (configuration.samplerate / WIDTH));
            paint.setColor(Color.GRAY);
            canvas.drawRect(filterLeft + offset, 0, filterRight + offset, HEIGHT, paint);
        }

        // plot the spectrum levels
        int V = configuration.spectrumHigh - configuration.spectrumLow;
        int numSteps = V / 20;
        paint.setTextSize(20.0F);
        for (int i = 1; i < numSteps; i++) {
            int num = configuration.spectrumHigh - i * 20;
            int y = (int) Math.floor((configuration.spectrumHigh - num) * HEIGHT / V);

            paint.setColor(Color.YELLOW);
            canvas.drawLine(0, y, WIDTH, y, paint);

            paint.setColor(Color.WHITE);
            canvas.drawText(Integer.toString(num) + " dBm", 3, y - 2, paint);
        }

        // plot the vertical frequency markers
        float hzPerPixel = (float) configuration.samplerate / (float) WIDTH;
        long f = frequency - ((long) configuration.samplerate / 2);
        String fs;
        for (int i = 0; i < WIDTH; i++) {
            f = frequency - ((long) configuration.samplerate / 2) + (long) (hzPerPixel * i);
            if (f > 0) {
                if ((f % 20000) < (long) hzPerPixel) {
                    paint.setColor(Color.YELLOW);

                    paint.setPathEffect(dashPath);
                    paint.setStrokeWidth(1);
                    canvas.drawLine(i, 0, i, HEIGHT, paint);

                    paint.setColor(Color.WHITE);
                    paint.setPathEffect(null);
                    /*
                    fs = String.format("%d.%02d", f / 1000000, (f % 1000000) / 10000);
                    canvas.drawText(fs, i - 20, HEIGHT - 5, paint);
                    */
                }
            }
        }

        // plot the cursor
        paint.setColor(Color.RED);
        canvas.drawLine((WIDTH / 2), 0, (WIDTH / 2), HEIGHT, paint);

        /*
        // paint the frequency
        if (metis!=null && metis.isTransmitting()) {
            paint.setColor(Color.RED);
        } else {
            paint.setColor(Color.GREEN);
        }
        paint.setTextSize(paint.getTextSize() * 2.0F);
        canvas.drawText(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz", WIDTH / 8, 40, paint);

        if (configuration.subrx) {
            paint.setColor(Color.GRAY);
            canvas.drawText(Frequency.toString(bandstack.getSubRxFrequency()), WIDTH - (WIDTH / 4), 40, paint);
        }
        */

        // plot the band edge
        BandEdge bandedge = band.getBandEdge();
        if (bandedge.getLow() != 0) {
            long minfrequency = frequency - ((long) configuration.samplerate / 2);
            long maxfrequency = frequency + ((long) configuration.samplerate / 2);
            double hzperpixel = configuration.samplerate / (double) WIDTH;
            if (bandedge.getLow() > minfrequency && bandedge.getLow() < maxfrequency) {
                // show lower band edge
                float x = (float) (bandedge.getLow() - minfrequency) / (float) hzperpixel;
                canvas.drawLine(x, 0, x, HEIGHT, paint);
                canvas.drawLine(x + 1, 0, x + 1, HEIGHT, paint);
            }

            if (bandedge.getHigh() > minfrequency && bandedge.getHigh() < maxfrequency) {
                // show upper band edge
                float x = (float) (bandedge.getHigh() - minfrequency) / (float) hzperpixel;
                canvas.drawLine(x, 0, x, HEIGHT, paint);
                canvas.drawLine(x + 1, 0, x + 1, HEIGHT, paint);
            }
        }

        // plot the spectrum
        paint.setColor(Color.WHITE);

        if (configuration.panadapter) {
            canvas.drawLines(points, paint);
        }

        // paint the radio details
        //float sz=paint.getTextSize();
        //paint.setTextSize(sz*2.0F);
        paint.setColor(Color.GRAY);
        Discovered d=configuration.discovered;
        if(d!=null){
            canvas.drawText(d.getDeviceName() + " " + d.getAddress() + " (" + d.getMac() + ")", 0, 20, paint);
        }
        //paint.setTextSize(sz);


        if(metis!=null && metis.isADC1Overflow()) {
            paint.setColor(Color.RED);
            canvas.drawText("ADC Overflow", WIDTH - (WIDTH / 4), 20, paint);
        }

    }

    public void plotSpectrum(float[] samples) {
        //Log.i("PanadapterView","plotSpectrum: width="+WIDTH+" height="+HEIGHT);

        BandStack bandstack = configuration.bands.get().get();

        if (points == null) {
            points = new float[WIDTH * 4];
        }

        int p = 0;
        float sample;
        float previous = 0.0F;

        float max = -400.0F;

        for (int i = 0; i < WIDTH; i++) {

            sample = samples[i];

            sample = sample + configuration.displayCalibrationOffset + configuration.preampOffset;

            if (sample > max) max = sample;

            sample = (float) Math
                    .floor(((float) configuration.spectrumHigh - sample)
                            * (float) HEIGHT
                            / (float) (configuration.spectrumHigh - configuration.spectrumLow));
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

    public void setVfoLock() {
        vfoLocked = !vfoLocked;
    }

    public void scroll(int step) {
        if (!vfoLocked) {
            BandStack bandstack = configuration.bands.get().get();
            bandstack.setFrequency((long) (bandstack.getFrequency() + (step * (configuration.samplerate / WIDTH))));
        }
    }

    public boolean onTouch(View view, MotionEvent event) {
        if (!vfoLocked) {
            BandStack bandstack = configuration.bands.get().get();
            switch (event.getAction()) {
                case MotionEvent.ACTION_CANCEL:
                    // Log.i("onTouch","ACTION_CANCEL");
                    break;
                case MotionEvent.ACTION_DOWN:
                    Log.i("onTouch", "ACTION_DOWN");

                    // connection.setStatus("onTouch.ACTION_DOWN: "+event.getX());
                    startX = event.getX();
                    startY = event.getY();
                    moved = false;
                    scroll = false;
                    jog = false;
                    if (startX <= 50 && startY >= (getHeight() - 50)) {
                        // frequency down
                        jog = true;
                        jogAmount = -100;
                        bandstack.setFrequency((long) (bandstack.getFrequency() + jogAmount));
                        timer = new Timer();
                        timer.schedule(new JogTask(), 500);
                    } else if (startX >= (getWidth() - 50) && startY >= (getHeight() - 50)) {
                        // frequency up
                        jog = true;
                        jogAmount = 100;
                        bandstack.setFrequency((long) (bandstack.getFrequency() + jogAmount));
                        timer = new Timer();
                        timer.schedule(new JogTask(), 500);
                    }

                    break;
                case MotionEvent.ACTION_MOVE:
                    // Log.i("onTouch","ACTION_MOVE");

                    if (!jog) {
                        // connection.setStatus("onTouch.ACTION_MOVE: "+(int)event.getX());
                        int increment = (int) (startX - event.getX());
                        if (!scroll) {
                            bandstack.setFrequency((long) (bandstack.getFrequency() + (increment * (configuration.samplerate / WIDTH))) / 10 * 10);
                            startX = event.getX();
                            moved = true;
                        }
                    }
                    break;
                case MotionEvent.ACTION_OUTSIDE:
                    // Log.i("onTouch","ACTION_OUTSIDE");
                    break;
                case MotionEvent.ACTION_UP:
                    Log.i("onTouch", "ACTION_UP");
                    if (!jog) {
                        int scrollAmount = (int) ((event.getX() - (WIDTH / 2)) * (configuration.samplerate / WIDTH));

                        if (!moved & !scroll) {
                            // move this frequency to center of filter
                            Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                            if (filter.getHigh() < 0) {
                                bandstack.setFrequency(bandstack.getFrequency()
                                        + (scrollAmount + (((long) filter.getHigh() - (long) filter.getLow()) / 2)));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency()
                                        + (scrollAmount - (((long) filter.getHigh() - (long) filter.getLow()) / 2)));
                            }
                        }
                    } else {
                        jog = false;
                        timer.cancel();
                    }
                    break;
            }
        }

        try {
            Thread.sleep(50);
        } catch (Exception e) {
            Log.i("PanadapterView", "onTouch: " + e.toString());
        }

        return true;
    }

    class JogTask extends TimerTask {
        public void run() {
            BandStack bandstack = configuration.bands.get().get();
            bandstack.setFrequency((long) (bandstack.getFrequency() + jogAmount));
            timer.schedule(new JogTask(), 50);
        }
    }

    private Configuration configuration;
    private Metis metis;

    private Paint paint;

    private int WIDTH = 0;
    private int HEIGHT = 0;

    private float[] points;

    private int filterLeft;
    private int filterRight;

    private boolean vfoLocked = false;

    private float startX;
    private float startY;
    private boolean moved;
    private boolean scroll;
    private boolean jog;

    private Timer timer;
    private long jogAmount;

    private DashPathEffect dashPath = new DashPathEffect(new float[]{1, 4}, 1);

    private SurfaceHolder holder;

    private long lastfrequency = 0L;
    private int lastfilterleft = 0;
    private int lastfilterright = 0;

}

