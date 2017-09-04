using UnityEditor;

[CustomEditor(typeof(ResetToogleReaction))]
public class ResetToogleReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel()
    {
        return "Reset Toogle Reaction";
    }
}
