namespace highload.Settings
{
    public interface ICurrentSettings
    {
        string DataPath { get; }

        int NumberOfThreads { get; }

        int MinimumWordLength { get; }
    }
}