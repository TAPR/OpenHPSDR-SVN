package org.g0orx.openhpsdr;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.view.Display;
import android.view.Menu;
import android.view.MenuItem;


public class BandscopeActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_band_scope);


    }

    @Override
    public void onStart() {
        //Log.i("RadioActivity","onStart");
        super.onStart();

        Display display = getWindowManager().getDefaultDisplay();
        final int width = (display.getWidth());
        final int height = (display.getHeight());


        Log.i("BandscopeActivity", "onStart: width=" + width + " height=" + height);

        metis = new Metis(width, false);
        metis.start();

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_band_scope, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    private Metis metis;
}
