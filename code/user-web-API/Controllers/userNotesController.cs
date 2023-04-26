using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace user_web_API.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class userNotesController : ControllerBase
{
	private string APIKEY = "X00162027";

	private static readonly List<Prompt> prompts = new();
	private static List<PromptBody> promptBodies = new();
	private static readonly List<User> users = new();
	private static readonly List<WeekProgress> weekProgresses = new();
	private static readonly List<YearProgress> yearProgresses = new();
	private static readonly List<CompletedPremadeTitle> completedPremadeTitles = new();

	[HttpGet("key/{keyIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult getAll([FromRoute] string keyIn)
	{
		if (keyIn == APIKEY)
		{
			if (prompts.Count == 0)
			{
				return NoContent();
			}

			return Ok(prompts.OrderBy(n => n.Title));
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
			if (prompts.Where(n => n.Title == titleIn).Where(u => u.UserName == userIn).Count() == 0)
			{
				return NoContent();
			}

			Guid idToFind = prompts.Where(n => n.Title == titleIn).Where(u => u.UserName == userIn).Select(a => a.Id).First();

			List<PromptBody> p_content = promptBodies.Where(b => b.PromptId == idToFind).ToList();
			int num_prompts = p_content.Count();

			List<List<string>> content = new();

			for (int i = 0; i < num_prompts; i++)
			{
				content.Add(new List<string> { p_content[i].Heading, p_content[i].Body });
			}

			return Ok(content);
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}

			Guid targetID = existingPrompt.Id;
			List<int> scores = new List<int>();

			List<PromptBody> p_content = promptBodies.Where(b => b.PromptId == targetID).ToList();

			foreach (var p in p_content)
			{
				scores.Add(p.Score);
			}

			return Ok(scores);
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}

			return Ok(existingPrompt.Score);
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.LastProgressed == null)
			{
				return NoContent();
			}
			return Ok(existingPrompt.LastProgressed);
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.LastProgressed == null)
			{
				return NoContent();
			}
			return Ok(DateTime.Now - existingPrompt.LastProgressed);
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.LastProgressed == null)
			{
				return Ok(false);
			}
			return Ok(true);
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
			if (prompts.Where(u => u.UserName == userIn).Select(t => t.Title).Count() == 0)
			{
				return NoContent();
			}
			return Ok(prompts.Where(u => u.UserName == userIn).Select(t => new TitleGroup
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
	public IActionResult newNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<string>> PromptsIn, [FromRoute] int scoreIn)
	{
		if (keyIn == APIKEY)
		{
			if (prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn) != null)
			{
				return BadRequest();
			}

			User user = users.FirstOrDefault(u => u.UserName == userIn);
			if (user == null)
			{
				user = new User() { Language = Language.English, MostRecentDate = DateTime.Now, UserName = userIn };
				users.Add(user);
			}

			Prompt newPrompt = new Prompt { UserName = userIn, LastProgressed = null, Score = scoreIn, Title = titleIn, UserId = user.Id };

			prompts.Add(newPrompt);

			foreach (var pair in PromptsIn)
			{
				promptBodies.Add(new PromptBody { Heading = pair[0], Body = pair[1], PromptId = newPrompt.Id, Score = 2 });
			}

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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			IEnumerable<PromptBody> targetPromptsB = promptBodies.Where(i => i.PromptId == existingPrompt.Id);
			/*foreach (PromptBody pb in targetPromptsB)
			{
				promptBodies.Remove(pb);
			}*/

			int index = 0;
			foreach (var p in targetPromptsB)
			{
				promptBodies.Where(a => a.Id == p.Id).FirstOrDefault().Score = scoresIn[index];
				//promptBodies.Add(new PromptBody { Heading = p.Heading, Body = p.Body, PromptId = p.PromptId, Score = scoresIn[index] });
				index++;
			}
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.Score < 10)
			{
				existingPrompt.Score = existingPrompt.Score + 1;
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.Score > 0)
			{
				existingPrompt.Score = existingPrompt.Score - 1;
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
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}
			if (existingPrompt.LastProgressed == null)
			{
				existingPrompt.LastProgressed = DateTime.Now;
				return Ok();
			}
			DateTime now = DateTime.Now;
			if (!(existingPrompt.LastProgressed > now.AddHours(-4) && existingPrompt.LastProgressed <= now))
			{
				existingPrompt.LastProgressed = DateTime.Now;
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
			User user = users.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}

			WeekProgress targetWeek = weekProgresses.FirstOrDefault(n => n.UserId == user.Id);
			if (targetWeek == null)
			{
				targetWeek = new WeekProgress { UserId = user.Id };
				weekProgresses.Add(targetWeek);
			}

			return Ok(targetWeek.GetDays());
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
			User user = users.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}

			YearProgress targetYear = yearProgresses.FirstOrDefault(n => n.UserId == user.Id);
			if (targetYear == null)
			{
				targetYear = new YearProgress { UserId = user.Id };
				yearProgresses.Add(targetYear);
			}

			return Ok(targetYear.GetMonths());
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
			User user = users.FirstOrDefault(n => n.UserName == userIn);
			if (user == null)
			{
				return NotFound();
			}
			WeekProgress targetWeek = weekProgresses.FirstOrDefault(u => u.UserId == user.Id);
			if (targetWeek == null)
			{
				targetWeek = new WeekProgress { UserId = user.Id };
				weekProgresses.Add(targetWeek);
			}
			DateTime now = DateTime.Now;
			if (DateTime.Now.Year == user.MostRecentDate.Year)
			{
				CultureInfo culture = CultureInfo.CurrentCulture;
				CalendarWeekRule weekRule = culture.DateTimeFormat.CalendarWeekRule;
				DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
				int week1 = culture.Calendar.GetWeekOfYear(now, weekRule, firstDayOfWeek);
				int week2 = culture.Calendar.GetWeekOfYear(user.MostRecentDate, weekRule, firstDayOfWeek);

				if (week1 == week2)
				{
					int dayIndex = ((int)now.DayOfWeek - 1) % 7;
					weekProgresses.FirstOrDefault(u => u.UserId == user.Id).IncDay(dayIndex);
				}
				else
				{
					weekProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetDays();
				}
				YearProgress targetYear = yearProgresses.FirstOrDefault(u => u.UserId == user.Id);
				if (targetYear == null)
				{
					targetYear = new YearProgress { UserId = user.Id };
					yearProgresses.Add(targetYear);
				}
				yearProgresses.FirstOrDefault(u => u.UserId == user.Id).IncMonth(now.Month - 1);
				return Ok();
			}
			else // New year
			{
				YearProgress targetYear = yearProgresses.FirstOrDefault(u => u.UserId == user.Id);
				if (targetYear == null)
				{
					targetYear = new YearProgress { UserId = user.Id };
					yearProgresses.Add(targetYear);
				}
				weekProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetDays();
				yearProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetMonths();
			}
			return Ok();
		}
		return Unauthorized();
	}

	[HttpDelete("delete/key/{keyIn}/user/{userIn}/title/{titleIn}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public IActionResult deleteNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
	{
		if (keyIn == APIKEY)
		{
			Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

			if (existingPrompt == null)
			{
				return BadRequest();
			}

			promptBodies.RemoveAll(a => a.PromptId == existingPrompt.Id);
			prompts.Remove(existingPrompt);

			return Ok();
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
			User userToDelete = users.FirstOrDefault(u => u.UserName == userIn);

			if (userToDelete != null)
			{
				if (prompts.FirstOrDefault(n => n.UserName == userIn) != null)
				{

					List<Prompt> promptsToDelete = prompts.Where(a => a.UserName == userIn).ToList();

					foreach (Prompt prompt in promptsToDelete)
					{
						promptBodies.RemoveAll(a => a.PromptId == prompt.Id);
					}

					prompts.RemoveAll(n => n.UserName == userIn);
				}

				return Ok(users.Remove(userToDelete));
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
			if (users.FirstOrDefault(u => u.UserName == userIn) != null)
			{
				return BadRequest();
			}

			users.Add(new User { UserName = userIn, Language = languageIn, MostRecentDate = DateTime.Now });
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
			User user = users.FirstOrDefault(u => u.UserName == userIn);
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
			User user = users.FirstOrDefault(u => u.UserName == userIn);
			if (user != null)
			{

				List<string> rTitles = completedPremadeTitles.Where(a => a.UserId == user.Id).Select(t => t.titlePremade).ToList();

				if (rTitles.Count > 0)
				{
					return Ok(rTitles);
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
			User user = users.FirstOrDefault(u => u.UserName == userIn);
			if (user == null)
			{
				return BadRequest();
			}
			if (completedPremadeTitles.Where(n => n.titlePremade == titleIn).Where(u => u.UserId == user.Id).FirstOrDefault() != null)
			{
				return NoContent();
			}
			completedPremadeTitles.Add(new CompletedPremadeTitle { titlePremade = titleIn, UserId = user.Id });
			return Ok();

		}
		return Unauthorized();
	}
}