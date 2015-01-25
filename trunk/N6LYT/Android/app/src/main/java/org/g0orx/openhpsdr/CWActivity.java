package org.g0orx.openhpsdr;

import org.g0orx.openhpsdr.util.SystemUiHider;

import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.RadioButton;
import android.widget.Switch;

public class CWActivity extends Activity {


    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_cw);

        configuration=Configuration.getInstance();

        setTitle("openHPSDR Configure CW ("+configuration.discovered.getDeviceName()+": IP="+configuration.discovered.getAddress()+" MAC="+configuration.discovered.getMac()+")");

        Switch internal=(Switch)findViewById(R.id.switchInternal);
        if(configuration.cwinternal!=1) configuration.cwinternal=1;
        internal.setChecked(true);
        internal.setEnabled(false);
        //internal.setChecked(configuration.cwinternal!=0);

        RadioButton rb=null;
        switch(configuration.cwkeyermode) {
            case 0: // Straight
                rb=(RadioButton)findViewById(R.id.radioButtonStraight);
                break;
            case 1: // Mode A
                rb=(RadioButton)findViewById(R.id.radioButtonModeA);
                break;
            case 2: // Mode B
                rb=(RadioButton)findViewById(R.id.radioButtonModeB);
                break;
        }
        if(rb!=null) rb.setChecked(true);

        NumberPicker speed=(NumberPicker)findViewById(R.id.numberPickerSpeed);
        speed.setMinimum(1);
        speed.setMaximum(60);
        if(configuration.cwkeyerspeed<1 || configuration.cwkeyerspeed>60) configuration.cwkeyerspeed=20;
        speed.setValue(configuration.cwkeyerspeed);

        CheckBox rev=(CheckBox)findViewById(R.id.checkBoxKeysReversed);
        rev.setChecked(configuration.cwkeysreversed!=0);

        NumberPicker weight=(NumberPicker)findViewById(R.id.numberPickerWeight);
        weight.setMinimum(1);
        weight.setMaximum(100);
        if(configuration.cwkeyerweight<1 || configuration.cwkeyerweight>100) configuration.cwkeyerweight=30;
        weight.setValue(configuration.cwkeyerweight);

        NumberPicker pttdelay=(NumberPicker)findViewById(R.id.numberPickerPTTDelay);
        pttdelay.setMinimum(0);
        pttdelay.setMaximum(255);
        if(configuration.cwpttdelay<0 || configuration.cwpttdelay>255) configuration.cwpttdelay=20;
        pttdelay.setValue(configuration.cwpttdelay);

        NumberPicker hangtime=(NumberPicker)findViewById(R.id.numberPickerHangTime);
        hangtime.setMinimum(0);
        hangtime.setMaximum(1023);
        if(configuration.cwhangtime<0 || configuration.cwhangtime>1023) configuration.cwhangtime=10;
        hangtime.setValue(configuration.cwhangtime);

        CheckBox spacing=(CheckBox)findViewById(R.id.checkBoxSpacing);
        spacing.setChecked(configuration.cwkeyerspacing!=0);

        NumberPicker volume=(NumberPicker)findViewById(R.id.numberPickerSidetoneVolume);
        volume.setMinimum(0);
        volume.setMaximum(127);
        if(configuration.cwsidetonevolume<0 || configuration.cwsidetonevolume>127) configuration.cwsidetonevolume=127;
        volume.setValue(configuration.cwsidetonevolume);

        NumberPicker frequency=(NumberPicker)findViewById(R.id.numberPickerSidetoneFrequency);
        frequency.setMinimum(0);
        frequency.setMaximum(4095);
        if(configuration.cwsidetonefrequency<0 || configuration.cwsidetonefrequency>4095) configuration.cwsidetonefrequency=450;
        frequency.setValue(configuration.cwsidetonefrequency);


    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.cw, menu);
        return true;
    }

    protected void onRestart() {
        super.onRestart();
        Log.i("CWActivity","onRestart");

    }

    public void onRadioButtonModeClicked(View view) {
        // Is the button now checked?
        boolean checked = ((RadioButton) view).isChecked();

        Log.i("CWActivity","onRadioButtonModeClicked: "+((RadioButton) view).getText());

        // Check which radio button was clicked
        switch(view.getId()) {
            case R.id.radioButtonStraight:
                configuration.cwkeyermode=0;
                break;
            case R.id.radioButtonModeA:
                configuration.cwkeyermode=1;
                break;
            case R.id.radioButtonModeB:
                configuration.cwkeyermode=2;
                break;
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();

        Switch internal=(Switch)findViewById(R.id.switchInternal);
        configuration.cwinternal=internal.isChecked()?1:0;

        RadioButton rb=(RadioButton)findViewById(R.id.radioButtonStraight);
        if(rb.isChecked()) {
            configuration.cwkeyermode=Configuration.KEYER_STRAIGHT;
        }
        rb=(RadioButton)findViewById(R.id.radioButtonModeA);
        if(rb.isChecked()) {
            configuration.cwkeyermode=Configuration.KEYER_MODE_A;
        }
        rb=(RadioButton)findViewById(R.id.radioButtonModeB);
        if(rb.isChecked()) {
            configuration.cwkeyermode=Configuration.KEYER_MODE_B;
        }

        CheckBox rev=(CheckBox)findViewById(R.id.checkBoxKeysReversed);
        configuration.cwkeysreversed=rev.isChecked()?1:0;

        NumberPicker weight=(NumberPicker)findViewById(R.id.numberPickerWeight);
        configuration.cwkeyerweight=weight.getValue();

        NumberPicker pttdelay=(NumberPicker)findViewById(R.id.numberPickerPTTDelay);
        configuration.cwpttdelay=pttdelay.getValue();

        NumberPicker hangtime=(NumberPicker)findViewById(R.id.numberPickerHangTime);
        configuration.cwhangtime=hangtime.getValue();

        CheckBox spacing=(CheckBox)findViewById(R.id.checkBoxSpacing);
        configuration.cwkeyerspacing=spacing.isChecked()?1:0;

        NumberPicker volume=(NumberPicker)findViewById(R.id.numberPickerSidetoneVolume);
        configuration.cwsidetonevolume=volume.getValue();

        NumberPicker frequency=(NumberPicker)findViewById(R.id.numberPickerSidetoneFrequency);
        configuration.cwsidetonefrequency=frequency.getValue();
    }

    private Configuration configuration;

}
