using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Midi2Cat.Data
{
    public class MappedCommand
    {
        public int CmdId { get; set; }
        public string Description { get; set; }
        public ControlType ControlType { get; set; }
        public string Controller { get; set; }
        public string ControlName { get; set; }
        public string Remove { get; set; }

        public MappedCommand(DataRow row)
        {
            CmdId = (int)row["CmdId"];
            Description = (string)row["CmdDescription"];
            Controller = "";
            ControlName = "";
            ControlType = ControlType.Unknown;
            Remove = ""; 
        }

        public MappedCommand(MappedCommand src)
        {
            CmdId = src.CmdId;
            Description = src.Description;
            Controller = src.Controller;
            ControlName = src.ControlName;
            ControlType = src.ControlType;
            Remove = src.Remove;
        }
    }

    public class MappedCommands : List<MappedCommand>
    {
        public MappedCommands(DataTable Cmds, DataTable controllerDT)
        {
            foreach (DataRow row in Cmds.Rows)
            {
                if ((CatCmd)(row["CmdId"]) == CatCmd.None) 
                    continue;
                MappedCommand mappedCommand=new MappedCommand(row);
                if (GetDeviceMappings(mappedCommand, controllerDT) <= 0)
                {
                    mappedCommand.ControlType = ((ControlType)row["ControlType"]);
                    this.Add(mappedCommand);
                }
            }
        }

        private int GetDeviceMappings(MappedCommand mappedCmd, DataTable controllerDT)
        {
            int NumAdded = 0;
            foreach (DataRow dr in controllerDT.Rows)
            {                        
                if ((int)(dr["CatCmdId"]) == mappedCmd.CmdId)
                {
                    MappedCommand mc = new MappedCommand(mappedCmd);
                    mc.Controller = controllerDT.TableName;
                    mc.ControlName = (string)dr["MidiControlName"];
                    mc.ControlType = FixUp.FixControlType(((int)dr["MidiControlType"]));
                    mc.Remove = "Unmap";
                    this.Add(mc);
                    NumAdded++;
                }
            }
            return NumAdded;
        }

        

    } // class
}//namespace
