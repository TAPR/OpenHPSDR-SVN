package org.g0orx.openhpsdr;


import java.io.FileOutputStream;
import java.io.ObjectOutputStream;
import java.text.DecimalFormat;
import java.util.ArrayList;

import org.g0orx.openhpsdr.discovery.Discovered;


import android.os.Bundle;
import android.app.Activity;
import android.app.ActivityManager;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.ConfigurationInfo;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

public class SettingsActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        setContentView(R.layout.activity_settings);

        context = this;

        configuration = Configuration.getInstance();

        setTitle("openHPSDR Configure (" + configuration.discovered.getDeviceName() + ": IP=" + configuration.discovered.getAddress() + " MAC=" + configuration.discovered.getMac() + ")");

        Setting s;

        // create the list
        switch (configuration.discovered.getDevice()) {
            case Discovered.DEVICE_METIS:
                s = new Setting();

                s.setTitle("Radio");
                if (configuration.radio == Configuration.RADIO_UNKNOWN) {
                    configuration.radio = Configuration.METIS_PENELOPE;
                }
                switch (configuration.radio) {
                    case Configuration.METIS_PENELOPE:
                        s.setValue("Metis and Penelope only");
                        break;
                    case Configuration.METIS_PENNYLANE:
                        s.setValue("Metis and Pennylane only");
                        break;
                    case Configuration.METIS_PENELOPE_ALEX:
                        s.setValue("Metis and Penelope with Alex");
                        break;
                    case Configuration.METIS_PENNYLANE_ALEX:
                        s.setValue("Metis and Pennylane with Alex");
                        break;
                }
                settings.add(s);



                s = new Setting();
                s.setTitle("10 MHz Clock Source");
                if (configuration.clock10 == Metis.ATLAS_10MHZ_SOURCE) {
                    s.setValue("Atlas");
                } else if (configuration.clock10 == Metis.MERCURY_10MHZ_SOURCE) {
                    s.setValue("Mercury");
                } else if (configuration.clock10 == Metis.PENELOPE_10MHZ_SOURCE) {
                    s.setValue("Penelope/Pennylane");
                }
                settings.add(s);

                s = new Setting();
                s.setTitle("122.88 MHz Clock Source");
                if (configuration.clock122 == Metis.MERCURY_122_88MHZ_SOURCE) {
                    s.setValue("Mercury");
                } else if (configuration.clock122 == Metis.PENELOPE_122_88MHZ_SOURCE) {
                    s.setValue("Penelope/Pennylane");
                }
                settings.add(s);
                break;

            case Discovered.DEVICE_HERMES:
                s = new Setting();
                s.setTitle("Radio");
                if (configuration.radio == Configuration.RADIO_UNKNOWN) {
                    configuration.radio = Configuration.HERMES_BOARD_ONLY;
                }

                switch (configuration.radio) {
                    case Configuration.HERMES_BOARD_ONLY:
                        s.setValue("Hermes board only");
                        break;
                    case Configuration.HERMES_ALEX:
                        s.setValue("Hermes and Alex");
                        break;
                    case Configuration.HERMES_APOLLO:
                        s.setValue("Hermes and Apollo");
                        break;
                    case Configuration.HERMES_ANAN10:
                        s.setValue("ANAN-10");
                        break;
                    case Configuration.HERMES_ANAN100:
                        s.setValue("ANAN-100");
                        break;
                }
                settings.add(s);
                break;

            case Discovered.DEVICE_ANGELIA:
                s = new Setting();
                s.setTitle("Radio");
                if (configuration.radio == Configuration.RADIO_UNKNOWN) {
                    configuration.radio = Configuration.ANGELIA_BOARD_ONLY;
                }

                switch (configuration.radio) {
                    case Configuration.ANGELIA_BOARD_ONLY:
                        s.setValue("Angelia board only");
                        break;
                    case Configuration.ANGELIA_ANAN100D:
                        s.setValue("ANAN-100D");
                        break;
                }
                settings.add(s);
                break;
            case Discovered.DEVICE_ORION:
                s = new Setting();
                s.setTitle("Radio");
                if (configuration.radio == Configuration.RADIO_UNKNOWN) {
                    configuration.radio = Configuration.ORION_BOARD_ONLY;
                }

                switch (configuration.radio) {
                    case Configuration.ORION_BOARD_ONLY:
                        s.setValue("Orion board only");
                        break;
                    case Configuration.ORION_ANAN200D:
                        s.setValue("ANAN-200D");
                        break;
                }
                settings.add(s);

                s = new Setting();
                s.setTitle("Orion Mic Tip/Ring");
                if (configuration.oriontipring == Configuration.MIC_PTT_TO_RING_BIAS_TO_TIP) {
                    s.setValue("PTT to ring, Mic/Bias to tip");
                } else if (configuration.oriontipring == Configuration.MIC_PTT_TO_TIP_BIAS_TO_RING) {
                    s.setValue("PTT to tip, Mic/Bias to ring");
                }
                settings.add(s);

                s = new Setting();
                s.setTitle("Orion Mic Bias");
                if (configuration.orionmicbias == Configuration.MIC_BIAS_DISABLE) {
                    s.setValue("Disabled");
                } else if (configuration.orionmicbias == Configuration.MIC_BIAS_ENABLE) {
                    s.setValue("Enabled");
                }
                settings.add(s);

                s = new Setting();
                s.setTitle("Orion Mic PTT");
                if (configuration.orionmicptt == Configuration.MIC_PTT_DISABLE) {
                    s.setValue("Disabled");
                } else if (configuration.orionmicptt == Configuration.MIC_PTT_ENABLE) {
                    s.setValue("Enabled");
                }
                settings.add(s);
                break;
        }

        s = new Setting();
        s.setTitle("CW Configuration");
        s.setValue("");
        settings.add(s);

        s = new Setting();
        s.setTitle("Transverter Configuration");
        s.setValue("");
        settings.add(s);

        s = new Setting();
        s.setTitle("Sample Rate");
        s.setValue(Integer.toString((int) configuration.samplerate));
        settings.add(s);

        s = new Setting();
        s.setTitle("LT2208 Dither");
        s.setValue(configuration.dither == Metis.LT2208_DITHER_ON ? "On" : "Off");
        settings.add(s);

        s = new Setting();
        s.setTitle("LT2208 Random");
        s.setValue(configuration.random == Metis.LT2208_RANDOM_ON ? "On" : "Off");
        settings.add(s);

        s = new Setting();
        s.setTitle("LT2208 Preamp");
        s.setValue(configuration.preamp == Metis.LT2208_GAIN_ON ? "On" : "Off");
        settings.add(s);


        s = new Setting();
        s.setTitle("Microphone Source");
        if (configuration.micsource == Configuration.MIC_SOURCE_RADIO) {
            s.setValue("Radio");
        } else if (configuration.micsource == Configuration.MIC_SOURCE_LOCAL) {
            s.setValue("Local");
        }
        settings.add(s);

        s = new Setting();
        s.setTitle("Microphone Boost");
        if (configuration.micboost) {
            s.setValue("20dB");
        } else {
            s.setValue("0dB");
        }
        settings.add(s);

        s = new Setting();
        s.setTitle("Audio Output");
        if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_RADIO) {
            s.setValue("Radio");
        } else if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_LOCAL) {
            s.setValue("Local");
        } else if (configuration.audiooutput == Configuration.AUDIO_OUTPUT_BOTH) {
            s.setValue("Both");
        }
        settings.add(s);

        s = new Setting();
        s.setTitle("Spectrum");
        s.setValue(configuration.spectrum ? "On" : "Off");
        settings.add(s);

        s = new Setting();
        s.setTitle("Waterfall");
        s.setValue(configuration.waterfall ? "On" : "Off");
        settings.add(s);

        s = new Setting();
        s.setTitle("Waterfall Display");
        s.setValue(configuration.waterfallGrayscale ? "Gray Scale" : "Color");
        settings.add(s);

        s = new Setting();
        s.setTitle("Waterfall Automatic Level");
        s.setValue(configuration.waterfallAutomatic ? "Enabled" : "Disabled");
        settings.add(s);

        s = new Setting();
        s.setTitle("FPS");
        s.setValue(Integer.toString(configuration.fps));
        settings.add(s);


        s = new Setting();
        s.setTitle("Allow Out Of Band TX");
        s.setValue(configuration.allowoutofband ? "Yes" : "No");
        settings.add(s);

        s = new Setting();
        s.setTitle("Tuna Knob (See http://www.tunadjgear.com/)");
        s.setValue(configuration.tunaknob ? "Yes" : "No");
        settings.add(s);

        /*
        s=new Setting();
        s.setTitle("Meter Image");
        switch(configuration.meter) {
            case Configuration.METER_PNG:
                s.setValue("meter.png");
                break;
            case Configuration.SMETER_PNG:
                s.setValue("smeter.png");
                break;
        }
        settings.add(s);
        */

        /*
        s=new Setting();
		s.setTitle("OpenGL");
		s.setValue(configuration.opengl?"Yes":"No");
		settings.add(s);
	    */


        listView = (ListView) this.findViewById(R.id.listViewSettings);
        adapter = new SettingsAdapter(this, settings);
        listView.setAdapter(adapter);


        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {

            public void onItemClick(AdapterView parentView, View childView, int position, long id) {
                Log.i("MainActivity", "onItemClick: " + position);

                final int index = position;
                setting = settings.get(index);
                String option = setting.getTitle();

                if ("Radio".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Radio";
                    String[] options = {};
                    switch (configuration.discovered.getDevice()) {
                        case Discovered.DEVICE_METIS:
                            options = new String[4];
                            options[0] = "Metis and Penelope only";
                            options[1] = "Metis and Pennylane only";
                            options[2] = "Metis and Penelope with Alex";
                            options[3] = "Metis and Pennylane with Alex";
                            break;
                        case Discovered.DEVICE_HERMES:
                            options = new String[5];
                            options[0] = "Hermes board only";
                            options[1] = "Hermes and Alex";
                            options[2] = "Hermes and Apollo";
                            options[3] = "ANAN-10";
                            options[4] = "ANAN-100";
                            break;
                        case Discovered.DEVICE_ANGELIA:
                            options = new String[2];
                            options[0] = "Angelia board only";
                            options[1] = "ANAN-100D";
                            break;
                        case Discovered.DEVICE_ORION:
                            options = new String[2];
                            options[0] = "Orion board only";
                            options[1] = "ANAN-200D";
                            break;
                    }

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                                                configuration.radio = Configuration.METIS_PENELOPE;
                                                setting.setValue("Metis and Penelope only");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                                                configuration.radio = Configuration.HERMES_BOARD_ONLY;
                                                setting.setValue("Hermes board only");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ANGELIA) {
                                                configuration.radio = Configuration.ANGELIA_BOARD_ONLY;
                                                setting.setValue("Angelia board only");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ORION) {
                                                configuration.radio = Configuration.ORION_BOARD_ONLY;
                                                setting.setValue("Orion board only");
                                            }
                                            break;
                                        case 1:
                                            if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                                                configuration.radio = Configuration.METIS_PENNYLANE;
                                                setting.setValue("Metis and Pennylane only");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                                                configuration.radio = Configuration.HERMES_ALEX;
                                                setting.setValue("Hermes and Alex");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ANGELIA) {
                                                configuration.radio = Configuration.ANGELIA_ANAN100D;
                                                setting.setValue("ANAN-100D");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_ORION) {
                                                configuration.radio = Configuration.ORION_ANAN200D;
                                                setting.setValue("ANAN-200D");
                                            }
                                            break;
                                        case 2:
                                            if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                                                configuration.radio = Configuration.METIS_PENELOPE_ALEX;
                                                setting.setValue("Metis and Penelope with Alex");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                                                configuration.radio = Configuration.HERMES_APOLLO;
                                                setting.setValue("Hermes and Apollo");
                                            }
                                            break;
                                        case 3:
                                            if (configuration.discovered.getDevice() == Discovered.DEVICE_METIS) {
                                                configuration.radio = Configuration.METIS_PENNYLANE_ALEX;
                                                setting.setValue("Metis and Pennylane with Alex");
                                            } else if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                                                configuration.radio = Configuration.HERMES_ANAN10;
                                                setting.setValue("ANAN-10");
                                            }
                                            break;
                                        case 4:
                                            if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES) {
                                                configuration.radio = Configuration.HERMES_ANAN100;
                                                setting.setValue("ANAN-100");
                                            }
                                            break;
                                    }

                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Sample Rate".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Sample Rate";
                    String[] options = {"48000", "96000", "192000", "384000"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.samplerate = 48000;
                                            break;
                                        case 1:
                                            configuration.samplerate = 96000;
                                            break;
                                        case 2:
                                            configuration.samplerate = 192000;
                                            break;
                                        case 3:
                                            configuration.samplerate = 384000;
                                            break;
                                    }
                                    setting.setValue(Integer.toString((int) configuration.samplerate));
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("LT2208 Dither".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "LT2208 Dither";
                    String[] options = {"Off", "On"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.dither = Metis.LT2208_DITHER_OFF;
                                            break;
                                        case 1:
                                            configuration.dither = Metis.LT2208_DITHER_ON;
                                            break;
                                    }
                                    setting.setValue(configuration.dither == Metis.LT2208_DITHER_OFF ? "Off" : "On");
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("LT2208 Random".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "LT2208 Random";
                    String[] options = {"Off", "On"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.random = Metis.LT2208_RANDOM_OFF;
                                            break;
                                        case 1:
                                            configuration.random = Metis.LT2208_RANDOM_ON;
                                            break;
                                    }
                                    setting.setValue(configuration.random == Metis.LT2208_RANDOM_OFF ? "Off" : "On");
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("LT2208 Preamp".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "LT2208 Preamp";
                    String[] options = {"Off", "On"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.preamp = Metis.LT2208_GAIN_OFF;
                                            configuration.preampOffset = 0.0F;
                                            break;
                                        case 1:
                                            configuration.preamp = Metis.LT2208_GAIN_ON;
                                            configuration.preampOffset = -20.0F;
                                            break;
                                    }
                                    setting.setValue(configuration.preamp == Metis.LT2208_GAIN_OFF ? "Off" : "On");
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("10 MHz Clock Source".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "10 MHz Clock Source";
                    String[] options = {"Atlas", "Mercury", "Penelope/Pennylane"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.clock10 = Metis.ATLAS_10MHZ_SOURCE;
                                            value = "Atlas";
                                            break;
                                        case 1:
                                            configuration.clock10 = Metis.MERCURY_10MHZ_SOURCE;
                                            value = "Mercury";
                                            break;
                                        case 2:
                                            configuration.clock10 = Metis.PENELOPE_10MHZ_SOURCE;
                                            value = "Penelope/Pennylane";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("122.88 MHz Clock Source".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "122.88 MHz Clock Source";
                    String[] options = {"Mercury", "Penelope/Pennylane"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.clock122 = Metis.MERCURY_122_88MHZ_SOURCE;
                                            value = "Mercury";
                                            break;
                                        case 1:
                                            configuration.clock122 = Metis.PENELOPE_122_88MHZ_SOURCE;
                                            value = "Penelope/Pennylane";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();

                } else if ("Orion Mic Tip/Ring".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Orion Mic Tip/Ring";
                    String[] options = {"PTT to ring, Mic/Bias to tip", "PTT to tip, Mic/Bias to ring"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    switch (i) {
                                        case 0:
                                            configuration.oriontipring = Configuration.MIC_PTT_TO_RING_BIAS_TO_TIP;
                                            value = "PTT to ring, Mic/Bias to tip";
                                            break;
                                        case 1:
                                            configuration.oriontipring = Configuration.MIC_PTT_TO_TIP_BIAS_TO_RING;
                                            value = "PTT to tip, Mic/Bias to ring";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Orion Mic Bias".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Orion Mic Bias";
                    String[] options = {"Disabled", "Enabled"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    switch (i) {
                                        case 0:
                                            configuration.orionmicbias = Configuration.MIC_BIAS_DISABLE;
                                            value = "Disabled";
                                            break;
                                        case 1:
                                            configuration.orionmicbias = Configuration.MIC_BIAS_ENABLE;
                                            value = "Enabled";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Orion Mic PTT".equals(option)) {

                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Orion Mic PTT";
                    String[] options = {"Disabled", "Enabled"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    switch (i) {
                                        case 0:
                                            configuration.orionmicptt = Configuration.MIC_PTT_DISABLE;
                                            value = "Disabled";
                                            break;
                                        case 1:
                                            configuration.orionmicptt = Configuration.MIC_PTT_ENABLE;
                                            value = "Enabled";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Microphone Source".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Microphone Source";
                    String[] options = {"Radio", "Local"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.micsource = Configuration.MIC_SOURCE_RADIO;
                                            value = "Radio";
                                            break;
                                        case 1:
                                            configuration.micsource = Configuration.MIC_SOURCE_LOCAL;
                                            value = "Local";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Microphone Boost".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Microphone Boost";
                    String[] options = {"0dB", "20dB"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.micboost = false;
                                            value = "0dB";
                                            break;
                                        case 1:
                                            configuration.micboost = true;
                                            value = "20dB";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Audio Output".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Audio Output";
                    String[] options = {"Radio", "Local", "Both"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.audiooutput = Configuration.AUDIO_OUTPUT_RADIO;
                                            value = "Radio";
                                            break;
                                        case 1:
                                            configuration.audiooutput = Configuration.AUDIO_OUTPUT_LOCAL;
                                            value = "Local";
                                            break;
                                        case 2:
                                            configuration.audiooutput = Configuration.AUDIO_OUTPUT_BOTH;
                                            value = "Both";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("FPS".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "FPS";
                    String[] options = {"5", "10", "15", "20", "25", "30", "35", "40", "45", "50", "60", "70","80", "90","100"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    configuration.fps = (i + 1) * 5;
                                    value = Integer.toString(configuration.fps);
                                    switch (i) {

                                        case 0:
                                            configuration.fps = 5;
                                            value = "5";
                                            break;
                                        case 1:
                                            configuration.fps = 10;
                                            value = "10";
                                            break;
                                        case 2:
                                            configuration.fps = 15;
                                            value = "15";
                                            break;
                                        case 3:
                                            configuration.fps = 20;
                                            value = "20";
                                            break;
                                        case 4:
                                            configuration.fps = 25;
                                            value = "25";
                                            break;
                                        case 5:
                                            configuration.fps = 30;
                                            value = "30";
                                            break;
                                        case 6:
                                            configuration.fps = 35;
                                            value = "35";
                                            break;
                                        case 7:
                                            configuration.fps = 40;
                                            value = "40";
                                            break;
                                        case 8:
                                            configuration.fps = 45;
                                            value = "45";
                                            break;
                                        case 9:
                                            configuration.fps = 50;
                                            value = "50";
                                            break;
                                        case 10:
                                            configuration.fps = 60;
                                            value = "60";
                                            break;
                                        case 11:
                                            configuration.fps = 70;
                                            value = "70";
                                            break;
                                        case 12:
                                            configuration.fps = 80;
                                            value = "80";
                                            break;
                                        case 13:
                                            configuration.fps = 90;
                                            value = "90";
                                            break;
                                        case 14:
                                            configuration.fps = 100;
                                            value = "100";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Waterfall".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Waterfall";
                    String[] options = {"On", "Off"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.waterfall = true;
                                            value = "On";
                                            break;
                                        case 1:
                                            configuration.waterfall = false;
                                            value = "Off";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Waterfall Display".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Waterfall Display";
                    String[] options = {"Color", "Gray Scale"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.waterfallGrayscale = false;
                                            value = "Color";
                                            break;
                                        case 1:
                                            configuration.waterfallGrayscale = true;
                                            value = "Gray Scale";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Waterfall Automatic Level".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Waterfall Automatic Level";
                    String[] options = {"Disabled", "Enabled"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    switch (i) {
                                        case 0:
                                            configuration.waterfallAutomatic = false;
                                            value = "Disabled";
                                            break;
                                        case 1:
                                            configuration.waterfallAutomatic = true;
                                            value = "Enabled";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Spectrum".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Spectrum";
                    String[] options = {"On", "Off"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.spectrum = true;
                                            value = "On";
                                            break;
                                        case 1:
                                            configuration.spectrum = false;
                                            value = "Off";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Allow Out Of Band TX".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Allow Out Of Band TX";
                    String[] options = {"Yes", "No"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.allowoutofband = true;
                                            value = "Yes";
                                            break;
                                        case 1:
                                            configuration.allowoutofband = false;
                                            value = "No";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if (option.startsWith("Tuna Knob")) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Use Tuna Knob";
                    String[] options = {"Yes", "No"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.tunaknob = true;
                                            value = "Yes";
                                            break;
                                        case 1:
                                            configuration.tunaknob = false;
                                            value = "No";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if (option.equals("Meter Image")) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "Meter Image";
                    String[] options = {"smeter.png", "meter.png"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:
                                            configuration.meter = Configuration.SMETER_PNG;
                                            value = "smeter.png";
                                            break;
                                        case 1:
                                            configuration.meter = Configuration.METER_PNG;
                                            value = "meter.png";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("OpenGL".equals(option)) {
                    AlertDialog.Builder d = new AlertDialog.Builder(context);

                    String title = "OpenGL";
                    String[] options = {"Yes", "No"};

                    d.setTitle(title);
                    d.setItems(options,
                            new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialoginterface, int i) {
                                    Configuration configuration = Configuration.getInstance();
                                    String value = "";
                                    Log.i("Configuration", "onClick:" + i);
                                    switch (i) {
                                        case 0:

                                            ActivityManager activityManager = (ActivityManager) getSystemService(Context.ACTIVITY_SERVICE);
                                            ConfigurationInfo configurationInfo = activityManager.getDeviceConfigurationInfo();
                                            if (configurationInfo.reqGlEsVersion >= 0x20000) {
                                                configuration.opengl = true;
                                                value = "Yes";
                                            } else {
                                                Toast.makeText(context, "Device does not support OpenGL ES 2.0", Toast.LENGTH_LONG);
                                            }
                                            break;
                                        case 1:
                                            configuration.opengl = false;
                                            value = "No";
                                            break;
                                    }
                                    setting.setValue(value);
                                    adapter.notifyDataSetChanged();
                                }
                            });
                    d.show();
                } else if ("Transverter Configuration".equals(option)) {
                    Intent xvtrActivity = new Intent(getApplicationContext(), TransverterActivity.class);
                    startActivity(xvtrActivity);
                } else if ("CW Configuration".equals(option)) {
                    Intent cwActivity = new Intent(getApplicationContext(), CWActivity.class);
                    startActivity(cwActivity);
                }

            }
        });
    }

    public void onRestart() {
        super.onRestart();
        Log.i("SettingsActivity", "onRestart");
        if (setting != null) {
            Log.i("SettingsActivity", "setting=" + setting.getTitle());
        } else {
            Log.i("SettingsActivity", "setting is null");
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.settings, menu);
        return true;
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        String filename = configuration.discovered.getMac() + ".conf";
        Log.i("ConfigureActivity", "onBackPressed");
        Log.i("ConfigureActivity", "onBackPressed: save " + filename);
        try {
            FileOutputStream fos = openFileOutput(filename, Context.MODE_PRIVATE);
            ObjectOutputStream oos = new ObjectOutputStream(fos);
            oos.writeObject(Configuration.getInstance());
            oos.close();
            fos.close();
        } catch (Exception e) {
            Log.i("ConfigureActivity", "onbackPressed: " + e.toString());
        }
    }

    Context context;
    ListView listView;
    private ArrayAdapter<Setting> adapter;
    Configuration configuration;
    ArrayList<Setting> settings = new ArrayList<Setting>();
    Setting setting;

    DecimalFormat decimalformat = new DecimalFormat("#.0");

}
