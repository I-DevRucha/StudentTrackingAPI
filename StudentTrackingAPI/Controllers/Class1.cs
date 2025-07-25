﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Dapper;
//using Abp.Domain.Entities;

using System.Net;

using System.Data;
using Microsoft.Extensions.ObjectPool;
//using System.Web.Http.Results;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
//using System.Linq.Dynamic.Core.Tokenizer;

using Newtonsoft.Json.Linq;

using Microsoft.Extensions.Configuration;

//using System.Web.Http.Filters;
using IActionFilter = Microsoft.AspNetCore.Mvc.Filters.IActionFilter;
using Context;


using ActionExecutedContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext;
using ActionExecutingContext = Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;
using ExceptionContext = Microsoft.AspNetCore.Mvc.Filters.ExceptionContext;
using System.Diagnostics.Metrics;

using IExceptionFilter = Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter;
using System.Diagnostics;

using static common.ExampleFilterAttribute;
using Token;
using StudentTrackingAPI.DataAccess;
using StudentTrackingAPI.Core.ModelDtos;
using Org.BouncyCastle.Utilities.Net;
using DocumentFormat.OpenXml.Drawing.Charts;
using StudentTrackingAPI.DataAccess.Context;
//using ModelState = System.Web.Mvc.ModelState;

namespace common
{


    public class ExampleFilterAttribute : Attribute, IOrderedFilter, IActionFilter
    {
        private TokenRepo? _loginRepo;
        private DatabaseContext? _context;

        private IConfiguration? con;

        //public ExampleFilterAttribute(IConfiguration configuration)
        //{
        //    con = configuration;
        //}

        public int Order { get; set; }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            dynamic? newResult = null;
            dynamic data = ((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value;
            var expiration = "";
            string Token = "";
            string SessionId = "";
            string IpAddress = "";

            try
            {
                if (context.Result is ObjectResult objectResult)
                {

                    if (data != null && data.GetType().GetProperty("Data") != null)

                    {
                        if (data != null && data.Data != null)
                        {
                            //Guid? Id = null;
                            dynamic data1 = data.Data;



                            //Guid? Id = data1.UserId ?? data1.CreatedBy;
                            string? Id = data.UserId ?? data1.UserId ?? data1.createdBy;
                            //string? Id = null;
                            //if (data1 is IEnumerable<dynamic> data1Enumerable)
                            //{
                            //	foreach (var item in data1Enumerable)
                            //	{
                            //		if (item.UserId != null)
                            //		{
                            //			Id = item.UserId;
                            //			break;
                            //		}
                            //	}
                            //}
                            //else
                            //{
                            //	Id = data.UserId ?? data1.UserId ?? data1.createdBy;
                            //}


                            var httpContext = context.HttpContext;
                            con = httpContext.RequestServices.GetService<IConfiguration>(); // Retrieve IConfiguration from the HttpContext
                            _context = new DatabaseContext(con);
                            _loginRepo = new TokenRepo(_context);

                            //Creating New Token While one operation
                            jwtTokenCreate tk = new jwtTokenCreate(con);
                            var GetAnotherToken = tk.createToken(Id);
                            Token = new JwtSecurityTokenHandler().WriteToken(GetAnotherToken);
                            expiration = GetAnotherToken.ValidTo.ToString();
                            string ipaddress = context.HttpContext.Request.Headers["ipaddress"].ToString();
                            string sessionid = context.HttpContext.Request.Headers["sessionid"].ToString();
                            SessionId = sessionid;
                            IpAddress = ipaddress;


                            if (string.IsNullOrEmpty(ipaddress))
                            {
                                string? sessionIdd = data.SessionId ?? data1.SessionId;
                                string? IpAddressd = data.IpAddress ?? data1.IpAddress;
                                SessionId = sessionIdd;
                                IpAddress = IpAddressd;

                            }
                            var OperationType = "UpdateToken";
                            //string? connectionstring = data1.BaseModel.Server_Value;
                            dynamic InsertTokenResult = _loginRepo.InsertToken(Token, expiration, Id, OperationType, SessionId, IpAddress);

                            //Token encrypted
                            string encrypttext = tk.Encrypt(Token, "abcdefghijklmnop");
                            Outcome outcome = null;
                            // Create a new instance of Outcome and assign values
                            if (data.Outcome == null)
                            {
                                outcome = new Outcome
                                {

                                    OutcomeId = 1,
                                    OutcomeDetail = "GetData",
                                    Tokens = encrypttext,
                                    Expiration = expiration
                                };
                            }
                            else
                            {
                                outcome = new Outcome
                                {

                                    OutcomeId = data.Outcome.OutcomeId,
                                    OutcomeDetail = data.Outcome.OutcomeDetail,
                                    Tokens = encrypttext,
                                    Expiration = expiration
                                };
                            }

                            newResult = new Result
                            {
                                Outcome = outcome,
                                Data = data1
                            };



                            // Update the context.Result with the new ObjectResult
                            context.Result = new ObjectResult(newResult)
                            {
                                StatusCode = objectResult.StatusCode ?? StatusCodes.Status200OK
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }




        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {


            try
            {


                string? idValue = null; string? SessionId = null; string? IpAddress = null; string? sessionIdd = null; string? ipAddressd = null;

                var request = context.ActionArguments.Values.FirstOrDefault() != null ? context.ActionArguments.Values.FirstOrDefault() : context.HttpContext.Request.Form;

                string authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                string ipaddress = context.HttpContext.Request.Headers["ipaddress"].ToString();
                string sessionid = context.HttpContext.Request.Headers["sessionid"].ToString();

                if (string.IsNullOrEmpty(ipaddress))
                {
                    sessionIdd = context.ActionArguments.TryGetValue("sessionid", out var sessionidValue) ? sessionidValue?.ToString() : null;
                    ipAddressd= context.ActionArguments.TryGetValue("ipaddress", out var ipaddressValue) ? ipaddressValue?.ToString() : null;
                    SessionId=sessionIdd;

                    IpAddress=ipAddressd;

                }
                jwtTokenCreate tk = new jwtTokenCreate(con);
                //string Tokenkey = tk.Decrypt(authorizationHeader, "abcdefghijklmnop");
                dynamic idProperty = request.GetType().GetProperty("UserId")?? request;

                if (idProperty == null)
                {
                    if (request.GetType().GetProperty("Id") != null)
                    {

                    }
                    else if (request is Guid)
                    {
                        idValue = request.ToString();
                    }
                    else //if (context.HttpContext.Request.Form != null)
                    {
                        dynamic? formData = context.HttpContext.Request.Form;
                        // Fetch a specific field from the form data
                        string userIdValue = formData["UserId"];
                        string Sessiondata = ipaddress;
                        string IpAddressData = sessionid;
                        //idValue = formData["UserId"];
                        //if (!string.IsNullOrEmpty(userIdValue) && Guid.TryParse(userIdValue, out Guid userIdGuid))
                        //{
                        idValue = userIdValue;// userIdGuid;
                        SessionId = sessionid;
                        IpAddress = ipaddress;  //}

                        if (string.IsNullOrEmpty(ipaddress))
                        {
                            SessionId=sessionIdd;
                            IpAddress=ipAddressd;

                        }

                    }
                    //else
                    //{
                    //    idValue = (Guid?)request;
                    //}
                }
                else
                {

                    var requestType = request.GetType();
                    var propertyInfo = requestType.GetProperty("UserId");

                    if (propertyInfo != null)
                    {
                        idValue = (idProperty?.GetValue(request));
                        SessionId = sessionid;
                        IpAddress = ipaddress;
                        if (string.IsNullOrEmpty(ipaddress))
                        {
                            SessionId=sessionIdd;
                            IpAddress=ipAddressd;

                        }
                    }
                    else
                    {
                        // If propertyInfo is null, fallback to using the request object itself

                        var request1 = context.HttpContext.Request;
                        if (request1.HasFormContentType)
                        {
                            var formData = request1.Form;
                            if (formData.ContainsKey("UserId"))
                            {
                                idValue = formData["UserId"].ToString();
                                SessionId = sessionid;
                                IpAddress = ipaddress;
                                if (string.IsNullOrEmpty(ipaddress))
                                {
                                    SessionId=sessionIdd;
                                    IpAddress=ipAddressd;

                                }
                            }
                        }
                        else
                        {
                            SessionId = sessionid;
                            IpAddress = ipaddress;
                            if (string.IsNullOrEmpty(ipaddress))
                            {
                                SessionId=sessionIdd;
                                IpAddress=ipAddressd;

                            }
                            idValue = (string)request;
                        }
                    }



                    // idValue = (idProperty?.GetValue(request))?? request;
                }
                var httpContext = context.HttpContext;
                con = httpContext.RequestServices.GetService<IConfiguration>();
                _context = new DatabaseContext(con);
                _loginRepo = new TokenRepo(_context);

                string ResultforToken = null;
                //SessionId = request.SessionId.ToString;
                //IpAddress = data.IpAddress;


                if (idValue != null)
                {
                    string Tokenkey = tk.Decrypt(authorizationHeader, "abcdefghijklmnop");

                    Task.Run(async () =>
                    {
                        ResultforToken = await _loginRepo.ValidateTokenAsync(Tokenkey, idValue, SessionId, IpAddress);
                        if (ResultforToken.ToString() == "false")
                        {
                            // context.Result = new RedirectToActionResult("Unauthorised", "UnAuthorised", null);

                            // Response.Headers.Add("WWW-Authenticate", "YourChallengeHere");

                            // Return 403 Forbidden status code with the error message
                            // The error message will be sent in the response body
                            // StatusCode(StatusCodes.Status403Forbidden, "Authentication failed.");

                            var outcome = new Outcome
                            {

                                OutcomeId = 0,
                                OutcomeDetail = "Unauthorised",
                                Tokens = null,
                                Expiration = null
                            };


                            var newResult = new Result
                            {
                                Outcome = outcome,
                                Data = "Unauthorised"
                            };



                            // Update the context.Result with the new ObjectResult
                            ObjectResult objectResult = new ObjectResult(newResult);
                            context.Result = new ObjectResult(newResult)
                            {
                                StatusCode = 401
                            };
                        }
                    }).Wait();
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }


        }

        //protected void OnException(ExceptionContext filterContext)
        //{
        //    var httpContext = filterContext.HttpContext;
        //    con = httpContext.RequestServices.GetService<IConfiguration>(); // Retrieve IConfiguration from the HttpContext
        //    _context = new DapperContext(con);
        //    ExceptionHandle objException = new ExceptionHandle(_context);
        //    var controllerName = (string)filterContext.RouteData.Values["controller"];
        //    var actionName = (string)filterContext.RouteData.Values["action"];
        //    var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
        //    Exception ex = filterContext.Exception;



        //    //CookieUtility cookiesutility = new CookieUtility();
        //    //var cookiedetails = cookiesutility.GetHttpCookieParameter();
        //    filterContext.Result = new ViewResult
        //    {
        //        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
        //        TempData = filterContext.Controller.TempData
        //    };



        //    ViewData.Model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);



        //    StringBuilder str = new StringBuilder();



        //    //string m;
        //    string[] arrKeys = new string[100];
        //    string[] arrValues = new string[100];
        //    StringBuilder strAppend = new StringBuilder();
        //    string value = "";



        //    int i = 0;



        //    foreach (string item in ViewData.ModelState.Keys)
        //    {



        //        ModelState state;
        //        //  m = item;



        //        if (ModelState.TryGetValue(item, out state))
        //        {
        //            if (state.Value != null)
        //            {
        //                if (state.Value.AttemptedValue != null)
        //                {
        //                    value = Convert.ToString(state.Value.AttemptedValue);
        //                }
        //            }
        //        }
        //        arrKeys[i] = item;
        //        arrValues[i] = value;
        //        i++;
        //    }



        //    for (int j = 0; j < ViewData.ModelState.Keys.Count; j++)
        //    {
        //        string strconcat = arrKeys[j] + " : " + arrValues[j] + " ";
        //        strAppend = str.Append(strconcat);



        //    }



        //    string resultobj = objException.InsertExceptionLog(strAppend.ToString(), ex.Message, ex.StackTrace, ex.TargetSite.ToString(), controllerName, actionName, cookiedetails.UserId);



        //}
        //public void OnException(ExceptionContext context)
        //{
        //    var httpContext = context.HttpContext;
        //    var controllerName = (string)context.RouteData.Values["controller"];
        //    var actionName = (string)context.RouteData.Values["action"];
        //    var model = new HandleErrorInfo(context.Exception, controllerName, actionName);
        //    con = httpContext.RequestServices.GetService<IConfiguration>();// Retrieve IConfiguration from the HttpContext

        //    _context = new DatabaseContext(con);
        //    //ExceptionHandle objException = new ExceptionHandle(_context);




        //    // Log the exception here using _context

        //    // Create a StringBuilder to build the error message
        //    // var errorDetails = new StringBuilder();

        //    //foreach (var key in context.ModelState.Keys)
        //    //{
        //    //    if (context.ModelState.TryGetValue(key, out var state) && state.Value is string stringValue && stringValue != null)
        //    //    {
        //    //        var value = Convert.ToString(stringValue);
        //    //        errorDetails.Append($"{key} : {value} ");
        //    //    }
        //    //}

        //    //errorDetails.Append($"Exception Message: {context.Exception.Message} ");
        //    //errorDetails.Append($"Exception Stack Trace: {context.Exception.StackTrace} ");
        //    //errorDetails.Append($"Exception Target Site: {context.Exception.TargetSite} ");
        //    //errorDetails.Append($"Controller: {controllerName} ");
        //    //errorDetails.Append($"Action: {actionName} ");

        //    // Now, log errorDetails to your desired log storage using _context

        //    //context.Result = new ViewResult
        //    //{
        //    //    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
        //    //    TempData = context.Controller.TempData
        //    //};
        //    //context.Result = new ViewResult
        //    //{

        //    //    ViewName = View,
        //    //    MasterName = Master,
        //    //    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
        //    //    TempData = context.Controller.TempData
        //    //};


        //    var apiPath = context.ActionDescriptor.AttributeRouteInfo.Template;

        //    //EX
        //    ExHandle ExHan = new ExHandle();
        //    ExHan.Message = model.Exception.Message;
        //    //ExHan.Message = httpContext.Message;
        //    //ExHan.Message = ex.Message;
        //    ExHan.Source = model.Exception.Source;
        //    ExHan.InnerException = model.Exception.InnerException;
        //    ExHan.API = apiPath;
        //    if (ExHan.BaseModel == null)
        //    {
        //        ExHan.BaseModel = new BaseModel();
        //    }

        //    ExHan.BaseModel.OperationType = "InsertException";
        //    // ExceptionHandle exceptionHandle = new ExceptionHandle(_context);
        //    //  var abc =  exceptionHandle.ExeptionHandle(ExHan);
        //    //return abc;




        //    context.Result = new ObjectResult(new
        //    {
        //        Message = model.Exception.Message,
        //        api = apiPath,
        //        InnerException = model.Exception.InnerException,
        //        Source = model.Exception.Source
        //        // Details = errorDetails.ToString()
        //    })
        //    {
        //        StatusCode = StatusCodes.Status500InternalServerError
        //    };
        //}





        //public async Task CheckAccess(ActionExecutingContext context, HttpContext httpContext)
        //{
        //    var controllerName = (string)context.RouteData.Values["controller"];
        //    string server = httpContext.Session.GetString("Server_Value");
        //    string comid = httpContext.Session.GetString("com_id");
        //    _loginRepo = new TokenRepo(_context);
        //    string result = await _loginRepo.Access(server, comid);

        //}


    }
}


