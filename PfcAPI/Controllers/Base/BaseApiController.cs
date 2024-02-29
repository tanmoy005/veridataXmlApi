
using Microsoft.AspNetCore.Mvc;

namespace PfcAPI.Controllers.Base
{
    public abstract class BaseApiController : ControllerBase
    {
        //protected readonly DbContextDB _context;
        //protected readonly IWorkFlowContext _workflowcontext;
        //protected readonly IuserContext _userContext;
        //protected readonly IEmailSender _emailSender;
        //protected readonly EmailConfiguration _config;
        //public BaseApiController(IWorkFlowContext workflowcontext, DbContextDB context, IuserContext userContext, IEmailSender emailSender, EmailConfiguration config)
        //{
        //    _context = context;
        //    _workflowcontext = workflowcontext;
        //    _userContext = userContext;
        //    _emailSender = emailSender;
        //    _config = config;
        //}
        //protected ActionResult ValidateModelState(ModelStateDictionary modelState)
        //{
        //    if (!modelState.IsValid)
        //    {
        //        var response = new BaseResponse<ErrorResponse>(HttpStatusCode.BadRequest,
        //            new ErrorResponse
        //            {
        //                ErrorCode = 500,
        //                UserMessage = "Invalid argument passed",
        //                InternalMessages = ModelState.Keys.SelectMany
        //                    (key => modelState[key].Errors.Select(y => y.ErrorMessage)).Distinct().ToList()
        //            });

        //        return BadRequest(response);
        //    }
        //    return Ok(HttpStatusCode.OK);
        //}
    }
}
