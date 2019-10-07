namespace Memories.Services.Interfaces
{
    public interface IFileService
    {
        string SaveFilePath(string filter = null, string title = "Memories");
        string OpenFilePath(string filter = null, string title = "Memories");
    }
}
