﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Dungeon : MonoBehaviour 
{
	[SerializeField]
	Rigidbody2D rigidbody;
	[SerializeField]
	private float speed, constraint;

	void Update()
	{
		MoveCamera();
		Constraint();
	}

	void MoveCamera()
	{
		if((Input.GetAxisRaw("Horizontal") == 1 && this.transform.position.x < this.constraint) ||
			(Input.GetAxisRaw("Horizontal") == -1 && this.transform.position.x > -this.constraint))
			this.rigidbody.velocity = new Vector2(this.speed * Input.GetAxisRaw("Horizontal") * 60 * Time.deltaTime, this.rigidbody.velocity.y);
		else
			this.rigidbody.velocity = new Vector2(0, this.rigidbody.velocity.y);

		if((Input.GetAxisRaw("Vertical") == 1 && this.transform.position.y < this.constraint) ||
			(Input.GetAxisRaw("Vertical") == -1 && this.transform.position.y > -this.constraint))
		this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, this.speed * Input.GetAxisRaw("Vertical") * 60 * Time.deltaTime);
		else
			this.rigidbody.velocity = new Vector2(this.rigidbody.velocity.x, 0);
	}

	void Constraint()
	{
		if(this.transform.position.x > this.constraint)
			this.transform.position = new Vector3(this.constraint, this.transform.position.y, -10);
		if(this.transform.position.x < -this.constraint)
			this.transform.position = new Vector3(-this.constraint, this.transform.position.y, -10);
		if(this.transform.position.y > this.constraint)
			this.transform.position = new Vector3(this.transform.position.x, this.constraint, -10);
		if(this.transform.position.y < -this.constraint)
			this.transform.position = new Vector3(this.transform.position.x, -this.constraint, -10);
	}
}
