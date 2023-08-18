using Dignite.Abp.DynamicForms.Matrix;

namespace Dignite.Cms.Public.Web.Models
{
    public class MatrixBlockViewModel
    {
        public MatrixBlockViewModel(MatrixBlockType type, MatrixBlock block)
        {
            Type = type;
            Block = block;
        }

        public MatrixBlockType Type { get; set; }
        public MatrixBlock Block { get; set; }
    }
}
