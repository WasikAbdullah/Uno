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
    private int CurrentCard
    {
        set => CardHolder.Content = new Card(value);
    }
    
    public MainPage(Game game)
    {
        InitializeComponent();
        _game = game;
        for (var i = 0; i < 4; i++) AddStack(i);
        _game.Events.Start += Start;
        _game.Events.CardPenalty += penalty => AddCardsSelf(penalty.Cards);
        _game.Events.CardPicked += pick => AddCards(pick.Id,pick.Count);
        _game.Events.CardSet += CardSet;
    }

    private void Start(Start start)
    {
        CurrentCard = start.StartCard;
        AddCardsSelf(start.Cards); 
        for (var i = 0; i < 4; i++)
             if(i != _game.Id) AddCards(i,7);
    }

    private void CardSet(CardSet cardSet)
    {
        CurrentPlayer = cardSet.NextId;
        if(cardSet.Card is null) return;
        CurrentCard = cardSet.Card.Value;
        var stack = (StackLayout)CardTable[cardSet.Id];
        stack.RemoveAt(stack.Count - 1);
    }

    private void AddCardsSelf(int[] cards)
    {
        foreach (var card in cards)
        {
            var cardView = new Card(card);
            cardView.Clicked += OnCardPicked;
            CardStack.Add(cardView);
        }
    }

    private void AddCards(int id,int count)
    {
        for (var i = 0; i < count; i++)
            ((StackLayout)CardTable[id]).Add(new Image
            {
                Source = "uno.png",
                HeightRequest = 100,
                WidthRequest = 100
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
        CardStack.Remove(card);
        CardHolder.Content = card;
        CurrentPlayer = await _game.SetCardAsync(card.CardValue,CardStack.Count is 1);
    }
}