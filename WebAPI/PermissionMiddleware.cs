using Microsoft.AspNetCore.Mvc.Controllers;

namespace WebAPI
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var endpoint = context.GetEndpoint();
            var data = RequiresAuthorization(endpoint);
            if (endpoint == null|| !RequiresAuthorization(endpoint))
            {
                await _next(context); // Bỏ qua middleware và chuyển tiếp yêu cầu
                return;
            }
            // Lấy user từ context
            var user = context.User;

            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("User is not authenticated");
                return;
            }

            // Kiểm tra quyền
            var permissions = context.User.Claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
            /*var permissions = context.User.Claims.Where(c => c.Type == "Permission").Select(c => c.Value);*/

            // Lấy route và method
            if (endpoint != null)
            {
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (actionDescriptor != null)
                {
                    var requiredPermissions = actionDescriptor.MethodInfo
                        .GetCustomAttributes(typeof(AuthorizePermissionAttribute), false)
                        .Cast<AuthorizePermissionAttribute>()
                        .Select(a => a.Permission)
                        .ToList();

                    if (!requiredPermissions.All(rp => permissions.Contains(rp)))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("User does not have required permissions");
                        return;
                    }
                }
            }

            await _next(context);
        }
        private bool RequiresAuthorization(Endpoint endpoint)
        {
            // Kiểm tra xem endpoint có chứa attribute AuthorizePermissionAttribute hay không
            return endpoint.Metadata.Any(meta => meta.GetType() == typeof(AuthorizePermissionAttribute));
        }

    }

}
