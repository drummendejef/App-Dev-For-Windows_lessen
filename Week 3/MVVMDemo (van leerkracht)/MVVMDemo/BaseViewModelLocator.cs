using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MVVMDemo.Repositories;
using MVVMDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMDemo
{
    public class BaseViewModelLocator
    {
        public BaseViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageVM>();
            SimpleIoc.Default.Register<RegistrationVM>();
            SimpleIoc.Default.Register<WeerVM>();

            SimpleIoc.Default.Register<ICountryRepository, CountryRepository>();
            SimpleIoc.Default.Register<IRegistrationRepository, RegistrationRepository>();
            SimpleIoc.Default.Register<ICityRepository, CityRepository>();
            SimpleIoc.Default.Register<IWeerRepository, WeerRepository>();
        }

        public MainPageVM MainPage
        {
            get { return ServiceLocator.Current.GetInstance<MainPageVM>(); }
        }

        public RegistrationVM Page2
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegistrationVM>();
            }
        }

        public WeerVM Weer
        {
            get { return ServiceLocator.Current.GetInstance<WeerVM>(); }
        }
    }
}
