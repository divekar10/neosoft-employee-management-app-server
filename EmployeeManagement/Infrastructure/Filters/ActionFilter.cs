using EmployeeManagement.Infrastructure.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Utilities.Content;
using EmployeeManagement.Shared.Constants;

namespace EmployeeManagement.Infrastructure.Filters;

public class ActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // our code before action executes
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
            return;

        if (context.Result!.GetType() == typeof(FileContentResult))
            return;

        var result = context.Result;
        var responseObj = new ResponseModel
        {
            Message = string.Empty,
            StatusCode = 200
        };

        switch (result)
        {
            case OkObjectResult okresult:
                responseObj.Data = okresult.Value!;
                break;
            case ObjectResult objectResult:
                var data = (Dictionary<string, object>)(objectResult.Value!);

                var errorType = data.Keys.LastOrDefault();

                switch (errorType)
                {
                    case "Data":
                        responseObj.StatusCode = (int)HttpStatusCode.OK;
                        responseObj.Message = ContentLoader.ReturnLanguageData(Convert.ToString(data[Constants.RESPONSE_MESSAGE_FIELD])!, "")!;
                        responseObj.Data = data[Constants.RESPONSE_DATA_FIELD];
                        break;

                    case "ModelStateErrors":
                        responseObj.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                        responseObj.Message = ContentLoader.ReturnLanguageData(Convert.ToString(data[Constants.RESPONSE_MESSAGE_FIELD])!, "")!;
                        responseObj.Data = data[Constants.RESPONSE_MODEL_STATE_ERRORS_FIELD];
                        break;

                    default:
                        responseObj.StatusCode = (int)HttpStatusCode.NotFound;
                        responseObj.Message = "";
                        responseObj.Data = null;
                        break;
                }


                break;
            case JsonResult json:
                responseObj.Data = json.Value;
                break;
            case OkResult _:
            case EmptyResult _:
                responseObj.Data = null;
                break;
            case UnauthorizedResult _:
                responseObj.StatusCode = (int)HttpStatusCode.Unauthorized;
                responseObj.Message = ContentLoader.ReturnLanguageData("MSG101", "");
                responseObj.Data = null;
                break;
            default:
                responseObj.Data = result;
                break;
        }

        context.Result = new JsonResult(responseObj);
    }
}