using Foundation;
using Dynatrace.MAUI;
namespace MyGithubApp;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	public override bool FinishedLaunching(UIApplication app, NSDictionary options)
	{
		Agent.Instance.Start();
		return base.FinishedLaunching(app, options);
	}
}
