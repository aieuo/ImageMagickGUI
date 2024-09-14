using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ImageEditor.Models.Actions.Parameters;

namespace ImageEditor.Models.Actions
{
    internal class FlipAction : Action
    {
        internal enum FlipType
        {
            Flip,
            Flop,
        }


        public override string Name => "Flip";

        public override string Description => "画像を反転させる";

        public override string IconPath => "../Resources/flip.png";


        private readonly FlipType _type;

        internal FlipAction(FlipType type = FlipType.Flip)
        {
            _type = type;
            
            Parameters.Add(new ActionParameter<FlipType>("enum", "方向", in _type));
        }
        
        public override Dictionary<string, string> GetCommandParameters()
        {
            return new Dictionary<string, string>
            {
                { $"-{_type.ToString().ToLower()}", "" }
            };
        }

        public override string ToString()
        {
            return _type == FlipType.Flip
                ? "左右反転させる"
                : "上下反転させる";
        }
    }
}