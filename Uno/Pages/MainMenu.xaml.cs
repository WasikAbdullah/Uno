using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Core;

namespace Uno.Pages;

public partial class MainMenu
{
    private readonly Game _game;
    
    public MainMenu(HttpClient client)
    {
        InitializeComponent();
        _game = new(client);
    }
    
    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        ((Button)sender!).IsEnabled = false;
        var create = !int.TryParse(GameId.Text,out var gameId);
        await _game.JoinAsync(UserName.Text, 0, create ? null : gameId);
        await Navigation.PushAsync(new MainPage(_game),true);
    }
}