using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace NirDobovizki.MvvmControls.AdornerLib
{
    public class AdornedPlaceholder : FrameworkElement
    {
        public Adorner Adorner
        {
            get
            {
                Visual current = this;
                while (current != null && !(current is Adorner))
                {
                    current = (Visual)VisualTreeHelper.GetParent(current);
                }

                return (Adorner)current;
            }
        }

        public FrameworkElement AdornedElement
        {
            get
            {
                return Adorner == null ? null : Adorner.AdornedElement as FrameworkElement;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var controlAdorner = Adorner as ControlAdorner;
            if (controlAdorner != null)
            {
                controlAdorner.Placeholder = this;
                controlAdorner.DataContext = AdornedElement.DataContext;
            }

            FrameworkElement e = AdornedElement;
            return new Size(e.ActualWidth, e.ActualHeight);
        }
    }

}
