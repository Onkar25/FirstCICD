using Dynatrace.MAUI;
namespace MyGithubApp;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		InitiateDynatrace();
	}

	private void InitiateDynatrace()
	{

		// Privacy settings configured below are only provided
		// to allow a quick start with capturing monitoring data.
		// This has to be requested from the user
		// (e.g. in a privacy settings screen) and the user decision
		// has to be applied similar to this example.
		UserPrivacyOptions options = new(DataCollectionLevel.UserBehavior, true);
		Agent.Instance.ApplyUserPrivacyOptions(options);
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		IRootAction myAction = Agent.Instance.EnterAction("Tap on Confirm");
		//Perform the action and whatever else is needed.
		myAction.LeaveAction();
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		OnConfirmTapped();

		SemanticScreenReader.Announce(CounterBtn.Text);

		// throw new Exception("This is a test exception to check Dynatrace error handling.");
	}
	private void ErrorButtonPressed()
	{
		// Start an action
		IRootAction myAction = Agent.Instance.EnterAction("Test Error Action");

		try
		{
			// Simulate some faulty logic
			throw new InvalidOperationException("⚠️ This is a test error for Dynatrace");
		}
		catch (Exception ex)
		{
			// Report error to Dynatrace without crashing the app
			myAction.ReportError("Test Error occurred", -1);
		}
		finally
		{
			myAction.LeaveAction();
		}
	}

	private void CrashButtonPressed()
	{
		// Unhandled exception = app crash
		Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		// throw new Exception("💥 This is a test crash for Dynatrace");
	}
	public void OnConfirmTapped()
	{
		// Start the root action for the tap event
		IRootAction myAction = Agent.Instance.EnterAction("Tap on Confirm");

		try
		{
			// Attach custom metadata (key/value pairs)
			myAction.ReportValue("UserID", "12345");
			myAction.ReportValue("OrderID", "ORD-9988");
			myAction.ReportValue("ButtonColor", "Green");

			// Simulate a child action (e.g., API request)
			IAction apiAction = myAction.EnterAction("Call PlaceOrder API");
			try
			{
				// Simulate the API call work
				CallPlaceOrderApi();
			}
			catch (Exception)
			{
				// Report an error to Dynatrace
				apiAction.ReportError("PlaceOrder API failed", -1);
			}
			finally
			{
				apiAction.LeaveAction(); // End API action
			}

			// Simulate another child action (e.g., Local DB write)
			IAction dbAction = myAction.EnterAction("Save to Local Database");
			SaveToLocalDatabase();
			dbAction.LeaveAction();
		}
		finally
		{
			// End the root action
			myAction.LeaveAction();
		}
	}

	private void CallPlaceOrderApi()
	{
		// Simulated API call
		ErrorButtonPressed();
		Thread.Sleep(500); // pretend network delay
	}

	private void SaveToLocalDatabase()
	{
		// Simulated DB write
		CrashButtonPressed();
		Thread.Sleep(200);
	}
}

