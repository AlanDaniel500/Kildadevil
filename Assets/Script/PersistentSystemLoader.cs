using UnityEngine;

public class PersistentSystemLoader : MonoBehaviour
{
    public GameObject persistentSystemPrefab;

    void Awake()
    {
        if (PersistentUpgrades.Instance == null)
        {
            var sys = Instantiate(persistentSystemPrefab);
            sys.name = "System";
            DontDestroyOnLoad(sys);
        }
    }
}
