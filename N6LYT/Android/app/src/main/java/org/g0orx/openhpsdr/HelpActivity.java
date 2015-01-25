package org.g0orx.openhpsdr;

import android.os.Bundle;
import android.app.Activity;
import android.text.Html;
import android.view.Menu;
import android.webkit.WebView;
import android.widget.TextView;

public class HelpActivity extends Activity {

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_help);

        /*
        String help="<blockquote>"+
                    "<b>FFTW Wisdom:</b><br><br>"+
                    "When the application is run for the first time the FFTW Wisdom files will be created.  This can take a couple of minutes to complete."+
                    "<br><br>"+
                    "<b>Discovery:</b><br><br>"+
                    "Each time the application is started a discovery process is run to find the devices on the network.  They are listed showing their IP address, MAC address, Device type, Software version and their state.  Click on the <b>Discovery</b> button to re-run the discovery process."+
                    "<br><br>"+
                    "<b>Device Configuration:</b><br><br>"+
                    "To configure a device tap and hold the device in the discovery list."+
                    "<br><br>"+
                    "If the device is a Metis you need to configure if the exciter is Penelope or PennyLane and if Alex is present. For other devices you can configure which radio it is."+
                    "<br>"+
                    "Your can configure the sample rate, dither, random, preamp, clock sources, microphone source, audio output, if the waterfall is displayed and Transverters."+
                    "<br><br>"+
                    "By default the audio is sent back to the Radio device and to the Android device. You can disable the audio in Device Configuration."+
                    "<br><br>"+
                    "By default the microphone audio is taken from the Radio device. You can configure the microphone source in Device Configuration."+
                    "<br><br>"+
                    "When adding a transverter set the start and end frequency in Hz and set the Oscillator frequency in Hz. For example with a 2 Mtr Transverter with the IF at 28MHz set the Oscillator to 116000000Hz."+
                    "<br><br>"+
                    "<b>Running</b><br><br>"+
                    "To run the radio tap on the device in the Discovery list."+
                    "<br><br>"+
                    "To change frequency drag left/right on the panadapter or waterfall display, tap (and hold for repeat) the +/- 1000/500/100/100 buttons. The <b>Lock</b> button will lock the current frequency."+
                    "<br><br>"+
                    "To change bands select the band from the dropdown."+
                    "<br><br>"+
                    "Tap on the <b>Configure</b> button to set the Mode and Antenna configuration for the current Band, and to set the transmit TX Levels and Audio levels."+
                    "<br><br>"+
                    "</blockquote>";

		
		TextView tv=(TextView)this.findViewById(R.id.textViewHelp);
		tv.setText(Html.fromHtml(help));
		*/

        String url="http://g0orx.blogspot.co.uk/2015/01/openhpsdr-android-application.html";
        setTitle("openHPSDR Help: "+url);
        WebView wv=(WebView)this.findViewById(R.id.webViewHelp);
        wv.loadUrl(url);
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.about, menu);
		return true;
	}

}
