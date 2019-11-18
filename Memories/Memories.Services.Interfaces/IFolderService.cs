namespace Memories.Services.Interfaces
{
    public interface IFolderService
    {
        /// <summary>
        /// Get application installation folder
        /// </summary>
        /// <returns></returns>
        string GetInstallationFolder();
        
        /// <summary>
        /// Get full path of folder in application installation folder
        /// </summary>
        /// <param name="paths">Names of subfolders</param>
        /// <returns></returns>
        string GetAppFolder(params string[] paths);

        string GetBookTemplateFolder(string paperSize);
        string GetPageTemplateFolder(string paperSize);
    }
}
