using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Memories.Core.Controls
{
    public class DrawableCanvas : Canvas
    {
        #region Field

        private Point _startPoint;
        private Rectangle _rect;

        #endregion Field

        #region Property

        public Brush Stroke { get; set; }
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Last Rectangle drawn
        /// </summary>
        public Rectangle Result { get; private set; }

        #endregion Property

        #region Dependency Property

        public static readonly DependencyProperty IsDrawProperty =
            DependencyProperty.Register(nameof(IsDraw), typeof(bool), typeof(DrawableCanvas), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsDrawChangedCallBack)));

        public static readonly DependencyProperty DrawEndCommandProperty =
            DependencyProperty.Register(nameof(DrawEndCommand), typeof(ICommand), typeof(DrawableCanvas), new PropertyMetadata(null));

        public bool IsDraw
        {
            get { return (bool)GetValue(IsDrawProperty); }
            //Why Binding doesn't work in SetValue, but work in SetCurrentValue?
            set { SetCurrentValue(IsDrawProperty, value); }
        }

        public ICommand DrawEndCommand
        {
            get { return (ICommand)GetValue(DrawEndCommandProperty); }
            set { SetValue(DrawEndCommandProperty, value); }
        }

        #endregion Dependency Property

        #region Event

        public event EventHandler<Rectangle> DrawEnded;

        #endregion Event

        #region Constructor

        public DrawableCanvas()
        {
            Result = null;

            Stroke = Brushes.Black;
            StrokeThickness = 2;

            Background = new SolidColorBrush(Colors.White) { Opacity = 0.5 };

            Visibility = Visibility.Collapsed;
        }

        #endregion Constructor

        #region Method

        private static void OnIsDrawChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DrawableCanvas canvas)
            {
                canvas.OnIsDrawChanged();
            }
        }

        private void OnIsDrawChanged()
        {
            if (IsDraw)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                ClearDraw();
            }
        }

        public void ClearDraw()
        {
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();
            }
            if (Children.Contains(_rect))
            {
                Children.Remove(_rect);
            }
            _rect = null;
            Visibility = Visibility.Collapsed;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(this);

            _rect = new Rectangle
            {
                Stroke = Stroke,
                StrokeThickness = StrokeThickness
            };
            SetLeft(_rect, _startPoint.X);
            SetTop(_rect, _startPoint.Y);

            Children.Add(_rect);
            CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released || _rect == null)
            {
                return;
            }

            var pos = e.GetPosition(this);

            if (pos.X < 0)
            {
                pos.X = 0;
            }
            else if (pos.X > ActualWidth)
            {
                pos.X = ActualWidth;
            }

            if (pos.Y < 0)
            {
                pos.Y = 0;
            }
            else if (pos.Y > ActualHeight)
            {
                pos.Y = ActualHeight;
            }

            var x = Math.Min(pos.X, _startPoint.X);
            var y = Math.Min(pos.Y, _startPoint.Y);

            var w = Math.Max(pos.X, _startPoint.X) - x;
            var h = Math.Max(pos.Y, _startPoint.Y) - y;

            _rect.Width = w;
            _rect.Height = h;

            SetLeft(_rect, x);
            SetTop(_rect, y);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_rect == null)
            {
                return;
            }

            DrawEnded?.Invoke(this, _rect);
            if (DrawEndCommand != null && DrawEndCommand.CanExecute(_rect) == true)
            {
                DrawEndCommand.Execute(_rect);
            }
            Result = _rect;
            IsDraw = false;
            ClearDraw();
        }

        #endregion Method
    }
}

