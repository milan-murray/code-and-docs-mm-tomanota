using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace user_web_API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class userNotesController : ControllerBase
{
	private String APIKEY = "X00162027-l*8£!dcILkp";

	private static readonly List<userNotes> noteStorage = new();

	[HttpGet("key/{keyIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult getAll([FromRoute] string keyIn)
	{
		if (keyIn == APIKEY)
		{
			if (noteStorage.Count == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.OrderBy(n => n.Title));
		}
		return Unauthorized();
	}

	[HttpGet("key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult getNotes([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			if (noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.Prompts).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.Prompts.Select(i => i.Cast<object>().ToList().ToList())).SingleOrDefault());
		}
		return Unauthorized();
	}

	[HttpGet("key/{keyIn}/user/{userIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult getTitles([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			if (noteStorage.Where(u => u.User == userIn).Select(t => t.Title).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(u => u.User == userIn).Select(t => t.Title));
		}
		return Unauthorized();
	}

	[HttpPut("newNote/key/{keyIn}/{userIn}/{titleIn}/{scoreIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult putNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<object>> PromptsIn, [FromRoute] int scoreIn)
	{
		if (keyIn == APIKEY)
		{
			noteStorage.Add(new userNotes { User = userIn, Title = titleIn, Prompts = PromptsIn, Score = scoreIn });
			return NoContent();
		}
		return Unauthorized();
	}

}