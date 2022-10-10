namespace ProgressTracker.Models
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides method to read user data.
    /// </summary>
    public static class UserDataReader
    {
        /// <summary>
        /// Reads users data from json files inside specified directory.
        /// </summary>
        /// <param name="fileNames">Array with names of the data files.</param>
        /// <returns>Sequence of user information data transfer objects.</returns>
        public static IEnumerable<UserInfoDataTransferObject> GetUsersDataFromFiles(string[] fileNames)
        {
            foreach (var filePath in fileNames)
            {
                if (!TryGetDayFromFilePath(filePath, out int day))
                {
                    continue;
                }

                using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                IEnumerable<UserInfoDataTransferObject>? data = Enumerable.Empty<UserInfoDataTransferObject>();
                try
                {
                    data = JsonSerializer.Deserialize<IEnumerable<UserInfoDataTransferObject>>(fileStream);
                    if (data is null)
                    {
                        continue;
                    }
                }
                catch (JsonException)
                {
                    continue;
                }

                foreach (var userData in data)
                {
                    userData.Day = day;
                    yield return userData;
                }
            }
        }

        private static bool TryGetDayFromFilePath(string filePath, out int day)
        {
            string pattern = @"(.*\\\D*)|(\.\w*)";
            Regex regex = new Regex(pattern);
            return int.TryParse(regex.Replace(filePath, string.Empty), out day);
        }
    }
}
