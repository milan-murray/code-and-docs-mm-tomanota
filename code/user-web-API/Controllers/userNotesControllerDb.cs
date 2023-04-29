using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Globalization;
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

		[HttpGet("key/{keyIn}/user/{userIn}/title/{titleIn}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> getNotes([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				var promptBodies = await _dataContext.PromptBodies.ToListAsync();
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
		public async Task<IActionResult> getScoreDetail([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				var promptBodies = await _dataContext.PromptBodies.ToListAsync();
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
		public async Task<ActionResult<int>> getScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
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
		public async Task<ActionResult<DateTime>> getTime([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
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
		public async Task<ActionResult<TimeSpan>> getTimeDelta([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
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
		public async Task<ActionResult<DateTime>> getStarted([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
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
		public async Task<IActionResult> getTitles([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
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

		[HttpGet("userWeekProgress/key/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> getWeekProgress([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var weekProgresses = await _dataContext.WeekProgresses.ToListAsync();
				User user = users.FirstOrDefault(n => n.UserName == userIn);
				if (user == null)
				{
					return NotFound();
				}

				WeekProgress targetWeek = weekProgresses.FirstOrDefault(n => n.UserId == user.Id);
				if (targetWeek == null)
				{
					targetWeek = new WeekProgress { UserId = user.Id };
					_dataContext.WeekProgresses.Add(targetWeek);
					await _dataContext.SaveChangesAsync();
				}

				return Ok(targetWeek.GetDays());
			}
			return Unauthorized();
		}

		[HttpGet("userMonthProgress/key/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> getMonthProgress([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var yearProgresses = await _dataContext.YearProgresses.ToListAsync();
				User user = users.FirstOrDefault(n => n.UserName == userIn);
				if (user == null)
				{
					return NotFound();
				}

				YearProgress targetYear = yearProgresses.FirstOrDefault(n => n.UserId == user.Id);
				if (targetYear == null)
				{
					targetYear = new YearProgress { UserId = user.Id };
					_dataContext.YearProgresses.Add(targetYear);
					await _dataContext.SaveChangesAsync();
				}

				return Ok(targetYear.GetMonths());
			}
			return Unauthorized();
		}

		[HttpGet("completedPremades/keyIn/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> getCompletedPremades([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var completedPremadeTitles = await _dataContext.CompletedPremadeTitles.ToListAsync();
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

		[HttpPost("newNote/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}/scoreIn/{scoreIn}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> newNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<string>> PromptsIn, [FromRoute] int scoreIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				var users = await _dataContext.Users.ToListAsync();
				if (prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn) != null)
				{
					return BadRequest();
				}

				User user = users.FirstOrDefault(u => u.UserName == userIn);
				if (user == null)
				{
					user = new User() { Language = Language.English, MostRecentDate = DateTime.Now, UserName = userIn };
					_dataContext.Users.Add(user);
					//users.Add(user);
				}

				Prompt newPrompt = new Prompt { UserName = userIn, LastProgressed = null, Score = scoreIn, Title = titleIn, UserId = user.Id };

				_dataContext.Prompts.Add(newPrompt);
				//prompts.Add(newPrompt);

				foreach (var pair in PromptsIn)
				{
					//promptBodies.Add(new PromptBody { Heading = pair[0], Body = pair[1], PromptId = newPrompt.Id, Score = 2 });
					var promptBodyToAdd = new PromptBody { Heading = pair[0], Body = pair[1], PromptId = newPrompt.Id, Score = 2 };
					_dataContext.PromptBodies.Add(promptBodyToAdd);
				}

				await _dataContext.SaveChangesAsync();
				return Ok();
			}
			return Unauthorized();
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

		[HttpPut("userProgress/key/{keyIn}/userIn/{userIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> addProgress([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var weekProgresses = await _dataContext.WeekProgresses.ToListAsync();
				var yearProgresses = await _dataContext.YearProgresses.ToListAsync();
				User user = users.FirstOrDefault(n => n.UserName == userIn);
				if (user == null)
				{
					return NotFound();
				}
				WeekProgress targetWeek = weekProgresses.FirstOrDefault(u => u.UserId == user.Id);
				if (targetWeek == null)
				{
					targetWeek = new WeekProgress { UserId = user.Id };
					_dataContext.WeekProgresses.Add(targetWeek);
					await _dataContext.SaveChangesAsync();
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
						_dataContext.WeekProgresses.FirstOrDefault(u => u.UserId == user.Id).IncDay(dayIndex);
						await _dataContext.SaveChangesAsync();
					}
					else
					{
						_dataContext.WeekProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetDays();
						await _dataContext.SaveChangesAsync();
					}
					YearProgress targetYear = yearProgresses.FirstOrDefault(u => u.UserId == user.Id);
					if (targetYear == null)
					{
						targetYear = new YearProgress { UserId = user.Id };
						_dataContext.YearProgresses.Add(targetYear);
						await _dataContext.SaveChangesAsync();
					}
					_dataContext.YearProgresses.FirstOrDefault(u => u.UserId == user.Id).IncMonth(now.Month - 1);
					await _dataContext.SaveChangesAsync();
					return Ok();
				}
				else // New year
				{
					YearProgress targetYear = yearProgresses.FirstOrDefault(u => u.UserId == user.Id);
					if (targetYear == null)
					{
						targetYear = new YearProgress { UserId = user.Id };
						_dataContext.YearProgresses.Add(targetYear);
					}
					_dataContext.WeekProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetDays();
					_dataContext.YearProgresses.FirstOrDefault(u => u.UserId == user.Id).ResetMonths();
				}
				await _dataContext.SaveChangesAsync();
				return Ok();
			}
			return Unauthorized();
		}

		[HttpPut("scoreDetail/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}/")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> setScoreDetail([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<int> scoresIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				var promptBodies = await _dataContext.PromptBodies.ToListAsync();
				Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

				if (existingPrompt == null)
				{
					return BadRequest();
				}
				IEnumerable<PromptBody> targetPromptsB = promptBodies.Where(i => i.PromptId == existingPrompt.Id);

				int index = 0;
				foreach (var p in targetPromptsB)
				{
					_dataContext.PromptBodies.Where(a => a.Id == p.Id).FirstOrDefault().Score = scoresIn[index];
					index++;
				}

				await _dataContext.SaveChangesAsync();
				return Ok();
			}
			return Unauthorized();
		}

		[HttpPut("incScore/key/{keyIn}/userIn/{userIn}/titleIn/{titleIn}")]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> incScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

				if (existingPrompt == null)
				{
					return BadRequest();
				}
				if (existingPrompt.Score < 10)
				{
					_dataContext.Prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn).Score = existingPrompt.Score + 1;
					await _dataContext.SaveChangesAsync();
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
		public async Task<IActionResult> decScore([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

				if (existingPrompt == null)
				{
					return BadRequest();
				}
				if (existingPrompt.Score > 0)
				{
					_dataContext.Prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn).Score = existingPrompt.Score - 1;
					await _dataContext.SaveChangesAsync();
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
		public async Task<IActionResult> setTime([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

				if (existingPrompt == null)
				{
					return BadRequest();
				}
				if (existingPrompt.LastProgressed == null)
				{
					_dataContext.Prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn).LastProgressed = DateTime.Now;
					await _dataContext.SaveChangesAsync();
					return Ok();
				}
				DateTime now = DateTime.Now;
				if (!(existingPrompt.LastProgressed > now.AddHours(-4) && existingPrompt.LastProgressed <= now))
				{
					_dataContext.Prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn).LastProgressed = DateTime.Now;
					await _dataContext.SaveChangesAsync();
					return Ok();
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
		public async Task<IActionResult> completeNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var completedPremadeTitles = await _dataContext.CompletedPremadeTitles.ToListAsync();
				User user = users.FirstOrDefault(u => u.UserName == userIn);
				if (user == null)
				{
					return BadRequest();
				}
				if (completedPremadeTitles.Where(n => n.titlePremade == titleIn).Where(u => u.UserId == user.Id).FirstOrDefault() != null)
				{
					return NoContent();
				}
				_dataContext.CompletedPremadeTitles.Add(new CompletedPremadeTitle { titlePremade = titleIn, UserId = user.Id });
				await _dataContext.SaveChangesAsync();
				return Ok();

			}
			return Unauthorized();
		}

		[HttpDelete("delete/key/{keyIn}/user/{userIn}/title/{titleIn}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> deleteNote([FromRoute] string keyIn, [FromRoute] string userIn, [FromRoute] string titleIn)
		{
			if (keyIn == APIKEY)
			{
				var prompts = await _dataContext.Prompts.ToListAsync();
				var promptBodies = await _dataContext.PromptBodies.ToListAsync();
				Prompt existingPrompt = prompts.FirstOrDefault(n => n.UserName == userIn && n.Title == titleIn);

				if (existingPrompt == null)
				{
					return BadRequest();
				}

				_dataContext.Database.ExecuteSqlRaw($"DELETE FROM promptbodies WHERE promptid = '{existingPrompt.Id}'");

				_dataContext.Database.ExecuteSqlRaw($"DELETE FROM prompts WHERE id = '{existingPrompt.Id}'");
				await _dataContext.SaveChangesAsync();

				return Ok();
			}
			return Unauthorized();
		}

		[HttpDelete("deleteUser/key/{keyIn}/user/{userIn}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<IActionResult> deleteUser([FromRoute] string keyIn, [FromRoute] string userIn)
		{
			if (keyIn == APIKEY)
			{
				var users = await _dataContext.Users.ToListAsync();
				var prompts = await _dataContext.Prompts.ToListAsync();
				var promptBodies = await _dataContext.PromptBodies.ToListAsync();
				User userToDelete = users.FirstOrDefault(u => u.UserName == userIn);

				if (userToDelete != null)
				{
					if (prompts.FirstOrDefault(n => n.UserName == userIn) != null)
					{

						List<Prompt> promptsToDelete = prompts.Where(a => a.UserName == userIn).ToList();

						foreach (Prompt prompt in promptsToDelete)
						{
							_dataContext.Database.ExecuteSqlRaw($"DELETE FROM promptbodies WHERE promptid = '{prompt.Id}'");
						}

						_dataContext.Database.ExecuteSqlRaw($"DELETE FROM prompts WHERE username = '{userIn}'");
						_dataContext.Database.ExecuteSqlRaw($"DELETE FROM completedpremadetitles WHERE userid = '{userToDelete.Id}'");
						_dataContext.Database.ExecuteSqlRaw($"DELETE FROM weekProgresses WHERE userid = '{userToDelete.Id}'");
						_dataContext.Database.ExecuteSqlRaw($"DELETE FROM yearProgresses WHERE userid = '{userToDelete.Id}'");
					}

					_dataContext.Users.Remove(userToDelete);

					await _dataContext.SaveChangesAsync();
					return Ok();
				}
				return NotFound();
			}
			return Unauthorized();
		}
	}
}
