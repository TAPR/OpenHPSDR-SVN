package org.g0orx;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;

public class JMonitor extends Activity {
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        connection=new Connection("81.146.61.118",8000);
        
        update=new Update(connection);
        
        spectrumView=new SpectrumView(this,connection);
        
        connection.setSpectrumView(spectrumView);
        
        setContentView(spectrumView);
        
    }
    
    public void onStart() {
    	super.onStart();
    	//Log.i("Jmonitor","onStart");
    	
    	connection.start();        
        connection.setFrequency(7048000);
        connection.setMode(0);
        connection.setFilter(-2850,-150);
        connection.setGain(30);
    }
    
    public void onRestart() {
    	super.onRestart();
    	//Log.i("Jmonitor","onRestart");
    	
    }
    
    public void onResume() {
    	super.onResume();
    	//Log.i("Jmonitor","onResume");
    	update.start();
    }
    
    public void onPause() {
    	super.onPause();
    	//Log.i("Jmonitor","onPause");
    }
    
    public void onStop() {
    	super.onStop();
    	//Log.i("Jmonitor","onStop");
    	update.close();
    	connection.close();
    }
    
    public void onDestroy() {
    	super.onDestroy();
    	//Log.i("Jmonitor","onDestroy");
    }

    public boolean onCreateOptionsMenu(Menu menu) {
    	menu.add(0, MENU_BAND, 0, "Band");
    	menu.add(0, MENU_MODE, 0, "Mode");
    	menu.add(0, MENU_FILTER, 0, "FILTER");
    	menu.add(0, MENU_AGC, 0, "AGC");
    	menu.add(0, MENU_DSP, 0, "DSP");
    	menu.add(0, MENU_GAIN, 0, "GAIN");
    	menu.add(0, MENU_QUIT, 0, "Quit");
    	return true;
    }
    
    public boolean onOptionsItemSelected(MenuItem item) {
    	switch(item.getItemId()) {
    	    case MENU_QUIT:
    		    this.finish();
    		    break;
    	    case MENU_BAND:
                showDialog(MENU_BAND);
    	        break;
    	    case MENU_MODE:
                showDialog(MENU_MODE);
    	        break;
    	    case MENU_FILTER:
                showDialog(MENU_FILTER);
    	        break;
    	    case MENU_AGC:
                showDialog(MENU_AGC);
    	        break;
    	    case MENU_DSP:
                showDialog(MENU_DSP);
    	    	break;
    	    case MENU_GAIN:
    	    	showDialog(MENU_GAIN);
    	    	break;
 
    	}
    	return true;
    }
    
    protected Dialog onCreateDialog(int id) {
    	Dialog dialog;
    	AlertDialog.Builder builder;
    	
    	switch(id) {
    	    case MENU_BAND:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Band");
    	    	builder.setSingleChoiceItems(bands,band,new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			//
    	    			band=item;
    	    			switch(item) {
    	    			case BAND_160:
    	    				connection.setMode(MODE_LSB);
    	    				connection.setFilter(-2850,-150);
    	    				connection.setFrequency(1850000);
    	    				break;
    	    			case BAND_80:
    	    				connection.setMode(MODE_LSB);
    	    				connection.setFilter(-2850,-150);
    	    				connection.setFrequency(3790000);
    	    				break;
    	    			case BAND_60:
    	    				connection.setMode(MODE_LSB);
    	    				connection.setFilter(-2850,-150);
    	    				connection.setFrequency(5371500);
    	    				break;
    	    			case BAND_40:
    	    				connection.setMode(MODE_LSB);
    	    				connection.setFilter(-2850,-150);
    	    				connection.setFrequency(7048000);
    	    				break;
    	    			case BAND_30:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(10135600);
    	    				break;
    	    			case BAND_20:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(14200000);
    	    				break;
    	    			case BAND_17:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(18118900);
    	    				break;
    	    			case BAND_15:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(21200000);
    	    				break;
    	    			case BAND_12:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(24910000);
    	    				break;
    	    			case BAND_10:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(28500000);
    	    				break;
    	    			case BAND_6:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(150,2850);
    	    				connection.setFrequency(50200000);
    	    				break;
    	    			case BAND_GEN:
    	    				connection.setMode(MODE_AM);
    	    				connection.setFilter(-4000,4000);
    	    				connection.setFrequency(909000);
    	    				break;
    	    			case BAND_WWV:
    	    				connection.setMode(MODE_USB);
    	    				connection.setFilter(-4000,4000);
    	    				connection.setFrequency(5000000);
    	    				break;
    	    			}
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    	    case MENU_MODE:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Mode");
    	    	builder.setSingleChoiceItems(modes,connection.getMode(),new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			connection.setMode(item);
    	    			switch(item) {
    	    			case MODE_LSB:
    	    				connection.setFilter(-2850,-150);
    	    				break;
    	    			case MODE_USB:
    	    				connection.setFilter(150,2850);
    	    				break;
    	    			case MODE_DSB:
    	    				connection.setFilter(-2600,2600);
    	    				break;
    	    			case MODE_CWL:
    	    				connection.setFilter(-800,-400);
    	    				break;
    	    			case MODE_CWU:
    	    				connection.setFilter(400,800);
    	    				break;
    	    			case MODE_FMN:
    	    				connection.setFilter(-2600,2600);
    	    				break;
    	    			case MODE_AM:
    	    				connection.setFilter(-4000,4000);
    	    				break;
    	    			case MODE_DIGU:
    	    				connection.setFilter(150,3450);
    	    				break;
    	    			case MODE_SPEC:
    	    				connection.setFilter(-6000,6000);
    	    				break;
    	    			case MODE_DIGL:
    	    				connection.setFilter(-3450,-150);
    	    				break;
    	    			case MODE_SAM:
    	    				connection.setFilter(-4000,4000);
    	    				break;
    	    			case MODE_DRM:
    	    				connection.setFilter(-6000,6000);
    	    				break;
    	    			}
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    	    case MENU_FILTER:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Band");
    	    	builder.setSingleChoiceItems(bands,-1,new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			//
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    	    case MENU_AGC:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Band");
    	    	builder.setSingleChoiceItems(bands,-1,new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			//
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    	    case MENU_DSP:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Band");
    	    	builder.setSingleChoiceItems(bands,-1,new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			//
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    	    case MENU_GAIN:
    	    	builder=new AlertDialog.Builder(this);
    	    	builder.setTitle("Select a Band");
    	    	builder.setSingleChoiceItems(bands,-1,new DialogInterface.OnClickListener() {
    	    		public void onClick(DialogInterface dialog,int item) {
    	    			//
    	    			dialog.dismiss();
    	    		}
    	    	});
    	    	dialog=builder.create();
    		    break;
    		default:
    		    dialog=null;
    		    break;
    	}
    	return dialog;
    }
    
    private Connection connection;
    private SpectrumView spectrumView;
    private Update update;
    
    public static final int MENU_QUIT=0;
    public static final int MENU_BAND=1;
    public static final int MENU_MODE=2;
    public static final int MENU_FILTER=3;
    public static final int MENU_AGC=4;
    public static final int MENU_DSP=5;
    public static final int MENU_GAIN=6;
    
    public static final CharSequence[] bands={"160","80","60","40","30","20","17","15","12","10","6","GEN","WWV"};
    public static final CharSequence[] modes={"LSB","USB","DSB","CWL","CWU","FMN","AM","DIGU","SPEC","DIGL","SAM","DRM"};
    
    private int band=BAND_40;
    
    public static final int BAND_160=0;
    public static final int BAND_80=1;
    public static final int BAND_60=2;
    public static final int BAND_40=3;
    public static final int BAND_30=4;
    public static final int BAND_20=5;
    public static final int BAND_17=6;
    public static final int BAND_15=7;
    public static final int BAND_12=8;
    public static final int BAND_10=9;
    public static final int BAND_6=10;
    public static final int BAND_GEN=11;
    public static final int BAND_WWV=12;
    
    public static final int MODE_LSB=0;
    public static final int MODE_USB=1;
    public static final int MODE_DSB=2;
    public static final int MODE_CWL=3;
    public static final int MODE_CWU=4;
    public static final int MODE_FMN=5;
    public static final int MODE_AM=6;
    public static final int MODE_DIGU=7;
    public static final int MODE_SPEC=8;
    public static final int MODE_DIGL=9;
    public static final int MODE_SAM=10;
    public static final int MODE_DRM=11;
    
    
}