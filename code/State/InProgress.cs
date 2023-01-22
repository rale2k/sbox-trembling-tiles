using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.State;

public partial class InProgress : BaseState
{
	public override void OnStart( bool setForced )
	{
		base.OnStart( setForced );

		foreach ( var player in TGame.Current.GetPlayers() )
		{
			player.ToggleFreeze();
		}
	}

	public override void OnKilled( Sandbox.Entity entity )
	{
		base.OnKilled( entity );

		if ( TGame.Current.GetAlivePlayerCount() < 2 )
		{
			TGame.Current.ChangeGameState( new RoundEnd() );
		}
	}

	public override void OnPlayerJoin( IClient client )
	{
		base.OnPlayerJoin( client );

		TPlayer player = client.Pawn as TPlayer;
		player.EnableDrawing = false;
		player.EnableAllCollisions = false;
	}

	public override State GetState()
	{
		return State.Inprogress;
	}
}
