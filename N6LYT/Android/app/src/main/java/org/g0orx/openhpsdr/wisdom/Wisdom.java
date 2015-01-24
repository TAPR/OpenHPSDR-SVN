package org.g0orx.openhpsdr.wisdom;

import android.os.Handler;
import android.os.Message;
import android.util.Log;

import org.g0orx.openhpsdr.WisdomActivity;
import org.g0orx.openhpsdr.wdsp.WDSP;

/**
 * Created by john on 06/11/14.
 */
public class Wisdom extends Thread {

    public Wisdom(Handler handler, String dir, WisdomActivity activity) {
        this.handler = handler;
        this.dir=dir;
        this.activity=activity;
        running = true;
    }

    public void run() {

        WDSP wdsp=WDSP.getInstance();
        wdsp= WDSP.getInstance();
        Log.i("Wisdom", "Create dir: "+dir);
        wdsp.setActivity(activity);
        wdsp.WDSPwisdom(dir);
        Log.i("Wisdom","Completed");

        handler.sendMessage(Message.obtain(handler, END_WISDOM));
    }


    private WisdomActivity activity;

    public static final int END_WISDOM=1024;
    private Handler handler;
    private String dir;
    private boolean running;

}
