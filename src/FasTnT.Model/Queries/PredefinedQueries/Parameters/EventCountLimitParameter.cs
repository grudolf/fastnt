﻿namespace FasTnT.Model.Queries.PredefinedQueries.Parameters
{
    public class EventCountLimitParameter : SimpleEventQueryParameter
    {
        public int Limit => int.Parse(Value);
    }
}
