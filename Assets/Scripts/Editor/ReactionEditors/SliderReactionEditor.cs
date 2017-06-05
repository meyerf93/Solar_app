using UnityEditor;

[CustomEditor(typeof(SliderReaction))]
public class SliderReactionEditor : ReactionEditor
{
    protected override string GetFoldoutLabel()
    {
        return "Slider Reaction";
    }
}
