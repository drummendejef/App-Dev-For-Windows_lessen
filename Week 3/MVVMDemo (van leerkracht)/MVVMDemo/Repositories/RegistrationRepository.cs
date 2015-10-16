using MVVMDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private static List<Models.Registration> Registrations = new List<Models.Registration>();

        public void Add(Models.Registration r)
        {
            Registrations.Add(r);
        }

        public List<Models.Registration> Get()
        {
            return Registrations;
        }
    }
}
