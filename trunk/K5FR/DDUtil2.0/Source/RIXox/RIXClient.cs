using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Soap;
using System.Threading;

namespace RIXox
{
    public class RIXClient
    {
        public event ObjectReceivedEventHandler ObjectReceivedEvent;

        public bool IsConnected { 
            get 
            { 
                if(_client != null) 
                    return _client.Connected;
                return false; 
            } 
        }

        public Type DataType { get; set; }

        private TcpClient _client;
        private Thread _clientThread;
        private bool _stopThreads;

        public RIXClient(Type dataType, String hostname, int port)
        {
            try
            {
                _client = new TcpClient(hostname, port);     
                _clientThread = new Thread(ClientThread);
                _clientThread.Start(_client);
                DataType = dataType;
            }
            catch (Exception e)
            {                
                throw new Exception("Could not connect to RIXServer", e);
            }            
        }

        public void SendCommand(RIXCommand command)
        {
            NetworkStream ns = _client.GetStream();
            SoapFormatter sf = new SoapFormatter();
            sf.Serialize(ns, command);
            ns.Flush();
        }

        private void ClientThread(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            NetworkStream ns = client.GetStream();
            SoapFormatter sf = new SoapFormatter();
            
            while (!_stopThreads)
            {
                try
                {
                    if (ns.DataAvailable)
                    {
                        Object o = sf.Deserialize(ns);
                        if(o.GetType() != DataType)
                        {
                            throw new Exception("Object received does not match DataType");
                        }
                        ns.Flush();
                        ObjectReceivedEvent(this, new ObjectReceivedEventArgs(o));
                    }                
                }
                catch (Exception e)
                {
                    throw new Exception("Object deserialization error", e);
                }
            }
        }

        public void Close()
        {
            _stopThreads = true;
            _client.Close();
        }

        public delegate void ObjectReceivedEventHandler(object o, ObjectReceivedEventArgs e);
        public class ObjectReceivedEventArgs : EventArgs
        {
            public Object DataObject { get; set;}
            public ObjectReceivedEventArgs(Object dataObject)
            {
                DataObject = dataObject;
            }
        }
    }
}
