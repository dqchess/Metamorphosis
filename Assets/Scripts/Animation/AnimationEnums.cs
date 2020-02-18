using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnimationEnums {
    public enum PlayerMovingAnimations {
        Idle = 0,
        MoveForward = 1,
        MoveForwardRight = 2,
        MoveRight = 3,
        MoveBackwardRight = 4,
        MoveBackward = 5,
        MoveBackwardLeft = 6,
        MoveLeft = 7,
        MoveForwardLeft = 8
    }
    public enum PlayerFightAnimations {
        Idle = 0,
        SimpleAttack = 1,
        SpecialAttack = 2
    }
    public enum EnemyAnimations {
        Idle = 0,
        MoveForward = 1
    }
    public enum EnemyAttackAnimations {
        Idle = 0,
        SimpleAttack = 1,
        SpecialAttack = 2
    }

}
