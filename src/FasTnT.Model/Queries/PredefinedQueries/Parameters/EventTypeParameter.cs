﻿using FasTnT.Domain;
using FasTnT.Model.Utils;
using System.Collections.Generic;
using System.Linq;

namespace FasTnT.Model.Queries.PredefinedQueries.Parameters
{
    public class EventTypeParameter : SimpleEventQueryParameter
    {
        public IEnumerable<EventType> EventTypes => Values.Select(Enumeration.GetByDisplayName<EventType>);
    }
}
