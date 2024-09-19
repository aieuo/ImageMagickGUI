using System.Windows.Media;
using ImageEditor.Models.Actions;
using ImageMagick;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.Models.Deserializer;

public class ActionDeserializer
{
    private static ActionDeserializer? _instance = null;

    private readonly Dictionary<string, Func<string[], Action>> _deserializers = [];

    private ActionDeserializer()
    {
    }

    public static ActionDeserializer GetInstance()
    {
        if (_instance != null) return _instance;

        _instance = new ActionDeserializer();
        _instance.Init();

        return _instance;
    }

    private void Init()
    {
        Add("Rotate", parameters =>
        {
            Console.WriteLine(parameters);
            return new RotateAction(
                float.Parse(parameters[0]),
                (Color)ColorConverter.ConvertFromString(parameters[1])
            );
        });
        Add("Flip", parameters => new FlipAction(
            (FlipAction.FlipType)Enum.Parse(typeof(FlipAction.FlipType), parameters[0])
        ));
        Add("Trim", parameters => new TrimAction(
            float.Parse(parameters[0])
        ));
        Add("Filter", parameters => new FilterAction(
            (FilterAction.FilterType)Enum.Parse(typeof(FilterAction.FilterType), parameters[0])
        ));
    }

    public void Add(string name, Func<string[], Action> deserializer)
    {
        if (!_deserializers.TryAdd(name, deserializer))
        {
            throw new ArgumentException($"Action {name} is already registered.");
        }
    }

    public List<Action> Deserialize(string json)
    {
        var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
        if (values == null)
        {
            throw new ArgumentException("Invalid action list file.");
        }

        var actions = new List<Action>();
        foreach (var data in values)
        {
            if (!data.TryGetValue("Name", out var value) || value is not string name)
            {
                throw new ArgumentException("Action name is invalid.");
            }

            if (data["Parameters"] is not JArray parameters)
            {
                throw new ArgumentException("Action parameter is invalid.");
            }
            
            var deserializer = _deserializers[name];
            actions.Add(deserializer(parameters.Select(p => p.ToString()).ToArray()));
        }

        return actions;
    }
}