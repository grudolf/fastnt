﻿using FasTnT.Domain.Formatter;

namespace FasTnT.Domain.Model.Responses
{
    public class GetVendorVersionResponse : IEpcisResponse
    {
        public string Version { get; set; }

        public void Accept(IEpcisResponseFormatter formatter) => formatter.Accept(this);
    }
}
