using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class HelperTools {
    public static GameObject FindObjectInChildWithTag(GameObject parent, string tag) { 
        Transform t = parent.transform;
        foreach (Transform tr in t) {
            if (tr.tag == tag) {
                return tr.gameObject;
            }
        }
        return null;
    }
}

