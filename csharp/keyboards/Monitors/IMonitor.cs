using System.Threading.Tasks;

namespace keyboards.Monitors
{
    public interface IMonitor
    {
        Task<double> Percentage();
    }
}