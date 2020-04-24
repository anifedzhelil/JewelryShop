namespace JewelryShop.Web.Infrastructure.VilidationAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class SaleDateLessThanAttribute : ValidationAttribute
    {
        private readonly string salePriceProperty;

        public SaleDateLessThanAttribute(string salePriceProperty)
        {
            this.salePriceProperty = salePriceProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var salePriceProp = validationContext.ObjectType.GetProperty(this.salePriceProperty);
            var salePriceValue = salePriceProp.GetValue(validationContext.ObjectInstance);

            if (value == null && salePriceValue == null)
            {
                return ValidationResult.Success;
            }
            else if (value != null && salePriceValue == null)
            {
                return new ValidationResult("Въведете промоционална цена");
            }
            else if (value != null)
            {
                DateTime currentValue = (DateTime)value;

                if (currentValue < DateTime.UtcNow)
                {
                    return new ValidationResult("Дата на промоцията трябва да бъде по-голяма от текущата дата");
                }
            }

            return ValidationResult.Success;
        }
    }
}
