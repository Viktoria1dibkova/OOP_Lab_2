using Library;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            var vm = (MainViewModel)DataContext;
            void Bind(RoutedCommand cmd, ICommand? vmCmd)
            {
                CommandBindings.Add(new CommandBinding(cmd, (s, e) => { vmCmd?.Execute(null); e.Handled = true; }, (s, e) => { e.CanExecute = vmCmd?.CanExecute(null) ?? false; e.Handled = true; }));
            }

            Bind(ApplicationCommands.New, vm.AddCommand);
            Bind(ApplicationCommands.Properties, vm.EditCommand);
            Bind(ApplicationCommands.Delete, vm.DeleteCommand);
            Bind(ApplicationCommands.Copy, vm.CopyCommand);
            Bind(ApplicationCommands.Paste, vm.PasteCommand);

            foreach (var grid in new[] { SourceGrid, TargetGrid })
            {
                grid.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => { vm.CopyCommand.Execute(null); e.Handled = true; }, (s, e) => { e.CanExecute = vm.CopyCommand.CanExecute(null); e.Handled = true; }));
                grid.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (s, e) => { vm.PasteCommand.Execute(null); e.Handled = true; }, (s, e) => { e.CanExecute = vm.PasteCommand.CanExecute(null); e.Handled = true; }));
                grid.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (s, e) => { vm.DeleteCommand.Execute(null); e.Handled = true; },(s, e) => { e.CanExecute = vm.DeleteCommand.CanExecute(null); e.Handled = true; }));
            }

            SourceGrid.SelectionChanged += (s, e) =>
            {
                vm.SelectedItems = SourceGrid.SelectedItems;
                CommandManager.InvalidateRequerySuggested();
            };
            Loaded += (_, __) => CommandManager.InvalidateRequerySuggested();

        }
    }
}