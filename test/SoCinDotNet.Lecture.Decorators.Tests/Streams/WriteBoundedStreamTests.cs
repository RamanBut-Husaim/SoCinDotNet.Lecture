using System;
using System.IO;
using FluentAssertions;
using SoCinDotNet.Lecture.Decorators.Streams;
using Xunit;

namespace SoCinDotNet.Lecture.Decorators.Tests.Streams
{
    public sealed class WriteBoundedStreamTests
    {
        [Fact]
        public void Write_WhenFileDoesNotExceedLimit_ShouldReadSuccessfully()
        {
            // arrange
            const int limit = 20 * 1024;

            using (var sourceStream = GetResourceFileStream("CheckFileSize_Payload.json"))
            using (var destinationStream = CreateUnderTest(new MemoryStream(), limit))
            {
                // act
                Action act = () => sourceStream.CopyTo(destinationStream);

                // assert
                act.Should().NotThrow();
            }
        }

        [Fact]
        public void Write_WhenFileExceedsLimit_ShouldFailWithException()
        {
            // arrange
            const int limit = 3 * 1024;

            using (var sourceStream = GetResourceFileStream("CheckFileSize_Payload.json"))
            using (var destinationStream = CreateUnderTest(new MemoryStream(), limit))
            {
                // act
                Action act = () => sourceStream.CopyTo(destinationStream);

                // assert
                act.Should().ThrowExactly<StreamOutOfBoundException>();
            }
        }

        private static Stream GetResourceFileStream(string fileName)
        {
            string fullName = $"{typeof(WriteBoundedStreamTests).Namespace}.{fileName}";

            return typeof(WriteBoundedStreamTests)
                .Assembly
                .GetManifestResourceStream(fullName);
        }

        private static Stream CreateUnderTest(Stream inner, int limit)
        {
            return new WriteBoundedStream(inner, limit);
        }
    }
}
