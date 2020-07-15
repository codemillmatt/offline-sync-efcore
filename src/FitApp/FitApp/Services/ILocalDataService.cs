using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitApp.Core
{
    public interface ILocalDataService
    {
        public Task SaveSessionsFromWeb(DataSyncInfo dataSync, List<TrainingSession> trainingSessions);
        public Task<List<TrainingSession>> GetLocalSessions();
        public Task<long> GetLatestSyncVersion();
    }
}
