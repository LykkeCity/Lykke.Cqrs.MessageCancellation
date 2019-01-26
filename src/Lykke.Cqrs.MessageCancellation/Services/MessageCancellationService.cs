﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Cqrs.MessageCancellation.Services.Interfaces;

namespace Lykke.Cqrs.MessageCancellation.Services
{
    public class MessageCancellationService : IMessageCancellationService
    {
        private ReaderWriterLockSlim _readWriterLockSlim;
        private HashSet<Guid> _operationsToCancel;

        public MessageCancellationService()
        {
            _readWriterLockSlim = new ReaderWriterLockSlim();
            _operationsToCancel = new HashSet<Guid>();
        }

        public Task RequestMessageCancellationAsync(Guid operationId)
        {
            _readWriterLockSlim.EnterWriteLock();

            try
            {
                _operationsToCancel.Add(operationId);
            }
            finally
            {
                if (_readWriterLockSlim.IsWriteLockHeld)
                    _readWriterLockSlim.ExitWriteLock();
            }

            return Task.FromResult(0);
        }

        public Task RemoveMessageFromCancellationAsync(Guid operationId)
        {
            _readWriterLockSlim.EnterWriteLock();

            try
            {
                _operationsToCancel.Remove(operationId);
            }
            finally
            {
                if (_readWriterLockSlim.IsWriteLockHeld)
                    _readWriterLockSlim.ExitWriteLock();
            }
            return Task.FromResult(0);
        }

        public Task<bool> CheckIfOperationRequiresCancellationAsync(Guid operationId)
        {
            _readWriterLockSlim.EnterReadLock();

            try
            {
                bool operationRequiresCancellation = _operationsToCancel.Contains(operationId);

                return Task.FromResult(operationRequiresCancellation);
            }
            finally
            {
                if (_readWriterLockSlim.IsReadLockHeld)
                    _readWriterLockSlim.ExitReadLock();
            }
        }

        public Task<IEnumerable<Guid>> GetAllMessagesToCancellAsync()
        {
            _readWriterLockSlim.EnterReadLock();

            try
            {
                var enumerator = _operationsToCancel.ToImmutableArray();

                return Task.FromResult((IEnumerable<Guid>)enumerator);
            }
            finally
            {
                if (_readWriterLockSlim.IsReadLockHeld)
                    _readWriterLockSlim.ExitReadLock();
            }
        }
    }
}
