// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;

namespace dotnetCampus.UITest.WPFTestHelper.FaultInjection.Conditions
{
    [Serializable()]
    internal sealed class NeverTrigger : ICondition
    {
        public bool Trigger(IRuntimeContext context)
        {
            return false;
        }
    }
}
