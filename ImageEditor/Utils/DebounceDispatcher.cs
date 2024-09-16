namespace ImageEditor.Utils;

// https://qiita.com/azarasi1226/items/f787626738a9ee26d4d2
public class DebounceDispatcher(int debounceTime = 300)
{
    private CancellationTokenSource? _cancellationToken = null;

    public void Debounce(Action func)
    {
        _cancellationToken?.Cancel();
        _cancellationToken = new CancellationTokenSource();

        Task.Delay(debounceTime, _cancellationToken.Token)
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    func();
                }
            }, TaskScheduler.Default);
    }
}