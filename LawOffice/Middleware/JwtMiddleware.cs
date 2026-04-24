using LawOffice.Services;

namespace LawOffice.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) => _next = next;

        //public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        //{
        //    // اول کوکی چک کن، بعد هدر
        //    var token = context.Request.Cookies["access_token"]
        //        ?? context.Request.Headers["Authorization"]
        //            .FirstOrDefault()?.Replace("Bearer ", "");

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        var principal = jwtService.ValidateToken(token);
        //        if (principal != null)
        //            context.User = principal;
        //    }

        //    await _next(context);
        //}

        public async Task InvokeAsync(HttpContext context, IJwtService jwtService)
        {
            var path = context.Request.Path.Value?.ToLower();

            // ۱. مشخص کردن مسیرهایی که نیاز به احراز هویت ندارند (عمومی هستند)
            // نام مسیر لاگین خود را در صورت نیاز تغییر دهید
            bool isAnonymousPath = path == "/account/login" ||
                                   path == "/auth/login" ||
                                   path.StartsWith("/api/auth") || // برای APIهای لاگین
                                   path.Contains("."); // برای عبور فایل‌های استاتیک مثل .css و .js

            // ۲. استخراج توکن
            var token = context.Request.Cookies["access_token"]
                ?? context.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Replace("Bearer ", "");

            bool isAuthenticated = false;

            // ۳. اعتبارسنجی توکن
            if (!string.IsNullOrEmpty(token))
            {
                var principal = jwtService.ValidateToken(token);
                if (principal != null)
                {
                    context.User = principal;
                    isAuthenticated = true;
                }
            }

            // ۴. هدایت به لاگین در صورت عدم احراز هویت و عمومی نبودن مسیر
            if (!isAuthenticated && !isAnonymousPath)
            {
                // اگر درخواست از نوع API است، خطای 401 برگردانید
                if (path != null && path.StartsWith("/api/"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return; // توقف چرخه
                }

                // اگر درخواست مربوط به صفحات وب است، ریدایرکت کنید
                context.Response.Redirect("/Auth/Login");
                return; // توقف چرخه تا ادامه کدهای برنامه اجرا نشود
            }

            // ۵. ادامه چرخه برای کاربران تایید شده یا مسیرهای عمومی
            await _next(context);
        }

    }
}