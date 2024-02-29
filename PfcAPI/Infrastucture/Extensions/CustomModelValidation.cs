using PfcAPI.Model.RequestModel;
using System.ComponentModel.DataAnnotations;

namespace PfcAPI.Infrastucture.Extensions
{
    public class CustomModelValidation
    {
        public class Min18Years : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
            {
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit)
                {
                    if (Appointee?.DateOfBirth == null)
                        return new ValidationResult("Date of Birth is required, Please fill Date of Birth field.");

                    var age = DateTime.Today.Year - Appointee?.DateOfBirth.Value.Year;

                    return (age >= 18)
                        ? ValidationResult.Success
                        : new ValidationResult("Appointee should be at least 18 years old.");
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
                        msg = ($"{objname} {"field is required,"} {"Please fill the"} {objname} {"feild."}");

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
                        msg = ($"{objname} {"field is required,"} {"Please fill the"} {objname} {"feild."}");

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
                msg = ($"{name} {"field is required, "}{"Please fill the "}{name}{" feild."}");
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
                        msg = ($"{objname} {"field is required,"} {"Please fill the"} {objname} {"feild."}");
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
                var Appointee = (AppointeeSaveDetailsRequest)validationContext.ObjectInstance;
                if (Appointee.IsSubmit && Appointee.IsPassportAvailable == "Y")
                {
                    var objname = validationContext.DisplayName;
                    var msg = string.Empty;
                    if (value == null)
                    {
                        msg = ($"{objname} {"field is required,"} {"Please fill the"} {objname} {"feild."}");
                    }
                    else if (Convert.ToDateTime(value) > DateTime.Now)
                    {
                        msg = ($"{objname} {"must have past date ."}");
                    }

                    return (string.IsNullOrEmpty(msg))
                        ? ValidationResult.Success
                        : new ValidationResult(msg);
                }
                return ValidationResult.Success;
            }
        }

    }
}
