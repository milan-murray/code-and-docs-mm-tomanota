using System.Globalization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace user_web_API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class userNotesController : ControllerBase
{
	private string APIKEY = "X00162027";

	private static readonly List<userNotes> noteStorage = new();
	private static readonly List<User> userStorage = new();

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
			if (noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.Prompts).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.Prompts.Select(i => i.Cast<object>().ToList().ToList())).SingleOrDefault());
		}
		return Unauthorized();
	}

	[HttpGet("scoreDetail/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult getScoreDetail([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
			if (note == null)
			{
				return BadRequest();
			}
			return Ok(noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(s => s.ParallelIndividualScores));
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
			if (noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.Prompts).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.Score).FirstOrDefault());
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.LastProgressed).FirstOrDefault();
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.LastProgressed).FirstOrDefault();
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
			if (note == null)
			{
				return NotFound();
			}
			var noteTime = noteStorage.Where(n => n.Title == titleIn).Where(u => u.UserInfo.UserName == userIn).Select(p => p.LastProgressed).FirstOrDefault();
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
		public string Title { get; set; }
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
			if (noteStorage.Where(u => u.UserInfo.UserName == userIn).Select(t => t.Title).Count() == 0)
			{
				return NoContent();
			}
			return Ok(noteStorage.Where(u => u.UserInfo.UserName == userIn).Select(t => new TitleGroup
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
			if (noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn) != null)
			{
				return BadRequest();
			}
			List<int> setScores = new();
			foreach (var p in PromptsIn) { setScores.Add(2); }

			User user = userStorage.FirstOrDefault(u => u.UserName == userIn);
			if (user == null)
			{
				user = new User() { UserName = userIn, CurrentDate = DateTime.Now, MonthProgress = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, WeekProgress = new List<int> { 0, 0, 0, 0, 0, 0, 0 }, Language = Language.English };
				userStorage.Add(user);
			}

			noteStorage.Add(new userNotes { UserInfo = user, Title = titleIn, Prompts = PromptsIn, Score = scoreIn, ParallelIndividualScores = setScores, LastProgressed = null });
			return Ok();
		}
		return Unauthorized();
	}

	[HttpPut("scoreDetail/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}/")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult setScoreDetail([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<int> scoresIn)
	{
		if (keyIn == APIKEY)
		{
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
			if (note == null)
			{
				return BadRequest();
			}
			note.ParallelIndividualScores = scoresIn;
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
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
			userNotes note = noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn && n.Title == titleIn);
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

	[HttpGet("userWeekProgress/key/{keyIn}/userIn/{userIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult getWeekProgress([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			User user = userStorage.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user.WeekProgress);
		}
		return Unauthorized();
	}

	[HttpGet("userMonthProgress/key/{keyIn}/userIn/{userIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult getMonthProgress([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			User user = userStorage.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}
			return Ok(user.MonthProgress);
		}
		return Unauthorized();
	}

	[HttpPut("userProgress/key/{keyIn}/userIn/{userIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public IActionResult addProgress([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			User user = userStorage.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}
			DateTime now = DateTime.Now;
			if (DateTime.Now.Year == user.CurrentDate.Year)
			{
				CultureInfo culture = CultureInfo.CurrentCulture;
				CalendarWeekRule weekRule = culture.DateTimeFormat.CalendarWeekRule;
				DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
				int week1 = culture.Calendar.GetWeekOfYear(now, weekRule, firstDayOfWeek);
				int week2 = culture.Calendar.GetWeekOfYear(user.CurrentDate, weekRule, firstDayOfWeek);
				
				if (week1 == week2)
				{
					int dayIndex = ((int)now.DayOfWeek - 1) % 7;
					user.WeekProgress[dayIndex] += 1;
				}
				else
				{
					user.WeekProgress = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
				}
				user.MonthProgress[now.Month - 1] += 1;
				return Ok();
			}
			else // New year
			{
				user.WeekProgress = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
				user.MonthProgress = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
			}
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
			userNotes noteToDelete = noteStorage.Where(u => u.UserInfo.UserName == userIn).Where(t => t.Title == titleIn).FirstOrDefault();

			if (noteToDelete != null)
			{
				return Ok(noteStorage.Remove(noteToDelete));
			}
			return NotFound();
		}
		return Unauthorized();
	}

	[HttpDelete("deleteUser/key/{keyIn}/user/{userIn}")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult deleteUser([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			User userToDelete = userStorage.FirstOrDefault(u => u.UserName == userIn);

			if (userToDelete != null)
			{
				if (noteStorage.FirstOrDefault(n => n.UserInfo.UserName == userIn) != null )
				{
					noteStorage.RemoveAll(n => n.UserInfo.UserName == userIn);
				}

				return Ok(userStorage.Remove(userToDelete));
			}
			return NotFound();
		}
		return Unauthorized();
	}

	[HttpPost("newUser/keyIn/{keyIn}/userIn/{userIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult newUser([FromRoute] string keyIn, [FromRoute] string userIn, [FromBody] Language languageIn)
	{
		if (keyIn == APIKEY)
		{
			if (userStorage.FirstOrDefault(u => u.UserName == userIn) != null)
			{
				return BadRequest();
			}

			userStorage.Add(new User { UserName = userIn, CurrentDate = DateTime.Now, Language = languageIn, MonthProgress = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, WeekProgress = new List<int> { 0, 0, 0, 0, 0, 0, 0 } });
			return Ok();
		}
		return Unauthorized();
	}

	[HttpGet("language/keyIn/{keyIn}/userIn/{userIn}")]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult getLang([FromRoute] string keyIn, [FromRoute] string userIn)
	{
		if (keyIn == APIKEY)
		{
			User user = userStorage.FirstOrDefault(u => u.UserName == userIn);
			if (user != null)
			{
				return Ok(user.Language);
			}
			return BadRequest();
		}
		return Unauthorized();
	}

    [HttpGet("completedPremades/keyIn/{keyIn}/userIn/{userIn}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult getCompletedPremades([FromRoute] string keyIn, [FromRoute] string userIn)
    {
        if (keyIn == APIKEY)
        {
            User user = userStorage.FirstOrDefault(u => u.UserName == userIn);
            if (user != null)
            {
				if (user.CompletedPremade.Count > 0)
				{
					return Ok(user.CompletedPremade);
				}
				return NoContent();
            }
            return BadRequest();
        }
        return Unauthorized();
    }

    [HttpPut("compeltePremade/keyIn/{keyIn}/userIn/{userIn}/noteTitle/{titleIn}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
	public IActionResult completeNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
    {
        if (keyIn == APIKEY)
        {
            User user = userStorage.FirstOrDefault(u => u.UserName == userIn);
            if (user == null)
            {
                return BadRequest();
            }
			if (user.CompletedPremade.Contains(titleIn))
			{
				return NoContent();
			}
			user.CompletedPremade.Add(titleIn);
			return Ok();
            
        }
        return Unauthorized();
    }
}