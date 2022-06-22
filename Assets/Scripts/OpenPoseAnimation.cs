using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public struct OpenPoseAnimationKeyframe
{
    public long Time;
    public Vector2[] Keypoints;
}

public class OpenPoseAnimation
{
    public Vector2Int ImageSize;
    public List<OpenPoseAnimationKeyframe> Keyframes;
    public long NumKeyframes => Keyframes.Count;
    public long Length => Keyframes.Last().Time;

    private const int NumKeypoints = 18;

    public OpenPoseAnimation()
    {
    }

    public OpenPoseAnimation(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);
        var imageX = reader.ReadInt32();
        var imageY = reader.ReadInt32();
        ImageSize.Set(imageX, imageY);

        var num = reader.ReadInt64();
        Keyframes = new List<OpenPoseAnimationKeyframe>();
        for (var i = 0; i < num; i++)
        {
            var time = reader.ReadInt64();
            var keypoints = new Vector2[NumKeypoints];
            for (var j = 0; j < NumKeypoints; j++)
            {
                var x = reader.ReadInt32();
                var y = reader.ReadInt32();
                keypoints[j].Set(x, y);
            }

            Keyframes.Add(new OpenPoseAnimationKeyframe
            {
                Time = time,
                Keypoints = keypoints
            });
        }
    }
}