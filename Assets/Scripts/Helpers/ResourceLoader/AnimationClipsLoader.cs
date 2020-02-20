using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimationClipsLoader 
{
    public static List<AnimationClip> GetAnimationClips(string objectName, string animationType) {
        var animationClips = Resources.LoadAll(string.Format("{0}\\{1}\\{2}", "Models\\Animations", objectName, animationType), typeof(AnimationClip)).Cast<AnimationClip>();
        return animationClips.ToList();
    }
}
