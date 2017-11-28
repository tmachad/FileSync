
namespace FileSync.Views
{
    interface IFileSyncView
    {
        /// <summary>
        /// Sets some text to alert the user of something.
        /// </summary>
        /// <param name="text">The text to display to the user</param>
        void SetAlertText(string text);

        /// <summary>
        /// Clears the alert text.
        /// </summary>
        void ClearAlertText();

        /// <summary>
        /// Sets the UI to it's initial state.
        /// </summary>
        void InitialState();

        /// <summary>
        /// Sets the UI to a state that indicates that the
        /// application is in the process of locating files.
        /// </summary>
        void FindingFiles();

        /// <summary>
        /// Sets the UI to a state that indicates that the 
        /// application is in the process of copying files.
        /// </summary>
        /// <param name="total">The number of items being copied</param>
        void CopyingFiles(int total);

        /// <summary>
        /// Sets the UI to a state that indicates that the 
        /// application has finished copying files.
        /// </summary>
        void DoneCopyingFiles();

        /// <summary>
        /// Sets the progress displayed by the UI. This number
        /// is in relation to the total amount set in the
        /// CopyingFiles(int) method. Calling SetProgress(int)
        /// before CopyingFiles(int) should do nothing.
        /// </summary>
        /// <param name="amount"></param>
        void SetProgress(int amount);
    }
}
