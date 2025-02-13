using System.ComponentModel.DataAnnotations;

namespace BusinessObject.ModelDTOs
{
	public class UserDTO
	{
		public string UserId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string FullUserName
		{
			get
			{
				return $"{FirstName} {MiddleName} {LastName}";
			}
		}
		public string PasswordHash { get; set; }

		public string Email { get; set; }
		public string PersonalEmail { get; set; }
		public string PhoneNumber { get; set; }
		public string UserAvartar { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public int RoleId { get; set; }
		public string RoleName { get; set; }
		public string MajorId { get; set; }
		public string MajorName { get; set; }
		public int Status { get; set; }
		public string Address { get; set; }

	}
	public class AddUserDTO
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string MajorId { get; set; }
		public string FullUserName
		{
			get
			{
				return $"{FirstName} {MiddleName} {LastName}";
			}
		}
		public string PasswordHash { get; set; }
		public string UserAvartar { get; set; }
		public DateOnly DateOfBirth { get; set; }

		public string PersonalEmail { get; set; }
		public string PhoneNumber { get; set; }
		public int RoleId { get; set; }
		public int Status { get; set; }
		public string Address { get; set; }
	}
	public class UpdateUserDTO
	{
		public string PersonalEmail { get; set; }
		public string PhoneNumber { get; set; }
	}
	public class UpdateInforDTO
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string UserAvartar { get; set; }
		public DateOnly DateOfBirth { get; set; }
		public string PersonalEmail { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
	}
	//public class UpdateStudentStatusDTO
	//{
	//    public int Status { get; set; }
	//}
	public class ResetPasswordDTO
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
