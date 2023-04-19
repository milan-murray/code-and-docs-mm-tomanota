namespace user_web_API;

public class userNotes
{
	public User UserInfo { get; set; }
	public string Title { get; set; } = string.Empty;
	public List<List<object>>? Prompts { get; set; } = new List<List<object>>();
	public List<int> ParallelIndividualScores { get; set; } = new List<int>();
	public int Score { get; set; } = 0;
	public DateTime? LastProgressed { get; set; }
}

public class User
{
	public string UserName { get; set; } = string.Empty;
	public DateTime CurrentDate { get; set; }
	public List<int> WeekProgress { get; set; }
	public List<int> MonthProgress { get; set; }
}
