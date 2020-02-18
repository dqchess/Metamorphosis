using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationEnums;

public class CharacterControllerBase : MonoBehaviour {
    
    public int HealthPoints { get; set; } = 100;
    public int AttackDamage { get; set; } = 10;
    public float Speed { get; set; } = 8.0f;
    public float PunchForce { get; set; } = 1.0f;

       

    protected float AttackSpeed { get; set; } = 0.2f;
    protected float LastAttackTime { get; set; } = 0.0f;
    protected bool CanAttack {
        get {
            return Time.time > LastAttackTime + AttackSpeed;
        }
    }
    
    public List<GameObject> TargetsInAttackArea { get; set; }
    protected Rigidbody Rigidbody;

    // Start is called before the first frame update
    protected virtual void Start() {              
        TargetsInAttackArea = new List<GameObject>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update() {

    }
    protected virtual void FixedUpdate() {
        ApplyAnimations();
    }

    protected virtual void ApplyDamage(int damage) {
        HealthPoints -= damage;
        Debug.Log(string.Format("{0}_{1}", gameObject.tag, HealthPoints));
        CheckToDead();
    }

    protected virtual void ApplyPunch(float punchForce) {

    }
    protected virtual void ApplyAnimations() {

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
}
