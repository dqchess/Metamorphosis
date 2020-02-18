using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers {
    public static class AnimationHelper {
        public static AnimationClip GetAnimationClipByName(List<AnimationClip> animationClipsList, string animationName) {
            return animationClipsList.FirstOrDefault(s => s.name == animationName);
        }
        public static Dictionary<int,AnimationClip> GetAnimationsDictionary(Type enumType,List<AnimationClip> animationClipsList) {
            Dictionary<int, AnimationClip> dictionary = new Dictionary<int, AnimationClip>();
            var values = Enum.GetValues(enumType);
            var names = Enum.GetNames(enumType);
            for (int i = 0; i < names.Length; i++) {
                var animClip = animationClipsList.FirstOrDefault(s => s.name == names[i]);
                var intValue = Convert.ToInt32(values.GetValue(i));
                dictionary.Add(intValue, animClip);
            }
            return dictionary;
        }
    }
    
}
