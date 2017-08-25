using UnityEditor;

[CustomEditor(typeof(RotationAnimationReaction))]
public class RotationAnimationReactionEditor : ReactionEditor {

    protected override string GetFoldoutLabel()
    {
        return "RotationAnimation Reaction";
    }
}

