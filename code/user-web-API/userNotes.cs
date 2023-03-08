namespace user_web_API;

public class userNotes
{
	public string User { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public List<List<object>>? Prompts { get; set; } = new List<List<object>>();
	public int Score { get; set; } = 0;
}