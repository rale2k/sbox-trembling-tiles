using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.State;

public abstract partial class BaseState : BaseNetworkable
{
	[Net] public TimeUntil? TimeRemaining { get; set; }

	public virtual int Duration => 0;

	public bool Forced { get; set; }

	public virtual void OnPlayerJoin( IClient client )
	{
		var player = new TPlayer();
		client.Pawn = player;
	}

	public virtual void OnStart( bool setForced )
	{
		if ( setForced )
		{
			Forced = true;
			return;
		}

		if ( Duration > 0 )
		{
			TimeRemaining = Duration;
		}
	}

	public virtual void OnTick()
	{
		if ( TimeRemaining != null && TimeRemaining.Value && Game.IsServer )
		{
			OnTimeUp();
		}
	}

	protected virtual void OnTimeUp() { }

	public virtual void OnKilled( Sandbox.Entity entity ) { }

	public abstract State GetState();

	public static bool operator ==( BaseState left, State right ) => left?.GetState() == right;

	public static bool operator !=( BaseState left, State right ) => left?.GetState() != right;
}
