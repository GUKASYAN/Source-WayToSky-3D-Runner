using System.Collections.Generic;
using UnityEngine;
using System.Collections;
[System.Serializable]
public class WaySystem : MonoBehaviour {
    public GameObject spawnObj_Pref;
    public GameObject ground_Pref;
    public List<GameObject> item_Pref = new List<GameObject>();  

    Vector3[] SpawnLeft;
    Vector3[] SpawnRight;
    Vector3[] SpawnMiddle;

    private int amountGroundSpawn = 4;
    private float nextPosGround = 40;
    public static WaySystem instance;
    private List<GameObject> ground_Obj = new List<GameObject>();
    
    private List<SequenceGround> queneGround = new List<SequenceGround>();
    private Vector3 posGroundLast;

    private Vector3 posStart = new Vector3(-100, -100, -100);
    private CheckSpawn checkSpawn;
    private GameObject spawnObj_Obj;
    private int randomItem;

    [System.Serializable]
    public class SequenceGround
    {                              
        public GameObject groundObj;
    }     
    void Start()
    {
        instance = this;
        StartCoroutine(InitializeGround());  
    }
    IEnumerator InitializeGround()
    {
        int i = 0;  
        while (i < amountGroundSpawn)
        {
            GameObject go = (GameObject)Instantiate(ground_Pref, posStart, Quaternion.identity);
            go.name = "Ground[" + i + "]";
            ground_Obj.Add(go);          
            SequenceGround qGround = new SequenceGround();
            qGround.groundObj = ground_Obj[i];                  
            queneGround.Add(qGround);
            i++;   
            int startZ = -18;
            SpawnLeft = new Vector3[18];
            SpawnRight = new Vector3[18];
            SpawnMiddle = new Vector3[18];      
            for (int j = 0; j < SpawnLeft.Length; j++)
            {
                SpawnLeft[j] = new Vector3(-1.6f, 0, startZ);
                SpawnRight[j] = new Vector3(1.6f, 0, startZ);
                SpawnMiddle[j] = new Vector3(0, 0, startZ);
                GameObject item;
                if (Random.Range(0, 100) > 50)
                {
                    randomItem = Random.Range(0, item_Pref.Count);
                  
                    if (checkRisk(randomItem))
                    {
                        item = (GameObject)
                            Instantiate(item_Pref[randomItem], SpawnLeft[j], item_Pref[randomItem].transform.rotation);
                        item.name = "Item[" + j + "]-TYPE" + randomItem + "-Left";
                        item.transform.parent = go.transform.FindChild("WayItems");
                        item.transform.localPosition = SpawnLeft[j];
                    }
                }
                if (Random.Range(0, 100) > 50)
                {
                    randomItem = Random.Range(0, item_Pref.Count);
                    if (checkRisk(randomItem))
                    {
                        item =
                            (GameObject)
                                Instantiate(item_Pref[randomItem], SpawnRight[j],
                                    item_Pref[randomItem].transform.rotation);
                        item.name = "Item[" + j + "]-TYPE" + randomItem + "-Right";
                        item.transform.parent = go.transform.FindChild("WayItems");
                        item.transform.localPosition = SpawnRight[j];
                    }
                }
                if (Random.Range(0, 100) > 50)
                {       
                    randomItem = Random.Range(0, item_Pref.Count);
                    if (checkRisk(randomItem))
                    {
                        item =
                            (GameObject)
                                Instantiate(item_Pref[randomItem], SpawnMiddle[j],
                                    item_Pref[randomItem].transform.rotation);
                        item.name = "Item[" + j + "]-TYPE" + randomItem + "-Middle";
                        item.transform.parent = go.transform.FindChild("WayItems");
                        item.transform.localPosition = SpawnMiddle[j];
                    }
                }                    
                startZ += 2;        
            }                   
            yield return 0;
        }
        spawnObj_Obj = (GameObject)Instantiate(spawnObj_Pref, posStart, Quaternion.identity);
        checkSpawn = spawnObj_Obj.GetComponentInChildren<CheckSpawn>();  
        checkSpawn.headParent = spawnObj_Obj;       
        StartCoroutine(SetStarter());
        yield return 0;
    }

    private bool checkRisk(int type)
    {
        if (type == 0)
        {
            GameParameters.gameParameters.ScoreToWin++; 
            return true;
            
        }
        if (type == 1) return Random.Range(0, 100) > 70; ;
        
        return Random.Range(0, 100) > 85;
    }

    IEnumerator SetStarter()
    {
        Vector3 pos = Vector3.zero;
        pos.z = nextPosGround;
        int i = 0;
        while (i < ground_Obj.Count)
        {             
            queneGround[i].groundObj.transform.position = pos;
            pos.z += nextPosGround;
            i++;
            yield return 0;
        }
         
        posGroundLast = pos;
        pos = Vector3.zero;
        pos.z += nextPosGround * 2;
        checkSpawn.headParent.transform.position = pos;
        yield return new WaitForSeconds(1);
        checkSpawn.isCollision = false;
        StartCoroutine(WaitCheckGround());
        yield return 0;
    }
    IEnumerator WaitCheckGround()
    {                          
         while (checkSpawn.isCollision == false)
        {
            yield return 0;
        }                             
        checkSpawn.isCollision = false;  
        StartCoroutine(SetPosGround());  
    }                                   
    IEnumerator SetPosGround()
    {                                    
        Vector3 pos = Vector3.zero;
        pos.z = checkSpawn.headParent.transform.position.z;  
        pos.z += nextPosGround;
        checkSpawn.headParent.transform.position = pos;
        checkSpawn.nextPos = checkSpawn.headParent.transform.position.z;  
        StartCoroutine(AddGround());
        yield return 0;                                            
    }
    IEnumerator AddGround()
    {                                                                 
        SequenceGround qGround = queneGround[0];
        queneGround.RemoveRange(0, 1); 
        qGround.groundObj.transform.position = posGroundLast;
        posGroundLast.z += nextPosGround;
        queneGround.Add(qGround);
        StartCoroutine(WaitCheckGround());
        yield return 0;
    }     
}
