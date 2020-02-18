using AnimationEnums;
using CleverCamera;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : CharacterControllerBase {
    private float SpeedMultiplier { get; set; } = 0.01f;
    public PlayerMovingAnimations PlayerMovingAnimationState { get; set; } = PlayerMovingAnimations.Idle;
    public PlayerFightAnimations PlayerFightAnimationState { get; set; } = PlayerFightAnimations.Idle;    
    
    private CameraController cameraController;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
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
    void ApplyPlayerMovement() {
        var verticalAxis = Input.GetAxis("Vertical");
        var horizontalAxis = Input.GetAxis("Horizontal");

        var cameraAngle = cameraController.CameraRotationAngle;
        var movementVector = new Vector3(horizontalAxis * Speed * SpeedMultiplier, 0.0f, verticalAxis * Speed * SpeedMultiplier);
        var rotatedVector = Quaternion.AngleAxis(cameraAngle, Vector3.up) * movementVector;
        if (verticalAxis != 0.0f || horizontalAxis != 0.0f) {
            transform.Translate(rotatedVector, Space.World);
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
    protected bool isRunned = false;
    protected override void ApplyAnimations() {
        //if (!Animation.IsPlaying("Running")) {
        //    Animation.Play("Running");
        //}
        if (!isRunned) {
            isRunned = true;
            var animClips = AnimationClipsLoader.GetAnimationClips("Player");
            AnimationClip runningClip = animClips.FirstOrDefault(s => s.name == "Left");
            GameObject animationObject = HelperTools.FindObjectInChildWithTag(gameObject, "BodyModel");
            playAnim(runningClip, animationObject);
        }
    }

    private List<PlayableGraph> graphs = new List<PlayableGraph>();
    private PlayableGraph playAnim(AnimationClip clip, GameObject obj) {
        PlayableGraph playableGraph;

        AnimationPlayableUtilities.PlayClip(obj.AddComponent<Animator>(), clip, out playableGraph);

        // save all graphs we create and destroy them at the end of our scene.
        // you might need to optimize this if you make a lot of animations.
        graphs.Add(playableGraph);

        return playableGraph;
    }
    void OnDisable() {
        foreach (var g in graphs) {
            g.Destroy();
        }
        graphs.Clear();
    }

}
