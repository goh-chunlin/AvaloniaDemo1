using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvaloniaDemo1
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
