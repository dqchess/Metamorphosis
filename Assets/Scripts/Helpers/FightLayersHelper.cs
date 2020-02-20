using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FightLayersHelper {
    public static bool IsCharacterInTargetsList(GameObject parentObject,GameObject targetObject) {
        bool isInList = false;
        FightCorrelationsProvider correlationsProvider = new FightCorrelationsProvider();
        List<FightCorrelation> correlations = correlationsProvider.GetFightCorrelations();
        FightCorrelation currentFightCorrelation = correlations.FirstOrDefault(s => s.ObjectTag == parentObject.tag);
        if (currentFightCorrelation!=null) {
            isInList = currentFightCorrelation.TargetTags.Contains(targetObject.tag);
        }        
        return isInList;
    }
}

public class FightCorrelationsProvider {
    List<FightCorrelation> GetDinamicallyGeneratedCorrelations() {
        List<FightCorrelation> list = new List<FightCorrelation>();

        FightCorrelation playerFightCorrelation = new FightCorrelation("Player");
        playerFightCorrelation.TargetTags.Add("SimpleEnemy");
        playerFightCorrelation.TargetTags.Add("SmartEnemy");
        list.Add(playerFightCorrelation);

        FightCorrelation enemyFightCorrelation = new FightCorrelation("SimpleEnemy");
        enemyFightCorrelation.TargetTags.Add("Player");
        enemyFightCorrelation.TargetTags.Add("SmartEnemy");
        list.Add(enemyFightCorrelation);

        return list;
    }
    public List<FightCorrelation> GetFightCorrelations() {
        List<FightCorrelation> fightCorrelations = new List<FightCorrelation>();
        fightCorrelations = GetDinamicallyGeneratedCorrelations();
        return fightCorrelations;
    }
}

public class FightCorrelation {
    public string ObjectTag { get; set; }
    public List<string> TargetTags { get; set; }
    public FightCorrelation() {
        TargetTags = new List<string>();
    }
    public FightCorrelation(string objectTag) {
        ObjectTag = objectTag;
        TargetTags = new List<string>();
    }
}
