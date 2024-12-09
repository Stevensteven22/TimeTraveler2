using CommunityToolkit.Mvvm.ComponentModel;

namespace TimeTraveler.Libary.ViewModels;

public abstract class ViewModelBase : ObservableObject {
    public virtual void SetParameter(object parameter) { }
}