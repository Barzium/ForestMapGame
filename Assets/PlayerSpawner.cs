
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    static PlayerSpawner _instance;
    public static PlayerSpawner GetInstance => _instance;
  

    [SerializeField] GameObject player;

    [SerializeField] Vector3 spawnPoint = new Vector3(61.8f, 1.75f, 66f);

    [SerializeField] RedEyesManager redEyes;
    [SerializeField] PlayerController plyrctrl;
    [SerializeField] TimeManager timeManager;



    public void InitAll() {


        Instantiate(player, spawnPoint, Quaternion.AngleAxis(200,Vector3.up));
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
