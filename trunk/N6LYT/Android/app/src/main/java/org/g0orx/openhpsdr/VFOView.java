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
public class VFOView extends SurfaceView {

    public VFOView(Context context, AttributeSet attributes) {
        super(context, attributes);

        Log.i("VFOView", "constructor");
        configuration = Configuration.getInstance();
        holder = this.getHolder();
        paint = new Paint();
    }

    public void setMetis(Metis metis) {
        this.metis=metis;
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

        if (WIDTH == 0 || HEIGHT == 0 || canvas == null) {
            return;
        }



        Band band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter = Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());

        paint.setColor(Color.BLACK);
        canvas.drawRect(0, 0, WIDTH, HEIGHT, paint);

        paint.setTextSize(50.0F);

        long frequency = bandstack.getFrequency();
        if (metis!=null && metis.isTransmitting()) {
            paint.setColor(Color.RED);
        } else {
            paint.setColor(Color.GREEN);
        }

        String status="";
        if(metis.getStatus()!=null) {
            status=" Error: "+metis.getStatus();
        }

        canvas.drawText(Frequency.toString(frequency) + " " + Modes.getMode(bandstack.getMode()).getName() + " " + filter.getName()+"Hz"+status, 0, HEIGHT-10, paint);

        if (configuration.subrx) {
            paint.setColor(Color.GRAY);
            canvas.drawText(Frequency.toString(bandstack.getSubRxFrequency()), WIDTH/2, HEIGHT-10, paint);
        }
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
    private Metis metis;

    private Paint paint;
    private SurfaceHolder holder;

    private DashPathEffect dashPath = new DashPathEffect(new float[]{1, 4}, 1);

    private int WIDTH = 0;
    private int HEIGHT = 0;
}
