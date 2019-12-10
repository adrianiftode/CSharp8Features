using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CSharpFeatures
{
    public class CSharp8FeaturesDemo
    {
        #region Readonly members
        public struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public readonly double Distance => Math.Sqrt(X * X + Y * Y);
            public override readonly string ToString() => $"({X}, {Y}) is {Distance} from the origin";
        }
        #endregion

        #region Default Interface Methods

        public interface IFilesStorage
        {
            Task<IReadOnlyCollection<string>> ListFiles();
            Task DeleteFile(string reference);

            #region Default Implementation
            public async Task<int> DeleteAllFiles()
            {
                IEnumerable<string> files = await ListFiles();
                while (files.Any())
                {
                    var batch = files.Take(DeleteBatch);
                    await DoDeleteFiles(batch);
                    files = files.Skip(DeleteBatch);
                }

                return DeleteBatch;
            }

            private async Task DoDeleteFiles(IEnumerable<string> files)
            {
                foreach (var file in files)
                {
                    await DeleteFile(file);
                }
            }

            private static int DeleteBatch { get; set; } = 10;
            public static void SetThresholds(int deleteBatch) => DeleteBatch = deleteBatch;
            #endregion
        }

        public class InMempryFilesStorage : IFilesStorage
        {
            private readonly List<string> _files;
            public InMempryFilesStorage()
            {
                _files = Enumerable.Range(1, 100)
                        .Select(c => Guid.NewGuid().ToString())
                        .ToList();
            }
            public Task DeleteFile(string reference)
            {
                var fileToDelete = _files.First(c => c == reference);
                _files.Remove(fileToDelete);

                return Task.CompletedTask;
            }

            public Task<IReadOnlyCollection<string>> ListFiles()
            {
                IReadOnlyCollection<string> files = _files.ToList();
                return Task.FromResult(files);
            }
        }

        public class FilesTests
        {
            [Fact]
            public async Task GetFiles()
            {
                IFilesStorage filesStorage = new InMempryFilesStorage();

                var batchSize = await filesStorage.DeleteAllFiles();

                var files = await filesStorage.ListFiles();
                Assert.Empty(files);
                Assert.Equal(10, batchSize);
            }

            [Fact]
            public async Task GetFilesParameterized()
            {
                IFilesStorage.SetThresholds(5);
                IFilesStorage filesStorage = new InMempryFilesStorage();

                var batchSize = await filesStorage.DeleteAllFiles();

                var files = await filesStorage.ListFiles();
                Assert.Empty(files);
                Assert.Equal(5, batchSize);
            }

            [Fact]
            public async Task DefaultMethodAvailability()
            {
                IFilesStorage.SetThresholds(5);
                IFilesStorage filesStorage = new InMempryFilesStorage();

                var batchSize = await filesStorage.DeleteAllFiles();

                var files = await filesStorage.ListFiles();
                Assert.Empty(files);
                Assert.Equal(5, batchSize);
            }
        }

        #endregion

        #region Pattern matching
        #region 
        public enum Rainbow
        {
            Red,
            Orange,
            Yellow,
            Green,
            Blue,
            Indigo,
            Violet
        }
        public class RGBColor
        {
            private readonly int _v1;
            private readonly int _v2;
            private readonly int _v3;

            public RGBColor(int v1, int v2, int v3)
            {
                _v1 = v1;
                _v2 = v2;
                _v3 = v3;
            }
        }

        public static RGBColor FromRainbowClassicSwitch(Rainbow colorBand)
        {
            switch (colorBand)
            {
                case Rainbow.Red: return new RGBColor(0xFF, 0x00, 0x00);
                case Rainbow.Orange: return new RGBColor(0xFF, 0x7F, 0x00);
                case Rainbow.Yellow: return new RGBColor(0xFF, 0xFF, 0x00);
                case Rainbow.Green: return new RGBColor(0x00, 0xFF, 0x00);
                case Rainbow.Blue: return new RGBColor(0x00, 0x00, 0xFF);
                case Rainbow.Indigo: return new RGBColor(0x4B, 0x00, 0x82);
                case Rainbow.Violet: return new RGBColor(0x94, 0x00, 0xD3);
                default: throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand));
            }
        }

        // convert statement to expression
        public static RGBColor FromRainbow(Rainbow colorBand)
        {
            switch (colorBand)
            {
                case Rainbow.Red: return new RGBColor(0xFF, 0x00, 0x00);
                case Rainbow.Orange: return new RGBColor(0xFF, 0x7F, 0x00);
                case Rainbow.Yellow: return new RGBColor(0xFF, 0xFF, 0x00);
                case Rainbow.Green: return new RGBColor(0x00, 0xFF, 0x00);
                case Rainbow.Blue: return new RGBColor(0x00, 0x00, 0xFF);
                case Rainbow.Indigo: return new RGBColor(0x4B, 0x00, 0x82);
                case Rainbow.Violet: return new RGBColor(0x94, 0x00, 0xD3);
                default: throw new ArgumentException(message: "invalid enum value", paramName: nameof(colorBand));
            }

            // The variable comes before the switch keyword
            // The case and : elements are replaced with =>
            // The default case is replaced with a _ discard
            // The bodies are expressions, not statements.
        }

        #endregion

        #region Property patterns
        public class Address
        {
            public string County { get; set; }
        }
        public static decimal ComputeSalesTax(Address location, decimal salePrice)
            => location switch
            {
                { County: "Dublin" } => salePrice * 0.75M,
                { County: "Cork" } => salePrice * 0.25M,
                { County: "Limerik" } => salePrice * 0.05M,
                // other cases removed for brevity...
                _ => 0M
            };
        #endregion

        #region Tuple patterns
        public static string RockPaperScissors(string first, string second)
            => (first, second) switch
            {
                ("rock", "paper") => "rock is covered by paper. Paper wins.",
                ("rock", "scissors") => "rock breaks scissors. Rock wins.",
                ("paper", "rock") => "paper covers rock. Paper wins.",
                ("paper", "scissors") => "paper is cut by scissors. Scissors wins.",
                ("scissors", "rock") => "scissors is broken by rock. Rock wins.",
                ("scissors", "paper") => "scissors cuts paper. Scissors wins.",
                (_, _) => "tie"
            };
        #endregion

        #region Positional patterns
        public class QuadrantPoint
        {
            public int X { get; }
            public int Y { get; }

            public QuadrantPoint(int x, int y) => (X, Y) = (x, y);

            public void Deconstruct(out int x, out int y) =>
                (x, y) = (X, Y);
        }
        public enum Quadrant
        {
            Unknown,
            Origin,
            One,
            Two,
            Three,
            Four,
            OnBorder
        }

        static Quadrant GetQuadrant(QuadrantPoint point)
            => point switch
            {
                (0, 0) => Quadrant.Origin,
                var (x, y) when x > 0 && y > 0 => Quadrant.One,
                var (x, y) when x < 0 && y > 0 => Quadrant.Two,
                var (x, y) when x < 0 && y < 0 => Quadrant.Three,
                var (x, y) when x > 0 && y < 0 => Quadrant.Four,
                var (_, _) => Quadrant.OnBorder,
                _ => Quadrant.Unknown
            };

        [Fact]
        public void ThrowsWhenNoMatch()
        {
            var point = new QuadrantPoint(0, 0);

            var result = GetQuadrant(point);

            Assert.Equal(Quadrant.Origin, result); // SwitchExpressionException
        }
        #endregion

        #region more pattern matching

        public class Car
        {
            public int Passengers { get; set; }
        }

        public class DeliveryTruck
        {
            public int GrossWeightClass { get; set; }
        }

        public class Taxi
        {
            public int Fares { get; set; }
        }

        public class Bus
        {
            public int Capacity { get; set; }
            public int Riders { get; set; }
        }

        public decimal CalculateToll(object vehicle)
            => vehicle switch
            {
                Car _ => 2.00m,
                Taxi _ => 3.50m,
                Bus _ => 5.00m,
                DeliveryTruck _ => 10.00m,
                { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };

        // recursive patterns
        // constant patterns in a property pattern
        public decimal ExhaustiveCalculateToll(object vehicle)
            => vehicle switch
            {
                Car { Passengers: 0 } => 2.00m + 0.50m,
                Car { Passengers: 1 } => 2.0m,
                Car { Passengers: 2 } => 2.0m - 0.50m,
                Car _ => 2.00m - 1.0m,

                Taxi { Fares: 0 } => 3.50m + 1.00m,
                Taxi { Fares: 1 } => 3.50m,
                Taxi { Fares: 2 } => 3.50m - 0.50m,
                Taxi _ => 3.50m - 1.00m,

                Bus b when ((double)b.Riders / (double)b.Capacity) < 0.50 => 5.00m + 2.00m,
                Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
                Bus _ => 5.00m,

                DeliveryTruck t when t.GrossWeightClass > 5000 => 10.00m + 5.00m,
                DeliveryTruck t when t.GrossWeightClass < 3000 => 10.00m - 2.00m,
                DeliveryTruck _ => 10.00m,

                { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };
        #region nested switches
        // Car c - type pattern
        // c.Passengers - type pattern that feeds into property pattern
        public decimal CalculateTollNested(object vehicle) =>
            vehicle switch
            {
                Car c => c.Passengers switch
                {
                    0 => 2.00m + 0.5m,
                    1 => 2.0m,
                    2 => 2.0m - 0.5m,
                    _ => 2.00m - 1.0m
                },

                Taxi t => t.Fares switch
                {
                    0 => 3.50m + 1.00m,
                    1 => 3.50m,
                    2 => 3.50m - 0.50m,
                    _ => 3.50m - 1.00m
                },

                Bus b when ((double)b.Riders / (double)b.Capacity) < 0.50 => 5.00m + 2.00m,
                Bus b when ((double)b.Riders / (double)b.Capacity) > 0.90 => 5.00m - 1.00m,
                Bus _ => 5.00m,

                DeliveryTruck t when (t.GrossWeightClass > 5000) => 10.00m + 5.00m,
                DeliveryTruck t when (t.GrossWeightClass < 3000) => 10.00m - 2.00m,
                DeliveryTruck _ => 10.00m,

                { } => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle)),
                null => throw new ArgumentNullException(nameof(vehicle))
            };
        #endregion

        #region use discards for repetitive cases
        //https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/pattern-matching#add-peak-pricing
        #endregion
        #endregion

        #region Disposable ref structs and using enhancements
        ref struct FrameCalculator
        {
            private byte[] _arrayToReturnToPool;
            private Span<byte> _bytes;

            public FrameCalculator(Span<byte> initialBuffer)
            {
                _arrayToReturnToPool = ArrayPool<byte>.Shared.Rent(1024);
                _bytes = initialBuffer;
            }

            public int ComputeRate() => 1;

            public void Dispose() => ArrayPool<byte>.Shared.Return(_arrayToReturnToPool);
        }

        [Fact]
        public void ShouldDisposeARef()
        {
            using var frameCalculator = new FrameCalculator(new byte[] { 1 });

            var rate = frameCalculator.ComputeRate();

            Assert.Equal(1, rate);
        }

        [Fact]
        public void ShouldCallWhenException()
        {
            using var frameCalculator = new FrameCalculator(new byte[] { 1 });

            var rate = frameCalculator.ComputeRate();

            Assert.Equal(2, rate);
        }

        #endregion

        #region nullable reference types
        [Fact]
        public void Nullables()
        {
            var name = "John";
            var address = "Wallstreet";

            //address = null;

            var customer = PrintCustomer(name, address);

            Assert.NotNull(customer);
        }
        public static string PrintCustomer(string name, string? address) => name.ToUpper() + " " + address.ToUpper();

#nullable disable
        public static string PrintCustomerEnabledDisabled(string name, string? address) => name.ToUpper() + " " + address.ToUpper();
#nullable restore

        #endregion

        #region Asynchronous streams
        public static async IAsyncEnumerable<int> GenerateSequence()
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(100);
                yield return i;
            }
        }

        [Fact]
        public async Task ConsumeAsyncStream()
        {
            await foreach (var number in GenerateSequence())
            {
                Console.WriteLine(number);
            }

            // configurare await is also available
            await foreach (var number in GenerateSequence().ConfigureAwait(false))
            {
                Console.WriteLine(number);
            }
        }

        public class MyOwnAsyncStream<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>
        {
            public T Current => default;

            public ValueTask DisposeAsync() => default;

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => this;

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(false);
        }

        [Fact]
        public async Task ConsumingMyOwnAsyncStream()
        {
            await foreach (var item in new MyOwnAsyncStream<object>())
            {
            }
        }
        #endregion

        #region indices & ranges
        [Fact]
        public void Indices()
        {
            var words = new string[]
{
                            // index from start    index from end
                "The",      // 0                   ^9
                "quick",    // 1                   ^8
                "brown",    // 2                   ^7
                "fox",      // 3                   ^6
                "jumped",   // 4                   ^5
                "over",     // 5                   ^4
                "the",      // 6                   ^3
                "lazy",     // 7                   ^2
                "dog"       // 8                   ^1
            };              // 9 (or words.Length) ^0

            Assert.Equal("dog", words[^1]);

            var subset = words[1..4];
            Assert.Equal("quick", subset[0]);
            Assert.Equal("brown", subset[1]);
            Assert.Equal("fox", subset[2]);

            var lazyDog = words[^2..^0];
            Assert.Equal("lazy", lazyDog[0]);
            Assert.Equal("dog", lazyDog[1]);

            var allWords = words[..];
            Assert.Equal("The", allWords[0]);
            Assert.Equal("dog", allWords[8]);

            var firstPhrase = words[..4];
            Assert.Equal("The", firstPhrase[0]);
            Assert.Equal("fox", firstPhrase[3]);

            var lastPhrase = words[6..];
            Assert.Equal("the", lastPhrase[0]);
            Assert.Equal("dog", lastPhrase[2]);

            //Assert.Equal("dog", words[^0]);

            Range phrase = 1..4;
            var text = words[phrase];

            Range fromCtor = new Range(1, 4);
            var fromCtorText = words[fromCtor];
        }
        #endregion

        #region null coalescing assignment
        [Fact]
        public void NullCoalescingAssignment()
        {
            List<int> numbers = null;
            int? i = null;

            numbers ??= new List<int>();
            numbers.Add(i ??= 17);
            numbers.Add(i ??= 20);


            Assert.Equal(17, i);
            Assert.Equal("17 17", string.Join(" ", numbers));

            //d ??= (e ??= f)
        }

        public static int? SometimesReturnsANull() => default;

        [Fact]
        public void CheckingNullAssignment()
        {
            var x = SometimesReturnsANull();
            x ??= 2;

            Assert.Equal(2, x);
        }
        #endregion

        #region Unmanaged constructed types
        public struct Coords<T>
        {
            public T X;
            public T Y;
        }

        [Fact]
        public void AllocateAnArrayOfSomeValueTypesOnStack()
        {
            // Coords is now considered an unmanaged type, as ints, because is constrcuted from other unmanaged types
            //A type is an unmanaged type if it's any of the following types:
            //sbyte, byte, short, ushort, int, uint, long, ulong, char, float, double, decimal, or bool
            //Any enum type
            //Any pointer type

            Span<Coords<int>> coordinates = stackalloc[]
            {
                new Coords<int> { X = 0, Y = 0 },
                new Coords<int> { X = 0, Y = 3 },
                new Coords<int> { X = 4, Y = 0 }
            };

            Assert.Equal(3, coordinates.Length);

            //also in nested

            Span<int> numbers = stackalloc[] { 1, 2, 3, 4, 5, 6 };
            var ind = numbers.IndexOfAny(stackalloc[] { 2, 4, 6, 8 });

            Assert.Equal(1, ind);
        }
        #endregion

        #region IAsyncDisposable
        public class AsyncDispose : IAsyncDisposable
        {
            public Task DoSomethingAboutIt() => Task.CompletedTask;
            public ValueTask DisposeAsync() => new ValueTask();
        }

        [Fact]
        public async Task ConsumingDisposableAsync()
        {
            await using AsyncDispose sut = new AsyncDispose();

            await sut.DoSomethingAboutIt();
        }

        #endregion

        #endregion
    }
}
