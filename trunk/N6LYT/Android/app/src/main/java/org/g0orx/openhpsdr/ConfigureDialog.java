package org.g0orx.openhpsdr;

import android.app.Activity;
import android.app.Dialog;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.MotionEvent;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.SeekBar;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.ViewFlipper;

import org.g0orx.openhpsdr.discovery.Discovered;
import org.g0orx.openhpsdr.wdsp.WDSP;

import java.util.ArrayList;

/**
 * Created by john on 14/02/15.
 */
public class ConfigureDialog extends Dialog {

    public ConfigureDialog(Activity a, boolean locked) {
        super(a);
        this.activity = a;
        this.locked = locked;
        this.configuration = Configuration.getInstance();
        this.wdsp = WDSP.getInstance();
    }


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        //requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.dialog_configure);
        viewFlipper = (ViewFlipper) findViewById(R.id.viewFlipper);
        updateTitle();

        Window window = getWindow();
        Rect displayRect = new Rect();
        window.getDecorView().getWindowVisibleDisplayFrame(displayRect);
        window.setLayout((int) (displayRect.width() * 0.8F), WindowManager.LayoutParams.WRAP_CONTENT);
        //getWindow().setLayout(WindowManager.LayoutParams.MATCH_PARENT, WindowManager.LayoutParams.WRAP_CONTENT);

        Band band = configuration.bands.get();

        final RadioButton radioButtonAudioLocal = (RadioButton) this.findViewById(R.id.radioButtonAudioLocal);
        radioButtonAudioLocal.setChecked(configuration.audiooutput == Configuration.AUDIO_OUTPUT_LOCAL);
        radioButtonAudioLocal.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.audiooutput = Configuration.AUDIO_OUTPUT_LOCAL;
                }
            }
        });

        final RadioButton radioButtonAudioRadio = (RadioButton) this.findViewById(R.id.radioButtonAudioRadio);
        radioButtonAudioRadio.setChecked(configuration.audiooutput == Configuration.AUDIO_OUTPUT_RADIO);
        radioButtonAudioRadio.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.audiooutput = Configuration.AUDIO_OUTPUT_RADIO;
                }
            }
        });

        final RadioButton radioButtonAudioBoth = (RadioButton) this.findViewById(R.id.radioButtonAudioBoth);
        radioButtonAudioBoth.setChecked(configuration.audiooutput == Configuration.AUDIO_OUTPUT_BOTH);
        radioButtonAudioBoth.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.audiooutput = Configuration.AUDIO_OUTPUT_BOTH;
                }
            }
        });

        final TextView textViewafgain = (TextView) this.findViewById(R.id.textViewAFGain);
        textViewafgain.setText("AF Gain: " + (int) (configuration.afgain * 100.0F) + "%");

        SeekBar seekbarafgain = (SeekBar) this.findViewById(R.id.seekBarAFGain);
        seekbarafgain.setMax(255);
        seekbarafgain.setProgress((int) (255.0F * configuration.afgain));
        seekbarafgain.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
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

        BandStack bandstack = band.get();
        final TextView textViewagcgain = (TextView) this.findViewById(R.id.textViewAGCGain);
        textViewagcgain.setText("AGC Gain: " + (int) band.getAGCGain());

        SeekBar seekbaragcgain = (SeekBar) this.findViewById(R.id.seekBarAGCGain);
        seekbaragcgain.setMax(120);

        seekbaragcgain.setProgress((int) band.getAGCGain());
        seekbaragcgain.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                double gain = (double) progress;
                //Log.i("RadioActivity","rf gain="+gain);
                Band band = configuration.bands.get();
                band.setAGCGain(gain);
                textViewagcgain.setText("AGC Gain: " + (int) band.getAGCGain());
                wdsp.SetRXAAGCTop(Channel.RX, gain);
                wdsp.SetRXAAGCTop(Channel.SUBRX, gain);
            }

            public void onStartTrackingTouch(SeekBar seekBar) {
            }

            public void onStopTrackingTouch(SeekBar seekBar) {
            }
        });

        Spinner spinneragc = (Spinner) this.findViewById(R.id.spinnerAGC);
        ArrayList<String> agclist = new ArrayList<String>();
        agclist.add("AGC Off");
        agclist.add("AGC Long");
        agclist.add("AGC Slow");
        agclist.add("AGC Med");
        agclist.add("AGC Fast");
        ArrayAdapter agcadapter = new ArrayAdapter(activity, android.R.layout.simple_spinner_item, agclist);
        agcadapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spinneragc.setAdapter(agcadapter);
        spinneragc.setSelection(band.getAGC());
        spinneragc.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            public void onItemSelected(AdapterView parent, View view, int pos, long id) {
                if (!locked) {
                    Band band = configuration.bands.get();
                    band.setAGC(pos);
                    setAGC(Channel.RX, band.getAGC());
                    setAGC(Channel.SUBRX, band.getAGC());
                }
            }

            public void onNothingSelected(AdapterView parent) {
            }

        });

        CheckBox checkBoxDither = (CheckBox) this.findViewById(R.id.checkBoxDither);
        checkBoxDither.setChecked(configuration.dither == Metis.LT2208_DITHER_ON);
        checkBoxDither.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.dither = Metis.LT2208_DITHER_ON;
                } else {
                    configuration.dither = Metis.LT2208_DITHER_OFF;
                }
            }
        });

        if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
            checkBoxDither.setText("ADC-wide hardware AGC");
        }

        CheckBox checkBoxRandom = (CheckBox) this.findViewById(R.id.checkBoxRandom);
        checkBoxRandom.setChecked(configuration.random == Metis.LT2208_RANDOM_ON);
        checkBoxRandom.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.random = Metis.LT2208_RANDOM_ON;
                } else {
                    configuration.random = Metis.LT2208_RANDOM_OFF;
                }
            }
        });

        if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
            checkBoxRandom.setVisibility(View.GONE);
        }

        CheckBox checkBoxPreamp = (CheckBox) this.findViewById(R.id.checkBoxPreamp);
        checkBoxPreamp.setChecked(configuration.preamp == Metis.LT2208_GAIN_ON);
        checkBoxPreamp.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.preamp = Metis.LT2208_GAIN_ON;
                    //configuration.preampOffset = -20.0F;
                } else {
                    configuration.preamp = Metis.LT2208_GAIN_OFF;
                    //configuration.preampOffset = 0.0F;
                }
            }
        });

        if (configuration.discovered.getDevice() == Discovered.DEVICE_HERMES_LITE) {
            checkBoxPreamp.setVisibility(View.GONE);
        }

        final NumberPicker spectrumHigh = (NumberPicker) this.findViewById(R.id.numberPickerSpectrumHigh);
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

        final NumberPicker spectrumLow = (NumberPicker) this.findViewById(R.id.numberPickerSpectrumLow);
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

        final NumberPicker waterfallHigh = (NumberPicker) this.findViewById(R.id.numberPickerWaterfallHigh);
        final NumberPicker waterfallLow = (NumberPicker) this.findViewById(R.id.numberPickerWaterfallLow);
        final CheckBox checkBoxWaterfallAutomatic = (CheckBox) this.findViewById(R.id.checkBoxWaterfallAutomatic);
        checkBoxWaterfallAutomatic.setChecked(configuration.waterfallAutomatic);
        checkBoxWaterfallAutomatic.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                configuration.waterfallAutomatic = isChecked;
                if (isChecked) {
                    waterfallHigh.setEnabled(false);
                    waterfallLow.setEnabled(false);
                } else {
                    waterfallHigh.setEnabled(true);
                    waterfallLow.setEnabled(true);
                }
            }
        });

        final CheckBox checkBoxWaterfallGrayscale = (CheckBox) this.findViewById(R.id.checkBoxWaterfallGrayscale);
        checkBoxWaterfallGrayscale.setChecked(configuration.waterfallGrayscale);
        checkBoxWaterfallGrayscale.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                configuration.waterfallGrayscale = isChecked;
            }
        });

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

        if (configuration.waterfallAutomatic) {
            waterfallHigh.setEnabled(false);
            waterfallLow.setEnabled(false);
        }

        final RadioButton radioButtonLocal = (RadioButton) this.findViewById(R.id.radioButtonLocal);
        final RadioButton radioButtonRadio = (RadioButton) this.findViewById(R.id.radioButtonRadio);
        final CheckBox micBoostCheckBox = (CheckBox) this.findViewById(R.id.checkBoxMicBoost);

        radioButtonLocal.setChecked(configuration.micsource == Configuration.MIC_SOURCE_LOCAL);
        radioButtonRadio.setChecked(configuration.micsource == Configuration.MIC_SOURCE_RADIO);
        radioButtonLocal.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.micsource = Configuration.MIC_SOURCE_LOCAL;
                    micBoostCheckBox.setChecked(false);
                    micBoostCheckBox.setEnabled(false);
                }
            }
        });

        radioButtonRadio.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked) {
                    configuration.micsource = Configuration.MIC_SOURCE_RADIO;
                    micBoostCheckBox.setEnabled(true);
                    micBoostCheckBox.setChecked(configuration.micsource == Configuration.MIC_SOURCE_RADIO);
                }
            }
        });


        micBoostCheckBox.setChecked(configuration.micboost);
        micBoostCheckBox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                configuration.micboost = isChecked;
            }
        });

        if (configuration.micsource == Configuration.MIC_SOURCE_LOCAL) {
            micBoostCheckBox.setChecked(false);
            micBoostCheckBox.setEnabled(false);
        }

        final TextView textViewmicgain = (TextView) this.findViewById(R.id.textViewMicGain);
        textViewmicgain.setText("Mic Gain: " + (int) (configuration.micgain * 100.0F) + "%");

        final SeekBar seekbarmicgain = (SeekBar) this.findViewById(R.id.seekBarMicGain);
        seekbarmicgain.setMax(150);
        seekbarmicgain.setProgress((int) (configuration.micgain * 100.0F));
        seekbarmicgain.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
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

        final TextView textViewrfpower = (TextView) this.findViewById(R.id.textViewRFPower);
        textViewrfpower.setText("RF Power: " + (int) (configuration.rfgain * 100.0F) + "%");

        SeekBar seekbarrfpower = (SeekBar) this.findViewById(R.id.seekBarRFPower);
        seekbarrfpower.setMax(255);
        seekbarrfpower.setProgress((int) (255.0F * configuration.rfgain));
        seekbarrfpower.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
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

        final TextView textViewtunegain = (TextView) this.findViewById(R.id.textViewTunePower);
        textViewtunegain.setText("Tune Power: " + (int) (configuration.tunegain * 100.0F) + "%");

        SeekBar seekbartunegain = (SeekBar) this.findViewById(R.id.seekBarTunePower);
        seekbartunegain.setMax(255);
        seekbartunegain.setProgress((int) (255.0F * configuration.tunegain));
        seekbartunegain.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
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

        final TextView textViewtxgain = (TextView) this.findViewById(R.id.textViewTxGain);
        textViewtxgain.setText("TX Gain (" + band.getName() + " Mtrs): " + (int) (band.getTxGain() * 100.0F) + "%");

        SeekBar seekbartxgain = (SeekBar) this.findViewById(R.id.seekBarTxGain);
        seekbartxgain.setMax(255);
        seekbartxgain.setProgress((int) (255.0F * band.getTxGain()));
        seekbartxgain.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                Band band = configuration.bands.get();
                float gain = (float) progress / 255.0F;
                band.setTxGain(gain);
                textViewtxgain.setText("TX Gain (" + band.getName() + " Mtrs): " + (int) (band.getTxGain() * 100.0F) + "%");
            }

            public void onStartTrackingTouch(SeekBar seekBar) {
            }

            public void onStopTrackingTouch(SeekBar seekBar) {
            }
        });

        TextView textViewAnt = (TextView) this.findViewById(R.id.textViewAnt);
        textViewAnt.setText("Antenna Configuration (" + band.getName() + " Mtrs)");

        RadioGroup txgroup = (RadioGroup) this.findViewById(R.id.radioGroupTx);
        if (configuration.radio != Configuration.METIS_PENELOPE &&
                configuration.radio != Configuration.METIS_PENNYLANE) {
            RadioButton rb;
            switch (band.get().getTxAntenna()) {
                case 0:
                    rb = (RadioButton) this.findViewById(R.id.radioAnt1);
                    rb.setChecked(true);
                    break;
                case 1:
                    rb = (RadioButton) this.findViewById(R.id.radioAnt2);
                    rb.setChecked(true);
                    break;
                case 2:
                    if (configuration.radio != Configuration.METIS_PENELOPE &&
                            configuration.radio != Configuration.METIS_PENNYLANE) {
                        switch (band.get().getTxAntenna()) {
                            case 0:
                                rb = (RadioButton) this.findViewById(R.id.radioAnt1);
                                rb.setChecked(true);
                                break;
                            case 1:
                                rb = (RadioButton) this.findViewById(R.id.radioAnt2);
                                rb.setChecked(true);
                                break;
                            case 2:
                                rb = (RadioButton) this.findViewById(R.id.radioAnt3);
                                rb.setChecked(true);
                                break;
                        }

                        txgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                            @Override
                            public void onCheckedChanged(RadioGroup group,
                                                         int checkedId) {
                                //Log.i("RadioActivity","Tx Group:"+checkedId);
                                Band band = configuration.bands.get();
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
                    rb = (RadioButton) this.findViewById(R.id.radioAnt3);
                    rb.setChecked(true);
                    break;
            }

            txgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                @Override
                public void onCheckedChanged(RadioGroup group,
                                             int checkedId) {
                    //Log.i("RadioActivity","Tx Group:"+checkedId);
                    Band band = configuration.bands.get();
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
            TextView tv = (TextView) this.findViewById(R.id.textViewTx);
            tv.setVisibility(View.GONE);
            RadioButton rb = (RadioButton) this.findViewById(R.id.radioAnt1);
            rb.setVisibility(View.GONE);
            rb = (RadioButton) this.findViewById(R.id.radioAnt2);
            rb.setVisibility(View.GONE);
            rb = (RadioButton) this.findViewById(R.id.radioAnt3);
            rb.setVisibility(View.GONE);
            txgroup.setVisibility(View.GONE);
        }


        RadioGroup rxgroup = (RadioGroup) this.findViewById(R.id.radioGroupRx);
        if (configuration.radio != Configuration.METIS_PENELOPE &&
                configuration.radio != Configuration.METIS_PENNYLANE) {
            RadioButton rb;
            if (configuration.radio == Configuration.ANGELIA_ANAN100D || configuration.radio == Configuration.ORION_ANAN200D) {
                rb = (RadioButton) this.findViewById(R.id.radioRX1);
                rb.setText("EXT 2");
                rb = (RadioButton) this.findViewById(R.id.radioRX2);
                rb.setText("EXT 1");
            }

            switch (band.get().getRxAntenna()) {
                case 0:
                    rb = (RadioButton) this.findViewById(R.id.radioNone);
                    rb.setChecked(true);
                    break;
                case 1:
                    rb = (RadioButton) this.findViewById(R.id.radioRX1);
                    rb.setChecked(true);
                    break;
                case 2:
                    rb = (RadioButton) this.findViewById(R.id.radioRX2);
                    rb.setChecked(true);
                    break;
                case 3:
                    rb = (RadioButton) this.findViewById(R.id.radioRXXV);
                    rb.setChecked(true);
                    break;
            }

            rxgroup.setOnCheckedChangeListener(new android.widget.RadioGroup.OnCheckedChangeListener() {
                @Override
                public void onCheckedChanged(RadioGroup group,
                                             int checkedId) {
                    //Log.i("RadioActivity","Rx Group:"+checkedId);
                    Band band = configuration.bands.get();
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
            TextView tv = (TextView) this.findViewById(R.id.textViewRx);
            tv.setVisibility(View.GONE);
            RadioButton rb = (RadioButton) this.findViewById(R.id.radioNone);
            rb.setVisibility(View.GONE);
            rb = (RadioButton) this.findViewById(R.id.radioRX1);
            rb.setVisibility(View.GONE);
            rb = (RadioButton) this.findViewById(R.id.radioRX2);
            rb.setVisibility(View.GONE);
            rb = (RadioButton) this.findViewById(R.id.radioRXXV);
            rb.setVisibility(View.GONE);
            rxgroup.setVisibility(View.GONE);
        }

        TextView textViewOC = (TextView) this.findViewById(R.id.textViewOC);
        textViewOC.setText("Open Collector Configuration (" + band.getName() + ")");

        CheckBox cbocrx1 = (CheckBox) this.findViewById(R.id.checkBoxOCRx1);
        cbocrx1.setChecked((band.getOCRx() & 0x01) == 0x01);
        cbocrx1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x01));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xFE));
                }
            }
        });

        CheckBox cbocrx2 = (CheckBox) this.findViewById(R.id.checkBoxOCRx2);
        cbocrx2.setChecked((band.getOCRx() & 0x02) == 0x02);
        cbocrx2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x02));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xFD));
                }
            }
        });

        CheckBox cbocrx3 = (CheckBox) this.findViewById(R.id.checkBoxOCRx3);
        cbocrx3.setChecked((band.getOCRx() & 0x04) == 0x04);
        cbocrx3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x04));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xFB));
                }
            }
        });

        CheckBox cbocrx4 = (CheckBox) this.findViewById(R.id.checkBoxOCRx4);
        cbocrx4.setChecked((band.getOCRx() & 0x08) == 0x08);
        cbocrx4.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x08));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xF7));
                }
            }
        });

        CheckBox cbocrx5 = (CheckBox) this.findViewById(R.id.checkBoxOCRx5);
        cbocrx5.setChecked((band.getOCRx() & 0x10) == 0x10);
        cbocrx5.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x10));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xEF));
                }
            }
        });

        CheckBox cbocrx6 = (CheckBox) this.findViewById(R.id.checkBoxOCRx6);
        cbocrx6.setChecked((band.getOCRx() & 0x20) == 0x20);
        cbocrx6.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x20));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xDF));
                }
            }
        });

        CheckBox cbocrx7 = (CheckBox) this.findViewById(R.id.checkBoxOCRx7);
        cbocrx7.setChecked((band.getOCRx() & 0x40) == 0x40);
        cbocrx7.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCRx((byte) (band.getOCRx() | 0x40));
                } else {
                    band.setOCRx((byte) (band.getOCRx() & 0xBF));
                }
            }
        });

        CheckBox cboctx1 = (CheckBox) this.findViewById(R.id.checkBoxOCTx1);
        cboctx1.setChecked((band.getOCTx() & 0x01) == 0x01);
        cboctx1.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x01));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xFE));
                }
            }
        });

        CheckBox cboctx2 = (CheckBox) this.findViewById(R.id.checkBoxOCTx2);
        cboctx2.setChecked((band.getOCTx() & 0x02) == 0x02);
        cboctx2.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x02));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xFD));
                }
            }
        });

        CheckBox cboctx3 = (CheckBox) this.findViewById(R.id.checkBoxOCTx3);
        cboctx3.setChecked((band.getOCTx() & 0x04) == 0x04);
        cboctx3.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x04));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xFB));
                }
            }
        });

        CheckBox cboctx4 = (CheckBox) this.findViewById(R.id.checkBoxOCTx4);
        cboctx4.setChecked((band.getOCTx() & 0x08) == 0x08);
        cboctx4.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x08));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xF7));
                }
            }
        });

        CheckBox cboctx5 = (CheckBox) this.findViewById(R.id.checkBoxOCTx5);
        cboctx5.setChecked((band.getOCTx() & 0x10) == 0x10);
        cboctx5.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x10));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xEF));
                }
            }
        });

        CheckBox cboctx6 = (CheckBox) this.findViewById(R.id.checkBoxOCTx6);
        cboctx6.setChecked((band.getOCTx() & 0x20) == 0x20);
        cboctx6.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x20));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xDF));
                }
            }
        });

        CheckBox cboctx7 = (CheckBox) this.findViewById(R.id.checkBoxOCTx7);
        cboctx7.setChecked((band.getOCTx() & 0x40) == 0x40);
        cboctx7.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton arg0, boolean arg1) {
                Band band = configuration.bands.get();
                if (arg1) {
                    band.setOCTx((byte) (band.getOCTx() | 0x40));
                } else {
                    band.setOCTx((byte) (band.getOCTx() & 0xBF));
                }
            }
        });
    }

    public boolean onTouchEvent(MotionEvent touchevent) {
        switch (touchevent.getAction()) {

            case MotionEvent.ACTION_DOWN:
                lastX = touchevent.getX();
                break;
            case MotionEvent.ACTION_UP:
                float currentX = touchevent.getX();

                // Handling left to right screen swap.
                if (lastX < currentX) {

                    // If there aren't any other children, just break.
                    if (viewFlipper.getDisplayedChild() == 0)
                        break;

                    // Next screen comes in from left.
                    viewFlipper.setInAnimation(this.getContext(), R.anim.slide_in_from_left);
                    // Current screen goes out from right.
                    viewFlipper.setOutAnimation(this.getContext(), R.anim.slide_out_to_right);

                    // Display next screen.
                    viewFlipper.showPrevious();
                    updateTitle();
                }

                // Handling right to left screen swap.
                if (lastX > currentX) {

                    // If there is a child (to the left), kust break.
                    if (viewFlipper.getDisplayedChild() == (viewFlipper.getChildCount() - 1))
                        break;

                    // Next screen comes in from right.
                    viewFlipper.setInAnimation(this.getContext(), R.anim.slide_in_from_right);
                    // Current screen goes out from left.
                    viewFlipper.setOutAnimation(this.getContext(), R.anim.slide_out_to_left);

                    // Display previous screen.
                    viewFlipper.showNext();
                    updateTitle();
                }
                break;
        }
        return false;
    }

    private void setAGC(int channel, int agc) {
        wdsp.SetRXAAGCMode(channel, agc);
        switch (agc) {
            case AGCLong:
                wdsp.SetRXAAGCAttack(channel, 2);
                wdsp.SetRXAAGCHang(channel, 2000);
                wdsp.SetRXAAGCDecay(channel, 2000);
                break;
            case AGCSlow:
                wdsp.SetRXAAGCAttack(channel, 2);
                wdsp.SetRXAAGCHang(channel, 1000);
                wdsp.SetRXAAGCDecay(channel, 500);
                break;
            case AGCMed:
                wdsp.SetRXAAGCAttack(channel, 2);
                wdsp.SetRXAAGCHang(channel, 0);
                wdsp.SetRXAAGCDecay(channel, 250);
                break;
            case AGCFast:
                wdsp.SetRXAAGCAttack(channel, 2);
                wdsp.SetRXAAGCHang(channel, 0);
                wdsp.SetRXAAGCDecay(channel, 50);
                break;
        }
    }

    private void updateTitle() {
        setTitle("Configuration - page " + (viewFlipper.getDisplayedChild() + 1) + " of " + viewFlipper.getChildCount() + " (swipe this title bar left or right to change)");
    }

    private Activity activity;
    private boolean locked;
    private Configuration configuration;
    private WDSP wdsp;
    private ViewFlipper viewFlipper;
    private float lastX;

    private static final int AGCOff = 0;
    private static final int AGCLong = 1;
    private static final int AGCSlow = 2;
    private static final int AGCMed = 3;
    private static final int AGCFast = 4;

}
