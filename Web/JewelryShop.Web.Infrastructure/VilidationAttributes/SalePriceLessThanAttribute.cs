namespace JewelryShop.Web.Infrastructure.VilidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class SalePriceLessThanAttribute : ValidationAttribute
    {
        private readonly string priceProperty;
        private readonly string saleDateProperty;

        public SalePriceLessThanAttribute(string priceProperty, string saleDateProperty)
        {
            this.priceProperty = priceProperty;
            this.saleDateProperty = saleDateProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var saleDateProp = validationContext.ObjectType.GetProperty(this.saleDateProperty);
            var saleDateValue = saleDateProp.GetValue(validationContext.ObjectInstance);

            if (value == null && saleDateValue == null)
            {
                return ValidationResult.Success;
            }
            else if (value != null && saleDateValue == null)
            {
                return new ValidationResult("Въведете промоционална дата");
            }
            else if (value != null)
            {
                this.ErrorMessage = this.ErrorMessageString;
                var currentValue = (decimal)value;
                var priceProp = validationContext.ObjectType.GetProperty(this.priceProperty);
                var priceValue = (decimal)priceProp.GetValue(validationContext.ObjectInstance);

                if (currentValue > priceValue)
                {
                    return new ValidationResult("Цена на промоцията трябва да бъде по-малка от редовната цена");
                }
            }

            return ValidationResult.Success;
        }
    }
}
