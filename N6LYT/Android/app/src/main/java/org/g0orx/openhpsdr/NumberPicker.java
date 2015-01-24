package org.g0orx.openhpsdr;

/*
 * Copyright (c) 2010, Jeffrey F. Cole
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 	Redistributions of source code must retain the above copyright notice, this
 * 	list of conditions and the following disclaimer.
 *
 * 	Redistributions in binary form must reproduce the above copyright notice,
 * 	this list of conditions and the following disclaimer in the documentation
 * 	and/or other materials provided with the distribution.
 *
 * 	Neither the name of the technologichron.net nor the names of its contributors
 * 	may be used to endorse or promote products derived from this software
 * 	without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 *
 * Modified 2014, John D. Melton
 */


import android.content.Context;
import android.graphics.Typeface;
import android.os.Handler;
import android.text.InputType;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.TextView;

/**
 * A simple layout group that provides a numeric text area with two buttons to
 * increment or decrement the value in the text area. Holding either button
 * will auto increment the value up or down appropriately.
 *
 * @author Jeffrey F. Cole
 *
 */
public class NumberPicker extends LinearLayout {

    private final long REPEAT_DELAY = 50;

    private final int ELEMENT_HEIGHT = 100;
    private final int ELEMENT_WIDTH = 100;
    private int MINIMUM = 0;
    private int MAXIMUM = 999;

    public Integer value;

    Button decrement;
    Button increment;
    public TextView valueText;

    private Handler repeatUpdateHandler = new Handler();

    private boolean autoIncrement = false;
    private boolean autoDecrement = false;

    class RepetetiveUpdater implements Runnable {
        public void run() {
            if( autoIncrement ){
                increment();
                repeatUpdateHandler.postDelayed( new RepetetiveUpdater(), REPEAT_DELAY );
            } else if( autoDecrement ){
                decrement();
                repeatUpdateHandler.postDelayed( new RepetetiveUpdater(), REPEAT_DELAY );
            }
        }
    }

    public NumberPicker( Context context, AttributeSet attributeSet ) {
        super(context, attributeSet);

        this.setLayoutParams(new LinearLayout.LayoutParams(LayoutParams.WRAP_CONTENT, LayoutParams.WRAP_CONTENT));
        initDecrementButton(context);
        initValueTextView(context);
        initIncrementButton( context );

        // Can be configured to be vertical or horizontal
        // Thanks for the help, LinearLayout!
        if( getOrientation() == VERTICAL ){
            LayoutParams elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( increment/*, elementParams*/ );
            elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( valueText/*, elementParams*/ );
            elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( decrement/*, elementParams*/ );
        } else {
            LayoutParams elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( decrement/*, elementParams*/ );
            elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( valueText/*, elementParams*/ );
            elementParams = new LinearLayout.LayoutParams( ELEMENT_WIDTH, ELEMENT_HEIGHT );
            addView( increment/*, elementParams*/ );
        }

        increment.setVisibility(View.VISIBLE);
        valueText.setVisibility(View.VISIBLE);
        decrement.setVisibility(View.VISIBLE);

    }

    public void setMinimum(int i) {
        MINIMUM=i;
    }

    public void setMaximum(int i) {
        MAXIMUM=i;
    }

    private void initIncrementButton( Context context){
        Log.i("NumberPicker","initValueIncrementButton");
        increment = new Button( context );
        //increment.setTextSize( 25 );
        increment.setText( " + " );

        // Increment once for a click
        increment.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                increment();
            }
        });

        // Auto increment for a long click
        increment.setOnLongClickListener(
                new View.OnLongClickListener(){
                    public boolean onLongClick(View arg0) {
                        autoIncrement = true;
                        repeatUpdateHandler.post( new RepetetiveUpdater() );
                        return false;
                    }
                }
        );

        // When the button is released, if we're auto incrementing, stop
        increment.setOnTouchListener( new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                if( event.getAction() == MotionEvent.ACTION_UP && autoIncrement ){
                    autoIncrement = false;
                }
                return false;
            }
        });
    }

    private void initValueTextView( Context context){

        Log.i("NumberPicker","initValueTextView");

        value = new Integer( 0 );

        valueText = new TextView( context );
        valueText.setTypeface(null, Typeface.BOLD);
        valueText.setGravity( Gravity.CENTER_VERTICAL | Gravity.CENTER_HORIZONTAL );
        valueText.setText( value.toString() );
    }

    private void initDecrementButton( Context context){
        Log.i("NumberPicker","initDecrementButton");
        decrement = new Button( context );
        //decrement.setTextSize( 25 );
        decrement.setText( " - " );


        // Decrement once for a click
        decrement.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                decrement();
            }
        });


        // Auto Decrement for a long click
        decrement.setOnLongClickListener(
                new View.OnLongClickListener(){
                    public boolean onLongClick(View arg0) {
                        autoDecrement = true;
                        repeatUpdateHandler.post( new RepetetiveUpdater() );
                        return false;
                    }
                }
        );

        // When the button is released, if we're auto decrementing, stop
        decrement.setOnTouchListener( new View.OnTouchListener() {
            public boolean onTouch(View v, MotionEvent event) {
                if( event.getAction() == MotionEvent.ACTION_UP && autoDecrement ){
                    autoDecrement = false;
                }
                return false;
            }
        });
    }

    public void increment(){
        Log.i("NumberPicker","increment");
        if( value < MAXIMUM ){
            value = value + i;
            valueText.setText( value.toString() );
        }
        if(listener!=null) {
            listener.numberPickerChanged(value);
        }
    }

    public void decrement(){
        Log.i("NumberPicker","decrement");
        if( value > MINIMUM ){
            value = value - i;
            valueText.setText( value.toString() );
        }
        if(listener!=null) {
            listener.numberPickerChanged(value);
        }
    }

    public int getValue(){
        return value;
    }

    public void setValue( int value ){
        if( value > MAXIMUM ) value = MAXIMUM;
        if( value < MINIMUM ) value = MINIMUM;
        this.value = value;
        valueText.setText( this.value.toString() );
    }

    public void setIncrement(int increment) {
        this.i=increment;
    }

    public void setOnNumberPickerChangedListener(OnNumberPickerChangedListener listener) {
        this.listener = listener;
    }

    private OnNumberPickerChangedListener listener;

    private int i=1;
}
