using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using Player.Avalonia.ViewModels;

namespace Player.Avalonia.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.Window = this;
            }
        };
    }
}
