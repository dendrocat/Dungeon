using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Finds the first child object of the specified parent <see cref="Transform"/> that has the given tag.
    /// </summary>
    /// <param name="parent">The parent <see cref="Transform"/> in which to search.</param>
    /// <param name="tag">The tag of the child object to find.</param>
    /// <returns>
    /// The found child <see cref="Transform"/> with the specified tag, or <c>null</c> if no such object was found.
    /// </returns>
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
