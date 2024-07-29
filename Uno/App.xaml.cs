using Uno.Pages;

namespace Uno;

public partial class App 
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}