package org.g0orx.openhpsdr;

import android.os.Bundle;
import android.app.Activity;
import android.text.Html;
import android.util.Log;
import android.view.Menu;
import android.widget.TextView;

public class AboutActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_about);
		
		String version="Unknown!";
        try {
		    version = this.getPackageManager().getPackageInfo(this.getPackageName(), 0).versionName;
        } catch (Exception e ) {
        	Log.i("AboutActivity","onCreate: "+e.toString());
        }
		
		TextView tv=(TextView)this.findViewById(R.id.textViewAbout);
		tv.setText(Html.fromHtml("<body><p><center><b>HPSDR Radio for Android</b></center>"+
                "<br><br>"+
                "<center><i>by John Melton G0ORX/N6LYT</i></center>"+
                "<br><br>"+
                "<center>Version "+version+"</center>"+
                "<br><br>"+
                "This application uses the FFTW3 library."+
                "<br><br>"+
                "This application uses a ported version of the WDSP library by Warren Pratt, NR0V.</p></body>"));
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		getMenuInflater().inflate(R.menu.about, menu);
		return true;
	}

}
