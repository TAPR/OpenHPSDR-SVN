package org.g0orx.openhpsdr;


// based on code from http://go-lambda.blogspot.co.uk/2012/02/rotary-knob-widget-on-android.html

import android.content.Context;
import android.graphics.Canvas;
import android.util.AttributeSet;
import android.view.View;
import android.view.MotionEvent;
import android.widget.ImageView;

public class TuningKnobView extends ImageView {
    private float angle = 0f;
    private float theta_old=0f;

    private TuningKnobListener listener;

    public interface TuningKnobListener {
        public void onKnobChanged(int arg);
    }

    public void setTuningKnobListener(TuningKnobListener l )
    {
        listener = l;
    }

    public TuningKnobView(Context context) {
        super(context);
        initialize();
    }

    public TuningKnobView(Context context, AttributeSet attrs)
    {
        super(context, attrs);
        initialize();
    }

    public TuningKnobView(Context context, AttributeSet attrs, int defStyle)
    {
        super(context, attrs, defStyle);
        initialize();
    }

    private float getTheta(float x, float y)
    {
        float sx = x - (getWidth() / 2.0f);
        float sy = y - (getHeight() / 2.0f);

        float length = (float)Math.sqrt( sx*sx + sy*sy);
        float nx = sx / length;
        float ny = sy / length;
        float theta = (float)Math.atan2( ny, nx );

        final float rad2deg = (float)(180.0/Math.PI);
        float thetaDeg = theta*rad2deg;

        return (thetaDeg < 0) ? thetaDeg + 360.0f : thetaDeg;
    }

    public void initialize()
    {
        this.setImageResource(R.drawable.tune);
        setOnTouchListener(new OnTouchListener()
        {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                float x = event.getX(0);
                float y = event.getY(0);
                float theta = getTheta(x,y);

                switch(event.getAction() & MotionEvent.ACTION_MASK)
                {
                    case MotionEvent.ACTION_POINTER_DOWN:
                        theta_old = theta;
                        break;
                    case MotionEvent.ACTION_MOVE:
                        invalidate();
                        float delta_theta = theta - theta_old;
                        theta_old = theta;
                        int direction = (delta_theta > 0) ? 1 : -1;
                        angle += 3*direction;
                        notifyListener(direction);
                        break;
                }
                return true;
            }
        });
    }

    private void notifyListener(int arg)
    {
        if (null!=listener)
            listener.onKnobChanged(arg);
    }

    protected void onDraw(Canvas c)
    {
        c.rotate(angle,getWidth()/2,getHeight()/2);
        super.onDraw(c);
    }
}
