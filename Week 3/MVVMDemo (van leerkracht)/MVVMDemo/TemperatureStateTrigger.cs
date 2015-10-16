using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MVVMDemo
{
    public class TemperatureStateTrigger : StateTriggerBase
    {
        
        public float Temperature
        {
            get
            {
                return (float)GetValue(TemperatureProperty);
            }
            set
            {
                SetValue(TemperatureProperty, value);
            }
        }

        public static readonly DependencyProperty TemperatureProperty 
            = DependencyProperty.Register("Temperature", typeof(float),
                typeof(TemperatureStateTrigger), 
                new PropertyMetadata(true, OnTemperatureChanged));

        private static void OnTemperatureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TemperatureStateTrigger obj = (TemperatureStateTrigger)d;
            if ((float)e.NewValue > 18.0)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }

    }
}
