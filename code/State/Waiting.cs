using Sandbox;
using TremblingGame.Player;
using TremblingGame.Util;

namespace TremblingGame.State;

public class Waiting : BaseState
{
	public override void OnPlayerJoin( IClient client )
	{
		base.OnPlayerJoin( client );

		TPlayer player = client.Pawn as TPlayer;
		player.EnableDrawing = false;
		player.EnableAllCollisions = false;

		if ( Players.GetPlayerCount() >= TGame.MinPlayers )
		{
			TGame.Current.ChangeGameState( new RoundStart() );
		}
	}

	public override State GetState()
	{
		return State.Waiting;
	}
}
