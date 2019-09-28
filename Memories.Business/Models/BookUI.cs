﻿using Memories.Business.Enums;

namespace Memories.Business.Models
{
    public abstract class BookUI : BusinessBase
    {
        #region Field

        private Point _margin;
        private double _width;
        private double _height;
        private int _zIndex;

        #endregion Field

        #region Property

        /// <summary>
        /// Property for json serialization
        /// </summary>
        public BookUIEnum UIType { get; set; }

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

        public int ZIndex
        {
            get { return _zIndex; }
            set { SetProperty(ref _zIndex, value); }
        }

        #endregion Property
    }
}