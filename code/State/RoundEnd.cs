using Sandbox;

namespace TremblingGame.State;

public class RoundEnd : BaseState
{
	public override int Duration => TGame.RoundEndDuration;

	public override void OnTimeUp()
	{
		GameManager.ChangeGameState(new InProgress());
	}

	public override State GetState()
	{
		return State.Roundend;
	}
}
