using System.Linq;
using Sandbox;
using Sandbox.Diagnostics;
using TremblingGame.Entity;

namespace TremblingGame.Player;

public partial class TPlayer : Sandbox.Player 
{
	public bool ThirdPersonCamera { get; set; }

	public override void Respawn()
	{
		Game.AssertServer();

		Controller = new TWalkController();

		SetModel( "models/citizen/citizen.vmdl");
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
		
		base.Respawn();
	}

	public override void OnKilled()
	{
		Controller = null;
		EnableDrawing = false;
		LifeState = LifeState.Dead;
		EnableAllCollisions = false;
		Client?.AddInt( "deaths", 1 );

		GameManager.Current?.OnKilled( this );
	}

	public override void Simulate( IClient cl )
	{
		SimulateAnimation();

		if ( Input.Pressed( InputButton.View ) )
		{
			ThirdPersonCamera = !ThirdPersonCamera;
		}

		if (Input.Pressed(InputButton.SecondaryAttack) && Game.IsClient)
		{
			TraceRay();
		}

		if ( GroundEntity is TileEntity groundEnt && Game.IsServer)
		{
			groundEnt.StartTremble();
		}

		GetActiveController()?.Simulate( cl, this );
	}

	public override void FrameSimulate( IClient cl )
	{
		Camera.Rotation = ViewAngles.ToRotation();
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
		
		if ( ThirdPersonCamera )
		{
			Camera.FirstPersonViewer = null;

			Vector3 targetPos;
			var center = Position + Vector3.Up * 64;

			var pos = center;
			var rot = Rotation.FromAxis( Vector3.Up, -16 ) * Camera.Rotation;

			float distance = 80.0f * Scale;
			targetPos = pos + rot.Right * ((CollisionBounds.Mins.x + 32) * Scale);
			targetPos += rot.Forward * -distance;

			var tr = Trace.Ray( pos, targetPos )
				.WithAnyTags( "solid" )
				.Ignore( this )
				.Radius( 8 )
				.Run();

			Camera.Position = tr.EndPosition;
		}
		else
		{
			Camera.Position = EyePosition;
			Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
			Camera.FirstPersonViewer = this;
			Camera.Main.SetViewModelCamera( Camera.FieldOfView );
		}
	}

	private void TraceRay()
	{
		Vector3 targetPos = EyePosition + EyeRotation.Forward * 64;
		TraceResult result = Trace.Ray(EyePosition, targetPos)
			.Ignore(this)
			.Run();
		Log.Info($"Traceray result: {result.Entity}");
	}
}
