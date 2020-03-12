﻿namespace Com.Qazima.NetCore.Library.Http.Action.Database {
    public interface IDatabaseAction : IAction {
        string ConnectionString { get; set; }

        string SqlUrl { get; set; }

        string TableUrl { get; set; }

        bool ExposeDataModel { get; set; }

        void FetchSchemas();
    }
}
