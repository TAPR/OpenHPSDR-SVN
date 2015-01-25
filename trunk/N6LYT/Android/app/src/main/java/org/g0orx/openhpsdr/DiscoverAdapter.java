package org.g0orx.openhpsdr;

import java.text.DecimalFormat;
import java.util.ArrayList;


import org.g0orx.openhpsdr.discovery.Discovered;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

public class DiscoverAdapter extends ArrayAdapter<Discovered> {

	private ArrayList<Discovered> items;
	private LayoutInflater vi;
	 
	public DiscoverAdapter(Context context,ArrayList<Discovered> items) {
        super(context,0, items);
        //this.context = context;
        this.items = items;
        vi = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

	public View getView(int position, View convertView, ViewGroup parent) {
		//Log.i("DiscoveryAdapter","getView");
        View v = convertView;
        final Discovered d = items.get(position);
        if (d != null) {
        	v = vi.inflate(R.layout.item_discover, null);
        	TextView t=(TextView)v.findViewById(R.id.textViewTitle);
        	t.setText(d.getDeviceName()+" "+d.getAddress()+" ("+d.getMac()+")");
        	TextView s=(TextView)v.findViewById(R.id.textViewSubtitle);
        	s.setText("software version: "+format.format((float)d.getSoftwareversion()/10.0F)+" state: "+d.getStateName());
        }
        return v;
	}

	private DecimalFormat format=new DecimalFormat("##0.0#");
}
