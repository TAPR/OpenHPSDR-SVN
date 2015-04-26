package org.g0orx.openhpsdr;

import java.util.ArrayList;

/**
 * Created by john on 07/02/15.
 */
public class Step {

    public static int getStep(int x) {
        if(x>=0 && x<steps.length) {
            return steps[x];
        }
        return 100;
    }

    public static int length() {
        return steps.length;
    }

    private static int[] steps = {10,50,100,500,1000};

}
