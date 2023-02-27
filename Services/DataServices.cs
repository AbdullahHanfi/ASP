using ASP.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;

namespace ASP.Services
{
    interface IAddData
    {
        int Add(Scheduler scheduler);
    }
    interface IDeleteData
    {
        bool Delete(int? id, int? userid);
    }
    interface IEditData
    {
        bool Edit(Scheduler schedulers, int? id);
    }


    public class DataServices: IAddData , IDeleteData , IEditData 
    {
        AS_DBEntities db;
        public DataServices() { 
            db = new AS_DBEntities();
        }
        /// <summary>
        /// Check Name and EndDate if valid 
        /// </summary>
        /// <param name="scheduler"></param>
        /// <returns>return 1 if correct answer , zero if EndDate is not valid ,nagtive one if name is not valid , nagtive Two if both of them </returns>
        public int Add(Scheduler scheduler)
        {
            if (scheduler.EndDate != null && DateTime.Compare(scheduler.EndDate, DateTime.Now) > 0 && !string.IsNullOrEmpty(scheduler.Name))
            {
                db.Schedulers.Add(scheduler);
                db.SaveChanges();
                return 1;
            }
            else if ((scheduler.EndDate == null || DateTime.Compare(scheduler.EndDate, DateTime.Now) < 1) && string.IsNullOrEmpty(scheduler.Name))
            {
                return -2;
            }
            else if (string.IsNullOrEmpty(scheduler.Name))
                return -1;
            else
            {
                return 0;
            }
        }

        public bool Delete(int? id,int? userid)
        {
            Scheduler Scheduler =db.Schedulers
                .Where(Sh => Sh.id == id && userid == Sh.userId)
                .First();
            if (Scheduler!=null)
            {
                db.Schedulers.Remove(Scheduler);
                db.SaveChanges();
                return true;   
            }
            return false;
        }

        public bool Edit(Scheduler scheduler, int? userid)
        {
            var item = db.Schedulers.SingleOrDefault(M => M.id == scheduler.id);
            if (userid != scheduler.userId|| item==null )
            {
                return false;
            }
            if (scheduler.EndDate != null 
                && DateTime.Compare(scheduler.EndDate, DateTime.Now) > 0 
                && !string.IsNullOrEmpty(scheduler.Name))
            {
                item.description = scheduler.description;
                item.Name = scheduler.Name;
                item.EndDate = scheduler.EndDate;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}