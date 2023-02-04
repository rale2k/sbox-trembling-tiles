using System.Collections.Generic;
using System.Linq;
using Sandbox;
using TremblingGame.Player;

namespace TremblingGame.Util;

public static class Players
{
	
	public static ICollection<TPlayer> GetPlayers()
	{
		return TGame.All.OfType<TPlayer>().ToList();
	}

	public static int GetPlayerCount()
	{
		return TGame.All.OfType<TPlayer>().Count();
	}

	public static int GetAlivePlayerCount()
	{
		return TGame.All.OfType<TPlayer>().Count( player => player.LifeState == LifeState.Alive );
	}
}
