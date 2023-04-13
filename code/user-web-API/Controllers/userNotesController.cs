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

	[HttpGet("score/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<int> getScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			if (noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.Prompts).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.Score).FirstOrDefault());
		}
		return Unauthorized();
	}

	[HttpGet("time/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DateTime> getTime([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.LastProgressed).FirstOrDefault();
			if (noteTime == null)
			{
				return NoContent();
			}
			return Ok(noteTime);
		}
		return Unauthorized();
	}

	[HttpGet("started/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<DateTime> getStarted([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.LastProgressed).FirstOrDefault();
			if (noteTime == null)
			{
				return Ok(false);
			}
			return Ok(true);
		}
		return Unauthorized();
	}

	[HttpGet("timeDelta/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<TimeSpan> getTimeDelta([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.User == userIn).Select(p => p.LastProgressed).FirstOrDefault();
			if (noteTime == null)
			{
				return NoContent();
			}
			return Ok(DateTime.Now - noteTime);
		}
		return Unauthorized();
	}

	// Class for title return type below
	public class TitleGroup
	{
		public String Title { get; set; }
		public int Score { get; set; }
		public TimeSpan? LastProgressed { get; set; }
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
			return Ok(noteStorage.Where(u => u.User == userIn).Select(t => new TitleGroup
			{
				Title = t.Title,
				Score = t.Score,
				LastProgressed = DateTime.Now - t.LastProgressed
			}));
		}
		return Unauthorized();
	}

	[HttpPost("newNote/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}/scoreIn/{scoreIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult newNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<object>> PromptsIn, [FromRoute] int scoreIn)
	{
		if (keyIn == APIKEY)
		{
			if (noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn) != null)
			{
				return BadRequest();
			}
			noteStorage.Add(new userNotes { User = userIn, Title = titleIn, Prompts = PromptsIn, Score = scoreIn, LastProgressed = null });
			return Ok();
		}
		return Unauthorized();
	}

	[HttpPut("incScore/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult incScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			if (note.Score < 10)
			{
				note.Score = note.Score + 1;
				return Ok();
			}
			return NoContent();
		}
		return Unauthorized();
	}

	[HttpPut("decScore/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult decScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			if (note.Score > 0)
			{
				note.Score = note.Score - 1;
				return Ok();
			}
			return NoContent();
		}
		return Unauthorized();
	}

	[HttpPut("setTime/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult setTime([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.User == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			if (note.LastProgressed == null)
			{
				note.LastProgressed = DateTime.Now;
				return Ok();
			}
			DateTime now = DateTime.Now;
			if (!(note.LastProgressed > now.AddHours(-4) && note.LastProgressed <= now))
			{
				note.LastProgressed = DateTime.Now;
				return Ok();
			}
			return BadRequest();
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