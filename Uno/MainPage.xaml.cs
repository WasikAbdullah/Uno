using Uno.Core;
using Uno.Views;

namespace Uno;

public partial class MainPage
{
    private readonly Game _game;
    
    public MainPage(Game game)
    {
        InitializeComponent();
        _game = game;
        var layout = new StackLayout()
        {
            Children = { new Card(0b0010001) }
        };
        Microsoft.Maui.Controls.Grid.SetColumn(layout,1);
        Microsoft.Maui.Controls.Grid.SetRow(layout,0);
        Grid.Add(layout);
    }
}