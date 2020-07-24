// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NuGet.Test.Utility
{
    public class V3PackageSearchMetadataFixture : IDisposable
    {
        private bool _disposedValue = false; // To detect redundant calls
        //private readonly TestDirectory _testDirectory;
        public IPackageSearchMetadata TestData { get; private set; }

        public V3PackageSearchMetadataFixture()
        {
            TestData = new MockPackageSearchMetadata()
            {
                VulnerabilityMetadata = new List<IPackageVulnerabilityMetadata>()
                {
                    new MockPackageVulnerabilityMetadata()
                    {
                        AdvisoryUrl = new Uri("https://example/advisory/ABCD-1234-5678-9012")
                    }
                }
            }; 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    //_testDirectory.Dispose();
                }

                TestData = null;
                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        public class MockPackageVulnerabilityMetadata : IPackageVulnerabilityMetadata
        {
            public Uri AdvisoryUrl { get; internal set; }
            public int Severity { get; internal set; }
        }

        public class MockPackageSearchMetadata : IPackageSearchMetadata
        {
            public string Authors => throw new NotImplementedException();

            public IEnumerable<PackageDependencyGroup> DependencySets => throw new NotImplementedException();

            public string Description => throw new NotImplementedException();

            public long? DownloadCount => throw new NotImplementedException();

            public Uri IconUrl => throw new NotImplementedException();

            public PackageIdentity Identity => throw new NotImplementedException();

            public Uri LicenseUrl => throw new NotImplementedException();

            public Uri ProjectUrl => throw new NotImplementedException();

            public Uri ReportAbuseUrl => throw new NotImplementedException();

            public Uri PackageDetailsUrl => throw new NotImplementedException();

            public DateTimeOffset? Published => throw new NotImplementedException();

            public string Owners => throw new NotImplementedException();

            public bool RequireLicenseAcceptance => throw new NotImplementedException();

            public string Summary => throw new NotImplementedException();

            public string Tags => throw new NotImplementedException();

            public string Title => throw new NotImplementedException();

            public bool IsListed => throw new NotImplementedException();

            public bool PrefixReserved => throw new NotImplementedException();

            public LicenseMetadata LicenseMetadata => throw new NotImplementedException();

            public Task<PackageDeprecationMetadata> GetDeprecationMetadataAsync()
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<VersionInfo>> GetVersionsAsync()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IPackageVulnerabilityMetadata> VulnerabilityMetadata { get; internal set; }

            public Task<IEnumerable<IPackageVulnerabilityMetadata>> GetVulnerabilityMetadataAsync() => Task.FromResult(VulnerabilityMetadata);
        }
    }
}
