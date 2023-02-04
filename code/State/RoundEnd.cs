using Sandbox;
using TremblingGame.UI.Notifications;
using TremblingGame.Util;

namespace TremblingGame.State;

public partial class RoundEnd : BaseState
{
	public override int Duration => TGame.RoundEndDuration;

	public override void OnStart( bool setForced )
	{
		base.OnStart( setForced );

		CenterNotification.CreateCenterNotification( "Round over!" );
	}

	protected override void OnTimeUp()
	{
		BaseState newState = Players.GetPlayerCount() >= TGame.MinPlayers ? new RoundStart() : new Waiting();
		TGame.Current.ChangeGameState( newState );
	}

	public override State GetState()
	{
		return State.Roundend;
	}
}
