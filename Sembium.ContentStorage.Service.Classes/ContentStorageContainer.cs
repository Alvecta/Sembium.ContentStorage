using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Service.Hosting;
using Sembium.ContentStorage.Service.Results;
using Sembium.ContentStorage.Service.Security;
using Sembium.ContentStorage.Service.ServiceResults;
using Sembium.ContentStorage.Service.ServiceResults.Tools;
using Sembium.ContentStorage.Storage.Common;
using Sembium.ContentStorage.Storage.Common.Factories;
using Sembium.ContentStorage.Storage.Hosting;
using Sembium.ContentStorage.Storage.HostingResults;
using Sembium.ContentStorage.Storage.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Service
{
    public class ContentStorageContainer : IContentStorageContainer
    {
        private const string ReadOnlySubcontainersContentName = "subcontainers.json";
        private const string URLExpirySecondsAppSettingName = "URLExpirySeconds";
        private const int DefaultURLExpirySeconds = 10 * 60;
        private const string ReadOnlySubcontainersContainerName = "subcontainers";
        private readonly string _containerName;
        private readonly IContentStorageHost _contentStorageHost;
        private readonly IContentNameProvider _contentNameProvider;
        private readonly IConfigurationSettings _configurationSettings;
        private readonly IContentIdentifierGenerator _contentIdentifierGenerator;
        private readonly IDocumentIDUploadInfoProvider _documentIDUploadInfoProvider;
        private readonly IDocumentMultiPartIDUploadInfoProvider _documentMultiPartIDUploadInfoProvider;
        private readonly IContentIdentifierSerializer _contentIdentifierSerializer;
        private readonly IDocumentIdentifierSerializer _documentIdentifierSerializer;
        private readonly IDocumentIdentifierProvider _documentIdentifierProvider;
        private readonly IUploadIdentifierSerializer _uploadIdentifierSerializer;
        private readonly IUploadIdentifierProvider _uploadIdentifierProvider;
        private readonly IDocumentUploadInfoFactory _documentUploadInfoFactory;
        private readonly IDocumentMultiPartUploadInfoFactory _documentMultiPartUploadInfoFactory;
        private readonly IIDUploadInfoProvider _idUploadInfoProvider;
        private readonly IHashProvider _hashProvider;
        private readonly IHashStringProvider _hashStringProvider;
        private readonly ISerializer _serializer;
        private readonly IMultiPartIDUploadInfoProvider _multiPartIDUploadInfoProvider;
        private readonly IUploadInfoFactory _uploadInfoFactory;
        private readonly IContainerStateRepository _containerStateRepository;
        private readonly ISystemContainerProvider _systemContainerProvider;
        private readonly IAuthorizationChecker _authorizationChecker;
        private readonly IContentNamesRepository _committedContentNamesRepository;
        private readonly IContentMonthProvider _contentMonthProvider;
        private readonly IContentsMonthHashProvider _contentsMonthHashProvider;
        private readonly IContentsMonthHashRepository _contentsMonthHashRepository;
        private readonly IContentIdentifiersProvider _contentIdentifiersProvider;

        public ContentStorageContainer(string containerName,   
            IContentStorageHost contentStorageHost, 
            IContentNameProvider contentNameProvider, 
            IConfigurationSettings configurationSettings, 
            IContentIdentifierGenerator contentIdentifierGenerator,
            IDocumentIDUploadInfoProvider documentIDUploadInfoProvider,
            IDocumentMultiPartIDUploadInfoProvider documentMultiPartIDUploadInfoProvider,
            IContentIdentifierSerializer contentIdentifierSerializer,
            IDocumentIdentifierSerializer documentIdentifierSerializer,
            IDocumentIdentifierProvider documentIdentifierProvider,
            IUploadIdentifierSerializer uploadIdentifierSerializer,
            IUploadIdentifierProvider uploadIdentifierProvider,
            IDocumentUploadInfoFactory documentUploadInfoFactory,
            IDocumentMultiPartUploadInfoFactory documentMultiPartUploadInfoFactory,
            IIDUploadInfoProvider idUploadInfoProvider,
            IHashProvider hashProvider,
            IHashStringProvider hashStringProvider,
            ISerializer serializer,
            IMultiPartIDUploadInfoProvider multiPartIDUploadInfoProvider,
            IUploadInfoFactory uploadInfoFactory,
            IContainerStateRepository containerStateRepository,
            ISystemContainerProvider systemContainerProvider,
            IAuthorizationChecker authorizationChecker,
            IContentNamesRepository committedContentNamesRepository,
            IContentMonthProvider contentMonthProvider,
            IContentsMonthHashProvider contentsMonthHashProvider,
            IContentsMonthHashRepository contentsMonthHashRepository,
            IContentIdentifiersProvider contentIdentifiersProvider)
        {
            _containerName = containerName;

            _contentStorageHost = contentStorageHost;
            _contentNameProvider = contentNameProvider;
            _configurationSettings = configurationSettings;
            _contentIdentifierGenerator = contentIdentifierGenerator;
            _documentIDUploadInfoProvider = documentIDUploadInfoProvider;
            _documentMultiPartIDUploadInfoProvider = documentMultiPartIDUploadInfoProvider;
            _contentIdentifierSerializer = contentIdentifierSerializer;
            _documentIdentifierSerializer = documentIdentifierSerializer;
            _documentIdentifierProvider = documentIdentifierProvider;
            _uploadIdentifierSerializer = uploadIdentifierSerializer;
            _uploadIdentifierProvider = uploadIdentifierProvider;
            _documentUploadInfoFactory = documentUploadInfoFactory;
            _documentMultiPartUploadInfoFactory = documentMultiPartUploadInfoFactory;
            _idUploadInfoProvider = idUploadInfoProvider;
            _hashProvider = hashProvider;
            _hashStringProvider = hashStringProvider;
            _serializer = serializer;
            _multiPartIDUploadInfoProvider = multiPartIDUploadInfoProvider;
            _uploadInfoFactory = uploadInfoFactory;
            _containerStateRepository = containerStateRepository;
            _systemContainerProvider = systemContainerProvider;
            _authorizationChecker = authorizationChecker;
            _committedContentNamesRepository = committedContentNamesRepository;
            _contentMonthProvider = contentMonthProvider;
            _contentsMonthHashProvider = contentsMonthHashProvider;
            _contentsMonthHashRepository = contentsMonthHashRepository;
            _contentIdentifiersProvider = contentIdentifiersProvider;
        }

        private int URLExpirySeconds
        {
            get
            {
                int result;
                if (int.TryParse(_configurationSettings.GetAppSetting(URLExpirySecondsAppSettingName), out result))
                    return result;
                else
                    return DefaultURLExpirySeconds;
            }
        }

        private IContainer GetContainer()
        {
            return GetContainer(_containerName);
        }

        private IContainer GetContainer(string containerName)
        {
            return _contentStorageHost.GetContainer(containerName);
        }

        private IContentIdentifier GenerateUniqueContentIdentifier(IContainer container, string hash, string extention)
        {
            var result =
                Enumerable.Range(1, 5)
                .Select(x => _contentIdentifierGenerator.GenerateContentIdentifier(hash, extention))
                .Where(x => (!container.ContentExists(x)) && (!container.ContentExists(_contentIdentifierGenerator.GetCommittedContentIdentifier(x))))
                .FirstOrDefault();

            if (result == null)
                throw new UserException("Could not generate unique content name");

            return result;
        }

        private IDocumentIdentifier GetExistingDocumentIdentifier(IContainer container, string hash, string extension)
        {
            return
                _contentIdentifiersProvider.GetContentIdentifiers(container, true, hash)
                .Where(x => string.Equals(x.Extension, extension, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => _documentIdentifierProvider.GetDocumentIdentifier(x))
                .FirstOrDefault();
        }

        public IDocumentMultiPartUploadInfo GetMultiPartUploadInfoOrDocumentIdentifier(string hash, string extention, long size)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Operator);

            var container = GetContainer();

            var existingDocumentIdentifier = GetExistingDocumentIdentifier(container, hash, extention);

            if (existingDocumentIdentifier != null)
                return _documentMultiPartUploadInfoFactory(null, existingDocumentIdentifier);

            CheckContainerState(true);

            var newContentIdentifier = GenerateUniqueContentIdentifier(container, hash, extention);

            var content = container.CreateContent(newContentIdentifier) as IURLContent;

            var multiPartUploadInfo = content.GetMultiPartUploadInfo(URLExpirySeconds, size);

            return _documentMultiPartUploadInfoFactory(multiPartUploadInfo, null);
        }

        private void CheckContainerState(bool checkReadOnly)
        {
            var containerState = _containerStateRepository.GetContainerStateAsync(_containerName).Result;

            if (checkReadOnly && (containerState != null) && (containerState.IsReadOnly))
            {
                throw new UserException("Operation not allowed in ReadOnly mode");
            }

            if ((containerState != null) && (containerState.IsMaintained))
            {
                throw new UserException("Operation not allowed during Maintainance");
            }
        }

        public IDocumentMultiPartIDUploadInfo GetMultiPartIDUploadInfoOrDocumentID(string hash, string extention, long size)
        {
            var documentMultiPartUploadInfo = GetMultiPartUploadInfoOrDocumentIdentifier(hash, extention, size);
            return _documentMultiPartIDUploadInfoProvider.GetDocumentMultiPartIDUploadInfo(documentMultiPartUploadInfo);
        }

        public IMultiPartUploadInfo GetMultiPartUploadInfo(IContentIdentifier contentIdentifier, long size)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator);

            var container = GetContainer();

            if (container.ContentExists(contentIdentifier))
                return null;

            CheckContainerState(false);

            var uncommittedContentIdentifier = _contentIdentifierGenerator.GetUncommittedContentIdentifier(contentIdentifier);

            var content = (container.ContentExists(uncommittedContentIdentifier) ? container.GetContent(uncommittedContentIdentifier) : container.CreateContent(uncommittedContentIdentifier)) as IURLContent;

            return content.GetMultiPartUploadInfo(URLExpirySeconds, size);
        }

        public IMultiPartIDUploadInfo GetMultiPartIDUploadInfo(string contentID, long size)
        {
            var contentIdentifier = _contentIdentifierSerializer.Deserialize(contentID);
            var multiPartuploadInfo = GetMultiPartUploadInfo(contentIdentifier, size);
            return _multiPartIDUploadInfoProvider.GetMultiPartIDUploadInfo(multiPartuploadInfo);
        }

        public async Task<IDocumentIdentifier> CommitMultiPartUploadAsync(IUploadIdentifier uploadIdentifier, IEnumerable<KeyValuePair<string, string>> partUploadResults)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Operator, Security.Roles.Replicator);

            var container = GetContainer();

            var multiPartUploadContainerContainer = container as IMultiPartUploadContainer;
            if (multiPartUploadContainerContainer != null)
            {
                multiPartUploadContainerContainer.FinalizeMultiPartUpload(uploadIdentifier.HostIdentifier, partUploadResults);
            }

            var uncommittedContentIdentifier = _uploadIdentifierProvider.GetUncommittedContentIdentifier(container, uploadIdentifier);
            var contentIdentifier = await container.CommitContentAsync(uncommittedContentIdentifier);

            var contentName = _contentNameProvider.GetContentName(contentIdentifier);
            await PersistCommittedContentNameAsync(contentName, contentIdentifier.ModifiedMoment);

            return _documentIdentifierProvider.GetDocumentIdentifier(contentIdentifier);
        }

        public async Task<string> CommitMultiPartUploadAsync(string uploadID, IEnumerable<KeyValuePair<string, string>> partUploadResults)
        {
            var uploadIdentifier = _uploadIdentifierSerializer.Deserialize(uploadID);
            var documentIdentifier = await CommitMultiPartUploadAsync(uploadIdentifier, partUploadResults);
            return _documentIdentifierSerializer.Serialize(documentIdentifier);
        }

        private IDownloadInfo GetContainerDocumentDownloadInfo(IDocumentIdentifier documentIdentifier, string containerName)
        {
            var container = GetContainer(containerName);

            var contentIdentifier = _documentIdentifierProvider.GetContentIdentifier(container, documentIdentifier);

            if (container.ContentExists(contentIdentifier))
            {
                var content = container.GetContent(contentIdentifier) as IURLContent;
                return content.GetDownloadInfo(URLExpirySeconds);
            }

            return null;
        }

        public IDownloadInfo GetDocumentDownloadInfo(IDocumentIdentifier documentIdentifier)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Operator);

            var result = GetContainerDocumentDownloadInfo(documentIdentifier, _containerName);

            if (result == null)
            {
                var readOnlySubcontainerNames = InternalGetReadOnlySubcontainerNames();
                result =
                    readOnlySubcontainerNames
                    .Select(x => GetContainerDocumentDownloadInfo(documentIdentifier, x))
                    .Where(x => x != null)
                    .FirstOrDefault();
            }

            if (result == null)
            {
                throw new ApplicationException("Document does not exist");
            }

            return result;
        }

        public IDownloadInfo GetDocumentDownloadInfo(string documentID)
        {
            var documentIdentifier = _documentIdentifierSerializer.Deserialize(documentID);
            return GetDocumentDownloadInfo(documentIdentifier);
        }

        private IDownloadInfo GetContainerContentDownloadInfo(IContentIdentifier contentIdentifier, string containerName)
        {
            var container = GetContainer(containerName);

            if (container.ContentExists(contentIdentifier))
            {
                var content = container.GetContent(contentIdentifier) as IURLContent;
                return content.GetDownloadInfo(URLExpirySeconds);
            }

            return null;
        }

        public IDownloadInfo GetContentDownloadInfo(IContentIdentifier contentIdentifier)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Backup);

            var result = GetContainerContentDownloadInfo(contentIdentifier, _containerName);

            if (result == null)
            {
                var readOnlySubcontainerNames = InternalGetReadOnlySubcontainerNames();
                result =
                    readOnlySubcontainerNames
                    .Select(x => GetContainerContentDownloadInfo(contentIdentifier, x))
                    .Where(x => x != null)
                    .FirstOrDefault();
            }

            if (result == null)
            {
                throw new ApplicationException("Content does not exist");
            }

            return result;
        }

        public IDownloadInfo GetContentDownloadInfo(string contentID)
        {
            var contentIdentifier = _contentIdentifierSerializer.Deserialize(contentID);
            return GetContentDownloadInfo(contentIdentifier);
        }

        public IEnumerable<IContentIdentifier> GetContentIdentifiers()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Backup);

            return GetChronologicallyOrderedContentIdentifiers(null, null);
        }

        private IEnumerable<IContentIdentifier> GetChronologicallyOrderedContentIdentifiers(DateTimeOffset? beforeMoment, DateTimeOffset? afterMoment)
        {
            var container = GetContainer();

            var contentNames = 
                    _committedContentNamesRepository.GetChronologicallyOrderedContentNames(
                        _containerName, 
                        GetMomentMonth(beforeMoment, 1), 
                        GetMomentMonth(afterMoment, -1), 
                        CancellationToken.None);

            var result =
                    _contentIdentifiersProvider.GetContentIdentifiers(contentNames)
                    .Where(x => !_contentIdentifierGenerator.IsSystemContent(x));

            if (beforeMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment < beforeMoment.Value);
            }

            if (afterMoment.HasValue)
            {
                result = result.Where(x => x.ModifiedMoment > afterMoment.Value);
            }

            return result;
        }

        private DateTimeOffset? GetMomentMonth(DateTimeOffset? moment, int monthsOffset = 0)
        {
            if (moment.HasValue)
            {
                return new DateTimeOffset(moment.Value.ToUniversalTime().Year, moment.Value.ToUniversalTime().Month, 1, 0, 0, 0, TimeSpan.FromHours(0)).AddMonths(monthsOffset);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<string> GetContentIDs(DateTimeOffset afterMoment)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Backup);

            return
                GetChronologicallyOrderedContentIdentifiers(null, afterMoment)                
                .Select(x => _contentIdentifierSerializer.Serialize(x));
        }

        public string GetContentsHash(DateTimeOffset beforeMoment)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Backup);

            var container = GetContainer();

            var hashResult = GetContainerMonthsHash(container, beforeMoment);

            return _hashStringProvider.GetHashString(hashResult.Hash, hashResult.Count);
        }

        public void AddReadOnlySubcontainer(string subcontainerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            var container = GetContainer();

            var subcontainerNames = LoadReadOnlySubcontainerNames(container);

            if (subcontainerNames.Contains(subcontainerName))
            {
                throw new UserException("Subcontainer already exists");
            }

            var newSubcontainerNames = subcontainerNames.Union(new[] { subcontainerName });

            SaveReadOnlySubcontainerNames(container, newSubcontainerNames);
        }

        public void RemoveReadOnlySubcontainer(string subcontainerName)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            var container = GetContainer();

            var subcontainerNames = LoadReadOnlySubcontainerNames(container);

            if (!subcontainerNames.Contains(subcontainerName))
            {
                throw new UserException("Subcontainer not found");
            }

            var newSubcontainerNames = subcontainerNames.Except(new[] { subcontainerName });

            SaveReadOnlySubcontainerNames(container, newSubcontainerNames);
        }

        public IEnumerable<string> GetReadOnlySubcontainerNames()
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            return InternalGetReadOnlySubcontainerNames();
        }

        private IEnumerable<string> InternalGetReadOnlySubcontainerNames()
        {
            var container = GetContainer();
            return LoadReadOnlySubcontainerNames(container);
        }

        private string GetReadOnlySubcontainersContentName()
        {
            return _containerName + "-" + ReadOnlySubcontainersContentName;
        }

        private IEnumerable<string> LoadReadOnlySubcontainerNames(IContainer container)
        {
            var readOnlySubcontainersContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(GetReadOnlySubcontainersContentName());
            var namesContainer = _systemContainerProvider.GetSystemContainer(ReadOnlySubcontainersContainerName);
            var serializedReadOnlySubcontainerNames = namesContainer.GetStringContent(readOnlySubcontainersContentIdentifier);

            if (string.IsNullOrEmpty(serializedReadOnlySubcontainerNames))
            {
                return Enumerable.Empty<string>();
            }

            return _serializer.Deserialize<IEnumerable<string>>(serializedReadOnlySubcontainerNames);
        }
        
        private void SaveReadOnlySubcontainerNames(IContainer container, IEnumerable<string> subcontainerNames)
        {
            var serializedReadOnlySubcontainerNames = subcontainerNames.Any() ? _serializer.Serialize(subcontainerNames) : null;

            var readOnlySubcontainersContentIdentifier = _contentIdentifierGenerator.GetSystemContentIdentifier(GetReadOnlySubcontainersContentName());

            var namesContainer = _systemContainerProvider.GetSystemContainer(ReadOnlySubcontainersContainerName);

            namesContainer.SetStringContent(readOnlySubcontainersContentIdentifier, serializedReadOnlySubcontainerNames);
        }

        public IDocumentUploadInfo GetUploadInfoOrDocumentIdentifier(string hash, string extention)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Operator);

            var result = GetMultiPartUploadInfoOrDocumentIdentifier(hash, extention, -1);

            if (result == null)
            {
                return null;
            }

            if ((result.DocumentIdentifier == null) &&
                (result.MultiPartUploadInfo?.PartUploadInfos?.Count() != 1))
            {
                throw new UserException("Multipart upload not supported");
            }

            var partUploadInfo = result?.MultiPartUploadInfo?.PartUploadInfos?.FirstOrDefault();

            var uploadInfo = 
                    partUploadInfo == null ? 
                    null :
                    _uploadInfoFactory(partUploadInfo.URL, result.MultiPartUploadInfo.HttpMethod, partUploadInfo.RequestHttpHeaders, result.MultiPartUploadInfo.UploadIdentifier);

            return _documentUploadInfoFactory(uploadInfo, result.DocumentIdentifier);
        }

        public IDocumentIDUploadInfo GetIDUploadInfoOrDocumentID(string hash, string extention)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator);

            var documentUploadInfo = GetUploadInfoOrDocumentIdentifier(hash, extention);
            return _documentIDUploadInfoProvider.GetDocumentIDUploadInfo(documentUploadInfo);
        }

        public IUploadInfo GetUploadInfo(IContentIdentifier contentIdentifier)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator);

            var result = GetMultiPartUploadInfo(contentIdentifier, -1);

            if (result == null)
            {
                return null;
            }

            if (result?.PartUploadInfos?.Count() != 1)
            {
                throw new UserException("Multipart upload not supported");
            }

            var partUploadInfo = result?.PartUploadInfos?.FirstOrDefault();

            var uploadInfo = 
                    partUploadInfo == null ? 
                    null :
                    _uploadInfoFactory(partUploadInfo.URL, result.HttpMethod, partUploadInfo.RequestHttpHeaders, result.UploadIdentifier);

            return uploadInfo;
        }

        public IIDUploadInfo GetIDUploadInfo(string contentID)
        {
            var contentIdentifier = _contentIdentifierSerializer.Deserialize(contentID);
            var uploadInfo = GetUploadInfo(contentIdentifier);
            return _idUploadInfoProvider.GetIDUploadInfo(uploadInfo);
        }

        public Task<IDocumentIdentifier> CommitUploadAsync(IUploadIdentifier uploadIdentifier)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Operator);

            return CommitMultiPartUploadAsync(uploadIdentifier, null);
        }

        public Task<string> CommitUploadAsync(string uploadID)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Replicator, Security.Roles.Operator);

            return CommitMultiPartUploadAsync(uploadID, null);
        }

        public string GetDocumentDownloadURL(IDocumentIdentifier documentIdentifier)
        {
            return GetDocumentDownloadInfo(documentIdentifier).Url;
        }

        public string GetDocumentDownloadURL(string documentID)
        {
            return GetDocumentDownloadInfo(documentID)?.Url;
        }

        public string GetContentDownloadURL(IContentIdentifier contentIdentifier)
        {
            return GetContentDownloadInfo(contentIdentifier)?.Url;
        }

        public string GetContentDownloadURL(string contentID)
        {
            return GetContentDownloadInfo(contentID).Url;
        }

        public async Task<string> MaintainAsync(CancellationToken cancellationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var count = await SafeMaintainAsync(cancellationToken);

            watch.Stop();

            return $"Persisted {count} committed content names in the {_containerName} container. Elapsed time: {watch.Elapsed.ToString()}";
        }

        private async Task<int> SafeMaintainAsync(CancellationToken cancellationToken)
        {
            CheckContainerState(false);

            await _containerStateRepository.SetContainerStateAsync(_containerName, null, true);
            try
            {
                var container = GetContainer();
                return await MaintainContainerAsync(container, cancellationToken);
            }
            finally
            {
                await _containerStateRepository.SetContainerStateAsync(_containerName, null, false);
            }
        }

        private async Task<int> MaintainContainerAsync(IContainer container, CancellationToken cancellationToken)
        {
            var persistedCommittedContentNames =
                    _committedContentNamesRepository
                    .GetChronologicallyOrderedContentNames(_containerName, null, null, cancellationToken);

            var notPersistedContentNames =
                    container.GetContentNames(null)
                    .Except(persistedCommittedContentNames.OrderBy(x => x));

            var notPersistedCommittedContents =
                    _contentIdentifiersProvider.GetContentIdentifiers(notPersistedContentNames)
                    .Where(x => !x.Uncommitted)
                    .OrderBy(x => x.ModifiedMoment)
                    .ThenBy(x => x.Guid)
                    .Select(x => new KeyValuePair<string, DateTimeOffset>(_contentNameProvider.GetContentName(x), x.ModifiedMoment));

            var result = _committedContentNamesRepository.AddContents(_containerName, notPersistedCommittedContents, cancellationToken);

            return await Task.FromResult(result);
        }

        private async Task PersistCommittedContentNameAsync(string contentName, DateTimeOffset contentDate)
        {
            await Task.Run(() =>
            {
                _committedContentNamesRepository.AddContent(_containerName, contentName.ToLowerInvariant(), contentDate, CancellationToken.None);
            });
        }

        private (byte[] Hash, int Count) GetContainerMonthsHash(IContainer container, DateTimeOffset beforeMoment)
        {
            var beforeMomentMonth = _contentMonthProvider.GetContentMonth(beforeMoment);

            var persistedMonthHashAndCounts =
                    _contentsMonthHashRepository.GetMonthHashAndCounts(_containerName)
                    .OrderBy(x => x.Month)
                    .ToList();

            var persistedPastMonthHashAndCounts =
                    persistedMonthHashAndCounts
                    .Where(x => x.Month < beforeMomentMonth)
                    .ToList();

            var lastPersistedPastMonth = persistedPastMonthHashAndCounts.Select(x => (DateTimeOffset?)x.Month).LastOrDefault();

            var nextContentIdentifiers = GetChronologicallyOrderedContentIdentifiers(beforeMoment, lastPersistedPastMonth?.AddMonths(1).AddTicks(-1));
            var nextMonthHashAndCounts = _contentsMonthHashProvider.GetMonthHashAndCounts(nextContentIdentifiers).ToList();

            AddMissingMonthHashAndCounts(nextMonthHashAndCounts);

            var allMonthHashAndCounts = persistedPastMonthHashAndCounts.Concat(nextMonthHashAndCounts);

            return _hashProvider.GetHashAndCount(allMonthHashAndCounts);
        }

        private void AddMissingMonthHashAndCounts(IEnumerable<IMonthHashAndCount> monthHashAndCounts)
        {
            var missingMonthHashAndCounts =
                    monthHashAndCounts
                    .Where(x =>
                        monthHashAndCounts
                        .Any(y => (y.Month > x.Month) && (y.LastModifiedMoment > x.Month.AddMonths(1).AddDays(15)))
                    );
            _contentsMonthHashRepository.AddMonthHashAndCounts(_containerName, missingMonthHashAndCounts);
        }

        public async Task CompactContentNamesAsync(CancellationToken cancellationToken)
        {
            _authorizationChecker.CheckUserIsInRole(Security.Roles.Admin);
            await _committedContentNamesRepository.CompactAsync(_containerName, cancellationToken);
        }
    }
}
