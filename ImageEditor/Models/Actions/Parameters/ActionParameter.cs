using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Models.Actions.Parameters
{
    internal class ActionParameter<T> : ActionParameter
    {
        string ParameterType { get; }
        string Description { get; }
        T Value { get; set; }

        public ActionParameter(string type, string description, in T defaultValue)
        {
            ParameterType = type;
            Description = description;
            Value = defaultValue;
        }
    }

    internal class ActionParameter
    {
        internal ActionParameter()
        {
        }
    }
}
