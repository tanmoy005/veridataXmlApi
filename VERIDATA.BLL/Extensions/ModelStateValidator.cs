using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using VERIDATA.Model.Base;

namespace VERIDATA.BLL.Extensions
{
    public class ModelStateValidator
    {
        public static IActionResult ValidateModelState(ActionContext context)
        {
            (string fieldName, ModelStateEntry entry) = context.ModelState
                .First(x => x.Value?.Errors.Count > 0);

            BaseResponse<ErrorResponse> response = new(HttpStatusCode.BadRequest,
                   new ErrorResponse
                   {
                       ErrorCode = 500,
                       UserMessage = "Invalid argument passed",
                       InternalMessages = context.ModelState.SelectMany
                           (key => key.Value.Errors.Select(y => y.ErrorMessage)).Distinct().ToList()
                   });

            BadRequestObjectResult result = new(response);

            return result;
        }
    }
}
