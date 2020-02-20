using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour {
    CharacterControllerBase CharacterControllerBase;
    GameObject ParentObject;
    // Start is called before the first frame update
    void Start() {
        ParentObject = transform.parent.gameObject;
        CharacterControllerBase = GetComponentInParent<CharacterControllerBase>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider collider) {        
        if (FightLayersHelper.IsCharacterInTargetsList(ParentObject, collider.gameObject)) {
            Debug.Log(string.Format("{0} In attack area of the {1}", collider.gameObject.name, ParentObject.name));
            CharacterControllerBase.TargetsInAttackArea.Add(collider.gameObject);                        
        }
    }
    void OnTriggerExit(Collider collider) {
        if (CharacterControllerBase.TargetsInAttackArea.Contains(collider.gameObject)) {
            CharacterControllerBase.TargetsInAttackArea.Remove(collider.gameObject);
        }
    }
}
