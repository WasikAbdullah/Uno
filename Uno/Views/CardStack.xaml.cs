using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uno.Core.Models;

namespace Uno.Views;

public partial class CardStack : ContentView
{
    public event Func<int?, Task> CardPicked = null!;
    public int CurrentCard { get; set; }

    public CardStack()
    {
        InitializeComponent();
    }

    public void CardPenalty(CardPenalty penalty)
    {
        foreach (var card in penalty.Cards)
        {
            var cardView = new Card(card);
            cardView.Clicked += OnCardPicked;
            Stack.Add(cardView);
        }
    }
    
    private async void OnCardPicked(object? sender, EventArgs e)
    {
        var card = (Card)sender!;
        if (card.CardValue >> 4 is not 0 &&
            card.CardValue >> 4 != CurrentCard >> 4 &&
            (card.CardValue & 0xF) != (CurrentCard & 0xF)) return;
        if (card.CardValue >> 4 is 0)
            card.CardValue = await _colorPicker.GetColorAsync();
        await CardPicked(card.CardValue);
    }
}