using Uno.Pages;

namespace Uno;

public class AppShell : Shell
{
    public AppShell(HttpClient client) => CurrentItem = new MainMenu(client);
}