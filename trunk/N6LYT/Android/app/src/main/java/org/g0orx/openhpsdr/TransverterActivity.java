package org.g0orx.openhpsdr;

import java.util.ArrayList;

import android.os.Bundle;
import android.app.Activity;
import android.content.Intent;
import android.util.Log;
import android.view.Menu;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

public class TransverterActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        configuration=Configuration.getInstance();
        
        setTitle("openHPSDR XVTR Configuration ("+configuration.discovered.getDeviceName()+": IP="+configuration.discovered.getAddress()+" MAC="+configuration.discovered.getMac()+")");

        setContentView(R.layout.activity_transverter);
        
        Button addButton = (Button)findViewById(R.id.buttonAddTransverter);
		addButton.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if(v.getId()==R.id.buttonAddTransverter) {
		    		if (event.getAction() == MotionEvent.ACTION_DOWN) {
		    			Intent transverterAddActivity = new Intent(getApplicationContext(), TransverterAddActivity.class);
		    			startActivity(transverterAddActivity);
		    		}
		    	}
				return false;
			}
		});
    		
        Setting s;
        final Band[] bands=configuration.bands.getBands();
        for(int i=0;i<bands.length;i++) {
        	if(bands[i] instanceof Transverter) {
        		Transverter xvtr=(Transverter)bands[i];
		        s=new Setting();
		        s.setTitle(xvtr.getName());
		        s.setValue(Long.toString(xvtr.getBandEdge().getLow())+" to "+Long.toString(xvtr.getBandEdge().getHigh())+" Osc="+Long.toString(xvtr.getIfFrequency()));
		        settings.add(s);
        	}
        }
        
        listView=(ListView)this.findViewById(R.id.listViewTransverters);
		adapter=new SettingsAdapter(this,settings);
        listView.setAdapter(adapter);
        listView.setOnItemClickListener(new OnItemClickListener() {
			@Override
			public void onItemClick(AdapterView<?> arg0, View view, int position, long id) {
				Log.i("TransverterActivity","selected "+position);
				int i;
                final Band[] bands=configuration.bands.getBands();
				for(i=0;i<bands.length;i++) {
					if(bands[i] instanceof Transverter) {
						Transverter xvtr=(Transverter)bands[i+position];
					    Log.i("TransverterActivity","selected "+xvtr.getName());
					    Intent transverterUpdateActivity = new Intent(getApplicationContext(), TransverterUpdateActivity.class);
					    Bundle b = new Bundle();
					    b.putInt("index", i+position);
					    transverterUpdateActivity.putExtras(b);
					    startActivity(transverterUpdateActivity);
					    break;
					}
				}
				    

			}
        });
		
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.transverter, menu);
        return true;
    }
    
    protected void onRestart() {
		super.onRestart();
		Log.i("TransverterActivity","onRestart");
		Setting s;
		settings.clear();
		Band[] bands=configuration.bands.getBands();
        for(int i=0;i<bands.length;i++) {
        	if(bands[i] instanceof Transverter) {
        		Transverter xvtr=(Transverter)bands[i];
		        s=new Setting();
		        s.setTitle(xvtr.getName());
		        s.setValue(Long.toString(xvtr.getBandEdge().getLow())+" to "+Long.toString(xvtr.getBandEdge().getHigh())+" Osc="+Long.toString(xvtr.getIfFrequency()));
		        settings.add(s);
        	}
        }
		adapter.notifyDataSetChanged();
	}
    
    private Configuration configuration;
    
    private ListView listView;
	private ArrayAdapter<Setting> adapter;
    private ArrayList<Setting> settings=new ArrayList<Setting>();
}
