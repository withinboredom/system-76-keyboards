using System.Threading.Tasks;
using keyboards.ColorSpace;

namespace keyboards.Filters
{
    public interface IFilter
    {
        Task PreApply(long time);
        
        Task<Rgb> ApplyFilter(Rgb color);
    }
}