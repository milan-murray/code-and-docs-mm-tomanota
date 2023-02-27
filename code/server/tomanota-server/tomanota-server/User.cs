namespace tomanota_server
{
	public class User
	{
		public String Username { get; set; } = string.Empty;

		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
	}
}
