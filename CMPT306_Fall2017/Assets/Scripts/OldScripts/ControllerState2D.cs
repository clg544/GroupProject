using UnityEngine;
using System.Collections;

public class ControllerState2D
{
	public bool IsCollideRight{ get; set;}
	public bool IsCollideLeft{ get; set;}
	public bool IsCollideAbove{ get; set;}
	public bool IsCollideBelow{ get; set;}
	public bool IsMovingDownSlope{ get; set;}
	public bool IsMovingUpSlope{ get; set;}
	public bool IsGrounded{ get { return IsCollideBelow; } }
	public float SlopeAngle{ get; set;}

	public bool HasCollisions { get {return IsCollideRight || IsCollideLeft || IsCollideAbove || IsCollideBelow;}}

	public void Reset()
	{
		IsMovingUpSlope =
			IsMovingDownSlope =
			IsCollideLeft =
			IsCollideRight =
			IsCollideAbove =
			IsCollideBelow = false;

		SlopeAngle = 0;
	}

	public override string ToString()
	{
		return string.Format(
			"(controller: r:{0} 1:{1} a:{2} b:{3} down-slope:{4} up-slope: {5} angle: {6}",
			IsCollideLeft,
			IsCollideRight,
			IsCollideAbove,
			IsCollideBelow,
			IsMovingDownSlope,
			IsMovingUpSlope,
			SlopeAngle);
	}
}