using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi2Cat.Data;

namespace Midi2Cat.Data
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class CatCommandAttribute : System.Attribute
    {
        public string Desc;
        public ControlType ControlType;
        public CatCmd CatCommandId;
        public bool IsToggled;

        public CatCommandAttribute() { }

        public CatCommandAttribute(string desc, ControlType ctrlType)
        {
            this.Desc = desc;
            this.ControlType = ctrlType;
        }

        public CatCommandAttribute(string desc, ControlType ctrlType, bool isToggled)
        {
            this.Desc = desc;
            this.ControlType = ctrlType;
            this.IsToggled = isToggled;
        }

        public override string ToString()
        {
            return Desc;
        }
    }
}
