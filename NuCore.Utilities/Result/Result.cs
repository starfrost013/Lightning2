using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuCore.Utilities
{
    /// <summary>
    /// Result
    /// 
    /// Defines abstract base class for Lightning2.Core result classes. 
    /// Written as interface                                2021/03/05
    /// Changed to abstract class for design consistency    2022/02/07
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// Determines if the operation was successful.
        /// </summary>
        public virtual bool Successful { get; set; }

        /// <summary>
        /// If <see cref="Successful"/> is false, 
        /// </summary>
        public virtual string FailureReason { get; set; }
    }
}
