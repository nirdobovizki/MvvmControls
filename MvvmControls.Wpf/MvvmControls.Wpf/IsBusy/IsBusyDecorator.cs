using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NirDobovizki.MvvmControls.IsBusy
{
    public class IsBusyDecorator : Decorator
    {
        public static DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(IsBusyDecorator),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, (s, ea) => ((IsBusyDecorator)s).IsBusyChanged(ea)));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        private void IsBusyChanged(DependencyPropertyChangedEventArgs ea)
        {
            if (Child != null)
            {
                Child.IsEnabled = !IsBusy;
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (IsBusy)
            {
                var centerX = ActualWidth / 2;
                var centerY = ActualHeight / 2;
                var drawing = new DrawingGroup();
                var radius = 30.0;
                var ballSize = 10.0;
                var transform = new RotateTransform();

                for (double i = 0; i < 8; ++i)
                {
                    var color = (byte)(double)(((1.0 - i / 8.0) * 255.0));
                    drawing.Children.Add(
                        new GeometryDrawing(
                            new SolidColorBrush(Color.FromArgb(255, color, color, color)),
                            null,
                            new EllipseGeometry(
                                new Point(
                                    centerX + radius * Math.Cos((Math.PI * 2 / 8.0) * i),
                                    centerY + radius * Math.Sin((Math.PI * 2 / 8.0) * i)),
                                ballSize, ballSize)));
                }

                transform.CenterX = centerX;
                transform.CenterY = centerY;
                transform.BeginAnimation(RotateTransform.AngleProperty,
                    new DoubleAnimation(0, 360, new Duration(TimeSpan.FromSeconds(1.5)))
                    {
                        RepeatBehavior = RepeatBehavior.Forever,
                    });

                drawing.Transform = transform;
                drawingContext.DrawDrawing(drawing);
            }
        }
    }
}
