﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace Dapp.Api.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        private bool disposed;

        private IDeviceRepository deviceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public UnitOfWork(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        /// <inheritdoc />
        public IDeviceRepository DeviceRepository => deviceRepository ?? (deviceRepository = new DeviceRepository(transaction));

        /// <inheritdoc />
        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Dispose();
                transaction = connection.BeginTransaction();
                ResetRepositories();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepositories()
        {
            deviceRepository = null;
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
                disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}