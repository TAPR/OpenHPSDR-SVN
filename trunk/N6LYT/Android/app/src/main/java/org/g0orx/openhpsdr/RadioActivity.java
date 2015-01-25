package org.g0orx.openhpsdr;

import java.io.FileOutputStream;
import java.io.ObjectOutputStream;
import java.text.DecimalFormat;
import java.util.ArrayList;

import android.app.Dialog;
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

        setContentView(R.layout.activity_radio);
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

        metis = new Metis(width,false);
        metis.setPTTListener(new PTTListener() {
            @Override
            public void PTTChanged(boolean ptt) {
                Log.i("RadioActivity", "PTTChanged:" + ptt);
                handler.sendEmptyMessage(ptt?PTT_TRUE:PTT_FALSE);
            }
        });

        // get the main views
        panadapterView = (PanadapterView) this.findViewById(R.id.viewPanadapter);
        panadapterView.setMetis(metis);

        waterfallView = (WaterfallView) this.findViewById(R.id.viewWaterfall);
        meterView = (MeterView) this.findViewById(R.id.viewMeter);
        DisplayMetrics metrics = new DisplayMetrics();
        display.getMetrics(metrics);
        float density = metrics.density;
        meterView.setDensity(density);

        if (configuration.bands.getCurrent() >= configuration.bands.getBands().length) {
            configuration.bands.set(0);
        }
        band = configuration.bands.get();
        BandStack bandstack = band.get();


        wdsp = WDSP.getInstance();

        // setup receiver
        wdsp.OpenChannel(rxchannel, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 0/*rx*/, 1/*RUNNING*/, 0.0,
                0.0, 0.0, 0.0);

        wdsp.SetRXAMode(rxchannel, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(rxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
        wdsp.SetRXABandpassRun(rxchannel, 1);
        wdsp.SetRXAAGCMode(rxchannel, bandstack.getAGC());
        wdsp.SetRXAAMDSBMode(rxchannel, 0);

        // setup transmitter
        wdsp.OpenChannel(txchannel, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 1/*tx*/, 0/*NOT RUNNING*/, 0.0,
                0.0, 0.0, 0.0);
        wdsp.SetTXAMode(txchannel, bandstack.getMode());
        wdsp.SetTXABandpassFreqs(txchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
        wdsp.SetTXABandpassRun(txchannel, 1);

        // setup sub receiver
        wdsp.OpenChannel(subrxchannel, configuration.fftsize, configuration.fftsize, (int) configuration.samplerate,
                (int) configuration.samplerate, (int) configuration.samplerate, 0/*rx*/, 0/*NOT RUNNING*/, 0.0,
                0.0, 0.0, 0.0);

        wdsp.SetRXAMode(subrxchannel, bandstack.getMode());
        wdsp.SetRXABandpassFreqs(subrxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
        wdsp.SetRXABandpassRun(subrxchannel, 1);
        wdsp.SetRXAAGCMode(subrxchannel, bandstack.getAGC());
        wdsp.SetRXAAMDSBMode(subrxchannel, 0);
        wdsp.SetRXAShiftRun(subrxchannel, configuration.subrx ? 1 : 0);
        wdsp.SetRXAShiftFreq(subrxchannel, bandstack.getSubRxFrequency());

        // create and start the Metis interface

        metis.start();


        // start the panadapter update
        if (panadapterUpdate == null || !panadapterUpdate.isAlive()) {
            //Log.i("RadioActivity","onStart: new PanadapterUpdate");
            panadapterUpdate = new DisplayUpdate(panadapterView, waterfallView, meterView, metis, width);
        }
        panadapterUpdate.startTimer();

        // setup the touch listener
        panadapterView.setOnTouchListener(this);
        waterfallView.setOnTouchListener(this);

        // step spinner
        Spinner spinnerstep = (Spinner) findViewById(R.id.spinnerStep);
        ArrayList<String> steplist = new ArrayList<String>();
        steplist.add(0, "Step 10 Hz");
        steplist.add(1, "Step 50 Hz");
        steplist.add(2, "Step 100 Hz");
        steplist.add(3, "Step 500 Hz");
        steplist.add(4, "Step 1000 Hz");
        ArrayAdapter stepadapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, steplist);
        stepadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerstep.setAdapter(stepadapter);
        spinnerstep.setSelection(configuration.step);
        spinnerstep.setOnItemSelectedListener(new OnItemSelectedListener() {

            public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                configuration.step = pos;
                int step = 1000;
                switch (pos) {
                    case 0: // 10
                        step = 10;
                        break;
                    case 1: // 50
                        step = 50;
                        break;
                    case 2: // 100
                        step = 100;
                        break;
                    case 3: // 500
                        step = 500;
                        break;
                    case 4: // 1000
                        step = 1000;
                        break;
                }
                Button buttonMinus = (Button) findViewById(R.id.buttonMinus);
                buttonMinus.setText("-" + Integer.toString(step));
                Button buttonPlus = (Button) findViewById(R.id.buttonPlus);
                buttonPlus.setText("+" + Integer.toString(step));
            }

            public void onNothingSelected(AdapterView parent) {
            }
        });

        int step = 1000;
        switch (configuration.step) {
            case 0:
                step = 10;
                break;
            case 1:
                step = 50;
                break;
            case 2:
                step = 100;
                break;
            case 3:
                step = 500;
                break;
            case 4:
                step = 1000;
                break;
        }

        // tuning knob
        TuningKnobView tuningView = (TuningKnobView) findViewById(R.id.tuningKnob);
        if (configuration.tunaknob) {
            tuningView.setTuningKnobListener(new TuningKnobView.TuningKnobListener() {
                                                 @Override
                                                 public void onKnobChanged(int arg) {
                                                     switch (configuration.step) {
                                                         case 0:
                                                             increment = arg * 10;
                                                             break;
                                                         case 1:
                                                             increment = arg * 50;
                                                             break;
                                                         case 2:
                                                             increment = arg * 100;
                                                             break;
                                                         case 3:
                                                             increment = arg * 500;
                                                             break;
                                                         case 4:
                                                             increment = arg * 1000;
                                                             break;
                                                     }
                                                     BandStack bandstack = band.get();
                                                     if (configuration.subrx) {
                                                         bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() + increment));
                                                         wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                                                     } else {
                                                         bandstack.setFrequency(bandstack.getFrequency() + increment);
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
                        if (!locked) {
                            switch (configuration.step) {
                                case 0:
                                    increment = -10;
                                    break;
                                case 1:
                                    increment = -50;
                                    break;
                                case 2:
                                    increment = -100;
                                    break;
                                case 3:
                                    increment = -500;
                                    break;
                                case 4:
                                    increment = -1000;
                                    break;
                            }
                            BandStack bandstack = band.get();
                            if (configuration.subrx) {
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() + increment));
                                wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() + increment);
                                setFrequency(bandstack.getFrequency());
                            }
                            handler.removeCallbacks(updateTask);
                            handler.postAtTime(updateTask, SystemClock.uptimeMillis() + 500);
                        }
                    } else if (event.getAction() == MotionEvent.ACTION_UP) {
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
                            switch (configuration.step) {
                                case 0:
                                    increment = 10;
                                    break;
                                case 1:
                                    increment = 50;
                                    break;
                                case 2:
                                    increment = 100;
                                    break;
                                case 3:
                                    increment = 500;
                                    break;
                                case 4:
                                    increment = 1000;
                                    break;
                            }
                            BandStack bandstack = band.get();
                            if (configuration.subrx) {
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() + increment));
                                wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() + increment);
                                setFrequency(bandstack.getFrequency());
                            }
                            handler.removeCallbacks(updateTask);
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
                if(event.getAction()==MotionEvent.ACTION_DOWN) {
                    handler.sendEmptyMessage(metis.isTransmitting()?PTT_FALSE:PTT_TRUE);
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
                            wdsp.SetChannelState(txchannel, 0, 1);
                            //Log.i("RadioActivity","TUN: SetChannelState(0,1,0)");
                            wdsp.SetChannelState(rxchannel, 1, 0);
                            if (configuration.subrx) {
                                wdsp.SetChannelState(subrxchannel, 1, 0);
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
                            wdsp.SetChannelState(rxchannel, 0, 1);
                            if (configuration.subrx) {
                                wdsp.SetChannelState(subrxchannel, 0, 1);
                            }
                            //Log.i("RadioActivity","TUN: SetChannelState(1,1,0)");
                            wdsp.SetChannelState(txchannel, 1, 0);

                            //configuration.transmit=true;
                            //configuration.tuning=true;
                            metis.setTransmit(true, true);
                            v.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
                            /*
        					frequency.setTextColor(Color.RED);
        					*/
                        }
                    }
                }
                return false;
            }

        });

        // Band spinner
        final Spinner spinnerband = (Spinner) findViewById(R.id.spinnerBand);

        ArrayList<String> bandlist = new ArrayList<String>();
        Band[] bands = configuration.bands.getBands();
        for (int i = 0; i < bands.length; ++i) {
            bandlist.add(bands[i].getName());
        }
        ArrayAdapter bandadapter = new ArrayAdapter(this, android.R.layout.simple_spinner_item, bandlist);
        bandadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinnerband.setAdapter(bandadapter);
        spinnerband.setSelection(configuration.bands.getCurrent());
        spinnerband.setOnItemSelectedListener(new OnItemSelectedListener() {

            public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                Log.i("RadioActivity", "spinnerband: pos=" + pos);
                configuration.bands.set(pos);
                band = configuration.bands.get();
                BandStack bandstack = configuration.bands.get().get();
                if (bandstack != null && !locked) {

                    wdsp.SetRXABandpassFreqs(rxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                    wdsp.SetRXABandpassFreqs(subrxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                    wdsp.SetTXABandpassFreqs(txchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                    wdsp.SetRXAAGCMode(rxchannel, bandstack.getAGC());

                    if (configuration.subrx) {
                        configuration.subrx = false;
                        wdsp.SetChannelState(subrxchannel, 0, 0);
                        wdsp.SetRXAShiftRun(subrxchannel, 0);
                        wdsp.SetRXAAGCMode(subrxchannel, bandstack.getAGC());
                    }

                    setMode(bandstack.getMode());
                    setFrequency(bandstack.getFrequency());

                    Spinner spinnermode = (Spinner) findViewById(R.id.spinnerMode);
                    if (spinnermode != null) {
                        spinnermode.setSelection(bandstack.getMode());
                    }

    				Button squelch = (Button)findViewById(R.id.buttonSquelch);
    				squelch.getBackground().setColorFilter(band.getSquelch()?Color.RED:Color.LTGRAY,Mode.MULTIPLY);

                    if (band.get().getMode() == Modes.FMN) {
                        double v1=-2.0 * (double) band.getSquelchValue() / 100.0;
                        double v2=Math.pow(10.0,v1);
                        Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " "+v1+" "+v2);
                        wdsp.SetRXAFMSQThreshold (rxchannel, v2 );
                        wdsp.SetRXAFMSQRun(rxchannel, band.getSquelch() ? 1 : 0);
                    } else {
                        wdsp.SetRXAAMSQThreshold(rxchannel, (double) -band.getSquelchValue());
                        wdsp.SetRXAAMSQRun(rxchannel, band.getSquelch() ? 1 : 0);
                    }
                }
            }

            public void onNothingSelected(AdapterView parent) {
            }
        });

        // Lock button
        Button lock = (Button) findViewById(R.id.buttonLock);
        lock.setOnTouchListener(new OnTouchListener() {
            @Override
            public boolean onTouch(View v, MotionEvent event) {
                if (event.getAction() == MotionEvent.ACTION_DOWN) {
                    Spinner spinnerband = (Spinner) findViewById(R.id.spinnerBand);
                    Button buttonPlus = (Button) findViewById(R.id.buttonPlus);
                    Button buttonMinus = (Button) findViewById(R.id.buttonMinus);
                    Spinner spinnerStep = (Spinner) findViewById(R.id.spinnerStep);
                    TuningKnobView tuningView = (TuningKnobView) findViewById(R.id.tuningKnob);
                    if (locked) {
                        locked = false;
                        v.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                        spinnerband.setEnabled(true);
                        buttonPlus.setEnabled(true);
                        buttonMinus.setEnabled(true);
                        spinnerStep.setEnabled(true);
                        tuningView.setEnabled(true);
                    } else {
                        locked = true;
                        v.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
                        spinnerband.setEnabled(false);
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
                            wdsp.SetChannelState(subrxchannel, 0, 0);
                            wdsp.SetRXAShiftRun(subrxchannel, 0);
                            /*
                            subrxfrequency.setText("");
                            */
                        } else {
                            configuration.subrx = true;
                            BandStack bandstack = configuration.bands.get().get();
                            Log.i("RadioActivity", "subrx freq:" + bandstack.getSubRxFrequency());
                            if (bandstack.getSubRxFrequency() == 0) {
                                bandstack.setSubRxFrequency(bandstack.getFrequency());
                            }
                            bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency()));
                            wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            wdsp.SetRXAShiftRun(subrxchannel, 1);
                            wdsp.SetChannelState(subrxchannel, 1, 0);
                        }
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
                                    if(keyCode == KeyEvent.KEYCODE_BACK && event.getAction()==KeyEvent.ACTION_DOWN) {
                                        dialog.dismiss();
                                        return true;
                                    }
                                    return false;
                                }
                            });
                    final AlertDialog dialog = builder.create();
                    dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
                    builder.show();

                    f=0L;

                    final TextView message = (TextView)view.findViewById(R.id.textViewFrequency);

                    Button setFrequency=(Button)view.findViewById(R.id.buttonSetFrequency);
                    setFrequency.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            try {
                                if(f>0L) {
                                    // lookup band
                                    int b=configuration.bands.get(f);
                                    if(b==-1) {
                                        b=configuration.bands.get("AM");
                                    }
                                    if(b!=-1) {
                                        if(configuration.bands.getCurrent()!=b) {
                                            configuration.bands.set(b);
                                            spinnerband.setSelection(configuration.bands.getCurrent());
                                        }
                                        Band band = configuration.bands.get();
                                        BandStack bandstack = band.get();
                                        bandstack.setFrequency(f);
                                        setFrequency(f);
                                        if (bandstack != null && !locked) {

                                            wdsp.SetRXABandpassFreqs(rxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                                            wdsp.SetRXABandpassFreqs(subrxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                                            wdsp.SetTXABandpassFreqs(txchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                                            wdsp.SetRXAAGCMode(rxchannel, bandstack.getAGC());

                                            if (configuration.subrx) {
                                                configuration.subrx = false;
                                                wdsp.SetChannelState(subrxchannel, 0, 0);
                                                wdsp.SetRXAShiftRun(subrxchannel, 0);
                                                wdsp.SetRXAAGCMode(subrxchannel, bandstack.getAGC());
                                            }

                                            setMode(bandstack.getMode());
                                            setFrequency(bandstack.getFrequency());

                                            Spinner spinnermode = (Spinner) findViewById(R.id.spinnerMode);
                                            if (spinnermode != null) {
                                                spinnermode.setSelection(bandstack.getMode());
                                            }
                                        }
                                    }
                                }
                            } catch (Exception e) {
                                // invalid value
                                Log.i("RadioActivity","Invalid frequency: "+f);
                            }

                        }
                    });

                    Button button1=(Button)view.findViewById(R.id.button1);
                    button1.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+1;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button2=(Button)view.findViewById(R.id.button2);
                    button2.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+2;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button3=(Button)view.findViewById(R.id.button3);
                    button3.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+3;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button4=(Button)view.findViewById(R.id.button4);
                    button4.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+4;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button5=(Button)view.findViewById(R.id.button5);
                    button5.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+5;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button6=(Button)view.findViewById(R.id.button6);
                    button6.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+6;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button7=(Button)view.findViewById(R.id.button7);
                    button7.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+7;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button8=(Button)view.findViewById(R.id.button8);
                    button8.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+8;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button9=(Button)view.findViewById(R.id.button9);
                    button9.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=(f*10)+9;
                            message.setText(Long.toString(f));
                        }
                    });

                    Button button0=(Button)view.findViewById(R.id.button0);
                    button0.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=f*10;
                            if(f>0) {
                                message.setText(Long.toString(f));
                            }
                        }
                    });



                    Button buttonDelete=(Button)view.findViewById(R.id.buttonDelete);
                    buttonDelete.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=f/10;
                            if(f>0) {
                                message.setText(Long.toString(f));
                            }
                        }
                    });

                    Button buttonClear=(Button)view.findViewById(R.id.buttonClear);
                    buttonClear.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            f=0;
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
                            final View view = View.inflate(activity, R.layout.dialog_configure, null);

                            final AlertDialog.Builder builder = new AlertDialog.Builder(activity);

                            builder.setMessage("Configure Band " + band.getName())
                                    .setView(view)
                                    .setCancelable(false)
                                    .setPositiveButton("Close", new DialogInterface.OnClickListener() {
                                        public void onClick(DialogInterface dialog, int id) {
                                        }
                                    });
                            AlertDialog dialog = builder.create();

                            dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);

                            builder.show();

                            final NumberPicker spectrumHigh = (NumberPicker) view.findViewById(R.id.numberPickerSpectrumHigh);
                            spectrumHigh.setMaximum(100);
                            spectrumHigh.setMinimum(-200);
                            spectrumHigh.setIncrement(10);
                            spectrumHigh.setValue(configuration.spectrumHigh);
                            spectrumHigh.setOnNumberPickerChangedListener(new OnNumberPickerChangedListener() {
                                @Override
                                public void numberPickerChanged(int value) {
                                    if (value > configuration.spectrumLow)
                                        configuration.spectrumHigh = value;
                                }
                            });

                            final NumberPicker spectrumLow = (NumberPicker) view.findViewById(R.id.numberPickerSpectrumLow);
                            spectrumLow.setMaximum(100);
                            spectrumLow.setMinimum(-200);
                            spectrumLow.setIncrement(10);
                            spectrumLow.setValue(configuration.spectrumLow);
                            spectrumLow.setOnNumberPickerChangedListener(new OnNumberPickerChangedListener() {
                                @Override
                                public void numberPickerChanged(int value) {
                                    if (value < configuration.spectrumHigh)
                                        configuration.spectrumLow = value;
                                }
                            });

                            final NumberPicker waterfallHigh = (NumberPicker) view.findViewById(R.id.numberPickerWaterfallHigh);
                            waterfallHigh.setMaximum(100);
                            waterfallHigh.setMinimum(-200);
                            waterfallHigh.setIncrement(1);
                            waterfallHigh.setValue(configuration.waterfallHigh);
                            waterfallHigh.setOnNumberPickerChangedListener(new OnNumberPickerChangedListener() {
                                @Override
                                public void numberPickerChanged(int value) {
                                    if (value > configuration.waterfallLow)
                                        configuration.waterfallHigh = value;
                                }
                            });



                            final NumberPicker waterfallLow = (NumberPicker) view.findViewById(R.id.numberPickerWaterfallLow);
                            waterfallLow.setMaximum(100);
                            waterfallLow.setMinimum(-200);
                            waterfallLow.setIncrement(1);
                            waterfallLow.setValue(configuration.waterfallLow);
                            waterfallLow.setOnNumberPickerChangedListener(new OnNumberPickerChangedListener() {
                                @Override
                                public void numberPickerChanged(int value) {
                                    if (value < configuration.waterfallHigh)
                                        configuration.waterfallLow = value;
                                }
                            });

                            if(configuration.waterfallAutomatic) {
                                waterfallHigh.setVisibility(View.GONE);
                                waterfallLow.setVisibility(View.GONE);
                                TextView textViewWaterfallHigh=(TextView)view.findViewById(R.id.textViewWaterfallHigh);
                                textViewWaterfallHigh.setVisibility(View.GONE);
                                TextView textViewWaterfallLow=(TextView)view.findViewById(R.id.textViewWaterfallLow);
                                textViewWaterfallLow.setVisibility(View.GONE);
                            }

                            final TextView textViewmicgain = (TextView) view.findViewById(R.id.textViewMicGain);
                            textViewmicgain.setText("Mic Gain: " + (int) (configuration.micgain * 100.0F) + "%");

                            final SeekBar seekbarmicgain = (SeekBar) view.findViewById(R.id.seekBarMicGain);
                            seekbarmicgain.setMax(150);
                            seekbarmicgain.setProgress((int) (configuration.micgain * 100.0F));
                            seekbarmicgain.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                                @Override
                                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                    float gain = (float) progress / 100.0F;
                                    configuration.micgain = gain;
                                    textViewmicgain.setText("Mic Gain: " + (int) (configuration.micgain * 100.0F) + "%");
                                }

                                @Override
                                public void onStartTrackingTouch(SeekBar seekBar) {

                                }

                                @Override
                                public void onStopTrackingTouch(SeekBar seekBar) {

                                }
                            });

                            final TextView textViewrfpower = (TextView) view.findViewById(R.id.textViewRFPower);
                            textViewrfpower.setText("RF Power: " + (int) (configuration.rfgain * 100.0F) + "%");

                            SeekBar seekbarrfpower = (SeekBar) view.findViewById(R.id.seekBarRFPower);
                            seekbarrfpower.setMax(255);
                            seekbarrfpower.setProgress((int) (255.0F * configuration.rfgain));
                            seekbarrfpower.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                    float gain = (float) progress / 255.0F;
                                    //Log.i("RadioActivity","rf gain="+gain);
                                    configuration.rfgain = gain;
                                    textViewrfpower.setText("RF Power: " + (int) (configuration.rfgain * 100.0F) + "%");
                                }

                                public void onStartTrackingTouch(SeekBar seekBar) {
                                }

                                public void onStopTrackingTouch(SeekBar seekBar) {
                                }
                            });

                            final TextView textViewtunegain = (TextView) view.findViewById(R.id.textViewTunePower);
                            textViewtunegain.setText("Tune Power: " + (int) (configuration.tunegain * 100.0F) + "%");

                            SeekBar seekbartunegain = (SeekBar) view.findViewById(R.id.seekBarTunePower);
                            seekbartunegain.setMax(255);
                            seekbartunegain.setProgress((int) (255.0F * configuration.tunegain));
                            seekbartunegain.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                    float gain = (float) progress / 255.0F;
                                    //Log.i("RadioActivity","rf gain="+gain);
                                    configuration.tunegain = gain;
                                    textViewtunegain.setText("Tune Power: " + (int) (configuration.tunegain * 100.0F) + "%");
                                }

                                public void onStartTrackingTouch(SeekBar seekBar) {
                                }

                                public void onStopTrackingTouch(SeekBar seekBar) {
                                }
                            });
                            final TextView textViewafgain = (TextView) view.findViewById(R.id.textViewAFGain);
                            textViewafgain.setText("AF Gain: " + (int) (configuration.afgain * 100.0F) + "%");

                            SeekBar seekbarafgain = (SeekBar) view.findViewById(R.id.seekBarAFGain);
                            seekbarafgain.setMax(255);
                            seekbarafgain.setProgress((int) (255.0F * configuration.afgain));
                            seekbarafgain.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                    float gain = (float) progress / 255.0F;
                                    //Log.i("RadioActivity","af gain="+gain);
                                    configuration.afgain = gain;
                                    textViewafgain.setText("AF Gain: " + (int) (configuration.afgain * 100.0F) + "%");
                                }

                                public void onStartTrackingTouch(SeekBar seekBar) {
                                }

                                public void onStopTrackingTouch(SeekBar seekBar) {
                                }
                            });

                            final TextView textViewTitle = (TextView) view.findViewById(R.id.textViewTitle);
                            textViewTitle.setText("Configure Band: " + band.getName());

                            final TextView textViewtxgain = (TextView) view.findViewById(R.id.textViewTxGain);
                            textViewtxgain.setText("TX Gain: " + (int) (band.getTxGain() * 100.0F) + "%");

                            SeekBar seekbartxgain = (SeekBar) view.findViewById(R.id.seekBarTxGain);
                            seekbartxgain.setMax(255);
                            seekbartxgain.setProgress((int) (255.0F * band.getTxGain()));
                            seekbartxgain.setOnSeekBarChangeListener(new OnSeekBarChangeListener() {
                                public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                                    float gain = (float) progress / 255.0F;
                                    //Log.i("RadioActivity","tx gain="+gain);
                                    band.setTxGain(gain);
                                    textViewtxgain.setText("TX Gain: " + (int) (band.getTxGain() * 100.0F) + "%");
                                    //Log.i("RadioActivity","TX Gain: "+(int)(band.getTxGain()*100.0F)+"%");
                                }

                                public void onStartTrackingTouch(SeekBar seekBar) {
                                }

                                public void onStopTrackingTouch(SeekBar seekBar) {
                                }
                            });


                            RadioGroup txgroup = (RadioGroup) view.findViewById(R.id.radioGroupTx);
                            if (configuration.radio != Configuration.METIS_PENELOPE &&
                                    configuration.radio != Configuration.METIS_PENNYLANE) {
                                RadioButton rb;
                                switch (band.get().getTxAntenna()) {
                                    case 0:
                                        rb = (RadioButton) view.findViewById(R.id.radioAnt1);
                                        rb.setChecked(true);
                                        break;
                                    case 1:
                                        rb = (RadioButton) view.findViewById(R.id.radioAnt2);
                                        rb.setChecked(true);
                                        break;
                                    case 2:
                                        if (configuration.radio != Configuration.METIS_PENELOPE &&
                                                configuration.radio != Configuration.METIS_PENNYLANE) {
                                            switch (band.get().getTxAntenna()) {
                                                case 0:
                                                    rb = (RadioButton) view.findViewById(R.id.radioAnt1);
                                                    rb.setChecked(true);
                                                    break;
                                                case 1:
                                                    rb = (RadioButton) view.findViewById(R.id.radioAnt2);
                                                    rb.setChecked(true);
                                                    break;
                                                case 2:
                                                    rb = (RadioButton) view.findViewById(R.id.radioAnt3);
                                                    rb.setChecked(true);
                                                    break;
                                            }

                                            txgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                                                @Override
                                                public void onCheckedChanged(RadioGroup group,
                                                                             int checkedId) {
                                                    //Log.i("RadioActivity","Tx Group:"+checkedId);
                                                    if (checkedId == R.id.radioAnt1) {
                                                        //Log.i("RadioActivity","ANT 1");
                                                        band.get().setTxAntenna(0);
                                                    } else if (checkedId == R.id.radioAnt2) {
                                                        //Log.i("RadioActivity","ANT 2");
                                                        band.get().setTxAntenna(1);
                                                    } else if (checkedId == R.id.radioAnt3) {
                                                        //Log.i("RadioActivity","ANT 3");
                                                        band.get().setTxAntenna(2);
                                                    }
                                                }
                                            });
                                        } else {
                                            txgroup.setEnabled(false);
                                        }
                                        rb = (RadioButton) view.findViewById(R.id.radioAnt3);
                                        rb.setChecked(true);
                                        break;
                                }

                                txgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                                    @Override
                                    public void onCheckedChanged(RadioGroup group,
                                                                 int checkedId) {
                                        //Log.i("RadioActivity","Tx Group:"+checkedId);
                                        if (checkedId == R.id.radioAnt1) {
                                            //Log.i("RadioActivity","ANT 1");
                                            band.get().setTxAntenna(0);
                                        } else if (checkedId == R.id.radioAnt2) {
                                            //Log.i("RadioActivity","ANT 2");
                                            band.get().setTxAntenna(1);
                                        } else if (checkedId == R.id.radioAnt3) {
                                            //Log.i("RadioActivity","ANT 3");
                                            band.get().setTxAntenna(2);
                                        }
                                    }
                                });
                            } else {
                                TextView tv = (TextView) view.findViewById(R.id.textViewTx);
                                tv.setVisibility(View.GONE);
                                RadioButton rb = (RadioButton) view.findViewById(R.id.radioAnt1);
                                rb.setVisibility(View.GONE);
                                rb = (RadioButton) view.findViewById(R.id.radioAnt2);
                                rb.setVisibility(View.GONE);
                                rb = (RadioButton) view.findViewById(R.id.radioAnt3);
                                rb.setVisibility(View.GONE);
                                txgroup.setVisibility(View.GONE);
                            }


                            RadioGroup rxgroup = (RadioGroup) view.findViewById(R.id.radioGroupRx);
                            if (configuration.radio != Configuration.METIS_PENELOPE &&
                                    configuration.radio != Configuration.METIS_PENNYLANE) {
                                RadioButton rb;
                                if (configuration.radio == Configuration.ANGELIA_ANAN100D || configuration.radio == Configuration.ORION_ANAN200D) {
                                    rb = (RadioButton) view.findViewById(R.id.radioRX1);
                                    rb.setText("EXT 2");
                                    rb = (RadioButton) view.findViewById(R.id.radioRX2);
                                    rb.setText("EXT 1");
                                }

                                switch (band.get().getRxAntenna()) {
                                    case 0:
                                        rb = (RadioButton) view.findViewById(R.id.radioNone);
                                        rb.setChecked(true);
                                        break;
                                    case 1:
                                        rb = (RadioButton) view.findViewById(R.id.radioRX1);
                                        rb.setChecked(true);
                                        break;
                                    case 2:
                                        rb = (RadioButton) view.findViewById(R.id.radioRX2);
                                        rb.setChecked(true);
                                        break;
                                    case 3:
                                        rb = (RadioButton) view.findViewById(R.id.radioRXXV);
                                        rb.setChecked(true);
                                        break;
                                }

                                rxgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                                    @Override
                                    public void onCheckedChanged(RadioGroup group,
                                                                 int checkedId) {
                                        //Log.i("RadioActivity","Rx Group:"+checkedId);
                                        if (checkedId == R.id.radioNone) {
                                            //Log.i("RadioActivity","NONE");
                                            band.get().setRxAntenna(0);
                                        } else if (checkedId == R.id.radioRX1) {
                                            //Log.i("RadioActivity","RX 1");
                                            band.get().setRxAntenna(1);
                                        } else if (checkedId == R.id.radioRX2) {
                                            //Log.i("RadioActivity","RX 2");
                                            band.get().setRxAntenna(2);
                                        } else if (checkedId == R.id.radioRXXV) {
                                            //Log.i("RadioActivity","RX XV");
                                            band.get().setRxAntenna(3);
                                        }
                                    }
                                });
                            } else {
                                TextView tv = (TextView) view.findViewById(R.id.textViewRx);
                                tv.setVisibility(View.GONE);
                                RadioButton rb = (RadioButton) view.findViewById(R.id.radioNone);
                                rb.setVisibility(View.GONE);
                                rb = (RadioButton) view.findViewById(R.id.radioRX1);
                                rb.setVisibility(View.GONE);
                                rb = (RadioButton) view.findViewById(R.id.radioRX2);
                                rb.setVisibility(View.GONE);
                                rb = (RadioButton) view.findViewById(R.id.radioRXXV);
                                rb.setVisibility(View.GONE);
                                rxgroup.setVisibility(View.GONE);
                            }

                            Spinner spinneragc = (Spinner) view.findViewById(R.id.spinnerAGC);
                            ArrayList<String> agclist = new ArrayList<String>();
                            agclist.add("AGC Off");
                            agclist.add("AGC Long");
                            agclist.add("AGC Slow");
                            agclist.add("AGC Med");
                            agclist.add("AGC Fast");
                            ArrayAdapter agcadapter = new ArrayAdapter(activity, android.R.layout.simple_spinner_item, agclist);
                            agcadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                            spinneragc.setAdapter(agcadapter);
                            BandStack bandstack = configuration.bands.get().get();
                            spinneragc.setSelection(bandstack.getAGC());
                            spinneragc.setOnItemSelectedListener(new OnItemSelectedListener() {

                                public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                                    //Log.i("RadioActivity","spinneragc: pos="+pos);
                                    if (!locked) {
                                        BandStack bandstack = configuration.bands.get().get();
                                        bandstack.setAGC(pos);

                                        wdsp.SetRXAAGCMode(rxchannel, pos);
                                        wdsp.SetRXAAGCMode(subrxchannel, pos);
                                    }
                                }

                                public void onNothingSelected(AdapterView parent) {
                                }

                            });


                            CheckBox cbocrx1 = (CheckBox) view.findViewById(R.id.checkBoxOCRx1);
                            cbocrx1.setChecked((band.getOCRx() & 0x01) == 0x01);
                            cbocrx1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x01));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xFE));
                                    }
                                }
                            });

                            CheckBox cbocrx2 = (CheckBox) view.findViewById(R.id.checkBoxOCRx2);
                            cbocrx2.setChecked((band.getOCRx() & 0x02) == 0x02);
                            cbocrx2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x02));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xFD));
                                    }
                                }
                            });

                            CheckBox cbocrx3 = (CheckBox) view.findViewById(R.id.checkBoxOCRx3);
                            cbocrx3.setChecked((band.getOCRx() & 0x04) == 0x04);
                            cbocrx3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x04));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xFB));
                                    }
                                }
                            });

                            CheckBox cbocrx4 = (CheckBox) view.findViewById(R.id.checkBoxOCRx4);
                            cbocrx4.setChecked((band.getOCRx() & 0x08) == 0x08);
                            cbocrx4.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x08));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xF7));
                                    }
                                }
                            });

                            CheckBox cbocrx5 = (CheckBox) view.findViewById(R.id.checkBoxOCRx5);
                            cbocrx5.setChecked((band.getOCRx() & 0x10) == 0x10);
                            cbocrx5.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x10));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xEF));
                                    }
                                }
                            });

                            CheckBox cbocrx6 = (CheckBox) view.findViewById(R.id.checkBoxOCRx6);
                            cbocrx6.setChecked((band.getOCRx() & 0x20) == 0x20);
                            cbocrx6.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x20));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xDF));
                                    }
                                }
                            });

                            CheckBox cbocrx7 = (CheckBox) view.findViewById(R.id.checkBoxOCRx7);
                            cbocrx7.setChecked((band.getOCRx() & 0x40) == 0x40);
                            cbocrx7.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCRx((byte) (band.getOCRx() | 0x40));
                                    } else {
                                        band.setOCRx((byte) (band.getOCRx() & 0xBF));
                                    }
                                }
                            });

                            CheckBox cboctx1 = (CheckBox) view.findViewById(R.id.checkBoxOCTx1);
                            cboctx1.setChecked((band.getOCTx() & 0x01) == 0x01);
                            cboctx1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x01));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xFE));
                                    }
                                }
                            });

                            CheckBox cboctx2 = (CheckBox) view.findViewById(R.id.checkBoxOCTx2);
                            cboctx2.setChecked((band.getOCTx() & 0x02) == 0x02);
                            cboctx2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x02));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xFD));
                                    }
                                }
                            });

                            CheckBox cboctx3 = (CheckBox) view.findViewById(R.id.checkBoxOCTx3);
                            cboctx3.setChecked((band.getOCTx() & 0x04) == 0x04);
                            cboctx3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x04));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xFB));
                                    }
                                }
                            });

                            CheckBox cboctx4 = (CheckBox) view.findViewById(R.id.checkBoxOCTx4);
                            cboctx4.setChecked((band.getOCTx() & 0x08) == 0x08);
                            cboctx4.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x08));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xF7));
                                    }
                                }
                            });

                            CheckBox cboctx5 = (CheckBox) view.findViewById(R.id.checkBoxOCTx5);
                            cboctx5.setChecked((band.getOCTx() & 0x10) == 0x10);
                            cboctx5.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x10));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xEF));
                                    }
                                }
                            });

                            CheckBox cboctx6 = (CheckBox) view.findViewById(R.id.checkBoxOCTx6);
                            cboctx6.setChecked((band.getOCTx() & 0x20) == 0x20);
                            cboctx6.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x20));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xDF));
                                    }
                                }
                            });

                            CheckBox cboctx7 = (CheckBox) view.findViewById(R.id.checkBoxOCTx7);
                            cboctx7.setChecked((band.getOCTx() & 0x40) == 0x40);
                            cboctx7.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                                @Override
                                public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                                    if (arg1) {
                                        band.setOCTx((byte) (band.getOCTx() | 0x40));
                                    } else {
                                        band.setOCTx((byte) (band.getOCTx() & 0xBF));
                                    }
                                }
                            });

                        }
                        return false;
                    }
                });

                // Mode spinner
                Spinner spinnermode = (Spinner) findViewById(R.id.spinnerMode);
                if (spinnermode != null) {
                    ArrayList<String> modelist = new ArrayList<String>();
                    for (int i = 0; i < Modes.modes.length; ++i) {
                        modelist.add(Modes.modes[i]);
                    }
                    ArrayAdapter modeadapter = new ArrayAdapter(activity, android.R.layout.simple_spinner_item, modelist);
                    modeadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    spinnermode.setAdapter(modeadapter);
                    spinnermode.setSelection(bandstack.getMode());
                    spinnermode.setOnItemSelectedListener(new OnItemSelectedListener() {

                        public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                            //Log.i("RadioActivity","spinnermode: pos="+pos);
                            if (!locked) {
                                BandStack bandstack = configuration.bands.get().get();
                                bandstack.setMode(pos);
                                bandstack.setFilterLow(Modes.filterLow[pos]);
                                bandstack.setFilterHigh(Modes.filterHigh[pos]);

                                wdsp.SetRXABandpassFreqs(rxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                                wdsp.SetRXABandpassFreqs(subrxchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());
                                wdsp.SetTXABandpassFreqs(txchannel, bandstack.getFilterLow(), bandstack.getFilterHigh());

                            }
                    /*
                    if(subrxfrequency!=null) {
                        subrxfrequency.setText("");
                    }
                    */
                        }

                        public void onNothingSelected(AdapterView parent) {
                        }

                    });
                } else {
                    Log.i("RadioActivity", "spinnermode is null");
                }

		// Squelch button
		final Button squelch = (Button)findViewById(R.id.buttonSquelch);
		squelch.getBackground().setColorFilter(band.getSquelch()?Color.RED:Color.LTGRAY,Mode.MULTIPLY);
		squelch.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if(event.getAction()==MotionEvent.ACTION_DOWN) {
                    final View view = View.inflate(activity, R.layout.dialog_squelch, null);

                    Band band = configuration.bands.get();
                    final CheckBox checkbox = (CheckBox) view.findViewById(R.id.checkBoxSquelch);
                    checkbox.setChecked(band.getSquelch());
                    if(band.get().getMode() == Modes.FMN) {
                        if(band.getSquelchValue()>100) band.setSquelchValue(50);
                        checkbox.setText("Squelch: " + (int) (band.getSquelchValue()) );
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
		    		seekbar.setOnSeekBarChangeListener( new OnSeekBarChangeListener() {
                        public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                            Log.i("RadioActivity", "squelch=" + progress);
                            Band band = configuration.bands.get();
                            band.setSquelchValue(progress);
                            if (band.get().getMode() == Modes.FMN) {
                                double v1=-2.0 * (double) band.getSquelchValue() / 100.0;
                                double v2=Math.pow(10.0,v1);
                                Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " "+v1+" "+v2);
                                wdsp.SetRXAFMSQThreshold (rxchannel, v2 );
                                checkbox.setText("Squelch: " + (int) (band.getSquelchValue()));
                            } else {
                                wdsp.SetRXAAMSQThreshold(rxchannel, (double) -band.getSquelchValue());
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
                            Log.i("RadioActivity","Squelch checkbox "+isChecked);
                            Band band=configuration.bands.get();
							squelch.getBackground().setColorFilter(isChecked?Color.RED:Color.LTGRAY,Mode.MULTIPLY);
							band.setSquelch(isChecked);
								if(band.get().getMode()==Modes.FMN) {
									if(isChecked) {
                                        double v1=-2.0 * (double) band.getSquelchValue() / 100.0;
                                        double v2=Math.pow(10.0,v1);
                                        Log.i("RadioActivity", "FM squelch set to " + band.getSquelchValue() + " "+v1+" "+v2);
                                        wdsp.SetRXAFMSQThreshold (rxchannel, v2 );
									}
									wdsp.SetRXAFMSQRun (rxchannel, isChecked?1:0);
		    		        	} else {
		    		        		if(isChecked) {
                                        Log.i("RadioActivity","AM squelch set to "+band.getSquelchValue());
										wdsp.SetRXAAMSQThreshold(rxchannel, (double) -band.getSquelchValue());
                                        checkbox.setText("Squelch: " + (int) -(band.getSquelchValue()) + " dBm");
									}
		    		        		wdsp.SetRXAAMSQRun (rxchannel, isChecked?1:0);
		    		        	}

						}		
		    		});
				}
				return false;
			}		
		});


        /*
		// AF Gain button
		Button afgain = (Button)findViewById(R.id.buttonAfGain);
		afgain.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if(event.getAction()==MotionEvent.ACTION_DOWN) {
					final View view = View.inflate(activity, R.layout.dialog_afgain, null);
					
					final TextView textView=(TextView)view.findViewById(R.id.textViewAfGain);
					textView.setText("AF Gain: "+(int)(configuration.afgain*100.0F)+"%");
					
					final AlertDialog.Builder builder = new AlertDialog.Builder(activity);

					builder.setMessage("Set AF Gain")
					       .setView(view)
					       .setCancelable(false)
					       .setPositiveButton("Close", new DialogInterface.OnClickListener() {
						           public void onClick(DialogInterface dialog, int id) {
						           }
					           });
					AlertDialog dialog=builder.create();
				
					dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
					
					builder.show();
					
					SeekBar seekbar=(SeekBar)view.findViewById(R.id.seekBarAfGain);
		    		seekbar.setMax(255);
		    		seekbar.setProgress((int)(255.0F*configuration.afgain));
		    		seekbar.setOnSeekBarChangeListener( new OnSeekBarChangeListener() {
		    		    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
		    		    	float gain=(float)progress/255.0F;
		    		    	//Log.i("RadioActivity","af gain="+gain);
		    		        configuration.afgain=gain;
		    		        textView.setText("AF Gain: "+(int)(configuration.afgain*100.0F)+"%");
		    		    }

		    		    public void onStartTrackingTouch(SeekBar seekBar) {
		    		    }

		    		    public void onStopTrackingTouch(SeekBar seekBar) {
		    		    }
		    		});
				}

				return false;
			}		
		});
		*/

        /*
		// RF Gain button
		Button rfgain = (Button)findViewById(R.id.buttonRfGain);
		rfgain.setOnTouchListener(new OnTouchListener() {
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				if(event.getAction()==MotionEvent.ACTION_DOWN) {
					
					final View view = View.inflate(activity, R.layout.dialog_rfgain, null);
					
					final TextView textView=(TextView)view.findViewById(R.id.textViewRfGain);
					textView.setText("RF Power: "+(int)(configuration.rfgain*100.0F)+"%");
					
					final TextView tuneTextView=(TextView)view.findViewById(R.id.textViewTuneGain);
					tuneTextView.setText("Tune Power: "+(int)(configuration.tunegain*100.0F)+"%");
					
					final AlertDialog.Builder builder = new AlertDialog.Builder(activity);
					builder.setMessage("Set RF Power")
					       .setView(view)
					       .setCancelable(false)
					       .setPositiveButton("Close", new DialogInterface.OnClickListener() {
						           public void onClick(DialogInterface dialog, int id) {
						           }
					           });SeekBar seekbar=(SeekBar)view.findViewById(R.id.seekBarRfGain);
					    		seekbar.setMax(255);
					    		seekbar.setProgress((int)(255.0F*configuration.rfgain));
					    		seekbar.setOnSeekBarChangeListener( new OnSeekBarChangeListener() {
					    		    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
					    		    	float gain=(float)progress/255.0F;
					    		    	//Log.i("RadioActivity","rf gain="+gain);
					    		        configuration.rfgain=gain;
					    		        textView.setText("RF Power: "+(int)(configuration.rfgain*100.0F)+"%");
					    		    }

					    		    public void onStartTrackingTouch(SeekBar seekBar) {
					    		    }

					    		    public void onStopTrackingTouch(SeekBar seekBar) {
					    		    }
					    		});
					AlertDialog dialog=builder.create();
				
					dialog.requestWindowFeature(Window.FEATURE_NO_TITLE);
					
					builder.show();
					
					SeekBar seekbarrfgain=(SeekBar)view.findViewById(R.id.seekBarRfGain);
		    		seekbarrfgain.setMax(255);
		    		seekbarrfgain.setProgress((int)(255.0F*configuration.rfgain));
		    		seekbarrfgain.setOnSeekBarChangeListener( new OnSeekBarChangeListener() {
		    		    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
		    		    	float gain=(float)progress/255.0F;
		    		    	//Log.i("RadioActivity","rf gain="+gain);
		    		        configuration.rfgain=gain;
		    		        textView.setText("RF Power: "+(int)(configuration.rfgain*100.0F)+"%");
		    		    }

		    		    public void onStartTrackingTouch(SeekBar seekBar) {
		    		    }

		    		    public void onStopTrackingTouch(SeekBar seekBar) {
		    		    }
		    		});
		    		
		    		SeekBar seekbartune=(SeekBar)view.findViewById(R.id.seekBarTuneGain);
		    		seekbartune.setMax(255);
		    		seekbartune.setProgress((int)(255.0F*configuration.tunegain));
		    		seekbartune.setOnSeekBarChangeListener( new OnSeekBarChangeListener() {
		    		    public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
		    		    	float gain=(float)progress/255.0F;
		    		    	//Log.i("RadioActivity","tune gain="+gain);
		    		        configuration.tunegain=gain;
		    		        tuneTextView.setText("Tune Power: "+(int)(configuration.tunegain*100.0F)+"%");
		    		    }

		    		    public void onStartTrackingTouch(SeekBar seekBar) {
		    		    }

		    		    public void onStopTrackingTouch(SeekBar seekBar) {
		    		    }
		    		});
				}

				return false;
			}		
		});
		*/

        /*
		// Debug button
		Button debug = (Button)findViewById(R.id.buttonDebug);
		debug.setOnTouchListener(new OnTouchListener() {

			@Override
			public boolean onTouch(View v, MotionEvent event) {

				if(event.getAction()==MotionEvent.ACTION_DOWN) {
					if(cpu==null) {
		    			cpu=new CPU();
		    		}
					
					StringBuffer text=new StringBuffer();
					text.append(configuration.discovered.getDeviceName());
					text.append(": ");
					text.append(format.format((float)metis.metis_software_version/10.0F));
					switch(configuration.discovered.getDevice()) {
					case Discovered.DEVICE_METIS:
						text.append(", Mercury: ");
						text.append(format.format((float)metis.mercury_software_version/10.0F));
						if(configuration.exciter==Configuration.PENELOPE) {
							text.append(", Penelope: ");
						} else {
							text.append(", PennyLane: ");
						}
						text.append(format.format((float)metis.penelope_software_version/10.0F));
						break;
					}
					text.append("\n");
					text.append(cpu.toString());
		    		Toast toast = Toast.makeText(activity.getApplicationContext(), text.toString(), Toast.LENGTH_LONG);
		    		toast.show();
				}
				return false;
			}
			
		});

		*/

                // get and configure the frequency view
        /*
		frequency=(TextView)findViewById(R.id.textViewFrequency);
        frequency.setTextColor(Color.GREEN);
        */
                setFrequency(configuration.bands.get().get().getFrequency());

                // get and set the SubRX frequency view
        /*
        subrxfrequency=(TextView)findViewById(R.id.TextViewSubRxFrequency);
        if(subrxfrequency!=null) {
            subrxfrequency.setTextColor(Color.GRAY);
        }
        */
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

                if (panadapterUpdate.isRunning()) {
                    //Log.i("RadioActivity","onPause: panadapterUpdate.terminate");
                    panadapterUpdate.terminate();
                }

                if (metis.isRunning()) {
                    //Log.i("RadioActivity","onPause: metis.terminate");
                    metis.terminate();
                }

                wdsp.CloseChannel(rxchannel);
                wdsp.CloseChannel(subrxchannel);
                wdsp.CloseChannel(txchannel);

                try {
                    FileOutputStream fos = openFileOutput(filename, Context.MODE_PRIVATE);
                    ObjectOutputStream oos = new ObjectOutputStream(fos);
                    oos.writeObject(Configuration.getInstance());
                    oos.close();
                    fos.close();
                } catch (Exception e) {
                    Log.i("RadioActivity", "onPause: " + e.toString());
                }

                //temp fix
                //System.exit(0);
            }

            @Override
            public void onBackPressed() {
                Log.i("RadioActivity", "onbackPressed");
                super.onBackPressed();

            }

            public boolean onKeyDown(int keyCode, KeyEvent event) {
                if (keyCode == KeyEvent.KEYCODE_BACK) {
                    AlertDialog alertDialog = new AlertDialog.Builder(this).create();
                    alertDialog.setTitle("HPSDR");
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
                            int freqincrement = 100;
                            //Log.i("RadioActivity","onGenericMotionEvent vscroll:"+event.getAxisValue(MotionEvent.AXIS_VSCROLL));
                            if (event.getAxisValue(MotionEvent.AXIS_VSCROLL) < 0.0f) {
                                freqincrement = -100;
                            } else {
                                freqincrement = 100;
                            }
                            if (configuration.subrx) {
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() - freqincrement));
                                wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() + freqincrement);
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
                            bandstack.setSubRxFrequency(bandstack.getSubRxFrequency() + increment);
                            setSubRxFrequency(bandstack.getSubRxFrequency());
                            wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            handler.postAtTime(this, SystemClock.uptimeMillis() + 100);
                        } else {
                            bandstack.setFrequency(bandstack.getFrequency() + increment);
                            setFrequency(bandstack.getFrequency());
                            handler.postAtTime(this, SystemClock.uptimeMillis() + 100);
                        }
                    }
                }
            };

            public boolean onTouch(View v, MotionEvent event) {

                if (!locked) {
                    double hzperpixel = configuration.samplerate / (double) panadapterView.getWidth();
                    BandStack bandstack = configuration.bands.get().get();
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
                                bandstack.setSubRxFrequency(setSubRxFrequency(bandstack.getSubRxFrequency() - (freqincrement / 100 * 100)));
                                wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
                            } else {
                                bandstack.setFrequency(bandstack.getFrequency() + (freqincrement / 100 * 100));
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

                                // make 100hz resolution
                                f = f / 100 * 100;

                                if (configuration.subrx) {
                                    bandstack.setSubRxFrequency(setSubRxFrequency(f));
                                    wdsp.SetRXAShiftFreq(subrxchannel, (double) (bandstack.getSubRxFrequency() - bandstack.getFrequency()));
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
            }

            private long setSubRxFrequency(long f) {
                BandStack bandstack = configuration.bands.get().get();
                long lowfreq = bandstack.getFrequency() - (long) (configuration.samplerate / 2.0);
                if (bandstack.getFilterLow() < 0) {
                    lowfreq = lowfreq + bandstack.getFilterLow();
                }
                long highfreq = bandstack.getFrequency() + (long) (configuration.samplerate / 2.0);
                if (bandstack.getFilterHigh() > 0) {
                    highfreq = highfreq - bandstack.getFilterHigh();
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
                return f;
            }

            private void setMode(int mode) {
                wdsp.SetRXAMode(rxchannel, mode);
                wdsp.SetRXAMode(subrxchannel, mode);
                wdsp.SetTXAMode(txchannel, mode);
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

                        Log.i("RadioActivity", "processPTT: SetChannelState(rxchannel,0,1)");
                        wdsp.SetChannelState(rxchannel, 0, 1);
                        if (configuration.subrx) {
                            Log.i("RadioActivity", "processPTT: SetChannelState(subrxchannel,0,1)");
                            wdsp.SetChannelState(subrxchannel, 0, 1);
                        }
                        Log.i("RadioActivity", "processPTT: SetChannelState(txchannel,1,0)");
                        wdsp.SetChannelState(txchannel, 1, 0);

                        Log.i("RadioActivity", "processPTT: metis.setTransmit(true,false)");
                        metis.setTransmit(true, false);
                        if (configuration.micsource == Configuration.MIC_SOURCE_LOCAL) {
                            microphone = new Microphone(metis, txchannel);
                            microphone.start();
                        }
                        Log.i("RadioActivity", "Set PTT button red");
                        ptt.getBackground().setColorFilter(Color.RED, Mode.MULTIPLY);
                    }
                } else {
                    // stop transmitting
                    wdsp.SetChannelState(txchannel, 0, 1);
                    wdsp.SetChannelState(rxchannel, 1, 0);
                    if (configuration.subrx) {
                        wdsp.SetChannelState(subrxchannel, 1, 0);
                    }
                    metis.setTransmit(false, false);
                    ptt.getBackground().setColorFilter(Color.LTGRAY, Mode.MULTIPLY);
                }
            }

            private static final int PTT_FALSE = 0;
            private static final int PTT_TRUE = 1;

            private Activity activity;

            private CPU cpu;

            private Configuration configuration;

            private Metis metis;

            private WDSP wdsp;
            private int rxchannel = 0;
            private int txchannel = 1;
            private int subrxchannel = 2;

            private Microphone microphone;

            private PanadapterView panadapterView;
            private WaterfallView waterfallView;
            //private MeterView meterView;
            private MeterView meterView;

            private DisplayUpdate panadapterUpdate;

            private Band band;
            private long increment;

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
