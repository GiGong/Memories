namespace Memories.Business.Models
{
    public class BookUIMatrix : BusinessBase
    {
        private double _m11;
        private double _m12;
        private double _m21;
        private double _m22;
        private double _offsetX;
        private double _offsetY;

        public double M11
        {
            get { return _m11; }
            set { SetProperty(ref _m11, value); }
        }

        public double M12
        {
            get { return _m12; }
            set { SetProperty(ref _m12, value); }
        }

        public double M21
        {
            get { return _m21; }
            set { SetProperty(ref _m21, value); }
        }

        public double M22
        {
            get { return _m22; }
            set { SetProperty(ref _m22, value); }
        }

        public double OffsetX
        {
            get { return _offsetX; }
            set { SetProperty(ref _offsetX, value); }
        }

        public double OffsetY
        {
            get { return _offsetY; }
            set { SetProperty(ref _offsetY, value); }
        }
    }
}
