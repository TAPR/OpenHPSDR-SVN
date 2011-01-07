using System;
using System.Runtime.Serialization;

namespace RIOX
{
    [Serializable()]
    public class RIOXCommand : ISerializable
    {
        public String Command { get; set; }
        public String Data { get; set; }        

        public RIOXCommand(String Command, String Data)
        {
            this.Command = Command;
            this.Data = Data;            
        }

        public RIOXCommand(SerializationInfo info, StreamingContext context)
        {
            Command = (String) info.GetValue("Command", typeof (String));
            Data = (String)info.GetValue("Data", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Command", Command);
            info.AddValue("Data", Data);            
        }

        public RIOXCommand() {}        
    }
}
