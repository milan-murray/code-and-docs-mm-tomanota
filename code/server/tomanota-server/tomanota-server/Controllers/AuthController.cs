﻿using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tomanota_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		public static User user = new User();

		[HttpPost("register")]
		public async Task<ActionResult<User>> Register(UserRegister request)
		{
			CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			user.Username = request.Username;
			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			return Ok(user);
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using(var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}
	}
}
