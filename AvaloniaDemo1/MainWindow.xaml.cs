using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaDemo1.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaDemo1
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel() { UserName = "chunlingoh", BoardName = "anime-art" };

#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OnButtonClicked(object sender, RoutedEventArgs args)
        {
            var context = this.DataContext as MainWindowViewModel;

            context.ReadPinterest();
        }
    }
}
