package org.g0orx.openhpsdr;

import org.g0orx.openhpsdr.modes.Modes;

import android.os.Bundle;
import android.app.Activity;
import android.util.Log;
import android.view.Menu;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

public class TransverterAddActivity extends Activity implements OnTouchListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        configuration=Configuration.getInstance();
        
        setTitle("openHPSDR XVTR Configuration ("+configuration.discovered.getDeviceName()+": IP="+configuration.discovered.getAddress()+" MAC="+configuration.discovered.getMac()+")");

        setContentView(R.layout.activity_transverter_add);
        
        Button addButton = (Button)findViewById(R.id.buttonAdd);
		addButton.setOnTouchListener(this);
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.transverter_add, menu);
        return true;
    }
    
    private Configuration configuration;

	@Override
	public boolean onTouch(View v, MotionEvent event) {
		if(v.getId()==R.id.buttonAdd && event.getAction()==MotionEvent.ACTION_DOWN) {
			EditText editText=(EditText)findViewById(R.id.editTextName);
			String name=editText.getText().toString();
			editText=(EditText)findViewById(R.id.editTextStart);
			String startFreq=editText.getText().toString();
			editText=(EditText)findViewById(R.id.editTextEnd);
			String endFreq=editText.getText().toString();
			editText=(EditText)findViewById(R.id.editTextIF);
			String ifFreq=editText.getText().toString();
			
			Log.i("TransverterAddActivity","Add: "+name+","+startFreq+","+endFreq+","+ifFreq);
			
			boolean error=false;
			if(name.length()==0) {
				Toast toast = Toast.makeText(this, "Name cannot be empty", Toast.LENGTH_SHORT);
				toast.show();
				error=true;
			} else  if(startFreq.length()==0) {
				Toast toast = Toast.makeText(this, "Start Frequency cannot be empty", Toast.LENGTH_SHORT);
				toast.show();
				error=true;
			} else if(endFreq.length()==0) {
				Toast toast = Toast.makeText(this, "End Frequency cannot be empty", Toast.LENGTH_SHORT);
				toast.show();
				error=true;
			} else if(ifFreq.length()==0) {
				Toast toast = Toast.makeText(this, "Osc Frequency cannot be empty", Toast.LENGTH_SHORT);
				toast.show();
				error=true;
			}
			
			long sfreq=0L;
			if(!error) {
				try {
				    sfreq=Long.parseLong(startFreq);
				} catch (NumberFormatException e) {
					Toast toast = Toast.makeText(this, "Start Frequency must be numeric", Toast.LENGTH_SHORT);
					toast.show();
					error=true;
				}
			}
			
			long efreq=0L;
			if(!error) {
				try {
				    efreq=Long.parseLong(endFreq);
				} catch (NumberFormatException e) {
					Toast toast = Toast.makeText(this, "End Frequency must be numeric", Toast.LENGTH_SHORT);
					toast.show();
					error=true;
				}
			}
			
			long ifreq=0L;
			if(!error) {
				try {
				    ifreq=Long.parseLong(ifFreq);
				} catch (NumberFormatException e) {
					Toast toast = Toast.makeText(this, "Osc Frequency must be numeric", Toast.LENGTH_SHORT);
					toast.show();
					error=true;
				}
			}
			
			if(!error) {
				if(sfreq>=efreq) {
					Toast toast = Toast.makeText(this, "End Frequency must be greater than Start Frequency", Toast.LENGTH_SHORT);
					toast.show();
					error=true;
				}
			}
			
			if(!error) {
				Transverter xvtr=new Transverter(3);
				xvtr.setName(name);
				xvtr.setIfFrequency(ifreq);
				BandEdge bandedge=new BandEdge();
				bandedge.setLow(sfreq);
				bandedge.setHigh(efreq);
				xvtr.setBandEdge(bandedge);
				BandStack bandstack=new BandStack();
                bandstack.setFilter(6);
				bandstack.setMode(Modes.USB);
				bandstack.setFrequency(sfreq+((efreq-sfreq)/2));
				xvtr.setBandStack(0, bandstack);
                bandstack=new BandStack();
                bandstack.setFilter(6);
                bandstack.setMode(Modes.USB);
                bandstack.setFrequency(sfreq+((efreq-sfreq)/2));
                xvtr.setBandStack(1, bandstack);
                bandstack=new BandStack();
                bandstack.setFilter(6);
                bandstack.setMode(Modes.USB);
                bandstack.setFrequency(sfreq+((efreq-sfreq)/2));
                xvtr.setBandStack(2, bandstack);
				configuration.bands.add(xvtr);
				
				Log.i("TransverterAddActivity",name+" Added");
				finish();
			}
		}
		return false;
	}
    
}
