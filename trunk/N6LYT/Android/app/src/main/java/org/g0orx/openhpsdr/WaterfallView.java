package org.g0orx.openhpsdr;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.util.AttributeSet;
import android.util.Log;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import org.g0orx.openhpsdr.discovery.Discovered;

public class WaterfallView extends SurfaceView {

	public WaterfallView(Context context,AttributeSet attributes) {
		super(context,attributes);
		Log.i("WaterfallView","constructor");
		paint = new Paint();
	}
	
	public void onLayout(boolean changed, int left, int top, int right, int bottom) {
        super.onLayout(changed, left, top, right, bottom);
		
		//Log.i("WaterfallView","onLayout changed:"+changed+" left:"+left+" top:"+top+" right:"+right+" bottom:"+bottom);
	    if(changed) {
	    	
	    	holder = this.getHolder();
			
		    width=right-left;
	        height=bottom-top;
	        
	        Log.i("WaterfallView","width="+width+" height="+height);

            waterfall = Bitmap.createBitmap(width, height,
                        Bitmap.Config.ARGB_8888);
	        
	        pixels = new int[width * height];

			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					waterfall.setPixel(x, y, Color.BLACK);
				}
			}

	    }
	}
	
	public void onSizeChanged (int w, int h, int oldw, int oldh) {
		super.onSizeChanged(w, h, oldw, oldh);
		Log.i("WaterfallView","onSizeChanged: w="+w+" h="+h+" oldw="+oldw+" oldh="+oldh);
	}
	
	public void draw(Canvas canvas) {
		//Log.i("WaterfallView","draw: width="+width+" height="+height);
		if(canvas!=null) {
			if(waterfall==null) {
				paint.setColor(Color.BLACK);
				canvas.drawRect(0, 0, width, height, paint);
			} else {
				canvas.drawBitmap(waterfall, 0, 0, paint);
			}
            /*
            // paint the radio details
            float sz=paint.getTextSize();
            paint.setTextSize(sz*2.0F);
            paint.setColor(Color.GRAY);
            Discovered d=configuration.discovered;
            if(d!=null){
                canvas.drawText(d.getDeviceName() + " " + d.getAddress() + " (" + d.getMac() + ")", 0, height - 40, paint);
            }
            paint.setTextSize(sz);
            */
		}
    }
	
	public void update(float[] samples) {
		if(samples!=null) {
			if(samples.length==width) {
				if(holder!=null) {
					if(holder.getSurface().isValid()) {
						if(waterfall!=null) {
							if(waterfall.isRecycled()) {
								Log.e("WaterfallView","add: waterfall.isRecycled");
								waterfall = Bitmap.createBitmap(width, height,
										Bitmap.Config.ARGB_8888);
								if(waterfall!=null) {
									for (int x = 0; x < width; x++) {
										for (int y = 0; y < height; y++) {
											waterfall.setPixel(x, y, Color.BLACK);
										}
									}
								} else {
									Log.e("WaterfallView","add: createBitmap failed!");
								}
							}

							//Log.i("WaterfallView","add: scroll waterfall");
							if(waterfall!=null) {
								waterfall.getPixels(pixels, 0, width, 0, 0, width, height - 1);
								waterfall.setPixels(pixels, 0, width, 0, 1, width, height - 1);


								int average=0;

								for(int i=0;i<width;i++) {
									waterfall.setPixel(i, 0, calculatePixel(samples[i]));
									average+=(int)samples[i];
								}
                                if(configuration.waterfallAutomatic) {
                                    average = average / samples.length;
                                    configuration.waterfallLow = average - 5;
                                    configuration.waterfallHigh = average + 55;
                                }
							} else {
								Log.e("WaterfallView", "waterfall is null");
							}
						}

						//Log.i("WaterfallView","add: call draw");
						Canvas canvas = holder.lockCanvas();
						if(canvas!=null) {
							draw(canvas);
							holder.unlockCanvasAndPost(canvas);
						}
					}
				}
			}
		}
	}
	
    private int calculatePixel(float sample) {
	    //Log.i("WaterfallView","calculatePixel:"+sample);
		
		int R,G,B;

            if (sample < configuration.waterfallLow) {
                R = colorLowR;
                G = colorLowG;
                B = colorLowB;
            } else if (sample > configuration.waterfallHigh) {
                R = colorHighR;
                G = colorHighG;
                B = colorHighB;
            } else {
                float range = configuration.waterfallHigh - configuration.waterfallLow;
                float offset = sample - configuration.waterfallLow;
                float percent = offset / range;
                if (percent < (2.0f / 9.0f)) {
                    float local_percent = percent / (2.0f / 9.0f);
                    R = (int) ((1.0f - local_percent) * colorLowR);
                    G = (int) ((1.0f - local_percent) * colorLowG);
                    B = (int) (colorLowB + local_percent * (255 - colorLowB));
                } else if (percent < (3.0f / 9.0f)) {
                    float local_percent = (percent - 2.0f / 9.0f) / (1.0f / 9.0f);
                    R = 0;
                    G = (int) (local_percent * 255);
                    B = 255;
                } else if (percent < (4.0f / 9.0f)) {
                    float local_percent = (percent - 3.0f / 9.0f) / (1.0f / 9.0f);
                    R = 0;
                    G = 255;
                    B = (int) ((1.0f - local_percent) * 255);
                } else if (percent < (5.0f / 9.0f)) {
                    float local_percent = (percent - 4.0f / 9.0f) / (1.0f / 9.0f);
                    R = (int) (local_percent * 255);
                    G = 255;
                    B = 0;
                } else if (percent < (7.0f / 9.0f)) {
                    float local_percent = (percent - 5.0f / 9.0f) / (2.0f / 9.0f);
                    R = 255;
                    G = (int) ((1.0f - local_percent) * 255);
                    B = 0;
                } else if (percent < (8.0f / 9.0f)) {
                    float local_percent = (percent - 7.0f / 9.0f) / (1.0f / 9.0f);
                    R = 255;
                    G = 0;
                    B = (int) (local_percent * 255);
                } else {
                    float local_percent = (percent - 8.0f / 9.0f) / (1.0f / 9.0f);
                    R = (int) ((0.75f + 0.25f * (1.0f - local_percent)) * 255.0f);
                    G = (int) (local_percent * 255.0f * 0.5f);
                    B = 255;
                }
            }

        if(configuration.waterfallGrayscale) {
            final float GS_RED = 0.299F;
            final float GS_GREEN = 0.587F;
            final float GS_BLUE = 0.114F;
            R = G = B = (int)(GS_RED * R + GS_GREEN * G + GS_BLUE * B);
        }

	    int pixel = (255 << 24)+(R << 16)+(G << 8) + B;

	    return pixel;
	}
	
    private Configuration configuration=Configuration.getInstance();
    
	private SurfaceHolder holder;
	
	private int width=0;
	private int height=0;

	private Bitmap waterfall;
	int[] pixels;
	private Paint paint;
	
	private int colorLowR=0;
	private int colorLowG=0;
	private int colorLowB=0;
	private int colorMidR=255;
	private int colorMidG=0;
	private int colorMidB=0;
	private int colorHighR=255;
	private int colorHighG=255;
	private int colorHighB=0;
	
}
