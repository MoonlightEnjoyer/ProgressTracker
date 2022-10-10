namespace ProgressTracker.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using OxyPlot;
    using ProgressTracker.Models;

    /// <summary>
    /// Represents view model for user inormation.
    /// </summary>
    public class UsersViewModel : INotifyPropertyChanged
    {
        private List<UserInfo> users;
        private PlotModel plotModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersViewModel"/> class.
        /// </summary>
        public UsersViewModel()
        {
            this.InitializeViewModel(UserDataReader.GetUsersDataFromFiles(Directory.GetFiles("TestData")));
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets collection of UserInfo objects.
        /// </summary>
        public List<UserInfo> Users
        {
            get => this.users;

            set
            {
                this.users = value;
            }
        }

        /// <summary>
        /// Gets or sets selected user.
        /// </summary>
        public UserInfo SelectedUser { get; set; }

        /// <summary>
        /// Gets average steps of user.
        /// </summary>
        public int AverageSteps { get; private set; }

        /// <summary>
        /// Gets or sets PlotModel.
        /// </summary>
        public PlotModel PlotModel
        {
            get => this.plotModel;

            set
            {
                this.plotModel = value;
                this.OnPropertyChanged("PlotModel");
            }
        }

        /// <summary>
        /// Changes active user.
        /// </summary>
        /// <param name="userInfo">Data of selected user.</param>
        public void ChangeSelectedUser(UserInfo userInfo)
        {
            this.SelectedUser = userInfo;
            this.PlotModel = this.users.Single(u => u.Name == userInfo.Name).PlotModel;
            this.AverageSteps = userInfo.AverageSteps;
        }

        /// <summary>
        /// Loads data from specified files and applies it to the view model.
        /// </summary>
        /// <param name="fileNames">Names of files with data to load.</param>
        public void LoadData(string[] fileNames)
        {
            this.InitializeViewModel(UserDataReader.GetUsersDataFromFiles(fileNames));
        }

        /// <summary>
        /// Invokes PropertyChanged event.
        /// </summary>
        /// <param name="property">Name of changed property.</param>
        protected virtual void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void InitializeViewModel(IEnumerable<UserInfoDataTransferObject> data)
        {
            this.users = new List<UserInfo>();

            foreach (var user in data)
            {
                if (!this.users.Any(u => u.Name == user.Name))
                {
                    this.users.Add(new UserInfo()
                    {
                        Name = user.Name,
                    });
                }

                this.users.Single(u => u.Name == user.Name).AddDayInfo(user.Day, user.Steps, user.Rank, user.Status);
            }
        }
    }
}
