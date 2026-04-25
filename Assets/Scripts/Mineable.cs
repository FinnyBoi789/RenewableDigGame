using UnityEngine;

public class Mineable : MonoBehaviour
{
    public float mineDuration = 3f;
    [HideInInspector] public float currentMine = 0f;
    [HideInInspector] public bool isMined = false;
    public bool requiresTurbine = false;
    public bool isSpaceship = false;
    
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private Transform spawnPosition;

    public void SpawnDrop(Vector3 position)
    {
        if (dropPrefab != null)
        {
            Vector3 pos = spawnPosition != null ? spawnPosition.position : position + Vector3.up * 0.5f;
            Quaternion rot = spawnPosition != null ? spawnPosition.rotation : Quaternion.identity;
            GameObject drop = Instantiate(dropPrefab, pos, rot);
            drop.SetActive(true);
        }
    }
}
