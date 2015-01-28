using UnityEngine;
using System.Collections;

public class GameParameters : MonoBehaviour {
    public float speed; 
    [HideInInspector]
    public int Score;   
    public int ScoreToWin;
    public static GameParameters gameParameters;
    public UILabel LabelScoreCount;
    public UILabel LabelSpeedCount;
    public Transform DeadPanel;
    public Transform WinPanel;
    private void Start()
    {                                
        gameParameters = this;
        speed = 5;
        ScoreToWin = 0;    
        DontDestroyOnLoad(this);
    }                     
    private void Update()
    {
        if (LabelSpeedCount != null) LabelSpeedCount.text = "" + speed;
    }           
    public void OpenDeadPanel()
    {
        if (!WinPanel.gameObject.activeInHierarchy)
        {
            SoundManager.soundManager.PlayDeadSound();
            DeadPanel.gameObject.SetActive(true);
        }
    }
    public void CloseDeadPanel()
    {
        DeadPanel.gameObject.SetActive(false);
    }
    public void OpenWinPanel()
    {
        WinPanel.gameObject.SetActive(true);
    }
    public void CloseWinPanel()
    {
        WinPanel.gameObject.SetActive(false);
    }             
    public void RestartButton()
    {
        StopAllCoroutines();
        GameParameters.gameParameters.CloseDeadPanel();
        Application.LoadLevel("level1");
    }
}
