using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.State;

public class Waiting : BaseState
{

	public override void OnPlayerJoin(IClient client)
	{
		base.OnPlayerJoin(client);
		
		TPlayer player = client.Pawn as TPlayer;
		player.EnableDrawing = false;
		player.EnableAllCollisions = false;

		if ( TGame.Current.GetPlayerCount() >= TGame.MinPlayers )
		{
			TGame.Current.ChangeGameState( new InProgress() );
		}
	}

	public override State GetState()
	{
		return State.Waiting;
	}
}
