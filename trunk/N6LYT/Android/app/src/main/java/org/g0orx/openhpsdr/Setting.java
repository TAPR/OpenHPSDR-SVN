package org.g0orx.openhpsdr;

public class Setting {
	
	public Setting() {
		
	}
	
	public void setTitle(String title) {
		this.title=title;
	}
	
	public void setValue(String value) {
		this.value=value;
	}
	
	public String getTitle() {
		return title;
	}
	
	public String getValue() {
		return value;
	}
	
	private String title;
    private String value;
}
