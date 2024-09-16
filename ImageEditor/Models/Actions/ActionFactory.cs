using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Models.Actions;

internal class ActionFactory
{
    private static ActionFactory? _instance = null;

    private readonly Dictionary<string, Func<Action>> _actions = [];

    private ActionFactory()
    {
    }

    public static ActionFactory GetInstance()
    {
        if (_instance != null) return _instance;
            
        _instance = new ActionFactory();
        _instance.Init();

        return _instance;
    }

    private void Init()
    {
        Register(() => new RotateAction());
        Register(() => new FlipAction());
        Register(() => new TrimAction());
    }

    public void Register(Func<Action> actionCreator)
    {
        var action = actionCreator();
        if (!_actions.TryAdd(action.Name, actionCreator))
        {
            throw new ArgumentException($"Action {action.Name} is already registered.");
        }
    }

    public Action Get(string name)
    {
        if (!_actions.TryGetValue(name, out var creator))
        {
            throw new ArgumentException($"Action {name} is not registered.");
        }

        return creator();
    }

    public List<Action> All()
    {
        return _actions.Values.Select(creator => creator()).ToList();
    }
}