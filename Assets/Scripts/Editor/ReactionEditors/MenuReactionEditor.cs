using UnityEditor;

[CustomEditor(typeof(MenuReaction))]
public class MenuReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel ()
    {
        return "Menu Reaction";
    }
}
