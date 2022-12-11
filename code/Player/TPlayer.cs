using Sandbox;
using TremblingGame.Entity;

namespace TremblingGame.Player;

public partial class TPlayer : Sandbox.Player 
{
	[Net, Predicted]
	public bool ThirdPersonCamera { get; set; }
	
	public override void Respawn()
	{
		base.Respawn();

		SetModel( "models/citizen/citizen.vmdl");
		
		Controller = new TWalkController();
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void OnKilled()
	{
		base.OnKilled();
		
		Controller = null;
		EnableAllCollisions = false;
		EnableDrawing = false;
	}

	public override void Simulate( IClient cl )
	{
		if ( Input.Pressed( InputButton.View ) )
		{
			ThirdPersonCamera = !ThirdPersonCamera;
		}
		
		if (Input.Pressed(InputButton.SecondaryAttack) && Game.IsClient)
		{
			TraceRay();
		}

		TileEntity groundEnt = GroundEntity as TileEntity;
		if ( groundEnt != null && Game.IsServer)
		{
			groundEnt.StartTremble();
		}
		
		var controller = GetActiveController();
		if ( controller != null )
		{
			EnableSolidCollisions = !controller.HasTag( "noclip" );

			SimulateAnimation( controller );
		}
		
		base.Simulate( cl );
	}

	private void TraceRay()
	{
		Vector3 targetPos = EyePosition + EyeRotation.Forward * 64;
		TraceResult result = Trace.Ray(EyePosition, targetPos)
			.Ignore(this)
			.Run();
		Log.Info(result.Entity);
	}

	public override void FrameSimulate( IClient cl )
	{
		Camera.Rotation = ViewAngles.ToRotation();
		
		if ( ThirdPersonCamera )
		{
			Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
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
	

}
