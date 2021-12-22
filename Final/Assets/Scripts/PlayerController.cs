using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class PlayerController : MonoBehaviourPunCallbacks
{
	public CharacterController controller;


	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;
	public GameObject cameraParent;

	public int maxHealth;
	private int currentHealth; 
	private Transform UiHealthBar; 

	public float speed = 12f;
	public float gravity = -9.81f; 
	public float jumpHeight = 3f; 

	Vector3 velocity; 
	bool isGrounded; 

	private Manager manager; 

	void Start () {

		manager = GameObject.Find("Manager").GetComponent<Manager>(); 
		currentHealth = maxHealth; 

		cameraParent.SetActive(photonView.IsMine);
		if(!photonView.IsMine) gameObject.layer = 6; 

		if(photonView.IsMine){
			UiHealthBar = GameObject.Find("HUD/Health/Bar").transform;
			RefreshHealthBar();
		}  
		
	}

	void Update () 
	{
		if(!photonView.IsMine) return;
		 
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f; 
		}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical"); 

		Vector3 move = transform.right * x + transform.forward * z;

		controller.Move(move * speed * Time.deltaTime);

		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		velocity.y += gravity * Time. deltaTime; 

		controller.Move(velocity * Time.deltaTime);
	}

	void RefreshHealthBar (){
		float t_health_ratio = (float)currentHealth / (float)maxHealth;
		UiHealthBar.localScale = new Vector3(t_health_ratio, 1, 1);   
	}

	#region Public Methods

	public void TakeDamage (int damage){
		if(photonView.IsMine){
			currentHealth -= damage; 
			RefreshHealthBar(); 

			if(currentHealth <= 0){
				manager.Spawn();
				PhotonNetwork.Destroy(gameObject); 
			} 
		}
	}



	#endregion 

}
