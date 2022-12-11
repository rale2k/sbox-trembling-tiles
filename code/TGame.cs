using System;
using System.Linq;
using TremblingGame.UI;
using Sandbox;
using TremblingGame.Entity;
using TremblingGame.Player;

namespace TremblingGame;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
partial class TGame : GameManager
{
	
	public TGame()
	{
		if ( Game.IsServer )
		{
			// Create the HUD
			_ = new TGameUI();
		}
	}
	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );
			
		// Create a pawn for this client to play with
		var player = new TPlayer();
		client.Pawn = player;
		
		player.Respawn();
	}

	public override void MoveToSpawnpoint( Sandbox.Entity pawn )
	{
		var tileEnt = All
			.OfType<TileEntity>().MinBy(_ => Guid.NewGuid()); 

		if ( tileEnt == null )
		{
			Log.Warning( $"Couldn't find spawnpoint for {pawn}!" );
			return;
		}

		var tileEntTransform = tileEnt.Transform;
		var tileEntPosition = tileEnt.Position;
		tileEntPosition.z += 8;

		tileEntTransform.Position = tileEntPosition;
		
		pawn.Transform = tileEntTransform;

	}

	[ConCmd.AdminAttribute( "trt_resettiles", Help = "Reset all trembling tiles")]
	static void ResetTiles()
	{
		var tremblingTiles = All.Where( ent => ent is TileEntity );
		
		foreach (var tremblingTile in tremblingTiles)
		{
			(tremblingTile as TileEntity)?.ResetTile();
		}
	}


}
