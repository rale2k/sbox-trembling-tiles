using TremblingGame.Util;

namespace TremblingGame.State;

public class RoundStart : BaseState
{
	public override int Duration => TGame.Freezetime;

	public override void OnStart( bool setForced )
	{
		base.OnStart( setForced );
		
		TGame.ResetTiles();
		foreach ( var player in Players.GetPlayers() )
		{
			player.Respawn();
			player.ToggleFreeze();
		}
	}

	public override State GetState()
	{
		return State.RoundStart;
	}

	protected override void OnTimeUp()
	{
		TGame.Current.ChangeGameState( new InProgress() );
	}
}
