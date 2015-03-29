package org.g0orx.openhpsdr;

/**
 * Created by john on 28/01/15.
 */
public class Frequency {

    public static String toString(long f) {
        return String.format("%d.%03d.%03d",
                f / 1000000,
                (f % 1000000) / 1000,
                (f % 1000));
    }
}

