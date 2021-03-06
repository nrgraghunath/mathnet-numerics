﻿using System;

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    using MathNet.Numerics.LinearAlgebra.Storage.Indexers.Static;
    using MathNet.Numerics.Properties;

    public class DenseColumnMajorSymmetricMatrixStorage<T> : SymmetricMatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        // [ruegg] public fields are OK here

        public readonly T[] Data;

        public readonly PackedStorageIndexerUpper Indexer;

        internal DenseColumnMajorSymmetricMatrixStorage(int order)
            : base(order)
        {
            Indexer = new PackedStorageIndexerUpper(order);
            Data = new T[Indexer.DataLength];
        }

        internal DenseColumnMajorSymmetricMatrixStorage(int order, T[] data)
            : base(order)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Indexer = new PackedStorageIndexerUpper(order);

            if (data.Length != Indexer.DataLength)
            {
                throw new ArgumentOutOfRangeException("data", string.Format(Resources.ArgumentArrayWrongLength, Indexer.DataLength));
            }

            Data = data;
        }

        /// <summary>
        /// Retrieves the requested element without range checking.
        /// </summary>
        /// <param name="row">
        /// The row of the element.
        /// </param>
        /// <param name="column">
        /// The column of the element.
        /// </param>
        /// <returns>
        /// The requested element.
        /// </returns>
        /// <remarks>Not range-checked.</remarks>
        public override T At(int row, int column)
        {
            var r = Math.Min(row, column);
            var c = Math.Max(row, column);
            return Data[Indexer.Of(r, c)];
        }

        /// <summary>
        /// Sets the element without range checking.
        /// </summary>
        /// <param name="row"> The row of the element. </param>
        /// <param name="column"> The column of the element. </param>
        /// <param name="value"> The value to set the element to. </param>
        /// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
        public override void At(int row, int column, T value)
        {
            if (row > column)
            {
                throw new IndexOutOfRangeException("Setting an element in the strictly lower triangle of a symmetric matrix is disabled to avoid errors");
            }

            Data[Indexer.Of(row, column)] = value;
        }

        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }
    }
}
