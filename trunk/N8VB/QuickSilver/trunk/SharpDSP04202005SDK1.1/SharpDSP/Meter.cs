//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General public License
//==============================================================
//meter.cs
//
//==============================================================

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace SharpDSP
{
	[Serializable()]
    public struct Meter
    {
        #region Private members

        private int size;
        private float meter_average_value;

        #endregion

        #region Properties

        private float meter_value;
        public float MeterValue
        {
            get { return meter_value; }
        }
        private MeterType meter_type;
        public MeterType Meter_Type
        {
            get { return meter_type; }
            set
            {
                meter_type = value;
            }
        }

        #endregion

        #region Constructor

        public Meter(int size)
        {
            this.meter_value = 0F;
            this.size = size;
            this.meter_value = 0F;
            this.meter_type = MeterType.SIGNAL_STRENGTH;
            this.meter_average_value = 0F;
        }

        #endregion

        #region Public Methods

        unsafe public void DoMeter(float* real, float* imag, MeterPosition position)
        {
            switch (this.meter_type)
            {
                case MeterType.SIGNAL_STRENGTH:
                    {
                        if (position != MeterPosition.POSTFILTER) { return; }
                        float temp_metervalue = 0F;
                        for (int i = 0; i < this.size; i++)
                        {
                            temp_metervalue += (real[i] * real[i] + imag[i] * imag[i]);
                        }
                        this.meter_value = (float)(10.0 * Math.Log10(temp_metervalue + 1e-20));
                    }
                    break;
                case MeterType.AVG_SIGNAL_STRENGTH:
                    {
                        if (position != MeterPosition.POSTFILTER) { return; }
                        float temp_metervalue = 0F;
                        for (int i = 0; i < this.size; i++)
                        {
                            temp_metervalue += (real[i] * real[i] + imag[i] * imag[i]);
                        }
                        temp_metervalue = (float)(Math.Log10(temp_metervalue + 1e-20));
                        this.meter_average_value = (this.meter_average_value * 0.9F) + temp_metervalue;
                        this.meter_value = this.meter_average_value;
                    }
                    break;
                case MeterType.ADC_REAL:
                    {
                        if (position != MeterPosition.PREFILTER) { return; }
                        float temp_metervalue = 0F;
                        for (int i = 0; i < this.size; i++)
                        {
                            temp_metervalue = Math.Max(Math.Abs(real[i]), temp_metervalue);
                        }
                        this.meter_value = (float)(20.0 * Math.Log10(temp_metervalue + 1e-10));
                    }
                    break;
                case MeterType.ADC_IMAG:
                    {
                        if (position != MeterPosition.PREFILTER) { return; }
                        float temp_metervalue = 0F;
                        for (int i = 0; i < this.size; i++)
                        {
                            temp_metervalue = Math.Max(Math.Abs(imag[i]), temp_metervalue);
                        }
                        this.meter_value = (float)(20.0 * Math.Log10(temp_metervalue + 1e-10));
                    }
                    break;
                case MeterType.PWR:
                case MeterType.PKPWR:

                    break;
                case MeterType.ALC:
                    break;
                default:
                    break;
            }

        }

        #endregion

    }

}