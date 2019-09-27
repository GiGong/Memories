namespace Memories.Business.Models
{
    public abstract class BookUI : BusinessBase
    {
        #region Field

        private Point _margin;
        private double _width;
        private double _height;

        #endregion Field

        #region Property

        /// <summary>
        /// Margin from left top.
        /// </summary>
        public Point Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        #endregion Property
    }
}