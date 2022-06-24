using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> Walks { get; set; }

        public int TotalDuration
        {
            get
            {
                int totalTime = 0;
                foreach (var walk in Walks)
                {
                    totalTime += walk.Duration;
                }
                return totalTime;
            }
        }
    }
}