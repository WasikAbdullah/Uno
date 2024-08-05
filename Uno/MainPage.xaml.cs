using Uno.Core;
using Uno.Core.Models;
using Uno.Views;

namespace Uno;

public partial class MainPage
{
    private readonly Game _game;
    private StackLayout CardStack => (StackLayout)CardTable[_game.Id];
    private bool CardsEnabled
    {
        set
        {
            foreach (var view in CardStack)
                ((Card)view).IsEnabled = value;
        }
    }
    private int CurrentPlayer
    {
        set
        {
            CardsEnabled = value == _game.Id;
            var stack = (StackLayout)CardTable[value];
            stack.BackgroundColor = StaticData.HighlightColor;
        }
    }
    
    public MainPage(Game game)
    {
        InitializeComponent();
        _game = game;
        for (var i = 0; i < 4; i++) AddStack(i);
        _game.Events.CardPenalty += CardPenalty;
        _game.Events.CardPicked += CardPicked;
        _game.Events.CardSet += CardSet;
    }

    private void CardSet(CardSet cardSet)
    {
        CurrentPlayer = cardSet.NextId;
        if(cardSet.Card is null) return;
        CardHolder.Content = new Card(cardSet.Card!.Value);
        var stack = (StackLayout)CardTable[cardSet.Id];
        stack.RemoveAt(stack.Count - 1);
    }

    private void CardPenalty(CardPenalty penalty)
    {
        foreach (var card in penalty.Cards)
        {
            var cardView = new Card(card);
            cardView.Clicked += OnCardPicked;
            CardStack.Add(cardView);
        }
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
        var rowColumn = StaticData.RowColumns[(4 + (i - _game.Id)) % 4];
        var stack = new StackLayout
        {
            Orientation = (rowColumn & 1) is 1 ? StackOrientation.Horizontal : StackOrientation.Vertical
        };
        Grid.SetColumn(stack, rowColumn & 0b11);
        Grid.SetRow(stack, rowColumn >> 2);
        CardTable.Add(stack);
    }
    
    private async void OnCardPicked(object? sender, EventArgs e)
    {
        var card = (Card)sender!;
        if(!card.IsValid(((Card?)CardHolder.Content!).CardValue)) return;
        CardsEnabled = false;
        if (card.IsBlack) card.CardValue |= await ColorPicker.GetColorAsync();
        CardHolder.Content = card;
        CardStack.Remove(card);
        CurrentPlayer = await _game.SetCardAsync(card.CardValue,CardStack.Count is 1);
    }
}