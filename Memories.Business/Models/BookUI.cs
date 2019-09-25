using System.Drawing;

namespace Memories.Business.Models
{
    public abstract class BookUI : BusinessBase
    {
        private Point _margin;

        /// <summary>
        /// Margin from left top.
        /// </summary>
        public Point Margin
        {
            get { return _margin; }
            set { SetProperty(ref _margin, value); }
        }

        private double _width;
        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }
    }
}