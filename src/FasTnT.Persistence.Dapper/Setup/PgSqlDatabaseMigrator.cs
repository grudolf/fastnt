﻿using FasTnT.Domain.Services.Setup;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FasTnT.Persistence.Dapper.Setup
{
    public class PgSqlDatabaseMigrator : IDatabaseMigrator
    {
        private readonly IUnitOfWork _unitOfWork;

        public PgSqlDatabaseMigrator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Migrate()
        {
            using (var msi = new MemoryStream(Convert.FromBase64String(SqlRequests.CreateDatabaseZipped)))
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                using (var sr = new StreamReader(gs))
                {
                    await _unitOfWork.Execute(await sr.ReadToEndAsync(), null);
                }
            }
        }

        public async Task Rollback()
        {
            using (var msi = new MemoryStream(Convert.FromBase64String(SqlRequests.DropDatabaseZipped)))
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                using (var sr = new StreamReader(gs))
                {
                    await _unitOfWork.Execute(await sr.ReadToEndAsync(), null);
                }
            }
        }
    }
}
