using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Finds the first child object in the entire hierarchy of the specified parent <see cref="Transform"/> that has the given tag.
    /// </summary>
    /// <param name="parent">The parent <see cref="Transform"/> in which to search.</param>
    /// <param name="tag">The tag of the child object to find.</param>
    /// <returns>
    /// The found child <see cref="Transform"/> with the specified tag, or <c>null</c> if no such object was found.
    /// </returns>
    public static Transform FindChildWithTag(this Transform parent, string tag)
    {
        if (parent.CompareTag(tag)) return parent;

        for (int i = 0; i < parent.childCount; ++i)
        {
            var found = parent.GetChild(i).FindChildWithTag(tag);
            if (found != null) return found;
        }
        return null;
    }
}
