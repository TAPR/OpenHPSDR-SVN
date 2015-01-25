package org.g0orx.openhpsdr;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.app.Activity;
import android.content.Intent;
import android.graphics.Point;
import android.util.Log;
import android.view.Display;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.io.File;
import java.io.FileInputStream;
import java.io.ObjectInputStream;
import java.util.ArrayList;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.discovery.Discovery;

public class MainActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		
		super.onCreate(savedInstanceState);

		Log.i("MainActivity","onCreate");

		activity=this;

        setContentView(R.layout.activity_main);
        String version="Unknown!";
        try {
            version = this.getPackageManager().getPackageInfo(this.getPackageName(), 0).versionName;
        } catch (Exception e ) {
            Log.i("MainActivity","onCreate: "+e.toString());
        }
		setTitle("openHPSDR (v"+version+")");

        Display display = getWindowManager().getDefaultDisplay();
        Point size=new Point();
        display.getSize(size);
        Log.i("MainActivity","Display: width="+size.x+" height="+size.y);

        textViewStatus = (TextView)this.findViewById(R.id.textViewStatus);

        discoverButton=(Button)this.findViewById(R.id.buttonDiscovery);
        discoverButton.setEnabled(false);
		
		listView=(ListView)this.findViewById(R.id.listViewDiscover);
		adapter=new DiscoverAdapter(this,discoveredlist);
        listView.setAdapter(adapter);
        
        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
        	
        	public void onItemClick(AdapterView parentView, View childView, int position, long id) {
        		Log.i("MainActivity","onItemClick: "+position);
        		
        		if(!discovering) {
        			Configuration configuration=null;

        			// try to load an existing configuration
        			selected=discoveredlist.get(position);
        			
        			if(selected.getState()==Discovered.STATE_AVAILABLE) {
        				String filename=selected.getMac()+".conf";
        				Log.i("MainActivity","onItemClick: load "+filename);
        				try {
        					FileInputStream fis = openFileInput(filename);
        					ObjectInputStream ois = new ObjectInputStream(fis);
        					Configuration.setInstance((Configuration)ois.readObject());
        					configuration=Configuration.getInstance();
        					configuration.discovered=selected;
        					ois.close();
        					fis.close();
        				} catch (Exception e) {
        					Log.i("MainActivity","onItemClick: "+e.toString());
        					configuration=Configuration.getInstance();
        					configuration.discovered=selected;
        					configuration.bands=new Bands();
        				}
        		        
        		        textViewStatus.setText("Starting "+configuration.discovered.getDeviceName()+" ("+configuration.discovered.getAddress()+") ... Please Wait");

        				radioActivity = new Intent(getApplicationContext(), RadioActivity.class);
        			    startActivity(radioActivity);
        			} else {
        		        Toast toast = Toast.makeText(activity, "Device is in use by another application", Toast.LENGTH_LONG);
        		        toast.show();
        			}
        		}
        	}
            
        });
        
        listView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
        	
            public boolean onItemLongClick(AdapterView parentView, View childView, int position, long id) {
                Log.i("MainActivity","onLongItemClick: "+position);
                if(!discovering) {
                	Configuration configuration=null;

                	// try to load an existing configuration
                	selected=discoveredlist.get(position);

                	Log.i("MainActivity","onItemLongClick: "+selected.toString());

                	String filename=selected.getMac()+".conf";
                	Log.i("MainActivity","onItemLongClick: load "+filename);
                	try {
                		FileInputStream fis = openFileInput(filename);
                		if(fis==null) {
                			Log.i("MainActivity","onItemLongClick: fis is null");
                		}
                		ObjectInputStream ois = new ObjectInputStream(fis);
                		if(ois==null) {
                			Log.i("MainActivity","onItemLongClick: ois is null");
                		}
                		Configuration.setInstance((Configuration)ois.readObject());
                		configuration=Configuration.getInstance();
                    	configuration.discovered=selected;
                		ois.close();
                		fis.close();
                	} catch (Exception e) {
                		Log.i("MainActivity","onItemLongClick: "+e.toString());
                		configuration=Configuration.getInstance();
                		configuration.discovered=selected;
                		configuration.bands=new Bands();
                		Configuration.setInstance(configuration);
                	}

                	Intent settingsActivity = new Intent(getApplicationContext(), SettingsActivity.class);
                	startActivity(settingsActivity);
                }
                return true;
            }
            
        });
        
        handler = new Handler() {
            @Override
            public void handleMessage (Message msg) {
                if(msg.what==Discovery.ADD_DEVICE) {
                    discoveredlist.add((Discovered)msg.obj);
                    adapter.notifyDataSetChanged();
                } else if(msg.what==Discovery.END_DISCOVERY) {
                    if(discoveredlist.isEmpty()) {
                        textViewStatus.setText("No devices found");
                    } else {
                        textViewStatus.setText("Tap device to start, hold to configure");
                    }
                    discoverButton.setEnabled(true);
                    discovering=false;
                }
            }
        };

        // check/create wisdom files

        if(wisdom()) {
            TextView wisdomTextView=(TextView) this.findViewById(R.id.textViewWisdom);
            wisdomTextView.setVisibility(View.GONE);
            Button wisdomButton=(Button) this.findViewById(R.id.buttonWisdom);
            wisdomButton.setVisibility(View.GONE);
        }
	}
	
	@Override
	public void onRestart() {
		Log.i("MainActivity","onRestart");
		super.onRestart();
	}
	
	@Override
	public void onStart() {
		Log.i("MainActivity","onStart");
		super.onStart();
	}
	
	@Override
	public void onResume() {
		Log.i("MainActivity","onResume");
		super.onResume();
        if(wisdom()) {
            TextView wisdomTextView=(TextView) this.findViewById(R.id.textViewWisdom);
            if(wisdomTextView!=null){
                wisdomTextView.setVisibility(View.GONE);
            }
            Button wisdomButton=(Button) this.findViewById(R.id.buttonWisdom);
            if(wisdomButton!=null) {
                wisdomButton.setVisibility(View.GONE);
            }
        }
        discover();
	}

	@Override
	public void onPause() {
		Log.i("MainActivity","onPause");
		super.onPause();
	}
	
	@Override
	public void onDestroy() {
		Log.i("MainActivity","onDestroy");
		super.onDestroy();
	}
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}
	
	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
	    // Handle item selection
	    switch (item.getItemId()) {
	        case R.id.action_about:
	        	Intent aboutActivity = new Intent(getApplicationContext(), AboutActivity.class);
	            startActivity(aboutActivity);
	            break;
	        case R.id.action_help:
	        	Intent helpActivity = new Intent(getApplicationContext(), HelpActivity.class);
	            startActivity(helpActivity);
	            break;
	    }
	    return true;
	}
	
	private void discover() {
        Log.i("MainActivity","discover");
		String text = "Discovering ... Please Wait";
 
        textViewStatus.setText(text);
        textViewStatus.invalidate();

        Toast toast = Toast.makeText(this, text, Toast.LENGTH_SHORT);
        toast.show();
        
		discoveredlist.clear();
		adapter.notifyDataSetChanged();

		Discovery discovery=new Discovery(handler);
		discovery.start();
		
		discovering=true;
	}

    private boolean wisdom() {
        boolean result=true;
        Log.i("MainActivity","Checking wisdom files");
        String dir=getFilesDir().getParentFile().getPath()+"/";
        File f=new File(dir+"wdspWisdom");
        if(!f.exists()) {
            Log.i("MainActivity",dir+"wdspWisdom does not exist");
            result=false;
        } else {
            Log.i("MainActivity",dir+"wdspWisdom does exist");
        }
        return result;
    }
	
	public void onDiscoveryClick(View v) {
		if(!discovering) {
		    discover();
		}
    }

    public void onWisdomClick(View v) {
        if(!discovering) {
            Intent wisdomActivity = new Intent(getApplicationContext(), WisdomActivity.class);
            startActivity(wisdomActivity);
        }
    }
	
	private Activity activity;

    //private boolean checkingWisdom=false;
	private Handler handler;

	private ArrayList<Discovered> discoveredlist=new ArrayList<Discovered>();
	private Discovered selected;
	
	private ListView listView;
	private TextView textViewStatus;
    private Button discoverButton;
	
	private ArrayAdapter<Discovered> adapter;
	private Intent radioActivity;
	
	private boolean discovering=false;


}
