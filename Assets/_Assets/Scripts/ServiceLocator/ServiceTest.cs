using System.Collections;
using System.Collections.Generic;
using ProjectScare.ServiceLocator;
using UnityEngine;

public class ServiceTest : MonoBehaviour
{
    [SerializeField] private PlayerEntityData _testPlayerData;
    [SerializeField] private EntityData _testEntityData;
    [SerializeField] private CreatureEntityData _testCreatureData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("D");
            //ServiceLocator.Get<IServicePlayerManager>().TestFunc();
            //ServiceLocator.Get<IServiceDataManager>().TestFunc();
        }
    }
}
