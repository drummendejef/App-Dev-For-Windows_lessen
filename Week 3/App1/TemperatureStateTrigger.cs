using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace App1
{
    class TemperatureStateTrigger : StateTriggerBase
    {

        //Propdp tab tab, en dan dit aangemaakt
        public float Temperature
        {
            get { return (float)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Temperature.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register("Temperature", typeof(float), typeof(TemperatureStateTrigger), new PropertyMetadata(0));

        private static void OnTemperatureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TemperatureStateTrigger obj = (TemperatureStateTrigger)d;
            if((float)e.NewValue > 18.0)
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
