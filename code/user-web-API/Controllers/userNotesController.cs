using Microsoft.AspNetCore.Mvc;

namespace user_web_API.Controllers;

[ApiController]
[Route("[controller]")]
// [Produces("Application:JSON")]
public class userNotesController : ControllerBase
{
	// List<List<object>> testNote = new ()
	// {
	// 	new List<object>() { "String1", "String2" },
	// 	new List<object>() { "String3", "String4" }
	// };

	private static readonly List<userNotes> noteStorage = new()
	{
		new userNotes { User = "Tester", Title = "Personal-Notes", Score = 0  }
	};

	[HttpGet]
	public IEnumerable<userNotes> getAll()
	{
		return noteStorage;
	}

	[HttpPut("newNote/{userIn}/{titleIn}/{scoreIn}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public IActionResult putNote([FromRoute] string userIn, [FromRoute] string titleIn, [FromBody] List<List<object>> PromptsIn, [FromRoute] int scoreIn)
	{
		noteStorage.Add(new userNotes { User = userIn, Title = titleIn, Prompts = PromptsIn, Score = scoreIn });
		return NoContent();
	}

}