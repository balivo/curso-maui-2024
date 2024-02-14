using CommunityToolkit.Mvvm.ComponentModel;

namespace BuscaCep.ViewModels;

abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool _IsBusy = false;

    public bool IsNotBusy => !IsBusy;
}