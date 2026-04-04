namespace HotelReservationsManager.Models
{
    /// <summary>
    /// Represents the outcome of a service operation that may fail with a
    /// user-facing validation message rather than throw an exception.
    /// </summary>
    public class ServiceResult
    {
        public bool Succeeded { get; private init; }
        public string? Error { get; private init; }

        public static ServiceResult Ok() => new() { Succeeded = true };
        public static ServiceResult Fail(string error) => new() { Succeeded = false, Error = error };
    }
}