using System.Collections.Generic;
using UnityEngine;

public class EnemyModelValidator : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject root;
        public GameObject model;
        public Health health;
        public SkinnedMeshRenderer meshRenderer;
    }

    void Start()
    {
        Debug.Log("🔍 Running Enemy Model Validation...");
        
        var allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<EnemyInfo> enemyInfos = new List<EnemyInfo>();
        HashSet<GameObject> sharedModels = new HashSet<GameObject>();
        HashSet<SkinnedMeshRenderer> sharedMeshes = new HashSet<SkinnedMeshRenderer>();
        HashSet<Health> sharedHealth = new HashSet<Health>();

        foreach (var enemy in allEnemies)
        {
            var info = new EnemyInfo
            {
                root = enemy,
                health = enemy.GetComponent<Health>(),
                model = enemy.GetComponent<EnemyAI>()?.GetType()
                            .GetField("model", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.GetValue(enemy.GetComponent<EnemyAI>()) as GameObject,
                meshRenderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>()
            };

            // Log findings
            if (info.model != null)
            {
                if (!sharedModels.Add(info.model))
                    Debug.LogWarning($"⚠️ Shared model found: {info.model.name} used by multiple enemies!");
            }

            if (info.health != null)
            {
                if (!sharedHealth.Add(info.health))
                    Debug.LogWarning($"⚠️ Shared Health reference found on: {enemy.name}!");
            }

            if (info.meshRenderer != null)
            {
                if (!sharedMeshes.Add(info.meshRenderer))
                    Debug.LogWarning($"⚠️ Shared SkinnedMeshRenderer found on: {enemy.name}!");
            }

            enemyInfos.Add(info);
        }

        Debug.Log($"✅ Enemy validation complete. {enemyInfos.Count} enemies scanned.");
    }
}

