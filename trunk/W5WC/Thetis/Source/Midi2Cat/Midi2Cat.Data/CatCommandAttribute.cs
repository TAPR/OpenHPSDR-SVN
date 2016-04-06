﻿/*  Midi2Cat

Description: A subsystem that facilitates mapping Windows MIDI devices to CAT commands.
 
Copyright (C) 2016 Andrew Mansfield, M0YGG

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at:  midi2cat@cametrix.com

*/

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
