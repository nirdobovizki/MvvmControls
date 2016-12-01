using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NirDobovizki.MvvmControls.Validation
{
    public class ValidationFrame : Decorator
    {
        public static DependencyProperty ValidatesOnDataErrorsProperty = DependencyProperty.Register("ValidatesOnDataErrors", typeof(bool?), typeof(ValidationFrame));
        public bool? ValidatesOnDataErrors
        {
            get { return (bool?)GetValue(ValidatesOnDataErrorsProperty); }
            set { SetValue(ValidatesOnDataErrorsProperty, value); }
        }
        public static DependencyProperty ValidatesOnExceptionsProperty = DependencyProperty.Register("ValidatesOnExceptions", typeof(bool?), typeof(ValidationFrame));
        public bool? ValidatesOnExceptions
        {
            get { return (bool?)GetValue(ValidatesOnExceptionsProperty); }
            set { SetValue(ValidatesOnExceptionsProperty, value); }
        }
        public static DependencyProperty ValidatesOnNotifyDataErrorsProperty = DependencyProperty.Register("ValidatesOnNotifyDataErrors", typeof(bool?), typeof(ValidationFrame));
        public bool? ValidatesOnNotifyDataErrors
        {
            get { return (bool?)GetValue(ValidatesOnNotifyDataErrorsProperty); }
            set { SetValue(ValidatesOnNotifyDataErrorsProperty, value); }
        }
        public static DependencyProperty ValidationRulesProperty = DependencyProperty.Register("ValidationRules", typeof(Collection<ValidationRule>), typeof(ValidationFrame));
        public Collection<ValidationRule> ValidationRules
        {
            get { return (Collection<ValidationRule>)GetValue(ValidationRulesProperty); }
            set { SetValue(ValidationRulesProperty, value); }
        }
        public static DependencyProperty ErrorsProperty = DependencyProperty.Register("Errors", typeof(ReadOnlyObservableCollection<ValidationError>), typeof(ValidationFrame));
        public ReadOnlyObservableCollection<ValidationError> Errors
        {
            get { return (ReadOnlyObservableCollection<ValidationError>)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }

        private List<WeakReference<ValidationFrame>> _childFrames = new List<WeakReference<ValidationFrame>>();
        private ValidationFrame _parentFrame;
        private List<WeakReference<FrameworkElement>> _controls = new List<WeakReference<FrameworkElement>>();
        private ObservableCollection<ValidationError> _errors = new ObservableCollection<ValidationError>();

        public ValidationFrame()
        {
            ValidationRules = new Collection<ValidationRule>();
            Errors = new ReadOnlyObservableCollection<ValidationError>(_errors);
            Loaded += OnLoaded;
        }

        private void UpdateErrors()
        {
            _errors.Clear();
            foreach (var current in new List<WeakReference<FrameworkElement>>(_controls))
            {
                FrameworkElement control;
                if (!current.TryGetTarget(out control))
                {
                    _controls.Remove(current);
                    continue;
                }
                foreach(var err in System.Windows.Controls.Validation.GetErrors(control))
                {
                    _errors.Add(err);
                }
            }
            foreach (var current in new List<WeakReference<ValidationFrame>>(_childFrames))
            {
                ValidationFrame frame;
                if (!current.TryGetTarget(out frame))
                {
                    _childFrames.Remove(current);
                    continue;
                }
                foreach (var err in frame.Errors)
                {
                    _errors.Add(err);
                }
            }

            if(_parentFrame!=null)
            {
                _parentFrame.UpdateErrors();
            }
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ConnectToParentFrame();
            SetBindingParameters(this);
        }

        private void SetBindingParameters(DependencyObject current)
        {
            // we don't drill down into controls that are
            // typically used with data templates, 
            // you need a ValidationFrame inside the template
            // to make this work
            var itemsControl = current as ItemsControl;
            if(itemsControl!=null)
            {
                if (itemsControl.ItemsSource != null) return;
            }
            var contentControl = current as ContentControl;
            if(contentControl!=null)
            {
                if ((contentControl.Content as System.Windows.Media.Visual) == null) return;
            }

            // handle supported controls
            var textBox = current as TextBox;
            if (textBox != null)
            {
                var bindingExp = textBox.GetBindingExpression(TextBox.TextProperty);
                if (bindingExp != null)
                {
                    Binding newBinding = new Binding();
                    foreach (var propAndVal in typeof(Binding).GetProperties().Where(o => o.CanWrite & o.CanRead).Select(o=>new { Prop = o, Val = o.GetValue(bindingExp.ParentBinding) }).Where(o=>o.Val!=null))
                        propAndVal.Prop.SetValue(newBinding, propAndVal.Val);
                    BindingOperations.ClearBinding(textBox, TextBox.TextProperty);
                    if (ValidatesOnDataErrors != null)
                        newBinding.ValidatesOnDataErrors = ValidatesOnDataErrors.Value;
                    if (ValidatesOnExceptions != null)
                        newBinding.ValidatesOnExceptions = ValidatesOnExceptions.Value;
                    if (ValidatesOnNotifyDataErrors != null)
                        newBinding.ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors.Value;
                    foreach (var currentRule in ValidationRules)
                        newBinding.ValidationRules.Add(currentRule);
                    newBinding.NotifyOnValidationError = true;
                    textBox.SetBinding(TextBox.TextProperty, newBinding);
                    _controls.Add(new WeakReference<FrameworkElement>(textBox));
                    System.Windows.Controls.Validation.AddErrorHandler(textBox, (s, e) => UpdateErrors());
                    //((INotifyCollectionChanged)System.Windows.Controls.Validation.GetErrors(textBox)).CollectionChanged += (s, e) => UpdateErrors();
                }

                return;
            }

            for(int i=0;i< VisualTreeHelper.GetChildrenCount(current);++i)
            {
                SetBindingParameters(VisualTreeHelper.GetChild(current, i));
            }
        }

        private void ConnectToParentFrame()
        {
            DependencyObject current = VisualTreeHelper.GetParent(this);
            while(current!=null)
            {
                _parentFrame = current as ValidationFrame;
                if(_parentFrame!=null)
                {
                    if (ValidatesOnDataErrors == null) ValidatesOnDataErrors = _parentFrame.ValidatesOnDataErrors;
                    if (ValidatesOnExceptions == null) ValidatesOnExceptions = _parentFrame.ValidatesOnExceptions;
                    if (ValidatesOnNotifyDataErrors == null) ValidatesOnNotifyDataErrors = _parentFrame.ValidatesOnNotifyDataErrors;
                    foreach (var currentRule in _parentFrame.ValidationRules) ValidationRules.Add(currentRule);
                    _parentFrame._childFrames.Add(new WeakReference<ValidationFrame>(this));
                    return;
                }
                current = VisualTreeHelper.GetParent(current);
            }
        }
    }
}
