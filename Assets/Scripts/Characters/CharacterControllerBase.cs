using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationEnums;
using UnityEngine.Playables;
using Assets.Scripts.Helpers;

public class CharacterControllerBase : MonoBehaviour {

    public int HealthPoints { get; set; } = 100;
    public int AttackDamage { get; set; } = 10;
    public float Speed { get; set; } = 8.0f;
    public float PunchForce { get; set; } = 1.0f;

    protected bool _animationStateChanged = true;
    protected float AttackSpeed { get; set; } = 2.0f;
    protected float LastAttackTime { get; set; } = 0.0f;
    protected PlayableGraph PlayableGraphObject { get; set; }
    protected CustomAnimationController CustomAnimationController { get; set; }


    protected bool CanAttack {
        get {
            return Time.time > LastAttackTime + 1 / AttackSpeed;
        }
    }

    public List<GameObject> TargetsInAttackArea { get; set; }


    protected int AnimationState { get; set; } = 0;
    protected Dictionary<int, AnimationClip> MovementAnimationDictionary { get; set; }
    protected Rigidbody Rigidbody;
    protected List<AnimationClip> AnimationClips { get; set; }
    protected GameObject AnimationBody { get; set; }


    
    protected virtual void Start() {
        TargetsInAttackArea = new List<GameObject>();
        Rigidbody = GetComponent<Rigidbody>();
        AnimationClips = AnimationClipsLoader.GetAnimationClips(gameObject.tag, "Movement");
        MovementAnimationDictionary = InitAnimationsDictionary();
        PlayableGraphObject = new PlayableGraph();
        AnimationBody = HelperTools.FindObjectInChildWithTag(gameObject, "BodyModel");
        CustomAnimationController = new CustomAnimationController(AnimationBody, AnimationBody.GetComponent<Animator>(), MovementAnimationDictionary, PlayableGraphObject);
    }
    private int lastAnimState { get; set; }
    private float weight = 0.0f;    
    protected virtual void Update() {
        
    }
    protected virtual void FixedUpdate() {        
        if ( lastAnimState != AnimationState) {            
            CustomAnimationController.ChangeWeight(AnimationState, weight);
            weight += 0.1f;
        }
        if (weight>=1.0f) {
            CustomAnimationController.ChangeWeight(AnimationState, 1.0f);
            weight = 0.0f;
            lastAnimState = AnimationState;
        }   
    }

    protected virtual void ApplyDamage(int damage) {
        HealthPoints -= damage;        
        CheckToDead();
    }

    protected virtual void ApplyPunch(float punchForce) {

    }
    protected bool isAnimationChanged = false;
    protected virtual void ChangeAnimation( int nextAnimationState) {        
        AnimationState = nextAnimationState;                     
    }
    protected virtual Dictionary<int, AnimationClip> InitAnimationsDictionary() {
        return null;
    }

    protected virtual void CheckToDead() {
        if (HealthPoints <= 0) {
            Destroy(gameObject);
        }
    }
    protected virtual bool DoAttack() {
        if (!CanAttack)
            return false;
        LastAttackTime = Time.time;
        List<GameObject> needToRemove = new List<GameObject>();
        TargetsInAttackArea.RemoveAll(s => s == null);
        foreach (var character in TargetsInAttackArea) {
            CharacterControllerBase characterControllerBase = character.GetComponent<CharacterControllerBase>();
            characterControllerBase.ApplyPunch(PunchForce);
            characterControllerBase.ApplyDamage(AttackDamage);
        }
        return true;
    }

    protected virtual void SetAnimationState(int nextAnimationState) {
        if (AnimationState != nextAnimationState) {
            ChangeAnimation(nextAnimationState);
        }
    }
}
