using AnimationEnums;
using Assets.Scripts.Helpers;
using CleverCamera;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : CharacterControllerBase {
    private float SpeedMultiplier { get; set; } = 0.01f;
    private PlayerMovingAnimations _playerMovingAnimationState;
    private PlayerFightAnimations _playerFightAnimationState;

    public PlayerMovingAnimations PlayerMovingAnimationState {
        get {
            return _playerMovingAnimationState;
        }
        set {
            if (_playerMovingAnimationState != value) {
                _animationStateChanged = true;
            }
            _playerMovingAnimationState = value;
        }
    }
    public PlayerFightAnimations PlayerFightAnimationState {
        get {
            return _playerFightAnimationState;
        }
        set {
            _animationStateChanged = true;
            _playerFightAnimationState = value;
        }
    }
    private CameraController cameraController;
    // Start is called before the first frame update
    protected override Dictionary<int, AnimationClip> InitAnimationsDictionary() {
        var dictionary = AnimationHelper.GetAnimationsDictionary(typeof(PlayerMovingAnimations), AnimationClips);
        return dictionary;
    }

    protected override void Start() {
        base.Start();
        MovementAnimationDictionary = InitAnimationsDictionary();
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }
    #region MovementControll
    void ApplyPlayerRotation() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Vector3 newpoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(newpoint);
        }
    }
    PlayerMovingAnimations GetPlayerMovementAnimationState() {
        PlayerMovingAnimations playerMovingAnimations = PlayerMovingAnimations.Idle;
        var verticalAxis = Input.GetAxis("Vertical");
        var horizontalAxis = Input.GetAxis("Horizontal");
        var cameraAngle = cameraController.CameraRotationAngle;
        var movementVector = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        var rotatedVector = Quaternion.AngleAxis(cameraAngle, Vector3.up) * movementVector;
        playerMovingAnimations = (PlayerMovingAnimations)Enum.ToObject(typeof(PlayerMovingAnimations),GetMovementDirection(rotatedVector));
        return playerMovingAnimations;
    }
    int GetMovementDirection(Vector3 velocity) {
        int movementDirection = 0;
        Vector3 dirVector = transform.forward;
        float angle = Vector3.SignedAngle(velocity, dirVector, new Vector3(0.0f, 1.0f, 0.0f));
        if (angle >= -22.5f) {
            movementDirection = Mathf.FloorToInt((angle + 22.5f) / 45.0f);
        }
        else {
            angle = -1 * angle;
            movementDirection = Mathf.FloorToInt((angle + 22.5f) / 45.0f);
            movementDirection = 8 - movementDirection;
        }
        //Debug.Log(movementDirection);
        return movementDirection + 1;
    }
    void ApplyPlayerMovement() {
        var verticalAxis = Input.GetAxis("Vertical");
        var horizontalAxis = Input.GetAxis("Horizontal");

        var cameraAngle = cameraController.CameraRotationAngle;
        var movementVector = new Vector3(horizontalAxis * Speed * SpeedMultiplier, 0.0f, verticalAxis * Speed * SpeedMultiplier);
        var rotatedVector = Quaternion.AngleAxis(cameraAngle, Vector3.up) * movementVector;
        if (verticalAxis != 0.0f || horizontalAxis != 0.0f) {
            transform.Translate(rotatedVector, Space.World);
            PlayerMovingAnimationState = GetPlayerMovementAnimationState();
        }
        else {
            PlayerMovingAnimationState = PlayerMovingAnimations.Idle;
        }
    }
    #endregion
    void ApplyFightControll() {
        if (Input.GetButtonDown("Fire1")) {
            bool isAtackOccured = DoAttack();
        }
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
        ApplyPlayerRotation();
        ApplyPlayerMovement();
        ApplyFightControll();
    }
    protected override void ApplyAnimations() {
        if (!_animationStateChanged)
            return;
        DistroyAllAnimations();
        AnimationClip animationClip = MovementAnimationDictionary[(int)PlayerMovingAnimationState];
        PlayableGraph playableGraph = playAnim(animationClip, AnimationBody);
        _animationStateChanged = false;
    }

    private List<PlayableGraph> graphs = new List<PlayableGraph>();
    private PlayableGraph playAnim(AnimationClip clip, GameObject obj) {
        PlayableGraph playableGraph;

        AnimationPlayableUtilities.PlayClip(obj.GetComponent<Animator>(), clip, out playableGraph);

        // save all graphs we create and destroy them at the end of our scene.
        // you might need to optimize this if you make a lot of animations.
        graphs.Add(playableGraph);

        return playableGraph;
    }
    void OnDisable() {
        DistroyAllAnimations();

    }
    void DistroyAllAnimations() {
        foreach (var g in graphs) {
            g.Destroy();
        }
        graphs.Clear();
    }

}
