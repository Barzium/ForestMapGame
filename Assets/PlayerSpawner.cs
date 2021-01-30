
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    static PlayerSpawner _instance;
    public static PlayerSpawner GetInstance => _instance;
  

    [SerializeField] GameObject player;

    [SerializeField] Vector3 spawnPoint = new Vector3(59.3f,3.75f,61f);

   [SerializeField] RedEyesManager redEyes;
    [SerializeField] PlayerController plyrctrl;
    [SerializeField] TimeManager timeManager;



    public void InitAll() {


        Instantiate(player, spawnPoint, Quaternion.identity);
       plyrctrl = PlayerController.GetInstance;
        plyrctrl.RestartPosition(spawnPoint);
        timeManager= TimeManager._instance;
       
        timeManager.RestartSetting();
        redEyes.AssignPlayerTransform(player.transform);
        redEyes.RestartEyes();
    }

    private void Awake()
    {
        _instance = this;
    }
}
