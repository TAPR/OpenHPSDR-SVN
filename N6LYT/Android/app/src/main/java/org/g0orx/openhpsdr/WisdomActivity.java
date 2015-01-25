package org.g0orx.openhpsdr;

import android.app.Activity;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import org.g0orx.openhpsdr.wdsp.WDSP;
import org.g0orx.openhpsdr.wisdom.Wisdom;




public class WisdomActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_wisdom);

        handler = new Handler() {
            @Override
            public void handleMessage (Message msg) {
                if(msg.what==Wisdom.END_WISDOM) {
                    finish();
                }
            }
        };

        statusTextView=(TextView)findViewById(R.id.textViewStatus);

        Button buttonCreate=(Button)findViewById(R.id.buttonCreate);
        buttonCreate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                v.setEnabled(false);
                creating=true;
                String dir=getFilesDir().getParentFile().getPath()+"/";
                Log.i("WisdomActivity","Creating Wisdom");
                Wisdom wisdom = new Wisdom(handler, dir, activity);
                wisdom.start();
            }
        });

        activity=this;
    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_wisdom, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onBackPressed() {
        Log.i("WisdomActivity", "onbackPressed");
        if(!creating) {
            super.onBackPressed();
        }
    }

    public void updateStatus(String status) {
        Log.i("WisdomActivity","updateStatus:"+status);
        final String s=status;
        Thread t = new Thread() {
            public void run() {
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        Log.i("UpdateStatus","run:"+s);
                        statusTextView.setText(s);
                        statusTextView.invalidate();
                    }
                });
            }
        };
        t.start();
    }

    private WisdomActivity activity;

    private boolean creating=false;
    private Handler handler;

    private TextView statusTextView;

}
