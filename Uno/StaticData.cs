namespace Uno;

public static class StaticData
{
    public static Color HighlightColor { get; } = Color.Parse("#1abc9c");
    public static string[] Colors { get; } =
    [
        "black",
        "red",
        "blue",
        "yellow",
        "green"
    ];
    public static int[] RowColumns { get; } =
    [
        0b1001,
        0b0110,
        0b0001,
        0b0100
    ];
}