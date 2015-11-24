using UnityEngine;
using System.Collections;

public class PersonController : MonoBehaviour {

	/*
	public float walkSpeed;
	Rigidbody rb3d;
	Vector3 moveAmount = 8;
	Vector3 smoothMoveVelocity;

	void Start()
	{
		//cameraT = Camera.main.transform;
		//rb3d = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);

		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, 0.15f);
	}

	void FixedUpdate()
	{
		//rb3d.MovePosition(rb3d.position * transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveAmount) * moveAmount * Time.deltaTime);
	}
	*/

	private float moveSpeed = 15;
	private Vector3 moveDirection;
	
	
	void Update() {
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
	}
	
	void FixedUpdate() {
		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
	}

}
