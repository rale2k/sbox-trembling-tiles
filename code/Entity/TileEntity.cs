using System;
using Editor;
using Sandbox;

namespace TremblingGame.Entity;

/// <summary>
/// A tile that trembles
/// </summary>
[Library( "trembling_tile" ), HammerEntity]
[Category("TremblingTile")]
[Title( "A Trembling Tile" )]
[EditorModel( "models/tt-tile.vmdl" )]
[Solid]
partial class TileEntity : ModelEntity
{

	
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
			TimeUntilTremble = TGame.TremblingTimeInSeconds;
		}
	}
}
