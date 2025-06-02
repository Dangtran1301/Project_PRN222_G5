namespace Project_PRN222_G5.Web.Utilities;

public static class PageRoutes
{
    private static string Route(string controller, string action) => $"/{controller}/{action}";

    public static class Public
    {
        public const string Home = "/Index";
        public const string Error = "/Shared/Error";
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
        public static readonly string Refresh = Route("Auth", "Refresh");
    }

    public static class Cinema
    {
        public static readonly string Index = Route("Cinema", "Index");
        public static readonly string Detail = Route("Cinema", "Details");
        public static readonly string Create = Route("Cinema", "Create");
        public static readonly string Edit = Route("Cinema", "Edit");
        public static readonly string Delete = Route("Cinema", "Delete");
    }

    public static class Movie
    {
        public static readonly string Index = Route("Movie", "Index");
        public static readonly string Detail = Route("Movie", "Details");
        public static readonly string Create = Route("Movie", "Create");
        public static readonly string Edit = Route("Movie", "Edit");
        public static readonly string Delete = Route("Movie", "Delete");
    }

    public static class Showtime
    {
        public static readonly string Index = Route("Showtime", "Index");
        public static readonly string Detail = Route("Showtime", "Details");
        public static readonly string Create = Route("Showtime", "Create");
        public static readonly string Edit = Route("Showtime", "Edit");
        public static readonly string Delete = Route("Showtime", "Delete");
    }

    public static class Room
    {
        public static readonly string Index = Route("Room", "Index");
        public static readonly string Detail = Route("Room", "Details");
        public static readonly string Create = Route("Room", "Create");
        public static readonly string Edit = Route("Room", "Edit");
        public static readonly string Delete = Route("Room", "Delete");
    }

    public static class Seat
    {
        public static readonly string Index = Route("Seat", "Index");
        public static readonly string Detail = Route("Seat", "Details");
        public static readonly string Create = Route("Seat", "Create");
        public static readonly string Edit = Route("Seat", "Edit");
        public static readonly string Delete = Route("Seat", "Delete");
    }

    public static class Booking
    {
        public static readonly string Index = Route("Booking", "Index");
        public static readonly string Detail = Route("Booking", "Details");
        public static readonly string Create = Route("Booking", "Create");
        public static readonly string Edit = Route("Booking", "Edit");
        public static readonly string Delete = Route("Booking", "Delete");
    }
}