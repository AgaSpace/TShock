﻿// Copyright (c) 2019 Pryaxis & TShock Contributors
// 
// This file is part of TShock.
// 
// TShock is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// TShock is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with TShock.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TShock.Commands.Parsers {
    /// <summary>
    /// Provides parsing support.
    /// </summary>
    public interface IArgumentParser {
        /// <summary>
        /// Parses <paramref name="input"/> and returns a corresponding object. <paramref name="input"/> will be
        /// consumed as necessary.
        /// </summary>
        /// <param name="input">The input. This is guaranteed to start with a non-whitespace character.</param>
        /// <param name="options">The parse options.</param>
        /// <returns>A corresponding object.</returns>
        /// <exception cref="CommandParseException">The input could not be parsed properly.</exception>
        object? Parse(ref ReadOnlySpan<char> input, ISet<string>? options = null);
    }

    /// <summary>
    /// Provides type-safe parsing support.
    /// </summary>
    /// <typeparam name="TParse">The parse type.</typeparam>
    public interface IArgumentParser<out TParse> : IArgumentParser {
        /// <summary>
        /// Parses <paramref name="input"/> and returns a corresponding <typeparamref name="TParse"/> instance.
        /// <paramref name="input"/> will be consumed as necessary.
        /// </summary>
        /// <param name="input">The input. This is guaranteed to start with a non-whitespace character.</param>
        /// <param name="options">The parse options.</param>
        /// <returns>A corresponding <typeparamref name="TParse"/> instance.</returns>
        /// <exception cref="CommandParseException">The input could not be parsed properly.</exception>
        new TParse Parse(ref ReadOnlySpan<char> input, ISet<string>? options = null);

        [ExcludeFromCodeCoverage]
        object? IArgumentParser.Parse(ref ReadOnlySpan<char> input, ISet<string>? options) => Parse(ref input, options);
    }
}
