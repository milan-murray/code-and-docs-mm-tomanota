using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace user_web_API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class userNotesController : ControllerBase
{
	private String APIKEY = "X00162027";

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

	[HttpPost("newNote/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}/scoreIn/{scoreIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult putNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<object>> PromptsIn, [FromRoute] int scoreIn)
	{
		if (keyIn == APIKEY)
		{
			noteStorage.Add(new userNotes { User = userIn, Title = titleIn, Prompts = PromptsIn, Score = scoreIn });
			return Ok();
		}
		return Unauthorized();
	}

	[HttpDelete("delete/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult deleteNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes noteToDelete = noteStorage.Where(u => u.User == userIn).Where(t => t.Title == titleIn).FirstOrDefault();

			if (noteToDelete != null)
			{
				return Ok(noteStorage.Remove(noteToDelete));
			}
			return NotFound();
		}
		return Unauthorized();
	}
}