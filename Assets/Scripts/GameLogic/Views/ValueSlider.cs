using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

internal sealed class ValueSlider : IDisposable
{
    private const int c_pointsSlideSpeed = 10;
    private readonly TextMeshProUGUI _slider;
    private readonly CancellationTokenSource _cts;

    public ValueSlider(TextMeshProUGUI slider)
    {
        _slider = slider;
        _cts = new CancellationTokenSource();
    }

    public async void Start(int from, int to) => await SlidePointsField(from, to);

    public void Stop() => _cts.Cancel();

    private async UniTask SlidePointsField(int from, int to)
    {
        if (from < to)
        {
            while (from <= to)
            {
                if (_cts.IsCancellationRequested) return;
                _slider.text = $"{from++}";
                await UniTask.DelayFrame(c_pointsSlideSpeed);
            }
        }
        else
        {
            while (from >= to)
            {
                if (_cts.IsCancellationRequested) return;
                _slider.text = $"{from--}";
                await UniTask.DelayFrame(c_pointsSlideSpeed);
            }
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}