using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using LibMpv.Client;
using LibMpv.MVVM;
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Player.Avalonia.ViewModels;

public partial class MainViewModel : BaseMpvContextViewModel
{
    [ObservableProperty] private Symbol playPauseSymbol = Symbol.PlayFilled;
    [ObservableProperty] private Symbol muteUnmuteSymbol = Symbol.Volume;
    
    public Window? Window { get; set; }

    public bool IsTextDurationsVisible => FunctionResolverFactory.GetPlatformId() != LibMpvPlatformID.Android;
    public MainViewModel()
    {
        this.Volume = 50;
    }

    [RelayCommand]
    private void PlayBigBuckBunny()
    {
        this.LoadFile("http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
        this.Play();
    }
    
    [RelayCommand]
    private async Task OpenAndPlayVideoFile()
    {
        // Ensure we have a window reference (e.g., if this is a ViewModel)
        var topLevel = TopLevel.GetTopLevel(Window);
        if (topLevel?.StorageProvider is null)
        {
            return;
        }

        // Open file picker
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select Video File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Video Files")
                {
                    Patterns = ["*.mp4", "*.mkv"]
                }
            }
        });

        if (files is { Count: > 0 })
        {
            var file = files[0];
            this.LoadFile(file.Path.LocalPath);
            this.Play();
        }
    }

    public override void InvokeInUIThread(Action action)
    {
        Dispatcher.UIThread.Invoke( action );
    }
}
