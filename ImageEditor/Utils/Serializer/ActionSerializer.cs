using System.Text.Json;
using ImageEditor.Models.Actions.Parameters;
using Newtonsoft.Json;
using Action = ImageEditor.Models.Actions.Action;

namespace ImageEditor.Utils.Serializer;

public class ActionSerializer
{
    private static ActionSerializer? _instance = null;

    private ActionSerializer()
    {
    }

    public static ActionSerializer GetInstance()
    {
        if (_instance != null) return _instance;

        _instance = new ActionSerializer();

        return _instance;
    }
    
    public string Serialize(ICollection<Action> actions)
    {
        return JsonConvert.SerializeObject(actions.Select(action =>
        {
            return new Dictionary<string, object>
            {
                { "Name", action.Name },
                { "Parameters", action.Parameters.Select<ActionParameter, object>(p => p.ToString()) },
            };
        }), Formatting.Indented);
    }
}