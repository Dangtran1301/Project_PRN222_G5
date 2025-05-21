namespace Project_PRN222_G5.Web.Pages.Shared
{
    public static class PageRoutes
    {
        private static string Route(string controller, string action) => $"/{controller}/{action}";

        public static class Static
        {
            public const string Home = "/Index";
            public const string NotFound = "/404";
        }

        public static class Users
        {
            public static readonly string Index = Route("Users", "Index");
            public static readonly string Detail = Route("Users", "Details");
            public static readonly string Create = Route("Users", "Create");
            public static readonly string Edit = Route("Users", "Edit");
            public static readonly string Delete = Route("Users", "Delete");
        }

        public static class Auth
        {
            public static readonly string Login = Route("Auth", "Login");
            public static readonly string Register = Route("Auth", "Register");
            public static readonly string Logout = Route("Auth", "Logout");
        }

        public static class Cinema
        {
            public static readonly string Index = Route("Cinema", "Index");
            public static readonly string Detail = Route("Cinema", "Details");
            public static readonly string Create = Route("Cinema", "Create");
            public static readonly string Edit = Route("Cinema", "Edit");
            public static readonly string Delete = Route("Cinema", "Delete");
        }
    }
}