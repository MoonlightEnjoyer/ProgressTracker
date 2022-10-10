namespace ProgressTracker
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Win32;
    using ProgressTracker.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.UsersViewModel = new UsersViewModel();
            this.usersGrid.ItemsSource = this.UsersViewModel.Users;
            this.DataContext = this.UsersViewModel;
        }

        /// <summary>
        /// Gets or sets UsersViewModel.
        /// </summary>
        public UsersViewModel UsersViewModel { get; set; }

        /// <summary>
        /// Changes active user.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void ChangeSelectedUser(object sender, MouseEventArgs eventArgs)
        {
            var dataGridRow = sender as DataGridRow;
            var userInfo = dataGridRow?.DataContext as UserInfo;
            if (userInfo is not null)
            {
                this.UsersViewModel.ChangeSelectedUser(userInfo);
            }

            for (int i = 0; i < this.usersGrid.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)this.usersGrid.ItemContainerGenerator.ContainerFromIndex(i);
                if (row is not null)
                {
                    var rowData = row.DataContext as UserInfo;
                    if ((double)rowData!.BestSteps / (double)this.UsersViewModel.AverageSteps > 1.2 || (double)rowData.WorstSteps / (double)this.UsersViewModel.AverageSteps > 1.2)
                    {
                        row.Background = Brushes.Green;
                    }
                    else if ((double)rowData.BestSteps / (double)this.UsersViewModel.AverageSteps < 0.8 || (double)rowData.WorstSteps / (double)this.UsersViewModel.AverageSteps < 0.8)
                    {
                        row.Background = Brushes.Red;
                    }
                    else
                    {
                        row.Background = Brushes.White;
                    }
                }
            }
        }

        /// <summary>
        /// Exports data of the active user.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void ExportData(object sender, RoutedEventArgs eventArgs)
        {
            if (this.UsersViewModel.SelectedUser is not null)
            {
                UserInfoXmlWriter.Write($"{this.UsersViewModel.SelectedUser.Name}.xml", this.UsersViewModel.SelectedUser);
            }
        }

        /// <summary>
        /// Chooses files with users data to display.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="eventArgs">Event arguments.</param>
        public void ChooseFiles(object sender, RoutedEventArgs eventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileNames.Length > 0)
            {
                this.UsersViewModel.LoadData(openFileDialog.FileNames);
                this.usersGrid.ItemsSource = this.UsersViewModel.Users;
            }
        }
    }
}
