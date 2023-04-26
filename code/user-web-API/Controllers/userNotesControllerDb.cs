using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using user_web_API.Data;

namespace user_web_API.Controllers
{
	[Route("db/")]
	[ApiController]
	public class userNotesControllerDb : ControllerBase
	{
		private string APIKEY = "X00162027";
		private readonly DataContext _dataContext;
		public userNotesControllerDb(DataContext dataContext)
		{
			_dataContext = dataContext;
		}

		[HttpPost("newUser/keyIn/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> newUser([FromRoute] string keyIn, [FromRoute] string userIn, [FromBody] Language languageIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				if (users.FirstOrDefault(u => u.UserName == userIn) != null)
				{
					return BadRequest();
				}

				var newUser = new User { UserName = userIn, Language = languageIn };

				_dataContext.Users.Add(newUser);

				try
				{
					await _dataContext.SaveChangesAsync();
					return Ok();
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);
					return StatusCode(500);
				}
			}
			return Unauthorized();
		}

		[HttpGet("language/keyIn/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> getLang([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				User user = users.FirstOrDefault(u => u.UserName == userIn);
				if (user != null)
				{
					return Ok(user.Language);
				}
				return BadRequest();
			}
			return Unauthorized();
		}
	}
}
