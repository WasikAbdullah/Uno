using Uno.Pages;

namespace Uno;

public partial class App 
{
    public App(HttpClient client)
    {
        InitializeComponent();
        MainPage = new AppShell(client);
    }
}