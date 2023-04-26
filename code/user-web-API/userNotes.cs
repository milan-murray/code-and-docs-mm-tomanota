using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace user_web_API;

public class Prompt
{
	[Key]
	public Guid Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public int Score { get; set; } = 0;
	public DateTime? LastProgressed { get; set; }
	public Guid UserId { get; set; }
	public Prompt()
	{
		Id = Guid.NewGuid();
	}
}

public class PromptBody
{
	[Key]
	public Guid Id { get; set; }
	public string Heading { get; set; }
	public string Body { get; set; }
	public int Score { get; set; }
	[ForeignKey("promptId")]
	public Guid PromptId { get; set; }

	public PromptBody()
	{
		Id = Guid.NewGuid();
	}
}

public enum Language
{
	English,
	Spanish,
	German
}
public class User
{
	[Key]
	public Guid Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public DateTime MostRecentDate { get; set; }
	public Language Language { get; set; }
	public Guid CompletedPremadeId { get; set; }

	public User()
	{
		Id = Guid.NewGuid();
		CompletedPremadeId = Guid.NewGuid();
		MostRecentDate = DateTime.Now;
	}
}

public class WeekProgress
{
	[Key]
	public Guid Id { get; set; }
	[ForeignKey("UserId")]
	public Guid UserId { get; set; }
	public int Monday { get; set; } = 0;
	public int Tuesday { get; set; } = 0;
	public int Wednesday { get; set; } = 0;
	public int Thursday { get; set; } = 0;
	public int Friday { get; set; } = 0;
	public int Saturday { get; set; } = 0;
	public int Sunday { get; set; } = 0;

	public List<int> GetDays()
	{
		return new List<int> { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };
	}

	public void IncDay(int day)
	{
		if (day == 0)
		{
			Monday++;
		}else if (day == 1)
		{
			Tuesday++;
		}else if (day == 2)
		{
			Wednesday++;
		}else if (day == 3)
		{
			Thursday++;
		}else if (day == 4)
		{
			Friday++;
		}else if (day == 5)
		{
			Saturday++;
		}
		else
		{
			Sunday++;
		}
	}

	public void ResetDays()
	{
		Monday = 0; Tuesday = 0; Wednesday = 0; Thursday = 0; Friday = 0; Saturday = 0;	Sunday = 0;
	}

	public WeekProgress()
	{
		Id = Guid.NewGuid();
	}
}

public class YearProgress
{
	[Key]
	public Guid Id { get; set; }
	[ForeignKey("UserId")]
	public Guid UserId { get; set; }
	public int Jan { get; set; } = 0;
	public int Feb { get; set; } = 0;
	public int Mar { get; set; } = 0;
	public int Apr { get; set; } = 0;
	public int May { get; set; } = 0;
	public int Jun { get; set; } = 0;
	public int Jul { get; set; } = 0;
	public int Aug { get; set; } = 0;
	public int Sep { get; set; } = 0;
	public int Oct { get; set; } = 0;
	public int Nov { get; set; } = 0;
	public int Dec { get; set; } = 0;

	public List<int> GetMonths()
	{
		return new List<int> { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
	}

	public void IncMonth(int month)
	{
		if (month == 0)
		{
			Jan++;
		}
		else if (month == 1)
		{
			Feb++;
		}
		else if (month == 2)
		{
			Mar++;
		}
		else if (month == 3)
		{
			Apr++;
		}
		else if (month == 4)
		{
			May++;
		}
		else if (month == 5)
		{
			Jun++;
		}
		else if (month == 6)
		{
			Jul++;
		}
		else if (month == 7)
		{
			Aug++;
		}
		else if (month == 8)
		{
			Sep++;
		}
		else if (month == 9)
		{
			Oct++;
		}
		else if (month == 10)
		{
			Nov++;
		}
		else
		{
			Dec++;
		}
	}

	public void ResetMonths()
	{
		Jan = 0; Feb = 0; Mar = 0; Apr = 0; May = 0; Jun = 0; Jul = 0; Aug = 0; Sep = 0; Nov = 0; Oct = 0; Dec = 0;	
	}
	public YearProgress()
	{
		Id = Guid.NewGuid();
	}
}

public class CompletedPremadeTitle
{
	[Key]
	public Guid Id { get; set; }

	[ForeignKey("UserId")]
	public Guid UserId { get; set; }

	public string titlePremade { get; set; }

	public CompletedPremadeTitle()
	{
		Id = Guid.NewGuid();
	}
}
