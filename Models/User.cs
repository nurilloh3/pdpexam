using System;
namespace pdpExam4.Models
{
	public class User
	{
		public User(int id, string name, string login, string password, int role)
		{
			this.Id = id;
			this.Name = name;
			this.Login = login;
			this.Password = password;
			this.Role = role;
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public int Role { get; set; }
	}
}

