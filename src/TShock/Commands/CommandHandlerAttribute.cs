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
using JetBrains.Annotations;

namespace TShock.Commands {
    /// <summary>
    /// Specifies that a method is a command handler. This can be applied multiple times to a method to provide
    /// aliasing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse]
    public sealed class CommandHandlerAttribute : Attribute {
        /// <summary>
        /// Gets the command's name. This includes the command's namespace: e.g., "tshock:kick".
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// Gets the command's sub-names.
        /// </summary>
        public IEnumerable<string> CommandSubNames { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerAttribute"/> class with the given command name
        /// and sub-names.
        /// </summary>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandSubNames">The command sub-names.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commandName"/> or <paramref name="commandSubNames"/> are <see langword="null" />.
        /// </exception>
        public CommandHandlerAttribute(string commandName, params string[] commandSubNames) {
            CommandName = commandName ?? throw new ArgumentNullException(nameof(commandName));
            CommandSubNames = commandSubNames ?? throw new ArgumentNullException(nameof(commandSubNames));
        }
    }
}
