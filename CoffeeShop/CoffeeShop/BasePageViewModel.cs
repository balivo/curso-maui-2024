
namespace CoffeeShop;

internal abstract partial class BasePageViewModel : BaseViewModel
{
    public virtual Task Initialize(object? parameter = null) => Task.CompletedTask;
}
