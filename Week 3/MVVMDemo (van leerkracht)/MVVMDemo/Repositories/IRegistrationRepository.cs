using System.Collections.Generic;

namespace MVVMDemo.Repositories
{
    public interface IRegistrationRepository
    {
        void Add(Models.Registration r);
        List<Models.Registration> Get();
    }
}