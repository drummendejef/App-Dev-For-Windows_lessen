using System.Threading.Tasks;

namespace MVVMDemo.Repositories
{
    public interface IWeerRepository
    {
        Task<Rootobject> GetWeer(string location);
    }
}