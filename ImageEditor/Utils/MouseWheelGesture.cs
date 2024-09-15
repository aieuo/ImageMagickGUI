using System.Windows.Input;

namespace ImageEditor.Utils;

// https://stackoverflow.com/questions/2271342/mousebinding-the-mousewheel-to-zoom-in-wpf-and-mvvm
public class MouseWheelGesture : MouseGesture
{
    public MouseWheelGesture() : base(MouseAction.WheelClick)
    {
    }

    public MouseWheelGesture(ModifierKeys modifiers) : base(MouseAction.WheelClick, modifiers)
    {
    }

    public static MouseWheelGesture CtrlUp =>
        new(ModifierKeys.Control) { Direction = WheelDirection.Up };

    public static MouseWheelGesture CtrlDown =>
        new(ModifierKeys.Control) { Direction = WheelDirection.Down };

    public static MouseWheelGesture ShiftUp =>
        new(ModifierKeys.Shift) { Direction = WheelDirection.Up };

    public static MouseWheelGesture ShiftDown =>
        new(ModifierKeys.Shift) { Direction = WheelDirection.Down };

    private WheelDirection Direction { get; set; }

    public override bool Matches(object targetElement, InputEventArgs inputEventArgs) =>
        base.Matches(targetElement, inputEventArgs)
        && inputEventArgs is MouseWheelEventArgs args
        && Direction switch
        {
            WheelDirection.None => args.Delta == 0,
            WheelDirection.Up => args.Delta > 0,
            WheelDirection.Down => args.Delta < 0,
            _ => false,
        };
}

public enum WheelDirection
{
    None,
    Up,
    Down,
}