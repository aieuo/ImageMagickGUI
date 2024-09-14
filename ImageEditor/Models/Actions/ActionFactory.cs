using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor.Models.Actions
{
    internal class ActionFactory
    {
        private static ActionFactory? instance = null;

        private readonly Dictionary<string, Action> Actions = [];

        private ActionFactory()
        {
        }

        public static ActionFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new ActionFactory();
                instance.Init();
            }

            return instance;
        }

        private void Init()
        {
            RegisterAction(new RotateAction());
            RegisterAction(new FlipAction());
        }

        public void RegisterAction(Action action)
        {
            if (Actions.ContainsKey(action.Name))
            {
                throw new ArgumentException($"Action {action.Name} is already registered.");
            }

            Actions[action.Name] = action;
        }

        public Action Get(string name)
        {
            if (!Actions.ContainsKey(name))
            {
                throw new ArgumentException($"Action {name} is not registered.");
            }

            return (Action)Actions[name].Clone();
        }

        public List<Action> All()
        {
            return [.. Actions.Values];
        }
    }
}