using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NirDobovizki.MvvmControls.SpeedDial
{
    public class SpeedDialTicks : FrameworkElement
    {
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public static readonly DependencyProperty StepsProperty =
            DependencyProperty.Register("Steps", typeof(int), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsRender));
        public int Steps
        {
            get { return (int)GetValue(StepsProperty); }
            set { SetValue(StepsProperty, value); }
        }
        public static readonly DependencyProperty ScaleFormatProperty =
            DependencyProperty.Register("ScaleFormat", typeof(string), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata("G", FrameworkPropertyMetadataOptions.AffectsRender));
        public string ScaleFormat
        {
            get { return (string)GetValue(ScaleFormatProperty); }
            set { SetValue(ScaleFormatProperty, value); }
        }
        public static readonly DependencyProperty MinimumAngleProperty =
            DependencyProperty.Register("MinimumAngle", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MinimumAngle
        {
            get { return (double)GetValue(MinimumAngleProperty); }
            set { SetValue(MinimumAngleProperty, value); }
        }
        public static readonly DependencyProperty MaximumAngleProperty =
            DependencyProperty.Register("MaximumAngle", typeof(double), typeof(SpeedDialTicks),
            new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double MaximumAngle
        {
            get { return (double)GetValue(MaximumAngleProperty); }
            set { SetValue(MaximumAngleProperty, value); }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            for (int i = 0; i < Steps + 1; ++i)
            {
                var value = (Maximum - Minimum) / Steps * i + Minimum;
                var location = (MaximumAngle - MinimumAngle) / Steps * i + MinimumAngle;
                var rad = (90 - location) * Math.PI / 180;
                while (rad > Math.PI * 2) rad -= Math.PI * 2;
                while (rad < 0) rad += Math.PI * 2;
                var point = new Point(
                    ActualWidth / 2 + ActualWidth / 2 * Math.Cos(rad),
                    ActualHeight / 2 - ActualHeight / 2 * Math.Sin(rad));
                drawingContext.DrawRectangle(Brushes.Red, null, new Rect(point.X - 2, point.Y - 2, 4, 4));
                var ft = new FormattedText(value.ToString(ScaleFormat),
                        CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight,
                        new Typeface("Arial"), 15, Brushes.Red);
                if (Math.Abs(point.X - ActualWidth / 2) < 1)
                {
                    point = new Point(point.X - ft.Width / 2, point.Y);
                }
                else if (point.X > ActualWidth / 2)
                {
                    point = new Point(point.X - ft.Width, point.Y);
                }
                drawingContext.DrawText(ft, point);
            }
        }
    }
}
