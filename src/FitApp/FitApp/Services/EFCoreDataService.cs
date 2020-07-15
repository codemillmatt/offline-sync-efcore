using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitApp.Core
{
    public class EFCoreDataService : ILocalDataService
    {
        string loggedInUser;

        public EFCoreDataService()
        {            
            loggedInUser = Preferences.Get(Constants.UserIdPreference, "Matt");
        }

        public void DeleteAllLocalSessions()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TrainingSession>> GetLocalSessions()
        {
            List<TrainingSession> localSessions = new List<TrainingSession>();

            using (var db = new EFCoreDataContext())
            {

                var allSessions = db.TrainingSessions.ToList();

                    localSessions.AddRange(
                    await db.TrainingSessions.Where(t => t.UserId == loggedInUser).ToListAsync()
                );
            }

            return localSessions;
        }

        public async Task SaveSessionsFromWeb(DataSyncInfo dataSync, List<TrainingSession> trainingSessions)
        {
            using (var db = new EFCoreDataContext())
            {
                // first save the data sync metadata
                dataSync.UserId = loggedInUser;

                await db.DataSyncInfo.AddAsync(dataSync);

                if (dataSync.Type == "Full")
                {
                    // delete the data for the logged in user and insert everything from scratch
                    if (await db.TrainingSessions.AnyAsync(t => t.UserId == loggedInUser))
                    {
                        var allSessions = db.TrainingSessions.Where(s => s.UserId == loggedInUser);
                        db.Remove(allSessions);
                    }

                    await db.TrainingSessions.AddRangeAsync(trainingSessions);
                }
                else
                {
                    // loop through the training sessions
                    foreach (var session in trainingSessions)
                    {
                        switch (session.DataOperation)
                        {
                            case Constants.DataOpUpdate:
                                var update = await db.TrainingSessions.FirstAsync(s => s.Id == session.Id);

                                update.Duration = session.Duration;
                                update.Calories = session.Calories;
                                update.Distance = session.Distance;
                                update.Steps = session.Steps;

                                break;

                            case Constants.DataOpInsert:
                                await db.TrainingSessions.AddAsync(session);

                                break;

                            case Constants.DataOpDelete:
                                var sessionToRemove = await db.TrainingSessions.FirstAsync(t => t.Id == session.Id);
                                db.TrainingSessions.Remove(sessionToRemove);

                                break;
                                
                            default:
                                break;
                        }
                    }
                }
                
                await db.SaveChangesAsync();
            }
        }

        public async Task<long> GetLatestSyncVersion()
        {
            long id = 0;

            try
            {
                using (var db = new EFCoreDataContext())
                {
                    var allDs = await db.DataSyncInfo.ToListAsync();

                    if (await db.DataSyncInfo.AnyAsync(t => t.UserId == loggedInUser))
                        id = await db.DataSyncInfo.Where(t => t.UserId == loggedInUser).MaxAsync(t => t.Version);
                }
            }
            catch (Exception ex)
            {
                // could happen if nothing in the db yet
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return id;
        }
    }
}
