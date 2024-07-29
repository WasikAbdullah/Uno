using System.Diagnostics;
using Uno.Core;
using Uno.Core.Models;
using Uno.Views;

namespace Uno;

public partial class MainPage
{
    private static readonly int[] RowColumns =
    [
        0b1001,
        0b0110,
        0b0001,
        0b0100
    ];
    private readonly Game _game;
    private CardStack CardStack => (CardStack)CardTable[_game.Id!.Value];
    
    public MainPage(Game game)
    {
        InitializeComponent();
        _game = game;
        for (var i = 0; i < 4; i++) AddStack(i);
        _game.Events.CardPenalty += CardStack.CardPenalty;
        _game.Events.CardPicked += CardPicked;
        CardStack.CardPicked += _game.SetCardAsync;
    }

    private void CardPicked(CardPick pick)
    {
        for (var i = 0; i < pick.Count; i++)
            ((StackLayout)CardTable[pick.Id]).Add(new Image
            {
                Source = "uno.png",
                HeightRequest = 200
            });
    }

    private void AddStack(int i)
    {
        var rowColumn = RowColumns[(4 + (i - _game.Id!.Value)) % 4];
        IView stack = i == _game.Id ? new CardStack() : new StackLayout
        {
            Orientation = (rowColumn & 1) is 1 ? StackOrientation.Horizontal : StackOrientation.Vertical
        };
        CardTable.SetColumn(stack, rowColumn & 0b11);
        CardTable.SetRow(stack, rowColumn >> 2);
        CardTable.Add(stack);
    }
}