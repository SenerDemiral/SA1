using System;
using Starcounter;

namespace SharedDB
{

    [Database]
    public class IotOwner
    {
        public string Id;
        public string Name;

        public QueryResultRows<Iot> Iots {
            get {
                return Db.SQL<Iot>("SELECT r FROM Iot r WHERE r=?", this);
            }
        }
    }

    [Database]
    public class Iot
    {
        public IotOwner iotOwner;
        public string Id;

        public QueryResultRows<IotData> LastIotData(int fetch)
        {
            return Db.SQL<IotData>("SELECT r FROM IotData r WHERE r=? ORDER BY r.SampleTime DESC FETCH ?", this, fetch);
        }

        public void SetOwner(IotOwner owner)
        {
            this.iotOwner = owner;
        }
    }

    [Database]
    public class IotData
    {
        public Iot iot;
        public DateTime SampleTime;
        public string Value;
    }
}