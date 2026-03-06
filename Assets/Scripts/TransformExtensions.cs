using UnityEngine;

public static class TransformExtensions
{
    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; ++i)
        {
            var child = parent.GetChild(i);
            if (child.CompareTag(tag)) return child;
        }
        return null;
    }
}
