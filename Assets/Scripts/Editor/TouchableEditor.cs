//http://answers.unity3d.com/questions/801928/46-ui-making-a-button-transparent.html?childToView=851816#answer-851816
// Touchable_Editor component, to prevent treating the component as a Text object.

using UnityEditor;

[CustomEditor (typeof(Touchable))]
public class Touchable_Editor : Editor
{
	public override void OnInspectorGUI ()
	{
		// Do nothing
	}
}