using System;
using System.Collections.Generic;

namespace FitApp.Core
{
    public class EFCoreDataService : ILocalDataService
    {
        public EFCoreDataService()
        {
        }

        public void DeleteAllLocalSessions()
        {
            throw new NotImplementedException();
        }

        public List<TrainingSession> GetLocalSessions()
        {
            throw new NotImplementedException();
        }

        public void SaveSessionFromWeb(List<TrainingSession> sessions, bool fullReload, int currentVersion)
        {
            throw new NotImplementedException();
        }
    }
}
