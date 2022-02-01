using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using Mirror;

public class PlayerManager : MonoBehaviour
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject PlayerModel;

    private NetworkIdentity identity;

    private CharacterController _controller;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        identity = GetComponent<NetworkIdentity>();

        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (identity.isLocalPlayer)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);

        //Camera Work stuff
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (identity.isLocalPlayer)
            {
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    #region Movement
    public float Speed = 4.0f;
    public float smooth = 5f;

    void Update()
    {
        if (identity.isLocalPlayer)
        {
            //Basic Movement 
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _controller.Move(move * Time.deltaTime * Speed);

            //Player look where they move
            if (move != Vector3.zero)
            {
                //We use model because it has no rotation where as player model does
                //We flip the movement  
                PlayerModel.transform.parent.gameObject.transform.forward = move * -1;
            }

            if (!_controller.isGrounded)
            {
                _controller.Move(new Vector3(0, -10, 0) * Time.deltaTime);
            }
            //Animation control

            //Getting animation to play when character moving
            if (move == new Vector3(0, 0, 0))
            {
                PlayerModel.GetComponent<Animator>().SetBool("Moving", false);
            }
            if (move != new Vector3(0, 0, 0))
            {
                PlayerModel.GetComponent<Animator>().SetBool("Moving", true);
            }
        }
    }
    #endregion
}
