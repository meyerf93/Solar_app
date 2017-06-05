using UnityEditor;

[CustomEditor(typeof(RotationReaction))]
public class RotationReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel ()
    {
        return "RotationReaction Reaction";
    }
}
