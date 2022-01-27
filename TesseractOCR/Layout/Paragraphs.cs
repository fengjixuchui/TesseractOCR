﻿//
// Paragraphs.cs
//
// Author: Kees van Spelde <sicos2002@hotmail.com>
//
// Copyright 2021-2022 Kees van Spelde
//
// Licensed under the Apache License, Version 2.0 (the "License");
//
// - You may not use this file except in compliance with the License.
// - You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TesseractOCR.Enums;
using TesseractOCR.Interop;

namespace TesseractOCR.Layout
{
    /// <summary>
    ///     All the <see cref="Paragraphs"/> in the <see cref="Block"/>
    /// </summary>
    public sealed class Paragraphs : IEnumerable<Paragraph>
    {
        #region Fields
        /// <summary>
        ///     Handle that is returned by TessApi.Native.BaseApiGetIterator
        /// </summary>
        private readonly HandleRef _iteratorHandle;
        #endregion

        #region GetEnumerator
        /// <inheritdoc />
        public IEnumerator<Paragraph> GetEnumerator()
        {
            return new Paragraph(_iteratorHandle);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Constructor
        internal Paragraphs(HandleRef iteratorHandle)
        {
            _iteratorHandle = iteratorHandle;
        }
        #endregion
    }

    /// <summary>
    ///     A single <see cref="Paragraph"/> in the <see cref="Block"/>
    /// </summary>
    public sealed class Paragraph : IEnumerator<Paragraph>
    {
        #region Fields
        /// <summary>
        ///     Handle that is returned by TessApi.Native.BaseApiGetIterator
        /// </summary>
        private readonly HandleRef _iteratorHandle;
        #endregion

        #region Properties
        public object Current => this;

        /// <summary>
        ///     Returns the current element
        /// </summary>
        Paragraph IEnumerator<Paragraph>.Current => this;

        /// <summary>
        ///     All the available <see cref="Lines"/> in this <see cref="Paragraph"/>
        /// </summary>
        public IEnumerable<TextLines> Lines { get; }
        #endregion

        #region Constructor
        internal Paragraph(HandleRef iteratorHandle)
        {
            _iteratorHandle = iteratorHandle;
        }
        #endregion

        #region MoveNext
        /// <summary>
        ///     Moves to the next <see cref="Paragraph"/> in the <see cref="Block"/>
        /// </summary>
        /// <returns><c>true</c> when there is a next <see cref="Line"/>, otherwise <c>false</c></returns>
        public bool MoveNext()
        {
            if (TessApi.Native.PageIteratorIsAtBeginningOf(_iteratorHandle, PageIteratorLevel.Paragraph) != 0)
                return true;

            return TessApi.Native.PageIteratorNext(_iteratorHandle, PageIteratorLevel.Paragraph) != 0;
        }
        #endregion

        #region Reset
        /// <summary>
        ///     Resets the enumerator to the first <see cref="Paragraph"/> in the <see cref="Block"/>
        /// </summary>
        public void Reset()
        {
            TessApi.Native.PageIteratorBegin(_iteratorHandle);
        }
        #endregion

        public void Dispose()
        {
            //TessApi.Native.PageIteratorDelete(_iteratorHandle);
        }
    }
}
