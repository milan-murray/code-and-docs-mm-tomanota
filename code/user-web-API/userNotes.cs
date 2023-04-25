using System.ComponentModel.DataAnnotations.Schema;

namespace user_web_API;

public class userNotes
{
	// public Guid Id { get; set; }
	public User UserInfo { get; set; }
	public string Title { get; set; } = string.Empty;
	public List<List<object>>? Prompts { get; set; } = new List<List<object>>();
	public List<int> ParallelIndividualScores { get; set; } = new List<int>();
	public int Score { get; set; } = 0;
	public DateTime? LastProgressed { get; set; }

	/*public userNotes()
	{
		Id = Guid.NewGuid();
	}*/
}

public enum Language
{
	English,
	Spanish,
	German
}
public class User
{
	//public Guid Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public DateTime CurrentDate { get; set; }
	public List<int> WeekProgress { get; set; }
	public List<int> MonthProgress { get; set; }
	public Language Language { get; set; }
	public List<string> CompletedPremade { get; set; }

	public User()
	{
		// Id = Guid.NewGuid();
		CompletedPremade = new List<string>();
		WeekProgress = new();
		MonthProgress = new();
	}
}
