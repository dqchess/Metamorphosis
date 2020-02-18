using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimationClipsLoader 
{
    public static List<AnimationClip> GetAnimationClips(string objectName) {
        var animationClips = Resources.LoadAll(string.Format("{0}\\{1}", "Models\\Animations", objectName), typeof(AnimationClip)).Cast<AnimationClip>();
        return animationClips.ToList();
    }
}
