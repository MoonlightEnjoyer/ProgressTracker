namespace ProgressTracker.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Series;

    /// <summary>
    /// Contains user information.
    /// </summary>
    public class UserInfo
    {
        private int averageSteps;
        private int bestSteps;
        private int worstSteps;
        private PlotModel plotModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        public UserInfo()
        {
            this.averageSteps = -1;
            this.bestSteps = -1;
            this.worstSteps = -1;
            this.DailyInfo = new SortedDictionary<int, (int rank, int steps, string status)>();
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets AverageSteps.
        /// </summary>
        public int AverageSteps
        {
            get
            {
                if (this.averageSteps == -1)
                {
                    this.averageSteps = this.DailyInfo.Sum(s => s.Value.steps) / this.DailyInfo.Count;
                }

                return this.averageSteps;
            }

            set
            {
                this.averageSteps = value;
            }
        }

        /// <summary>
        /// Gets or sets BestSteps.
        /// </summary>
        public int BestSteps
        {
            get
            {
                if (this.bestSteps == -1)
                {
                    this.bestSteps = this.DailyInfo.Max(s => s.Value.steps);
                }

                return this.bestSteps;
            }

            set
            {
                this.bestSteps = value;
            }
        }

        /// <summary>
        /// Gets or sets WorstSteps.
        /// </summary>
        public int WorstSteps
        {
            get
            {
                if (this.worstSteps == -1)
                {
                    this.worstSteps = this.DailyInfo.Min(s => s.Value.steps);
                }

                return this.worstSteps;
            }

            set
            {
                this.worstSteps = value;
            }
        }

        /// <summary>
        /// Gets DailyInfo.
        /// </summary>
        public SortedDictionary<int, (int rank, int steps, string status)> DailyInfo { get; private set; }

        /// <summary>
        /// Gets plotModel.
        /// </summary>
        public PlotModel PlotModel
        {
            get
            {
                if (this.plotModel is null)
                {
                    this.plotModel = new PlotModel() { Title = this.Name };

                    foreach (var item in this.DailyInfo)
                    {
                        (byte alpha, byte red, byte green, byte blue) color;
                        if (item.Value.steps == this.worstSteps)
                        {
                            color = (255, 255, 0, 0);
                        }
                        else if (item.Value.steps == this.bestSteps)
                        {
                            color = (255, 0, 255, 0);
                        }
                        else
                        {
                            color = (150, 0, 0, 255);
                        }

                        var series = new LinearBarSeries() { FillColor = OxyColor.FromArgb(color.alpha, color.red, color.green, color.blue) };
                        series.Points.Add(new DataPoint(item.Key, item.Value.steps));
                        this.plotModel.Series.Add(series);
                    }
                }

                return this.plotModel;
            }
        }

        /// <summary>
        /// Saves number of steps, rank and status for specified day.
        /// </summary>
        /// <param name="day">Day to save.</param>
        /// <param name="steps">Number of steps for specified day.</param>
        /// <param name="rank">Rank.</param>
        /// <param name="status">Status.</param>
        public void AddDayInfo(int day, int steps, int rank, string status)
        {
            if (!this.DailyInfo.ContainsKey(day))
            {
                this.DailyInfo.Add(day, (rank, steps, status));
            }
        }
    }
}
