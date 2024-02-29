using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using VERIDATA.Model.Base;

namespace PfcAPI.Infrastucture.Extensions
{
    public class ModelStateValidator
    {
        public static IActionResult ValidateModelState(ActionContext context)
        {
            (string fieldName, ModelStateEntry entry) = context.ModelState
                .First(x => x.Value.Errors.Count > 0);

            var response = new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest,
                   new ErrorResponse
                   {
                       ErrorCode = 500,
                       UserMessage = "Invalid argument passed",
                       InternalMessages = context.ModelState.SelectMany
                           (key => key.Value.Errors.Select(y => y.ErrorMessage)).Distinct().ToList()
                   });

            var result = new BadRequestObjectResult(response);

            return result;
        }
    }
}
