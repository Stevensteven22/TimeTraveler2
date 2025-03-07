using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using TimeTraveler.Libary.ViewModels;
using TimeTraveler.Views;

namespace TimeTraveler
{
    public class ViewLocator : IDataTemplate
    {
        private Dictionary<Type, Control> _cache = new();

        public Control Build(object data)
        {
            if (data is null)
                return null;

            var name = data.GetType()
                .FullName!.Replace("ViewModel", "View", StringComparison.Ordinal)
                .Replace($"{nameof(TimeTraveler)}.Libary.", $"{nameof(TimeTraveler)}.");
            var type = Type.GetType(name);

            if (type == null)
            {
                return new TextBlock { Text = "Not Found: " + name };
            }

            var control = _cache.TryGetValue(type, out var value)
                ? value
                : _cache[type] = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
