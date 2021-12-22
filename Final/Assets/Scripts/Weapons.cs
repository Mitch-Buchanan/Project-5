using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 






public class Weapons : MonoBehaviourPunCallbacks
{

	public Gun[] loadout;
	public Transform weaponParent;
	public GameObject bulletHolePrefab; 
	public LayerMask canBeShot; 

	private GameObject currentWeapon;
	private int currentIndex;  


    void Update()
    {
    	if(!photonView.IsMine) return;

        if(Input.GetKeyDown(KeyCode.Alpha1)) {photonView.RPC("Equip", RpcTarget.All, 0);}

        if (currentWeapon != null)
        {
	        	if(Input.GetMouseButtonDown(0)) photonView.RPC("Shoot", RpcTarget.All);
        }
    }
    [PunRPC]
    void Equip (int p_ind)
    {

    	if(currentWeapon != null) Destroy(currentWeapon);

    	currentIndex = p_ind; 

    	GameObject t_newWeapon = Instantiate (loadout[p_ind].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
    	t_newWeapon.transform.localPosition = Vector3.zero; 
    	t_newWeapon.transform.localEulerAngles = Vector3.zero;

    	currentWeapon = t_newWeapon;
    }

    [PunRPC]
    void Shoot () 
    {
    	Debug.Log("Shooting"); 
	    	Transform spawn = transform.Find("Weapon/Pistol(Clone)/Anchor/Design/Barrel");

	    	RaycastHit hit = new RaycastHit(); 
	    	if(Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
	    	{
	    		GameObject newHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
	    		newHole.transform.LookAt(hit.point + hit.normal); 
	    		Destroy(newHole, 5f);

	    		if(photonView.IsMine){
	    			if(hit.collider.transform.gameObject.layer == 6){
	    				hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage); 
	    			}
	    		}
	    	} 
	    
    }

    [PunRPC]
    private void TakeDamage(int damage){
    	GetComponent<PlayerController>().TakeDamage(damage); 
    }
}
