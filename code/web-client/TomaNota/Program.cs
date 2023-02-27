using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using TomaNota;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IFirebaseAuthService, FirebaseAuthService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();

/*
// Configure...
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
	UserRepository = new FileUserRepository("FirebaseSample") // persist data into %AppData%\FirebaseSample
	// UWP:
	// UserRepository = new StorageRepository() // persist data into ApplicationDataContainer
};

var client = new FirebaseAuthClient(config);
*/