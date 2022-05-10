using Jering.Javascript.NodeJS;
using System.Numerics;
using Xunit;

namespace Example.BigInt
{
    internal class Program
    {
        static async Task Main(string[] _)
        {
            // Serialize bigints as strings
            const string dummyModule = @"
BigInt.prototype[""toJSON""] = function () {
  return this.toString();
};

module.exports = (callback) => {
    callback(null, [1n, 2n, 3n]);
}";

            // Invoke module
            string[]? invocationResult = await StaticNodeJSService.InvokeFromStringAsync<string[]>(dummyModule);

            // Should not be null
            if (invocationResult == null)
            {
                Console.WriteLine("should not be null");
                return;
            }

            // Convert
            int numBigInts = invocationResult.Length;
            var convertedResult = new BigInteger[numBigInts];
            for(int i = 0; i < numBigInts; i++)
            {
                convertedResult[i] = BigInteger.Parse(invocationResult[i]);
            }

            // Assert
            Assert.Equal(new BigInteger[] {1, 2, 3}, convertedResult);
            Console.WriteLine("success");
        }
    }
}