using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.State;

public class InProgress : BaseState
{
	public override void OnStart()
	{
		base.OnStart();
		
		TGame.ResetTiles();
		foreach (var player in GameManager.GetPlayers())
		{
			player.Respawn();
		}
	}

	public override void OnKilled( Sandbox.Entity entity )
	{
		base.OnKilled( entity );
		
		Log.Info( $"{this.ClassName}.OnKilled({entity}) - playercount = {GameManager.GetAlivePlayerCount()}" );
		if ( GameManager.GetAlivePlayerCount() == 1 )
		{
			GameManager.ChangeGameState( new RoundEnd() );
		}
	}

	public override void OnPlayerJoin(IClient client)
	{
		base.OnPlayerJoin(client);
		
		TPlayer player = client.Pawn as TPlayer;
        player.EnableDrawing = false;
        player.EnableAllCollisions = false;
	}

	public override State GetState()
	{
		return State.Inprogress;
	}
}
