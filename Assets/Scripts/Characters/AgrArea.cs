using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgrArea : MonoBehaviour {
    SimpleEnemyController SimpleEnemyController;
    GameObject ParentObject;
    // Start is called before the first frame update
    void Start() {
        ParentObject = transform.parent.gameObject;
        SimpleEnemyController = GetComponentInParent<SimpleEnemyController>();
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter(Collider collider) {
        if (FightLayersHelper.IsCharacterInTargetsList(ParentObject, collider.gameObject)) {
            SimpleEnemyController.TargetsInAgrArea.Add(collider.gameObject);
        }
    }
    void OnTriggerExit(Collider collider) {
        if (SimpleEnemyController.TargetsInAgrArea.Contains(collider.gameObject)) {
            SimpleEnemyController.TargetsInAgrArea.Remove(collider.gameObject);
        }
    }
}
