using System;
using System.Linq;

public static class EnumExtensions
{
    /// <summary>
    /// Translates an int describing a flag-type enum, into an array of maching enum entries
    /// </summary>
    /// <param name="input">Int describing a flag-type enum</param>
    /// <typeparam name="T">Enum type</typeparam>
    /// <returns>An array of matching enum entires</returns>
    public static T[] GetMatchingFlags<T>(this int input) where T : Enum
    {
        // Get all possible enum values except the default value (e.g., 'None')
        var allFlags = Enum.GetValues(typeof(T))
                           .Cast<T>()
                           .Where(f => !f.Equals(default(T)))
                           .ToArray();

        // Check which flags are set in the input number
        var matchingFlags = allFlags.Where(f => (input & Convert.ToInt32(f)) == Convert.ToInt32(f)).ToArray();

        // If no flags are set, return an array with the default value (e.g., 'None')
        return matchingFlags.Length > 0 ? matchingFlags : new T[] { default };
    }
}
