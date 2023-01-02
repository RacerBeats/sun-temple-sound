using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple{


	public class CharController_Motor : MonoBehaviour {
		
		public float speed = 10.0f;
		public float sensitivity = 60.0f;
		CharacterController character;
		public GameObject cam;
		float moveFB, moveLR;	
		float rotHorizontal, rotVertical;
		public bool webGLRightClickRotation = true;
		float gravity = -9.8f;

        //FOOTSTEP PLAYER
        float timer = 0.0f;
        [SerializeField]
        float footstepSpeed = 0.66f;


		private enum CURRENT_TERRAIN { STONE, DIRT, GRASS, WOOD, WATER };

        [SerializeField]
        private CURRENT_TERRAIN currentTerrain;

        private FMOD.Studio.EventInstance foosteps;

        private void DetermineTerrain()
        {
            RaycastHit[] hit;

            hit = Physics.RaycastAll(transform.position, Vector3.down, 4.0f);

            foreach (RaycastHit rayhit in hit)
            {
                if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Stone"))
                {
                    currentTerrain = CURRENT_TERRAIN.STONE;
                    break;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Dirt"))
                {
                    currentTerrain = CURRENT_TERRAIN.DIRT;
                    break;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
                {
                    currentTerrain = CURRENT_TERRAIN.GRASS;
                    break;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
                {
                    currentTerrain = CURRENT_TERRAIN.WOOD;
                    break;
                }
                else if (rayhit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    currentTerrain = CURRENT_TERRAIN.WATER;
                    break;
                }
            }
        }

        public void SelectAndPlayFootstep()
        {
            switch (currentTerrain)
            {
                case CURRENT_TERRAIN.STONE:
                    PlayFootstep(0);
                    break;

                case CURRENT_TERRAIN.DIRT:
                    PlayFootstep(1);
                    break;

                case CURRENT_TERRAIN.GRASS:
                    PlayFootstep(2);
                    break;

                case CURRENT_TERRAIN.WOOD:
                    PlayFootstep(3);
                    break;

                case CURRENT_TERRAIN.WATER:
                    PlayFootstep(4);
                    break;

                default:
                    PlayFootstep(0);
                    break;
            }
        }

        private bool isWalking()
        {
            if (Input.GetKey(KeyCode.W))
            {
                //Debug.Log("walking");
                return true;
                
            }
            else if (Input.GetKey(KeyCode.A))
            {
                //Debug.Log("walking");
                return true;
                
            }
            else if (Input.GetKey(KeyCode.S))
            {
                //Debug.Log("walking");
                return true;
                
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //Debug.Log("walking");
                return true;
                
            }
            else return false;
            
        }

        private void PlayFootstep(int terrain)
        {
            foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/Player/walking");
            foosteps.setParameterByName("Terrain", terrain);
            foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
            foosteps.start();
            foosteps.release();
        }

        void Start(){

			character = GetComponent<CharacterController> ();

			webGLRightClickRotation = false;

			if (Application.platform == RuntimePlatform.WebGLPlayer) {
				webGLRightClickRotation = true;
				sensitivity = sensitivity * 1.5f;
			}


		}

		void Update()
		{
            timer += Time.deltaTime;
            DetermineTerrain();
            //Debug.Log(timer);
            if (isWalking())
            {
                
                if (timer >= footstepSpeed)
                {
                    SelectAndPlayFootstep();
                    timer = 0.0f;
                }
                
            }
            
            
        }



		void FixedUpdate(){
			moveFB = Input.GetAxis ("Horizontal") * speed;
			moveLR = Input.GetAxis ("Vertical") * speed;

			rotHorizontal = Input.GetAxisRaw ("Mouse X") * sensitivity;
			rotVertical = Input.GetAxisRaw ("Mouse Y") * sensitivity;


			Vector3 movement = new Vector3 (moveFB, gravity, moveLR);


			if (webGLRightClickRotation) {
				if (Input.GetKey (KeyCode.Mouse0)) {
					CameraRotation (cam, rotHorizontal, rotVertical);
				}
			} else if (!webGLRightClickRotation) {
				CameraRotation (cam, rotHorizontal, rotVertical);
			}

			movement = transform.rotation * movement;
			character.Move (movement * Time.fixedDeltaTime);
		}






        void CameraRotation(GameObject cam, float rotHorizontal, float rotVertical){	

			transform.Rotate (0, rotHorizontal * Time.fixedDeltaTime, 0);
			cam.transform.Rotate (-rotVertical * Time.fixedDeltaTime, 0, 0);



			if (Mathf.Abs (cam.transform.localRotation.x) > 0.7) {

				float clamped = 0.7f * Mathf.Sign (cam.transform.localRotation.x); 

				Quaternion adjustedRotation = new Quaternion (clamped, cam.transform.localRotation.y, cam.transform.localRotation.z, cam.transform.localRotation.w);
				cam.transform.localRotation = adjustedRotation;
			}


		}




	}



}