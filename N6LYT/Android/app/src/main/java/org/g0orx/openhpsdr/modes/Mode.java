package org.g0orx.openhpsdr.modes;

import org.g0orx.openhpsdr.Filter;

/**
 * Created by john on 28/01/15.
 */
public class Mode {

    public Mode(String name,Filter[] filters) {
        this.name=name;
        this.filters=filters;
    }

    public String getName() {
        return name;
    }

    public int length() {
        return filters.length;
    }

    public Filter getFilter(int i) {
        if(i<filters.length) {
            return filters[i];
        }
        return null;
    }

    private String name;
    private Filter[] filters;
}
