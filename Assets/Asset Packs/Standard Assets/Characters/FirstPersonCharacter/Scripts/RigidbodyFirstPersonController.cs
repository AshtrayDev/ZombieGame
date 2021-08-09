using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class RigidbodyFirstPersonController : MonoBehaviour
	{
		/*

		Attempts on making an FPS controller with a rigidbody.
		Requires a little bit of set up to be used, such as a "Crouch" input, Rigidbody and Collider components and a child Camera.
		The camera isn't actually used in the code right now and should have it's own script to turn itself and the player.
		

		Perfect solution would meet the following requirements:
			- Players can move around with a fixed movement speed limit.
			- They can still be accelerated infinitely by external forces/other movement options.
			- Air acceleration is slower, but has the same limit.
			- In the air, movement is not slowed down by anything.
			- On the ground, movement is quickly stopped when not moving.
			- Movement is quick and responsive.
			- Players can interact with physics objects.
			- Players will slide down slopes and be pushed by the environment.


		Approach 1:
			- Movement can speed up normally, but won't accelerate the body over a certain limit on it's own.
			- Done with dot product of velocity vector and the direction the player is trying to move to.

			Results achieved:
				- Apparently I made bunny hopping.
				- Kinda responsive and fun, though a little too fun in the wrong way, completely broken.


		Approach 2:
			- Movement from the character is made with a completely separate velocity vector, we literally just move the player manually every tick.
			- Might go through things, we'll see about that.

			Results achieved:
				- The rigidbody goes through objects in some cases, but doesn't in others.
				- Decent, allows the player to push objects, which is amazing.
				- The player does enter other objects, which makes some smaller bugs happen.
				- Some games could get away with a controller like this and some adjustments.
				- Biggest problem is interactions with colliders without rigidbodies, they just don't move at all and glitch a little.

		
		Approach 3:
			- Approach 1, but we also slow down the player before accelerating him, not sure how much it should be.

			Results achieved:
				- No, it should be obvious it wouldn't work.
				- Deleted the code out of embarassement for thinking it could somehow work.
				- Oh god, it was so bad, who would think it could work.

		
		Approach 4:
			- Speed limit the velocity vector of the rigidbody except in the Y axis.
			- If the player should accelerate over that amount, the speed limit should change first.

			Results achieved:
				- Obviously, it worked.
				- Obviously, it sucks.
				- Imagine the player is launched by something, we either need to lock the player movement until it ends or he can just 180 and cancel all velocity.
				- Obviously, it doesn't fill all requirements. In fact, it doesn't fill even the second one, which is one of the most important.

		
		Approach 5:
			- Each update, create a vector to be the target velocity and adjust based on input.
			- If on ground, slow down/speed up to the target velocity at the end of the update.
			- If mid air, speed up but not slow down.
			- This is done in every axis separately.
			- Separate function for movement, so it works better.

			Results achieved:
				- Player can move the expected way.
				- Slow down on the ground is dictated by friction, which is good in general, but also means the player almost never really reaches the movement speed set.
				- Almost perfect, didn't see any problem yet.
				- In the air the player can still accelerate similarly to approach 1, but to a completely unnoticeable level.


		The code for approaches 1-4 has been removed as the adequate solution has been found in approach 5, but I'll keep the descriptions there.
		

	*/

		[Header("Target Objects")]
		[Tooltip("The Rigidbody the controller will use. NOT required if the Rigidbody is set up on the same gameobject.")]
		public Rigidbody rb;
		[Tooltip("The Camera the controller will use. NOT required if the camera is set up as a child gameobject.")]
		public Camera cam;

		[Header("Movement")]
		[Tooltip("The desired magnitude of the velocity vector. It's not perfect so it won't get to that amount most of the time.")]
		public float currentMoveSpeed;
		public float sprintSpeed = 8f;
		public float originalSpeed = 4f;
		[Tooltip("The vertical speed of the jump. The script won't add force normally, it'll instead set velocity in the Y axis to that amount.")]
		public float jumpSpeed = 5.4f;
		[Tooltip("How fast the character accelerates when moving on the ground.")]
		public float groundAcceleration = 32f;
		[Tooltip("How fast the character accelerates when moving mid air.")]
		public float airAcceleration = 16f;

		[Header("Crouching")]
		[Tooltip("How fast the character crouches.")]
		public float crouchSpeed = 5f;

		// This value controls the Y axis scale of the character.
		private float heightScale = 1f;

		// True if the character should be crouching, used for setting heightScale and reducing movement speed.
		private bool crouch = false;

		[Header("Air Jumping")]
		[Tooltip("Maximum amount of jumps before hitting the ground.")]
		public int maxAirJumps = 1;
		[Tooltip("Exponential increase in jump force. Air jump 1 would be jumpSpeed * this^1, 2 would be jumpSpeed * this^2. Values below 1 make jumps exponentially weaker.")]
		public float airJumpSpeedMultiplier = 1.35f;

		// Current air jumps remaining, resets to maxAirJumps when hitting the ground. It should work even if it's higher than maxAirJumps, but bugs are to be expected.
		private int airJumpsLeft = 0;

		[Header("Ground detection")]
		[Tooltip("Public only for debugging purposes. True if on the ground.")]
		public bool isGrounded = false;
		[Tooltip("The maximum angle a slope can be while still being considered steppable ground.")]
		public float slopeLimit = 45f;

		[Header("Input")]
		[Tooltip("Public only for debugging purposes. Represents the movement the player is trying to make at the moment.")]
		public Vector3 movementDirection;
		[Tooltip("Public only for debugging purposes. True if the player pressed the jump key since the last FixedUpdate.")]
		public bool jump = false;
		[Tooltip("Public only for debugging purposes. True if the player pressed the dash key since the last FixedUpdate.")]
		public bool dash = false;

		public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
		public RotationAxes axes = RotationAxes.MouseXAndY;
		public float sensitivityX = 15F;
		public float sensitivityY = 15F;
		public float minimumX = -360F;
		public float maximumX = 360F;
		public float minimumY = -60F;
		public float maximumY = 60F;
		float rotationX = 0F;
		float rotationY = 0F;
		private List<float> rotArrayX = new List<float>();
		float rotAverageX = 0F;
		private List<float> rotArrayY = new List<float>();
		float rotAverageY = 0F;
		public float frameCounter = 20;
		Quaternion originalRotation;


		void Start()
		{

			// In case these haven't been set up, we look for them on the appropriate places.
			if (rb == null)
				rb = GetComponent<Rigidbody>();
			if (cam == null)
				cam = GetComponentInChildren<Camera>();

			if (rb)
				rb.freezeRotation = true;
			originalRotation = transform.localRotation;
			currentMoveSpeed = originalSpeed;

		}

		void Update()
		{

			// Stolen code, sets jump to true if the jump key was pressed.
			jump = jump || Input.GetButtonDown("Jump");

			// Set movementDirection based on input and the Rigidbody transform.
			movementDirection = (rb.transform.right * Input.GetAxisRaw("Horizontal") + rb.transform.forward * Input.GetAxisRaw("Vertical")).normalized;

			// Crouching stuff, it doesn't detect if there's enough space to stand up right now.
			if (Input.GetButtonDown("Crouch"))
			{
				crouch = true;
				heightScale = .5f;

			}

			if (Input.GetButtonUp("Crouch"))
			{
				crouch = false;
				heightScale = 1f;

			}

			if (Input.GetButton("Dash"))
			{
				dash = true;
				currentMoveSpeed = sprintSpeed;
			}

			if (Input.GetButtonUp("Dash"))
			{
				dash = false;
				currentMoveSpeed = originalSpeed;
			}


			if (axes == RotationAxes.MouseXAndY)
			{
				//Resets the average rotation
				rotAverageY = 0f;
				rotAverageX = 0f;

				//Gets rotational input from the mouse
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;

				//Adds the rotation values to their relative array
				rotArrayY.Add(rotationY);
				rotArrayX.Add(rotationX);

				//If the arrays length is bigger or equal to the value of frameCounter remove the first value in the array
				if (rotArrayY.Count >= frameCounter)
				{
					rotArrayY.RemoveAt(0);
				}
				if (rotArrayX.Count >= frameCounter)
				{
					rotArrayX.RemoveAt(0);
				}

				//Adding up all the rotational input values from each array
				for (int j = 0; j < rotArrayY.Count; j++)
				{
					rotAverageY += rotArrayY[j];
				}
				for (int i = 0; i < rotArrayX.Count; i++)
				{
					rotAverageX += rotArrayX[i];
				}

				//Standard maths to find the average
				rotAverageY /= rotArrayY.Count;
				rotAverageX /= rotArrayX.Count;

				//Clamp the rotation average to be within a specific value range
				rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
				rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

				//Get the rotation you will be at next as a Quaternion
				Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

				//Rotate
				transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			}
			else if (axes == RotationAxes.MouseX)
			{
				rotAverageX = 0f;
				rotationX += Input.GetAxis("Mouse X") * sensitivityX;
				rotArrayX.Add(rotationX);
				if (rotArrayX.Count >= frameCounter)
				{
					rotArrayX.RemoveAt(0);
				}
				for (int i = 0; i < rotArrayX.Count; i++)
				{
					rotAverageX += rotArrayX[i];
				}
				rotAverageX /= rotArrayX.Count;
				rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
			}
			else
			{
				rotAverageY = 0f;
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
				rotArrayY.Add(rotationY);
				if (rotArrayY.Count >= frameCounter)
				{
					rotArrayY.RemoveAt(0);
				}
				for (int j = 0; j < rotArrayY.Count; j++)
				{
					rotAverageY += rotArrayY[j];
				}
				rotAverageY /= rotArrayY.Count;
				rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
				Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
				transform.localRotation = originalRotation * yQuaternion;
			}
		}

		void FixedUpdate()
		{

			// Reset air jumps if the player touches the ground.
			if (isGrounded)
				airJumpsLeft = maxAirJumps;

			// Make gravity 1.6G.
			rb.AddForce(Physics.gravity * .6f * Time.deltaTime, ForceMode.VelocityChange);


			// The code below makes tea bagging decently satisfying.
			float actualMoveSpeed;

			if (transform.localScale.y != heightScale)
			{
				Vector3 newScale = transform.localScale;

				// heightScale is the target scale in the Y axis, if rb.transform.scale.y is different than that, it'll change at crouchSpeed * Time.deltaTime per update.
				if (newScale.y < heightScale - crouchSpeed * Time.deltaTime)
				{
					newScale.y += crouchSpeed * Time.deltaTime;

					if (isGrounded)
						transform.Translate(transform.up * crouchSpeed * Time.deltaTime / 2f);

				}
				else if (newScale.y > heightScale + crouchSpeed * Time.deltaTime)
				{
					newScale.y -= crouchSpeed * Time.deltaTime;

					if (isGrounded)
						transform.Translate(transform.up * -crouchSpeed * Time.deltaTime / 2f);

				}
				else
					newScale.y = heightScale;

				transform.localScale = newScale;

			}

			// If crouching and on the ground, movement speed is cut in half.
			if (crouch && isGrounded)
				actualMoveSpeed = currentMoveSpeed / 2;
			else
				actualMoveSpeed = currentMoveSpeed;

			// Saves your PC from a few calculations when the player isn't doing anything.
			if (movementDirection.magnitude != 0f)
				Move(movementDirection * actualMoveSpeed);

			// When grounded, gravity is increased to 2.6G to prevent crazy movement and make slopes harder to climb.
			if (isGrounded)
				rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.VelocityChange);

			// Code that makes both jumping and air jumping work, they both set velocity instead of adding. This means an air jump can reset a big fall.
			if (isGrounded && jump)
				rb.AddForce(rb.transform.up * (-rb.velocity.y + jumpSpeed), ForceMode.VelocityChange);
			else if (jump && airJumpsLeft > 0)
			{
				rb.AddForce(rb.transform.up * (-rb.velocity.y + jumpSpeed * Mathf.Pow(airJumpSpeedMultiplier, 1f + maxAirJumps - airJumpsLeft)), ForceMode.VelocityChange);
				airJumpsLeft--;

			}

			// Resets input to false so it requires another key press.
			jump = false;
			dash = false;

		}

		private void Move(Vector3 targetVelocity)
		{

			// Actually useless, but allows one lining some operations.
			Vector3 rbVelocity = rb.velocity;

			// Set the acceleration that should be used on the X axis, considering the direction the player is moving and if he's on the ground or mid air.
			float acceleration = (isGrounded ? groundAcceleration : airAcceleration) * Mathf.Abs(targetVelocity.normalized.x) * Time.deltaTime;

			// This condition checks if the Rigidbody velocity needs to accelerate in the X axis, this controller never wants to slow down with movement.
			if ((targetVelocity.x < 0 && targetVelocity.x < rbVelocity.x) || (targetVelocity.x >= 0 && targetVelocity.x > rbVelocity.x))
			{

				// We accelerate if we're not there yet.
				if (rbVelocity.x - acceleration > targetVelocity.x)
					rbVelocity.x -= acceleration;
				else if (rbVelocity.x + acceleration < targetVelocity.x)
					rbVelocity.x += acceleration;
				else
					rbVelocity.x = targetVelocity.x;

			}

			// Change the acceleration that should be used to deal with the Z axis now.
			acceleration = (isGrounded ? groundAcceleration : airAcceleration) * Mathf.Abs(targetVelocity.normalized.z) * Time.deltaTime;

			// Same as for the X axis, just for the Z.aaaa
			if ((targetVelocity.z < 0 && targetVelocity.z < rbVelocity.z) || (targetVelocity.z >= 0 && targetVelocity.z > rbVelocity.z))
			{
				if (rbVelocity.z - acceleration > targetVelocity.z)
					rbVelocity.z -= acceleration;
				else if (rbVelocity.z + acceleration < targetVelocity.z)
					rbVelocity.z += acceleration;

			}

			// Set the Rigidbody velocity to the new velocity.
			rb.velocity = rbVelocity;

		}


		void OnCollisionStay(Collision collisionInfo)
		{
			isGrounded = false;

			// Stolen code from documentation, go through each contact in a continuous collision.
			foreach (ContactPoint contact in collisionInfo.contacts)
			{

				// Draw a ray to visualize it and debug weird interactions.
				Debug.DrawRay(contact.point, contact.normal, Color.red);

				// If the angle of a slope is smaller than slopeLimit, it's considered steppable ground.
				if (Vector3.Angle(rb.transform.up, contact.normal) <= slopeLimit)
					isGrounded = true;

			}
		}

		void OnCollisionExit()
		{

			// No matter what collision exits, all of them make isGrounded false because it's easier.
			isGrounded = false;

		}

		public static float ClampAngle(float angle, float min, float max)
		{
			angle = angle % 360;
			if ((angle >= -360F) && (angle <= 360F))
			{
				if (angle < -360F)
				{
					angle += 360F;
				}
				if (angle > 360F)
				{
					angle -= 360F;
				}
			}
			return Mathf.Clamp(angle, min, max);
		}
		public float GetActualMoveSpeed()
		{
			return rb.velocity.magnitude;
		}
	}
}