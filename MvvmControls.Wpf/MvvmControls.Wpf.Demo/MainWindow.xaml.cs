using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MvvmControls.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class DemoModel
        {
            public string Name { get; set; }
            public Type Type { get; set; }
        }

        public class SelectDemoCommand : ICommand
        {
            private MainWindow _owner;

            public SelectDemoCommand(MainWindow owner)
            {
                _owner = owner;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                var model = (DemoModel)parameter;
                _owner._title.Text = model.Name;
                _owner._demo.Child = (UIElement)Activator.CreateInstance(model.Type);
            }
        }

        public ICommand SwitchDemoCommand { get; set; }

        private string GetDisplayName(Type type)
        {
            var attr = type.GetCustomAttributes(typeof(DisplayNameAttribute),false);
            if (attr.Length == 0) return null;
            return ((DisplayNameAttribute)attr[0]).DisplayName;
        }

        public MainWindow()
        {
            SwitchDemoCommand = new SelectDemoCommand(this);
            DataContext = GetType().Assembly.GetTypes().Select(o => new DemoModel { Name = GetDisplayName(o), Type = o }).Where(o => o.Name != null).ToList();
            InitializeComponent();
        }
    }
}
