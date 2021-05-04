using System.Threading.Tasks;

namespace EventsAPI.Controllers
{
    public interface IEmployeeService
    {
        Task<bool> IsActiveAsync(int id);
    }
}