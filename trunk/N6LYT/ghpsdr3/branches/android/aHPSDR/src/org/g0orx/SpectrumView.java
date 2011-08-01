package org.g0orx;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.DashPathEffect;
import android.graphics.Paint;
import android.graphics.Path;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.util.Log;

public class SpectrumView extends View implements OnTouchListener {

	public SpectrumView(Context context, int width,int height,Connection connection) {
		super(context);
		this.connection = connection;
		paint = new Paint();
		WIDTH=width;
		HEIGHT=height/2;
		points = new float[WIDTH * 4];

		waterfall = Bitmap.createBitmap(WIDTH, HEIGHT,
				Bitmap.Config.ARGB_8888);
		pixels = new int[WIDTH * HEIGHT];

		for (int x = 0; x < WIDTH; x++) {
			for (int y = 0; y < HEIGHT; y++) {
				waterfall.setPixel(x, y, Color.BLACK);
			}
		}

		this.setOnTouchListener(this);
	}

	void setSensors(float sensor1,float sensor2,float sensor3) {
		
		if(sensor2>(-1.9F+4.0F)) {
			connection.setFrequency((long) (connection.getFrequency() + 1000));
		} else if(sensor2>(-1.9F+3.0F)) {
			connection.setFrequency((long) (connection.getFrequency() + 100));
		} else if(sensor2>(-1.9F+2.0F)) {
			connection.setFrequency((long) (connection.getFrequency() + 10));
		} else if(sensor2<(-1.9F-4.0F)) {
			connection.setFrequency((long) (connection.getFrequency() - 1000));
		} else if(sensor2<(-1.9F-3.0F)) {
			connection.setFrequency((long) (connection.getFrequency() - 100));
		} else if(sensor2<(-1.9F-2.0F)) {
			connection.setFrequency((long) (connection.getFrequency() - 10));
		}
	}
	
	protected void onDraw(Canvas canvas) {
		if (connection.isConnected()) {

			// draw the filter
			paint.setColor(Color.GRAY);
			canvas.drawRect(filterLeft, 0, filterRight, HEIGHT, paint);

			// plot the spectrum levels
			paint.setColor(Color.GRAY);
			int V = spectrumHigh - spectrumLow;
			int numSteps = V / 20;
			int pixelStepSize = HEIGHT / numSteps;
			for (int i = 1; i < numSteps; i++) {
				int num = spectrumHigh - i * 20;
				int y = (int) Math.floor((spectrumHigh - num) * HEIGHT / V);

				paint.setColor(Color.YELLOW);
				canvas.drawLine(0, y, WIDTH, y, paint);

				paint.setColor(Color.GREEN);
				canvas.drawText(Integer.toString(num), 3, y + 2, paint);
			}

			// plot the vertical frequency markers
			float hzPerPixel=(float)connection.getSampleRate()/(float)WIDTH;
			long f=connection.getFrequency()-(connection.getSampleRate()/2);
			for(int i=0;i<WIDTH;i++) {
				f=connection.getFrequency()-(connection.getSampleRate()/2)+(long)(hzPerPixel*i);
				if(f>0) {
					if((f%10000)<(long)hzPerPixel) {
						paint.setColor(Color.YELLOW);
						DashPathEffect dashPath = new DashPathEffect(new float[]{1,4}, 1);
						paint.setPathEffect(dashPath);
					    paint.setStrokeWidth(1);
						canvas.drawLine(i,0,i,HEIGHT,paint);
						paint.setColor(Color.WHITE);
						paint.setPathEffect(null);
						long mhz=f/1000000;
						long khz=(f%1000000)/10000;
						
						canvas.drawText(Long.toString(mhz)+"."+((khz<10)?"0":"")+Long.toString(khz), i, HEIGHT-5, paint);
					}
				}
			}
			
			
			// plot the cursor
			paint.setColor(Color.RED);
			canvas.drawLine(WIDTH/2, 0, WIDTH/2, HEIGHT, paint);

			// display the frequency and mode
			paint.setColor(Color.GREEN);
			paint.setTextSize(24.0F);
			canvas.drawText(Long.toString(connection.getFrequency()) + " "
					+ connection.getStringMode(), 100, 25, paint);

			if (vfoLocked) {
				paint.setColor(Color.RED);
				canvas.drawText("LOCKED", 300, 10, paint);
			}

			// plot the spectrum
			paint.setColor(Color.WHITE);
			canvas.drawLines(points, paint);

			// draw the waterfall
			canvas.drawBitmap(waterfall, 1, HEIGHT, paint);

			// draw the S-Meter
			int smeter=connection.getMeter()+121;
			//paint.setColor(Color.GREEN);
			//canvas.drawText(Integer.toString(dbm)+"dBm", WIDTH-100, 25, paint);
			paint.setColor(Color.RED);
			canvas.drawRect(WIDTH-125,10,(WIDTH-125)+smeter,25, paint);
			paint.setColor(Color.GREEN);
			canvas.drawLine(WIDTH-125, 25, WIDTH-125, 10, paint);
			canvas.drawLine((WIDTH-125)+9, 25, (WIDTH-125)+9, 10, paint);
			canvas.drawLine((WIDTH-125)+18, 25, (WIDTH-125)+18, 10, paint);
			canvas.drawLine((WIDTH-125)+27, 25, (WIDTH-125)+27, 10, paint);
			canvas.drawLine((WIDTH-125)+47, 25, (WIDTH-125)+47, 10, paint);
			canvas.drawLine((WIDTH-125)+67, 25, (WIDTH-125)+67, 10, paint);
			canvas.drawLine((WIDTH-125)+87, 25, (WIDTH-125)+87, 10, paint);
			paint.setTextSize(8.0F);
			canvas.drawText("3", (WIDTH-125)+9-2, 35, paint);
			canvas.drawText("6", (WIDTH-125)+18-2, 35, paint);
			canvas.drawText("9", (WIDTH-125)+27-2, 35, paint);
			canvas.drawText("+20", (WIDTH-125)+47-4, 35, paint);
			canvas.drawText("+40", (WIDTH-125)+67-4, 35, paint);
			canvas.drawText("+60", (WIDTH-125)+87-4, 35, paint);

			
			
			String status = connection.getStatus();
			if (status != null) {
				paint.setColor(Color.RED);
				canvas.drawText(status, 0, 10, paint);
			}

		} else {
			paint.setColor(0xffffffff);
			canvas.drawRect(0, 0, canvas.getWidth(), canvas.getHeight(), paint);
			paint.setColor(Color.RED);
			canvas.drawText("Server is busy - please wait", 20, canvas
					.getHeight() / 2, paint);
		}
	}

	public void plotSpectrum(int[] samples, int filterLow, int filterHigh,
			int sampleRate) {

		// scroll the waterfall down

		waterfall.getPixels(pixels, 0, WIDTH, 0, 0, WIDTH, HEIGHT - 1);
		waterfall.setPixels(pixels, 0, WIDTH, 0, 1, WIDTH, HEIGHT - 1);

		int p = 0;
		float sample;
		float previous = 0.0F;
		for (int i = 0; i < WIDTH; i++) {
			sample = (float) Math
					.floor(((float) spectrumHigh - (float) samples[i])
							* (float) HEIGHT
							/ (float) (spectrumHigh - spectrumLow));
			if (i == 0) {
				points[p++] = (float) i;
				points[p++] = sample;
			} else {
				points[p++] = (float) i;
				points[p++] = previous;
			}

			points[p++] = (float) i;
			points[p++] = sample;

			waterfall.setPixel(i, 0, calculatePixel(samples[i]));

			previous = sample;
		}

		this.filterLow = filterLow;
		this.filterHigh = filterHigh;
		filterLeft = (filterLow - (-sampleRate / 2)) * WIDTH / sampleRate;
		filterRight = (filterHigh - (-sampleRate / 2)) * WIDTH / sampleRate;

		this.postInvalidate();
	}

	private int calculatePixel(float sample) {
		// simple gray scale
		int v = ((int) sample - waterfallLow) * 255
				/ (waterfallHigh - waterfallLow);

		if (v < 0)
			v = 0;
		if (v > 255)
			v = 255;

		int pixel = (255 << 24) + (v << 16) + (v << 8) + v;
		return pixel;
	}

	public void setVfoLock() {
		vfoLocked = !vfoLocked;
	}

	public void scroll(int step) {
		if (!vfoLocked) {
			connection
					.setFrequency((long) (connection.getFrequency() + (step * (connection
							.getSampleRate() / WIDTH))));
		}
	}

	public boolean onTouch(View view, MotionEvent event) {
		if (!vfoLocked) {
			switch (event.getAction()) {
			case MotionEvent.ACTION_CANCEL:
				// Log.i("onTouch","ACTION_CANCEL");
				break;
			case MotionEvent.ACTION_DOWN:
				// Log.i("onTouch","ACTION_DOWN");
				if (connection.isConnected()) {
					// connection.setStatus("onTouch.ACTION_DOWN: "+event.getX());
					startX = event.getX();
					moved=false;
					scroll=false;
/*
					if(event.getY()>((HEIGHT*2)-100)) {
						int step=100;
					    if(startX<(WIDTH/4)) {
					    	step=-100;
					    } else if(startX<(WIDTH/2)) {
						    step=-1000;
					    } else if(startX<((WIDTH-WIDTH/4))) {
				            step=1000;
					    } else {
						    step=100;
					    }
					    connection.setFrequency((long) (connection.getFrequency() + step));
					    scroll=true;	
					}
*/
				}
				break;
			case MotionEvent.ACTION_MOVE:
				// Log.i("onTouch","ACTION_MOVE");
				if (connection.isConnected()) {
					// connection.setStatus("onTouch.ACTION_MOVE: "+(int)event.getX());
					int increment = (int) (startX - event.getX());
					if(!scroll) {
						connection.setFrequency((long) (connection.getFrequency() + (increment * (connection
									.getSampleRate() / WIDTH))));
					startX = event.getX();
					moved=true;
			        } 
				}
				break;
			case MotionEvent.ACTION_OUTSIDE:
				// Log.i("onTouch","ACTION_OUTSIDE");
				break;
			case MotionEvent.ACTION_UP:
				// Log.i("onTouch","ACTION_UP");
				if (connection.isConnected()) {
					int scrollAmount = (int) ((event.getX() - (WIDTH / 2)) * (connection
							.getSampleRate() / WIDTH));

					if (!moved & !scroll) {
						// move this frequency to center of filter
						if (filterHigh < 0) {
							connection
									.setFrequency(connection.getFrequency()
											+ (scrollAmount + ((filterHigh - filterLow) / 2)));
						} else {
							connection
									.setFrequency(connection.getFrequency()
											+ (scrollAmount - ((filterHigh - filterLow) / 2)));
						}
					}
				}
				break;
			}
		}

		return true;
	}
	
	private Paint paint;

	private Connection connection;

	private int WIDTH = 480;
	private int HEIGHT = 160;

	private float[] points;

	Bitmap waterfall;
	int[] pixels;

	private int spectrumHigh = 0;
	private int spectrumLow = -140;

	private int waterfallHigh = -75;
	private int waterfallLow = -115;

	private int filterLow;
	private int filterHigh;

	private int filterLeft;
	private int filterRight;

	private boolean vfoLocked = false;

	private float startX;
	private boolean moved;
	private boolean scroll;

}
