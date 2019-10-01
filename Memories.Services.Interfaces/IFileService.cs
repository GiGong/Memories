namespace Memories.Services.Interfaces
{
    public interface IFileService
    {
        string SaveFilePath(string filter = null);
        string OpenFilePath(string filter = null);
    }
}
