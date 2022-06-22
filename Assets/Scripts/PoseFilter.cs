using System;
using System.Collections.Generic;
using UnityEngine;

public class PoseFilter
{
    private List<Vector2[]> _window;
    private int _windowSize;
    private bool _full;
    private float[] _weights;

    public PoseFilter(int windowSize = 3)
    {
        _window = new List<Vector2[]>();
        _windowSize = windowSize;
        _full = false;

        _weights = new float[windowSize];
        var sumWeight = (1 + windowSize) * windowSize / 2;
        for (var i = 0; i < windowSize; i++)
        {
            _weights[i] = (float) (i + 1) / sumWeight;
        }
    }

    public Vector2[] Filter(Vector2[] pose)
    {
        _window.Add(pose);
        if (!_full && _window.Count == _windowSize)
        {
            _full = true;
        }

        if (!_full)
        {
            return pose;
        }

        var poseFiltered = new Vector2[18];
        var invalidWeights = new float[18];
        Array.Clear(invalidWeights, 0, 18);
        var pIndex = 0;
        foreach (var wPose in _window)
        {
            for (var j = 0; j < 18; j++)
            {
                var keypoint = wPose[j];
                if (keypoint.x < 0) invalidWeights[j] += _weights[pIndex];
                else poseFiltered[j] += wPose[j] * _weights[pIndex];
            }

            pIndex++;
        }

        // if some keypoints in poses is invalid, use current keypoint instead of them
        for (var i = 0; i < 18; i++)
        {
            if (pose[i].x < 0) poseFiltered[i] = pose[i];
            else poseFiltered[i] += pose[i] * invalidWeights[i];
        }

        _window.RemoveAt(0);
        return poseFiltered;
    }
}