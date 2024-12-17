using System;
using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using TimeTraveler.Libary.Services;
using TimeTraveler.Libary.ViewModels;
using TimeTraveler.Services;
using Ursa.Controls;

namespace TimeTraveler;

public class ServiceLocator {
    private readonly IServiceProvider _serviceProvider;

    private static ServiceLocator _current;

    public static ServiceLocator Current {
        get
        {
            if (_current is not null) {
                return _current;
            }

            if (Application.Current.TryGetResource(nameof(ServiceLocator),
                    out var resource) &&
                resource is ServiceLocator serviceLocator) {
                return _current = serviceLocator;
            }

            throw new Exception("理论上来讲不应该发生这种情况。");
        }
    }

    public ReturnViewModel ReturnViewModel =>
        _serviceProvider.GetService<ReturnViewModel>();
    
    public ResultViewModel ResultViewModel =>
        _serviceProvider.GetService<ResultViewModel>();
    
    public BackgroundViewModel BackgroundViewModel =>
        _serviceProvider.GetService<BackgroundViewModel>();
    
    public GameViewModel GameViewModel =>
        _serviceProvider.GetService<GameViewModel>();
    public MainWindowViewModel MainWindowViewModel =>
        _serviceProvider.GetService<MainWindowViewModel>();

    public MainViewModel MainViewModel =>
        _serviceProvider.GetService<MainViewModel>();

    public InitializationViewModel InitializationViewModel =>
        _serviceProvider.GetService<InitializationViewModel>();
    
    public GameTwoViewModel GameTwoViewModel =>
        _serviceProvider.GetService<GameTwoViewModel>();

    public BackgroundTwoViewModel BackgroundTwoViewModel =>
        _serviceProvider.GetService<BackgroundTwoViewModel>();

    public ResultTwoViewModel ResultTwoViewModel =>
        _serviceProvider.GetService<ResultTwoViewModel>();
    
    
    public ServiceLocator() {
        var serviceCollection = new ServiceCollection();

        //ViewModel
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddSingleton<MainViewModel>();
        serviceCollection.AddSingleton<InitializationViewModel>();
        serviceCollection.AddSingleton<GameViewModel>();
        serviceCollection.AddSingleton<BackgroundViewModel>();
        serviceCollection.AddSingleton<ReturnViewModel>();
        serviceCollection.AddSingleton<ResultViewModel>();
        serviceCollection.AddSingleton<GameTwoViewModel>();
        serviceCollection.AddSingleton<BackgroundTwoViewModel>();
        serviceCollection.AddSingleton<ResultTwoViewModel>();


        //Services
        serviceCollection
            .AddSingleton<IRootNavigationService, RootNavigationService>();
        serviceCollection
            .AddSingleton<IChapterNavigationService, ChapterNavigationService>();
        serviceCollection.AddSingleton<IBuffStorage, BuffStorage>();
        serviceCollection
            .AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
        serviceCollection
            .AddSingleton<IResultVerifyService, ResultVerifyService>();
        serviceCollection.AddSingleton<IElementalService, ElementalService>();
        serviceCollection
            .AddSingleton<IMazeService, MazeService>();
        serviceCollection
            .AddSingleton<ILargeModelService, LargeModelService>();
        
        //Others
       
        
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}