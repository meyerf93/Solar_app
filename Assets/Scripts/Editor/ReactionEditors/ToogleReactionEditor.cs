using UnityEditor;

[CustomEditor(typeof(ToogleReaction))]
public class ToogleReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel ()
    {
        return "ToogleReaction";
    }
}
