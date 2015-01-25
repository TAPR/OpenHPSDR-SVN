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

public class TransverterUpdateActivity extends Activity implements OnTouchListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
        configuration=Configuration.getInstance();
        
        setTitle("openHPSDR XVTR Configuration ("+configuration.discovered.getDeviceName()+": IP="+configuration.discovered.getAddress()+" MAC="+configuration.discovered.getMac()+")");

        setContentView(R.layout.activity_transverter_update);
        
        Bundle b = getIntent().getExtras();
        index = b.getInt("index");
        
        Band[] bands=configuration.bands.getBands();
        
        xvtr=(Transverter)bands[index];
        
        EditText name=(EditText)findViewById(R.id.editTextName);
        name.setText(xvtr.getName());
        EditText low=(EditText)findViewById(R.id.editTextStart);
        low.setText(Long.toString(xvtr.getBandEdge().getLow()));
        EditText high=(EditText)findViewById(R.id.editTextEnd);
        high.setText(Long.toString(xvtr.getBandEdge().getHigh()));
        EditText iff=(EditText)findViewById(R.id.editTextIF);
        iff.setText(Long.toString(xvtr.getIfFrequency()));
           
        Button updateButton = (Button)findViewById(R.id.buttonUpdate);
		updateButton.setOnTouchListener(this);
		
		Button deleteButton = (Button)findViewById(R.id.buttonDelete);
		deleteButton.setOnTouchListener(this);
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
		if(v.getId()==R.id.buttonUpdate && event.getAction()==MotionEvent.ACTION_DOWN) {
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
				Toast toast = Toast.makeText(this, "IF Frequency cannot be empty", Toast.LENGTH_SHORT);
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
					Toast toast = Toast.makeText(this, "IF Frequency must be numeric", Toast.LENGTH_SHORT);
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
				xvtr.setName(name);
				xvtr.setIfFrequency(ifreq);
				BandEdge bandedge=new BandEdge();
				bandedge.setLow(sfreq);
				bandedge.setHigh(efreq);
				xvtr.setBandEdge(bandedge);
				BandStack bandstack=new BandStack();
				bandstack.setFilterLow(300);
				bandstack.setFilterHigh(2700);
				bandstack.setMode(Modes.USB);
				bandstack.setFrequency(sfreq+((efreq-sfreq)/2));
				xvtr.setBandStack(0, bandstack);
				
				Log.i("TransverterUpdateActivity",name+" Updated");
				finish();
			}
		} else if(v.getId()==R.id.buttonDelete && event.getAction()==MotionEvent.ACTION_DOWN) {
			Bands bands=configuration.bands;
			bands.delete(xvtr);
			finish();
		}
		return false;
	}
    
	private int index;
	private Transverter xvtr;
}
