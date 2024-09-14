using ImageEditor.Models.Actions.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Models.Actions
{
    internal class RotateAction(float angle = 0) : Action()
    {
        public override string Name => "Rotate";

        public override string Description => "画像を回転させる";

        public override string IconPath => "../Resources/rotation.png";


        public float Angle = angle;

        public override List<ActionParameter> Parameters =>
        [
            new ActionParameter<float>("float", "角度", in Angle)
        ];

        public override Dictionary<string, string> GetCommandParameters()
        {
            return new Dictionary<string, string>
            {
                { "-rotate", $"+{Angle}" }
            };
        }

        public override string ToString()
        {
            return $"{Angle}度回転させる";
        }
    }
}