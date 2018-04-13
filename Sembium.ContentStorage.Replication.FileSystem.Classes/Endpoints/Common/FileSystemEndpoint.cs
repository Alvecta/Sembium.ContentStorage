﻿using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Endpoints.Common;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.FileSystem.Endpoints.Common
{
    public abstract class FileSystemEndpoint : IEndpoint
    {
        private readonly string _id;
        private readonly IContainer _container;
        private readonly IHashProvider _hashProvider;
        private readonly IHashStringProvider _hashStringProvider;
        private readonly IContentsMonthHashProvider _contentsMonthHashProvider;

        public string ID
        {
            get { return _id; }
        }

        protected IContainer Container
        {
            get { return _container; }
        }

        public FileSystemEndpoint(string id, 
            IContainer container,
            IHashProvider hashProvider,
            IHashStringProvider hashStringProvider,
            IContentsMonthHashProvider contentsMonthHashProvider)
        {
            _id = id;
            _container = container;
            _hashProvider = hashProvider;
            _hashStringProvider = hashStringProvider;
            _contentsMonthHashProvider = contentsMonthHashProvider;
        }

        public async Task<IEnumerable<IContentIdentifier>> GetContentIdentifiersAsync(DateTimeOffset afterMoment)
        {
            return await Task.Run(() =>
                {
                    return _container.GetChronologicallyOrderedContentIdentifiers(null, afterMoment);
                });
        }

        public async Task<string> GetContentsHashAsync(DateTimeOffset beforeMoment)
        {
            return await Task.Run(() =>
                {
                    var monthHashAndCounts =
                            _contentsMonthHashProvider.GetMonthHashAndCounts(
                                _container.GetChronologicallyOrderedContentIdentifiers(beforeMoment, null)
                            );

                    var hashResult = _hashProvider.GetHashAndCount(monthHashAndCounts);

                    return _hashStringProvider.GetHashString(hashResult.Hash, hashResult.Count);
                });
        }
    }
}