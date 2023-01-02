using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SunTemple
{
   

    public class Door : MonoBehaviour
    {
		public bool IsLocked = false;
        public bool DoorClosed = true;
        public float OpenRotationAmount = 90;
        public float RotationSpeed = 1f;
        public float MaxDistance = 3.0f;
		public string playerTag = "Player";
		private Collider DoorCollider;

		private GameObject Player;
		private Camera Cam;
		private CursorManager cursor;

        Vector3 StartRotation;
        float StartAngle = 0;
        float EndAngle = 0;
        float LerpTime = 1f;
        float CurrentLerpTime = 0;
        bool Rotating;

		//ADD SFX
		[Header ("Audio")]
		[SerializeField] private bool pauseInteraction = false;
		[SerializeField] private AudioSource doorOpenAudioSource = null;
		[SerializeField] private float openDelay = 0;
		[Space(10)]
        [SerializeField] private AudioSource doorCloseAudioSource = null;
        [SerializeField] private float closeDelay = 0;
		FMOD.Studio.EventInstance Doors;
		FMOD.Studio.PARAMETER_ID Open_Door, CLose_Door;


        private bool scriptIsEnabled = true;



        void Start(){
            StartRotation = transform.localEulerAngles ;
			DoorCollider = GetComponent<BoxCollider> ();

			doorOpenAudioSource = GetComponent<AudioSource>();
            doorCloseAudioSource = GetComponent<AudioSource>();

            if (!DoorCollider) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + gameObject.name + "door has no collider", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Player = GameObject.FindGameObjectWithTag (playerTag);

			if (!Player) {
				Debug.LogWarning (this.GetType ().Name + ".cs on " + this.name + ", No object tagged with " + playerTag + " found in Scene", gameObject);
				scriptIsEnabled = false;
				return;
			}

			Cam = Camera.main;
			if (!Cam) {
				Debug.LogWarning (this.GetType ().Name + ", No objects tagged with MainCamera in Scene", gameObject);
				scriptIsEnabled = false;
			}
		
			cursor = CursorManager.instance;

			if (cursor != null) {
				cursor.SetCursorToDefault ();
			}

			//cache handle to door parameter for use in Update(), apparently its more efficient
			FMOD.Studio.EventDescription DoorPlayerDescriptor;
			Doors.getDescription(out DoorPlayerDescriptor);
            FMOD.Studio.PARAMETER_DESCRIPTION DoorParameterDescription;
			DoorPlayerDescriptor.getParameterDescriptionByName("Open_Door", out DoorParameterDescription);
			Open_Door = DoorParameterDescription.id;

            DoorPlayerDescriptor.getParameterDescriptionByName("Close_Door", out DoorParameterDescription);
            CLose_Door = DoorParameterDescription.id;
        }



		void Update()
		{
			if (scriptIsEnabled) {
				if (Rotating) {
					Rotate ();
				}

				if (Input.GetKeyDown (KeyCode.Mouse0)) {
					TryToOpen ();
				}


				if (cursor != null) {
					CursorHint ();
				}
			}

			Doors.setParameterByID(Open_Door, (float)00.00, false);
		} 




		void TryToOpen(){
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance){	

				Ray ray = Cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;
											
				if (DoorCollider.Raycast(ray, out hit, MaxDistance)){					
					if (IsLocked == false){
						Activate ();
					}
				}
			}
		}



		void CursorHint(){
			if (Mathf.Abs(Vector3.Distance(transform.position, Player.transform.position)) <= MaxDistance){	
				Ray ray = Cam.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				RaycastHit hit;

				if (DoorCollider.Raycast (ray, out hit, MaxDistance)) {
					if (IsLocked == false) {
						cursor.SetCursorToDoor ();
					} else if (IsLocked == true) {
						cursor.SetCursorToLocked ();
					}					
				} else {
					cursor.SetCursorToDefault ();
				}
			}
		}




        public void Activate()
        {
            if (DoorClosed)
                Open();
            else
                Close();
        }



       



        void Rotate()
        {
            CurrentLerpTime += Time.deltaTime * RotationSpeed;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }

            float _Perc = CurrentLerpTime / LerpTime;

            float _Angle = CircularLerp.Clerp(StartAngle, EndAngle, _Perc);
            transform.localEulerAngles = new Vector3(transform.eulerAngles.x, _Angle, transform.eulerAngles.z);

			if (CurrentLerpTime == LerpTime) {
				Rotating = false;
				DoorCollider.enabled = true;
			}
              
           
        }



        void Open()
        {
			DoorCollider.enabled = false;
            DoorClosed = false;
            StartAngle = transform.localEulerAngles.y;
            EndAngle =  StartRotation.y + OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
			doorOpenAudioSource.PlayDelayed(openDelay);
        }



        void Close()
        {
			DoorCollider.enabled = false;
            DoorClosed = true;
            StartAngle = transform.localEulerAngles.y;
            EndAngle = transform.localEulerAngles.y - OpenRotationAmount;
            CurrentLerpTime = 0;
            Rotating = true;
            doorCloseAudioSource.PlayDelayed(closeDelay);

        }

    }
}