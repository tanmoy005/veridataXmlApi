using System.ComponentModel.DataAnnotations;
using VERIDATA.Model.Request;

namespace VERIDATA.Model.Extensions
{
    public class CustomModelValidation
    {
        public class Min18Years : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;

                // Check if the appointee's date of birth is required
                if (appointee?.IsSubmit == true && appointee.DateOfBirth == null)
                    return new ValidationResult("Please fill in the Date of Birth field.");

                // If date of birth is provided, calculate age and check if they are at least 18 years old
                if (appointee?.DateOfBirth != null)
                {
                    var age = DateTime.Today.Year - appointee.DateOfBirth.Value.Year;

                    // Adjust for the case when the birthday hasn't occurred yet this year
                    if (DateTime.Today < appointee.DateOfBirth.Value.AddYears(age))
                        age--;

                    if (age < 18)
                        return new ValidationResult("Appointee should be at least 18 years old.");
                }

                return ValidationResult.Success;
            }
        }

        public class RequiredIfSubmit : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit)
                {
                    var objname = validationContext.DisplayName;
                    var msg = string.Empty;
                    if (value == null)
                        msg = ($"{"Please fill the"} {objname} {"field."}");

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class RequiredIfSubmitString : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit)
                {
                    var msg = ValidateString((string)value, validationContext.DisplayName);

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class RequiredIfPassportString : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit && Appointee.IsPassportAvailable == "Y")
                {
                    var msg = ValidateString((string)value, validationContext.DisplayName);

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class RequiredIfPassport : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit && Appointee.IsPassportAvailable == "Y")
                {
                    var objname = validationContext.DisplayName;
                    var msg = string.Empty;
                    if (value == null)
                        msg = ($"{"Please fill the"} {objname} {"field."}");

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class RequiredIfHandicap : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit && Appointee.IsHandicap == "Y")
                {
                    var msg = ValidateString((string)value, validationContext.DisplayName);

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        private static string ValidateString(string value, string? name)
        {
            var msg = string.Empty;
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                msg = ($"{"Please fill the "}{name}{" field."}");
            return msg;
        }

        public class RequiredAadhaarIfSubmit : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeFileDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit ?? false)
                {
                    var msg = ValidateString((string)value, validationContext.DisplayName);

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class Futuredate : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit && Appointee.IsPassportAvailable == "Y")
                {
                    var objname = validationContext.DisplayName;
                    var msg = string.Empty;
                    if (value == null)
                    {
                        msg = ($"{"Please fill the"} {objname} {"field."}");
                    }
                    else if (Convert.ToDateTime(value) < DateTime.Now)
                    {
                        msg = ($"{objname} {"must have future date ."}");
                    }

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

        public class Pastdate : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                var objName = validationContext.DisplayName;
                string msg = string.Empty;

                if (appointee.IsSubmit && appointee.IsPassportAvailable == "Y")
                {
                    if (value == null)
                    {
                        msg = $" Please fill the {objName} field.";
                    }
                    else
                    {
                        DateTime dateValue = Convert.ToDateTime(value);
                        if (dateValue > DateTime.Now)
                        {
                            msg = $"{objName} must have a past date.";
                        }
                    }
                }
                else if (appointee.IsPassportAvailable == "Y" && value != null)
                {
                    DateTime dateValue = Convert.ToDateTime(value);
                    if (dateValue > DateTime.Now)
                    {
                        msg = $"{objName} must have a past date.";
                    }
                    else if (dateValue < DateTime.Today.AddYears(-100)) // Restrict minimum date to 100 years before today
                    {
                        msg = $"{objName} cannot be older than 100 years.";
                    }
                }

                return string.IsNullOrEmpty(msg) ? ValidationResult.Success : new ValidationResult(msg);
            }
        }
    }
}