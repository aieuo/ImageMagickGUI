using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ImageEditor.Models.Actions.Parameters;

namespace ImageEditor.Models.Actions
{
    internal class FlipAction(FlipAction.FlipType type = FlipAction.FlipType.Flip) : Action
    {
        internal enum FlipType
        {
            Flip,
            Flop,
        }


        public override string Name => "Flip";

        public override string Description => "画像を反転させる";

        public override string IconPath => "../Resources/flip.png";


        private readonly FlipType Type = type;

        public override List<ActionParameter> Parameters => [
            new ActionParameter<FlipType>("enum", "方向", in Type)
        ];


        public override Dictionary<string, string> GetCommandParameters()
        {
            return new Dictionary<string, string>
            {
                { $"-{Type.ToString().ToLower()}", "" }
            };
        }

        public override string ToString()
        {
            return Type == FlipType.Flip
                ? "左右反転させる"
                : "上下反転させる";
        }
    }
}
