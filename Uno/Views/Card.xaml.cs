using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno.Views;

public partial class Card : ContentView
{
    private static readonly string[] Colors =
    [
        "black",
        "red",
        "blue",
        "yellow",
        "green"
    ];
    
    public Card(int card)
    {
        InitializeComponent();
        Button.BackgroundColor = Color.Parse(Colors[card >> 4]);
        Button.Source = $"i{card & 0xF}.png";
    }
}