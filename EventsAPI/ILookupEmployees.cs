namespace EventsAPI
{
    internal interface ILookupEmployees
    {
        Task<bool> CheckEmployeeIsActive(int id);
    }
}