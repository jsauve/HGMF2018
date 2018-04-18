using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace HGMF2018.Core
{
    public class CustomBackButtonPage : ContentPage
    {
        /// <summary>
        /// Gets or Sets the Back button click overriden custom action
        /// </summary>
        public Action CustomBackButtonAction { get; set; }

        public static readonly BindableProperty EnableBackButtonOverrideProperty = BindableProperty.Create(nameof(EnableBackButtonOverride), typeof(bool), typeof(CustomBackButtonPage), false, BindingMode.TwoWay);

        /// <summary>
        /// Gets or Sets Custom Back button overriding state
        /// </summary>
        public bool EnableBackButtonOverride
        {
            get
            {
                return (bool)GetValue(EnableBackButtonOverrideProperty);
            }
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
                OnPropertyChanged(nameof(EnableBackButtonOverride));
            }
        }
	}
}

