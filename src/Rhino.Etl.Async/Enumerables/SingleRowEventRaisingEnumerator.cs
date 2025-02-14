using System.Collections;
using Rhino.Etl.Async.Operations;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Enumerables;
using Rhino.Etl.Core.Operations;

namespace Rhino.Etl.Async.Enumerables
{
    /// <summary>
    /// An enumerator that will raise the events on the operation for each iterated item
    /// </summary>
    public class SingleRowEventRaisingAsyncEnumerator : IAsyncEnumerable<Row>, IAsyncEnumerator<Row>
    {
        /// <summary>
        /// Represents the operation on which to raise events
        /// </summary>
        protected readonly IAsyncOperation operation;
        private readonly IAsyncEnumerable<Row> inner;
        private IAsyncEnumerator<Row> innerEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleRowEventRaisingEnumerator"/> class.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="inner">The innerEnumerator.</param>
        public SingleRowEventRaisingAsyncEnumerator(IAsyncOperation operation, IAsyncEnumerable<Row> inner)
        {
            this.operation = operation;
            this.inner = inner;
        }

        ///<summary>
        ///Gets the element in the collection at the current position of the enumerator.
        ///</summary>
        ///
        ///<returns>
        ///The element in the collection at the current position of the enumerator.
        ///</returns>
        ///
        public Row Current
        {
            get { return innerEnumerator.Current; }
        }

        ///<summary>
        ///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public async ValueTask DisposeAsync()
        {
            if (innerEnumerator != null) await innerEnumerator.DisposeAsync();
        }

        ///<summary>
        ///Advances the enumerator to the next element of the collection.
        ///</summary>
        ///
        ///<returns>
        ///true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        ///</returns>
        ///
        ///<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public virtual async ValueTask<bool> MoveNextAsync()
        {
            var result = await innerEnumerator.MoveNextAsync();

            if (result) {
                operation.RaiseRowProcessed(Current);
            }

            return result;
        }

        /////<summary>
        /////Sets the enumerator to its initial position, which is before the first element in the collection.
        /////</summary>
        /////
        /////<exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        //public void Reset()
        //{
        //    innerEnumerator.Reset();
        //}

        /////<summary>
        /////Gets the current element in the collection.
        /////</summary>
        /////
        /////<returns>
        /////The current element in the collection.
        /////</returns>
        /////
        /////<exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.-or- The collection was modified after the enumerator was created.</exception><filterpriority>2</filterpriority>
        //object IAsyncEnumerator<Row>.Current
        //{
        //    get { return innerEnumerator.Current; }
        //}

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IAsyncEnumerator<Row> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            Guard.Against(inner == null, "Null enumerator detected, are you trying to read from the first operation in the process?");
            innerEnumerator = inner!.GetAsyncEnumerator(cancellationToken);
            return this;
        }

        /////<summary>
        /////Returns an enumerator that iterates through a collection.
        /////</summary>
        /////
        /////<returns>
        /////An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /////</returns>
        /////<filterpriority>2</filterpriority>
        //public IEnumerator GetEnumerator()
        //{
        //    return ((IEnumerable<Row>) this).GetEnumerator();
        //}
    }
}
