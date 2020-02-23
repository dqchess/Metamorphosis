using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class CustomAnimationController : MonoBehaviour {
    Dictionary<int, AnimationClip> AnimationsDictionary { get; set; }
    Animator AnimatorComponent { get; set; }
    GameObject AnimationObject { get; set; }
    PlayableGraph PlayableGraphObject { get; set; }
    AnimationMixerPlayable mixerPlayable { get; set; }


    public CustomAnimationController(GameObject animationObject, Animator animator, Dictionary<int, AnimationClip> animationsDictionary, PlayableGraph playableGraph) {
        this.AnimationObject = animationObject;
        this.AnimatorComponent = animator;
        this.PlayableGraphObject = playableGraph;
        this.AnimationsDictionary = animationsDictionary;
        InitAnimationController();
    }

    public CustomAnimationController() {
    }
    private void InitAnimationController() {
        PlayableGraphObject = PlayableGraph.Create();
        var playableOutput = AnimationPlayableOutput.Create(PlayableGraphObject, "Animation", AnimatorComponent);
        mixerPlayable = AnimationMixerPlayable.Create(PlayableGraphObject, AnimationsDictionary.Count);
        playableOutput.SetSourcePlayable(mixerPlayable);
        foreach (var anim in AnimationsDictionary) {
            var clipPlayable0 = AnimationClipPlayable.Create(PlayableGraphObject, anim.Value);
            bool isConnected = PlayableGraphObject.Connect(clipPlayable0, 0, mixerPlayable, anim.Key);
        }
        PlayableGraphObject.Play();
    }
    private int lastAnimState;
    public void ChangeAnimationState(int currentAnimationState, int nextAnimationState) {
        lastAnimState = nextAnimationState;        
    }

    public void ChangeWeight(int nextAnimationState, float weight) {
        for (int i = 0; i < mixerPlayable.GetInputCount(); i++) {
            if (AnimationsDictionary.ElementAt(i).Key == nextAnimationState) {
                mixerPlayable.SetInputWeight(nextAnimationState, weight);
            }
            else {
                if (mixerPlayable.GetInputWeight(AnimationsDictionary.ElementAt(i).Key) >= 1.0f - weight) {
                    mixerPlayable.SetInputWeight(AnimationsDictionary.ElementAt(i).Key, 1.0f - weight);
                }
            }
        }
    }


}