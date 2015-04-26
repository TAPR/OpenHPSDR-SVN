package org.g0orx.openhpsdr;


/**
 * Created by john on 28/01/15.
 */
public class Mode {

    public Mode(String name,int id,Filter[] filters) {
        this.name=name;
        this.id=id;
        this.filters=filters;
    }

    public String getName() {
        return name;
    }
    
    public int getId() {
        return id;
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
    
    public int getFilter(String name) {
        for(int i=0;i<filters.length;i++) {
            if(filters[i].getName().equals(name)) {
                return i;
            }
        }
        return -1;
    }

    private String name;
    private int id;
    private Filter[] filters;
}
