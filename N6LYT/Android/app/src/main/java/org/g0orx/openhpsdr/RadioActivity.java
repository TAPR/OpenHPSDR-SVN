package org.g0orx.openhpsdr;

import java.io.FileOutputStream;
import java.io.ObjectOutputStream;
import java.text.DecimalFormat;
import java.util.ArrayList;

import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.os.SystemClock;
import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Color;
import android.graphics.PorterDuff.Mode;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Display;
import android.view.InputDevice;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.View.OnTouchListener;
import android.view.Window;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.SeekBar;
import android.widget.SeekBar.OnSeekBarChangeListener;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ViewFlipper;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.wdsp.WDSP;
import org.g0orx.openhpsdr.modes.Modes;
import org.g0orx.openhpsdr.utilities.CPU;

public class RadioActivity extends Activity implements OnTouchListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        activity = this;

        configuration = Configuration.getInstance();

        filename = configuration.discovered.getMac() + ".conf";

        int layout=0;
        int layoutid=R.layout.activity_radio_panadapter_waterfall_bandscope;
        if(configuration.panadapter) {
            layout+=4;
        }
        if(configuration.waterfall) {
            layout+=2;
        }
        if(configuration.bandscope) {
            layout+=1;
        }

        switch(layout) {
            case 0:
                layoutid=R.layout.activity_radio_none;
                break;
            case 1:
                layoutid=R.layout.activity_radio_bandscope;
                break;
            case 2:
                layoutid=R.layout.activity_radio_waterfall;
                break;
            case 3:
                layoutid=R.layout.activity_radio_waterfall_bandscope;
                break;
            case 4:
                layoutid=R.layout.activity_radio_panadapter;
                break;
            case 5:
                layoutid=R.layout.activity_radio_panadapter_bandscope;
                break;
            case 6:
                layoutid=R.layout.activity_radio_panadapter_waterfall;
                break;
            case 7:
                layoutid=R.layout.activity_radio_panadapter_waterfall_bandscope;
                break;
        }

        setContentView(layoutid);

        getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
    }

    @Override
    public void onRestart() {
        super.onRestart();
        //Log.i("RadioActivity","onRestart");
    }

    @Override
    public void onStart() {
        //Log.i("RadioActivity","onStart");
        super.onStart();

        Display display = getWindowManager().getDefaultDisplay();
        final int width = (display.getWidth());
        final int height = (display.getHeight());


        Log.i("RadioActivity", "onStart: width=" + width + " height=" + height);

        metis = new Metis(width, configuration.bandscope);
        metis.setPTTListener(new PTTListener() {
            @Override
            public void PTTChanged(boolean ptt) {
                Log.i("RadioActivity", "PTTChanged:" + ptt);
                handler.sendEmptyMessage(ptt ? PTT_TRUE : PTT_FALSE);
            }
        });



        // get the main views
        vfoView = (VFOView) this.findViewById(R.id.viewVFO);
        vfoView.setMetis(metis);


        meterView = (MeterView) this.findViewById(R.id.viewMeter);

        if(configuration.panadapter) {
            panadapterView = (PanadapterView) this.findViewById(R.id.viewPanadapter);
            panadapterView.setMetis(metis);
        }


        frequencyView = (FrequencyView) this.findViewById(R.id.viewFrequency);

        if(configuration.waterfall) {
            waterfallView = (WaterfallView) this.findViewById(R.id.viewWaterfall);
        }

        if(configuration.bandscope) {
            bandscopeView = (BandscopeView) this.findViewById(R.id.viewBandscope);
        }

        DisplayMetrics metrics = new DisplayMetrics();
        display.getMetrics(metrics);
        float density = metrics.density;
        meterView.setDensity(density);

        if (configuration.bands.getCurrent() >= configuration.bands.getBands().length) {
            configuration.bands.set(0);
        }
        band = configuration.bands.get();
        BandStack bandstack = band.get();
        Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        int low=filter.getLow();
        int high=filter.getHigh();
        if(bandstack.getMode()==Modes.CWL) {
            low=-configuration.cwsidetonefrequency-low;
            high=-configuration.cwsidetonefrequency+high;
        } else if(bandstack.getMode()==Modes.CWU) {
            low=configuration.cwsidetonefrequency-low;
            high=configuration.cwsidetonefrequency+high;
        }


        wdsp = WDSP.getInstance();

        // setup receiver
        wdsp.OpenChannel(Channel.RX, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 0/*rx*/, 1/*RUNNING*/, 0.0,
                0.0, 0.0, 0.0);

        wdsp.SetRXAMode(Channel.RX, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(Channel.RX, low, high);
        wdsp.SetRXABandpassRun(Channel.RX, 1);
        setAGC(Channel.RX,band.getAGC());
        wdsp.SetRXAAGCTop(Channel.RX,band.getAGCGain());
        wdsp.SetRXAAMDSBMode(Channel.RX, 0);

        // setup transmitter
        wdsp.OpenChannel(Channel.TX, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 1/*tx*/, 0/*NOT RUNNING*/, 0.0,
                0.0, 0.0, 0.0);
        wdsp.SetTXAMode(Channel.TX, bandstack.getMode());
        wdsp.SetTXABandpassFreqs(Channel.TX, low, high);
        wdsp.SetTXABandpassRun(Channel.TX, 1);

        // setup sub receiver
        wdsp.OpenChannel(Channel.SUBRX, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 0/*rx*/, 0/*NOT RUNNING*/, 0.0,
                0.0, 0.0, 0.0);

        wdsp.SetRXAMode(Channel.SUBRX, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(Channel.SUBRX, low, high);
        wdsp.SetRXABandpassRun(Channel.SUBRX, 1);
        setAGC(Channel.SUBRX,band.getAGC());
        wdsp.SetRXAAGCTop(Channel.SUBRX,band.getAGCGain());
        wdsp.SetRXAAMDSBMode(Channel.SUBRX, 0);
        wdsp.SetRXAShiftRun(Channel.SUBRX, configuration.subrx ? 1 : 0);
        wdsp.SetRXAShiftFreq(Channel.SUBRX, bandstack.getSubRxFrequency());

        /*
        wdsp.OpenChannel(Channel.BS, 4096, 4096, 48000,
                48000, 48000, 0, 1, 0.0,
                0.0, 0.0, 0.0);
        */

        // create and start the Metis interface
        metis.start();


        // start the display update
        if (displayUpdate == null || !displayUpdate.isAlive()) {
            //Log.i("RadioActivity","onStart: new PanadapterUpdate");
            displayUpdate = new DisplayUpdate(vfoView,panadapterView, waterfallView, meterView, frequencyView, bandscopeView, metis, width);
        }
        displayUpdate.startTimer();

        // setup the touch listener
        if(panadapterView!=null) {
            panadapterView.setOnTouchListener(this);
        }
        if(waterfallView!=null) {
            waterfallView.setOnTouchListener(this);
        }
        frequencyView.setOnTouchListener(this);

        // step spinner
        Spinner spinnerstep = (Spinner) findViewById(R.id.spinnerStep);
        ArrayList<String> steplist = new ArrayList<String>();
        for(int i=0;i<Step.length();i++) {
            steplist.add("Step "+Integer.toString(Step.getStep(i))+" Hz");
        }
        ArrayAdapter stepadapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, steplist);
        stepadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerstep.setAdapter(stepadapter);
        spinnerstep.setSelection(configuration.step);
        spinnerstep.setOnItemSelectedListener(new OnItemSelectedListener() {

            public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                configuration.step = pos;
                int step = Step.getStep(configuration.step);
                Button buttonMinus = (Button) findViewById(R.id.buttonMinus);
                buttonMinus.setText("-" + Integer.toString(step));
                Button buttonPlus = (Button) findViewById(R.id.buttonPlus);
                buttonPlus.setText("+" + Integer.toString(step));
            }

            public void onNothingSelected(AdapterView parent) {
            }
        });

        int step = Step.getStep(configuration.step);

        // tuning knob
        TuningKnobView tuningView = (TuningKnobView) findViewById(R.id.tuningKnob);
        if (configuration.tunaknob) {
            tuningView.setTuningKnobListener(new TuningKnobView.TuningKnobListener() {
                                                 @Override
                                                 public void onKnobChanged(int arg) {
                                                     int step = Step.getStep(configuration.step);
                                                     BandStack bandstack = configuration.bands.get().get();
                                                     if (configuration.subrx) {
                                                         bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() + (step*arg)));
                                                         wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                                                     } else {
                                                         bandstack.setFrequency(bandstack.getFrequency() + (step*arg));
                                                         setFrequency(bandstack.getFrequency());
                                                     }
                                                 }
                                             }
            );
        } else {
            tuningView.setVisibility(View.GONE);
        }

        // frequency -
        Button buttonMinus = (Button) findViewById(R.id.buttonMinus);
        if (!configuration.tunaknob) {
            buttonMinus.setText("-" + Integer.toString(step));
            buttonMinus.setOnTouchListener(new OnTouchListener() {
                @Override
                public boolean onTouch(View v, MotionEvent event) {
                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                        Log.i("RadioActivity", "minus button down " + configuration.step);
                        if (!locked) {
                            int step = Step.getStep(configuration.step);
                            BandStack bandstack = configuration.bands.get().get();
                            if (configuration.subrx) {
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() - step));
                                setSubRxFrequency(bandstack.getSubRxFrequency());
                                wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() - step);
                                setFrequency(bandstack.getFrequency());
                            }
                            handler.removeCallbacks(updateTask);
                            updateincrement=-step;
                            handler.postAtTime(updateTask, SystemClock.uptimeMillis() + 500);
                        }
                    } else if (event.getAction() == MotionEvent.ACTION_UP) {
                        Log.i("RadioActivity", "minus button up");
                        handler.removeCallbacks(updateTask);
                    }
                    return false;
                }
            });
        } else {
            buttonMinus.setVisibility(View.GONE);
        }

        // frequency +
        Button buttonPlus = (Button) findViewById(R.id.buttonPlus);
        if (!configuration.tunaknob) {
            buttonPlus.setText("+" + Integer.toString(step));
            buttonPlus.setOnTouchListener(new OnTouchListener() {
                @Override
                public boolean onTouch(View v, MotionEvent event) {
                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                        if (!locked) {
                            int step = Step.getStep(configuration.step);
                            BandStack bandstack = configuration.bands.get().get();
                            if (configuration.subrx) {
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() + step));
                                setSubRxFrequency(bandstack.getSubRxFrequency());
                                wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() + step);
                                setFrequency(bandstack.getFrequency());
                            }

                            handler.removeCallbacks(updateTask);
                            updateincrement=step;
                            handler.postAtTime(updateTask, SystemClock.uptimeMillis() + 500);
                        }
                    } else if (event.getAction() == MotionEvent.ACTION_UP) {
                        handler.removeCallbacks(updateTask);
                    }
                    return false;
                }
            });
        } else {
            buttonPlus.setVisibility(View.GONE);
        }

        // PTT button
        Button ptt = (Button) findViewById(R.id.buttonPTT);
        ptt.setOnTouchListener(new OnTouchListener() {

            @Override
            public boolean onTouch(View v, MotionEvent event) {
                // only trigger off the down event
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    handler.sendEmptyMessage(metis.isTransmitting() ? PTT_FALSE : PTT_TRUE);
                }
                return false;
            }

        });

        // Tune button
        Button tune = (Button) findViewById(R.id.buttonTune);
        tune.setOnTouchListener(new OnTouchListener() {

            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    cantransmit = false;
                    switch (configuration.discovered.getDevice()) {
                        case Discovered.DEVICE_METIS:
                            if (metis.penelope_software_version > 0) {
                                cantransmit = true;
                            }
                            break;
                        case Discovered.DEVICE_HERMES:
                        case Discovered.DEVICE_GRIFFIN:
                        case Discovered.DEVICE_ANGELIA:
                        case Discovered.DEVICE_ORION:
                            cantransmit = true;
                            break;
                    }
                    if (!cantransmit) {
                        Toast toast = Toast.makeText(activity.getApplicationContext(), "Cannot Transmit: No Penelope or Pennylane", Toast.LENGTH_LONG);
                        toast.show();
                    } else if (outofband && !configuration.allowoutofband) {
                        Toast toast = Toast.makeText(activity.getApplicationContext(), "Out of Band", Toast.LENGTH_LONG);
                        toast.show();
                    } else {
                        if (metis.isTransmitting() && metis.isTuning()) {
                            //Log.i("RadioActivity","Tune button: stop tuning");

                            //Log.i("RadioActivity","TUN: SetChannelState(1,0,1)");
                            wdsp.SetChannelState(Channel.TX, 0, 1);
                            //Log.i("RadioActivity","TUN: SetChannelState(0,1,0)");
                            wdsp.SetChannelState(Channel.RX, 1, 0);
                            if (configuration.subrx) {
                                wdsp.SetChannelState(Channel.SUBRX, 1, 0);
                            }

                            metis.setTransmit(false, false);
                            //configuration.transmit=false;
                            //configuration.tuning=false;
                            v.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                            /*
                            frequency.setTextColor(Color.GREEN);
        					*/
                        } else {
                            if (metis.isTransmitting() && !metis.isTuning()) {
                                //Log.i("RadioActivity","Tune button: stop transmitting");
                                Button ptt = (Button) findViewById(R.id.buttonPTT);
                                ptt.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                            }
                            Log.i("RadioActivity", "Tune button: start tuning:" + configuration.tunegain);

                            // receive to transmit
                            //Log.i("RadioActivity","TUN: SetChannelState(0,0,1)");
                            wdsp.SetChannelState(Channel.RX, 0, 1);
                            if (configuration.subrx) {
                                wdsp.SetChannelState(Channel.SUBRX, 0, 1);
                            }
                            //Log.i("RadioActivity","TUN: SetChannelState(1,1,0)");
                            wdsp.SetChannelState(Channel.TX, 1, 0);

                            //configuration.transmit=true;
                            //configuration.tuning=true;
                            metis.setTransmit(true, true);
                            v.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
                            /*
        					frequency.setTextColor(Color.RED);
        					*/
                        }
                    }
                    vfoView.update();
                }
                return false;
            }

        });

        Button bandButton = (Button) findViewById(R.id.buttonBand);
        bandButton.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {

                    View view = View.inflate(activity, R.layout.dialog_band, null);

                    Band band = configuration.bands.get();
                    BandStack bandstack = configuration.bands.get().get();

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    builder.setView(view)
                            .setCancelable(false)
                            .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                }
                            })
                            .setOnKeyListener(new DialogInterface.OnKeyListener() {
                                public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
                                    if (keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN) {
                                        dialog.dismiss();
                                        return true;
                                    }
                                    return false;
                                }
                            });
                    AlertDialog dialog = builder.create();
                    final TextView status = (TextView) view.findViewById(R.id.status);
                    status.setText("Band " + band.getName() + "(" + band.getBandstackentry() + ")" + " " + Modes.getMode(bandstack.getMode()).getName() + " " + Frequency.toString(bandstack.getFrequency()));

                    final Band[] bands = configuration.bands.getBands();
                    final int[] buttonid = {R.id.button0, R.id.button1, R.id.button2, R.id.button3, R.id.button4,
                            R.id.button5, R.id.button6, R.id.button7, R.id.button8, R.id.button9,
                            R.id.button10, R.id.button11, R.id.button12, R.id.button13, R.id.button14,
                            R.id.button15, R.id.button16, R.id.button17, R.id.button18, R.id.button19,
                            R.id.button20, R.id.button21, R.id.button22, R.id.button23, R.id.button24};
                    band = null;
                    Button button = null;
                    for (int i = 0; i < bands.length; i++) {
                        band = bands[i];
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setText(band.getName());
                        button.setOnTouchListener(new OnTouchListener() {
                            @Override
                            public boolean onTouch(View v, MotionEvent event) {
                                if (!locked) {
                                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                                        for (int i = 0; i < buttonid.length; i++) {
                                            if (buttonid[i] == v.getId()) {
                                                if (configuration.bands.getCurrent() == i) {
                                                    // step through bandstack
                                                    Band band = configuration.bands.get();
                                                    band.next();
                                                } else {
                                                    // changing band
                                                    configuration.bands.set(i);
                                                }
                                                Band band = configuration.bands.get();
                                                BandStack bandstack = band.get();
                                                Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                                if (bandstack != null && !locked) {
                                                    int low=filter.getLow();
                                                    int high=filter.getHigh();
                                                    if(bandstack.getMode()==Modes.CWL) {
                                                        low=-configuration.cwsidetonefrequency-low;
                                                        high=-configuration.cwsidetonefrequency+high;
                                                    } else if(bandstack.getMode()==Modes.CWU) {
                                                        low=configuration.cwsidetonefrequency-low;
                                                        high=configuration.cwsidetonefrequency+high;
                                                    }

                                                    setFilter(low,high);
                                                    setAGC(Channel.RX,band.getAGC());

                                                    if (configuration.subrx) {
                                                        configuration.subrx = false;
                                                        wdsp.SetChannelState(Channel.SUBRX, 0, 0);
                                                        wdsp.SetRXAShiftRun(Channel.SUBRX, 0);
                                                        setAGC(Channel.SUBRX,band.getAGC());
                                                    }

                                                    setMode(bandstack.getMode());
                                                    setFrequency(bandstack.getFrequency());

                                                    Button squelch = (Button) findViewById(R.id.buttonSquelch);
                                                    squelch.getBackground().setColorFilter(band.getSquelch() ? Color.RED : Color.LTGRAY, Mode.MULTIPLY);

                                                    if (band.get().getMode() == Modes.FMN) {
                                                        double v1 = -2.0 * (double) band.getSquelchValue() / 100.0;
                                                        double v2 = Math.pow(10.0, v1);
                                                        Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " " + v1 + " " + v2);
                                                        wdsp.SetRXAFMSQThreshold(Channel.RX, v2);
                                                        wdsp.SetRXAFMSQRun(Channel.RX, band.getSquelch() ? 1 : 0);
                                                    } else {
                                                        wdsp.SetRXAAMSQThreshold(Channel.RX, (double) -band.getSquelchValue());
                                                        wdsp.SetRXAAMSQRun(Channel.RX, band.getSquelch() ? 1 : 0);
                                                    }

                                                    status.setText("Band " + band.getName() + "(" + band.getBandstackentry() + ")" + " " + Modes.getMode(bandstack.getMode()).getName() + " " + Frequency.toString(bandstack.getFrequency()));
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                return false;
                            }
                        });
                    }

                    for (int i = bands.length; i < 25; i++) {
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setVisibility(View.GONE);
                    }
                    builder.show();
                }
                return false;
            }
        });

        // Lock button
        Button lock = (Button) findViewById(R.id.buttonLock);
        lock.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    Button buttonFrequency=(Button)findViewById(R.id.buttonFrequency);
                    Button buttonBand = (Button) findViewById(R.id.buttonBand);
                    Button buttonPlus = (Button) findViewById(R.id.buttonPlus);
                    Button buttonMinus = (Button) findViewById(R.id.buttonMinus);
                    Spinner spinnerStep = (Spinner) findViewById(R.id.spinnerStep);
                    TuningKnobView tuningView = (TuningKnobView) findViewById(R.id.tuningKnob);
                    if (locked) {
                        locked = false;
                        v.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                        buttonFrequency.setEnabled(true);
                        buttonBand.setEnabled(true);
                        buttonPlus.setEnabled(true);
                        buttonMinus.setEnabled(true);
                        spinnerStep.setEnabled(true);
                        tuningView.setEnabled(true);
                    } else {
                        locked = true;
                        v.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
                        buttonFrequency.setEnabled(false);
                        buttonBand.setEnabled(false);
                        buttonPlus.setEnabled(false);
                        buttonMinus.setEnabled(false);
                        spinnerStep.setEnabled(false);
                        tuningView.setEnabled(false);
                    }
                }
                return false;
            }
        });

        // SubRX button
        Button subrx = (Button) findViewById(R.id.buttonSUBRX);
        if (subrx != null) {
            subrx.setOnTouchListener(new OnTouchListener() {
                @Override
                public boolean onTouch(View v, MotionEvent event) {
                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                        if (configuration.subrx) {
                            configuration.subrx = false;
                            wdsp.SetChannelState(Channel.SUBRX, 0, 0);
                            wdsp.SetRXAShiftRun(Channel.SUBRX, 0);
                        } else {
                            configuration.subrx = true;
                            BandStack bandstack = configuration.bands.get().get();
                            Log.i("RadioActivity", "subrx freq:" + bandstack.getSubRxFrequency());
                            if (bandstack.getSubRxFrequency() == 0) {
                                bandstack.setSubRxFrequency(bandstack.getFrequency());
                            }
                            bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency()));
                            wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            wdsp.SetRXAShiftRun(Channel.SUBRX, 1);
                            wdsp.SetChannelState(Channel.SUBRX, 1, 0);
                        }
                        vfoView.update();
                    }
                    return false;
                }
            });
        }

        // Frequency button
        final Button frequency = (Button) findViewById(R.id.buttonFrequency);
        frequency.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    final View view = View.inflate(activity, R.layout.dialog_frequency, null);

                    final AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    builder.setMessage("Set Frequency (Hz)")
                            .setView(view)
                            .setCancelable(false)
                            .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                }
                            })
                            .setOnKeyListener(new DialogInterface.OnKeyListener() {
                                public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
                                    if (keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN) {
                                        dialog.dismiss();
                                        return true;
                                    }
                                    return false;
                                }
                            });
                    final AlertDialog dialog = builder.create();
                    dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                    builder.show();

                    f = 0L;

                    final TextView message = (TextView) view.findViewById(R.id.textViewFrequency);

                    Button setFrequency = (Button) view.findViewById(R.id.buttonSetFrequency);
                    setFrequency.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            try {
                                if (f > 0L) {
                                    // lookup band
                                    int b = configuration.bands.get(f);
                                    if (b == -1) {
                                        b = configuration.bands.get("AM");
                                    }
                                    if (b != -1) {
                                        if (configuration.bands.getCurrent() != b) {
                                            configuration.bands.set(b);
                                            // FIX spinnerband.setSelection(configuration.bands.getCurrent());
                                        }
                                        Band band = configuration.bands.get();
                                        BandStack bandstack = band.get();
                                        Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                        bandstack.setFrequency(f);
                                        setFrequency(f);
                                        if (bandstack != null && !locked) {
                                            int low=filter.getLow();
                                            int high=filter.getHigh();
                                            if(bandstack.getMode()==Modes.CWL) {
                                                low=-configuration.cwsidetonefrequency-low;
                                                high=-configuration.cwsidetonefrequency+high;
                                            } else if(bandstack.getMode()==Modes.CWU) {
                                                low=configuration.cwsidetonefrequency-low;
                                                high=configuration.cwsidetonefrequency+high;
                                            }

                                            setFilter(low,high);
                                            setAGC(Channel.RX,band.getAGC());

                                            if (configuration.subrx) {
                                                configuration.subrx = false;
                                                wdsp.SetChannelState(Channel.SUBRX, 0, 0);
                                                wdsp.SetRXAShiftRun(Channel.SUBRX, 0);
                                                setAGC(Channel.SUBRX,band.getAGC());
                                            }

                                            setMode(bandstack.getMode());
                                            setFrequency(bandstack.getFrequency());

                                        }
                                    }
                                }
                            } catch (Exception e) {
                                // invalid value
                                Log.i("RadioActivity", "Invalid frequency: " + f);
                            }

                        }
                    });

                    Button button1 = (Button) view.findViewById(R.id.button1);
                    button1.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 1;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button2 = (Button) view.findViewById(R.id.button2);
                    button2.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 2;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button3 = (Button) view.findViewById(R.id.button3);
                    button3.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 3;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button4 = (Button) view.findViewById(R.id.button4);
                    button4.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 4;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button5 = (Button) view.findViewById(R.id.button5);
                    button5.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 5;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button6 = (Button) view.findViewById(R.id.button6);
                    button6.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 6;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button7 = (Button) view.findViewById(R.id.button7);
                    button7.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 7;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button8 = (Button) view.findViewById(R.id.button8);
                    button8.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 8;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button9 = (Button) view.findViewById(R.id.button9);
                    button9.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = (f * 10) + 9;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button0 = (Button) view.findViewById(R.id.button0);
                    button0.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = f * 10;
                            if (f > 0) {
                                message.setText(Long.toString(f));
                            }
                        }
                    });


                    Button buttonDelete = (Button) view.findViewById(R.id.buttonDelete);
                    buttonDelete.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = f / 10;
                            if (f > 0) {
                                message.setText(Long.toString(f));
                            }
                        }
                    });

                    Button buttonClear = (Button) view.findViewById(R.id.buttonClear);
                    buttonClear.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f = 0;
                            message.setText("");
                        }
                    });
                }
                return false;
            }
        });

        // Configure button
        final Button configure = (Button) findViewById(R.id.buttonConfigure);
        configure.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    ConfigureDialog d=new ConfigureDialog(activity,locked);
                    d.show();
                }
                return false;
            }
        });

        Button modeButton = (Button) findViewById(R.id.buttonMode);
        modeButton.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {

                    View view = View.inflate(activity, R.layout.dialog_mode, null);

                    Band band = configuration.bands.get();
                    BandStack bandstack = configuration.bands.get().get();

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    builder.setView(view)
                            .setCancelable(false)
                            .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                }
                            })
                            .setOnKeyListener(new DialogInterface.OnKeyListener() {
                                public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
                                    if (keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN) {
                                        dialog.dismiss();
                                        return true;
                                    }
                                    return false;
                                }
                            });
                    AlertDialog dialog = builder.create();
                    final TextView status = (TextView) view.findViewById(R.id.status);
                    status.setText("Mode " + Modes.getMode(bandstack.getMode()).getName());

                    final int[] buttonid = {R.id.button0, R.id.button1, R.id.button2, R.id.button3, R.id.button4,
                            R.id.button5, R.id.button6, R.id.button7, R.id.button8, R.id.button9,
                            R.id.button10, R.id.button11, R.id.button12, R.id.button13, R.id.button14,
                            R.id.button15, R.id.button16, R.id.button17, R.id.button18, R.id.button19,
                            R.id.button20, R.id.button21, R.id.button22, R.id.button23, R.id.button24};
                    String mode = null;
                    Button button = null;
                    for (int i = 0; i < Modes.length(); i++) {
                        mode = Modes.getMode(i).getName();
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setText(mode);
                        button.setOnTouchListener(new OnTouchListener() {
                            @Override
                            public boolean onTouch(View v, MotionEvent event) {
                                if (!locked) {
                                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                                        for (int i = 0; i < buttonid.length; i++) {
                                            if (buttonid[i] == v.getId()) {
                                                BandStack bandstack = configuration.bands.get().get();
                                                bandstack.setMode(i);
                                                Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                                int low=filter.getLow();
                                                int high=filter.getHigh();
                                                if(bandstack.getMode()==Modes.CWL) {
                                                    low=-configuration.cwsidetonefrequency-low;
                                                    high=-configuration.cwsidetonefrequency+high;
                                                } else if(bandstack.getMode()==Modes.CWU) {
                                                    low=configuration.cwsidetonefrequency-low;
                                                    high=configuration.cwsidetonefrequency+high;
                                                }
                                                setMode(bandstack.getMode());
                                                setFilter(low,high);
                                                String mode = Modes.getMode(i).getName();
                                                status.setText("Mode " + mode);
                                                break;
                                            }
                                        }
                                    }
                                }
                                return false;
                            }
                        });
                    }

                    for (int i = Modes.length(); i < 25; i++) {
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setVisibility(View.GONE);
                    }
                    builder.show();
                }
                return false;
            }
        });

        Button filterButton = (Button) findViewById(R.id.buttonFilter);
        filterButton.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {

                    View view = View.inflate(activity, R.layout.dialog_mode, null);

                    Band band = configuration.bands.get();
                    BandStack bandstack = band.get();
                    int length=Modes.getMode(bandstack.getMode()).length();


                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    builder.setView(view)
                            .setCancelable(false)
                            .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                }
                            })
                            .setOnKeyListener(new DialogInterface.OnKeyListener() {
                                public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
                                    if (keyCode == KeyEvent.KEYCODE_BACK && event.getAction() == KeyEvent.ACTION_DOWN) {
                                        dialog.dismiss();
                                        return true;
                                    }
                                    return false;
                                }
                            });
                    AlertDialog dialog = builder.create();
                    final TextView status = (TextView) view.findViewById(R.id.status);
                    status.setText("Mode " + Modes.getMode(bandstack.getMode()).getName()+" "+Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter()).getName()+"Hz");

                    final int[] buttonid = {R.id.button0, R.id.button1, R.id.button2, R.id.button3, R.id.button4,
                            R.id.button5, R.id.button6, R.id.button7, R.id.button8, R.id.button9,
                            R.id.button10, R.id.button11, R.id.button12, R.id.button13, R.id.button14,
                            R.id.button15, R.id.button16, R.id.button17, R.id.button18, R.id.button19,
                            R.id.button20, R.id.button21, R.id.button22, R.id.button23, R.id.button24};
                    String filter = null;
                    Button button = null;
                    for (int i = 0; i < length; i++) {
                        filter=Modes.getMode(bandstack.getMode()).getFilter(i).getName();
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setText(filter+"Hz");
                        button.setOnTouchListener(new OnTouchListener() {
                            @Override
                            public boolean onTouch(View v, MotionEvent event) {
                                if (!locked) {
                                    if (event.getAction() == MotionEvent.ACTION_DOWN) {
                                        for (int i = 0; i < buttonid.length; i++) {
                                            if (buttonid[i] == v.getId()) {
                                                BandStack bandstack = configuration.bands.get().get();
                                                bandstack.setFilter(i);
                                                Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
                                                int low=filter.getLow();
                                                int high=filter.getHigh();
                                                if(bandstack.getMode()==Modes.CWL) {
                                                    low=-configuration.cwsidetonefrequency-low;
                                                    high=-configuration.cwsidetonefrequency+high;
                                                } else if(bandstack.getMode()==Modes.CWU) {
                                                    low=configuration.cwsidetonefrequency-low;
                                                    high=configuration.cwsidetonefrequency+high;
                                                }
                                                setFilter(low,high);
                                                status.setText("Mode " + Modes.getMode(bandstack.getMode()).getName()+" "+Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter()).getName()+"Hz");
                                                break;
                                            }
                                        }
                                    }
                                }
                                return false;
                            }
                        });
                    }

                    for (int i = length; i < 25; i++) {
                        button = (Button) view.findViewById(buttonid[i]);
                        button.setVisibility(View.GONE);
                    }
                    builder.show();
                }
                return false;
            }
        });

        // Squelch button
        final Button squelch = (Button) findViewById(R.id.buttonSquelch);
        squelch.getBackground().setColorFilter(band.getSquelch() ? Color.RED : Color.LTGRAY, Mode.MULTIPLY);
        squelch.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    final View view = View.inflate(activity, R.layout.dialog_squelch, null);

                    Band band = configuration.bands.get();
                    final CheckBox checkbox = (CheckBox) view.findViewById(R.id.checkBoxSquelch);
                    checkbox.setChecked(band.getSquelch());
                    if (band.get().getMode() == Modes.FMN) {
                        if (band.getSquelchValue() > 100) band.setSquelchValue(50);
                        checkbox.setText("Squelch: " + (int) (band.getSquelchValue()));
                    } else {
                        checkbox.setText("Squelch: " + (int) -(band.getSquelchValue()) + " dBm");
                    }

                    final AlertDialog.Builder builder = new AlertDialog.Builder(activity);

                    builder.setMessage("Set Squelch")
                            .setView(view)
                            .setCancelable(false)
                            .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                public void onClick(DialogInterface dialog, int id) {
                                }
                            });
                    AlertDialog dialog = builder.create();

                    dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);

                    builder.show();

                    SeekBar seekbar = (SeekBar) view.findViewById(R.id.seekBarSquelch);
                    if (band.get().getMode() == Modes.FMN) {
                        seekbar.setMax(100);
                    } else {
                        seekbar.setMax(160);
                    }
                    seekbar.setProgress((int) band.getSquelchValue());
                    seekbar.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                        public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                            Log.i("RadioActivity", "squelch=" + progress);
                            Band band = configuration.bands.get();
                            band.setSquelchValue(progress);
                            if (band.get().getMode() == Modes.FMN) {
                                double v1 = -2.0 * (double) band.getSquelchValue() / 100.0;
                                double v2 = Math.pow(10.0, v1);
                                Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " " + v1 + " " + v2);
                                wdsp.SetRXAFMSQThreshold(Channel.RX, v2);
                                checkbox.setText("Squelch: " + (int) (band.getSquelchValue()));
                            } else {
                                wdsp.SetRXAAMSQThreshold(Channel.RX, (double) -band.getSquelchValue());
                                checkbox.setText("Squelch: " + (int) -(band.getSquelchValue()) + " dBm");
                            }
                        }

                        public void onStartTrackingTouch(SeekBar seekBar) {
                        }

                        public void onStopTrackingTouch(SeekBar seekBar) {
                        }
                    });

                    checkbox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                        @Override
                        public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                            Log.i("RadioActivity", "Squelch checkbox " + isChecked);
                            Band band = configuration.bands.get();
                            squelch.getBackground().setColorFilter(isChecked ? Color.RED : Color.LTGRAY, Mode.MULTIPLY);
                            band.setSquelch(isChecked);
                            if (band.get().getMode() == Modes.FMN) {
                                if (isChecked) {
                                    double v1 = -2.0 * (double) band.getSquelchValue() / 100.0;
                                    double v2 = Math.pow(10.0, v1);
                                    Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " " + v1 + " " + v2);
                                    wdsp.SetRXAFMSQThreshold(Channel.RX, v2);
                                }
                                wdsp.SetRXAFMSQRun(Channel.RX, isChecked ? 1 : 0);
                            } else {
                                if (isChecked) {
                                    Log.i("RadioActivity", "AM squelch set to " + band.getSquelchValue());
                                    wdsp.SetRXAAMSQThreshold(Channel.RX, (double) -band.getSquelchValue());
                                    checkbox.setText("Squelch: " + (int) -(band.getSquelchValue()) + " dBm");
                                }
                                wdsp.SetRXAAMSQRun(Channel.RX, isChecked ? 1 : 0);
                            }

                        }
                    });
                }
                return false;
            }
        });


        setFrequency(configuration.bands.get().get().getFrequency());

        // disable subrx
        configuration.subrx = false;

    }

    @Override
    public void onResume() {
        //Log.i("RadioActivity","onResume");
        super.onResume();
    }

    @Override
    public void onPause() {
        //Log.i("RadioActivity","onPause");
        super.onPause();

        if (metis.isTransmitting()) {
            metis.setTransmit(false, false);
            //configuration.transmit=false;
            //configuration.tuning=false;
        }

        if (displayUpdate.isRunning()) {
            //Log.i("RadioActivity","onPause: panadapterUpdate.terminate");
            displayUpdate.terminate();
        }

        if (metis.isRunning()) {
            //Log.i("RadioActivity","onPause: metis.terminate");
            metis.terminate();
        }



        wdsp.CloseChannel(Channel.RX);
        wdsp.CloseChannel(Channel.SUBRX);
        wdsp.CloseChannel(Channel.TX);
        //wdsp.CloseChannel(Channel.BS);






        try {
            FileOutputStream fos = openFileOutput(filename, Context.MODE_PRIVATE);
            ObjectOutputStream oos = new ObjectOutputStream(fos);
            oos.writeObject(Configuration.getInstance());
            oos.close();
            fos.close();
        } catch (Exception e) {
            Log.i("RadioActivity", "onPause: " + e.toString());
        }

    }

    @Override
    public void onBackPressed() {
        Log.i("RadioActivity", "onbackPressed");
        super.onBackPressed();

    }

    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            AlertDialog alertDialog = new AlertDialog.Builder(this).create();
            alertDialog.setTitle("openHPSDR");
            //alertDialog.setIcon(R.drawable.appicon);

            alertDialog.setMessage("Do you really want to return to Discovery?");
            alertDialog.setButton("Yes", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    finish();
                    return;
                }
            });
            alertDialog.setButton2("No", new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    dialog.cancel();
                    return;
                }
            });
            alertDialog.show();

            return true;
        }
        return super.onKeyDown(keyCode, event);
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
    }

    @Override
    public boolean onGenericMotionEvent(MotionEvent event) {
        if (0 != (event.getSource() & InputDevice.SOURCE_CLASS_POINTER)) {
            switch (event.getAction()) {
                case MotionEvent.ACTION_SCROLL:
                    BandStack bandstack = configuration.bands.get().get();
                    int step = Step.getStep(configuration.step);
                    //Log.i("RadioActivity","onGenericMotionEvent vscroll:"+event.getAxisValue(MotionEvent.AXIS_VSCROLL));
                    if (event.getAxisValue(MotionEvent.AXIS_VSCROLL) < 0.0f) {
                        step = -step;
                    }
                    if (configuration.subrx) {
                        bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() - step));
                        wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                    } else {
                        bandstack.setFrequency(bandstack.getFrequency() + step);
                        setFrequency(bandstack.getFrequency());
                    }
                    return true;
            }
        }
        return super.onGenericMotionEvent(event);
    }

    private Handler handler = new Handler() {
        @Override
        public void handleMessage(Message msg) {
            super.handleMessage(msg);
            switch (msg.what) {
                case PTT_FALSE:
                    processPTT(false);
                    break;
                case PTT_TRUE:
                    processPTT(true);
                    break;
            }
        }
    };

    private Runnable updateTask = new Runnable() {
        public void run() {
            if (!locked) {
                BandStack bandstack = configuration.bands.get().get();
                if (configuration.subrx) {
                    bandstack.setSubRxFrequency(bandstack.getSubRxFrequency() + updateincrement);
                    setSubRxFrequency(bandstack.getSubRxFrequency());
                    wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                    handler.postAtTime(this, SystemClock.uptimeMillis() + 100);
                } else {
                    bandstack.setFrequency(bandstack.getFrequency() + updateincrement);
                    setFrequency(bandstack.getFrequency());
                    handler.postAtTime(this, SystemClock.uptimeMillis() + 100);
                }
            }
        }
    };

    public boolean onTouch(View v, MotionEvent event) {

        if (!locked) {
            double hzperpixel = configuration.samplerate / (double) frequencyView.getWidth();
            BandStack bandstack = configuration.bands.get().get();
            int step = Step.getStep(configuration.step);
            switch (event.getAction()) {
                case MotionEvent.ACTION_CANCEL:
                    break;
                case MotionEvent.ACTION_DOWN:
                    startX = event.getX();
                    startY = event.getY();
                    moved = false;
                    break;
                case MotionEvent.ACTION_MOVE:
                    long increment = (long) (startX - event.getX());
                    long freqincrement = (long) ((double) increment * hzperpixel);
                    freqincrement = (freqincrement / 100L) * 100L;
                    if (configuration.subrx) {
                        bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() - (freqincrement/* / 100 * 100*/)));
                        wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                    } else {
                        bandstack.setFrequency(bandstack.getFrequency() + (freqincrement/* / 100 * 100*/));
                        setFrequency(bandstack.getFrequency());
                    }
                    startX = event.getX();
                    moved = true;
                    break;
                case MotionEvent.ACTION_OUTSIDE:
                    break;
                case MotionEvent.ACTION_UP:
                    if (!moved) {
                        long start = bandstack.getFrequency() - ((long) configuration.samplerate / 2);
                        long f = start + (long) (event.getX() * hzperpixel);

                        // make it step resolution
                        f = f / (long)step * (long)step;

                        if (configuration.subrx) {
                            bandstack.setSubRxFrequency(setSubRxFrequency(f));
                            wdsp.SetRXAShiftFreq(Channel.SUBRX, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                        } else {
                            bandstack.setFrequency(f);
                            setFrequency(bandstack.getFrequency());
                        }
                    }
                    break;
            }

        }
        return true;
    }

    private void setFrequency(long f) {
        String fs = String.format("%d.%03d.%03d",
                f / 1000000,
                (f % 1000000) / 1000,
                (f % 1000));
        /*
    	frequency.setText(fs);
    	*/
        BandEdge bandedge = configuration.bands.get().getBandEdge();
        if (f >= bandedge.getLow() && f < bandedge.getHigh()) {
            outofband = false;
        } else {
            outofband = true;
        }

        frequencyView.update();
        vfoView.update();
    }

    private long setSubRxFrequency(long f) {
        BandStack bandstack = configuration.bands.get().get();
        Filter filter=Modes.getMode(bandstack.getMode()).getFilter(bandstack.getFilter());
        long lowfreq = bandstack.getFrequency() - (long) (configuration.samplerate / 2.0);
        if (filter.getLow() < 0) {
            lowfreq = lowfreq + filter.getLow();
        }
        long highfreq = bandstack.getFrequency() + (long) (configuration.samplerate / 2.0);
        if (filter.getHigh() > 0) {
            highfreq = highfreq - filter.getHigh();
        }
        if (f < lowfreq) {
            f = lowfreq;
        } else if (f > highfreq) {
            f = highfreq;
        }

        BandEdge bandedge = configuration.bands.get().getBandEdge();
        if (f >= bandedge.getLow() && f < bandedge.getHigh()) {
            outofband = false;
        } else {
            outofband = true;
        }
        vfoView.update();
        return f;
    }

    private void setMode(int mode) {
        wdsp.SetRXAMode(Channel.RX, mode);
        wdsp.SetRXAMode(Channel.SUBRX, mode);
        wdsp.SetTXAMode(Channel.TX, mode);
        vfoView.update();
    }

    private void setFilter(int low, int high) {
        wdsp.SetRXABandpassFreqs(Channel.RX, low, high);
        wdsp.SetRXABandpassFreqs(Channel.SUBRX, low, high);
        wdsp.SetTXABandpassFreqs(Channel.TX, low, high);
        vfoView.update();
        frequencyView.update();
    }

    private void setAGC(int channel,int agc) {
        wdsp.SetRXAAGCMode(channel,agc);
        switch(agc) {
            case AGCLong:
                wdsp.SetRXAAGCAttack(channel,2);
                wdsp.SetRXAAGCHang(channel, 2000);
                wdsp.SetRXAAGCDecay(channel,2000);
                break;
            case AGCSlow:
                wdsp.SetRXAAGCAttack(channel,2);
                wdsp.SetRXAAGCHang(channel,1000);
                wdsp.SetRXAAGCDecay(channel,500);
                break;
            case AGCMed:
                wdsp.SetRXAAGCAttack(channel,2);
                wdsp.SetRXAAGCHang(channel,0);
                wdsp.SetRXAAGCDecay(channel,250);
                break;
            case AGCFast:
                wdsp.SetRXAAGCAttack(channel,2);
                wdsp.SetRXAAGCHang(channel,0);
                wdsp.SetRXAAGCDecay(channel,50);
                break;
        }
    }

    private void processPTT(boolean state) {

        Log.i("RadioActivity", "processPTT:" + state);

        Button ptt = (Button) findViewById(R.id.buttonPTT);
        Button tune = (Button) findViewById(R.id.buttonTune);

        if (state) {
            // start transmitting
            cantransmit = false;
            switch (configuration.discovered.getDevice()) {
                case Discovered.DEVICE_METIS:
                    if (metis.penelope_software_version > 0) {
                        cantransmit = true;
                    }
                    break;
                case Discovered.DEVICE_HERMES:
                case Discovered.DEVICE_GRIFFIN:
                case Discovered.DEVICE_ANGELIA:
                case Discovered.DEVICE_ORION:
                    cantransmit = true;
                    break;
            }
            if (!cantransmit) {
                Toast toast = Toast.makeText(activity.getApplicationContext(), "Cannot Transmit: No Penelope or Pennylane", Toast.LENGTH_LONG);
                toast.show();
            } else if (outofband && !configuration.allowoutofband) {
                Toast toast = Toast.makeText(activity.getApplicationContext(), "Out of Band", Toast.LENGTH_LONG);
                toast.show();
            } else {
                if (metis.isTuning()) {
                    metis.setTransmit(false, false);
                    tune.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                }
                Log.i("RadioActivity", "processPTT: start transmitting");
                BandStack bandstack = configuration.bands.get().get();

                Log.i("RadioActivity", "processPTT: SetChannelState(Channel.RX,0,1)");
                wdsp.SetChannelState(Channel.RX, 0, 1);
                if (configuration.subrx) {
                    Log.i("RadioActivity", "processPTT: SetChannelState(Channel.SUBRX,0,1)");
                    wdsp.SetChannelState(Channel.SUBRX, 0, 1);
                }
                Log.i("RadioActivity", "processPTT: SetChannelState(Channel.TX,1,0)");
                wdsp.SetChannelState(Channel.TX, 1, 0);

                Log.i("RadioActivity", "processPTT: metis.setTransmit(true,false)");
                metis.setTransmit(true, false);
                if (configuration.micsource == Configuration.MIC_SOURCE_LOCAL) {
                    microphone = new Microphone(metis, Channel.TX);
                    microphone.start();
                }
                Log.i("RadioActivity", "Set PTT button red");
                ptt.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
            }
        } else {
            // stop transmitting
            wdsp.SetChannelState(Channel.TX, 0, 1);
            wdsp.SetChannelState(Channel.RX, 1, 0);
            if (configuration.subrx) {
                wdsp.SetChannelState(Channel.SUBRX, 1, 0);
            }
            metis.setTransmit(false, false);
            ptt.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
        }

        vfoView.update();
    }

    private static final int PTT_FALSE = 0;
    private static final int PTT_TRUE = 1;

    private static final int AGCOff=0;
    private static final int AGCLong=1;
    private static final int AGCSlow=2;
    private static final int AGCMed=3;
    private static final int AGCFast=4;

    private Activity activity;

    private CPU cpu;

    private Configuration configuration;

    private Metis metis;

    private WDSP wdsp;

    private Microphone microphone;

    private VFOView vfoView;
    private PanadapterView panadapterView;
    private WaterfallView waterfallView;
    private FrequencyView frequencyView;
    private MeterView meterView;
    private BandscopeView bandscopeView;

    private DisplayUpdate displayUpdate;

    private Band band;
    private long updateincrement;

    private String filename = "hpsdr.conf"; // will be replaced with macaddress.conf

    private SeekBar seekbar;

    private boolean cantransmit = false;

    private boolean locked = false;

    private boolean outofband = false;

    private DecimalFormat format = new DecimalFormat("##0.0#");

    private float startX;
    private float startY;
    private boolean moved;

    private long f;
}
