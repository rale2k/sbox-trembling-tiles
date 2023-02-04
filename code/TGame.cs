using System;
using System.Collections.Generic;
using System.Linq;
using TremblingGame.UI;
using Sandbox;
using TremblingGame.Entity;
using TremblingGame.Player;
using TremblingGame.State;

namespace TremblingGame;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class TGame : GameManager
{
	public static TGame Current { get; private set; }

	[Net, Change] public BaseState Gamestate { get; set; }

	public TGame()
	{
		Current = this;

		if ( Game.IsClient )
			_ = new UI.Hud();
	}

	public override void ClientJoined( IClient client )
	{
		Gamestate.OnPlayerJoin( client );
	}

	public override void OnKilled( Sandbox.Entity pawn )
	{
		base.OnKilled( pawn );
		Gamestate.OnKilled( pawn );
	}

	[Event.Tick]
	public void OnTick()
	{
		Gamestate.OnTick();
	}

	public override void MoveToSpawnpoint( Sandbox.Entity pawn )
	{
		var tileEnt = All
			.OfType<TileEntity>().MinBy( _ => Guid.NewGuid() );

		if ( tileEnt == null )
		{
			Log.Warning( $"Couldn't find spawnpoint for {pawn}!" );
			return;
		}

		var tileEntTransform = tileEnt.Transform;
		var tileEntPosition = tileEnt.Position;
		tileEntPosition.z += 5;

		tileEntTransform.Position = tileEntPosition;

		pawn.Transform = tileEntTransform;
	}

	public override void PostLevelLoaded()
	{
		ChangeGameState( new Waiting() );
	}

	public void ChangeGameState( BaseState state, bool forced = false )
	{
		Game.AssertServer();

		Gamestate = state;
		Gamestate.OnStart( forced );
	}

	private void OnGamestateChanged( BaseState oldState, BaseState newState )
	{
		Log.Info( $"Changing state to {newState.GetState()}" );
		Gamestate = newState;
	}
}
