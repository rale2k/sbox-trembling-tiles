using System.Linq;
using System.Threading.Tasks;
using Sandbox;
using Sandbox.UI;

namespace TremblingGame.UI.Notifications;

public partial class CenterNotification : Panel
{
	public string Text { get; set; }

	private CenterNotification()
	{
		LifeCycle();
	}

	[ClientRpc]
	public static void CreateCenterNotification( string text )
	{
		var centerNotificationArea =
			Game.RootPanel.Children.First( p => p.Id.Equals( CenterNotificationArea.CENTER_NOTIFICATION_AREA
			) );
		centerNotificationArea.AddChild(
			new CenterNotification() { Text = text } );
	}

	private async Task LifeCycle()
	{
		await GameTask.DelaySeconds( 5 );
		Delete();
	}
}
