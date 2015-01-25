package org.g0orx.openhpsdr;

import java.util.ArrayList;


import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

public class SettingsAdapter extends ArrayAdapter<Setting> {

	private ArrayList<Setting> items;
	private LayoutInflater vi;
	 
	public SettingsAdapter(Context context,ArrayList<Setting> items) {
        super(context,0, items);
        //this.context = context;
        this.items = items;
        vi = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

	public View getView(int position, View convertView, ViewGroup parent) {
		//Log.i("SettingsAdapter","getView");
        View v = convertView;
        final Setting s = items.get(position);
        if (s != null) {
        	v = vi.inflate(R.layout.item_discover, null);
        	TextView title=(TextView)v.findViewById(R.id.textViewTitle);
        	title.setText(s.getTitle());
        	TextView value=(TextView)v.findViewById(R.id.textViewSubtitle);
        	value.setText(s.getValue());
        }
        return v;
	}

}
