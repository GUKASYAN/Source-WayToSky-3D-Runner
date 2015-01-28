using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public WaySystem waySystem;    
    public GameObject playerPref;
    public Vector3 posStart;
    public CameraFollow cameraFol;       

    public static GameManager instace;

    void Start()
    {
        if (Application.isPlaying == true)
        {
            instace = this;
            GameObject go = (GameObject)Instantiate(playerPref, posStart, Quaternion.identity);
            cameraFol.target = go.transform;   
        }
    }                   
 }
