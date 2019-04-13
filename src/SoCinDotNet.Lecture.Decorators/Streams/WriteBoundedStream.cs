using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SoCinDotNet.Lecture.Decorators.Streams
{
    public sealed class WriteBoundedStream : Stream
    {
        private readonly Stream inner;
        private readonly long sizeLimit;

        private long processedBytes;

        public WriteBoundedStream(Stream inner, long sizeLimit)
        {
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (sizeLimit < 0)
                throw new ArgumentOutOfRangeException(nameof(sizeLimit));

            this.inner = inner;
            this.sizeLimit = sizeLimit;
        }

        public override void Flush() => this.inner.Flush();

        public override int Read(byte[] buffer, int offset, int count) => this.inner.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => this.inner.Seek(offset, origin);

        public override void SetLength(long value) => this.inner.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            this.IncreaseProcessedBytesOrThrow(count);

            this.inner.Write(buffer, offset, count);
        }

        public override bool CanRead => this.inner.CanRead;

        public override bool CanSeek => this.inner.CanSeek;

        public override bool CanWrite => this.inner.CanWrite;

        public override long Length => this.inner.Length;

        public override long Position
        {
            get => this.inner.Position;
            set => this.inner.Position = value;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset <= 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            this.IncreaseProcessedBytesOrThrow(count);

            return this.inner.WriteAsync(buffer, offset, count, cancellationToken);
        }

        private void IncreaseProcessedBytesOrThrow(int count)
        {
            var actual = this.processedBytes + count;

            if (actual > this.sizeLimit)
                throw new StreamOutOfBoundException($"The size of the stream is exceeded, Limit={this.sizeLimit}");

            this.processedBytes += count;
        }
    }
}
