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

namespace NirDobovizki.MvvmControls.Wpf.Demo.Demos
{
    /// <summary>
    /// Interaction logic for ValidationDemo.xaml
    /// </summary>
    [DisplayName("Validation Template")]
    public partial class ValidationDemo : UserControl
    {
        public ValidationDemo()
        {
            InitializeComponent();
        }

        private int _count;

        public string AlwaysThrow
        {
            get { return ""; }
            set
            {
                // switching to the debugger window causes WPF to retry 
                // the binding and so to rethrow the exception
                // to see this demo you need to run without breaking
                // on exceptions or without a debugger
                throw new Exception("error #"+(++_count).ToString()+": don't like " + value);
            }
        }
    }
}
