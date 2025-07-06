using finanzas_project.IAM.Domain.Model.ValueObjects;
using System.Text.Json.Serialization;

namespace finanzas_project.IAM.Domain.Model.Aggregates
{
    public class User(string username, string passwordHash)
    {
        public User() : this(string.Empty, string.Empty)
        {
        }

        public int Id { get; }

        public string Username { get; private set; } = username;

        [JsonIgnore]
        public string PasswordHash { get; private set; } = passwordHash;

        // ✅ Nueva propiedad
        public UserRole Role { get; private set; } = UserRole.TRANSMITTER;

        // Métodos de dominio
        public User UpdatePasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            return this;
        }

        public User UpdateUsername(string username)
        {
            Username = username;
            return this;
        }

        public User UpdateRole(UserRole newRole)
        {
            Role = newRole;
            return this;
        }
    }
}
