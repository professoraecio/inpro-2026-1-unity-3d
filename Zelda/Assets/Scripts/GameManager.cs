using UnityEngine;

public enum enemyState
{
    IDLE, ALERT, EXPLORE,
    PATROL, FOLLOW, FURY
}

public class GameManager : MonoBehaviour
{
    [Header("Slime IA")]
    public Transform[] slimeWayPoints;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
