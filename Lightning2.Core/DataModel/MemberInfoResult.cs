using NuCore.Utilities; 
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lightning.Core.API
{
    /// <summary>
    /// MemberInfoResult
    /// 
    /// May 25, 2021
    /// 
    /// Result class for MemberInfo.
    /// </summary>
    public class MemberInfoResult : Result
    {
        public List<MemberInfo> MemberInfo { get; set; }
    }
}
