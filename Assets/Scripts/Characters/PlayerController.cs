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
using UnityEngine.UI;

public class PlayerController : CharacterControllerBase {
    private float SpeedMultiplier { get; set; } = 0.01f;
    private Image PlayerProgressBar;
    public PlayerMovingAnimations PlayerMovingAnimationState { get; set; }
    public PlayerFightAnimations PlayerFightAnimationState { get; set; }
    private CameraController cameraController;
    // Start is called before the first frame update
    protected override Dictionary<int, AnimationClip> InitAnimationsDictionary() {
        var dictionary = AnimationHelper.GetAnimationsDictionary(typeof(PlayerMovingAnimations), AnimationClips);
        return dictionary;
    }

    protected override void Start() {
        base.Start();
        PlayerProgressBar = GameObject.FindGameObjectsWithTag("PlayerProgressBar")[0].GetComponent<Image>();
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
    int GetPlayerMovementAnimationState() {
        PlayerMovingAnimations playerMovingAnimations = PlayerMovingAnimations.Idle;
        var verticalAxis = Input.GetAxis("Vertical");
        var horizontalAxis = Input.GetAxis("Horizontal");
        var cameraAngle = cameraController.CameraRotationAngle;
        var movementVector = new Vector3(horizontalAxis, 0.0f, verticalAxis);
        var rotatedVector = Quaternion.AngleAxis(cameraAngle, Vector3.up) * movementVector;
        playerMovingAnimations = (PlayerMovingAnimations)Enum.ToObject(typeof(PlayerMovingAnimations), GetMovementDirection(rotatedVector));
        return (int)playerMovingAnimations;
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
            SetAnimationState(GetPlayerMovementAnimationState());
        }
        else {
            SetAnimationState((int)PlayerMovingAnimations.Idle);
        }
    }
    #endregion
    void ApplyFightControll() {
        if (Input.GetButtonDown("Fire1")) {
            bool isAtackOccured = DoAttack();
        }
    }
    void ApplyProgressBar() {
        float playerHP = this.HealthPoints;
        PlayerProgressBar.fillAmount = playerHP / 100.0f;  //Later put maxHP
    }
    protected override void FixedUpdate() {
        base.FixedUpdate();
        ApplyProgressBar();
        ApplyPlayerRotation();
        ApplyPlayerMovement();
        ApplyFightControll();
    }
    protected override void ChangeAnimation( int nextAnimationState) {
        base.ChangeAnimation( nextAnimationState);
    }
    protected override void SetAnimationState(int animationState) {            
        base.SetAnimationState(animationState);
    }
    void OnDisable() {
        DistroyAllAnimations();
    }
    void DistroyAllAnimations() {
        PlayableGraphObject.Destroy();
    }
    
}
