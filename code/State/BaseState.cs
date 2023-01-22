using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.State;

public abstract partial class BaseState : BaseNetworkable
{
	[Net]
	public TimeUntil? TimeRemaining { get; set; }

	public virtual int Duration => 0;

	protected readonly TGame GameManager;

	protected BaseState()
	{
		GameManager = TGame.Current;
	}

	public virtual void OnPlayerJoin( IClient client )
	{
		var player = new TPlayer();
		client.Pawn = player;
	}

	public abstract State GetState();

	public virtual void OnStart()
	{
		Log.Info($"{this.ClassName}.OnStart() -> {Duration}");
		if ( Duration > 0 )
		{
			TimeRemaining = Duration;
		}
	}

	public virtual void OnTick()
	{
		if ( TimeRemaining != null && TimeRemaining.Value && Game.IsServer )
		{
			Log.Info($"{this.ClassName}.Ontick() -> {TimeRemaining}");
			OnTimeUp();
		}
	}
	
	public virtual void OnTimeUp(){}

	public virtual void OnKilled( Sandbox.Entity entity ){}

	public enum State
	{
		Waiting,
		Inprogress,
		Roundend
	}
}
