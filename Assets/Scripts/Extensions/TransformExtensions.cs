using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Recursive search for a child object by name at any nesting level.
    /// </summary>
    /// <param name="parent">Parent Transform.</param>
    /// <param name="childName">The name of the object you are looking for.</param>
    /// <returns>Transform the found object, or null if not found.</returns>
    public static Transform FindDeepChild(this Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform found = child.FindDeepChild(childName);
            if (found != null)
                return found;
        }
        return null;
    }
}
