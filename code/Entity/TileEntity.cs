using System;
using Editor;
using Sandbox;

namespace TremblingGame.Entity;

/// <summary>
/// This entity represents a trembling tile
/// </summary>
[Library( "trembling_tile" ), HammerEntity]
[Category("TremblingTile")]
[Title( "A Trembling Tile" )]
[EditorModel( "models/tt-tile.vmdl" )]
[Solid]
partial class TileEntity : ModelEntity
{
	[ConVar.Replicated("trt_tile_tremble_time", 
		Help = "Time after which a tile disappears after a player has stepped on it")
	]
	private static int TremblingTimeInSeconds{ get; set; } = 2;
	
	[Net] 
	private TimeUntil? TimeUntilTremble { get; set; }
	
	public override void Spawn()
	{
		base.Spawn();
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		RenderColor = Color.Random;
		Tags.Add( "solid" );
	}

	public void ResetTile()
	{
		TimeUntilTremble = null;
		RenderColor = Color.Random;
		EnableDrawing = true;
		EnableAllCollisions = true;
	}

	public bool HasFallen()
	{
		return TimeUntilTremble.HasValue && TimeUntilTremble.Value.Fraction >= 1;
	}
	
	[Event.Tick]
	public void Tremble()
	{
		if ( !TimeUntilTremble.HasValue )
			return;
		
		if (!HasFallen())
		{
			DebugOverlay.Text(TimeUntilTremble.Value.Fraction.ToString(), Position);
			var newRenderColor = RenderColor;
			newRenderColor.b = 1 - TimeUntilTremble.Value.Fraction;
			newRenderColor.g = 1 - TimeUntilTremble.Value.Fraction;

			RenderColor = newRenderColor;
		} else
		{
			EnableDrawing = false;
			EnableAllCollisions = false;
		}
	}

	public void StartTremble()
	{
		if (Game.IsServer && !TimeUntilTremble.HasValue)
		{
			TimeUntilTremble = TremblingTimeInSeconds;
		}
	}
}
