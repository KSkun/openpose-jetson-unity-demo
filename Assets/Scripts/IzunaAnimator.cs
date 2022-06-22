using System.Collections.Generic;
using UnityEngine;

public class IzunaAnimator : MonoBehaviour
{
    public GameObject lHand1, lHand2, rHand1, rHand2;
    public GameObject lLeg1, lLeg2, rLeg1, rLeg2;
    public GameObject head;
    private AnimationCurve[] _animLHand1, _animLHand2, _animRHand1, _animRHand2;
    private AnimationCurve[] _animLLeg1, _animLLeg2, _animRLeg1, _animRLeg2;

    private AnimationCurve[] _animHead;
    // private AnimationClip _animClip;
    // private Animation _anim;

    private OpenPoseAnimation _oanim;
    public string animFile;

    private static float Vec2Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v2.x * v1.y;
    }

    private static float Vec2Angle(Vector2 from, Vector2 to)
    {
        var dot = Vector2.Dot(from.normalized, to.normalized);
        dot = Mathf.Clamp(dot, -1.0f, 1.0f);
        var acos = Mathf.Acos(dot) / Mathf.PI * 180.0f;
        var cross = Vec2Cross(from, to);
        if (cross < 0) acos *= -1;
        return acos;
    }

    public void Start()
    {
        _oanim = new OpenPoseAnimation($"{Application.dataPath}/Animations/{animFile}.oanim");

        var keyframesLHand1 = new List<Keyframe>[4];
        var keyframesLHand2 = new List<Keyframe>[4];
        var keyframesRHand1 = new List<Keyframe>[4];
        var keyframesRHand2 = new List<Keyframe>[4];

        var keyframesLLeg1 = new List<Keyframe>[4];
        var keyframesLLeg2 = new List<Keyframe>[4];
        var keyframesRLeg1 = new List<Keyframe>[4];
        var keyframesRLeg2 = new List<Keyframe>[4];

        var keyframesHead = new List<Keyframe>[4];

        for (var i = 0; i < 4; i++)
        {
            keyframesLHand1[i] = new List<Keyframe>();
            keyframesLHand2[i] = new List<Keyframe>();
            keyframesRHand1[i] = new List<Keyframe>();
            keyframesRHand2[i] = new List<Keyframe>();

            keyframesLLeg1[i] = new List<Keyframe>();
            keyframesLLeg2[i] = new List<Keyframe>();
            keyframesRLeg1[i] = new List<Keyframe>();
            keyframesRLeg2[i] = new List<Keyframe>();

            keyframesHead[i] = new List<Keyframe>();
        }

        for (var i = 0; i < _oanim.NumKeyframes; i++)
        {
            var keyframes = _oanim.Keyframes[i];
            var keypoints = keyframes.Keypoints;

            if (keypoints[2].x >= 0 && keypoints[3].x >= 0 && keypoints[4].x >= 0 &&
                keypoints[5].x >= 0 && keypoints[6].x >= 0 && keypoints[7].x >= 0)
            {
                var lrShoulder = keypoints[2] - keypoints[5];

                var leftHand1 = keypoints[6] - keypoints[5];
                var leftHand2 = keypoints[7] - keypoints[6];
                var angleLHand1 = -180 - Vec2Angle(lrShoulder, leftHand1);
                var angleLHand2 = -Vec2Angle(leftHand1, leftHand2);
                var rotLHand1 = Quaternion.Euler(0, 0, angleLHand1);
                var rotLHand2 = Quaternion.Euler(0, 0, angleLHand2);
                keyframesLHand1[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand1.x));
                keyframesLHand1[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand1.y));
                keyframesLHand1[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand1.z));
                keyframesLHand1[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand1.w));
                keyframesLHand2[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand2.x));
                keyframesLHand2[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand2.y));
                keyframesLHand2[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand2.z));
                keyframesLHand2[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotLHand2.w));

                var rightHand1 = keypoints[3] - keypoints[2];
                var rightHand2 = keypoints[4] - keypoints[3];
                var angleRHand1 = -Vec2Angle(-lrShoulder, rightHand1);
                var angleRHand2 = -Vec2Angle(rightHand1, rightHand2);
                var rotRHand1 = Quaternion.Euler(0, 0, angleRHand1);
                var rotRHand2 = Quaternion.Euler(0, 0, angleRHand2);
                keyframesRHand1[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand1.x));
                keyframesRHand1[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand1.y));
                keyframesRHand1[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand1.z));
                keyframesRHand1[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand1.w));
                keyframesRHand2[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand2.x));
                keyframesRHand2[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand2.y));
                keyframesRHand2[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand2.z));
                keyframesRHand2[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotRHand2.w));
            }

            if (keypoints[8].x >= 0 && keypoints[9].x >= 0 && keypoints[10].x >= 0 &&
                keypoints[11].x >= 0 && keypoints[12].x >= 0 && keypoints[13].x >= 0)
            {
                var lrHip = keypoints[8] - keypoints[11];

                var leftLeg1 = keypoints[12] - keypoints[11];
                var leftLeg2 = keypoints[13] - keypoints[12];
                var angleLLeg1 = -180 - Vec2Angle(lrHip, leftLeg1);
                var angleLLeg2 = -Vec2Angle(leftLeg1, leftLeg2);
                var rotLLeg1 = Quaternion.Euler(0, 0, angleLLeg1);
                var rotLLeg2 = Quaternion.Euler(0, 0, angleLLeg2);
                keyframesLLeg1[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg1.x));
                keyframesLLeg1[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg1.y));
                keyframesLLeg1[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg1.z));
                keyframesLLeg1[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg1.w));
                keyframesLLeg2[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg2.x));
                keyframesLLeg2[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg2.y));
                keyframesLLeg2[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg2.z));
                keyframesLLeg2[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotLLeg2.w));

                var rightLeg1 = keypoints[9] - keypoints[8];
                var rightLeg2 = keypoints[10] - keypoints[9];
                var angleRLeg1 = -Vec2Angle(-lrHip, rightLeg1);
                var angleRLeg2 = -Vec2Angle(rightLeg1, rightLeg2);
                var rotRLeg1 = Quaternion.Euler(0, 0, angleRLeg1);
                var rotRLeg2 = Quaternion.Euler(0, 0, angleRLeg2);
                keyframesRLeg1[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg1.x));
                keyframesRLeg1[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg1.y));
                keyframesRLeg1[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg1.z));
                keyframesRLeg1[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg1.w));
                keyframesRLeg2[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg2.x));
                keyframesRLeg2[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg2.y));
                keyframesRLeg2[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg2.z));
                keyframesRLeg2[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotRLeg2.w));
            }

            if (keypoints[0].x >= 0 && keypoints[1].x >= 0 && keypoints[2].x >= 0 && keypoints[5].x >= 0)
            {
                var lrShoulder = keypoints[2] - keypoints[5];
                var neckNose = keypoints[0] - keypoints[1];
                var angleHead = 90 - Vec2Angle(lrShoulder, neckNose);
                var rotHead = Quaternion.Euler(0, 0, angleHead);
                keyframesHead[0].Add(new Keyframe(keyframes.Time / 1000.0f, rotHead.x));
                keyframesHead[1].Add(new Keyframe(keyframes.Time / 1000.0f, rotHead.y));
                keyframesHead[2].Add(new Keyframe(keyframes.Time / 1000.0f, rotHead.z));
                keyframesHead[3].Add(new Keyframe(keyframes.Time / 1000.0f, rotHead.w));
            }
        }

        _animLHand1 = new AnimationCurve[4];
        _animLHand2 = new AnimationCurve[4];
        _animRHand1 = new AnimationCurve[4];
        _animRHand2 = new AnimationCurve[4];

        _animLLeg1 = new AnimationCurve[4];
        _animLLeg2 = new AnimationCurve[4];
        _animRLeg1 = new AnimationCurve[4];
        _animRLeg2 = new AnimationCurve[4];

        _animHead = new AnimationCurve[4];

        for (var i = 0; i < 4; i++)
        {
            _animLHand1[i] = new AnimationCurve(keyframesLHand1[i].ToArray());
            _animLHand2[i] = new AnimationCurve(keyframesLHand2[i].ToArray());
            _animRHand1[i] = new AnimationCurve(keyframesRHand1[i].ToArray());
            _animRHand2[i] = new AnimationCurve(keyframesRHand2[i].ToArray());

            _animLLeg1[i] = new AnimationCurve(keyframesLLeg1[i].ToArray());
            _animLLeg2[i] = new AnimationCurve(keyframesLLeg2[i].ToArray());
            _animRLeg1[i] = new AnimationCurve(keyframesRLeg1[i].ToArray());
            _animRLeg2[i] = new AnimationCurve(keyframesRLeg2[i].ToArray());

            _animHead[i] = new AnimationCurve(keyframesHead[i].ToArray());
        }

        // _animClip = new AnimationClip
        // {
        //     legacy = true
        // };
        //
        // for (var i = 0; i < 4; i++)
        // {
        //     string axis = "";
        //     switch (i)
        //     {
        //         case 0:
        //             axis = "x";
        //             break;
        //         case 1:
        //             axis = "y";
        //             break;
        //         case 2:
        //             axis = "z";
        //             break;
        //         case 3:
        //             axis = "w";
        //             break;
        //     }
        //
        //     _animClip.SetCurve("Rig/left hand 1", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animLHand1[i]);
        //     _animClip.SetCurve("Rig/left hand 1/left hand 2", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animLHand2[i]);
        //     _animClip.SetCurve("Rig/right hand 1", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animRHand1[i]);
        //     _animClip.SetCurve("Rig/right hand 1/right hand 2", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animRHand2[i]);
        //
        //     _animClip.SetCurve("Rig/left leg 1", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animLLeg1[i]);
        //     _animClip.SetCurve("Rig/left leg 1/left leg 2", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animLLeg2[i]);
        //     _animClip.SetCurve("Rig/right leg 1", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animRLeg1[i]);
        //     _animClip.SetCurve("Rig/right leg 1/right leg 2", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animRLeg2[i]);
        //
        //     _animClip.SetCurve("Rig/neck/head", typeof(Transform),
        //         $"m_LocalRotation.{axis}", _animHead[i]);
        // }
        //
        //
        // _animClip.EnsureQuaternionContinuity();
        //
        // AssetDatabase.CreateAsset(_animClip, $"Assets/Animations/{animFile}.anim");
        // AssetDatabase.SaveAssets();
    }

    public void Update()
    {
        var t = Time.time;
        lHand1.transform.localRotation = new Quaternion(_animLHand1[0].Evaluate(t), _animLHand1[1].Evaluate(t),
            _animLHand1[2].Evaluate(t), _animLHand1[3].Evaluate(t));
        lHand2.transform.localRotation = new Quaternion(_animLHand2[0].Evaluate(t), _animLHand2[1].Evaluate(t),
            _animLHand2[2].Evaluate(t), _animLHand2[3].Evaluate(t));
        rHand1.transform.localRotation = new Quaternion(_animRHand1[0].Evaluate(t), _animRHand1[1].Evaluate(t),
            _animRHand1[2].Evaluate(t), _animRHand1[3].Evaluate(t));
        rHand2.transform.localRotation = new Quaternion(_animRHand2[0].Evaluate(t), _animRHand2[1].Evaluate(t),
            _animRHand2[2].Evaluate(t), _animRHand2[3].Evaluate(t));

        lLeg1.transform.localRotation = new Quaternion(_animLLeg1[0].Evaluate(t), _animLLeg1[1].Evaluate(t),
            _animLLeg1[2].Evaluate(t), _animLLeg1[3].Evaluate(t));
        lLeg2.transform.localRotation = new Quaternion(_animLLeg2[0].Evaluate(t), _animLLeg2[1].Evaluate(t),
            _animLLeg2[2].Evaluate(t), _animLLeg2[3].Evaluate(t));
        rLeg1.transform.localRotation = new Quaternion(_animRLeg1[0].Evaluate(t), _animRLeg1[1].Evaluate(t),
            _animRLeg1[2].Evaluate(t), _animRLeg1[3].Evaluate(t));
        rLeg2.transform.localRotation = new Quaternion(_animRLeg2[0].Evaluate(t), _animRLeg2[1].Evaluate(t),
            _animRLeg2[2].Evaluate(t), _animRLeg2[3].Evaluate(t));

        head.transform.localRotation = new Quaternion(_animHead[0].Evaluate(t), _animHead[1].Evaluate(t),
            _animHead[2].Evaluate(t), _animHead[3].Evaluate(t));
    }
}