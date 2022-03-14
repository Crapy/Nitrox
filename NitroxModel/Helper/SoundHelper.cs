namespace NitroxModel.Helper;

public static class SoundHelper
{
    /// <summary>
    ///     Volume calculation for listener (not 100% realistic)
    /// </summary>
    public static float CalculateVolume(float distance, float radius, float volume)
    {
        if (0f <= distance && distance < radius)
        {
            return Mathf.Clamp01((1f - distance / radius) * volume);
        }
        return 0f;
    }
}
