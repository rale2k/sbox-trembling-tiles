using Sandbox;
using Sandbox.UI;

namespace TremblingGame.UI;

public partial class TGameUI : HudEntity<RootPanel>
{
	public TGameUI()
	{
		if ( !Game.IsClient )
			return;

		RootPanel.StyleSheet.Load( "UI/Styles/TGameUi.scss" );

		RootPanel.AddChild<Hud>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
	}
}
