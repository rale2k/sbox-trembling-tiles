using Sandbox;
using TremblingGame.Events;

namespace TremblingGame.State;

public partial class RoundEnd : BaseState
{
	public override int Duration => TGame.RoundEndDuration;

	public override void OnStart( bool setForced )
	{
		base.OnStart( setForced );
		
		Event.Run( GameEvent.Notification.Create, "Round over!" );
	}

	public override void OnTimeUp()
	{
		BaseState newState = TGame.Current.GetPlayerCount() >= TGame.MinPlayers ? new InProgress() : new Waiting();
		TGame.Current.ChangeGameState( newState );
	}

	public override State GetState()
	{
		return State.Roundend;
	}
}
