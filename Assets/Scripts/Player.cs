using System.Collections;
using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class Player : MonoBehaviour
{

    private void Update()
    {
        if (this.transform.position.y < 0)
        {
            GameParameters.gameParameters.OpenDeadPanel();
        }
    }   
    private void OnTriggerEnter(Collider col)
    {   
        if (col.gameObject.name.Contains("TYPE0"))
        {
            Destroy(col.gameObject);
            GameParameters.gameParameters.Score++;
            GameParameters.gameParameters.LabelScoreCount.text =""+GameParameters.gameParameters.Score+" of "+ GameParameters.gameParameters.ScoreToWin;
            if (GameParameters.gameParameters.Score == GameParameters.gameParameters.ScoreToWin)
            {
                GameParameters.gameParameters.speed = 500;
                GameParameters.gameParameters.OpenWinPanel();;
            }
        }
        if (col.gameObject.name.Contains("TYPE1"))
        {  
            this.transform.FindChild("Smoke Trail").gameObject.SetActive(true);
            GameParameters.gameParameters.speed = 0;
            GameParameters.gameParameters.OpenDeadPanel();
         
        }    
    }          
}
