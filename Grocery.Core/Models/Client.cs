
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public enum Privilege
        {
            None,
            Admin
        }
        public Privilege Role { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Client(int id, string name, string emailAddress, string password, Privilege role = Privilege.None) : base(id, name)
        {
            EmailAddress=emailAddress;
            Password=password;
            Role=role;
        }
    }
}
