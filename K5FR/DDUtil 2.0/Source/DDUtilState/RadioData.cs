using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DDUtilState
{
    // this is the RadioData struct for DDUtil
    [Serializable()]
    public class RadioData : ISerializable, INotifyPropertyChanged
    {
        [NonSerialized]
        private string _vfoa = "00014000000";
        [NonSerialized]
        private string _vfob = "00007100000";
        [NonSerialized]
        private string _vfotx = "1";
        [NonSerialized]
        private string _antr1 = "1";
        [NonSerialized]
        private string _antr2 = "1";
        [NonSerialized]
        private string _txant = "1";
        [NonSerialized]
        private string _txline = "100";
        [NonSerialized]
        private string _fltr1 = "09";
        [NonSerialized]
        private string _fltr2 = "10";
        [NonSerialized]
        private string _bandr1 = "05";
        [NonSerialized]
        private string _bandr2 = "06";
        [NonSerialized]
        private string _moder1 = "06";
        [NonSerialized]
        private string _moder2 = "07";
        [NonSerialized]
        private string _model = "1";
        [NonSerialized]
        private string _temp = "35.3";
        [NonSerialized]
        private string _volts = "13.8";
        [NonSerialized]
        private string _tunstp = "3";
        [NonSerialized]
        private string _drive = "100";
        [NonSerialized]
        private string _vol = "100";
        [NonSerialized]
        private string _ritfreq = "000.0";
        [NonSerialized]
        private string _dispmode = "1";
        [NonSerialized]
        private string _agc = "3";
        [NonSerialized]
        private string _cwx = "025";
        [NonSerialized]
        private string _tunpwr = "050";
        [NonSerialized]
        private string _smtr = "048";
        [NonSerialized]
        private string _xitfreq = "+0100";
        [NonSerialized]
        private string _cpu = "013.88";
        [NonSerialized]
        private bool _ritsts = false;
        [NonSerialized]
        private bool _tun = false;
        [NonSerialized]
        private bool _split = false;
        [NonSerialized]
        private bool _mox = false;
        [NonSerialized]
        private bool _rx2sts = false;
        [NonSerialized]
        private bool _mulrxsts = false;
        [NonSerialized]
        private bool _xitsts = false;

        public string vfoa { get { return _vfoa; } set { _vfoa = value; NotifyPropertyChanged("vfoa"); } }
        public string vfob { get { return _vfob; } set { _vfob = value; NotifyPropertyChanged("vfob"); } }
        public string vfotx { get { return _vfotx; } set { _vfotx = value; NotifyPropertyChanged("vfotx"); } }
        public string antr1 { get { return _antr1; } set { _antr1 = value; NotifyPropertyChanged("antr1"); } }
        public string antr2 { get { return _antr2; } set { _antr2 = value; NotifyPropertyChanged("antr2"); } }
        public string txant { get { return _txant; } set { _txant = value; NotifyPropertyChanged("txant"); } }
        public string txline { get { return _txline; } set { _txline = value; NotifyPropertyChanged("txline"); } }
        public string fltr1 { get { return _fltr1; } set { _fltr1 = value; NotifyPropertyChanged("fltr1"); } }
        public string fltr2 { get { return _fltr2; } set { _fltr2 = value; NotifyPropertyChanged("fltr1"); } }
        public string bandr1 { get { return _bandr1; } set { _bandr1 = value; NotifyPropertyChanged("bandr1"); } }
        public string bandr2 { get { return _bandr2; } set { _bandr2 = value; NotifyPropertyChanged("bandr2"); } }
        public string moder1 { get { return _moder1; } set { _moder1 = value; NotifyPropertyChanged("moder1"); } }
        public string moder2 { get { return _moder2; } set { _moder2 = value; NotifyPropertyChanged("moder2"); } }
        public string model { get { return _model; } set { _model = value; NotifyPropertyChanged("model"); } }
        public string temp { get { return _temp; } set { _temp = value; NotifyPropertyChanged("temp"); } }
        public string volts { get { return _volts; } set { _volts = value; NotifyPropertyChanged("volts"); } }
        public string tunstp { get { return _tunstp; } set { _tunstp = value; NotifyPropertyChanged("tunstp"); } }
        public string drive { get { return _drive; } set { _drive = value; NotifyPropertyChanged("drive"); } }
        public string vol { get { return _vol; } set { _vol = value; NotifyPropertyChanged("vol"); } }
        public string ritfreq { get { return _ritfreq; } set { _ritfreq = value; NotifyPropertyChanged("ritfreq"); } }
        public string dispmode { get { return _dispmode; } set { _dispmode = value; NotifyPropertyChanged("dispmode"); } }
        public string agc { get { return _agc; } set { _agc = value; NotifyPropertyChanged("agc"); } }
        public string cwx { get { return _cwx; } set { _cwx = value; NotifyPropertyChanged("cwx"); } }
        public string tunpwr { get { return _tunpwr; } set { _tunpwr = value; NotifyPropertyChanged("tunpwr"); } }
        public string smtr { get { return _smtr; } set { _smtr = value; NotifyPropertyChanged("smtr"); } }
        public string xitfreq { get { return _xitfreq; } set { _xitfreq = value; NotifyPropertyChanged("xitfreq"); } }
        public string cpu { get { return _cpu; } set { _cpu = value; NotifyPropertyChanged("cpu"); } }
        public bool mox { get { return _mox; } set { _mox = value; NotifyPropertyChanged("mox"); } }
        public bool tun { get { return _tun; } set { _tun = value; NotifyPropertyChanged("tun"); } }
        public bool split { get { return _split; } set { _split = value; NotifyPropertyChanged("split"); } }
        public bool ritsts { get { return _ritsts; } set { _ritsts = value; NotifyPropertyChanged("ritsts"); } }
        public bool rx2sts { get { return _rx2sts; } set { _rx2sts = value; NotifyPropertyChanged("rx2sts"); } }
        public bool mulrxsts { get { return _mulrxsts; } set { _mulrxsts = value; NotifyPropertyChanged("mulrxsts"); } }
        public bool xitsts { get { return _xitsts; } set { _xitsts = value; NotifyPropertyChanged("xitsts"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RadioData()
        {
        }

        // This is for deserialization
        public RadioData(SerializationInfo info, StreamingContext context)
        {
            vfoa = (string)info.GetValue("vfoa", typeof(string));
            vfob = (string)info.GetValue("vfob", typeof(string));
            vfotx = (string)info.GetValue("vfotx", typeof(string));
            moder1 = (string)info.GetValue("moder1", typeof(string));
            moder2 = (string)info.GetValue("moder2", typeof(string));
            fltr1 = (string)info.GetValue("fltr1", typeof(string));
            fltr2 = (string)info.GetValue("fltr2", typeof(string));
            bandr1 = (string)info.GetValue("bandr1", typeof(string));
            bandr2 = (string)info.GetValue("bandr2", typeof(string));
            antr1 = (string)info.GetValue("antr1", typeof(string));
            antr2 = (string)info.GetValue("antr2", typeof(string));
            txant = (string)info.GetValue("txant", typeof(string));
            txline = (string)info.GetValue("txline", typeof(string));
            model = (string)info.GetValue("model", typeof(string));
            temp = (string)info.GetValue("temp", typeof(string));
            volts = (string)info.GetValue("volts", typeof(string));
            tunstp = (string)info.GetValue("tunstp", typeof(string));
            drive = (string)info.GetValue("drive", typeof(string));
            vol = (string)info.GetValue("vol", typeof(string));
            ritfreq = (string)info.GetValue("ritfreq", typeof(string));
            dispmode = (string)info.GetValue("dispmode", typeof(string));
            agc = (string)info.GetValue("agc", typeof(string));
            cwx = (string)info.GetValue("cwx", typeof(string));
            tunpwr = (string)info.GetValue("tunpwr", typeof(string));
            smtr = (string)info.GetValue("smtr", typeof(string));
            xitfreq = (string)info.GetValue("xitfreq", typeof(string));
            vol = (string)info.GetValue("vol", typeof(string));
            mox = (bool)info.GetValue("mox", typeof(bool));
            tun = (bool)info.GetValue("tun", typeof(bool));
            split = (bool)info.GetValue("split", typeof(bool));
            ritsts = (bool)info.GetValue("ritsts", typeof(bool));
            rx2sts = (bool)info.GetValue("rx2sts", typeof(bool));
            mulrxsts = (bool)info.GetValue("mulrxsts", typeof(bool));
            xitsts = (bool)info.GetValue("xitsts", typeof(bool));
        }

        // this is for serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("vfoa", vfoa);
            info.AddValue("vfob", vfob);
            info.AddValue("vfotx", vfotx);
            info.AddValue("moder1", moder1);
            info.AddValue("moder2", moder2);
            info.AddValue("fltr1", fltr1);
            info.AddValue("fltr2", fltr2);
            info.AddValue("bandr1", bandr1);
            info.AddValue("bandr2", bandr2);
            info.AddValue("antr1", antr1);
            info.AddValue("antr2", antr2);
            info.AddValue("txant", txant);
            info.AddValue("txline", txline);
            info.AddValue("model", model);
            info.AddValue("temp", temp);
            info.AddValue("volts", volts);
            info.AddValue("tunstp", tunstp);
            info.AddValue("drive", drive);
            info.AddValue("vol", vol);
            info.AddValue("ritfreq", ritfreq);
            info.AddValue("dispmode", dispmode);
            info.AddValue("agc", agc);
            info.AddValue("cwx", cwx);
            info.AddValue("tunpwr", tunpwr);
            info.AddValue("smtr", smtr);
            info.AddValue("xitfreq", xitfreq);
            info.AddValue("cpu", cpu);
            info.AddValue("mox", mox);
            info.AddValue("tun", tun);
            info.AddValue("split", split);
            info.AddValue("ritsts", ritsts);
            info.AddValue("rx2sts", rx2sts);
            info.AddValue("mulrxsts", mulrxsts);
            info.AddValue("xitsts", xitsts);
        }
    }
}
