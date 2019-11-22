using keyboards.ColorSpace;

namespace keyboards.Filters
{
    public interface IFilter
    {
        Rgb ApplyFilter(Rgb color);
    }
}