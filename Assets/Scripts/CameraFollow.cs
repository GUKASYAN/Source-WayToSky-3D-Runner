using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public Transform target;
	public float angle = 15; 
	public float distance = -4; 
	public float height = 2.5f; 
	private Vector3 posCamera;
	private Vector3 angleCam;
	public static CameraFollow instace;      
	void Start(){
		instace = this;	
	}                                         
	void LateUpdate(){
		if(target != null){
			if(target.position.z >= 0){               
					posCamera.x = Mathf.Lerp(posCamera.x, target.position.x, 5 * Time.deltaTime);
					posCamera.y = Mathf.Lerp(posCamera.y, target.position.y + height, 5 * Time.deltaTime);
					posCamera.z = Mathf.Lerp(posCamera.z, target.position.z + distance, GameParameters.gameParameters.speed); //* Time.deltaTime);
					transform.position = posCamera;
					angleCam.x = angle;
					angleCam.y = Mathf.Lerp(angleCam.y, 0, 1 * Time.deltaTime);
					angleCam.z = transform.eulerAngles.z;
					transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, angleCam, 1 * Time.deltaTime);
				
			}else{                     
					Vector3 dummy = Vector3.zero;
					posCamera.x = Mathf.Lerp(posCamera.x, 0, 5 * Time.deltaTime);
					posCamera.y = Mathf.Lerp(posCamera.y, dummy.y + height, 5 * Time.deltaTime);
					posCamera.z = dummy.z + distance;
					transform.position = posCamera;
					angleCam.x = angle;
					angleCam.y = transform.eulerAngles.y;
					angleCam.z = transform.eulerAngles.z;
					transform.eulerAngles = angleCam;          
			}
		}
	}
}
