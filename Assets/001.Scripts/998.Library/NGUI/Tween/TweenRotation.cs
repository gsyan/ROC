using UnityEngine;

[AddComponentMenu("BKST/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	public Vector3 from;
	public Vector3 to;
	public bool quaternionLerp = false;

	public Quaternion value
    {
        get { return transform.GetComponent<RectTransform>().localRotation; }
        set { transform.GetComponent<RectTransform>().localRotation = value; }
    }

	protected override void OnUpdate (float factor, bool isFinished)
	{
		value = quaternionLerp ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), factor) :
			Quaternion.Euler(new Vector3(
			Mathf.Lerp(from.x, to.x, factor),
			Mathf.Lerp(from.y, to.y, factor),
			Mathf.Lerp(from.z, to.z, factor)));
	}

	
	static public TweenRotation Begin (GameObject go, float duration, Quaternion rot)
	{
		TweenRotation comp = UITweener.Begin<TweenRotation>(go, duration);
		comp.from = comp.value.eulerAngles;
		comp.to = rot.eulerAngles;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue () { from = value.eulerAngles; }

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue () { to = value.eulerAngles; }

	[ContextMenu("Assume value of 'From'")]
	void SetCurrentValueToStart () { value = Quaternion.Euler(from); }

	[ContextMenu("Assume value of 'To'")]
	void SetCurrentValueToEnd () { value = Quaternion.Euler(to); }
}
