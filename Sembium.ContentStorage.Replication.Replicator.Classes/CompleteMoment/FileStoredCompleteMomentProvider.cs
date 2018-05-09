using Sembium.ContentStorage.Common;
using Sembium.ContentStorage.Replication.Common.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Replication.Replicator.CompleteMoment
{
    class FileStoredCompleteMomentProvider : ICompleteMomentProvider
    {
        private readonly IFileStoredCompleteMomentConfigProvider _fileStoredCompleteMomentConfigProvider;
        private readonly ICompleteMomentInfoFactory _completeMomentInfoFactory;
        private readonly ISerializer _seralizer;

        private IEnumerable<ICompleteMomentInfo> _completeMoments = new List<ICompleteMomentInfo>();
        private bool _loaded;
        private DateTimeOffset _lastSaveMoment;
        private IFileStoredCompleteMomentConfig _fileStoredCompleteMomentConfig;

        public FileStoredCompleteMomentProvider(
            IFileStoredCompleteMomentConfigProvider fileStoredCompleteMomentConfigProvider,
            ICompleteMomentInfoFactory completeMomentInfoFactory,
            ISerializer serializer)
        {
            _fileStoredCompleteMomentConfigProvider = fileStoredCompleteMomentConfigProvider;
            _completeMomentInfoFactory = completeMomentInfoFactory;
            _seralizer = serializer;
        }

        public IFileStoredCompleteMomentConfig FileStoredCompleteMomentConfig
        {
            get
            {
                if (_fileStoredCompleteMomentConfig == null)
                {
                    _fileStoredCompleteMomentConfig = _fileStoredCompleteMomentConfigProvider.GetConfig();
                }

                return _fileStoredCompleteMomentConfig;
            }
        }

        public DateTimeOffset GetCompleteMoment(string sourceID, string destinationID)
        {
            Load();

            return
                _completeMoments
                .Where(x => (x.SourceID == sourceID) && (x.DestinationID == destinationID))
                .Where(x => DateTimeOffset.Now.Subtract(x.OriginMoment).TotalMinutes < FileStoredCompleteMomentConfig.ExpiryMinutes)
                .Select(x => x.Moment)
                .SingleOrDefault();
        }

        public void SetCompleteMoment(string sourceID, string destinationID, DateTimeOffset moment)
        {
            Load();

            var completeMomentInfo = _completeMomentInfoFactory(sourceID, destinationID, moment, DateTimeOffset.Now);

            _completeMoments = 
                _completeMoments.Where(x => (x.SourceID != sourceID) || (x.DestinationID != destinationID))
                .Concat(new[] { completeMomentInfo })
                .ToList();  // to avoid building large expression and Stack overflow exception on evaluation at Save()

            DefferedSave();
        }

        private void Load()
        {
            if (_loaded)
                return;

            var fileName = FileStoredCompleteMomentConfig.FileName;

            if (System.IO.File.Exists(fileName))
            {
                var seralized = System.IO.File.ReadAllText(fileName);
                _completeMoments = _seralizer.Deserialize<IEnumerable<ICompleteMomentInfo>>(seralized);
            }

            _loaded = true;
        }

        private void Save()
        {
            if (!_loaded)
                return;

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FileStoredCompleteMomentConfig.FileName));

            var serialized = _seralizer.Serialize(_completeMoments);
            System.IO.File.WriteAllText(FileStoredCompleteMomentConfig.FileName, serialized);
        }

        private void DefferedSave()
        {
            if (DateTimeOffset.Now.Subtract(_lastSaveMoment).TotalSeconds < 60)
                return;

            Save();
            _lastSaveMoment = DateTimeOffset.Now;
        }

        public void Finish()
        {
            Save();
        }
    }
}
