namespace finanzas_project.IAM.Interfaces.ACL
{
    public interface IIamContextFacade
    {
        Task<int> CreateUser(string username, string password,string role);
        Task<int> FetchUserIdByUsername(string username);
        Task<string> FetchUsernameByUserId(int userId);
    }
}
