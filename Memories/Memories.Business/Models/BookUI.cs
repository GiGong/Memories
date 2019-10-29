using Memories.Business.Enums;

namespace Memories.Business.Models
{
    public abstract class BookUI : BusinessBase
    {
        #region Field

        private BookUIPoint _margin;
        private double _width;
        private double _height;
        private int _zIndex;
        private BookUIMatrix _transform;

        #endregion Field

        #region Property

        /// <summary>
        /// Property for json serialization
        /// </summary>
        public BookUIEnum UIType { get; set; }

        /// <summary>
        /// Margin from left top.
        /// </summary>
        public BookUIPoint Margin
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

        public int ZIndex
        {
            get { return _zIndex; }
            set { SetProperty(ref _zIndex, value); }
        }

        public BookUIMatrix Transform
        {
            get { return _transform; }
            set { SetProperty(ref _transform, value); }
        }

        #endregion Property

        #region Method

        public static BookUI GetBookUI(BookUIEnum type)
        {
            switch (type)
            {
                case BookUIEnum.TextUI:
                    return new BookTextUI();

                case BookUIEnum.ImageUI:
                    return  new BookImageUI();

                default:
                    throw new System.ArgumentOutOfRangeException("It's not " + typeof(BookUIEnum));
            }
        }

        #endregion Method
    }
}