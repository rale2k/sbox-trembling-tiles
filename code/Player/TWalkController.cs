using Sandbox;

namespace TremblingGame.Player;

public class TWalkController : WalkController
{
	public override float GetWishSpeed()
	{
		var ws = Duck.GetWishSpeed();
		if (ws >= 0) return ws;

		if (Input.Down(InputButton.Walk)) return WalkSpeed;

		return DefaultSpeed;
	}
}
