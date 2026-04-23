using UnityEngine;

public class Mineable : MonoBehaviour
{
    public float mineDuration = 3f;
    [HideInInspector] public float currentMine = 0f;
    [HideInInspector] public bool isMined = false;
    [HideInInspector] public bool requiresTurbine = false;
    
    [SerializeField] private GameObject dropPrefab;

    public void SpawnDrop(Vector3 position)
    {
        if (dropPrefab != null)
        {
            GameObject drop = Instantiate(dropPrefab, position + Vector3.up * 0.5f, Quaternion.identity);
            drop.SetActive(true);
        }
    }
}
