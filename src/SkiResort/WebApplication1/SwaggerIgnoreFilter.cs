using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.Swagger;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication1
{
    public class SwaggerIgnoreFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {
        private IServiceProvider _provider;

        public SwaggerIgnoreFilter(IServiceProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            this._provider = provider;
        }
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            IHttpContextAccessor httpContextAccessor = this._provider.GetRequiredService<IHttpContextAccessor>();
            HttpRequest httpRequest = httpContextAccessor.HttpContext.Request;
            var _bearer_token = httpRequest.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            string role;
            try
            {
                JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(_bearer_token);
                string roleClaimName = ClaimsIdentity.DefaultRoleClaimType;
                role = (string)token.Payload[roleClaimName];
            }
            catch (Exception ex)
            {
                role = "unauthorized";
            }

            var descriptions = context.ApiDescriptions.ToList();

            foreach (var description in descriptions)
            {
                IList<object> endpointMetadata = description.ActionDescriptor.EndpointMetadata;
                foreach (var metadata in endpointMetadata)
                {
                    if (metadata.GetType() == typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute))
                    {
                        Microsoft.AspNetCore.Authorization.AuthorizeAttribute authorizeAttribute = 
                            (Microsoft.AspNetCore.Authorization.AuthorizeAttribute)metadata;
                        string allowedRoles = authorizeAttribute.Roles;
                        if (!allowedRoles.Contains(role))
                        {
                            string route = "/" + description.RelativePath.TrimEnd('/');
                            OpenApiPathItem path;
                            swaggerDoc.Paths.TryGetValue(route, out path);

                            switch (description.HttpMethod)
                            {
                                case "DELETE": path.Operations.Remove(Microsoft.OpenApi.Models.OperationType.Delete); break;
                                case "GET": path.Operations.Remove(Microsoft.OpenApi.Models.OperationType.Get); break;

                                case "PATCH": path.Operations.Remove(Microsoft.OpenApi.Models.OperationType.Patch); break;
                                case "POST": path.Operations.Remove(Microsoft.OpenApi.Models.OperationType.Post); break;
                                case "PUT": path.Operations.Remove(Microsoft.OpenApi.Models.OperationType.Put); break;
                                default: throw new ArgumentOutOfRangeException("Method name not mapped to operation");
                            }
                        }
                    }
                }
            }

            //if (role != "admin")
            //{
            //    var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(i => i.GetTypes()).ToList();
            //    foreach (var definition in swaggerDoc.Components.Schemas)
            //    {
            //        swaggerDoc.Components.Schemas.Remove(definition);
            //        var type = allTypes.FirstOrDefault(x => x.Name == definition.Key);
            //        if (type != null)
            //        {
            //            var properties = type.GetProperties();
            //            foreach (var prop in properties.ToList())
            //            {
            //                var ignoreAttribute = prop.GetCustomAttribute(typeof(OpenApiIgnoreAttribute), false);

            //                if (ignoreAttribute != null)
            //                {
            //                    definition.Value.Properties.Remove(prop.Name);
            //                }
            //            }
            //        }
            //    }
            //}
            
        }
    }
}
        //public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        //{

            //var _bearer_token = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            //var token = new JwtSecurityTokenHandler().ReadJwtToken(_bearer_token);

            //var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(i => i.GetTypes()).ToList();

            //IHttpContextAccessor http = this._provider.GetRequiredService<IHttpContextAccessor>();
            //var authorizedIds = new[] { "00000000-1111-2222-1111-000000000000" };   // All the authorized user id's.
            //                                                                        // When using this in a real application, you should store these safely using appsettings or some other method.
            //var userId = http.HttpContext.User.Claims.Where(x => x.Type == "jti").Select(x => x.Value).FirstOrDefault();
            //var show = http.HttpContext.User.Identity.IsAuthenticated && authorizedIds.Contains(userId);



            //var Securitytoken = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            //var tokenstring = new JwtSecurityTokenHandler().WriteToken(Securitytoken);
            //var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenstring);
            //var claim = token.Claims.First(c => c.Type == "email").Value;


            //var descriptions = context.ApiDescriptions.ToList();

            //foreach (var description in descriptions)
            //{
            //    // Expose login so users can login through Swagger. 
            //    if (description.HttpMethod == "POST")
            //        continue;

            //    var route = "/" + description.RelativePath.TrimEnd('/');
            //    OpenApiPathItem path;
            //    swaggerDoc.Paths.TryGetValue(route, out path);

            //    switch (route)
            //    {
            //        case string s when s.Contains("/lifts"):
            //            {
            //                swaggerDoc.Paths.Remove(route);
            //                break;
            //            }

            //        default:
            //            break;
            //    }




                //Parametros parametros = new Parametros();
                //if (!show)
                //{
                //    var descriptions = context.ApiDescriptions.ToList();

                //    foreach (var description in descriptions)
                //    {
                //        // Expose login so users can login through Swagger. 
                //        if (description.HttpMethod == "POST" && description.RelativePath == "denarioapi/v1/auth/login")
                //            continue;

                //        var route = "/" + description.RelativePath.TrimEnd('/');
                //        OpenApiPathItem path;
                //        swaggerDoc.Paths.TryGetValue(route, out path);

                //        switch (route)
                //        {
                //            case string s when s.Contains("/Contabilidad"):
                //                if (parametros.contabilidadApi != "1")
                //                {
                //                    swaggerDoc.Paths.Remove(route);
                //                }
                //                break;

                //            default:
                //                break;
                //        }







                // remove method or entire path (if there are no more methods in this path)
                //switch (description.HttpMethod)
                //{
                //case "DELETE": path. = null; break;
                //case "GET": path.Get = null; break;
                //case "HEAD": path.Head = null; break;
                //case "OPTIONS": path.Options = null; break;
                //case "PATCH": path.Patch = null; break;
                //case "POST": path.Post = null; break;
                //case "PUT": path.Put = null; break;
                //default: throw new ArgumentOutOfRangeException("Method name not mapped to operation");
                //}

                //if (path.Delete == null && path.Get == null &&
                //    path.Head == null && path.Options == null &&
                //    path.Patch == null && path.Post == null && path.Put == null)
                //swaggerDoc.Paths.Remove(route);
                //    }

                //}




                //foreach (var definition in swaggerDoc.Components.Schemas)
                //{
                //    var type = allTypes.FirstOrDefault(x => x.Name == definition.Key);
                //    if (type != null)
                //    {
                //        var properties = type.GetProperties();
                //        foreach (var prop in properties.ToList())
                //        {
                //            //var ignoreAttribute = prop.GetCustomAttribute(typeof(OpenApiIgnoreAttribute), false);

                //            //if (ignoreAttribute != null)
                //            //{
                //            //    definition.Value.Properties.Remove(prop.Name);
                //            //}
                //        }
                //    }
    //            }
    //        }
    //    }
    //}

//namespace WebApplication1
//{
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
//    public class HideInDocsAttribute : Attribute
//    {
//    }

//    public class HideInDocsFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
//    {
//        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
//        {
//            foreach (var apiDescription in apiExplorer.ApiDescriptions)
//            {
//                if (!apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<HideInDocsAttribute>().Any() && 
//                    !apiDescription.ActionDescriptor.GetCustomAttributes<HideInDocsAttribute>().Any()) 
//                    continue;

//                var route = "/" + apiDescription.Route.RouteTemplate.TrimEnd('/');
//                swaggerDoc.paths.Remove(route);
//            }
//        }

//        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
