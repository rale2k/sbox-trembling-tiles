using System.Linq;
using System.Runtime.CompilerServices;
using Sandbox;
using TremblingGame.Entity;
using TremblingGame.Events;
using TremblingGame.Player;
using TremblingGame.State;

namespace TremblingGame;

public partial class TGame
{
	[ConVar.Replicated( "trt_tile_tremble_time",
		Help = "Time after which a tile disappears after a player has stepped on it" )
	]
	public static int TremblingTimeInSeconds { get; internal set; } = 2;
	
	[ConVar.Replicated( "trt_minplayers", Min = 2, Help = "Minimum amount of players")]
	public static int MinPlayers { get; internal set; } = 2;
	
	[ConVar.Replicated( "trt_roundendduration", Min = 5, Help = "Time between two rounds")]
	public static int RoundEndDuration { get; internal set; } = 5;

	[ConCmd.Admin( "trt_resettiles", Help = "Reset all trembling tiles" )]
	public static void ResetTiles()
	{
		var tremblingTiles = All.Where( ent => ent is TileEntity );

		foreach ( var tremblingTile in tremblingTiles )
		{
			(tremblingTile as TileEntity)?.ResetTile();
		}
	}	
	
	[ConCmd.Admin( "trt_startgame", Help = "(Re)Start game" )]
	public static void StartGame()
	{
		Current.ChangeGameState( new InProgress() );
	}	
	
	[ConCmd.Client( "trt_testnotification", Help = "Test notification" )]
	public static void TestNotification(string text)
	{
		Event.Run( GameEvent.Notification.Create, text );
	}	
	
	[ConCmd.Admin( "trt_forcestate", Help = "Force game state" )]
	public static void ForceState(string state)
	{
		if (state == "waiting")
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
	}

}
