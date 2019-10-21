namespace Memories.Business.Models
{
    public class BookUIPoint : BusinessBase
    {
        private double _x;
        private double _y;

        public double X
        {
            get { return _x; }
            set { SetProperty(ref _x, value); }
        }

        public double Y
        {
            get { return _y; }
            set { SetProperty(ref _y, value); }
        }

        public static bool operator ==(BookUIPoint source, BookUIPoint target)
        {
            return source.X == target.X && source.Y == target.Y;
        }

        public static bool operator !=(BookUIPoint source, BookUIPoint target)
        {
            return source.X != target.X || source.Y != target.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is BookUIPoint point)
            {
                return X == point.X && Y == point.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}