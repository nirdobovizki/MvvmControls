using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace NirDobovizki.MvvmControls.Utils
{
    public class ChangeAnimator : FrameworkElement
    {
        public static readonly DependencyProperty SourceValueProperty =
            DependencyProperty.Register("SourceValue", typeof(double), typeof(ChangeAnimator),
            new PropertyMetadata(0.0,
                (s, e) =>
                {
                    ChangeAnimator me = (ChangeAnimator)s;
                    if (me.SourceMin == me.SourceMax) return;
                    me.BeginAnimation(TargetValueProperty,
                        new DoubleAnimation(
                            ((((double)e.NewValue - me.SourceMin) / (me.SourceMax - me.SourceMin)) * (me.TargetMax - me.TargetMin)) + me.TargetMin,
                            new Duration(TimeSpan.FromSeconds(0.5))), HandoffBehavior.SnapshotAndReplace);
                }));
        public double SourceValue
        {
            get { return (double)GetValue(SourceValueProperty); }
            set { SetValue(SourceValueProperty, value); }
        }
        public static readonly DependencyProperty SourceMinProperty =
            DependencyProperty.Register("SourceMin", typeof(double), typeof(ChangeAnimator));
        public double SourceMin
        {
            get { return (double)GetValue(SourceMinProperty); }
            set { SetValue(SourceMinProperty, value); }
        }
        public static readonly DependencyProperty SourceMaxProperty =
            DependencyProperty.Register("SourceMax", typeof(double), typeof(ChangeAnimator),
            new PropertyMetadata(100.0));
        public double SourceMax
        {
            get { return (double)GetValue(SourceMaxProperty); }
            set { SetValue(SourceMaxProperty, value); }
        }
        public static readonly DependencyProperty TargetValueProperty =
            DependencyProperty.Register("TargetValue", typeof(double), typeof(ChangeAnimator));
        public double TargetValue
        {
            get { return (double)GetValue(TargetValueProperty); }
            set { SetValue(TargetValueProperty, value); }
        }
        public static readonly DependencyProperty TargetMinProperty =
            DependencyProperty.Register("TargetMin", typeof(double), typeof(ChangeAnimator));
        public double TargetMin
        {
            get { return (double)GetValue(TargetMinProperty); }
            set { SetValue(TargetMinProperty, value); }
        }
        public static readonly DependencyProperty TargetMaxProperty =
            DependencyProperty.Register("TargetMax", typeof(double), typeof(ChangeAnimator));
        public double TargetMax
        {
            get { return (double)GetValue(TargetMaxProperty); }
            set { SetValue(TargetMaxProperty, value); }
        }
    }
}

