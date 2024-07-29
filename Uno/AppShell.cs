using Uno.Pages;

namespace Uno;

public class AppShell : Shell
{
    public AppShell() => CurrentItem = new MainMenu();
}