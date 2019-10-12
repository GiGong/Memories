using Memories.Business.Models;
using System.Windows.Media;

namespace Memories.Core.Extensions
{
    public static class MatrixExtension
    {
        public static Matrix ToMatrix(this BookUIMatrix matrix)
        {
            return new Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
        }

        public static BookUIMatrix ToBookUIMatrix(this Matrix matrix)
        {
            return new BookUIMatrix() { M11 = matrix.M11, M12 = matrix.M12, M21 = matrix.M21, M22 = matrix.M22, OffsetX = matrix.OffsetX, OffsetY = matrix.OffsetY };
        }
    }
}
