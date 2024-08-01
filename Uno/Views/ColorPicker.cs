using System.Diagnostics;

namespace Uno.Views;

public class ColorPicker : HorizontalStackLayout
{
    private TaskCompletionSource<int> _source = new();
    
    public ColorPicker()
    {
        IsVisible = false;
        foreach (var color in StaticData.Colors)
        {
            var button = new Button { BackgroundColor = Color.Parse(color) };
            button.Clicked += ButtonOnClicked;
            Add(button);
        }
    }

    private void ButtonOnClicked(object? sender, EventArgs e)
    {
        _source.SetResult(IndexOf((Button)sender!));
        //_source = new();
    }

    public async Task<int> GetColorAsync()
    {
        IsVisible = true;
        var color = await _source.Task;
        Debug.WriteLine(color);
        IsVisible = false;
        return color << 4;
    }
}