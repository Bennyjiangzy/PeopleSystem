using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ValidationHelper
    {
        /// <summary>
        /// Check given obj by the custom validation model
        /// </summary>
        /// <param name="obj">Checked model</param>
        /// <exception cref="ArgumentException">
        /// If there is one or more invalid input, return it with error messages
        /// </exception>
        internal static void ModelValidation(object obj) 
        {
            ValidationContext validationContext = new ValidationContext(obj);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid_Model = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!isValid_Model)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
