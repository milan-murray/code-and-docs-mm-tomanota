using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;

namespace TomaNota
{
	public interface IFirebaseAuthService
	{
		FirebaseAuthClient FirebaseAuthClient { get; }
	}
	public class FirebaseAuthService : IFirebaseAuthService
	{
		public FirebaseAuthClient FirebaseAuthClient { get; }

		public FirebaseAuthService()
		{
			var config = new FirebaseAuthConfig
			{
				ApiKey = "AIzaSyCLIUAHHeGxM5Pe_AdC8o4jGMIFwr3ZlH0",
				AuthDomain = "tn-fb-ff018.firebaseapp.com",
				Providers = new FirebaseAuthProvider[]
				{
					// Add and configure individual providers
					new GoogleProvider().AddScopes("email"),
					new EmailProvider()
					// ...
				},
				// WPF:
				UserRepository = new FileUserRepository("FirebaseSample")
			};

			FirebaseAuthClient = new FirebaseAuthClient(config);
		}
	}
}
