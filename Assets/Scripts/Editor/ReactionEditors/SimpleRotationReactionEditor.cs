using UnityEditor;

[CustomEditor(typeof(SimpleRotationReaction))]
public class SimpleRotationReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel ()
    {
        return "SimpleRotation Reaction";
    }
}
