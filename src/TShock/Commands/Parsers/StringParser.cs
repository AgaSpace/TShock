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
using System.Text;
using TShock.Properties;

namespace TShock.Commands.Parsers {
    // It'd be nice to return ReadOnlySpan<char>, but because of escape characters, we have to return copies.
    internal sealed class StringParser : IArgumentParser<string> {
        public string Parse(ref ReadOnlySpan<char> input, ISet<string>? options = null) {
            if (options?.Contains(ParseOptions.ToEndOfInput) == true) {
                var result = input.ToString();
                input = default;
                return result;
            }

            // Begin building our string character-by-character.
            var builder = new StringBuilder();
            var end = 0;
            var isInQuotes = false;
            while (end < input.Length) {
                var c = input[end];

                // Handle quotes.
                if (c == '"') {
                    ++end;
                    if (isInQuotes) break;

                    isInQuotes = true;
                    continue;
                }

                // Handle escape characters.
                if (c == '\\') {
                    if (++end >= input.Length) {
                        throw new CommandParseException(Resources.StringParser_InvalidBackslash);
                    }

                    var nextC = input[end];
                    if (nextC == '"' || nextC == '\\' || char.IsWhiteSpace(nextC)) {
                        builder.Append(nextC);
                    } else if (nextC == 't') {
                        builder.Append('\t');
                    } else if (nextC == 'n') {
                        builder.Append('\n');
                    } else {
                        throw new CommandParseException(
                            string.Format(Resources.StringParser_UnrecognizedEscape, nextC));
                    }

                    ++end;
                    continue;
                }

                if (char.IsWhiteSpace(c)) {
                    if (!isInQuotes) break;
                }

                builder.Append(c);
                ++end;
            }

            input = input[end..];
            return builder.ToString();
        }

        public string GetDefault() => string.Empty;

        [ExcludeFromCodeCoverage]
        object IArgumentParser.Parse(ref ReadOnlySpan<char> input, ISet<string>? options) => Parse(ref input, options);

        [ExcludeFromCodeCoverage]
        object IArgumentParser.GetDefault() => GetDefault();
    }
}
