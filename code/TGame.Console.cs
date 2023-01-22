using System.Linq;
using Sandbox;
using TremblingGame.Entity;
using TremblingGame.State;
using TremblingGame.UI.Notifications;

namespace TremblingGame;

public partial class TGame
{
	[ConVar.ReplicatedAttribute( "trt_tile_tremble_time",
		Help = "Time after which a tile disappears after a player has stepped on it" )
	]
	public static int TremblingTimeInSeconds { get; internal set; } = 2;

	[ConVar.ReplicatedAttribute( "trt_freezetime",
		Help = "Time spent frozen before round starts" )
	]
	public static int Freezetime { get; internal set; } = 2;

	[ConVar.ReplicatedAttribute( "trt_minplayers", Min = 2, Help = "Minimum amount of players" )]
	public static int MinPlayers { get; internal set; } = 2;

	[ConVar.ReplicatedAttribute( "trt_roundendduration", Min = 5, Help = "Time between two rounds" )]
	public static int RoundEndDuration { get; internal set; } = 5;

	[ConCmd.AdminAttribute( "trt_resettiles", Help = "Reset all trembling tiles" )]
	public static void ResetTiles()
	{
		var tremblingTiles = All.Where( ent => ent is TileEntity );

		foreach ( var tremblingTile in tremblingTiles )
		{
			(tremblingTile as TileEntity)?.ResetTile();
		}
	}

	[ConCmd.AdminAttribute( "trt_startgame", Help = "(Re)Start game" )]
	public static void StartGame()
	{
		Current.ChangeGameState( new RoundStart() );
	}

	[ConCmd.AdminAttribute( "trt_testnotification", Help = "Test notification" )]
	public static void TestNotification( string text )
	{
		CenterNotification.CreateCenterNotification( text );
	}

	[ConCmd.AdminAttribute( "trt_forcestate", Help = "Force game state" )]
	public static void ForceState( string state )
	{
		if ( state == "waiting" )
		{
			Current.ChangeGameState( new Waiting(), true );
		}
		else if ( state == "inprogress" )
		{
			Current.ChangeGameState( new InProgress(), true );
		}
		else if ( state == "roundend" )
		{
			Current.ChangeGameState( new RoundEnd(), true );
		}
		else if ( state == "roundstart" )
		{
			Current.ChangeGameState( new RoundStart(), true );
		}
	}
}
