namespace ProfileManager
{
    public interface OnProfileApplied
    {
        void ProfileApplied(string statusCode, string extendedStatusCode);
        void ProfileError(string statusCode, string extendedStatusCode);
    }
}