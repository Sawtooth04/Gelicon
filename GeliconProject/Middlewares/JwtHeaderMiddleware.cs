namespace GeliconProject.Middlewares
{
    public class JwtHeaderMiddleware
    {
        RequestDelegate next;

        public JwtHeaderMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? token = context.Request.Cookies["Authorization"];
            if (!string.IsNullOrEmpty(token))
                context.Request.Headers.Add("Authorization", "Bearer " + token);
            await next.Invoke(context);
        }
    }
}
