using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AuthorizationLibrary
{
    public class LcrudRequirement : IAuthorizationRequirement
    {
        private string[] _set = new string[] { "L", "C", "R", "U", "D", };

        //public static LcrudRequirement List => new LcrudRequirement("L");
        //public static LcrudRequirement Create => new LcrudRequirement("C");
        //public static LcrudRequirement Read => new LcrudRequirement("R");
        //public static LcrudRequirement Update => new LcrudRequirement("U");
        //public static LcrudRequirement Delete => new LcrudRequirement("D");

        public static LcrudRequirement Create(
            bool list, bool create = false, bool read = false, bool update = false, bool delete = false)
        {
            int i = 0;
            var array = new string[5];
            if (list) array[i++] = "L";
            if (create) array[i++] = "C";
            if (read) array[i++] = "R";
            if (update) array[i++] = "U";
            if (delete) array[i++] = "D";

            if (i == 0) throw new ArgumentException("One or more operations must be specified");

            var mem = array.AsMemory(0, i);
            return new LcrudRequirement(mem);
        }

        private LcrudRequirement(Memory<string> operations)
        {
            this.Names = operations;
        }

        public ReadOnlyMemory<string> Names { get; private set; }

        /// <summary>
        /// Does the operation match?
        /// </summary>
        public bool Match(string operation)
        {
            if (Names.Length == 0 || string.IsNullOrEmpty(operation)) return false;

            if (Names.Span.IndexOf(operation) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Does any of the operations match with this one?
        /// </summary>
        public bool MatchAny(string[] operations)
        {
            foreach(var operation in operations)
            {
                if (Match(operation)) return true;
            }

            return false;
        }

        /// <summary>
        /// The input string represents one or more
        /// of the possible LCRUD values to match any
        /// </summary>
        public bool SplitMatchAny(string multipleOperations)
        {
            var strings = multipleOperations
                .Select(s => s.ToString())
                .ToArray();

            return MatchAny(strings);
        }

        /// <summary>
        /// Does all of the operations match with this one?
        /// </summary>
        public bool MatchAll(string[] operations)
        {
            foreach (var operation in operations)
            {
                if (!Match(operation)) return false;
            }

            return true;
        }

        /// <summary>
        /// The input string represents one or more
        /// of the possible LCRUD values to match all
        /// </summary>
        public bool SplitMatchAll(string multipleOperations)
        {
            var strings = multipleOperations
                .Select(s => s.ToString())
                .ToArray();

            return MatchAll(strings);
        }


    }
}
