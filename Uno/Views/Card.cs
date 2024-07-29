using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Views;

public class Card : ImageButton
{
    private static readonly string[] Colors =
    [
        "black",
        "red",
        "blue",
        "yellow",
        "green"
    ];
    private int _cardValue;
    public int CardValue
    {
        get => _cardValue;
        set
        {
            _cardValue = value;
            BackgroundColor = Color.Parse(Colors[value >> 4]);
        }
    }
    public bool IsBlack => CardValue >> 4 is 0;
    
    public Card(int card)
    {
        CardValue = card;
        Source = $"i{card & 0xF}.png";
    }

    public bool IsValid(int currentCard) =>
        IsBlack ||
        CardValue >> 4 == currentCard >> 4 ||
        (CardValue & 0xF) == (currentCard & 0xF);
}